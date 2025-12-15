using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Model for adding a penalty to a technician - matches backend PenaltyRequest
/// </summary>
public class PenaltyRequest
{
    [JsonPropertyName("technicianId")]
    public int? TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("superiorId")]
    public int? SuperiorId { get; set; }

    [JsonPropertyName("superiorUsername")]
    public string SuperiorUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El monto es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; } = 0;
}
