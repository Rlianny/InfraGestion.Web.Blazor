using InfraGestion.Web.Features.Users.Models;

namespace InfraGestion.Web.Features.Users.Services;

public class UserService
{
    private List<User> _users = new()
    {
        new User
        {
            Id = 1,
            Name = "Elena Morales",
            Department = "Análisis Forense y\nRespuesta a Incidentes",
            Role = UserRole.Director,
            Status = UserStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-6)
        },
        new User
        {
            Id = 2,
            Name = "Carmen Sánchez",
            Department = "Almacenamiento\ny Backup",
            Role = UserRole.Administrator,
            Status = UserStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-4)
        },
        new User
        {
            Id = 3,
            Name = "Isabel Castro",
            Department = "Infraestructura\ncomo Servicio",
            Role = UserRole.SectionManager,
            Status = UserStatus.Inactive,
            CreatedAt = DateTime.Now.AddMonths(-8)
        },
        new User
        {
            Id = 4,
            Name = "Jorge Silva",
            Department = "Soporte a Nodos\nRemotos",
            Role = UserRole.Technician,
            Status = UserStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-2)
        },
        new User
        {
            Id = 5,
            Name = "Carlos Ruiz",
            Department = "Reparación y\nRefabricación",
            Role = UserRole.Logistician,
            Status = UserStatus.Active,
            CreatedAt = DateTime.Now.AddMonths(-3)
        }
    };

    private int _nextId = 6;

    public Task<List<User>> GetAllUsersAsync()
    {
        return Task.FromResult(_users.ToList());
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<List<User>> SearchUsersAsync(string searchTerm, UserRole? roleFilter, UserStatus? statusFilter)
    {
        var query = _users.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                u.Id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Department.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (roleFilter.HasValue)
        {
            query = query.Where(u => u.Role == roleFilter.Value);
        }

        if (statusFilter.HasValue)
        {
            query = query.Where(u => u.Status == statusFilter.Value);
        }

        return Task.FromResult(query.ToList());
    }

    public Task<User> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = _nextId++,
            Name = request.Name,
            Department = request.Department,
            Role = request.Role,
            Status = UserStatus.Active,
            Password = request.Password,
            CreatedAt = DateTime.Now
        };

        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User?> UpdateUserAsync(UpdateUserRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Id == request.Id);
        if (user == null)
            return Task.FromResult<User?>(null);

        user.Name = request.Name;
        user.Department = request.Department;
        user.Role = request.Role;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = request.Password;
        }

        return Task.FromResult<User?>(user);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Task.FromResult(false);

        _users.Remove(user);
        return Task.FromResult(true);
    }

    public Task<bool> ToggleUserStatusAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Task.FromResult(false);

    user.Status = user.Status == UserStatus.Active ? UserStatus.Inactive : UserStatus.Active;
    return Task.FromResult(true);
    }
}