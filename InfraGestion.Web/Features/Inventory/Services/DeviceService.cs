using System.Net.Http.Json;
using InfraGestion.Web.Core.Constants;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Organization.Services;

namespace InfraGestion.Web.Features.Inventory.Services;

public class DeviceService
{
    private readonly HttpClient _httpClient;
    private readonly OrganizationService _organizationService;
    private readonly AuthService _authService;

    public DeviceService(
        HttpClient httpClient,
        OrganizationService organizationService,
        AuthService authService
    )
    {
        _httpClient = httpClient;
        _organizationService = organizationService;
        _authService = authService;
    }

    public async Task<List<Device>> GetAllDevicesAsync(DeviceFilterDto? filter = null)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new List<Device>();
            }

            var url = ApiRoutes.Devices.GetAllDevices;

            if (filter != null)
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                    queryParams.Add($"filter.SearchTerm={Uri.EscapeDataString(filter.SearchTerm)}");
                if (filter.DeviceType.HasValue)
                    queryParams.Add($"filter.DeviceType={filter.DeviceType.Value}");
                if (filter.OperationalState.HasValue)
                    queryParams.Add($"filter.OperationalState={filter.OperationalState.Value}");
                if (!string.IsNullOrEmpty(filter.DepartmentName))
                    queryParams.Add(
                        $"filter.DepartmentName={Uri.EscapeDataString(filter.DepartmentName)}"
                    );
                if (filter.DepartmentId.HasValue)
                    queryParams.Add($"filter.DepartmentId={filter.DepartmentId.Value}");

                if (queryParams.Any())
                    url += $"?{string.Join("&", queryParams)}";
            }

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<Device>();
            }

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                var root = doc.RootElement;

                System.Text.Json.JsonElement dataElement;

                if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    dataElement = root;
                }
                else if (
                    root.ValueKind == System.Text.Json.JsonValueKind.Object
                    && root.TryGetProperty("data", out var dataProp)
                )
                {
                    dataElement = dataProp;
                }
                else
                {
                    return new List<Device>();
                }

                if (
                    dataElement.ValueKind != System.Text.Json.JsonValueKind.Array
                    || !dataElement.EnumerateArray().Any()
                )
                {
                    return new List<Device>();
                }

                var devices = new List<Device>();

                foreach (var item in dataElement.EnumerateArray())
                {
                    var id = item.TryGetProperty("deviceId", out var idProp)
                        ? idProp.GetInt32()
                        : 0;
                    var name = item.TryGetProperty("name", out var nameProp)
                        ? nameProp.GetString() ?? string.Empty
                        : string.Empty;
                    var typeVal = item.TryGetProperty("deviceType", out var typeProp)
                        ? typeProp.GetInt32()
                        : 0;
                    var stateVal = item.TryGetProperty("operationalState", out var stateProp)
                        ? stateProp.GetInt32()
                        : 0;
                    var dept = item.TryGetProperty("departmentName", out var deptProp)
                        ? deptProp.GetString() ?? string.Empty
                        : string.Empty;

                    devices.Add(
                        new Device
                        {
                            Id = id,
                            Name = name,
                            Type = (DeviceType)typeVal,
                            State = (OperationalState)stateVal,
                            Location = string.IsNullOrWhiteSpace(dept) ? "Almacén General" : dept,
                        }
                    );
                }

                return devices;
            }
            catch (Exception)
            {
                return new List<Device>();
            }
        }
        catch (Exception)
        {
            return new List<Device>();
        }
    }

    public async Task<Device?> GetDeviceByIdAsync(int id)
    {
        try
        {
            var details = await GetDeviceDetailsAsync(id);

            if (details != null)
            {
                return new Device
                {
                    Id = details.Id,
                    Name = details.Name,
                    Type = details.Type,
                    State = details.State,
                    Location = details.Department,
                    AcquisitionDate = details.PurchaseDate,
                };
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<DeviceDetails?> GetDeviceDetailsAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Devices.GetDeviceById(id));

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[ERROR] GetDeviceDetailsAsync - Failed to fetch device {id}: {response.StatusCode}"
                );
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeviceDetailDto>>();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDetailDtoToDeviceDetails(apiResponse.Data);
            }

            Console.WriteLine($"[ERROR] GetDeviceDetailsAsync - Invalid response for device {id}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(
                $"[ERROR] GetDeviceDetailsAsync - Network error for device {id}: {ex.Message}"
            );
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[ERROR] GetDeviceDetailsAsync - Unexpected error for device {id}: {ex.Message}"
            );
            return null;
        }
    }

    public async Task<List<Device>> GetSectionDevicesAsync(int sectionId)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new List<Device>();
            }

            var response = await _httpClient.GetAsync(
                ApiRoutes.Devices.GetDevicesBySection(sectionId)
            );

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<
                ApiResponse<IEnumerable<DeviceDto>>
            >();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return apiResponse.Data.Select(MapDtoToDevice).ToList();
            }

            return new List<Device>();
        }
        catch (Exception)
        {
            return new List<Device>();
        }
    }

    public async Task<List<Device>> GetOwnSectionDevicesAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new List<Device>();
            }

            var response = await _httpClient.GetAsync(ApiRoutes.Devices.GetMySectionDevices);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<
                ApiResponse<IEnumerable<DeviceDto>>
            >();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return apiResponse.Data.Select(MapDtoToDevice).ToList();
            }

            return new List<Device>();
        }
        catch (Exception)
        {
            return new List<Device>();
        }
    }

    public async Task<List<Device>> SearchDevicesAsync(
        string searchTerm = "",
        DeviceType? type = null,
        OperationalState? state = null,
        string location = ""
    )
    {
        var filter = new DeviceFilterDto
        {
            SearchTerm = searchTerm,
            DeviceType = type,
            OperationalState = state,
            DepartmentName = location,
        };

        return await GetAllDevicesAsync(filter);
    }

    public async Task<Device?> CreateDeviceAsync(CreateDeviceRequest request)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return null;
            }

            var dto = new RegisterDeviceDto
            {
                Name = request.Name,
                DeviceType = request.Type,
                AcquisitionDate = request.PurchaseDate,
                TechnicianId = request.TechnicianId,
            };

            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Devices.CreateDevice, dto);

            if (
                !response.IsSuccessStatusCode
                && response.StatusCode != System.Net.HttpStatusCode.Created
            )
            {
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeviceDto>>();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDtoToDevice(apiResponse.Data);
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<Device?> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        try
        {
            var dto = new UpdateDeviceDto
            {
                DeviceId = request.Id,
                Name = request.Name,
                DeviceType = request.Type,
                OperationalState = request.State,
                DepartmentName = request.Location,
                Date = request.PurchaseDate,
            };

            var response = await _httpClient.PutAsJsonAsync(
                ApiRoutes.Devices.UpdateDevice(request.Id),
                dto
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string?>>();

            if (apiResponse?.Success == true)
            {
                return new Device
                {
                    Id = request.Id,
                    Name = request.Name,
                    Type = request.Type,
                    State = request.State,
                    Location = request.Location,
                };
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(ApiRoutes.Devices.DeleteDevice(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            try
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string?>>();

                if (apiResponse?.Success == true)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync()
    {
        var devices = await GetAllDevicesAsync();

        return new Dictionary<string, int>
        {
            ["Total"] = devices.Count,
            ["UnderRevision"] = devices.Count(d => d.State == OperationalState.UnderRevision),
            ["Revised"] = devices.Count(d => d.State == OperationalState.Revised),
            ["Operational"] = devices.Count(d => d.State == OperationalState.Operational),
            ["UnderMaintenance"] = devices.Count(d => d.State == OperationalState.UnderMaintenance),
            ["Decommissioned"] = devices.Count(d => d.State == OperationalState.Decommissioned),
            ["BeingTransferred"] = devices.Count(d => d.State == OperationalState.BeingTransferred),
        };
    }

    private Device MapDtoToDevice(DeviceDto dto)
    {
        return new Device
        {
            Id = dto.DeviceId,
            Name = dto.Name,
            Type = dto.DeviceType,
            State = dto.OperationalState,
            Location = NormalizeDepartmentName(dto.DepartmentName),
        };
    }

    private DeviceDetails MapDetailDtoToDeviceDetails(DeviceDetailDto dto)
    {
        var maintenanceHistory = MapMaintenanceHistory(dto.MaintenanceHistory.ToList());

        var maintenanceCount = maintenanceHistory.Count;
        var totalMaintenanceCost = maintenanceHistory.Sum(m => m.Cost);
        var lastMaintenanceDate = maintenanceHistory.Any()
            ? maintenanceHistory.Max(m => m.Date)
            : (DateTime?)null;

        return new DeviceDetails
        {
            Id = dto.DeviceId,
            Name = dto.Name,
            IdentificationNumber = $"DEV{dto.DeviceId:D3}",
            Type = (DeviceType)dto.DeviceType,
            State = (OperationalState)dto.OperationalState,
            PurchaseDate = dto.AcquisitionDate,
            DepartmentId = 0,
            Department = NormalizeDepartmentName(dto.DepartmentName),
            SectionId = 0,
            Section = dto.SectionName,
            SectionManager = dto.SectionManagerName ?? "N/A",
            MaintenanceCount = maintenanceCount,
            TotalMaintenanceCost = totalMaintenanceCost,
            LastMaintenanceDate = lastMaintenanceDate,
            MaintenanceHistory = maintenanceHistory,
            TransferHistory = MapTransferHistory(dto.TransferHistory.ToList()),
            DecommissioningInfo = MapDecommissioningInfo(dto.DecommissioningRequestInfo),
        };
    }

    private string NormalizeDepartmentName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Almacen General";
        }

        return name.Equals("Mocking Department", StringComparison.OrdinalIgnoreCase)
            ? "Almacen General"
            : name;
    }

    private List<MaintenanceRecord> MapMaintenanceHistory(List<MaintenanceRecordDto>? dtos)
    {
        if (dtos == null || !dtos.Any())
            return new List<MaintenanceRecord>();

        return dtos.Select(dto => new MaintenanceRecord
            {
                Id = dto.MaintenanceRecordId,
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName,
                TechnicianId = dto.TechnicianId,
                TechnicianName = dto.TechnicianName,
                Date = dto.MaintenanceDate,
                Type = (MaintenanceType)dto.MaintenanceType,
                Cost = (decimal)dto.Cost,
                Description = dto.Description,
            })
            .ToList();
    }

    private List<TransferRecord> MapTransferHistory(List<TransferDto>? dtos)
    {
        if (dtos == null || !dtos.Any())
            return new List<TransferRecord>();

        return dtos.Select(dto => new TransferRecord
            {
                Id = dto.TransferId,
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName,
                Date = dto.TransferDate,
                SourceSectionId = dto.SourceSectionId,
                SourceSectionName = dto.SourceSectionName,
                DestinationSectionId = dto.DestinationSectionId,
                DestinationSectionName = dto.DestinationSectionName,
                ReceiverId = dto.DeviceReceiverId,
                ReceiverName = dto.DeviceReceiverName,
                Status = (TransferStatus)dto.Status,
            })
            .ToList();
    }

    private DecommissioningRequest? MapDecommissioningInfo(IEnumerable<DecommissioningRequestDto>? dtos)
    {
        if (dtos == null || !dtos.Any())
            return null;

        // Take the most recent decommissioning request
        var dto = dtos.OrderByDescending(d => d.RequestDate).FirstOrDefault();
        
        if (dto == null)
            return null;

        return new DecommissioningRequest
        {
            Id = dto.DecommissioningRequestId,
            DeviceId = dto.DeviceId,
            DeviceName = dto.DeviceName,
            ReceiverName = dto.ReceiverUserName ?? "N/A",
            DecommissioningDate = dto.RequestDate,
            Reason = (DecommissioningReason)dto.Reason,
            ReasonDescription = dto.ReasonDescription,
            FinalDestination = dto.FinalDestinationName,
            ReviewedDate = dto.ReviewedDate,
            ReviewedByUserId = dto.ReviewedByUserId,
            ReviewedByUserName = dto.ReviewedByUserName,
        };
    }
}
