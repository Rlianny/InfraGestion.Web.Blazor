using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Transfers.Models;

/// <summary>
/// Request to create a new device transfer
/// </summary>
public class CreateTransferRequest
{
    [Required(ErrorMessage = "El equipo es requerido")]
    public string DeviceId { get; set; } = string.Empty;

    [Required(ErrorMessage = "El origen es requerido")]
    public string Origin { get; set; } = string.Empty;

    [Required(ErrorMessage = "El destino es requerido")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de traslado es requerida")]
    public DateTime TransferDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El receptor del equipo es requerido")]
    [StringLength(100, ErrorMessage = "El nombre del receptor no puede exceder 100 caracteres")]
    public string ReceiverName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
    public string? Notes { get; set; }
}
