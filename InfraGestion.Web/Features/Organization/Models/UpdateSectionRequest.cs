using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Organization.Models;

public class UpdateSectionRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la sección es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    public int? SectionManagerId { get; set; }

    public OrganizationStatus Status { get; set; }
}
