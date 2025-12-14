namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Represents a technician in the infrastructure team
/// </summary>
public class Technician
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Section { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty; // TODO: Add photo handling
    public TechnicianStatus Status { get; set; } = TechnicianStatus.Active;
    public decimal Rating { get; set; } = 0;
    public DateTime HireDate { get; set; }
}

/// <summary>
/// Possible states of a technician
/// </summary>
public enum TechnicianStatus
{
    Active,
    Inactive,
    OnLeave
}
