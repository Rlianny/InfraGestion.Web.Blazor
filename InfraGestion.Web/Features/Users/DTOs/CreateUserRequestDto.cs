namespace InfraGestion.Web.Features.Users.DTOs;

public record CreateUserRequestDto
{
    public required string Username { get; init; }
    public required string FullName { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
    public required string? DepartmentName { get; init; }
    public int? YearsOfExperience { get; init; }
    public string? Specialty { get; init; }
}