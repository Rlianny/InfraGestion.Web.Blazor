using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.Helpers;

public static class DecommissioningStatusHelper
{
    public static string GetStatusDisplayName(DecommissioningStatus status)
    {
        return status switch
        {
            DecommissioningStatus.Pending => "Pendiente",
            DecommissioningStatus.Accepted => "Aceptada",
            DecommissioningStatus.Rejected => "Rechazada",
            _ => status.ToString()
        };
    }

    public static string GetStatusBadgeClass(DecommissioningStatus status)
    {
        return status switch
        {
            DecommissioningStatus.Pending => "status-pending",
            DecommissioningStatus.Accepted => "status-accepted",
            DecommissioningStatus.Rejected => "status-rejected",
            _ => "status-default"
        };
    }
}
