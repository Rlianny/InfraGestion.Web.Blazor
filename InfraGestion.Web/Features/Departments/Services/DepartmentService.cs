using InfraGestion.Web.Features.Users.Services;
using InfraGestion.Web.Features.Inventory.Services;

namespace InfraGestion.Web.Features.Departments.Services;

/// <summary>
/// Service to manage departments dynamically from existing data
/// Until API provides Department endpoints
/// </summary>
public class DepartmentService
{
    private readonly UserService _userService;
    private readonly DeviceService _deviceService;

    public DepartmentService(UserService userService, DeviceService deviceService)
    {
        _userService = userService;
        _deviceService = deviceService;
    }

    /// <summary>
    /// Gets all unique department names from users and devices
    /// </summary>
    public async Task<List<string>> GetAllDepartmentNamesAsync()
    {
        try
        {
            var departmentNames = new HashSet<string>();

            // Get departments from users
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
            {
                if (!string.IsNullOrWhiteSpace(user.Department))
                {
                    departmentNames.Add(user.Department);
                }
            }

            // Get departments from devices
            var devices = await _deviceService.GetAllDevicesAsync();
            foreach (var device in devices)
            {
                if (!string.IsNullOrWhiteSpace(device.Location))
                {
                    departmentNames.Add(device.Location);
                }
            }

            var sortedDepartments = departmentNames.OrderBy(d => d).ToList();

            Console.WriteLine($"✅ Found {sortedDepartments.Count} unique departments");

            return sortedDepartments;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"🔴 Error fetching departments: {ex.Message}");
            return new List<string>();
        }
    }

    /// <summary>
    /// Checks if a department exists
    /// </summary>
    public async Task<bool> DepartmentExistsAsync(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
            return false;

        var departments = await GetAllDepartmentNamesAsync();
        return departments.Any(d => d.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
    }

}