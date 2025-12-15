using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Request model for creating a maintenance record
/// </summary>
public class CreateMaintenanceRequest
{
    [Required(ErrorMessage = "El equipo es requerido")]
    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("maintenanceDate")]
    public DateTime MaintenanceDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El tipo de mantenimiento es requerido")]
    [JsonPropertyName("maintenanceType")]
    public MaintenanceType MaintenanceType { get; set; } = MaintenanceType.Preventive;

    [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
    [JsonPropertyName("cost")]
    public double Cost { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
