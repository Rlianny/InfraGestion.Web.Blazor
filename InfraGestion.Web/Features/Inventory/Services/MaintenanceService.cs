using System.Net.Http.Json;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Core.Constants;

namespace InfraGestion.Web.Features.Inventory.Services;

/// <summary>
/// Service for maintenance-related operations
/// Base route: /maintenance
/// </summary>
public class MaintenanceService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public MaintenanceService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    /// <summary>
    /// Creates a new maintenance record
    /// POST /maintenance
    /// </summary>
    public async Task<bool> CreateMaintenanceRecordAsync(CreateMaintenanceRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Maintenance.Create;
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to create maintenance record: {errorContent}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CreateMaintenanceRecordAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Gets maintenance history for a specific technician
    /// GET /maintenance/technician/{technicianId}
    /// </summary>
    public async Task<List<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Maintenance.GetByTechnician(technicianId);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to get technician maintenance history: {errorContent}");
                return new List<MaintenanceRecordDto>();
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MaintenanceRecordDto>>>();
            return apiResponse?.Data ?? new List<MaintenanceRecordDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetTechnicianMaintenanceHistoryAsync: {ex.Message}");
            return new List<MaintenanceRecordDto>();
        }
    }

    private class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
