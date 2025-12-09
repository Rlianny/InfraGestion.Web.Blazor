using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class DeviceDto
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType DeviceType { get; set; } 
    public OperationalState OperationalState { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}

/// <summary>
/// DTO alternativo para deserialización cuando los enums vienen como int
/// </summary>
public class DeviceDtoRaw
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DeviceType { get; set; } 
    public int OperationalState { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public DeviceDto ToDeviceDto()
    {
        return new DeviceDto
        {
            DeviceId = DeviceId,
            Name = Name,
            DeviceType = (DeviceType)DeviceType,
            OperationalState = (OperationalState)OperationalState,
            DepartmentName = DepartmentName
        };
    }
}