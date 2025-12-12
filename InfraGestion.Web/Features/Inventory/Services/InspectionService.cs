using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Core.Constants;

namespace InfraGestion.Web.Features.Inventory.Services;

/// <summary>
/// Service for inspection-related operations
/// New in v2.1 - Extracted from InventoryController to InspectionController
/// Base route: /api/inspections
/// </summary>
public class InspectionService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public InspectionService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    // ==========================================
    // TECHNICIAN ENDPOINTS
    // ==========================================

    /// <summary>
    /// Gets inspection requests for a technician
    /// GET /api/inspections/technician/{technicianId}/requests
    /// </summary>
    public async Task<List<ReceivingInspectionRequestDto>> GetTechnicianInspectionRequestsAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetTechnicianRequests(technicianId);
            Console.WriteLine($"[DEBUG] GetTechnicianInspectionRequestsAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianInspectionRequestsAsync status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get technician inspection requests: {content}");
                return new List<ReceivingInspectionRequestDto>();
            }

            // Parse API response
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(), JsonOptions);
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetTechnicianInspectionRequestsAsync: {ex.Message}");
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    /// <summary>
    /// Gets pending inspections for a technician
    /// GET /api/inspections/technician/{technicianId}/pending
    /// </summary>
    public async Task<List<ReceivingInspectionRequestDto>> GetPendingInspectionsAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetPendingInspections(technicianId);
            Console.WriteLine($"[DEBUG] GetPendingInspectionsAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetPendingInspectionsAsync status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get pending inspections: {content}");
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(), JsonOptions);
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetPendingInspectionsAsync: {ex.Message}");
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    // ==========================================
    // ADMIN ENDPOINTS
    // ==========================================

    /// <summary>
    /// Gets revised devices for an administrator
    /// GET /api/inspections/admin/{adminId}/revised-devices
    /// </summary>
    public async Task<List<ReceivingInspectionRequestDto>> GetRevisedDevicesAsync(int adminId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetRevisedDevices(adminId);
            Console.WriteLine($"[DEBUG] GetRevisedDevicesAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetRevisedDevicesAsync status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get revised devices: {content}");
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(), JsonOptions);
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetRevisedDevicesAsync: {ex.Message}");
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    /// <summary>
    /// Gets inspection requests for an administrator
    /// GET /api/inspections/admin/{adminId}/requests
    /// </summary>
    public async Task<List<ReceivingInspectionRequestDto>> GetAdminInspectionRequestsAsync(int adminId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetAdminRequests(adminId);
            Console.WriteLine($"[DEBUG] GetAdminInspectionRequestsAsync URL: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetAdminInspectionRequestsAsync status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get admin inspection requests: {content}");
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(), JsonOptions);
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetAdminInspectionRequestsAsync: {ex.Message}");
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    // ==========================================
    // POST ENDPOINTS
    // ==========================================

    /// <summary>
    /// Processes the inspection decision (approve or reject a device)
    /// POST /api/inspections/decision
    /// </summary>
    public async Task<bool> ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.ProcessDecision;
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
    /// Assigns a device for inspection
    /// POST /api/inspections/assign
    /// </summary>
    public async Task<bool> AssignInspectionAsync(AssignDeviceForInspectionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.AssignInspection;
            Console.WriteLine($"[DEBUG] AssignInspectionAsync URL: {url}");

            var response = await _httpClient.PostAsJsonAsync(url, request);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] AssignInspectionAsync status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to assign inspection: {content}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AssignInspectionAsync: {ex.Message}");
            return false;
        }
    }

    // ==========================================
    // CONVENIENCE METHODS
    // ==========================================

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

    // ==========================================
    // LEGACY METHODS (Deprecated - for backward compatibility)
    // ==========================================

    /// <summary>
    /// Gets pending receiving inspection requests for a technician
    /// DEPRECATED: Use GetPendingInspectionsAsync instead
    /// </summary>
    [Obsolete("Use GetPendingInspectionsAsync instead")]
    public async Task<List<ReceivingInspectionRequestDto>> GetPendingReceivingInspectionRequestsAsync(int technicianId)
    {
        return await GetPendingInspectionsAsync(technicianId);
    }
}
