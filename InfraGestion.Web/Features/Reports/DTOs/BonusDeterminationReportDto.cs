namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Reporte de determinación de bonificaciones y penalizaciones del personal.
/// Compara rendimiento de técnicos basado en:
/// - Valoraciones de sus superiores
/// - Cantidad de intervenciones realizadas
/// - Calidad del trabajo (reflejada en equipos que llegan a baja por fallo irreparable)
/// Requisito de negocio: Funcionalidad 6
/// </summary>
public class BonusDeterminationReportDto
{
    public int TechnicianId { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int TotalInterventions { get; set; }
    public int MaintenanceCount { get; set; }
    public int DecommissioningCount { get; set; }
    public double TotalMaintenanceCost { get; set; }
    public double AverageRating { get; set; }
    public double HighestRating { get; set; }
    public double LowestRating { get; set; }
    public int RatingCount { get; set; }
    public double TotalBonuses { get; set; }
    public double TotalPenalties { get; set; }
    public double EffectivenessIndex { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public double SalaryAdjustmentAmount { get; set; }
    public string AdjustmentType { get; set; } = string.Empty; // "Bonificación" o "Penalización"
    public string Comments { get; set; } = string.Empty;
}
