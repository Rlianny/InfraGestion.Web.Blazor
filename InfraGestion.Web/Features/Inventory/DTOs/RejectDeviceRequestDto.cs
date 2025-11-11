namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Request to reject a device
/// POST /inventory/rejections
/// </summary>
public class RejectDeviceRequestDto
{
    public int DeviceID { get; set; }
    public int TechnicianID { get; set; }
    public string Reason { get; set; } = string.Empty;
}