using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Core.Constants;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.Services;

public class InspectionService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
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

    public async Task<List<ReceivingInspectionRequestDto>> GetTechnicianInspectionRequestsAsync(
        int technicianId
    )
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetTechnicianRequests(technicianId);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception)
        {
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    public async Task<List<ReceivingInspectionRequestDto>> GetPendingInspectionsAsync(
        int technicianId
    )
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetPendingInspections(technicianId);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception)
        {
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
    public async Task<List<RevisedDeviceDto>> GetRevisedDevicesAsync(int adminId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetRevisedDevices(adminId);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get revised devices: {content}");
                return new List<RevisedDeviceDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var devices = JsonSerializer.Deserialize<List<RevisedDeviceDto>>(
                    dataElement.GetRawText(), JsonOptions);
                return devices ?? new List<RevisedDeviceDto>();
            }

            return new List<RevisedDeviceDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetRevisedDevicesAsync: {ex.Message}");
            return new List<RevisedDeviceDto>();
        }
    }

    public async Task<List<ReceivingInspectionRequestDto>> GetAdminInspectionRequestsAsync(
        int adminId
    )
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.GetAdminRequests(adminId);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new List<ReceivingInspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<ReceivingInspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<ReceivingInspectionRequestDto>();
            }

            return new List<ReceivingInspectionRequestDto>();
        }
        catch (Exception)
        {
            return new List<ReceivingInspectionRequestDto>();
        }
    }

    public async Task<bool> ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.ProcessDecision;
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> AssignInspectionAsync(AssignDeviceForInspectionRequestDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.AssignInspection;

            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ApproveDeviceInspectionAsync(int deviceId, int technicianId)
    {
        var request = new InspectionDecisionRequestDto
        {
            DeviceId = deviceId,
            TechnicianId = technicianId,
            IsApproved = true,
            Reason = null,
        };

        return await ProcessInspectionDecisionAsync(request);
    }

    public async Task<bool> RejectDeviceInspectionAsync(
        int deviceId,
        int technicianId,
        DecommissioningReason reason
    )
    {
        var request = new InspectionDecisionRequestDto
        {
            DeviceId = deviceId,
            TechnicianId = technicianId,
            IsApproved = false,
            Reason = reason,
        };

        return await ProcessInspectionDecisionAsync(request);
    }
}
