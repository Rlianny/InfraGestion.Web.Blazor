using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para valoración/calificación - coincide con backend RateDto
/// </summary>
public class RateDto
{
    [JsonPropertyName("rateId")]
    public int RateId { get; set; }

    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("giverId")]
    public int GiverId { get; set; }

    [JsonPropertyName("giverName")]
    public string GiverName { get; set; } = string.Empty;

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
