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

	public async Task<List<DeviceReportDto>> GenerateInventoryReportAsync(DeviceReportFilterDto filter)
	{
		string endPoint = "reports/inventory";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DeviceReportDto>>>(endPoint + QueryString(filter))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<DeviceReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<DecommissioningReportDto>> GenerateDecommissioningReportAsync(DecommissioningReportFilterDto filter)
	{
		string endPoint = "reports/decommissionings";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DecommissioningReportDto>>>(endPoint + QueryString(filter))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<DecommissioningReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
	{
		string endPoint = "reports/personnel-effectiveness";
		
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PersonnelEffectivenessReportDto>>>(endPoint + QueryString(criteria))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<PersonnelEffectivenessReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
	{
		string endPoint = "reports/equipment-replacement";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DeviceReplacementReportDto>>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<DeviceReplacementReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<DepartmentTransferReportDto>> GenerateDepartmentTransferReportAsync(string? departmentId = null)
	{
		string endPoint = "reports/department-transfer";
		if (!string.IsNullOrEmpty(departmentId))
		{
			endPoint += $"?departmentId={departmentId}";
		}
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartmentTransferReportDto>>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<DepartmentTransferReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
	{
		string endPoint = "reports/correlation-analysis";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CorrelationAnalysisReportDto>>>(endPoint)
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<CorrelationAnalysisReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<List<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
	{
		string endPoint = "reports/bonus-determination";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BonusDeterminationReportDto>>>(endPoint + QueryString(criteria))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new List<BonusDeterminationReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

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
