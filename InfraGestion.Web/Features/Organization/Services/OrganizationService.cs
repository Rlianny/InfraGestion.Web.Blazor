using System.Net.Http.Json;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Organization.DTOs;
using InfraGestion.Web.Features.Organization.Models;

namespace InfraGestion.Web.Features.Organization.Services;

public class OrganizationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private const string BaseUrl = "organization";

    public OrganizationService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    #region Sections

    public async Task<List<Section>> GetAllSectionsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BaseUrl}/sections");

            if (!response.IsSuccessStatusCode)
                return new List<Section>();

            var apiResponse = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<SectionDto>>
            >();

            if (apiResponse?.Success != true || apiResponse.Data == null)
                return new List<Section>();

            return apiResponse.Data.Select(MapToSection).ToList();
        }
        catch (Exception ex)
        {
            LogError("obtener secciones", ex);
            return new List<Section>();
        }
    }

    public async Task<Section?> GetSectionByIdAsync(int id)
    {
        var sections = await GetAllSectionsAsync();
        return sections.FirstOrDefault(s => s.Id == id);
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
                SectionManagerId = request.SectionManagerId,
            };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/sections", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("crear sección", ex);
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
                SectionManagerId = request.SectionManagerId,
            };

            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/sections", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("actualizar sección", ex);
            return false;
        }
    }

    public async Task<bool> DeleteSectionAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/sections/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("eliminar sección", ex);
            return false;
        }
    }

    public async Task<bool> DisableSectionAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsync($"{BaseUrl}/sections/disable/{id}", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("desactivar sección", ex);
            return false;
        }
    }

    public async Task<List<(int Id, string Name)>> GetSectionOptionsAsync()
    {
        var sections = await GetAllSectionsAsync();
        return sections
            .Where(s => s.Status == OrganizationStatus.Active)
            .Select(s => (s.Id, s.Name))
            .ToList();
    }

    #endregion

    #region Section Managers

    public async Task<List<SectionManagerDto>> GetSectionManagersAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BaseUrl}/sections/managers");

            if (!response.IsSuccessStatusCode)
                return new List<SectionManagerDto>();

            var apiResponse = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<SectionManagerDto>>
            >();

            return apiResponse?.Success == true && apiResponse.Data != null
                ? apiResponse.Data
                : new List<SectionManagerDto>();
        }
        catch (Exception ex)
        {
            LogError("obtener managers", ex);
            return new List<SectionManagerDto>();
        }
    }

    #endregion

    #region Departments

    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"{BaseUrl}/departments");

            if (!response.IsSuccessStatusCode)
                return new List<Department>();

            var apiResponse = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<DepartmentDto>>
            >();

            if (apiResponse?.Success != true || apiResponse.Data == null)
                return new List<Department>();

            // Cargar nombres de secciones para mostrar
            var sections = await GetAllSectionsAsync();
            var sectionNames = sections.ToDictionary(s => s.Id, s => s.Name);

            return apiResponse.Data.Select(dto => MapToDepartment(dto, sectionNames)).ToList();
        }
        catch (Exception ex)
        {
            LogError("obtener departamentos", ex);
            return new List<Department>();
        }
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        var departments = await GetAllDepartmentsAsync();
        return departments.FirstOrDefault(d => d.Id == id);
    }

    public async Task<bool> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();

            var dto = new DepartmentRequestDto
            {
                DepartmentId = 0,
                SectionId = request.SectionId,
                Name = request.Name,
            };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/departments", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("crear departamento", ex);
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
                DepartmentId = request.Id,
                SectionId = request.SectionId,
                Name = request.Name,
            };

            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/departments", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("actualizar departamento", ex);
            return false;
        }
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/departments/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("eliminar departamento", ex);
            return false;
        }
    }

    public async Task<bool> DisableDepartmentAsync(int id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsync($"{BaseUrl}/departments/disable/{id}", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            LogError("desactivar departamento", ex);
            return false;
        }
    }

    #endregion

    #region Private Helpers

    private async Task EnsureAuthenticatedAsync()
    {
        await _authService.GetCurrentUserAsync();
    }

    private static Section MapToSection(SectionDto dto) =>
        new()
        {
            Id = dto.SectionId,
            Name = dto.Name,
            SectionManagerId = dto.SectionManagerId,
            SectionManagerFullName = dto.SectionManagerFullName ?? string.Empty,
            Status = OrganizationStatus.Active,
        };

    private static Department MapToDepartment(
        DepartmentDto dto,
        Dictionary<int, string> sectionNames
    ) =>
        new()
        {
            Id = dto.DepartmentId,
            Name = dto.Name,
            SectionId = dto.SectionId,
            SectionName = sectionNames.GetValueOrDefault(dto.SectionId, string.Empty),
            Status = OrganizationStatus.Active,
        };

    private static void LogError(string operation, Exception ex)
    {
        Console.WriteLine($"Error al {operation}: {ex.Message}");
    }

    #endregion
}
