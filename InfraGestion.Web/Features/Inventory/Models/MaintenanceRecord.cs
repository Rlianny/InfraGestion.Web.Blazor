namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Represents a maintenance record for a device
/// </summary>
public class MaintenanceRecord
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public MaintenanceType Type { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
}