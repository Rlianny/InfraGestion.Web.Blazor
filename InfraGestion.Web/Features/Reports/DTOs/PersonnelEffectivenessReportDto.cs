namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Reporte de efectividad del personal técnico.
/// Calcula métricas de rendimiento basadas en intervenciones, mantenimientos y valoraciones.
/// </summary>
public class PersonnelEffectivenessReportDto
{
    public int TechnicianId { get; set; }

    public string TechnicianName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public int MaintenanceInterventions { get; set; }

    public int DecommissioningRequests { get; set; }

    public int TotalInterventions { get; set; }

    public double TotalMaintenanceCost { get; set; }

    public double AverageCostPerIntervention { get; set; }

    public double AverageRating { get; set; }

    public DateTime? LastInterventionDate { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string SectionName { get; set; } = string.Empty;
}
