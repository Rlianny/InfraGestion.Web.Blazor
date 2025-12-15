namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Criterios para generar reportes de determinación de bonificaciones.
/// Define parámetros para el cálculo de bonos y penalizaciones basado en:
/// - Valoraciones de superiores
/// - Cantidad de intervenciones
/// - Calidad del trabajo y rendimiento
/// Requisito de negocio: Funcionalidad 6
/// </summary>
public class BonusReportCriteria
{
    /// <summary>
    /// Mes para el cual se calcula la bonificación (1-12)
    /// </summary>
    public int? Month { get; set; }

    /// <summary>
    /// Año para el cual se calcula la bonificación
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Departamento específico para el análisis
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// Sección específica para el análisis
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// Calificación mínima promedio para bonificación
    /// </summary>
    public double? MinimumRatingForBonus { get; set; }

    /// <summary>
    /// Calificación máxima promedio para penalización
    /// </summary>
    public double? MaximumRatingForPenalty { get; set; }

    /// <summary>
    /// Número mínimo de intervenciones para estar elegible
    /// </summary>
    public int? MinimumInterventions { get; set; }

    /// <summary>
    /// Incluir solo técnicos activos
    /// </summary>
    public bool? OnlyActiveTechnicians { get; set; }

    /// <summary>
    /// Campo por el cual ordenar resultados
    /// </summary>
    public string? SortBy { get; set; }
}
