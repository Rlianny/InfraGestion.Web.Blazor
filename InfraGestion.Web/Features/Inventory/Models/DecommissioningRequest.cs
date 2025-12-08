namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Represents a decommissioning request for a device
/// </summary>
public class DecommissioningRequest
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DecommissioningStatus Status { get; set; } = DecommissioningStatus.Pending;
    public string Justification { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime? ReviewedDate { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? ReviewedByUserName { get; set; }
}
