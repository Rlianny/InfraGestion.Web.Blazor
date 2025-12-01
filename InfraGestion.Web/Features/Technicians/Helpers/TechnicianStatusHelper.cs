using InfraGestion.Web.Features.Technicians.Models;

namespace InfraGestion.Web.Features.Technicians.Helpers;

/// <summary>
/// Helper para obtener información de display de los estados del técnico
/// </summary>
public static class TechnicianStatusHelper
{
    public static string GetStatusDisplayName(TechnicianStatus status)
    {
        return status switch
        {
            TechnicianStatus.Active => "Activo",
            TechnicianStatus.Inactive => "Inactivo",
            TechnicianStatus.OnLeave => "De Permiso",
            _ => "Desconocido"
        };
    }

    public static string GetStatusBadgeClass(TechnicianStatus status)
    {
        return status switch
        {
            TechnicianStatus.Active => "status-active",
            TechnicianStatus.Inactive => "status-inactive",
            TechnicianStatus.OnLeave => "status-onleave",
            _ => "status-inactive"
        };
    }

    public static string GetProposalStatusClass(string status)
    {
        return status switch
        {
            "Aprobado" => "proposal-approved",
            "Pendiente" => "proposal-pending",
            "Rechazado" => "proposal-rejected",
            _ => "proposal-pending"
        };
    }
}
