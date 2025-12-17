using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Transfers.DTOs;

/// <summary>
/// DTO to initiate a transfer request
/// Matches backend Application.DTOs.Transfer.CreateTransferRequestDto
/// </summary>
public class InitiateTransferDto
{
    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("destinationSectionName")]
    public string DestinationSectionName { get; set; } = string.Empty;

    [JsonPropertyName("deviceReceiverUsername")]
    public string DeviceReceiverUsername { get; set; } = string.Empty;

    [JsonPropertyName("transferDate")]
    public DateTime TransferDate { get; set; } = DateTime.Now;
}
