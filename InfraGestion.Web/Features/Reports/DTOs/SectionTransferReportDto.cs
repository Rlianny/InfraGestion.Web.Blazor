namespace InfraGestion.Web.Features.Reports.DTOs;



    /// <summary>
    /// Reporte de transferencias de equipos entre departamentos.
    /// Incluye información de envío, recepción e identidad de los responsables.
    /// </summary>
    public class SectionTransferReportDto
    {
        
        public int TransferId { get; set; }    
        public string DeviceName { get; set; } = string.Empty;  
        public DateTime TransferDate { get; set; }
        public string SourceSectionName { get; set; } = string.Empty;
        public string DestinationSectionName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;

    }

