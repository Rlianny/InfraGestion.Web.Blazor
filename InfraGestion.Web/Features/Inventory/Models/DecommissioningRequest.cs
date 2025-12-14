namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Model for decommissioning request in the UI
/// Used for admin management of technical decommissioning requests
/// </summary>
public class DecommissioningRequest
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DecommissioningStatus Status { get; set; }
    public string Justification { get; set; } = string.Empty;
    public DecommissioningReason Reason { get; set; }
    public string ReasonDescription { get; set; } = string.Empty;
    public DateTime? ReviewedDate { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? ReviewedByUserName { get; set; }
    
    // Additional properties for display/forms
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime? DecommissioningDate { get; set; }
    public string? FinalDestination { get; set; }
}
