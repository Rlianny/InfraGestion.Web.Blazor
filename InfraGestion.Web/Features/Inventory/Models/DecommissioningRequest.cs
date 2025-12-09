namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Represents decommissioning info for a device (matches DecommissioningDto from backend)
/// </summary>
public class DecommissioningRequest
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public int ReceiverDepartmentId { get; set; }
    public string ReceiverDepartmentName { get; set; } = string.Empty;
    public DateTime DecommissioningDate { get; set; }
    public DecommissioningReason Reason { get; set; }
    public string? FinalDestination { get; set; }
}
