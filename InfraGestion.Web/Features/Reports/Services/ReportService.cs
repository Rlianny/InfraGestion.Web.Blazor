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