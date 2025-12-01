namespace InfraGestion.Web.Features.Organization.Models;

/// <summary>
/// Departamento: Subdivisión de una Sección.
/// Pertenece a una Sección específica.
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
