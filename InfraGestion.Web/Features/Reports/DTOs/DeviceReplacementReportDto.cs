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
        private string name;

        public DeviceReplacementReportDto(int deviceId, string name)
        {
            DeviceId = deviceId;
            this.name = name;
        }

        public int DeviceId { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public string DeviceType { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string SectionName { get; set; } = string.Empty;

        public DateTime AcquisitionDate { get; set; }

        public int MaintenanceCountLastYear { get; set; }

        public double TotalMaintenanceCost { get; set; }

        public double AverageMaintenanceCost { get; set; }

        public DateTime? LastMaintenanceDate { get; set; }

        public int YearsInService { get; set; }

        public string OperationalState { get; set; } = string.Empty;

        public string ReplacementReason { get; set; } = "Mantenimientos excesivos (>3 en último año)";
    }
}
