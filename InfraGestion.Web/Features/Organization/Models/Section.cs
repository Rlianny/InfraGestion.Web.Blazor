namespace InfraGestion.Web.Features.Organization.Models;

/// <summary>
/// Section: Main division of the company. Has a Section Manager.
/// Contains multiple Departments.
/// </summary>
public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? SectionManagerId { get; set; }
    public string SectionManagerFullName { get; set; } = string.Empty;
    public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<Department> Departments { get; set; } = new();
}
