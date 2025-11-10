namespace InfraGestion.Web.Features.Auth.DTOs;

/// <summary>
/// Generic Wrapper used by the API for all responses 
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; set; }
}