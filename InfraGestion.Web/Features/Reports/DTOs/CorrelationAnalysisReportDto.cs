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
    
    
    public string TechnicianName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;
    
    public double CorrelationIndex { get; set; }

    public double AveragePerformanceRating { get; set; }

}
