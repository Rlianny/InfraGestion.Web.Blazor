namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// DTO for updating a decommissioning request
/// Maps to: PUT /decommissioning/requests
/// </summary>
public class UpdateDecommissioningRequestDto
{
    public int DecommissioningRequestId { get; set; }
    public int TechnicianId { get; set; }
    public int DeviceId { get; set; }
    public DateTime EmissionDate { get; set; }
    public DateTime? AnswerDate { get; set; }
    public int Status { get; set; }
    public int Reason { get; set; }
    public int? DeviceReceiverId { get; set; }
    public bool? IsApproved { get; set; }
    public int? FinalDestinationDepartmentID { get; set; }
    public int? LogisticId { get; set; }
    public string Description { get; set; } = "";
}