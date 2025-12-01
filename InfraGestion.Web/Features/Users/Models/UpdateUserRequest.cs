using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Users.Models;

public class UpdateUserRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    public UserRole Role { get; set; }

    [Required(ErrorMessage = "El departamento es requerido")]
    public string Department { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string? Password { get; set; }

    [Range(0, 50, ErrorMessage = "Los años de experiencia deben estar entre 0 y 50")]
    public int? YearsOfExperience { get; set; }

    public string? Specialty { get; set; }
}