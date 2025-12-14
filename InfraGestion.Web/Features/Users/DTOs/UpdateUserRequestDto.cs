namespace InfraGestion.Web.Features.Users.DTOs;

public record UpdateUserRequestDto
{
    public required int UserId { get; init; }
    public string? FullName { get; init; }
    public string? Role { get; init; }
    public string? DepartmentName { get; init; }
    public bool? IsActive { get; init; }
    public int? YearsOfExperience { get; init; }
    public string? Specialty { get; init; }
    public string? Password { get; init; }
}