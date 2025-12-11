using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para penalización - coincide con backend PenaltyDto
/// </summary>
public class PenaltyDto
{
    [JsonPropertyName("penaltyId")]
    public int PenaltyId { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("amount")]
    public double Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
