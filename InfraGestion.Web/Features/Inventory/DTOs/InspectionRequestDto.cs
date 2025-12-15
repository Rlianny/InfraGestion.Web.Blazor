using System.Text.Json.Serialization;
using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class InspectionRequestDto
{
    [JsonPropertyName("requestId")]
    public int RequestId { get; set; }

    [JsonPropertyName("requestDate")]
    public DateTime RequestDate { get; set; }

    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("deviceName")]
    public string? DeviceName { get; set; }

    [JsonPropertyName("requesterId")]
    public int RequesterId { get; set; }

    [JsonPropertyName("requesterFullName")]
    public string? RequesterFullName { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("technicianFullName")]
    public string? TechnicianFullName { get; set; }

    [JsonPropertyName("status")]
    public RequestStatus Status { get; set; }

    [JsonPropertyName("rejectReason")]
    public DecommissioningReason? RejectReason { get; set; }
}
