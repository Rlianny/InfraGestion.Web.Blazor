using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Detailed device information from API
/// Matches backend DeviceDetailDto : DeviceDto
/// </summary>
public class DeviceDetailDto
{
    // From DeviceDto base class
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType DeviceType { get; set; }
    public OperationalState OperationalState { get; set; }
    
    // Location info from backend
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    
    // DeviceDetailDto specific
    public DateTime AcquisitionDate { get; set; }
    public List<MaintenanceRecordDto> MaintenanceHistory { get; set; } = new();
    public List<TransferDto> TransferHistory { get; set; } = new();
    public InitialDefectDto? InitialDefect { get; set; }
    public DecommissioningRequestDto? DecommissioningInfo { get; set; }
}

/// <summary>
/// Maintenance record from API
/// Matches backend MaintenanceRecordDto
/// </summary>
public class MaintenanceRecordDto
{
    public int MaintenanceRecordId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime MaintenanceDate { get; set; }
    public MaintenanceType MaintenanceType { get; set; }
    public double Cost { get; set; }
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Transfer record from API
/// Matches backend TransferDto
/// </summary>
public class TransferDto
{
    public int TransferId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public DateOnly TransferDate { get; set; }
    public int SourceSectionId { get; set; }
    public string SourceSectionName { get; set; } = string.Empty;
    public int DestinationSectionId { get; set; }
    public string DestinationSectionName { get; set; } = string.Empty;
    public int ResponsiblePersonId { get; set; }
    public string ResponsiblePersonName { get; set; } = string.Empty;
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public TransferStatus Status { get; set; } = TransferStatus.Pending;
}

/// <summary>
/// Initial defect report from API
/// Matches backend InitialDefectDto
/// </summary>
public class InitialDefectDto
{
    public int InitialDefectId { get; set; }
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

/// <summary>
/// Decommissioning request from API
/// Matches backend DecommissioningRequestDto
/// </summary>
public class DecommissioningRequestDto
{
    public int DecommissioningRequestId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DecommissioningStatus Status { get; set; } = DecommissioningStatus.Pending;
    public string Justification { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime? ReviewedDate { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? ReviewedByUserName { get; set; }
}