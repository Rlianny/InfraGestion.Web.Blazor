using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO for technician detail - matches backend TechnicianDetailDto
/// </summary>
public class TechnicianDetailDto
{
    [JsonPropertyName("technicianId")]
    public int TechnicianId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("yearsOfExperience")]
    public int YearsOfExperience { get; set; }

    [JsonPropertyName("specialty")]
    public string Specialty { get; set; } = string.Empty;

    [JsonPropertyName("averageRating")]
    public double AverageRating { get; set; }

    [JsonPropertyName("lastInterventionDate")]
    public DateTime? LastInterventionDate { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("sectionName")]
    public string SectionName { get; set; } = string.Empty;

    [JsonPropertyName("sectionManagerName")]
    public string SectionManagerName { get; set; } = string.Empty;

    [JsonPropertyName("ratings")]
    public List<RateDto> Ratings { get; set; } = new();

    [JsonPropertyName("maintenanceRecords")]
    public List<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new();

    [JsonPropertyName("decommissioningRequests")]
    public List<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new();
}
