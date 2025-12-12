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
    public int DeviceType { get; set; } // Enum as int from backend
    public int OperationalState { get; set; } // Enum as int from backend
    public string DepartmentName { get; set; } = string.Empty;

    // DeviceDetailDto specific
    public DateTime AcquisitionDate { get; set; }
    public IEnumerable<MaintenanceRecordDto> MaintenanceHistory { get; set; } = new List<MaintenanceRecordDto>();
    public IEnumerable<TransferDto> TransferHistory { get; set; } = new List<TransferDto>();
    public DecommissioningDto? DecommissioningInfo { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string? SectionManagerName { get; set; }
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
    public int MaintenanceType { get; set; } // Enum as int from backend
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
    public DateTime TransferDate { get; set; }
    public int SourceSectionId { get; set; }
    public string SourceSectionName { get; set; } = string.Empty;
    public int DestinationSectionId { get; set; }
    public string DestinationSectionName { get; set; } = string.Empty;
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public int Status { get; set; } 
}

/// <summary>
/// Decommissioning info from API
/// Matches backend DecommissioningDto
/// </summary>
public class DecommissioningDto
{
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int DecommissioningRequestId { get; set; }
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public int ReceiverDepartmentId { get; set; }
    public string ReceiverDepartmentName { get; set; } = string.Empty;
    public DateTime DecommissioningDate { get; set; }
    public int Reason { get; set; } 
    public string? FinalDestination { get; set; }
}