using System.Net.Http.Json;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Auth.DTOs;

namespace InfraGestion.Web.Features.Inventory.Services;

public class DeviceService
{
    private readonly HttpClient _httpClient;

    public DeviceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET ENDPOINTS

    /// <summary>
    /// Gets all devices with optional filters
    /// GET /inventory?userId={userId}&filter.xxx=yyy
    /// </summary>
    public async Task<List<Device>> GetAllDevicesAsync(int userId = 1, DeviceFilterDto? filter = null)
    {
        try
        {

            var queryParams = new List<string> { $"userId={userId}" };

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                    queryParams.Add($"filter.SearchTerm={Uri.EscapeDataString(filter.SearchTerm)}");
                //Console.WriteLine("🔵 Fetching devices from API...");
                if (filter.DeviceType.HasValue)
                    queryParams.Add($"filter.DeviceType={filter.DeviceType.Value}");

                if (filter.OperationalState.HasValue)
                    queryParams.Add($"filter.OperationalState={filter.OperationalState.Value}");

                if (!string.IsNullOrEmpty(filter.DepartmentName))
                    queryParams.Add($"filter.DepartmentName={Uri.EscapeDataString(filter.DepartmentName)}");

                if (filter.DepartmentId.HasValue)
                    queryParams.Add($"filter.DepartmentId={filter.DepartmentId.Value}");
            }

            var url = $"inventory?{string.Join("&", queryParams)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<DeviceDto>>>();

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
                    AcquisitionDate = details.PurchaseDate
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
    /// Gets detailed device information
    /// GET /inventory/{id}
    /// </summary>
    public async Task<DeviceDetails?> GetDeviceDetailsAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"inventory/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeviceDetailDto>>();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDetailDtoToDeviceDetails(apiResponse.Data);
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets all company devices
    /// GET /inventory/company/devices
    /// </summary>
    public async Task<List<Device>> GetCompanyDevicesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("inventory/company/devices");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<DeviceDto>>>();

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
    /// GET /inventory/sections/{sectionId}
    /// </summary>
    public async Task<List<Device>> GetSectionDevicesAsync(int sectionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"inventory/sections/{sectionId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<DeviceDto>>>();

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
    /// GET /inventory/ownedSection
    /// Requires JWT token
    /// </summary>
    public async Task<List<Device>> GetOwnSectionDevicesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("inventory/ownedSection");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Device>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<DeviceDto>>>();

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
        string location = "")
    {
        var filter = new DeviceFilterDto
        {
            SearchTerm = searchTerm,
            DeviceType = type,
            OperationalState = state,
            DepartmentName = location
        };

        return await GetAllDevicesAsync(1, filter);
    }

    // POST ENDPOINTS

    /// <summary>
    /// Creates a new device
    /// POST /inventory
    /// </summary>
    public async Task<Device?> CreateDeviceAsync(CreateDeviceRequest request)
    {
        try
        {
            var dto = new InsertDeviceRequestDto
            {
                Name = request.Name,
                DeviceType = request.Type,
                AcquisitionDate = request.PurchaseDate
            };

            var response = await _httpClient.PostAsJsonAsync("inventory", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string?>>();

            if (apiResponse?.Success == true)
            {
                // Return local representation (API doesn't return created device)
                return new Device
                {
                    Id = 0,
                    Name = request.Name,
                    Type = request.Type,
                    State = OperationalState.UnderRevision,
                    Location = "Pendiente de revision"
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
                Reason = reason
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
                TechnicianId = technicianId
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

    /// <summary>
    /// Assigns device for inspection
    /// POST /inventory/reviews
    /// </summary>
    public async Task<bool> AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("inventory/reviews", request);

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
    /// PUT /inventory
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
                Date = request.PurchaseDate
            };

            var response = await _httpClient.PutAsJsonAsync("inventory", dto);

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
                    Location = request.Location
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    // DELETE - NOT AVAILABLE

    /// <summary>
    /// Deletes a device
    /// ⚠️ WARNING: DELETE endpoint does NOT exist in API
    /// Use RejectDeviceAsync instead
    /// </summary>
    public async Task<bool> DeleteDeviceAsync(int id)
    {
        try
        {
            Console.WriteLine($"🔵 Deleting device {id}...");

            var response = await _httpClient.DeleteAsync($"inventory/{id}");

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
            ["BeingTransferred"] = devices.Count(d => d.State == OperationalState.BeingTransferred)
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
            Location = NormalizeDepartmentName(dto.DepartmentName)
        };
    }

    private DeviceDetails MapDetailDtoToDeviceDetails(DeviceDetailDto dto)
    {
        return new DeviceDetails
        {
            Id = dto.DeviceId,
            Name = dto.Name,
            IdentificationNumber = $"DEV{dto.DeviceId:D3}",
            Type = dto.DeviceType,
            State = dto.OperationalState,
            PurchaseDate = dto.AcquisitionDate,
            Department = NormalizeDepartmentName(dto.DepartmentName),
            Section = dto.SectionName ?? "N/A",
            SectionManager = dto.SectionManager ?? "N/A",
            MaintenanceCount = dto.MaintenanceCount ?? 0,
            TotalMaintenanceCost = dto.TotalMaintenanceCost ?? 0m,
            LastMaintenanceDate = dto.LastMaintenanceDate,
            MaintenanceHistory = MapMaintenanceHistory(dto.MaintenanceHistory),
            TransferHistory = MapTransferHistory(dto.TransferHistory),
            InitialDefect = MapInitialDefect(dto.InitialDefect)
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
            Date = dto.Date,
            Type = dto.Type,
            Technician = dto.Technician,
            Notes = dto.Notes,
            Cost = dto.Cost
        }).ToList();
    }

    private List<TransferRecord> MapTransferHistory(List<TransferRecordDto>? dtos)
    {
        if (dtos == null || !dtos.Any())
            return new List<TransferRecord>();

        return dtos.Select(dto => new TransferRecord
        {
            Date = dto.Date,
            Origin = dto.Origin,
            Destination = dto.Destination,
            Manager = dto.ResponsiblePerson,
            Receiver = dto.Receiver
        }).ToList();
    }

    private InitialDefect? MapInitialDefect(InitialDefectDto? dto)
    {
        if (dto == null)
            return null;

        return new InitialDefect
        {
            SubmissionDate = dto.IssueDate,
            Requester = dto.Requester,
            Technician = dto.Technician,
            Status = dto.Status,
            ResponseDate = dto.ResponseDate
        };
    }
}