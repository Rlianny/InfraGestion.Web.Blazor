using InfraGestion.Web.Features.Inventory.Models;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;


public class InspectionDecisionDto
{
    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("isApproved")]
    public bool IsApproved { get; set; }

    [JsonPropertyName("reason")]
    public DecommissioningReason? Reason { get; set; }
}
