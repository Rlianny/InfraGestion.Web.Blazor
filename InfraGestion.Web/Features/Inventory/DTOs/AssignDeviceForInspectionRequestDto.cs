namespace InfraGestion.Web.Features.Inventory.DTOs;


public class AssignDeviceForInspectionRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
}