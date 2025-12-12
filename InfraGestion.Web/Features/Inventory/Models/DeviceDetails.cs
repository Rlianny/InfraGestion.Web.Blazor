namespace InfraGestion.Web.Features.Inventory.Models;


public class DeviceDetails
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty; // DEV002
    public DeviceType Type { get; set; }
    public OperationalState State { get; set; }
    public DateTime PurchaseDate { get; set; }

    public int MaintenanceCount { get; set; }
    public decimal TotalMaintenanceCost { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }

    public int DepartmentId { get; set; }
    public string Department { get; set; } = string.Empty;
    public int SectionId { get; set; }
    public string Section { get; set; } = string.Empty;
    public string SectionManager { get; set; } = string.Empty;

    public List<MaintenanceRecord> MaintenanceHistory { get; set; } = new();
    public List<TransferRecord> TransferHistory { get; set; } = new();
    public DecommissioningRequest? DecommissioningInfo { get; set; }
}