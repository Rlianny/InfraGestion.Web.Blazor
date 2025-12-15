using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO for maintenance record - matches backend MaintenanceRecordDto
/// </summary>
public class MaintenanceRecordDto
{
    [JsonPropertyName("maintenanceRecordId")]
    public int MaintenanceRecordId { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("maintenanceDate")]
    public DateTime MaintenanceDate { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("cost")]
    public double Cost { get; set; }

    [JsonPropertyName("maintenanceType")]
    public int MaintenanceType { get; set; }

    public string GetMaintenanceTypeName()
    {
        return MaintenanceType switch
        {
            0 => "Preventivo",
            1 => "Correctivo",
            2 => "Predictivo",
            _ => "Desconocido"
        };
    }
}
