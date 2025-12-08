namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Status of a device transfer between sections
/// </summary>
public enum TransferStatus
{
    Pending,        // Pendiente
    InTransit,      // En tránsito
    Completed,      // Completado
    Cancelled       // Cancelado
}
