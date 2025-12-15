using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Reports.DTOs;
using System.Net.Http.Json;
using System.Reflection;
namespace InfraGestion.Web.Features.Reports.Services;

public class FrontendReport
{
	private readonly HttpClient _httpClient;
	private Dictionary<Type, PropertyInfo[]> _typesCache;
	public FrontendReport(HttpClient httpClient)
	{
		_httpClient = httpClient;
		_typesCache = new Dictionary<Type, PropertyInfo[]>();
    }

	public async Task<DeviceReportDto> GenerateInventoryReportAsync(DeviceReportFilterDto filter)
	{
		string endPoint = "reports/inventory";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<DeviceReportDto>>(endPoint + QueryString(filter))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<DecommissioningReportDto> GenerateDecommissioningReportAsync(DecommissioningReportFilterDto filter)
	{
		string endPoint = "reports/decommissionings";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<DecommissioningReportDto>>(endPoint + QueryString(filter))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	// Simplified method for decommissioning report without filter
	public async Task<List<DecommissioningReportItemDto>> GetDecommissioningsAsync()
	{
		try
		{
			string endPoint = "reports/decommissionings";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DecommissioningReportItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<DecommissioningReportItemDto>();
			}
			return new List<DecommissioningReportItemDto>();
		}
		catch
		{
			return new List<DecommissioningReportItemDto>();
		}
	}

	public async Task<PersonnelEffectivenessReportDto> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
	{
		string endPoint = "reports/personnel-effectiveness";
		
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PersonnelEffectivenessReportDto>>(endPoint + QueryString(criteria))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
        if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<DeviceReplacementReportDto> GenerateEquipmentReplacementReportAsync()
	{
		string endPoint = "reports/equipment-replacement";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<DeviceReplacementReportDto>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	// Simplified method for device replacement report
	public async Task<List<DeviceReplacementReportItemDto>> GetDeviceReplacementAsync()
	{
		try
		{
			string endPoint = "reports/equipment-replacement";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DeviceReplacementReportItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<DeviceReplacementReportItemDto>();
			}
			return new List<DeviceReplacementReportItemDto>();
		}
		catch
		{
			return new List<DeviceReplacementReportItemDto>();
		}
	}

	public async Task<DepartmentTransferReportDto> GenerateDepartmentTransferReportAsync(string departmentId)
	{
		string endPoint = $"reports/department-transfer?departmentId={departmentId}";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<DepartmentTransferReportDto>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	// Simplified method for department transfers report (all transfers)
	public async Task<List<DepartmentTransferReportItemDto>> GetDepartmentTransfersAsync()
	{
		try
		{
			string endPoint = "reports/department-transfer";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartmentTransferReportItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<DepartmentTransferReportItemDto>();
			}
			return new List<DepartmentTransferReportItemDto>();
		}
		catch
		{
			return new List<DepartmentTransferReportItemDto>();
		}
	}

	// Method for department equipment report
	public async Task<List<DepartmentEquipmentItemDto>> GetDepartmentEquipmentAsync(int departmentId)
	{
		try
		{
			string endPoint = $"reports/department-equipment?departmentId={departmentId}";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartmentEquipmentItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<DepartmentEquipmentItemDto>();
			}
			return new List<DepartmentEquipmentItemDto>();
		}
		catch
		{
			return new List<DepartmentEquipmentItemDto>();
		}
	}

	public async Task<CorrelationAnalysisReportDto> GenerateCorrelationAnalysisReportAsync()
	{
		string endPoint = "reports/correlation-analysis";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<CorrelationAnalysisReportDto>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	// Simplified method for correlation analysis report
	public async Task<List<CorrelationAnalysisReportItemDto>> GetCorrelationAnalysisAsync()
	{
		try
		{
			string endPoint = "reports/correlation-analysis";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CorrelationAnalysisReportItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<CorrelationAnalysisReportItemDto>();
			}
			return new List<CorrelationAnalysisReportItemDto>();
		}
		catch
		{
			return new List<CorrelationAnalysisReportItemDto>();
		}
	}

	public async Task<BonusDeterminationReportDto> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
	{
		string endPoint = "reports/bonus-determination";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<BonusDeterminationReportDto>>(endPoint + QueryString(criteria))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data!;
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	// Simplified method for bonus determination report
	public async Task<List<BonusDeterminationReportItemDto>> GetBonusDeterminationAsync()
	{
		try
		{
			string endPoint = "reports/bonus-determination";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BonusDeterminationReportItemDto>>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data ?? new List<BonusDeterminationReportItemDto>();
			}
			return new List<BonusDeterminationReportItemDto>();
		}
		catch
		{
			return new List<BonusDeterminationReportItemDto>();
		}
	}

	// Method for equipment maintenance history
	public async Task<List<MaintenanceHistoryItemDto>> GetEquipmentMaintenanceHistoryAsync(int equipmentId)
	{
		try
		{
			string endPoint = $"reports/equipment-maintenance?equipmentId={equipmentId}";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<MaintenanceHistoryReportDto>>(endPoint);
			if (response?.Success == true)
			{
				return response.Data?.Items ?? new List<MaintenanceHistoryItemDto>();
			}
			return new List<MaintenanceHistoryItemDto>();
		}
		catch
		{
			return new List<MaintenanceHistoryItemDto>();
		}
	}

	public async Task<byte[]> GetPdfReportAsync(string reportName)
	{
		string endPoint = $"reports/export/{reportName}/pdf";
		var response = await _httpClient.GetAsync(endPoint);
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsByteArrayAsync();
		}
		throw new Exception($"Error while trying to make a GET {endPoint}");
	}

	// Alias for GetPdfReportAsync to match usage in pages
	public async Task<byte[]> ExportToPdfAsync(string reportName)
	{
		return await GetPdfReportAsync(reportName);
	}

	private string QueryString<T>(T obj)
	{
		var type = typeof(T);
		if (!_typesCache.TryGetValue(type, out var properties))
		{
			properties = type.GetProperties();
			_typesCache[type] = properties;
		}

		var queryParameters = properties
			.Where(p => p.GetValue(obj) != null)
			.Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(obj)!.ToString()!)}");

		return "?" + string.Join("&", queryParameters);
	}
}