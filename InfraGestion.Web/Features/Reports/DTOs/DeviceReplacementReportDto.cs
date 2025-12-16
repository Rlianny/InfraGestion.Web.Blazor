using System;

namespace InfraGestion.Web.Features.Reports.DTOs
{
    /// <summary>
    /// Reporte de equipos que deben ser reemplazados.
    /// Identifica equipos que han recibido más de 3 mantenimientos en el último año.
    /// Requisito de normativa: estos equipos deben ser reemplazados.
    /// </summary>
    public class DeviceReplacementReportDto
    {
       
        public int DeviceId { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public int MaintenanceCountLastYear { get; set; }
        public double TotalMaintenanceCost { get; set; }

    }
}
