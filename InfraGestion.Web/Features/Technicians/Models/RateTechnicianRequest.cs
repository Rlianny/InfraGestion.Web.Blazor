using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Model for rating a technician
/// </summary>
public class RateTechnicianRequest
{
    public int TechnicianId { get; set; }

    [Required(ErrorMessage = "La puntuación es requerida")]
    [Range(0, 10, ErrorMessage = "La puntuación debe estar entre 0 y 10")]
    public double Score { get; set; }

    [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
    public string? Comment { get; set; }
}
