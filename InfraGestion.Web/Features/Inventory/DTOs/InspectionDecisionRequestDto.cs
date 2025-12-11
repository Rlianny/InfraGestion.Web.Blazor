using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// DTO for sending inspection decision to backend
/// Maps to: POST /inventory/inspection-decision
/// </summary>
public class InspectionDecisionRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public bool IsApproved { get; set; }
    public DecommissioningReason Reason { get; set; }
}
