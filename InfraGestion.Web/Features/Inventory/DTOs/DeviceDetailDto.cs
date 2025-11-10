using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// Detailed device information from API
/// </summary>
public class DeviceDetailDto
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceType DeviceType { get; set; }
    public OperationalState OperationalState { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    
    // Optional fields
    public int? MaintenanceCount { get; set; }
    public decimal? TotalMaintenanceCost { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public string? SectionName { get; set; }
    public string? SectionManager { get; set; }
    
    // Related data
    public List<MaintenanceRecordDto>? MaintenanceHistory { get; set; }
    public List<TransferRecordDto>? TransferHistory { get; set; }
    public InitialDefectDto? InitialDefect { get; set; }
}

public class MaintenanceRecordDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Technician { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}

public class TransferRecordDto
{
    public DateTime Date { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
}

public class InitialDefectDto
{
    public DateTime IssueDate { get; set; }
    public string Requester { get; set; } = string.Empty;
    public string Technician { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ResponseDate { get; set; }
}