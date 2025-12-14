using InfraGestion.Web.Features.Auth.DTOs;
using InfraGestion.Web.Features.Transfers.Models;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Http.Json;

namespace InfraGestion.Web.Features.Transfers.Services;

public class TransferService // TODO: Replace with real implementation
{
    private readonly HttpClient _httpClient;
    public TransferService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Transfer>> GetAllTransfersAsync()
    {       
        string endPoint = "transfers/pending";
        var response =  await _httpClient.GetFromJsonAsync<ApiResponse<List<Transfer>>>(endPoint)??throw new Exception($"Error while trying to make a GET {endPoint}"); ;
        if (response.Success)
        {
            return response.Data!;
        }
        throw new Exception(string.Join("\n", response.Errors));

    }

    public async Task<Transfer?> GetTransferByIdAsync(int id)
    {
        var endPoint = $"/transfers{id}";
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

    public async Task<List<(string Id, string Name)>> GetDevicesAsync()
    {
        HashSet<(string Id, string Name)> devices = new();
        var transfers = await GetAllTransfersAsync();
        foreach (var transfer in transfers)
        {

            devices.Add(($"DEV{transfer.Id}", transfer.DeviceName));
        }
        return devices.ToList();
    }

    public async Task<List<string>> GetLocationsAsync()
    {
       HashSet<string> locations = new();
       var transfers = await GetAllTransfersAsync();
       foreach (var transfer in transfers)
       {
           locations.Add(transfer.Destination);
       }
        return locations.ToList();
    }
}
