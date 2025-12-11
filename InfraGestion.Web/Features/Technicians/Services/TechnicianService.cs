using System.Net.Http.Json;
using System.Text.Json;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Organization.Services;
using InfraGestion.Web.Features.Technicians.DTOs;
using InfraGestion.Web.Features.Technicians.Models;

namespace InfraGestion.Web.Features.Technicians.Services;

/// <summary>
/// Servicio para gestión de técnicos - consume /personnel/* endpoints del backend
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
    /// Obtiene todos los técnicos - GET /personnel/technicians
    /// </summary>
    public async Task<List<Technician>> GetAllTechniciansAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/technicians");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error al obtener técnicos: {response.StatusCode}"
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
                $"[TechnicianService] Exception en GetAllTechniciansAsync: {ex.Message}"
            );
            return new List<Technician>();
        }
    }

    /// <summary>
    /// Obtiene un técnico por ID - GET /personnel/technician/{id}
    /// </summary>
    public async Task<Technician?> GetTechnicianByIdAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/technician/{technicianId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error al obtener técnico {technicianId}: {response.StatusCode}"
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
                $"[TechnicianService] Exception en GetTechnicianByIdAsync: {ex.Message}"
            );
            return null;
        }
    }

    /// <summary>
    /// Obtiene detalles completos de un técnico - GET /personnel/technician/{id}/detail
    /// </summary>
    public async Task<TechnicianDetails?> GetTechnicianDetailsAsync(int technicianId)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{BASE_URL}/technician/{technicianId}/detail"
            );

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[TechnicianService] Error al obtener detalles del técnico {technicianId}: {response.StatusCode}"
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

            return await MapToTechnicianDetailsAsync(apiResponse.Data, technicianId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] Exception en GetTechnicianDetailsAsync: {ex.Message}"
            );
            return await BuildBasicTechnicianDetailsAsync(technicianId);
        }
    }

    /// <summary>
    /// Obtiene bonificaciones de un técnico - GET /personnel/bonuses/{id}
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
                $"[TechnicianService] Exception en GetTechnicianBonusesAsync: {ex.Message}"
            );
            return new List<BonusDto>();
        }
    }

    /// <summary>
    /// Obtiene penalizaciones de un técnico - GET /personnel/penalties/{id}
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
    /// Obtiene historial de rendimiento de un técnico - GET /personnel/performances/{id}
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
                $"[TechnicianService] Exception en GetTechnicianPerformancesAsync: {ex.Message}"
            );
            return new List<RateDto>();
        }
    }

    #endregion

    #region PUT Methods

    /// <summary>
    /// Actualiza un técnico - PUT /personnel/technician/{id}
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
                    $"[TechnicianService] Error al actualizar técnico {request.TechnicianId}: {response.StatusCode}"
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
                $"[TechnicianService] Exception en UpdateTechnicianAsync: {ex.Message}"
            );
            return null;
        }
    }

    #endregion

    #region POST Methods

    /// <summary>
    /// Califica a un técnico - POST /personnel/rate
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
                $"[TechnicianService] Exception en RateTechnicianAsync: {ex.Message}"
            );
            return false;
        }
    }

    /// <summary>
    /// Agrega una bonificación a un técnico - POST /personnel/bonus
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
            Console.WriteLine($"[TechnicianService] Exception en AddBonusAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Agrega una penalización a un técnico - POST /personnel/penalty
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
            Console.WriteLine($"[TechnicianService] Exception en AddPenaltyAsync: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Helper Methods (UI Support)

    /// <summary>
    /// Obtiene lista de especialidades únicas de los técnicos
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
    /// Obtiene lista de secciones únicas de los técnicos
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
    /// Mapea TechnicianDto a modelo Technician para UI
    /// </summary>
    private static Technician MapToTechnician(TechnicianDto dto)
    {
        return new Technician
        {
            Id = dto.TechnicianId,
            Name = dto.Name,
            Specialty = dto.Specialty,
            YearsOfExperience = dto.YearsOfExperience,
            Status = TechnicianStatus.Active,
            HireDate = DateTime.Now.AddYears(-dto.YearsOfExperience),
        };
    }

    /// <summary>
    /// Mapea TechnicianDetailDto a modelo TechnicianDetails para UI
    /// </summary>
    private async Task<TechnicianDetails> MapToTechnicianDetailsAsync(
        TechnicianDetailDto dto,
        int technicianId
    )
    {
        // Obtener valoraciones del endpoint de performances
        var ratings = await GetTechnicianPerformancesAsync(technicianId);

        // Mapear historial de mantenimientos
        var maintenanceHistory = dto
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
            .ToList();

        // Mapear proposiciones de baja
        var decommissionProposals = dto
            .DecommissioningRequests.Select(d => new DecommissionProposal
            {
                Id = d.DecommissioningRequestId,
                Date = d.RequestDate,
                DeviceId = d.DeviceId.ToString(),
                DeviceName = d.DeviceName,
                Cause = d.GetReasonName(),
                Receiver = d.DeviceReceiverName,
                Status = d.GetStatusName(),
            })
            .ToList();

        // Mapear valoraciones
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

        // Calcular valores derivados
        decimal averageRating = technicianRatings.Any()
            ? technicianRatings.Average(r => r.Score)
            : 0m;

        DateTime? lastInterventionDate = maintenanceHistory.Any()
            ? maintenanceHistory.Max(m => m.Date)
            : null;

        // Obtener información de ubicación
        var (sectionName, departmentName, sectionManager) = await GetLocationInfoAsync(
            dto.Specialty,
            technicianId
        );

        return new TechnicianDetails
        {
            Id = dto.TechnicianId,
            Name = dto.Name,
            IdentificationNumber = $"TECH{dto.TechnicianId:D3}",
            Specialty = dto.Specialty,
            YearsOfExperience = dto.YearsOfExperience,
            Status = TechnicianStatus.Active,
            Rating = averageRating,
            HireDate = DateTime.Now.AddYears(-dto.YearsOfExperience),
            LastInterventionDate = lastInterventionDate,
            Section = sectionName,
            Department = departmentName,
            SectionManager = sectionManager,
            MaintenanceHistory = maintenanceHistory,
            DecommissionProposals = decommissionProposals,
            Ratings = technicianRatings,
        };
    }

    /// <summary>
    /// Construye detalles básicos de técnico cuando el endpoint de detalle falla
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

        var (sectionName, departmentName, sectionManager) = await GetLocationInfoAsync(
            technician.Specialty,
            technicianId
        );

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
            Section = sectionName,
            Department = departmentName,
            SectionManager = sectionManager,
            MaintenanceHistory = new List<MaintenanceRecord>(),
            DecommissionProposals = new List<DecommissionProposal>(),
            Ratings = technicianRatings,
        };
    }

    /// <summary>
    /// Obtiene información de ubicación (sección, departamento, manager) desde servicios relacionados
    /// </summary>
    private async Task<(
        string sectionName,
        string departmentName,
        string sectionManager
    )> GetLocationInfoAsync(string specialty, int technicianId)
    {
        string sectionName = "Sección Técnica";
        string departmentName = "Departamento de Mantenimiento";
        string sectionManager = "No asignado";

        try
        {
            // Intentar obtener departamento del usuario actual si coincide
            var currentUser = await _authService.GetCurrentUserAsync();
            if (
                currentUser != null
                && currentUser.Id == technicianId
                && !string.IsNullOrEmpty(currentUser.DepartmentName)
            )
            {
                departmentName = currentUser.DepartmentName;
            }

            // Intentar obtener sección que coincida con la especialidad
            var sections = await _organizationService.GetAllSectionsAsync();
            if (sections.Any())
            {
                var matchingSection = sections.FirstOrDefault(s =>
                    s.Name.Contains(specialty, StringComparison.OrdinalIgnoreCase)
                    || specialty.Contains(s.Name, StringComparison.OrdinalIgnoreCase)
                );

                if (matchingSection != null)
                {
                    sectionName = matchingSection.Name;
                    sectionManager = matchingSection.SectionManagerFullName ?? sectionManager;
                }
                else
                {
                    var firstSection = sections.First();
                    sectionName = firstSection.Name;
                    sectionManager = firstSection.SectionManagerFullName ?? sectionManager;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[TechnicianService] No se pudo obtener información de ubicación: {ex.Message}"
            );
        }

        return (sectionName, departmentName, sectionManager);
    }

    #endregion
}
