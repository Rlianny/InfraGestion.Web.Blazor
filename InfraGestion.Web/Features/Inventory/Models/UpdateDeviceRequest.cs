using System.ComponentModel.DataAnnotations;

namespace InfraGestion.Web.Features.Inventory.Models;

public class UpdateDeviceRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo es requerido")]
    public DeviceType Type { get; set; }

    [Required(ErrorMessage = "La fecha de adquisición es requerida")]
    public DateTime PurchaseDate { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    public OperationalState State { get; set; }

    [Required(ErrorMessage = "La ubicación es requerida")]
    public string Location { get; set; } = string.Empty;
}