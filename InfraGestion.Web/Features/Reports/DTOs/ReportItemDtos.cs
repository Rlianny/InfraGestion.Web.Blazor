namespace InfraGestion.Web.Features.Reports.DTOs;

// Wrapper DTOs for API responses containing lists of items

/// <summary>
/// Wrapper for decommissioning report response
/// </summary>
public class DecommissioningReportResponseDto
{
    public List<DecommissioningReportItemDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
}

/// <summary>
/// Item DTO for decommissioning reports (renamed to avoid conflict with existing DTO)
/// </summary>
public class DecommissioningReportItemDto
{
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public string DecommissionCause { get; set; } = string.Empty;
    public string FinalDestination { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime DecommissionDate { get; set; }
}

/// <summary>
/// Item DTO for department transfer reports
/// </summary>
public class DepartmentTransferReportItemDto
{
    public int TransferId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string SourceDepartmentName { get; set; } = string.Empty;
    public string DestinationDepartmentName { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
}

/// <summary>
/// Item DTO for correlation analysis reports
/// </summary>
public class CorrelationAnalysisReportItemDto
{
    public int Rank { get; set; }
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public decimal TotalMaintenanceCost { get; set; }
    public decimal AverageEquipmentLongevity { get; set; }
    public decimal CorrelationIndex { get; set; }
}

/// <summary>
/// Item DTO for device replacement reports
/// </summary>
public class DeviceReplacementReportItemDto
{
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int MaintenanceCountLastYear { get; set; }
    public decimal TotalMaintenanceCost { get; set; }
    public string ReplacementReason { get; set; } = string.Empty;
}

/// <summary>
/// Item DTO for bonus determination reports
/// </summary>
public class BonusDeterminationReportItemDto
{
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int TotalInterventions { get; set; }
    public double AverageRating { get; set; }
    public double TotalBonuses { get; set; }
    public double TotalPenalties { get; set; }
    public double SalaryAdjustmentAmount { get; set; }
    public string AdjustmentType { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}

/// <summary>
/// DTO for department equipment report
/// </summary>
public class DepartmentEquipmentReportDto
{
    public List<SectionEquipmentDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
}

/// <summary>
/// Item DTO for department equipment
/// </summary>
public class SectionEquipmentDto
{
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
}

/// <summary>
/// DTO for maintenance history report
/// </summary>


/// <summary>
/// Item DTO for maintenance history
/// </summary>

