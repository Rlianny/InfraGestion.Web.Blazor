using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Organization.Models;

/// <summary>
/// Request para actualizar una Sección (división principal de la empresa)
/// </summary>
public class UpdateSectionRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la sección es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El responsable de sección es requerido")]
    [StringLength(150, ErrorMessage = "El nombre del responsable no puede exceder 150 caracteres")]
    public string ManagerName { get; set; } = string.Empty;

    public OrganizationStatus Status { get; set; }
}
