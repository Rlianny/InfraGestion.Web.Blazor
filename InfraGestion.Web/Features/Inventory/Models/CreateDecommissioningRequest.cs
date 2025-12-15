using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Request model for creating a decommissioning request
/// </summary>
public class CreateDecommissioningRequest
{
    [Required(ErrorMessage = "El equipo es requerido")]
    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [Required(ErrorMessage = "El motivo es requerido")]
    [JsonPropertyName("reason")]
    public DecommissioningReason Reason { get; set; }

    [JsonPropertyName("requestDate")]
    public DateTime RequestDate { get; set; } = DateTime.Now;
}
