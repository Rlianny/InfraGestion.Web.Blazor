using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Organization.Services;
using InfraGestion.Web.Features.Technicians.DTOs;
using InfraGestion.Web.Features.Technicians.Models;

namespace InfraGestion.Web.Features.Technicians.Services;

/// <summary>
/// Service for managing technicians - consumes /personnel/* endpoints from the backend
/// </summary>
public class TechnicianService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly OrganizationService _organizationService;
    private const string BASE_URL = "personnel";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public TechnicianService(
        HttpClient httpClient,
        AuthService authService,
        OrganizationService organizationService
    )
    {
        _httpClient = httpClient;
        _authService = authService;
        _organizationService = organizationService;
    }

    #region GET Methods

    /// <summary>
    /// Get all technicians - GET /personnel/technicians
    /// </summary>
    public async Task<List<Technician>> GetAllTechniciansAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/technicians");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error getting technicians: {response.StatusCode}"
                );
                return new List<Technician>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<TechnicianDto>>>(
                content,
                JsonOptions
            );

            if (apiResponse?.Success != true || apiResponse.Data == null)
            {
                return new List<Technician>();
            }

            return apiResponse.Data.Select(MapToTechnician).OrderBy(t => t.Name).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in GetAllTechniciansAsync: {ex.Message}"
            );
            return new List<Technician>();
        }
    }

    /// <summary>
    /// Get a technician by ID - GET /personnel/technician/{id}
    /// </summary>
    public async Task<Technician?> GetTechnicianByIdAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/technician/{technicianId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error getting technician {technicianId}: {response.StatusCode}"
                );
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TechnicianDto>>(
                content,
                JsonOptions
            );

            return apiResponse?.Success == true && apiResponse.Data != null
                ? MapToTechnician(apiResponse.Data)
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in GetTechnicianByIdAsync: {ex.Message}"
            );
            return null;
        }
    }

    /// <summary>
    /// Get full details of a technician - GET /personnel/technician/{id}/details
    /// </summary>
    public async Task<TechnicianDetails?> GetTechnicianDetailsAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{BASE_URL}/technician/{technicianId}/details"
            );

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error getting details of technician {technicianId}: {response.StatusCode}"
                );
                return await BuildBasicTechnicianDetailsAsync(technicianId);
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TechnicianDetailDto>>(
                content,
                JsonOptions
            );

            if (apiResponse?.Success != true || apiResponse.Data == null)
            {
                return await BuildBasicTechnicianDetailsAsync(technicianId);
            }

            return MapToTechnicianDetails(apiResponse.Data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in GetTechnicianDetailsAsync: {ex.Message}"
            );
            return await BuildBasicTechnicianDetailsAsync(technicianId);
        }
    }

    /// <summary>
    /// Get bonuses of a technician - GET /personnel/bonuses/{id}
    /// </summary>
    public async Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/bonuses/{technicianId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<BonusDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<BonusDto>>>(
                content,
                JsonOptions
            );

            return apiResponse?.Success == true && apiResponse.Data != null
                ? apiResponse.Data
                : new List<BonusDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in GetTechnicianBonusesAsync: {ex.Message}"
            );
            return new List<BonusDto>();
        }
    }

    /// <summary>
    /// Gets technician penalties - GET /personnel/penalties/{id}
    /// </summary>
    public async Task<List<PenaltyDto>> GetTechnicianPenaltiesAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/penalties/{technicianId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<PenaltyDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PenaltyDto>>>(
                content,
                JsonOptions
            );

            return apiResponse?.Success == true && apiResponse.Data != null
                ? apiResponse.Data
                : new List<PenaltyDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception en GetTechnicianPenaltiesAsync: {ex.Message}"
            );
            return new List<PenaltyDto>();
        }
    }

    /// <summary>
    /// Get technician performance history - GET /personnel/performances/{id}
    /// </summary>
    public async Task<List<RateDto>> GetTechnicianPerformancesAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/performances/{technicianId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<RateDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<RateDto>>>(
                content,
                JsonOptions
            );

            return apiResponse?.Success == true && apiResponse.Data != null
                ? apiResponse.Data
                : new List<RateDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in GetTechnicianPerformancesAsync: {ex.Message}"
            );
            return new List<RateDto>();
        }
    }

    #endregion

    #region PUT Methods

    /// <summary>
    /// Update a technician - PUT /personnel/technician/{id}
    /// </summary>
    public async Task<Technician?> UpdateTechnicianAsync(UpdateTechnicianRequest request)
    {
        try
        {
            var requestDto = new UpdateTechnicianRequestDto
            {
                TechnicianId = request.TechnicianId,
                FullName = request.FullName,
                Specialty = request.Specialty ?? string.Empty,
                YearsOfExperience = request.YearsOfExperience ?? 0,
                DepartmentId = request.DepartmentId ?? 0,
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"{BASE_URL}/technician/{request.TechnicianId}",
                requestDto
            );

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error updating technician {request.TechnicianId}: {response.StatusCode}"
                );
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TechnicianDto>>(
                content,
                JsonOptions
            );

            return apiResponse?.Success == true && apiResponse.Data != null
                ? MapToTechnician(apiResponse.Data)
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in UpdateTechnicianAsync: {ex.Message}"
            );
            return null;
        }
    }

    #endregion

    #region POST Methods

    /// <summary>
    /// Rate a technician - POST /personnel/rate
    /// </summary>
    public async Task<bool> RateTechnicianAsync(RateTechnicianRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/rate", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception in RateTechnicianAsync: {ex.Message}"
            );
            return false;
        }
    }

    /// <summary>
    /// Adds a bonus to a technician - POST /personnel/bonus
    /// </summary>
    public async Task<bool> AddBonusAsync(BonusRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/bonus", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TechnicianService] Exception in AddBonusAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Adds a penalty to a technician - POST /personnel/penalty
    /// </summary>
    public async Task<bool> AddPenaltyAsync(PenaltyRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/penalty", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TechnicianService] Exception in AddPenaltyAsync: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Helper Methods (UI Support)

    /// <summary>
    /// Get unique specialties of technicians
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
    /// Get unique sections of technicians
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

    #endregion

    #region Private Mapping Methods

    /// <summary>
    /// Maps TechnicianDto to Technician model for UI
    /// </summary>
    private static Technician MapToTechnician(TechnicianDto dto)
    {
        // Map fields provided by the summary DTO. Use boolean `IsActive` when available.
        var status = TechnicianStatus.Active;
        if (dto.IsActive.HasValue)
        {
            status = dto.IsActive.Value ? TechnicianStatus.Active : TechnicianStatus.Inactive;
        }

        var rating = dto.AverageRating.HasValue ? (decimal)dto.AverageRating.Value : 0m;
        
        return new Technician
        {
            Id = dto.TechnicianId,
            Name = dto.Name,
            Specialty = dto.Specialty,
            YearsOfExperience = dto.YearsOfExperience,
            Status = status,
            Rating = rating,
            HireDate = DateTime.Now.AddYears(-dto.YearsOfExperience),
        };
    }

    /// <summary>
    /// Maps TechnicianDetailDto to TechnicianDetails model for UI
    /// </summary>
    private static TechnicianDetails MapToTechnicianDetails(TechnicianDetailDto dto)
    {
        return new TechnicianDetails
        {
            Id = dto.TechnicianId,
            Name = dto.Name,
            IdentificationNumber = $"TECH{dto.TechnicianId:D3}",
            Specialty = dto.Specialty,
            YearsOfExperience = dto.YearsOfExperience,
            Status = TechnicianStatus.Active,
            Rating = (decimal)dto.AverageRating,
            HireDate = dto.CreatedAt,
            CreatedAt = dto.CreatedAt,
            LastInterventionDate = dto.LastInterventionDate,
            Section = dto.SectionName,
            Department = dto.DepartmentName,
            SectionManager = dto.SectionManagerName,

            MaintenanceHistory = dto
                .MaintenanceRecords.Select(m => new MaintenanceRecord
                {
                    Id = m.MaintenanceRecordId,
                    Date = m.MaintenanceDate,
                    Type = m.GetMaintenanceTypeName(),
                    TechnicianName = m.TechnicianName,
                    Notes = m.Description,
                    Cost = (decimal)m.Cost,
                    DeviceId = m.DeviceId.ToString(),
                    DeviceName = m.DeviceName,
                })
                .ToList(),

            DecommissionProposals = dto
                .DecommissioningRequests.Select(d => new DecommissionProposal
                {
                    Id = d.DecommissioningRequestId,
                    Date = d.RequestDate,
                    DeviceId = d.DeviceId.ToString(),
                    DeviceName = d.DeviceName,
                    Cause = d.ReasonDescription,
                    Receiver = d.DeviceReceiverName,
                    Status = d.GetStatusName(),
                })
                .ToList(),

            Ratings = dto
                .Ratings.Select(r => new TechnicianRating
                {
                    Id = r.RateId,
                    Date = r.Date,
                    Issuer = r.GiverName,
                    Score = (decimal)r.Score,
                    Description = r.Comment,
                })
                .ToList(),
        };
    }

    /// <summary>
    /// Builds basic technician details when the detail endpoint fails
    /// </summary>
    private async Task<TechnicianDetails?> BuildBasicTechnicianDetailsAsync(int technicianId)
    {
        var technician = await GetTechnicianByIdAsync(technicianId);
        if (technician == null)
        {
            return null;
        }

        var ratings = await GetTechnicianPerformancesAsync(technicianId);
        var technicianRatings = ratings
            .Select(r => new TechnicianRating
            {
                Id = r.RateId,
                Date = r.Date,
                Issuer = r.GiverName,
                Score = (decimal)r.Score,
                Description = r.Comment,
            })
            .ToList();

        decimal averageRating = technicianRatings.Any()
            ? technicianRatings.Average(r => r.Score)
            : 0m;

        return new TechnicianDetails
        {
            Id = technician.Id,
            Name = technician.Name,
            IdentificationNumber = $"TECH{technician.Id:D3}",
            Specialty = technician.Specialty,
            YearsOfExperience = technician.YearsOfExperience,
            Status = technician.Status,
            Rating = averageRating,
            HireDate = technician.HireDate,
            LastInterventionDate = null,
            Section = "N/A",
            Department = "N/A",
            SectionManager = "N/A",
            MaintenanceHistory = new List<MaintenanceRecord>(),
            DecommissionProposals = new List<DecommissionProposal>(),
            Ratings = technicianRatings,
        };
    }

    #endregion
}
