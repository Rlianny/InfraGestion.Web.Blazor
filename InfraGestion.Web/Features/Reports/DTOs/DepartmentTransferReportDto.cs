namespace InfraGestion.Web.Features.Reports.DTOs;

/// <summary>
/// Reporte de transferencias de equipos entre departamentos.
/// Incluye información de envío, recepción e identidad de los responsables.
/// </summary>
public class DepartmentTransferReportDto
{
    public int TransferId { get; set; }

    public int DeviceId { get; set; }

    public string DeviceName { get; set; } = string.Empty;

    public string DeviceType { get; set; } = string.Empty;

    public DateTime TransferDate { get; set; }

    public string SourceDepartmentName { get; set; } = string.Empty;

    public string SourceSectionName { get; set; } = string.Empty;

    public string DestinationDepartmentName { get; set; } = string.Empty;

    public string DestinationSectionName { get; set; } = string.Empty;

    public string SenderName { get; set; } = string.Empty;

    public string ReceiverName { get; set; } = string.Empty;

    public string SenderCompanyOrganization { get; set; } = string.Empty;

    public string TransferStatus { get; set; } = string.Empty;

    public DateTime? DeliveryDate { get; set; }
}
