namespace InfraGestion.Web.Features.Inventory.Models;

/// <summary>
/// Represents the initial defect report for a device
/// </summary>
public class InitialDefect
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public DateTime SubmissionDate { get; set; }
    public int RequesterId { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public InitialDefectStatus Status { get; set; } = InitialDefectStatus.Pending;
    public DateTime? ResponseDate { get; set; }
    public string? Description { get; set; }
}