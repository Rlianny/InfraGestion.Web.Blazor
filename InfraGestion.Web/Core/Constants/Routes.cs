namespace InfraGestion.Web.Core.Constants;

/// <summary>
/// API Routes constants - v2.1
/// Updated: December 12, 2025
/// </summary>
public static class ApiRoutes
{
    // ==========================================
    // AUTH CONTROLLER
    // ==========================================
    public static class Auth
    {
        private const string Base = "auth";

        public const string Login = $"{Base}/login";
        public const string ChangePassword = $"{Base}/change-password";
        public const string RefreshToken = $"{Base}/refresh-token";

        // v2.1 Changes - Now include userId in path
        public static string GetCurrentUser(int userId) => $"{Base}/me/{userId}";

        public static string Logout(int userId) => $"{Base}/logout/{userId}";
    }

    // ==========================================
    // USERS CONTROLLER
    // ==========================================
    public static class Users
    {
        private const string Base = "Users";

        // v2.1 Changes - Now include administratorId/currentUserId in path

        /// <summary>POST /Users/administrator/{administratorId}</summary>
        public static string CreateUser(int administratorId) =>
            $"{Base}/administrator/{administratorId}";

        /// <summary>GET /Users/user/{currentUserId}</summary>
        public static string GetAllUsers(int currentUserId) => $"{Base}/user/{currentUserId}";

        /// <summary>GET /Users/{id}</summary>
        public static string GetUserById(int id) => $"{Base}/{id}";

        /// <summary>GET /Users/user/{currentUserId}/department/{departmentId}</summary>
        public static string GetUsersByDepartment(int currentUserId, int departmentId) =>
            $"{Base}/user/{currentUserId}/department/{departmentId}";

        /// <summary>GET /Users/user/{currentUserId}/role/{role}</summary>
        public static string GetUsersByRole(int currentUserId, string role) =>
            $"{Base}/user/{currentUserId}/role/{role}";

        /// <summary>PUT /Users/administrator/{administratorId}/{id}</summary>
        public static string UpdateUser(int administratorId, int id) =>
            $"{Base}/administrator/{administratorId}/{id}";

        /// <summary>POST /Users/administrator/{administratorId}/{id}/deactivate</summary>
        public static string DeactivateUser(int administratorId, int id) =>
            $"{Base}/administrator/{administratorId}/{id}/deactivate";

        /// <summary>POST /Users/administrator/{administratorId}/{id}/activate</summary>
        public static string ActivateUser(int administratorId, int id) =>
            $"{Base}/administrator/{administratorId}/{id}/activate";

        /// <summary>GET /Users/user/{currentUserId}/department/{departmentId}/technicians</summary>
        public static string GetTechnicians(int currentUserId, int departmentId) =>
            $"{Base}/user/{currentUserId}/department/{departmentId}/technicians";
    }

    // ==========================================
    // DEVICES CONTROLLER
    // ==========================================
    public static class Devices
    {
        private const string Base = "api/devices";

        /// <summary>GET /api/devices - v2.0: userId removed, filtered by JWT</summary>
        public const string GetAllDevices = Base;

        /// <summary>GET /api/devices/sections/{sectionId} - v2.0: userId removed</summary>
        public static string GetDevicesBySection(int sectionId) => $"{Base}/sections/{sectionId}";

        /// <summary>GET /api/devices/my-section - v2.0: simplified route</summary>
        public const string GetMySectionDevices = $"{Base}/my-section";

        /// <summary>GET /api/devices/{id}</summary>
        public static string GetDeviceById(int id) => $"{Base}/{id}";

        /// <summary>POST /api/devices</summary>
        public const string CreateDevice = Base;

        /// <summary>PUT /api/devices/{id} - v2.0: ID now in route</summary>
        public static string UpdateDevice(int id) => $"{Base}/{id}";

        /// <summary>DELETE /api/devices/{id}</summary>
        public static string DeleteDevice(int id) => $"{Base}/{id}";

        /// <summary>POST /api/devices/{id}/disable</summary>
        public static string DisableDevice(int id) => $"{Base}/{id}/disable";
    }

    // ==========================================
    // INSPECTIONS CONTROLLER (new in v2.1)
    // ==========================================
    public static class Inspections
    {
        private const string Base = "api/inspections";

        /// <summary>GET /api/inspections/technician/{technicianId}/requests</summary>
        public static string GetTechnicianRequests(int technicianId) =>
            $"{Base}/technician/{technicianId}/requests";

        /// <summary>GET /api/inspections/technician/{technicianId}/pending</summary>
        public static string GetPendingInspections(int technicianId) =>
            $"{Base}/technician/{technicianId}/pending";

        /// <summary>GET /api/inspections/admin/{adminId}/revised-devices</summary>
        public static string GetRevisedDevices(int adminId) =>
            $"{Base}/admin/{adminId}/revised-devices";

        /// <summary>GET /api/inspections/admin/{adminId}/requests</summary>
        public static string GetAdminRequests(int adminId) => $"{Base}/admin/{adminId}/requests";

        /// <summary>POST /api/inspections/decision</summary>
        public const string ProcessDecision = $"{Base}/decision";

        /// <summary>POST /api/inspections/assign</summary>
        public const string AssignInspection = $"{Base}/assign";
    }

    // ==========================================
    // ORGANIZATION CONTROLLER
    // ==========================================
    public static class Organization
    {
        private const string Base = "organization";

        public const string GetSections = $"{Base}/sections";

        public static string GetSectionById(int id) => $"{Base}/sections/{id}";

        public static string GetDepartmentById(int id) => $"{Base}/departments/{id}";

        public static string GetDepartmentsBySection(int sectionId) =>
            $"{Base}/sections/{sectionId}/departments";
    }

    // ==========================================
    // PERSONNEL (TECHNICIANS) CONTROLLER
    // ==========================================
    public static class Personnel
    {
        private const string Base = "personnel";

        public const string GetTechnicians = $"{Base}/technicians";

        public static string GetTechnicianById(int id) => $"{Base}/technician/{id}";

        public static string GetTechnicianDetails(int id) => $"{Base}/technician/{id}/details";

        public static string GetTechnicianBonuses(int id) => $"{Base}/bonuses/{id}";

        public static string GetTechnicianPenalties(int id) => $"{Base}/penalties/{id}";
    }

    // ==========================================
    // TRANSFERS CONTROLLER
    // ==========================================
    public static class Transfers
    {
        private const string Base = "transfers";

        public const string GetAll = Base;

        public static string GetById(int id) => $"{Base}/{id}";

        public const string Create = Base;
    }

    // ==========================================
    // DECOMMISSIONING CONTROLLER
    // ==========================================
    public static class Decommissioning
    {
        private const string Base = "decommissioning";

        /// <summary>GET /decommissioning/requests</summary>
        public const string GetAllRequests = $"{Base}/requests";

        /// <summary>GET /decommissioning/requests/{id}</summary>
        public static string GetRequestById(int id) => $"{Base}/requests/{id}";

        /// <summary>POST /decommissioning/requests</summary>
        public const string CreateRequest = $"{Base}/requests";

        /// <summary>PUT /decommissioning/requests</summary>
        public const string UpdateRequest = $"{Base}/requests";

        /// <summary>DELETE /decommissioning/requests/{id}</summary>
        public static string DeleteRequest(int id) => $"{Base}/requests/{id}";
    }

    // ==========================================
    // MAINTENANCE CONTROLLER
    // ==========================================
    public static class Maintenance
    {
        private const string Base = "maintenance";

        /// <summary>POST /maintenance</summary>
        public const string Create = Base;
    }
}
