using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Request to create a new device
/// POST /api/devices
/// NOTE: Renamed from InsertDeviceRequestDto in v2.1
/// </summary>
public class RegisterNewDeviceDto
{
    public string Name { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    public DeviceType DeviceType { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public int TechnicianId { get; set; }
    public int UserId { get; set; } // Admin/User who creates the request
}

/// <summary>
/// Alias for backward compatibility
/// </summary>
[Obsolete("Use RegisterNewDeviceDto instead")]
public class InsertDeviceRequestDto : RegisterNewDeviceDto { }