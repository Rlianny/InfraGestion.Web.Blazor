using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Organization.Models;

/// <summary>
/// Request para crear un Departamento (subdivisión de una Sección)
/// </summary>
public class CreateDepartmentRequest
{
    [Required(ErrorMessage = "El nombre del departamento es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    public int SectionId { get; set; }
}
