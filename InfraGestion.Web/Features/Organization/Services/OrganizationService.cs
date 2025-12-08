using System.Net.Http.Json;
using InfraGestion.Web.Features.Organization.Models;
using InfraGestion.Web.Features.Organization.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Auth.DTOs;

namespace InfraGestion.Web.Features.Organization.Services;

public class OrganizationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private const string BASE_URL = "organization";

    public OrganizationService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    // ==================== SECTION METHODS ====================

    public async Task<List<Section>> GetAllSectionsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/sections");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<SectionDto>>>();
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data.Select(dto => new Section
                    {
                        Id = dto.SectionId,
                        Name = dto.Name,
                        SectionManager = dto.SectionManager ?? string.Empty,
                        Status = OrganizationStatus.Active
                    }).ToList();
                }
            }

            return new List<Section>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener secciones: {ex.Message}");
            return new List<Section>();
        }
    }

    public async Task<Section?> GetSectionByIdAsync(int id)
    {
        try
        {
            var sections = await GetAllSectionsAsync();
            return sections.FirstOrDefault(s => s.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener seccion: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateSectionAsync(CreateSectionRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new SectionRequestDto
            {
                SectionId = 0,
                Name = request.Name,
                SectionManager = request.ManagerName
            };

            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/sections", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear seccion: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateSectionAsync(UpdateSectionRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new SectionRequestDto
            {
                SectionId = request.Id,
                Name = request.Name,
                SectionManager = request.ManagerName
            };

            var response = await _httpClient.PutAsJsonAsync($"{BASE_URL}/sections", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar seccion: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteSectionAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var section = await GetSectionByIdAsync(id);
            if (section == null) return false;

            var dto = new SectionRequestDto
            {
                SectionId = id,
                Name = section.Name
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BASE_URL}/sections")
            {
                Content = JsonContent.Create(dto)
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar seccion: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ToggleSectionStatusAsync(int id)
    {
        return await DeleteSectionAsync(id);
    }

    public async Task<bool> AssignSectionResponsibleAsync(int sectionId, int userId)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new AssignSectionResponsibleRequestDto
            {
                SectionId = sectionId,
                UserId = userId
            };

            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/sections/managers", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al asignar responsable: {ex.Message}");
            return false;
        }
    }

    public async Task<List<(int Id, string Name)>> GetSectionNamesAsync()
    {
        var sections = await GetAllSectionsAsync();
        return sections
            .Where(s => s.Status == OrganizationStatus.Active)
            .Select(s => (s.Id, s.Name))
            .ToList();
    }

    // ==================== DEPARTMENT METHODS ====================

    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BASE_URL}/departments");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<DepartmentDto>>>();
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    // Obtener secciones para mapear nombres
                    var sections = await GetAllSectionsAsync();
                    
                    return apiResponse.Data.Select(dto => new Department
                    {
                        Id = dto.DepartmentId,
                        Name = dto.Name,
                        SectionId = dto.SectionId,
                        SectionName = sections.FirstOrDefault(s => s.Id == dto.SectionId)?.Name ?? string.Empty,
                        Status = OrganizationStatus.Active
                    }).ToList();
                }
            }

            return new List<Department>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener departamentos: {ex.Message}");
            return new List<Department>();
        }
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        try
        {
            var departments = await GetAllDepartmentsAsync();
            return departments.FirstOrDefault(d => d.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener departamento: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new DepartmentRequestDto
            {
                SectionId = request.SectionId,
                DepartmentId = 0,
                Name = request.Name
            };

            var response = await _httpClient.PostAsJsonAsync($"{BASE_URL}/departments", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear departamento: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateDepartmentAsync(UpdateDepartmentRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new DepartmentRequestDto
            {
                SectionId = request.SectionId,
                DepartmentId = request.Id,
                Name = request.Name
            };

            var response = await _httpClient.PutAsJsonAsync($"{BASE_URL}/departments", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar departamento: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var department = await GetDepartmentByIdAsync(id);
            if (department == null) return false;

            var dto = new DepartmentRequestDto
            {
                SectionId = department.SectionId,
                DepartmentId = id,
                Name = department.Name
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BASE_URL}/departments")
            {
                Content = JsonContent.Create(dto)
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar departamento: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ToggleDepartmentStatusAsync(int id)
    {
        return await DeleteDepartmentAsync(id);
    }
}
