using System.Text.Json.Serialization;

namespace InfraGestion.Web.Features.Inventory.DTOs;

public class DeviceDetailDto
{
    [JsonPropertyName("deviceId")]
    public int DeviceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("deviceType")]
    public int DeviceType { get; set; }

    [JsonPropertyName("operationalState")]
    public int OperationalState { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("acquisitionDate")]
    public DateTime AcquisitionDate { get; set; }

    [JsonPropertyName("maintenanceHistory")]
    public IEnumerable<MaintenanceRecordDto> MaintenanceHistory { get; set; } = new List<MaintenanceRecordDto>();

    [JsonPropertyName("transferHistory")]
    public IEnumerable<TransferDto> TransferHistory { get; set; } = new List<TransferDto>();

    [JsonPropertyName("decommissioningRequestInfo")]
    public IEnumerable<DecommissioningRequestDto> DecommissioningRequestInfo { get; set; } = new List<DecommissioningRequestDto>();

    [JsonPropertyName("sectionName")]
    public string SectionName { get; set; } = string.Empty;

    [JsonPropertyName("sectionManagerName")]
    public string? SectionManagerName { get; set; }
}