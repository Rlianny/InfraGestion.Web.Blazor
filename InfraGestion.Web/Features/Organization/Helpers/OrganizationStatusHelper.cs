using InfraGestion.Web.Features.Organization.Models;

namespace InfraGestion.Web.Features.Organization.Helpers;

public static class OrganizationStatusHelper
{
    public static string GetStatusDisplayName(OrganizationStatus status)
    {
        return status switch
        {
            OrganizationStatus.Active => "Activo",
            OrganizationStatus.Inactive => "Inactivo",
            _ => status.ToString()
        };
    }

    public static string GetStatusBadgeClass(OrganizationStatus status)
    {
        return status switch
        {
            OrganizationStatus.Active => "status-active",
            OrganizationStatus.Inactive => "status-inactive",
            _ => "status-default"
        };
    }
}
