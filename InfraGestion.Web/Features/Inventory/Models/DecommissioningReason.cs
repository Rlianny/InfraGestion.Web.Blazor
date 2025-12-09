namespace InfraGestion.Web.Features.Inventory.Models;

public enum DecommissioningReason
{
    IrreparableTechnicalFailure,
    TechnologicalObsolescence,
    EOL,
    ExcessiveRepairCost,
    SeverePhysicalDamage,
    IncompatibilityWithNewInfrastructure,
    PlannedTechnologyUpgrade,
    TheftOrLoss,
    CustomerContractTermination
}
