using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Request to update an existing device
/// PUT /api/devices/{id}
/// v2.0: ID now in route, DeviceId in body is kept for compatibility
/// </summary>
public class UpdateDeviceRequestDto
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType DeviceType { get; set; }
    public DateTime Date { get; set; }
    public OperationalState OperationalState { get; set; }
    public string? DepartmentName { get; set; }
}