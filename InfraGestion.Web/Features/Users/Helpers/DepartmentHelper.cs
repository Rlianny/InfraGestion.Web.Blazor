namespace InfraGestion.Web.Features.Users.Helpers;

public static class DepartmentHelper
{
    public static string FormatDepartmentForDisplay(string department)
    {
        // Convierte el nombre del departamento a formato multi-línea para visualización
        return department switch
        {
            "Análisis Forense y Respuesta a Incidentes" => "Análisis Forense y\nRespuesta a Incidentes",
            "Almacenamiento y Backup" => "Almacenamiento\ny Backup",
            "Infraestructura como Servicio" => "Infraestructura\ncomo Servicio",
            "Soporte a Nodos Remotos" => "Soporte a Nodos\nRemotos",
            "Reparación y Refabricación" => "Reparación y\nRefabricación",
            _ => department
        };
    }
}