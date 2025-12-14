namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// DTO for decommissioning request from API
/// Maps to: GET /decommissioning/requests
/// </summary>
public class DecommissioningRequestDto
{
    public int DecommissioningRequestId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public int Status { get; set; } // DecommissioningStatus as int
    public string Justification { get; set; } = string.Empty;
    public int Reason { get; set; } // DecommissioningReason as int
    public string ReasonDescription { get; set; } = string.Empty;
    public DateTime? ReviewedDate { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? ReviewedByUserName { get; set; }
    public int? ReceiverUserId { get; set; }
    public string? ReceiverUserName { get; set; }
    public int? FinalDestinationId { get; set; }
    public string? FinalDestinationName { get; set; }
}
