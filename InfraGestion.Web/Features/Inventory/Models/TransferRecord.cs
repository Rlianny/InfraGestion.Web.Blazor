namespace InfraGestion.Web.Features.Inventory.Models;

public class TransferRecord
{
    public DateTime Date { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty; 
    public string Receiver { get; set; } = string.Empty;
}