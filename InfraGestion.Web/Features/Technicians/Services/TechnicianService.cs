using System.Net.Http.Json;
using System.Linq;
using InfraGestion.Web.Features.Technicians.Models;
using InfraGestion.Web.Features.Technicians.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Organization.Services;

namespace InfraGestion.Web.Features.Technicians.Services;

public class TechnicianService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly OrganizationService _organizationService;
    private const string BASE_URL = "personnel";

    public TechnicianService(HttpClient httpClient, AuthService authService, OrganizationService organizationService)
    {
        _httpClient = httpClient;
        _authService = authService;
        _organizationService = organizationService;
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetAllTechniciansAsync status: {response.StatusCode}, response: {content}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Intentar deserializar como ApiResponse primero
                try
                {
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<List<TechnicianDto>>>(content, jsonOptions);
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
                catch { }

                // Si falla, intentar como lista directa
                try
                {
                    var directList = System.Text.Json.JsonSerializer.Deserialize<List<TechnicianDto>>(content, jsonOptions);
                    if (directList != null && directList.Any())
                    {
                        return directList.Select(dto => new Technician
                        {
                            Id = dto.TechnicianId,
                            Name = dto.Name,
                            Specialty = dto.Specialty,
                            YearsOfExperience = dto.YearsOfExperience,
                            Status = TechnicianStatus.Active
                        }).OrderBy(t => t.Name).ToList();
                    }
                }
                catch { }
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianByIdAsync status: {response.StatusCode}, response: {content}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Intentar deserializar como ApiResponse primero
                try
                {
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TechnicianDto>>(content, jsonOptions);
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
                catch { }

                // Si falla, intentar como objeto directo
                try
                {
                    var directDto = System.Text.Json.JsonSerializer.Deserialize<TechnicianDto>(content, jsonOptions);
                    if (directDto != null)
                    {
                        return new Technician
                        {
                            Id = directDto.TechnicianId,
                            Name = directDto.Name,
                            Specialty = directDto.Specialty,
                            YearsOfExperience = directDto.YearsOfExperience,
                            Status = TechnicianStatus.Active
                        };
                    }
                }
                catch { }
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianBonusesAsync status: {response.StatusCode}, response: {content}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Intentar deserializar como ApiResponse primero
                try
                {
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<List<BonusDto>>>(content, jsonOptions);
                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                catch { }

                // Si falla, intentar como lista directa
                try
                {
                    var directList = System.Text.Json.JsonSerializer.Deserialize<List<BonusDto>>(content, jsonOptions);
                    if (directList != null)
                    {
                        return directList;
                    }
                }
                catch { }
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianPenaltiesAsync status: {response.StatusCode}, response: {content}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Intentar deserializar como ApiResponse primero
                try
                {
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<List<PenaltyDto>>>(content, jsonOptions);
                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                catch { }

                // Si falla, intentar como lista directa
                try
                {
                    var directList = System.Text.Json.JsonSerializer.Deserialize<List<PenaltyDto>>(content, jsonOptions);
                    if (directList != null)
                    {
                        return directList;
                    }
                }
                catch { }
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianPerformancesAsync status: {response.StatusCode}, response: {content}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Intentar deserializar como ApiResponse primero
                try
                {
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<List<RateDto>>>(content, jsonOptions);
                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                catch { }

                // Si falla, intentar como lista directa
                try
                {
                    var directList = System.Text.Json.JsonSerializer.Deserialize<List<RateDto>>(content, jsonOptions);
                    if (directList != null)
                    {
                        return directList;
                    }
                }
                catch { }
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GetTechnicianDetailsAsync status: {response.StatusCode}, response: {content}");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error al obtener detalles del técnico: {response.StatusCode}");
                // Intentar obtener datos básicos del técnico si el endpoint de detalle falla
                return await GetBasicTechnicianDetailsAsync(id);
            }

            // API devuelve ApiResponse<TechnicianDetailDto>
            TechnicianDetailDto? payload = null;
            
            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var apiWrapper = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TechnicianDetailDto>>(content, jsonOptions);
            if (apiWrapper?.Data != null)
            {
                payload = apiWrapper.Data;
            }

            if (payload == null)
            {
                Console.WriteLine("La API no devolvió detalles del técnico.");
                return null;
            }

            // Obtener valoraciones adicionales del endpoint de performances
            var ratings = await GetTechnicianPerformancesAsync(id);
            
            // Mapear historiales
            var maintenanceHistory = payload.MaintenanceRecords.Select(m => new MaintenanceRecord
            {
                Id = m.MaintenanceRecordId,
                Date = m.MaintenanceDate,
                Type = m.GetMaintenanceTypeName(),
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
                Cause = d.GetReasonName(),
                Receiver = d.DeviceReceiverName,
                Status = d.GetStatusName()
            }).ToList();

            // Mapear valoraciones
            var technicianRatings = ratings.Select(r => new TechnicianRating
            {
                Id = r.RateId,
                Date = r.Date,
                Issuer = r.GiverName,
                Score = (decimal)r.Score,
                Description = r.Comment
            }).ToList();

            // Calcular rating promedio
            decimal averageRating = technicianRatings.Any() 
                ? technicianRatings.Average(r => r.Score) 
                : 0m;

            // Calcular fecha de última intervención (último mantenimiento realizado)
            DateTime? lastInterventionDate = maintenanceHistory.Any()
                ? maintenanceHistory.Max(m => m.Date)
                : null;

            // Calcular fecha de contratación aproximada (basada en años de experiencia)
            DateTime hireDate = DateTime.Now.AddYears(-payload.YearsOfExperience);

            // Intentar obtener información de ubicación del usuario actual si es el mismo técnico
            string sectionName = "Sección Técnica";
            string departmentName = "Departamento de Mantenimiento";
            string sectionManager = "No asignado";

            // Intentar obtener información del usuario actual si coincide con el técnico
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null && currentUser.Id == id)
            {
                departmentName = !string.IsNullOrEmpty(currentUser.DepartmentName) 
                    ? currentUser.DepartmentName 
                    : departmentName;
            }

            // Intentar obtener información de ubicación desde las secciones
            try
            {
                var sections = await _organizationService.GetAllSectionsAsync();
                if (sections.Any())
                {
                    // Buscar una sección que coincida con la especialidad del técnico
                    var matchingSection = sections.FirstOrDefault(s => 
                        s.Name.Contains(payload.Specialty, StringComparison.OrdinalIgnoreCase) ||
                        payload.Specialty.Contains(s.Name, StringComparison.OrdinalIgnoreCase));
                    
                    if (matchingSection != null)
                    {
                        sectionName = matchingSection.Name;
                        sectionManager = matchingSection.SectionManager ?? sectionManager;
                    }
                    else if (sections.Any())
                    {
                        // Usar la primera sección disponible como fallback
                        var firstSection = sections.First();
                        sectionName = firstSection.Name;
                        sectionManager = firstSection.SectionManager ?? sectionManager;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] No se pudo obtener información de secciones: {ex.Message}");
            }

            var details = new TechnicianDetails
            {
                Id = payload.TechnicianId,
                Name = payload.Name,
                IdentificationNumber = $"TECH{payload.TechnicianId:D3}",
                Specialty = payload.Specialty,
                YearsOfExperience = payload.YearsOfExperience,
                Status = TechnicianStatus.Active,
                Rating = averageRating,
                HireDate = hireDate,
                LastInterventionDate = lastInterventionDate,
                Section = sectionName,
                Department = departmentName,
                SectionManager = sectionManager,
                MaintenanceHistory = maintenanceHistory,
                DecommissionProposals = decommissionProposals,
                Ratings = technicianRatings
            };

            return details;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener detalles del tecnico: {ex.Message}");
            // Intentar obtener datos básicos como fallback
            return await GetBasicTechnicianDetailsAsync(id);
        }
    }

    /// <summary>
    /// Obtiene detalles básicos del técnico cuando el endpoint de detalle no está disponible
    /// </summary>
    private async Task<TechnicianDetails?> GetBasicTechnicianDetailsAsync(int id)
    {
        try
        {
            Console.WriteLine($"[DEBUG] Intentando obtener datos básicos del técnico {id}");
            
            // Obtener datos básicos del técnico
            var technician = await GetTechnicianByIdAsync(id);
            if (technician == null)
            {
                Console.WriteLine($"[DEBUG] No se encontró el técnico {id}");
                return null;
            }

            // Obtener valoraciones
            var ratings = await GetTechnicianPerformancesAsync(id);
            var technicianRatings = ratings.Select(r => new TechnicianRating
            {
                Id = r.RateId,
                Date = r.Date,
                Issuer = r.GiverName,
                Score = (decimal)r.Score,
                Description = r.Comment
            }).ToList();

            decimal averageRating = technicianRatings.Any()
                ? technicianRatings.Average(r => r.Score)
                : 0m;

            DateTime hireDate = DateTime.Now.AddYears(-technician.YearsOfExperience);

            // Intentar obtener información de ubicación
            string sectionName = "Sección Técnica";
            string departmentName = "Departamento de Mantenimiento";
            string sectionManager = "No asignado";

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.DepartmentName))
            {
                departmentName = currentUser.DepartmentName;
            }

            try
            {
                var sections = await _organizationService.GetAllSectionsAsync();
                if (sections.Any())
                {
                    var matchingSection = sections.FirstOrDefault(s =>
                        s.Name.Contains(technician.Specialty, StringComparison.OrdinalIgnoreCase) ||
                        technician.Specialty.Contains(s.Name, StringComparison.OrdinalIgnoreCase));

                    if (matchingSection != null)
                    {
                        sectionName = matchingSection.Name;
                        sectionManager = matchingSection.SectionManager ?? sectionManager;
                    }
                    else
                    {
                        var firstSection = sections.First();
                        sectionName = firstSection.Name;
                        sectionManager = firstSection.SectionManager ?? sectionManager;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] No se pudo obtener información de secciones: {ex.Message}");
            }

            return new TechnicianDetails
            {
                Id = technician.Id,
                Name = technician.Name,
                IdentificationNumber = $"TECH{technician.Id:D3}",
                Specialty = technician.Specialty,
                YearsOfExperience = technician.YearsOfExperience,
                Status = technician.Status,
                Rating = averageRating,
                HireDate = hireDate,
                LastInterventionDate = null,
                Section = sectionName,
                Department = departmentName,
                SectionManager = sectionManager,
                MaintenanceHistory = new List<MaintenanceRecord>(),
                DecommissionProposals = new List<DecommissionProposal>(),
                Ratings = technicianRatings
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] Error en GetBasicTechnicianDetailsAsync: {ex.Message}");
            return null;
        }
    }
}
