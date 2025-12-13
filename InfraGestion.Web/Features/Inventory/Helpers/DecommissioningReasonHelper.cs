using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.Helpers;

public static class DecommissioningReasonHelper
{
    public static string GetReasonDisplayName(DecommissioningReason reason)
    {
        return reason switch
        {
            DecommissioningReason.IrreparableTechnicalFailure => "Falla Técnica Irreparable",
            DecommissioningReason.TechnologicalObsolescence => "Obsolescencia Tecnológica",
            DecommissioningReason.EOL => "Fin de Vida Útil",
            DecommissioningReason.ExcessiveRepairCost => "Costo Excesivo de Reparación",
            DecommissioningReason.SeverePhysicalDamage => "Daño Físico Severo",
            DecommissioningReason.IncompatibilityWithNewInfrastructure => "Incompatibilidad con Nueva Infraestructura",
            DecommissioningReason.PlannedTechnologyUpgrade => "Actualización Tecnológica Planificada",
            DecommissioningReason.TheftOrLoss => "Robo o Pérdida",
            DecommissioningReason.CustomerContractTermination => "Terminación de Contrato de Cliente",
            _ => reason.ToString()
        };
    }

    public static string GetReasonBadgeClass(DecommissioningReason reason)
    {
        return reason switch
        {
            DecommissioningReason.IrreparableTechnicalFailure => "reason-technical",
            DecommissioningReason.TechnologicalObsolescence => "reason-obsolescence",
            DecommissioningReason.EOL => "reason-eol",
            DecommissioningReason.ExcessiveRepairCost => "reason-cost",
            DecommissioningReason.SeverePhysicalDamage => "reason-damage",
            DecommissioningReason.IncompatibilityWithNewInfrastructure => "reason-incompatibility",
            DecommissioningReason.PlannedTechnologyUpgrade => "reason-upgrade",
            DecommissioningReason.TheftOrLoss => "reason-theft",
            DecommissioningReason.CustomerContractTermination => "reason-contract",
            _ => "reason-default"
        };
    }
}
