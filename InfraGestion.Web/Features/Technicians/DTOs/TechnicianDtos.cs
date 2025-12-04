namespace InfraGestion.Web.Features.Technicians.DTOs;

/// <summary>
/// DTO para recibir datos de Técnico desde el API
/// </summary>
public class TechnicianDto
{
    public int TechnicianId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Specialty { get; set; } = string.Empty;
}

/// <summary>
/// DTO para recibir bonificaciones de un técnico
/// </summary>
public class BonusDto
{
    public int BonusId { get; set; }
    public int TechnicianId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

/// <summary>
/// DTO para recibir penalizaciones de un técnico
/// </summary>
public class PenaltyDto
{
    public int PenaltyId { get; set; }
    public int TechnicianId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

/// <summary>
/// DTO para recibir historial de rendimiento/calificaciones
/// </summary>
public class RateDto
{
    public int RateId { get; set; }
    public int TechnicianId { get; set; }
    public decimal Score { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string RatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Request para calificar a un técnico
/// </summary>
public class RateTechnicianRequest
{
    public int TechnicianId { get; set; }
    public decimal Score { get; set; }
    public string Comment { get; set; } = string.Empty;
}

/// <summary>
/// Request para registrar una bonificación
/// </summary>
public class BonusRequest
{
    public int TechnicianId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// Request para registrar una penalización
/// </summary>
public class PenaltyRequest
{
    public int TechnicianId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
