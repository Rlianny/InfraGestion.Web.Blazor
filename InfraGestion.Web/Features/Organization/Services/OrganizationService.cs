using InfraGestion.Web.Features.Organization.Models;

namespace InfraGestion.Web.Features.Organization.Services;

public class OrganizationService
{
    private readonly HttpClient _httpClient;

    // Mock data for Sections (divisiones principales de la empresa)
    private static List<Section> _mockSections = new()
    {
        new Section
        {
            Id = 1,
            Name = "Operaciones de Red Corporativa",
            ManagerName = "Sofía Ramírez",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-12)
        },
        new Section
        {
            Id = 2,
            Name = "Infraestructura de Centro de Datos (Data Center)",
            ManagerName = "Alejandro Torres",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-10)
        },
        new Section
        {
            Id = 3,
            Name = "Soporte Técnico en Campo",
            ManagerName = "Ricardo Díaz",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-8)
        }
    };

    // Mock data for Departments (subdivisiones de las secciones)
    private static List<Department> _mockDepartments = new()
    {
        new Department
        {
            Id = 1,
            Name = "Conmutación y Enrutamiento Avanzado",
            SectionId = 1,
            SectionName = "Operaciones de Red Corporativa",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-11)
        },
        new Department
        {
            Id = 2,
            Name = "Seguridad Perimetral y Firewalls",
            SectionId = 1,
            SectionName = "Operaciones de Red Corporativa",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-11)
        },
        new Department
        {
            Id = 3,
            Name = "Almacenamiento y Backup",
            SectionId = 2,
            SectionName = "Infraestructura de Centro de Datos (Data Center)",
            Status = OrganizationStatus.Inactive,
            CreatedAt = DateTime.Now.AddMonths(-9)
        },
        new Department
        {
            Id = 4,
            Name = "Instalaciones y Activaciones",
            SectionId = 3,
            SectionName = "Soporte Técnico en Campo",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-7)
        },
        new Department
        {
            Id = 5,
            Name = "Soporte a Nodos Remotos",
            SectionId = 3,
            SectionName = "Soporte Técnico en Campo",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-6)
        }
    };

    public OrganizationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // ==================== SECTION METHODS ====================
    // Secciones: Divisiones principales de la empresa (tienen Responsable)

    public Task<List<Section>> GetAllSectionsAsync()
    {
        return Task.FromResult(_mockSections.ToList());
    }

    public Task<Section?> GetSectionByIdAsync(int id)
    {
        var section = _mockSections.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(section);
    }

    public Task<Section> CreateSectionAsync(CreateSectionRequest request)
    {
        var newSection = new Section
        {
            Id = _mockSections.Any() ? _mockSections.Max(s => s.Id) + 1 : 1,
            Name = request.Name,
            ManagerName = request.ManagerName,
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now
        };
        _mockSections.Add(newSection);
        return Task.FromResult(newSection);
    }

    public Task<Section?> UpdateSectionAsync(UpdateSectionRequest request)
    {
        var section = _mockSections.FirstOrDefault(s => s.Id == request.Id);
        if (section != null)
        {
            section.Name = request.Name;
            section.ManagerName = request.ManagerName;
            section.Status = request.Status;
        }
        return Task.FromResult(section);
    }

    public Task<bool> ToggleSectionStatusAsync(int id)
    {
        var section = _mockSections.FirstOrDefault(s => s.Id == id);
        if (section != null)
        {
            section.Status = section.Status == OrganizationStatus.Active
                ? OrganizationStatus.Inactive
                : OrganizationStatus.Active;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    // Get section names for dropdowns (used in Department forms)
    public Task<List<(int Id, string Name)>> GetSectionNamesAsync()
    {
        var result = _mockSections
            .Where(s => s.Status == OrganizationStatus.Active)
            .Select(s => (s.Id, s.Name))
            .ToList();
        return Task.FromResult(result);
    }

    // ==================== DEPARTMENT METHODS ====================
    // Departamentos: Subdivisiones de las secciones

    public Task<List<Department>> GetAllDepartmentsAsync()
    {
        return Task.FromResult(_mockDepartments.ToList());
    }

    public Task<Department?> GetDepartmentByIdAsync(int id)
    {
        var department = _mockDepartments.FirstOrDefault(d => d.Id == id);
        return Task.FromResult(department);
    }

    public Task<Department> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        var section = _mockSections.FirstOrDefault(s => s.Id == request.SectionId);
        var newDepartment = new Department
        {
            Id = _mockDepartments.Any() ? _mockDepartments.Max(d => d.Id) + 1 : 1,
            Name = request.Name,
            SectionId = request.SectionId,
            SectionName = section?.Name ?? "Desconocida",
            Status = OrganizationStatus.Active,
            CreatedAt = DateTime.Now
        };
        _mockDepartments.Add(newDepartment);
        return Task.FromResult(newDepartment);
    }

    public Task<Department?> UpdateDepartmentAsync(UpdateDepartmentRequest request)
    {
        var department = _mockDepartments.FirstOrDefault(d => d.Id == request.Id);
        if (department != null)
        {
            var section = _mockSections.FirstOrDefault(s => s.Id == request.SectionId);
            department.Name = request.Name;
            department.SectionId = request.SectionId;
            department.SectionName = section?.Name ?? department.SectionName;
            department.Status = request.Status;
        }
        return Task.FromResult(department);
    }

    public Task<bool> ToggleDepartmentStatusAsync(int id)
    {
        var department = _mockDepartments.FirstOrDefault(d => d.Id == id);
        if (department != null)
        {
            department.Status = department.Status == OrganizationStatus.Active
                ? OrganizationStatus.Inactive
                : OrganizationStatus.Active;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
