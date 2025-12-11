using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Technicians.Models;

public class UpdateTechnicianRequest
{
    public int TechnicianId { get; set; }

    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "La especialidad no puede exceder 100 caracteres")]
    public string? Specialty { get; set; }

    [Range(0, 50, ErrorMessage = "Los años de experiencia deben estar entre 0 y 50")]
    public int? YearsOfExperience { get; set; }

    public int? DepartmentId { get; set; }
}
