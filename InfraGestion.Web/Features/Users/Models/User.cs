using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Users.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El departamento es requerido")]
    [StringLength(200, ErrorMessage = "El departamento no puede exceder 200 caracteres")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    public UserRole Role { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string? Password { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Technician specific fields
    public int? YearsOfExperience { get; set; }
    public string? Specialty { get; set; }
}