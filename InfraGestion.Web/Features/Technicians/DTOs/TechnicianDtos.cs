using InfraGestion.Web.Features.Technicians.Models;

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
/// DTO para recibir detalles completos de un técnico desde el API
/// </summary>
public class TechnicianDetailDto
{
    public int TechnicianId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public List<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new();
    public List<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new();
}

/// <summary>
/// DTO para recibir registros de mantenimiento
/// </summary>
public class MaintenanceRecordDto
{
    public int MaintenanceRecordId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = string.Empty;
    public double Cost { get; set; }
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO para recibir solicitudes de baja de equipos
/// </summary>
public class DecommissioningRequestDto
{
    public int DecommissioningRequestId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public int DeviceReceiverId { get; set; }
    public string DeviceReceiverName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime? ReviewedDate { get; set; }
    public int? ReviewedByUserId { get; set; }
    public string? ReviewedByUserName { get; set; }
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
    public int GiverId { get; set; }
    public string GiverName { get; set; } = string.Empty;
    public double Score { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

/// <summary>
/// Respuesta extendida de detalles de técnico
/// </summary>
public class TechnicianDetailResponse
{
    public int TechnicianId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string SectionManager { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? LastInterventionDate { get; set; }
    public List<MaintenanceRecordDetailDto> MaintenanceRecords { get; set; } = new();
    public List<DecommissioningRequestDetailDto> DecommissioningRequests { get; set; } = new();
    public List<RateDto> Ratings { get; set; } = new();
    public List<BonusDto> Bonuses { get; set; } = new();
    public List<PenaltyDto> Penalties { get; set; } = new();
}

public class MaintenanceRecordDetailDto
{
    public int MaintenanceRecordId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = string.Empty;
    public double Cost { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
}

public class DecommissioningRequestDetailDto
{
    public int DecommissioningRequestId { get; set; }
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceReceiverName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
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
