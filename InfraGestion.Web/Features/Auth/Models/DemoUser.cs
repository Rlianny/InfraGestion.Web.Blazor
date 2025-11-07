namespace InfraGestion.Web.Features.Auth.Models;

public class DemoUser
{
    public string Role { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}