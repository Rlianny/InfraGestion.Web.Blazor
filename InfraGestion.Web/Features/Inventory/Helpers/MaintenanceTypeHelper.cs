namespace InfraGestion.Web.Features.Inventory.Helpers;

using InfraGestion.Web.Features.Inventory.Models;

public static class MaintenanceTypeHelper
{
    public static string GetTypeDisplayName(MaintenanceType type)
    {
        return type switch
        {
            MaintenanceType.Preventivo => "Preventivo",
            MaintenanceType.Predictivo => "Predictivo",
            MaintenanceType.Correctivo => "Correctivo",
            _ => type.ToString()
        };
    }

    public static string GetTypeBadgeClass(MaintenanceType type)
    {
        return type switch
        {
            MaintenanceType.Preventivo => "badge-preventive",
            MaintenanceType.Predictivo => "badge-predictive",
            MaintenanceType.Correctivo => "badge-corrective",
            _ => "badge-default"
        };
    }
}
