using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Request para actualizar un técnico existente
/// </summary>
public class UpdateTechnicianRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    [Required(ErrorMessage = "El departamento es requerido")]
    public string Section { get; set; } = string.Empty;

    public string PhotoUrl { get; set; } = string.Empty;

    public TechnicianStatus Status { get; set; } = TechnicianStatus.Active;

    [Range(0, 5, ErrorMessage = "La valoración debe estar entre 0 y 5")]
    public decimal Rating { get; set; }

    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string? Password { get; set; }

    [Range(0, 50, ErrorMessage = "Los años de experiencia deben estar entre 0 y 50")]
    public int? YearsOfExperience { get; set; }

    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }
}
