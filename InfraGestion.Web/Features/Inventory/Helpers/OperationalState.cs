namespace InfraGestion.Web.Features.Inventory.Helpers;

using InfraGestion.Web.Features.Inventory.Models;

public static class OperationalStateHelper
{
    public static string GetStateDisplayName(OperationalState state)
    {
        return state switch
        {
            OperationalState.Operational => "Operativo",
            OperationalState.UnderMaintenance => "En Mantenimiento",
            OperationalState.Decommissioned => "De Baja",
            OperationalState.BeingTransferred => "Siendo Trasladado",
            _ => state.ToString()
        };
    }

    public static string GetStateBadgeClass(OperationalState state)
    {
        return state switch
        {
            OperationalState.Operational => "status-operational",
            OperationalState.UnderMaintenance => "status-maintenance",
            OperationalState.Decommissioned => "status-decommissioned",
            OperationalState.BeingTransferred => "status-transferred",
            _ => "status-default"
        };
    }
}