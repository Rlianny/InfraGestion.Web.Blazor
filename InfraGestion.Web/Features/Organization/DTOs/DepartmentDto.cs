namespace InfraGestion.Web.Features.Organization.DTOs;

/// <summary>
/// DTO para recibir datos de Departamento desde el API
/// </summary>
public class DepartmentDto
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
}
