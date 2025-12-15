using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;


public class UpdateDeviceDto //Update
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType DeviceType { get; set; }
    public DateTime Date { get; set; }
    public OperationalState OperationalState { get; set; }
    public string? DepartmentName { get; set; }
}