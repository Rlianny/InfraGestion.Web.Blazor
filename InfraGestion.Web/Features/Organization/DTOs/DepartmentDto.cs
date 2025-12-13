namespace InfraGestion.Web.Features.Organization.DTOs;

using System.Text.Json.Serialization;

public class DepartmentDto
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
    // API returns the property as 'departmentName' — map it explicitly
    [JsonPropertyName("departmentName")]
    public string Name { get; set; } = string.Empty;
}
