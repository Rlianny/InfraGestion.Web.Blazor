using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para registrar penalización - coincide con backend PenaltyRequest
/// </summary>
public class PenaltyRequestDto
{
    [JsonPropertyName("technicianId")]
    public int? TechnicianId { get; set; }

    [JsonPropertyName("technicianName")]
    public string? TechnicianName { get; set; }

    [JsonPropertyName("superiorId")]
    public int? SuperiorId { get; set; }

    [JsonPropertyName("superiorUsername")]
    public string? SuperiorUsername { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
