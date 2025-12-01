namespace InfraGestion.Web.Features.Transfers.Models;

/// <summary>
/// Representa un traslado de equipo entre secciones de la empresa
/// </summary>
public class Transfer
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
