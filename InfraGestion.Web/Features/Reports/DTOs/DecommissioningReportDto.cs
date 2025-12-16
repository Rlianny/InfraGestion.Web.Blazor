namespace InfraGestion.Web.Features.Reports.DTOs;

public class DecommissioningReportDto
{
    public int EquipmentId { get; set; }

    public string EquipmentName { get; set; }

    public string DecommissionCause { get; set; }

    public string FinalDestination { get; set; }

    public string ReceiverName { get; set; }

}
