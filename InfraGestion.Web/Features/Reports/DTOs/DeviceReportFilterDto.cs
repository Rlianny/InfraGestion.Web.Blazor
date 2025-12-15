

using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Filtros para generar reportes de inventario de equipos.
/// Permite filtrar por tipo de equipo, estado operacional, departamento y rango de fechas.
/// </summary>
public class DeviceReportFilterDto
{
    /// <summary>
    /// Identificador del departamento para filtrado de ubicación
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// Identificador de la sección para filtrado más granular
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// Tipo de equipo a filtrar
    /// </summary>
    public DeviceType? DeviceType { get; set; }

    /// <summary>
    /// Estado operacional del equipo a filtrar
    /// </summary>
    public OperationalState? OperationalState { get; set; }

    /// <summary>
    /// Fecha inicial de adquisición para el rango
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Fecha final de adquisición para el rango
    /// </summary>
    public DateTime? ToDate { get; set; }
    public string? Department { get; internal set; }
}
