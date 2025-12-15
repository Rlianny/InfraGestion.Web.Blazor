using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Core.Constants;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Inventory.DTOs;

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

    public async Task<List<InspectionRequestDto>> GetTechnicianInspectionRequestsAsync(
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
                return new List<InspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<InspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<InspectionRequestDto>();
            }

            return new List<InspectionRequestDto>();
        }
        catch (Exception)
        {
            return new List<InspectionRequestDto>();
        }
    }

    public async Task<List<InspectionRequestDto>> GetPendingInspectionsAsync(
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
                Console.WriteLine($"[ERROR] GetPendingInspectionsAsync failed: {response.StatusCode} - {content}");
                return new List<InspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<InspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<InspectionRequestDto>();
            }

            return new List<InspectionRequestDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetPendingInspectionsAsync exception: {ex.Message}");
            return new List<InspectionRequestDto>();
        }
    }

    
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

    public async Task<List<InspectionRequestDto>> GetAdminInspectionRequestsAsync(
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
                return new List<InspectionRequestDto>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var requests = JsonSerializer.Deserialize<List<InspectionRequestDto>>(
                    dataElement.GetRawText(),
                    JsonOptions
                );
                return requests ?? new List<InspectionRequestDto>();
            }

            return new List<InspectionRequestDto>();
        }
        catch (Exception)
        {
            return new List<InspectionRequestDto>();
        }
    }

    public async Task<bool> ProcessInspectionDecisionAsync(InspectionDecisionDto request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Inspections.ProcessDecision;
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] ProcessInspectionDecisionAsync failed: {response.StatusCode} - {content}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] ProcessInspectionDecisionAsync exception: {ex.Message}");
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
}
