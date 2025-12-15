using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;

/// <summary>
/// DTO for reviewing (approve/reject) a decommissioning request
/// POST /decommissioning/requests/review
/// </summary>
public class ReviewDecommissioningRequestDto
{
    [JsonPropertyName("decommissioningRequestId")]
    public int DecommissioningRequestId { get; set; }

    [JsonPropertyName("isApproved")]
    public bool IsApproved { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// Decommissioning reason (only if approved)
    /// </summary>
    [JsonPropertyName("decommissioningReason")]
    public int? DecommissioningReason { get; set; }

    /// <summary>
    /// Final destination department ID (only if approved)
    /// </summary>
    [JsonPropertyName("finalDestination")]
    public int? FinalDestination { get; set; }

    /// <summary>
    /// Justification for approval/rejection
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// ID of the user who will receive the equipment (only if approved)
    /// </summary>
    [JsonPropertyName("receiverID")]
    public int? ReceiverID { get; set; }

    /// <summary>
    /// ID of the logistic user processing this request
    /// </summary>
    [JsonPropertyName("logisticId")]
    public int? LogisticId { get; set; }
}
