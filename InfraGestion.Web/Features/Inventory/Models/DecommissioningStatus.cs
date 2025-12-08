namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Status of a decommissioning request
/// </summary>
public enum DecommissioningStatus
{
    Pending,        // Pendiente
    Approved,       // Aprobado
    Rejected        // Rechazado
}
