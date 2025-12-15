namespace InfraGestion.Web.Features.Inventory.Models;

public class PendingInspection
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string FormatedDeviceId => $"DEV{DeviceId:D3}";

    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public DateTime EmissionDate { get; set; }
    public string RequesterName { get; set; } = string.Empty;
}
