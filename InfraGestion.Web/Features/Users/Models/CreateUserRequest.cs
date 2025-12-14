using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Users.Models;

public class CreateUserRequest : IValidatableObject
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    public UserRole Role { get; set; }

    [Required(ErrorMessage = "El departamento es requerido")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Range(0, 50, ErrorMessage = "Los años de experiencia deben estar entre 0 y 50")]
    public int? YearsOfExperience { get; set; }

    public string? Specialty { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == UserRole.Technician)
        {
            if (!YearsOfExperience.HasValue)
            {
                yield return new ValidationResult(
                    "Los años de experiencia son requeridos para técnicos",
                    new[] { nameof(YearsOfExperience) }
                );
            }

            if (string.IsNullOrWhiteSpace(Specialty))
            {
                yield return new ValidationResult(
                    "La especialidad es requerida para técnicos",
                    new[] { nameof(Specialty) }
                );
            }
        }
    }
}