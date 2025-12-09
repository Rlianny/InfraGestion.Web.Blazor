using System.Net.Http.Json;
using System.Linq;
using InfraGestion.Web.Features.Technicians.Models;
using InfraGestion.Web.Features.Technicians.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Auth.DTOs;

namespace InfraGestion.Web.Features.Technicians.Services;

public class TechnicianService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private const string BASE_URL = "personnel";

    public TechnicianService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    // ==================== GET METHODS ====================

    public async Task<List<Technician>> GetAllTechniciansAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/technicians");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<TechnicianDto>>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data.Select(dto => new Technician
                    {
                        Id = dto.TechnicianId,
                        Name = dto.Name,
                        Specialty = dto.Specialty,
                        YearsOfExperience = dto.YearsOfExperience,
                        Status = TechnicianStatus.Active
                    }).OrderBy(t => t.Name).ToList();
                }
            }

            return new List<Technician>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener tecnicos: {ex.Message}");
            return new List<Technician>();
        }
    }

    public async Task<Technician?> GetTechnicianByIdAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/technician/{id}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TechnicianDto>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    var dto = apiResponse.Data;
                    return new Technician
                    {
                        Id = dto.TechnicianId,
                        Name = dto.Name,
                        Specialty = dto.Specialty,
                        YearsOfExperience = dto.YearsOfExperience,
                        Status = TechnicianStatus.Active
                    };
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener tecnico: {ex.Message}");
            return null;
        }
    }

    public async Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/bonuses/{technicianId}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BonusDto>>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
            }

            return new List<BonusDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener bonificaciones: {ex.Message}");
            return new List<BonusDto>();
        }
    }

    public async Task<List<PenaltyDto>> GetTechnicianPenaltiesAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/penalties/{technicianId}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<PenaltyDto>>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
            }

            return new List<PenaltyDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener penalizaciones: {ex.Message}");
            return new List<PenaltyDto>();
        }
    }

    public async Task<List<RateDto>> GetTechnicianPerformancesAsync(int technicianId)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/performances/{technicianId}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<RateDto>>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
            }

            return new List<RateDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener rendimientos: {ex.Message}");
            return new List<RateDto>();
        }
    }

    // ==================== POST METHODS ====================

    public async Task<bool> RateTechnicianAsync(RateTechnicianRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/rate", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al calificar tecnico: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddBonusAsync(BonusRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/bonus", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar bonificacion: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddPenaltyAsync(PenaltyRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/penalty", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar penalizacion: {ex.Message}");
            return false;
        }
    }

    // ==================== HELPER METHODS (UI Support) ====================

    /// <summary>
    /// Obtiene lista de especialidades para filtros (derivado de técnicos existentes)
    /// </summary>
    public async Task<List<string>> GetSpecialtiesAsync()
    {
        var technicians = await GetAllTechniciansAsync();
        return technicians
            .Select(t => t.Specialty)
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .OrderBy(s => s)
            .ToList();
    }

    /// <summary>
    /// Obtiene lista de secciones para filtros (derivado de técnicos existentes)
    /// </summary>
    public async Task<List<string>> GetSectionsAsync()
    {
        var technicians = await GetAllTechniciansAsync();
        return technicians
            .Select(t => t.Section)
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .OrderBy(s => s)
            .ToList();
    }

    /// <summary>
    /// Actualiza datos de un técnico
    /// </summary>
    public async Task<Technician?> UpdateTechnicianAsync(UpdateTechnicianRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var updateRequest = new
            {
                FullName = request.Name,
                Specialty = request.Specialty,
                YearsOfExperience = (int?)null,
                DepartmentId = (int?)null
            };
            
            var response = await _httpClient.PutAsJsonAsync($"{BASE_URL}/technician/{request.Id}", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TechnicianDto>>();

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    var dto = apiResponse.Data;
                    return new Technician
                    {
                        Id = dto.TechnicianId,
                        Name = dto.Name,
                        Specialty = dto.Specialty,
                        YearsOfExperience = dto.YearsOfExperience,
                        Status = TechnicianStatus.Active
                    };
                }
            }
            
            Console.WriteLine($"Error al actualizar técnico: {response.StatusCode}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar técnico: {ex.Message}");
            return null;
        }
    }

    // ==================== DETAIL METHODS ====================

    public async Task<TechnicianDetails?> GetTechnicianDetailsAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var response = await _httpClient.GetAsync($"{BASE_URL}/technician/{id}/detail");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error al obtener detalles del técnico: {response.StatusCode}");
                return null;
            }

            // API puede devolver ApiResponse o el objeto directo
            TechnicianDetailResponse? payload = null;
            var apiWrapper = await response.Content.ReadFromJsonAsync<ApiResponse<TechnicianDetailResponse>>();
            if (apiWrapper?.Data != null)
            {
                payload = apiWrapper.Data;
            }
            else
            {
                payload = await response.Content.ReadFromJsonAsync<TechnicianDetailResponse>();
            }

            if (payload == null)
            {
                Console.WriteLine("La API no devolvió detalles del técnico.");
                return null;

            // Obtener datos adicionales (performances, bonuses, penalties)
            var performances = await GetTechnicianPerformancesAsync(id);
            var bonuses = await GetTechnicianBonusesAsync(id);
            var penalties = await GetTechnicianPenaltiesAsync(id);

            var ratings = performances.Select(p => new TechnicianRating
            {
                Id = p.RateId,
                Date = p.Date,
                Issuer = string.IsNullOrWhiteSpace(p.GiverName) ? $"Usuario {p.GiverId}" : p.GiverName,
                Score = (decimal)p.Score,
                Description = p.Comment
            }).ToList();

            var avgRating = ratings.Any() ? ratings.Average(r => r.Score) : (decimal)payload.Rating;

            var maintenanceHistory = payload.MaintenanceRecords.Select(m => new MaintenanceRecord
            {
                Id = m.MaintenanceRecordId,
                Date = m.MaintenanceDate,
                Type = string.IsNullOrWhiteSpace(m.MaintenanceType) ? "Mantenimiento" : m.MaintenanceType,
                TechnicianName = m.TechnicianName,
                Notes = m.Description,
                Cost = (decimal)m.Cost,
                DeviceId = m.DeviceId.ToString(),
                DeviceName = m.DeviceName
            }).ToList();

            var decommissionProposals = payload.DecommissioningRequests.Select(d => new DecommissionProposal
            {
                Id = d.DecommissioningRequestId,
                Date = d.RequestDate,
                DeviceId = d.DeviceId.ToString(),
                DeviceName = d.DeviceName,
                Cause = d.Reason,
                Receiver = d.DeviceReceiverName,
                Status = string.IsNullOrWhiteSpace(d.Status) ? "Pendiente" : d.Status
            }).ToList();

            var details = new TechnicianDetails
            {
                Id = technician.Id,
                Name = technician.Name,
                IdentificationNumber = $"TECH{technician.Id:D3}",
                Specialty = technician.Specialty,
                YearsOfExperience = technician.YearsOfExperience,
                Status = technician.Status,
                Rating = avgRating,
                Ratings = performances.Select(p => new TechnicianRating
                {
                    Id = p.RateId,
                    Date = p.Date,
                    Issuer = p.RatedBy,
                    Score = p.Score,
                    Description = p.Comment
                }).ToList(),
                Bonuses = bonuses,
                Penalties = penalties
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener detalles del tecnico: {ex.Message}");
            return null;
        }
    }
}
