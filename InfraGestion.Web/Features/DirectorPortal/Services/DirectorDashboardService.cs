using System.Net.Http.Json;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.DirectorPortal.DTOs;

namespace InfraGestion.Web.Features.DirectorPortal.Services;

/// <summary>
/// Servicio para obtener datos del Dashboard del Director desde la API
/// </summary>
public class DirectorDashboardService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public DirectorDashboardService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    /// <summary>
    /// Obtiene la información del dashboard del director desde la API
    /// </summary>
    /// <returns>DTO con toda la información del dashboard o null si hay error</returns>
    public async Task<DirectorDashboardDto?> GetDashboardInfoAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var response = await _httpClient.GetAsync("Users/director/dashboardInfo");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] DirectorDashboardService - GetDashboardInfoAsync: HTTP {response.StatusCode}");
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DirectorDashboardDto>>();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return apiResponse.Data;
            }

            Console.WriteLine($"[ERROR] DirectorDashboardService - GetDashboardInfoAsync: {apiResponse?.Message ?? "Unknown error"}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] DirectorDashboardService - GetDashboardInfoAsync: {ex.Message}");
            return null;
        }
    }
}
