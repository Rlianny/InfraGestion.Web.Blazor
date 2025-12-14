namespace InfraGestion.Web.Features.Organization.DTOs;

using System.Text.Json.Serialization;


public class DepartmentDto
{
    [JsonPropertyName("sectionId")]
    public int SectionId { get; set; }
    
    [JsonPropertyName("departmentId")]
    public int DepartmentId { get; set; }
    
    [JsonPropertyName("departmentName")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("sectionName")]
    public string SectionName { get; set; } = string.Empty;
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
