using InfraGestion.Web.Core.Constants;
using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Transfers.DTOs;
using InfraGestion.Web.Features.Transfers.Models;
using System.Net.Http.Json;

namespace InfraGestion.Web.Features.Transfers.Services;

public class TransferService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public TransferService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<Transfer>> GetAllTransfersAsync()
    {       
        string endPoint = ApiRoutes.Transfers.GetPending;
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Transfer>>>(endPoint) 
            ?? throw new Exception($"Error while trying to make a GET {endPoint}");
        
        if (response.Success)
        {
            return response.Data ?? new List<Transfer>();
        }
        throw new Exception(string.Join("\n", response.Errors));
    }

    /// <summary>
    /// Get pending transfers for a specific logistician
    /// GET /transfers/pending/logistician/{logisticId}
    /// </summary>
    public async Task<List<Transfer>> GetPendingTransfersByLogisticianAsync(int logisticId)
    {
        try
        {
            string endPoint = ApiRoutes.Transfers.GetPendingByLogistician(logisticId);
            Console.WriteLine($"[DEBUG] TransferService - Calling endpoint: {endPoint}");
            
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Transfer>>>(endPoint);
            
            if (response?.Success == true && response.Data != null)
            {
                Console.WriteLine($"[DEBUG] TransferService - Got {response.Data.Count} pending transfers");
                return response.Data;
            }
            
            Console.WriteLine($"[DEBUG] TransferService - No pending transfers or error");
            return new List<Transfer>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] TransferService.GetPendingTransfersByLogisticianAsync: {ex.Message}");
            return new List<Transfer>();
        }
    }

    /// <summary>
    /// Confirm reception of a transfer
    /// POST /transfers/confirmations/{transferId}
    /// </summary>
    public async Task<bool> ConfirmReceptionAsync(int transferId)
    {
        try
        {
            string endPoint = ApiRoutes.Transfers.ConfirmReception(transferId);
            Console.WriteLine($"[DEBUG] TransferService - Confirming reception at: {endPoint}");
            
            var response = await _httpClient.PostAsync(endPoint, null);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[DEBUG] TransferService - Reception confirmed successfully");
                return true;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ERROR] TransferService - ConfirmReception failed: {response.StatusCode} - {content}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] TransferService.ConfirmReceptionAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<Transfer?> GetTransferByIdAsync(int id)
    {
        var endPoint = $"/transfers/{id}";
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<Transfer>>(endPoint)?? throw new Exception($"Error while trying to make a GET {endPoint}");
        if (response.Success)
        {
            return response.Data!;
        }
        throw new Exception(string.Join("\n", response.Errors));
    }

    public async Task CreateTransferAsync(CreateTransferRequest request)
    {
        string endPoint = "/transfers";
        var response = await _httpClient.PostAsJsonAsync(endPoint, request);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>() ?? throw new Exception($"Error while trying to make a POST {endPoint}");
        if (!result.Success)
        {
            throw new Exception(string.Join("\n", result.Errors));
        }
        
    }

    public async Task UpdateTransferAsync(UpdateTransferRequest request)
    {
        string endPoint = $"transfers";
        var response =  await _httpClient.PutAsJsonAsync(endPoint, request);
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<string?>>() ?? throw new Exception($"Error while trying to make a PUT {endPoint}");
        if (!content.Success)
        {
            throw new Exception(string.Join("\n", content.Errors));
        }
    }

    public async Task<bool> DeleteTransferAsync(int id)
    {
        string endPoint = $"transfers/delete/{id}";
        var response = await _httpClient.DeleteFromJsonAsync<ApiResponse<string?>>(endPoint)??throw new Exception($"Error while tryng to make PUT{endPoint}");
        if (!response.Success)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Initiate a new transfer request (for Section Managers)
    /// POST /transfers
    /// </summary>
    public async Task<bool> InitiateTransferAsync(InitiateTransferDto request)
    {
        try
        {
            string endPoint = "transfers";
            Console.WriteLine($"[DEBUG] TransferService.InitiateTransferAsync - Calling POST {endPoint}");
            Console.WriteLine($"[DEBUG] Request: DeviceName={request.DeviceName}, DestinationSection={request.DestinationSectionName}, Receiver={request.DeviceReceiverUsername}, Date={request.TransferDate}");
            
            var response = await _httpClient.PostAsJsonAsync(endPoint, request);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[DEBUG] TransferService.InitiateTransferAsync - Transfer initiated successfully");
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ERROR] TransferService.InitiateTransferAsync failed: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] TransferService.InitiateTransferAsync: {ex.Message}");
            return false;
        }
    }

}
