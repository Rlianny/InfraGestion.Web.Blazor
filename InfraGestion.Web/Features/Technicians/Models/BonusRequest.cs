using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Bonus type enum - matches backend BonusType
/// </summary>
public enum BonusType
{
    Descriptive,
    Monetary
}

/// <summary>
/// Model for adding a bonus to a technician - matches backend BonusRequest
/// </summary>
public class BonusRequest
{
    [JsonPropertyName("technicianId")]
    public int? TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("superiorId")]
    public int? SuperiorId { get; set; }

    [JsonPropertyName("superiorUsername")]
    public string SuperiorUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de bonificación es requerido")]
    [JsonPropertyName("bonusType")]
    public BonusType BonusType { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a 0")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; } = 0;
}
