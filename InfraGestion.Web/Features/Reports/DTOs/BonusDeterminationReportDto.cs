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
    
    public string TechnicianName { get; set; } = string.Empty;
    public int TotalInterventions { get; set; } 
    public double AverageRating { get; set; }
    public double HighestRating { get; set; }
    public double LowestRating { get; set; }
    public int RatingCount { get; set; }
    
}
