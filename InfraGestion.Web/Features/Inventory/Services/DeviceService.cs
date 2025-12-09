using System.Net.Http.Json;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Organization.Services;

namespace InfraGestion.Web.Features.Inventory.Services;

public class DeviceService
{
    private readonly HttpClient _httpClient;
    private readonly OrganizationService _organizationService;
    private readonly AuthService _authService;

    public DeviceService(HttpClient httpClient, OrganizationService organizationService, AuthService authService)
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
    /// GET /inventory?userID={userID}&filter.xxx=yyy
    /// </summary>
    public async Task<List<Device>> GetAllDevicesAsync(int userId = 0, DeviceFilterDto? filter = null)
    {
        try
        {
            // Asegurar autenticación
            await EnsureAuthenticatedAsync();

            // Obtener el ID del usuario autenticado si no se proporciona
            if (userId == 0)
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                userId = currentUser?.Id ?? 0;
                Console.WriteLine($"[DEBUG] GetAllDevicesAsync - currentUser: {currentUser?.Username ?? "NULL"}, Id: {currentUser?.Id ?? 0}");
            }
            
            // ⚠️ DEBUG TEMPORAL: Si el userId es 0, usar -5 (rlopez) para pruebas
            if (userId == 0)
            {
                Console.WriteLine($"[DEBUG] GetAllDevicesAsync - userId was 0, using -5 (rlopez) for testing");
                userId = -5;
            }
            
            Console.WriteLine($"[DEBUG] GetAllDevicesAsync - Final userId to send: {userId}");

            // ⚠️ El backend REQUIERE el parámetro userID
            var queryParams = new List<string> { $"userID={userId}" };

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                    queryParams.Add($"filter.SearchTerm={Uri.EscapeDataString(filter.SearchTerm)}");
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
            Console.WriteLine($"[DEBUG] GetAllDevicesAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetAllDevicesAsync status: {response.StatusCode}");
            Console.WriteLine($"[DEBUG] GetAllDevicesAsync body length: {content?.Length ?? 0}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[DEBUG] GetAllDevicesAsync failed with status: {response.StatusCode}");
                return new List<Device>();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine($"[DEBUG] GetAllDevicesAsync - Empty response");
                return new List<Device>();
            }

            // Manejo flexible: array directo o ApiResponse { data: [] }
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                var root = doc.RootElement;

                System.Text.Json.JsonElement dataElement;

