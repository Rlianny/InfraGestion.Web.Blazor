using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Model for adding a bonus to a technician
/// </summary>
public class BonusRequest
{
    public int TechnicianId { get; set; }

    [Required(ErrorMessage = "El monto es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public double Amount { get; set; }

    [Required(ErrorMessage = "La razón es requerida")]
    [StringLength(500, ErrorMessage = "La razón no puede exceder 500 caracteres")]
    public string Reason { get; set; } = string.Empty;
}
