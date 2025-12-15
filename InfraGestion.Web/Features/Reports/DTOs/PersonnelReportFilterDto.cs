namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Filtros para generar reportes de efectividad del personal.
/// Permite filtrar por ubicación, especialidad, experiencia y período de análisis.
/// </summary>
public class PersonnelReportFilterDto
{
    /// <summary>
    /// Identificador del departamento para filtrar por ubicación
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// Identificador de la sección para filtrado más granular
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// Especialidad técnica del personal a filtrar
    /// </summary>
    public string? Specialty { get; set; }

    /// <summary>
    /// Rango mínimo de años de experiencia
    /// </summary>
    public int? MinimumYearsOfExperience { get; set; }

    /// <summary>
    /// Rango máximo de años de experiencia
    /// </summary>
    public int? MaximumYearsOfExperience { get; set; }

    /// <summary>
    /// Calificación mínima promedio para incluir
    /// </summary>
    public double? MinimumAverageRating { get; set; }

    /// <summary>
    /// Fecha inicial para análisis de efectividad
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Fecha final para análisis de efectividad
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Incluir solo técnicos con intervenciones activas en el rango de fechas
    /// </summary>
    public bool? OnlyActiveInPeriod { get; set; }
}
