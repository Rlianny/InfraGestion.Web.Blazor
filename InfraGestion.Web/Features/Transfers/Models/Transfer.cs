namespace InfraGestion.Web.Features.Transfers.Models;

/// <summary>
/// Represents a device transfer between company sections
/// </summary>
public class Transfer
{
    public int TransferId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string SourceSectionName{ get; set; } = string.Empty;
    public string DestinationSectionName { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
