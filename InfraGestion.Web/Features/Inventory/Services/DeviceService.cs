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

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    // GET ENDPOINTS

    /// <summary>
    /// Gets all devices with optional filters
    /// GET /api/devices
    /// </summary>
    public async Task<List<Device>> GetAllDevicesAsync(DeviceFilterDto? filter = null)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Console.WriteLine("[ERROR] GetAllDevicesAsync - No authenticated user found");
                return new List<Device>();
            }

            Console.WriteLine(
                $"[DEBUG] GetAllDevicesAsync - Authenticated user: {currentUser.Username} (ID: {currentUser.Id})"
            );
            Console.WriteLine(
                $"[DEBUG] GetAllDevicesAsync - Auth header present: {_httpClient.DefaultRequestHeaders.Authorization != null}"
            );

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

            Console.WriteLine($"[DEBUG] GetAllDevicesAsync URL: {url}");

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
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Failed to parse devices JSON: {ex.Message}");
                return new List<Device>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetAllDevicesAsync: {ex.Message}");
            return new List<Device>();
        }
    }

    /// <summary>
    /// Gets device by ID (for edit modal)
    /// Uses GetDeviceDetailsAsync internally
    /// </summary>
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

    /// <summary>
    /// Gets detailed device information
    /// GET /api/devices/{id}
    /// </summary>
    public async Task<DeviceDetails?> GetDeviceDetailsAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Devices.GetDeviceById(id));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // Try to handle API responses that may be wrapped in a { success/data/... } envelope
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                var root = doc.RootElement;

                System.Text.Json.JsonElement dataElement;

                if (
                    root.ValueKind == System.Text.Json.JsonValueKind.Object
                    && root.TryGetProperty("data", out var dataProp)
                    && dataProp.ValueKind == System.Text.Json.JsonValueKind.Object
                )
                {
                    dataElement = dataProp;
                }
                else if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    // Assume the root object is the DTO itself
                    dataElement = root;
                }
                else
                {
                    // Unexpected shape
                    Console.WriteLine(
                        "[WARN] GetDeviceDetailsAsync - unexpected JSON shape for device detail"
                    );
                    return null;
                }

                var dto = System.Text.Json.JsonSerializer.Deserialize<DeviceDetailDto>(
                    dataElement.GetRawText(),
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }
                );

                if (dto != null)
                {
                    var deviceDetails = MapDetailDtoToDeviceDetails(dto);

                    // Resolve location info from Organization service (departmentId may be 0 if not present)
                    await ResolveLocationInfoAsync(deviceDetails, dto.DepartmentId);
                    return deviceDetails;
                }

                return null;
            }
            catch (System.Text.Json.JsonException jex)
            {
                Console.WriteLine(
                    $"[ERROR] GetDeviceDetailsAsync - JSON parse error: {jex.Message}"
                );
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetDeviceDetailsAsync: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Resolves Section and SectionManager from DepartmentId
    /// Chain: Device → Department → Section → SectionManager
    /// </summary>
    private async Task ResolveLocationInfoAsync(DeviceDetails deviceDetails, int departmentId)
    {
        try
        {
            // Get department info (includes SectionId)
            var department = await _organizationService.GetDepartmentByIdAsync(departmentId);

            if (department != null)
            {
                deviceDetails.DepartmentId = department.Id;
                deviceDetails.Department = department.Name;
                deviceDetails.SectionId = department.SectionId;
                deviceDetails.Section = department.SectionName;

                // Get section to resolve SectionManager
                var section = await _organizationService.GetSectionByIdAsync(department.SectionId);
                if (section != null)
                {
                    deviceDetails.SectionManager = section.SectionManagerFullName;
                }
            }
        }
        catch (Exception)
        {
            // Keep default values if resolution fails
        }
    }

    /// <summary>
    /// Gets all company devices
    /// GET /api/devices
    /// </summary>
    public async Task<List<Device>> GetCompanyDevicesAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Console.WriteLine("[ERROR] GetCompanyDevicesAsync - No authenticated user found");
                return new List<Device>();
            }

            Console.WriteLine($"[DEBUG] GetCompanyDevicesAsync - User authenticated");

            var response = await _httpClient.GetAsync(ApiRoutes.Devices.GetAllDevices);

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
        catch (Exception ex)
        {
            return new List<Device>();
        }
    }

    /// <summary>
    /// Gets devices from a specific section
    /// GET /api/devices/sections/{sectionId}
    /// </summary>
    public async Task<List<Device>> GetSectionDevicesAsync(int sectionId)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Console.WriteLine("[ERROR] GetSectionDevicesAsync - No authenticated user found");
                return new List<Device>();
            }

            Console.WriteLine($"[DEBUG] GetSectionDevicesAsync - sectionId: {sectionId}");

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
        catch (Exception ex)
        {
            return new List<Device>();
        }
    }

    /// <summary>
    /// Gets devices from authenticated user's section
    /// GET /api/devices/my-section
    /// </summary>
    public async Task<List<Device>> GetOwnSectionDevicesAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Console.WriteLine(
                    "[ERROR] GetOwnSectionDevicesAsync - No authenticated user found"
                );
                return new List<Device>();
            }

            Console.WriteLine($"[DEBUG] GetOwnSectionDevicesAsync - Fetching my section devices");

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
        catch (Exception ex)
        {
            return new List<Device>();
        }
    }

    /// <summary>
    /// Searches devices with filters
    /// Uses GetAllDevicesAsync internally
    /// </summary>
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

    // POST ENDPOINTS

    /// <summary>
    /// Creates a new device
    /// POST /api/devices
    /// </summary>
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
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] CreateDeviceAsync - API error: {error}");
                return null;
            }

            // API returns ApiResponse<DeviceDto> with the created device data
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

    /// <summary>
    /// Rejects a device
    /// POST /inventory/rejections
    /// </summary>
    public async Task<bool> RejectDeviceAsync(int deviceId, int technicianId, string reason)
    {
        try
        {
            var dto = new RejectDeviceRequestDto
            {
                DeviceID = deviceId,
                TechnicianID = technicianId,
                Reason = reason,
            };

            var response = await _httpClient.PostAsJsonAsync("inventory/rejections", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Approves a device
    /// POST /inventory/approbals
    /// </summary>
    public async Task<bool> ApproveDeviceAsync(int deviceId, int technicianId)
    {
        try
        {
            var dto = new AcceptDeviceRequestDto
            {
                DeviceId = deviceId,
                TechnicianId = technicianId,
            };

            var response = await _httpClient.PostAsJsonAsync("inventory/approbals", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    // PUT ENDPOINTS

    /// <summary>
    /// Updates an existing device
    /// PUT /api/devices/{id}
    /// </summary>
    public async Task<Device?> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        try
        {
            var dto = new UpdateDeviceRequestDto
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
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Deletes a device
    /// DELETE /api/devices/{id}
    /// </summary>
    public async Task<bool> DeleteDeviceAsync(int id)
    {
        try
        {
            Console.WriteLine($"🔵 Deleting device {id}...");

            var response = await _httpClient.DeleteAsync(ApiRoutes.Devices.DeleteDevice(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return false;
            }

            // Try to parse API response
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
                // If response doesn't contain JSON (e.g., 204 No Content)
                // Consider it successful
                return true;
            }
        }
        catch (HttpRequestException ex)
        {
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    // STATISTICS (for UI cards)

    /// <summary>
    /// Gets device statistics
    /// Calculates from GetAllDevicesAsync
    /// </summary>
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

    // MAPPERS

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
        // Map maintenance history
        var maintenanceHistory = MapMaintenanceHistory(dto.MaintenanceHistory);

        // Calculate maintenance statistics from history
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
            // Location info - set defaults, will be resolved via ResolveLocationInfoAsync
            DepartmentId = dto.DepartmentId,
            Department = NormalizeDepartmentName(dto.DepartmentName),
            SectionId = 0,
            Section = "Cargando...",
            SectionManager = "Cargando...",
            // Statistics
            MaintenanceCount = maintenanceCount,
            TotalMaintenanceCost = totalMaintenanceCost,
            LastMaintenanceDate = lastMaintenanceDate,
            // Related records
            MaintenanceHistory = maintenanceHistory,
            TransferHistory = MapTransferHistory(dto.TransferHistory),
            DecommissioningInfo = MapDecommissioningInfo(dto.DecommissioningInfo),
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

    private DecommissioningRequest? MapDecommissioningInfo(DecommissioningDto? dto)
    {
        if (dto == null)
            return null;

        return new DecommissioningRequest
        {
            Id = dto.DecommissioningRequestId,
            DeviceId = dto.DeviceId,
            DeviceName = dto.DeviceName,
            ReceiverId = dto.DeviceReceiverId,
            ReceiverName = dto.DeviceReceiverName,
            ReceiverDepartmentId = dto.ReceiverDepartmentId,
            ReceiverDepartmentName = dto.ReceiverDepartmentName,
            DecommissioningDate = dto.DecommissioningDate,
            Reason = (DecommissioningReason)dto.Reason,
            FinalDestination = dto.FinalDestination,
        };
    }

    
}
