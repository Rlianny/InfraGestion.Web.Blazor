using InfraGestion.Web.Features.Technicians.DTOs;

namespace InfraGestion.Web.Features.Technicians.Models;

/// <summary>
/// Detalles extendidos del técnico para la vista de perfil
/// </summary>
public class TechnicianDetails
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string SectionManager { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public TechnicianStatus Status { get; set; } = TechnicianStatus.Active;
    public decimal Rating { get; set; } = 0;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime HireDate { get; set; }
    public int YearsOfExperience { get; set; }
    public DateTime? LastInterventionDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Historiales relacionados
    public List<MaintenanceRecord> MaintenanceHistory { get; set; } = new();
    public List<DecommissionProposal> DecommissionProposals { get; set; } = new();
    public List<TechnicianRating> Ratings { get; set; } = new();
    public List<BonusDto> Bonuses { get; set; } = new();
    public List<PenaltyDto> Penalties { get; set; } = new();
}

/// <summary>
/// Registro de mantenimiento realizado por el técnico
/// </summary>
public class MaintenanceRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string TechnicianName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
}

/// <summary>
/// Proposición de baja de equipo
/// </summary>
public class DecommissionProposal
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Cause { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Aprobado, Pendiente, Rechazado
}

/// <summary>
/// Valoración del técnico
/// </summary>
public class TechnicianRating
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public string Description { get; set; } = string.Empty;
}
