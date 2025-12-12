using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;


public class ReceivingInspectionRequestDto
{
    public int RequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public int DeviceId { get; set; }
    public int UserId { get; set; }
    public int TechnicianId { get; set; }
    public RequestStatus Status { get; set; }
    public DecommissioningReason? RejectReason { get; set; }
}
