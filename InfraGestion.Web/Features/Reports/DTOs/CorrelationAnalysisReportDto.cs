namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Análisis de correlación entre rendimiento de técnicos y longevidad de equipos.
/// Identifica los 5 técnicos con peor correlación:
/// - Alto costo de mantenimiento
/// - Baja longevidad de equipos (dados de baja por fallo técnico irreparable)
/// Requisito de negocio: Funcionalidad 4
/// </summary>
public class CorrelationAnalysisReportDto
{
    public int Rank { get; set; }

    public int TechnicianId { get; set; }

    public string TechnicianName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public string EquipmentType { get; set; } = string.Empty;

    public int EquipmentCount { get; set; }

    public int DecommissionedEquipmentCount { get; set; }

    public int IrreparableFailureCount { get; set; }

    public double TotalMaintenanceCost { get; set; }

    public double AverageMaintenanceCostPerEquipment { get; set; }

    public double AverageEquipmentLongevity { get; set; }

    public double CorrelationIndex { get; set; }

    public double AveragePerformanceRating { get; set; }

    public double TotalBonuses { get; set; }

    public double TotalPenalties { get; set; }

    public double NetBalance { get; set; }

    public string Observations { get; set; } = string.Empty;
}
