namespace InfraGestion.Web.Features.Organization.DTOs;

public class SectionRequestDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? SectionManagerId { get; set; }
}
