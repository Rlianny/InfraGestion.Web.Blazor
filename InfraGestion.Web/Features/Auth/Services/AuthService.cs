using InfraGestion.Web.Core.Models;
using InfraGestion.Web.Features.Auth.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace InfraGestion.Web.Features.Auth.Services;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    private const string SessionKey = "userSession";
    private UserSession? _currentUser;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        // Admin mock validation
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var user = new UserSession
            {
                Id = 1,
                Username = "admin",
                FullName = "Administrador del Sistema",
                Role = "Administrador"
            };

            await SaveSessionAsync(user);
            _currentUser = user;

            return new LoginResponse
            {
                Success = true,
                User = user
            };
        }

        return new LoginResponse
        {
            Success = false,
            Message = "Usuario o contraseña incorrectos"
        };
    }

    public async Task<LoginResponse> LoginAsDemoAsync(string role)
    {
        if (role == "Administrador")
        {
            var user = new UserSession
            {
                Id = 99,
                Username = "admin.demo",
                FullName = "Ana García",
                Role = "Administrador"
            };

            await SaveSessionAsync(user);
            _currentUser = user;

            return new LoginResponse
            {
                Success = true,
                User = user
            };
        }

        return new LoginResponse
        {
            Success = false,
            Message = "Esta demostración estará disponible próximamente"
        };
    }

    public async Task<UserSession?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", SessionKey);
        
        if (string.IsNullOrEmpty(json))
            return null;

        _currentUser = JsonSerializer.Deserialize<UserSession>(json);
        return _currentUser;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", SessionKey);
        _currentUser = null;
    }

    private async Task SaveSessionAsync(UserSession user)
    {
        var json = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", SessionKey, json);
    }
}