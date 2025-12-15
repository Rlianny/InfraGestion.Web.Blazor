using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class MaintenanceRecordDto
{
    [JsonPropertyName("maintenanceRecordId")]
    public int MaintenanceRecordId { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("maintenanceDate")]
    public DateTime MaintenanceDate { get; set; }

    [JsonPropertyName("maintenanceType")]
    public int MaintenanceType { get; set; }

    [JsonPropertyName("cost")]
    public double Cost { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
