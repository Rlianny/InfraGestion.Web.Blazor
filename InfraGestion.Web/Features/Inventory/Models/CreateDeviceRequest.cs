using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Inventory.Models;

public class CreateDeviceRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo es requerido")]
    public DeviceType Type { get; set; }

    [Required(ErrorMessage = "La fecha de adquisición es requerida")]
    public DateTime PurchaseDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El técnico es requerido")]
    public string TechnicianName { get; set; } = string.Empty;
}