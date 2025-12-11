using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para detalle de técnico - coincide con backend TechnicianDetailDto
/// </summary>
public class TechnicianDetailDto
{
    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("yearsOfExperience")]
    public int YearsOfExperience { get; set; }

    [JsonPropertyName("specialty")]
    public string Specialty { get; set; } = string.Empty;

    [JsonPropertyName("maintenanceRecords")]
    public List<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new();

    [JsonPropertyName("decommissioningRequests")]
    public List<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new();
}
