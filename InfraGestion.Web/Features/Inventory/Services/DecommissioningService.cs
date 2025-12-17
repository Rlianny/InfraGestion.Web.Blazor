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
    /// Gets all pending decommissioning requests
    /// GET /decommissioning/requests/pending
    /// </summary>
    public async Task<List<DecommissioningRequest>> GetPendingDecommissioningRequestsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.GetPendingRequests;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] Failed to get pending decommissioning requests: {content}");
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
            Console.WriteLine($"[ERROR] GetPendingDecommissioningRequestsAsync: {ex.Message}");
            return new List<DecommissioningRequest>();
        }
    }

    /// <summary>
    /// Review (approve/reject) a decommissioning request
    /// POST /decommissioning/requests/review
    /// </summary>
    public async Task<bool> ReviewDecommissioningRequestAsync(ReviewDecommissioningRequestDto review)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.ReviewRequest;
            var response = await _httpClient.PostAsJsonAsync(url, review);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to review decommissioning request: {errorContent}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] ReviewDecommissioningRequestAsync: {ex.Message}");
            return false;
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

    /// <summary>
    /// Creates a new decommissioning request
    /// POST /decommissioning/requests
    /// </summary>
    public async Task<bool> CreateDecommissioningRequestAsync(CreateDecommissioningRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var url = ApiRoutes.Decommissioning.CreateRequest;
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to create decommissioning request: {errorContent}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CreateDecommissioningRequestAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Updates a decommissioning request
    /// PUT /decommissioning/requests
    /// </summary>
    public async Task<bool> UpdateDecommissioningRequestAsync(DecommissioningRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            // Map the model to the API DTO
            // Backend expects: 0=Accepted, 1=Rejected, 2=Pending
            var statusValue = request.Status switch
            {
                DecommissioningStatus.Accepted => 0,
                DecommissioningStatus.Rejected => 1,
                DecommissioningStatus.Pending => 2,
                _ => 2  // Default to Pending
            };

            var updateDto = new UpdateDecommissioningRequestDto
            {
                DecommissioningRequestId = request.Id,
                TechnicianId = request.TechnicianId,
                DeviceId = request.DeviceId,
                EmissionDate = request.RequestDate,
                AnswerDate = request.ReviewedDate,
                Status = statusValue,
                Reason = (int)request.Reason,
                DeviceReceiverId = request.ReceiverUserId,
                IsApproved = request.Status == DecommissioningStatus.Accepted ? true :
                             request.Status == DecommissioningStatus.Rejected ? false :
                             null, // Pending
                FinalDestinationDepartmentID = request.FinalDestinationId,
                LogisticId = request.ReviewedByUserId,
                Description = request.Justification
            };

            var url = ApiRoutes.Decommissioning.UpdateRequest;
            var response = await _httpClient.PutAsJsonAsync(url, updateDto);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to update decommissioning request: {errorContent}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] UpdateDecommissioningRequestAsync: {ex.Message}");
            return false;
        }
    }

    // Mappers
    private DecommissioningRequestDto MapModelToDto(DecommissioningRequest model)
    {
        return new DecommissioningRequestDto
        {
            DecommissioningRequestId = model.Id,
            TechnicianId = model.TechnicianId,
            TechnicianName = model.TechnicianName,
            DeviceId = model.DeviceId,
            DeviceName = model.DeviceName,
            RequestDate = model.RequestDate,
            ReviewedDate = model.ReviewedDate,
            // Backend expects: 0=Accepted, 1=Rejected, 2=Pending
            Status = model.Status switch
            {
                DecommissioningStatus.Accepted => 0,
                DecommissioningStatus.Rejected => 1,
                DecommissioningStatus.Pending => 2,
                _ => 2
            },
            Reason = (int)model.Reason,
            ReasonDescription = model.ReasonDescription,
            ReceiverUserId = model.ReceiverUserId,
            ReceiverUserName = model.ReceiverUserName,
            ReviewedByUserId = model.ReviewedByUserId,
            ReviewedByUserName = model.ReviewedByUserName,
            FinalDestinationId = model.FinalDestinationId,
            FinalDestinationName = model.FinalDestinationName,
            Justification = model.Justification
        };
    }

    private DecommissioningRequest MapDtoToModel(DecommissioningRequestDto dto)
    {
        // Backend status values: 0=Accepted, 1=Rejected, 2=Pending
        // Enum values match: Accepted=0, Rejected=1, Pending=2
        var status = dto.Status switch
        {
            0 => DecommissioningStatus.Accepted,
            1 => DecommissioningStatus.Rejected,
            2 => DecommissioningStatus.Pending,
            _ => DecommissioningStatus.Pending
        };

        return new DecommissioningRequest
        {
            Id = dto.DecommissioningRequestId,
            DeviceId = dto.DeviceId,
            DeviceName = dto.DeviceName,
            TechnicianId = dto.TechnicianId,
            TechnicianName = dto.TechnicianName,
            RequestDate = dto.RequestDate,
            Status = status,
            Justification = dto.Justification ?? string.Empty,
            Reason = (DecommissioningReason)dto.Reason,
            ReasonDescription = dto.ReasonDescription,
            ReviewedDate = dto.ReviewedDate,
            ReviewedByUserId = dto.ReviewedByUserId,
            ReviewedByUserName = dto.ReviewedByUserName,
            // Receiver info from backend
            ReceiverUserId = dto.ReceiverUserId,
            ReceiverUserName = dto.ReceiverUserName,
            // Final destination from backend
            FinalDestinationId = dto.FinalDestinationId,
            FinalDestinationName = dto.FinalDestinationName,
            // Decommissioning date - use reviewed date if status is accepted
            DecommissioningDate = status == DecommissioningStatus.Accepted ? dto.ReviewedDate : null
        };
    }
}
