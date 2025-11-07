using InfraGestion.Web.Core.Models;

namespace InfraGestion.Web.Features.Auth.Models;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public UserSession? User { get; set; }
}