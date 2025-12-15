using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class TransferDto
{
    [JsonPropertyName("transferId")]
    public int TransferId { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("transferDate")]
    public DateTime TransferDate { get; set; }

    [JsonPropertyName("sourceSectionId")]
    public int SourceSectionId { get; set; }

    [JsonPropertyName("sourceSectionName")]
    public string SourceSectionName { get; set; } = string.Empty;

    [JsonPropertyName("destinationSectionId")]
    public int DestinationSectionId { get; set; }

    [JsonPropertyName("destinationSectionName")]
    public string DestinationSectionName { get; set; } = string.Empty;

    [JsonPropertyName("deviceReceiverId")]
    public int DeviceReceiverId { get; set; }

    [JsonPropertyName("deviceReceiverName")]
    public string DeviceReceiverName { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }
}
