namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Represents a transfer record for a device between sections
/// </summary>
public class TransferRecord
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int SourceSectionId { get; set; }
    public string SourceSectionName { get; set; } = string.Empty;
    public int DestinationSectionId { get; set; }
    public string DestinationSectionName { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public TransferStatus Status { get; set; } = TransferStatus.Pending;
}