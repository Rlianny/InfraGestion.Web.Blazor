namespace InfraGestion.Web.Features.Auth.DTOs;

public record LoginRequestDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}