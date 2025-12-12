using System.Net.Http.Json;
using System.Net.Http.Headers;
using InfraGestion.Web.Features.Auth.Models;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Core.Constants;
using Microsoft.JSInterop;
using System.Text.Json;
using InfraGestion.Web.Core.Models;

namespace InfraGestion.Web.Features.Auth.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TOKEN_KEY = "authToken";
    private const string USER_KEY = "currentUser";
    private UserSession? _currentUser;

    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Init session with real API
    /// </summary>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // Try login with API
            return await LoginWithApiAsync(request);
        }
        catch (HttpRequestException)
        {
            // If API fails
            Console.WriteLine("⚠️ API no disponible");
            return new LoginResponse
            {
                Success = false,
                Message = "API no disponible"
            };
        }
        catch (Exception)
        {
            // If occurs any other error
            Console.WriteLine("⚠️ Error inesperado, usando login demo");
            return new LoginResponse
            {
                Success = false,
                Message = "Error inesperado"
            };
        }
    }

    /// <summary>
    /// Login with real API
    /// </summary>
    private async Task<LoginResponse> LoginWithApiAsync(LoginRequest request)
    {
        var loginDto = new LoginRequestDto
        {
            Username = request.Username,
            Password = request.Password
        };

        var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Credenciales inválidas");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

        if (apiResponse?.Success == true && apiResponse.Data != null)
        {
            var loginData = apiResponse.Data;

            var userSession = new UserSession
            {
                Id = loginData.UserId,
                Username = request.Username,
                FullName = loginData.FullName,
                Role = loginData.Role,
                DepartmentId = loginData.DepartmentId,
                DepartmentName = loginData.DepartmentName
            };

            await SaveTokenAsync(loginData.AccessToken);
            await SaveSessionAsync(userSession);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginData.AccessToken);

            _currentUser = userSession;
            OnAuthStateChanged?.Invoke();

            Console.WriteLine("✅ Login exitoso con API");

            return new LoginResponse
            {
                Success = true,
                User = userSession
            };
        }

        throw new Exception("Respuesta inválida de la API");
    }

    /// <summary>
    /// Login demo
    /// </summary>
    private async Task<LoginResponse> LoginDemoAsync(LoginRequest request)
    {
        var demoUsers = new Dictionary<string, (string password, string fullName, string role, int departmentId, string departmentName)>
        {
            { "admin", ("admin", "Carlos Administrador", "Administrador", 1, "Administración General") },
            { "director", ("director", "Ana Directora", "Director", 1, "Dirección") },
            { "jefe", ("jefe", "Jorge Jefe de Sección", "Jefe de Sección", 2, "Sección Técnica") },
            { "tecnico", ("tecnico", "Elena Técnica", "Técnico", 3, "Departamento de Mantenimiento") },
            { "logistica", ("logistica", "Luis Logística", "Logístico", 4, "Logística") }
        };

        if (demoUsers.TryGetValue(request.Username.ToLower(), out var userData))
        {
            if (request.Password == userData.password)
            {
                var userSession = new UserSession
                {
                    Id = Array.IndexOf(demoUsers.Keys.ToArray(), request.Username.ToLower()) + 1,
                    Username = request.Username,
                    FullName = userData.fullName,
                    Role = userData.role,
                    DepartmentId = userData.departmentId,
                    DepartmentName = userData.departmentName
                };

                var demoToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{request.Username}:{DateTime.UtcNow.Ticks}"));

                await SaveTokenAsync(demoToken);
                await SaveSessionAsync(userSession);

                _currentUser = userSession;
                OnAuthStateChanged?.Invoke();

                Console.WriteLine("✅ Login exitoso con DEMO");

                return new LoginResponse
                {
                    Success = true,
                    User = userSession
                };
            }
        }

        return new LoginResponse
        {
            Success = false,
            Message = "Usuario o contraseña incorrectos"
        };
    }

    /// <summary>
    /// Close session
    /// POST /auth/logout/{userId}
    /// </summary>
    public async Task LogoutAsync()
    {
        try
        {
            // Get current user ID before clearing session
            var userId = _currentUser?.Id ?? 0;
            
            if (userId > 0)
            {
                // API call for invalidate token with userId in path
                await _httpClient.PostAsync(ApiRoutes.Auth.Logout(userId), null);
            }
        }
        catch
        {
            // Continue even when call fail
        }
        finally
        {
            // Clean LocalStorage
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TOKEN_KEY);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", USER_KEY);

            // Clean Auth header
            _httpClient.DefaultRequestHeaders.Authorization = null;

            _currentUser = null;
            OnAuthStateChanged?.Invoke();
        }
    }

    /// <summary>
    /// Get user from sessionStorage
    /// </summary>
    public async Task<UserSession?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", USER_KEY);

            if (string.IsNullOrEmpty(json))
                return null;

            _currentUser = JsonSerializer.Deserialize<UserSession>(json);

            // Restore token if exists
            if (_currentUser != null)
            {
                var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", TOKEN_KEY);
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return _currentUser;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Verify if exists a loged user
    /// </summary>
    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }

    /// <summary>
    /// Change current user password
    /// </summary>
    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return false;

            var request = new
            {
                UserId = user.Id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            var response = await _httpClient.PostAsJsonAsync("auth/change-password", request);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Refresh token authentication
    /// </summary>
    public async Task<bool> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var request = new { RefreshToken = refreshToken };

            var response = await _httpClient.PostAsJsonAsync("auth/refresh-token", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                await SaveTokenAsync(apiResponse.Data.AccessToken);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiResponse.Data.AccessToken);

                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Demo Login
    /// NOTE: This method might not work if API refused
    /// </summary>
    public async Task<LoginResponse> LoginAsDemoAsync(string role)
    {
        // If API has a demo endpoint, use it here
        // For now, return that it is not available
        return new LoginResponse
        {
            Success = false,
            Message = "Login de demostración no disponible en la API"
        };
    }

    private async Task SaveTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TOKEN_KEY, token);
    }

    private async Task SaveSessionAsync(UserSession user)
    {
        var json = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", USER_KEY, json);
    }
}