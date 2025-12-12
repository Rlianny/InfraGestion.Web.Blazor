namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Common status for requests (inspections, decommissioning, etc.)
/// </summary>
public enum RequestStatus
{
    Pending,
    Approved,
    Rejected,
}
