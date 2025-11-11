namespace InfraGestion.Web.Features.Inventory.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public OperationalState State { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public string Location { get; set; } = string.Empty;
}