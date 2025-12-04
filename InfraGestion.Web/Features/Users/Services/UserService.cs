using System.Net.Http.Json;
using InfraGestion.Web.Features.Users.Models;
using InfraGestion.Web.Features.Users.DTOs;
using InfraGestion.Web.Features.Auth.DTOs;

namespace InfraGestion.Web.Features.Users.Services;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get users from API
    /// </summary>
    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            // GET /Users
            var response = await _httpClient.GetAsync("Users");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error HTTP {response.StatusCode} al obtener usuarios");
                return new List<User>();
            }
            
            // Deserialize ApiResponse<IEnumerable<UserDto>>
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDto>>>();
            
            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return apiResponse.Data.Select(MapDtoToUser).ToList();
            }
            
            return new List<User>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de red al obtener usuarios: {ex.Message}");
            return new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado al obtener usuarios: {ex.Message}");
            return new List<User>();
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            // GET /Users/{id}
            var response = await _httpClient.GetAsync($"Users/{id}");
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            
            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDtoToUser(apiResponse.Data);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener usuario {id}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get user by name
    /// </summary>
    public async Task<User?> GetUserByNameAsync(string name)
    {
        try
        {
            // Get all users and filter by name
            var allUsers = await GetAllUsersAsync();
            return allUsers.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener usuario por nombre {name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Search user with filters
    /// NOTE: the actual API has not search endpoint
    /// </summary>
    public async Task<List<User>> SearchUsersAsync(string searchTerm, UserRole? roleFilter, UserStatus? statusFilter)
    {
        try
        {
            // Get users
            var allUsers = await GetAllUsersAsync();
            
            // Filetr in client (because API has not search endpoint)
            var query = allUsers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.Id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Department.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (roleFilter.HasValue)
            {
                query = query.Where(u => u.Role == roleFilter.Value);
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(u => u.Status == statusFilter.Value);
            }

            return query.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en búsqueda: {ex.Message}");
            return new List<User>();
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    public async Task<User?> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            // Convert CreateUserRequest to CreateUserRequestDto
            var dto = new CreateUserRequestDto
            {
                Username = GenerateUsername(request.Name), // Generate username from name
                FullName = request.Name,
                Password = request.Password,
                Role = MapRoleToString(request.Role),
                DepartmentName = request.Department,
                YearsOfExperience = null,
                Specialty = null
            };

            // POST /Users
            var response = await _httpClient.PostAsJsonAsync("Users", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear usuario: {error}");
                return null;
            }
            
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            
            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDtoToUser(apiResponse.Data);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear usuario: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Update an existent user
    /// </summary>
    public async Task<User?> UpdateUserAsync(UpdateUserRequest request)
    {
        try
        {
            var dto = new UpdateUserRequestDto
            {
                UserId = request.Id,
                FullName = request.Name,
                Role = MapRoleToString(request.Role),
                DepartmentName = request.Department,
                IsActive = null,
                YearsOfExperience = request.YearsOfExperience,
                Specialty = request.Specialty
            };

            // PUT /Users/{id}
            var response = await _httpClient.PutAsJsonAsync($"Users/{request.Id}", dto);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            
            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return MapDtoToUser(apiResponse.Data);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Disable user (la API usa desactivación)
    /// </summary>
    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            // Disable
            var dto = new
            {
                UserId = id,
                Reason = "Eliminado desde la interfaz de usuario"
            };

            // POST /Users/{id}/deactivate
            var response = await _httpClient.PostAsJsonAsync($"Users/{id}/deactivate", dto);
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Enable/Disable user
    /// </summary>
    public async Task<bool> ToggleUserStatusAsync(int id)
    {
        try
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return false;

            HttpResponseMessage response;
            
            if (user.Status == UserStatus.Active)
            {
                var dto = new
                {
                    UserId = id,
                    Reason = "Desactivado manualmente"
                };
                response = await _httpClient.PostAsJsonAsync($"Users/{id}/deactivate", dto);
            }
            else
            {
                response = await _httpClient.PostAsync($"Users/{id}/activate", null);
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cambiar estado del usuario: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Get users by department
    /// </summary>
    public async Task<List<User>> GetUsersByDepartmentAsync(int departmentId)
    {
        try
        {
            // GET /Users/department/{id}
            var response = await _httpClient.GetAsync($"Users/department/{departmentId}");
            
            if (!response.IsSuccessStatusCode)
                return new List<User>();
            
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDto>>>();
            
            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                return apiResponse.Data.Select(MapDtoToUser).ToList();
            }
            
            return new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener usuarios del departamento: {ex.Message}");
            return new List<User>();
        }
    }

    // === Mapping methods ===

    /// <summary>
    /// Convert UserDto from API a User from UI
    /// </summary>
    private User MapDtoToUser(UserDto dto)
    {
        return new User
        {
            Id = dto.UserId,
            Name = dto.FullName,
            Department = dto.DepartmentName,
            Role = MapStringToRole(dto.Role),
            Status = dto.IsActive ? UserStatus.Active : UserStatus.Inactive,
            CreatedAt = dto.CreatedAt,
            YearsOfExperience = dto.YearsOfExperience,
            Specialty = dto.Specialty
        };
    }

    /// <summary>
    /// Convert UserRole (enum) to string for API
    /// </summary>
    private string MapRoleToString(UserRole role)
    {
        return role switch
        {
            UserRole.Administrator => "Administrator",
            UserRole.Director => "Director",
            UserRole.SectionManager => "SectionManager",
            UserRole.Technician => "Technician",
            UserRole.Logistician => "Logistician",
            _ => "Technician"
        };
    }

    /// <summary>
    /// Convert string from API to UserRole (enum)
    /// </summary>
    private UserRole MapStringToRole(string role)
    {
        return role switch
        {
            "Administrator" => UserRole.Administrator,
            "Director" => UserRole.Director,
            "SectionManager" => UserRole.SectionManager,
            "Technician" => UserRole.Technician,
            "Logistician" => UserRole.Logistician,
            _ => UserRole.Technician
        };
    }

    /// <summary>
    /// Generate a username from full name
    /// Example: "Elena Morales" → "elena.morales"
    /// </summary>
    private string GenerateUsername(string fullName)
    {
        return fullName.ToLower()
            .Replace(" ", "_")
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u");
    }
}