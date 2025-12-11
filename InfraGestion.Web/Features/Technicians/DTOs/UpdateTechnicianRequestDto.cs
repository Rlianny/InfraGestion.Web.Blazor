using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para actualizar técnico - coincide con backend UpdateTechnicianRequest
/// </summary>
public class UpdateTechnicianRequestDto
{
    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("specialty")]
    public string? Specialty { get; set; }

    [JsonPropertyName("yearsOfExperience")]
    public int? YearsOfExperience { get; set; }

    [JsonPropertyName("departmentId")]
    public int? DepartmentId { get; set; }
}
