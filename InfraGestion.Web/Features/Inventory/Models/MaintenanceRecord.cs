namespace InfraGestion.Web.Features.Inventory.Models;

public class MaintenanceRecord
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty; // Preventivo, Predictivo, Correctivo
    public string Technician { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}