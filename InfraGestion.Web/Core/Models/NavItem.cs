namespace InfraGestion.Web.Core.Models;

public class NavItem
{
    public string Title { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
    public string[] AllowedRoles { get; set; } = [];

    /// <summary>
    /// If must do match exactly on the route (NavLinkMatch.All)
    /// </summary>
    public bool ExactMatch { get; set; } = false;

    /// <summary>
    /// Order to apparition on the menu (less = topper)
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// If this item is a spacer not a link
    /// </summary>
    public bool IsSpacer { get; set; } = false;
}
