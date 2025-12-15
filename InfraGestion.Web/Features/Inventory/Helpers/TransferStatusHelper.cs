namespace InfraGestion.Web.Features.Inventory.Helpers;

using InfraGestion.Web.Features.Inventory.Models;

public static class TransferStatusHelper
{
    public static string GetStatusDisplayName(TransferStatus status)
    {
        return status switch
        {
            TransferStatus.Pending => "Pendiente",
            TransferStatus.InTransit => "En Tránsito",
            TransferStatus.Completed => "Completado",
            TransferStatus.Cancelled => "Cancelado",
            _ => status.ToString()
        };
    }

    public static string GetStatusBadgeClass(TransferStatus status)
    {
        return status switch
        {
            TransferStatus.Pending => "status-pending",
            TransferStatus.InTransit => "status-in-transit",
            TransferStatus.Completed => "status-completed",
            TransferStatus.Cancelled => "status-cancelled",
            _ => "status-default"
        };
    }
}
