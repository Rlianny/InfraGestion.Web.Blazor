

using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Filtros para generar reportes de bajas técnicas.
/// Permite filtrar por rango de fechas, causa de baja y estado.
/// </summary>
public class DecommissioningReportFilterDto
{
    /// <summary>
    /// Fecha inicial del rango de búsqueda
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Fecha final del rango de búsqueda
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Filtrar por causa específica de baja
    /// </summary>
    public DecommissioningReason? Reason { get; set; }

    /// <summary>
    /// Filtrar por estado de la solicitud de baja
    /// </summary>
    public RequestStatus? Status { get; set; }
}
