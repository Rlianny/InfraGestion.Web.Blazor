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

	
	public async Task<Report<DecommissioningReportDto>> GenerateDecommissioningReportAsync()
	{
		string endPoint = "reports/decommissionings";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<DecommissioningReportDto>>>(endPoint);
		if (response is not null && response.Success)
		{
			return response.Data!;
		}
		return new Report<DecommissioningReportDto>();
    }

	public async Task<Report<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
	{
		string endPoint = "reports/personnel-effectiveness";
		
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<PersonnelEffectivenessReportDto>>>(endPoint + QueryString(criteria))
			?? throw new Exception($"Error while trying to make a GET {endPoint}");
		if (response.Success)
		{
			return response.Data ?? new Report<PersonnelEffectivenessReportDto>();
		}
		throw new Exception(string.Join("\n", response.Errors));
	}

	public async Task<Report<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
	{
		string endPoint = "reports/equipment-replacement";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<DeviceReplacementReportDto>>>(endPoint);
			
		if (response is not null &&response.Success)
		{
			return response.Data!;
		}
		return new();
	}

	public async Task<Report<SectionTransferReportDto>> GenerateDepartmentTransferReportAsync()
	{
		string endPoint = "reports/transfers";
        try
		{
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<SectionTransferReportDto>>>(endPoint);
				
            if (response.Success)
            {
                return response.Data ?? new Report<SectionTransferReportDto>();
            }
            throw new Exception(string.Join("\n", response.Errors));
        }
		catch(Exception ex)
		{
			throw new Exception($"Failed to generate department transfer report: {ex.Message}");
        }
	}

	public async Task<Report<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
	{
		string endPoint = "reports/correlation-analysis";

		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<CorrelationAnalysisReportDto>>>(endPoint);
            if (response is not null && response.Success)
            {
				return response.Data!;
            }
            return new Report<CorrelationAnalysisReportDto>();
    }

	public async Task<Report<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync()
	{
		string endPoint = "reports/bonus-determination";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<BonusDeterminationReportDto>>>(endPoint);
		if (response is not null &&response.Success)
		{
			return response.Data!;
		}
		return new();
	}

	public async Task<Report<SectionEquipmentDto>> GetDepartmentEquipmentAsync(int sectionId)
	{
		try
		{
			string endPoint = $"reports/section-equipment/{sectionId}";
			var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<SectionEquipmentDto>>>(endPoint);
			if (response is not null && response.Success)
			{
				return response.Data!;
			}
			return new Report<SectionEquipmentDto>() {};
		}
		catch
		{
			return new Report<SectionEquipmentDto>();
		}
	}

	public async Task<Report<DeviceMantainenceReportDto>> GetEquipmentMaintenanceHistoryAsync(int equipmentId)
	{
		
		string endPoint = $"reports/equipment-maintenances/{equipmentId}";
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<Report<DeviceMantainenceReportDto>>>(endPoint);
		if (response is not null && response.Success)
		{
			return response.Data!;
		}
		return new();
			
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
