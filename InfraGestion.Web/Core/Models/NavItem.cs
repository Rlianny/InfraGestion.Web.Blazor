namespace InfraGestion.Web.Core.Models;

/// <summary>
/// Representa un item de navegación en el menú lateral
/// </summary>
public class NavItem
{
    /// <summary>
    /// Título que se muestra en el menú
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Ruta de navegación (href)
    /// </summary>
    public string Route { get; set; } = string.Empty;
    
    /// <summary>
    /// SVG path del icono (el contenido del path, no el SVG completo)
    /// </summary>
    public string IconPath { get; set; } = string.Empty;
    
    /// <summary>
    /// Roles que pueden ver este item. Si está vacío, todos pueden verlo.
    /// </summary>
    public string[] AllowedRoles { get; set; } = [];
    
    /// <summary>
    /// Si debe hacer match exacto en la ruta (NavLinkMatch.All)
    /// </summary>
    public bool ExactMatch { get; set; } = false;
    
    /// <summary>
    /// Orden de aparición en el menú (menor = más arriba)
    /// </summary>
    public int Order { get; set; } = 0;
    
    /// <summary>
    /// Si este item es un separador (espaciador) en lugar de un enlace
    /// </summary>
    public bool IsSpacer { get; set; } = false;
}
