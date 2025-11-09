namespace InfraGestion.Web.Features.Inventory.Helpers;

using InfraGestion.Web.Features.Inventory.Models;

public static class DeviceTypeHelper
{
    public static string GetTypeDisplayName(DeviceType type)
    {
        return type switch
        {
            DeviceType.ConnectivityAndNetwork => "Conectividad y Red",
            DeviceType.ComputingAndIT => "Cómputo e Informática",
            DeviceType.ElectricalInfrastructureAndSupport => "Infraestructura y Soporte",
            DeviceType.CommunicationsAndTransmission => "Comunicaciones y Transmisión",
            DeviceType.DiagnosticAndMeasurement => "Diagnóstico y Medición",
            _ => type.ToString()
        };
    }

    public static string GetTypeBadgeClass(DeviceType type)
    {
        return type switch
        {
            DeviceType.ConnectivityAndNetwork => "badge-connectivity",
            DeviceType.ComputingAndIT => "badge-computing",
            DeviceType.ElectricalInfrastructureAndSupport => "badge-electrical",
            DeviceType.CommunicationsAndTransmission => "badge-communications",
            DeviceType.DiagnosticAndMeasurement => "badge-diagnostic",
            _ => "badge-default"
        };
    }
}