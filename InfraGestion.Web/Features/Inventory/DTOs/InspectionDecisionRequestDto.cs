using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;


public class InspectionDecisionRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public bool IsApproved { get; set; }
    public DecommissioningReason? Reason { get; set; }
}
