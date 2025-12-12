namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Lightweight device model for list views
/// Contains only essential information for device listings
/// For full device details, use DeviceDetails instead
/// </summary>
public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public OperationalState State { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public string Location { get; set; } = string.Empty;
}