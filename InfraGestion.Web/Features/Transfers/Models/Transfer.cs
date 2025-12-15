namespace InfraGestion.Web.Features.Transfers.Models;

/// <summary>
/// Represents a device transfer between company sections
/// </summary>
public class Transfer
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
