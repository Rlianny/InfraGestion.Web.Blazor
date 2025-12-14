namespace InfraGestion.Web.Features.Organization.DTOs;

using System.Text.Json.Serialization;

public class SectionDto
{
    [JsonPropertyName("sectionId")]
    public int SectionId { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("sectionManagerId")]
    public int? SectionManagerId { get; set; }
    
    [JsonPropertyName("sectionManagerFullName")]
    public string? SectionManagerFullName { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
