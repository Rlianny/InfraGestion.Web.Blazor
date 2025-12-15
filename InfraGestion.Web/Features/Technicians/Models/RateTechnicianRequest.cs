using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Model for rating a technician - matches backend RateTechnicianRequest
/// </summary>
public class RateTechnicianRequest
{
    [JsonPropertyName("technicianId")]
    public int? TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string TechnicianName { get; set; } = string.Empty;

    [JsonPropertyName("superiorId")]
    public int? SuperiorId { get; set; }

    [JsonPropertyName("superiorUsername")]
    public string SuperiorUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "La puntuación es requerida")]
    [Range(0, 5, ErrorMessage = "La puntuación debe estar entre 0 y 5")]
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;
}
