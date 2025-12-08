namespace InfraGestion.Web.Features.Organization.DTOs;

/// <summary>
/// DTO para enviar datos de Sección al API (POST/PUT/DELETE)
/// </summary>
public class SectionRequestDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SectionManager { get; set; } = string.Empty;
}

/// <summary>
/// DTO para enviar datos de Departamento al API (POST/PUT/DELETE)
/// </summary>
public class DepartmentRequestDto
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// DTO para asignar responsable de sección
/// </summary>
public class AssignSectionResponsibleRequestDto
{
    public int UserId { get; set; }
    public int SectionId { get; set; }
}
