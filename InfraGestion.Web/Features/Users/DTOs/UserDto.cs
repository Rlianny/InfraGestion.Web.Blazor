namespace InfraGestion.Web.Features.Users.DTOs;

public record UserDto
{
    public required int UserId { get; init; }
    public required string Username { get; init; }
    public required string FullName { get; init; }
    public required string Role { get; init; }
    public required int DepartmentId { get; init; }
    public required string DepartmentName { get; init; }
    public required bool IsActive { get; init; }
    public required DateTime CreatedAt { get; init; }
    
    // Optionals
    public int? YearsOfExperience { get; init; }
    public string? Specialty { get; init; }
}