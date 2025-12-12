using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.DTOs;


public class DeviceFilterDto
{
    public string? SearchTerm { get; set; }
    public DeviceType? DeviceType { get; set; }
    public OperationalState? OperationalState { get; set; }
    public string? DepartmentName { get; set; }
    public int? DepartmentId { get; set; }
    public DateTime? AcquisitionDateFrom { get; set; }
    public DateTime? AcquisitionDateTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? SortBy { get; set; }
    public bool SortAscending { get; set; } = true;
}