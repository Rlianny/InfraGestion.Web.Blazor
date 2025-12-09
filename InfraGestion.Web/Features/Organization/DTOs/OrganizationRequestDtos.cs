namespace InfraGestion.Web.Features.Organization.DTOs;

public class SectionRequestDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? SectionManager { get; set; }
}

public class DepartmentRequestDto
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SectionManagerDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
