namespace InfraGestion.Web.Features.Inventory.Helpers;

using InfraGestion.Web.Features.Inventory.Models;

public static class MaintenanceTypeHelper
{
    public static string GetTypeDisplayName(MaintenanceType type)
    {
        return type switch
        {
            MaintenanceType.Preventive => "Preventivo",
            MaintenanceType.Predictive => "Predictivo",
            MaintenanceType.Corrective => "Correctivo",
            _ => type.ToString()
        };
    }

    public static string GetTypeBadgeClass(MaintenanceType type)
    {
        return type switch
        {
            MaintenanceType.Preventive => "badge-preventive",
            MaintenanceType.Predictive => "badge-predictive",
            MaintenanceType.Corrective => "badge-corrective",
            _ => "badge-default"
        };
    }
}
