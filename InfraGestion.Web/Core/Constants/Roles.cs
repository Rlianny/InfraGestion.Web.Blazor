namespace InfraGestion.Web.Core.Constants;

/// <summary>
/// Constantes de roles del sistema
/// </summary>
public static class Roles
{
    public const string Admin = "Administrator";
    public const string Technician = "Technician";
    public const string Manager = "SectionManager";
    public const string Receiver = "EquipmentReceiver";
    public const string Director = "Director";
    public const string Logistician = "Logistician";
    public static readonly string[] All = [Admin, Technician, Manager, Receiver, Director, Logistician];
}
