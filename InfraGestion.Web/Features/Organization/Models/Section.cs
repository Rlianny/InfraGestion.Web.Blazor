namespace InfraGestion.Web.Features.Organization.Models;

/// <summary>
/// Sección: División principal de la empresa. Tiene un Responsable de Sección.
/// Contiene múltiples Departamentos.
/// </summary>
public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty; // Responsable de Sección
    public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<Department> Departments { get; set; } = new();
}
