namespace InfraGestion.Web.Features.Inventory.Models;

public class InitialDefect
{
    public DateTime SubmissionDate { get; set; }
    public string Requester { get; set; } = string.Empty;
    public string Technician { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Aprobado, Pendiente, Rechazado
    public DateTime? ResponseDate { get; set; }
}