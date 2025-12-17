using System.Text.Json.Serialization;
using InfraGestion.Web.Features.Inventory.Helpers;
using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO for decommissioning request - matches backend DecommissioningRequestDto
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

    [JsonPropertyName("receiverUserId")]
    public int? ReceiverUserId { get; set; }

    [JsonPropertyName("receiverUserName")]
    public string? ReceiverUsername { get; set; }

    [JsonPropertyName("reviewedDate")]
    public DateTime? ReviewedDate { get; set; }

    [JsonPropertyName("reviewedByUserId")]
    public int? ReviewedByUserId { get; set; }

    [JsonPropertyName("reviewedByUserName")]
    public string? ReviewedByUserName { get; set; }

    [JsonPropertyName("finalDestinationId")]
    public int? FinalDestinationId { get; set; }

    [JsonPropertyName("finalDestinationName")]
    public string? FinalDestinationName { get; set; }

    [JsonPropertyName("justification")]
    public string? Justification { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    public string GetStatusName()
    {
        return Status switch
        {
            2 => "Pendiente",
            0 => "Aceptado",
            1 => "Rechazado",
            _ => "Desconocido"
        };
    }

    public string GetReasonName()
    {
        var reason = (DecommissioningReason)ReasonId;
        return DecommissioningReasonHelper.GetReasonDisplayName(reason);
    }
}