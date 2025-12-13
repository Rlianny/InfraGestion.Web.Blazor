using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Inventory.DTOs;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Core.Constants;

namespace InfraGestion.Web.Features.Inventory.Services;

/// <summary>
/// Service for decommissioning-related operations
/// Base route: /decommissioning
/// </summary>
public class DecommissioningService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public DecommissioningService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    /// <summary>
    /// Gets all decommissioning requests
    /// GET /decommissioning/requests
    /// </summary>
    public async Task<List<DecommissioningRequest>> GetAllDecommissioningRequestsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.GetAllRequests;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get decommissioning requests: {content}");
                return new List<DecommissioningRequest>();
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var dtos = JsonSerializer.Deserialize<List<DecommissioningRequestDto>>(
                    dataElement.GetRawText(), JsonOptions);

                if (dtos != null)
                {
                    return dtos.Select(MapDtoToModel).ToList();
                }
            }

            return new List<DecommissioningRequest>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetAllDecommissioningRequestsAsync: {ex.Message}");
            return new List<DecommissioningRequest>();
        }
    }

    /// <summary>
    /// Gets a decommissioning request by ID
    /// GET /decommissioning/requests/{id}
    /// </summary>
    public async Task<DecommissioningRequest?> GetDecommissioningRequestByIdAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.GetRequestById(id);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get decommissioning request: {content}");
                return null;
            }

            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement))
            {
                var dto = JsonSerializer.Deserialize<DecommissioningRequestDto>(
                    dataElement.GetRawText(), JsonOptions);

                if (dto != null)
                {
                    return MapDtoToModel(dto);
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetDecommissioningRequestByIdAsync: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Deletes a decommissioning request
    /// DELETE /decommissioning/requests/{id}
    /// </summary>
    public async Task<bool> DeleteDecommissioningRequestAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.DeleteRequest(id);
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to delete decommissioning request: {errorContent}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] DeleteDecommissioningRequestAsync: {ex.Message}");
            return false;
        }
    }

    // Mapper
    private DecommissioningRequest MapDtoToModel(DecommissioningRequestDto dto)
    {
        return new DecommissioningRequest
        {
            Id = dto.DecommissioningRequestId,
            DeviceId = dto.DeviceId,
            DeviceName = dto.DeviceName,
            TechnicianId = dto.TechnicianId,
            TechnicianName = dto.TechnicianName,
            RequestDate = dto.RequestDate,
            Status = (DecommissioningStatus)dto.Status,
            Justification = dto.Justification,
            Reason = (DecommissioningReason)dto.Reason,
            ReasonDescription = dto.ReasonDescription,
            ReviewedDate = dto.ReviewedDate,
            ReviewedByUserId = dto.ReviewedByUserId,
            ReviewedByUserName = dto.ReviewedByUserName
        };
    }
}
