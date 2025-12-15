namespace InfraGestion.Web.Features.Transfers.Models;

/// <summary>
/// Represents a device transfer between company sections
/// </summary>
public class Transfer
{
    public int TransferId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int SourceSectionId { get; set; }
    public string SourceSectionName{ get; set; } = string.Empty;
    public int DestinationSectionId { get; set; }
    public string DestinationSectionName { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public TransferStatus Status { get; set; } = TransferStatus.Pending;
    public string? Notes { get; set; }
}

/// <summary>
/// Possible states of a transfer
/// </summary>
public enum TransferStatus
{
    Pending = 0,
    InTransit = 1,
    Completed = 2,
    Cancelled = 3
}
