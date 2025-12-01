namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Representa un técnico del equipo de infraestructura
/// </summary>
public class Technician
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public TechnicianStatus Status { get; set; } = TechnicianStatus.Active;
    public decimal Rating { get; set; } = 0;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Estados posibles de un técnico
/// </summary>
public enum TechnicianStatus
{
    Active,
    Inactive,
    OnLeave
}
