namespace InfraGestion.Web.Core.Constants;

/// <summary>
/// Constantes de roles del sistema
/// </summary>
public static class Roles
{
    public const string Admin = "Administrator";
    public const string Technician = "Technician";
    public const string Specialist = "Specialist";
    public const string User = "User";
    
    /// <summary>
    /// Todos los roles disponibles en el sistema
    /// </summary>
    public static readonly string[] All = [Admin, Technician, Specialist, User];
}