                if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    dataElement = root;
                }
                else if (root.ValueKind == System.Text.Json.JsonValueKind.Object && root.TryGetProperty("data", out var dataProp))
                {
                    dataElement = dataProp;
                }
                else
                {
                    Console.WriteLine("[DEBUG] Unexpected JSON shape for devices");
                    return new List<Device>();
                }

                if (dataElement.ValueKind != System.Text.Json.JsonValueKind.Array || !dataElement.EnumerateArray().Any())
                {
                    Console.WriteLine("[DEBUG] Devices array empty or invalid");
                    return new List<Device>();
                }

                var devices = new List<Device>();

                foreach (var item in dataElement.EnumerateArray())
                {
                    var id = item.TryGetProperty("deviceId", out var idProp) ? idProp.GetInt32() : 0;
                    var name = item.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? string.Empty : string.Empty;
                    var typeVal = item.TryGetProperty("deviceType", out var typeProp) ? typeProp.GetInt32() : 0;
                    var stateVal = item.TryGetProperty("operationalState", out var stateProp) ? stateProp.GetInt32() : 0;
                    var dept = item.TryGetProperty("departmentName", out var deptProp) ? deptProp.GetString() ?? string.Empty : string.Empty;

                    devices.Add(new Device
                    {
                        Id = id,
                        Name = name,
                        Type = (DeviceType)typeVal,
                        State = (OperationalState)stateVal,
                        Location = string.IsNullOrWhiteSpace(dept) ? "Almacen General" : dept
                    });
                }

                Console.WriteLine($"[DEBUG] Parsed {devices.Count} devices");
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetDeviceDetailsAsync status: {response.StatusCode}, body: {content}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // Backend devuelve DeviceDetailDto directamente (sin ApiResponse wrapper)
            var dto = System.Text.Json.JsonSerializer.Deserialize<DeviceDetailDto>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dto != null)
            {
                var deviceDetails = MapDetailDtoToDeviceDetails(dto);
                
                // Resolve location info from Organization service
                await ResolveLocationInfoAsync(deviceDetails, dto.DepartmentId);
                
                return deviceDetails;
            }

            return null;
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
                    deviceDetails.SectionManager = section.SectionManager;
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
            // Get current user (admin) who creates the request
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return null;
            }

            var dto = new InsertDeviceRequestDto
            {
                Name = request.Name,
                DeviceType = request.Type,
                AcquisitionDate = request.PurchaseDate,
                TechnicianId = request.TechnicianId,
                UserId = currentUser.Id
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
            DecommissioningInfo = MapDecommissioningInfo(dto.DecommissioningInfo)
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
            Description = dto.Description
        }).ToList();
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
            Status = (TransferStatus)dto.Status
        }).ToList();
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
            FinalDestination = dto.FinalDestination
        };
    }

    // ==========================================
    // RECEIVING INSPECTION (Initial Defect) ENDPOINTS
    // ==========================================

    /// <summary>
    /// Gets pending receiving inspection requests for a technician
    /// GET /inventory/pendingFirstInspection/{technicianId}
    /// Note: Backend uses literal 'technicianId' in route, parameter passed as query string
    /// </summary>
    public async Task<List<ReceivingInspectionRequestDto>> GetPendingReceivingInspectionRequestsAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            // Backend route: [HttpGet("pendingFirstInspection/technicianId")]
            // This means the literal string "technicianId" is in the route, parameter comes via query
            var url = $"inventory/pendingFirstInspection/technicianId?technicianId={technicianId}";
            Console.WriteLine($"[DEBUG] GetPendingReceivingInspectionRequestsAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetPendingReceivingInspectionRequestsAsync status: {response.StatusCode}");
            Console.WriteLine($"[DEBUG] GetPendingReceivingInspectionRequestsAsync content: {content}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get pending inspection requests: {content}");
                return new List<ReceivingInspectionRequestDto>();
            }

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Backend returns ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>
            using var document = System.Text.Json.JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = System.Text.Json.JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(), options);
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetPendingReceivingInspectionRequestsAsync: {ex.Message}");
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    /// <summary>
    /// Processes the inspection decision (approve or reject a device)
    /// POST /inventory/inspection-decision
    /// </summary>
    public async Task<bool> ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = "inventory/inspection-decision";
            Console.WriteLine($"[DEBUG] ProcessInspectionDecisionAsync URL: {url}");
            Console.WriteLine($"[DEBUG] ProcessInspectionDecisionAsync request: DeviceId={request.DeviceId}, TechnicianId={request.TechnicianId}, IsApproved={request.IsApproved}, Reason={request.Reason}");

            var response = await _httpClient.PostAsJsonAsync(url, request);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] ProcessInspectionDecisionAsync status: {response.StatusCode}");
            Console.WriteLine($"[DEBUG] ProcessInspectionDecisionAsync content: {content}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to process inspection decision: {content}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] ProcessInspectionDecisionAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Approves a device inspection
    /// </summary>
    public async Task<bool> ApproveDeviceInspectionAsync(int deviceId, int technicianId)
    {
        var request = new InspectionDecisionRequestDto
        {
            DeviceId = deviceId,
            TechnicianId = technicianId,
            IsApproved = true,
            Reason = DecommissioningReason.IrreparableTechnicalFailure // Not used for approval
        };

        return await ProcessInspectionDecisionAsync(request);
    }

    /// <summary>
    /// Rejects a device inspection with a reason
    /// </summary>
    public async Task<bool> RejectDeviceInspectionAsync(int deviceId, int technicianId, DecommissioningReason reason)
    {
        var request = new InspectionDecisionRequestDto
        {
            DeviceId = deviceId,
            TechnicianId = technicianId,
            IsApproved = false,
            Reason = reason
        };

        return await ProcessInspectionDecisionAsync(request);
    }
}