using InfraGestion.Web.Features.Users.Models;

namespace InfraGestion.Web.Features.Users.Helpers;

public static class UserRoleHelper
{
    public static string GetRoleBadgeClass(UserRole role)
    {
        return role switch
        {
            UserRole.Director => "badge-director",
            UserRole.Administrator => "badge-administrador",
            UserRole.SectionManager => "badge-responsable",
            UserRole.Technician => "badge-tecnico",
            UserRole.Logistician => "badge-logistica",
            _ => "badge-default"
        };
    }

    public static string GetRoleDisplayName(UserRole role)
    {
        return role switch
        {
            UserRole.Director => "Director",
            UserRole.Administrator => "Administrador",
            UserRole.SectionManager => "Responsable de Sección",
            UserRole.Technician => "Técnico",
            UserRole.Logistician => "Logístico",
            _ => role.ToString()
        };
    }
}