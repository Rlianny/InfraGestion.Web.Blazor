using System.Net.Http.Json;
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
    /// Actualiza datos de un técnico (placeholder - backend no tiene este endpoint)
    /// </summary>
    public Task<Technician?> UpdateTechnicianAsync(UpdateTechnicianRequest request)
    {
        // TODO: Implementar cuando el backend tenga el endpoint PUT
        Console.WriteLine("UpdateTechnicianAsync: Endpoint no disponible en el backend");
        return Task.FromResult<Technician?>(null);
    }

    // ==================== DETAIL METHODS ====================

    public async Task<TechnicianDetails?> GetTechnicianDetailsAsync(int id)
    {
        try
        {
            var technician = await GetTechnicianByIdAsync(id);
            if (technician == null)
                return null;

            var performances = await GetTechnicianPerformancesAsync(id);
            var bonuses = await GetTechnicianBonusesAsync(id);
            var penalties = await GetTechnicianPenaltiesAsync(id);

            // Calcular rating promedio
            decimal avgRating = performances.Any() 
                ? performances.Average(p => p.Score) 
                : 0;

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

            return details;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener detalles del tecnico: {ex.Message}");
            return null;
        }
    }
}
