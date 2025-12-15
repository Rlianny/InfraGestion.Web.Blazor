using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class DecommissioningRequestDto
{
    [JsonPropertyName("decommissioningRequestId")]
    public int DecommissioningRequestId { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("requestDate")]
    public DateTime RequestDate { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("justification")]
    public string Justification { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public int Reason { get; set; }

    [JsonPropertyName("reasonDescription")]
    public string ReasonDescription { get; set; } = string.Empty;

    [JsonPropertyName("reviewedDate")]
    public DateTime? ReviewedDate { get; set; }

    [JsonPropertyName("reviewedByUserId")]
    public int? ReviewedByUserId { get; set; }

    [JsonPropertyName("reviewedByUserName")]
    public string? ReviewedByUserName { get; set; }

    [JsonPropertyName("receiverUserId")]
    public int? ReceiverUserId { get; set; }

    [JsonPropertyName("receiverUserName")]
    public string? ReceiverUserName { get; set; }

    [JsonPropertyName("finalDestinationId")]
    public int? FinalDestinationId { get; set; }

    [JsonPropertyName("finalDestinationName")]
    public string? FinalDestinationName { get; set; }
}
