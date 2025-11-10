namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Request to approve a device
/// POST /inventory/approbals
/// </summary>
public class AcceptDeviceRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
}