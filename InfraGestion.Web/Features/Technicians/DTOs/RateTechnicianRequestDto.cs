using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para calificar técnico - coincide con backend RateTechnicianRequest
/// </summary>
public class RateTechnicianRequestDto
{
    [JsonPropertyName("technicianId")]
    public int? TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string? TechnicianName { get; set; }

    [JsonPropertyName("superiorId")]
    public int? SuperiorId { get; set; }

    [JsonPropertyName("superiorUsername")]
    public string? SuperiorUsername { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;
}
