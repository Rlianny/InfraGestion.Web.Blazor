namespace InfraGestion.Web.Features.Inventory.DTOs;


public class RejectDeviceRequestDto
{
    public int DeviceID { get; set; }
    public int TechnicianID { get; set; }
    public string Reason { get; set; } = string.Empty;
}