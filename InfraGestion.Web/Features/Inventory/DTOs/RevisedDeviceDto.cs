namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// DTO for revised devices pending registration completion
/// Used by admin dashboard to show devices that have passed inspection
/// </summary>
public class RevisedDeviceDto
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DeviceType { get; set; }
    public int OperationalState { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}
