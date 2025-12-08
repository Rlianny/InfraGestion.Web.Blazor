namespace InfraGestion.Web.Features.Organization.DTOs;

/// <summary>
/// DTO para recibir datos de Sección desde el API
/// </summary>
public class SectionDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SectionManager { get; set; } = string.Empty;
}
