using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para solicitud de baja - coincide con backend DecommissioningRequestDto
/// </summary>
public class DecommissioningRequestDto
{
    [JsonPropertyName("decommissioningRequestId")]
    public int DecommissioningRequestId { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public int ReasonId { get; set; }

    [JsonPropertyName("reasonDescription")]
    public string ReasonDescription { get; set; } = string.Empty;

    [JsonPropertyName("requestDate")]
    public DateTime RequestDate { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("deviceReceiverId")]
    public int DeviceReceiverId { get; set; }

    [JsonPropertyName("deviceReceiverName")]
    public string DeviceReceiverName { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    public string GetStatusName()
    {
        return Status switch
        {
            0 => "Pendiente",
            1 => "Aceptado",
            2 => "Rechazado",
            _ => "Desconocido"
        };
    }

}
