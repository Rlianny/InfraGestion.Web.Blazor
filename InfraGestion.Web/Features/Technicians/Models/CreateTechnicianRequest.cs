using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Request para crear un nuevo técnico
/// </summary>
public class CreateTechnicianRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es requerida")]
    public string Specialty { get; set; } = string.Empty;

    [Required(ErrorMessage = "La sección es requerida")]
    public string Section { get; set; } = string.Empty;

    public string PhotoUrl { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string? Phone { get; set; }

    public DateTime HireDate { get; set; } = DateTime.Now;
}
