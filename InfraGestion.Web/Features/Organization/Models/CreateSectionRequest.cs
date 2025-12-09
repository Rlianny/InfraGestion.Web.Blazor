using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Organization.Models;


public class CreateSectionRequest
{
    [Required(ErrorMessage = "El nombre de la sección es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "El username no puede exceder 150 caracteres")]
    public string? SectionManager { get; set; }
}
