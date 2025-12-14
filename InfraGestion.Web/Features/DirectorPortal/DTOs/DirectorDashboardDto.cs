using System.Text.Json.Serialization;
using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.DirectorPortal.DTOs;

/// <summary>
/// DTO que contiene toda la información necesaria para la vista del Dashboard del Director.
/// Este DTO es el que el backend debe proporcionar para poblar la vista estratégica.
/// </summary>
public class DirectorDashboardDto
{
    /// <summary>
    /// Estadísticas generales (tarjetas superiores)
    /// </summary>
    [JsonPropertyName("summary")]
    public DashboardSummaryDto Summary { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Visión General del Inventario"
    /// </summary>
    [JsonPropertyName("inventoryOverview")]
    public InventoryOverviewDto InventoryOverview { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Análisis de Costos"
    /// </summary>
    [JsonPropertyName("costAnalysis")]
    public CostAnalysisDto CostAnalysis { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Análisis de Bajas"
    /// </summary>
    [JsonPropertyName("decommissionAnalysis")]
    public DecommissionAnalysisDto DecommissionAnalysis { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Efectividad por Departamento"
    /// </summary>
    [JsonPropertyName("departmentEffectiveness")]
    public DepartmentEffectivenessDto DepartmentEffectiveness { get; set; } = new();
}

/// <summary>
/// Estadísticas resumidas mostradas en las 4 tarjetas superiores
/// </summary>
public class DashboardSummaryDto
{
    /// <summary>
    /// Total de activos/equipos en el sistema
    /// </summary>
    [JsonPropertyName("totalAssets")]
    public int TotalAssets { get; set; }

    /// <summary>
    /// Número total de departamentos
    /// </summary>
    [JsonPropertyName("totalDepartments")]
    public int TotalDepartments { get; set; }

    /// <summary>
    /// Número de bajas en el trimestre actual
    /// </summary>
    [JsonPropertyName("quarterlyDecommissions")]
    public int QuarterlyDecommissions { get; set; }

    /// <summary>
    /// Costo total de mantenimiento (en la moneda local)
    /// </summary>
    [JsonPropertyName("maintenanceCost")]
    public double MaintenanceCost { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Visión General del Inventario"
/// </summary>
public class InventoryOverviewDto
{
    /// <summary>
    /// Número de equipos operativos (funcionando correctamente)
    /// </summary>
    [JsonPropertyName("operationalDevices")]
    public int OperationalDevices { get; set; }

    /// <summary>
    /// Número de equipos actualmente en mantenimiento
    /// </summary>
    [JsonPropertyName("devicesInMaintenance")]
    public int DevicesInMaintenance { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Análisis de Costos"
/// </summary>
public class CostAnalysisDto
{
    /// <summary>
    /// Costo mensual promedio de mantenimiento
    /// </summary>
    [JsonPropertyName("monthlyAverageCost")]
    public double MonthlyAverageCost { get; set; }

    /// <summary>
    /// Porcentaje de tendencia vs mes anterior (positivo = aumento, negativo = reducción)
    /// Ejemplo: -5.2 significa una reducción del 5.2%
    /// </summary>
    [JsonPropertyName("trendPercentage")]
    public double TrendPercentage { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Análisis de Bajas"
/// </summary>
public class DecommissionAnalysisDto
{
    /// <summary>
    /// Lista de las principales causas de baja con su cantidad
    /// </summary>
    [JsonPropertyName("topCauses")]
    public List<DecommissionCauseDto> TopCauses { get; set; } = new();
}

/// <summary>
/// Representa una causa de baja con su cantidad
/// </summary>
public class DecommissionCauseDto
{
    /// <summary>
    /// Razón/causa de la baja (enum del backend)
    /// </summary>
    [JsonPropertyName("reason")]
    public DecommissioningReason Reason { get; set; }

    /// <summary>
    /// Cantidad de equipos dados de baja por esta causa
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Efectividad por Departamento"
/// </summary>
public class DepartmentEffectivenessDto
{
    /// <summary>
    /// Lista de departamentos con su cantidad de mantenimientos
    /// </summary>
    [JsonPropertyName("departments")]
    public List<DepartmentEffectivenessItemDto> Departments { get; set; } = new();
}

/// <summary>
/// Representa la efectividad de un departamento
/// </summary>
public class DepartmentEffectivenessItemDto
{
    /// <summary>
    /// Nombre del departamento
    /// </summary>
    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de mantenimientos realizados por el departamento
    /// </summary>
    [JsonPropertyName("maintenanceCount")]
    public int MaintenanceCount { get; set; }
}
