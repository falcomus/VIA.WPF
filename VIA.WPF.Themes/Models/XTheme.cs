// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTheme.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Themes;

#region ### Class XTheme ###
/// <summary>
/// Represents a complete VIA.WPF theme.
/// </summary>
public sealed class XTheme
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the theme name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the foreground color pair for the theme mode button.
    /// </summary>
    public required XThemeModeColor ThemeModeForeground { get; init; }

    /// <summary>
    /// Gets or sets the control border color pair.
    /// </summary>
    public required XThemeModeColor ControlBorder { get; init; }

    /// <summary>
    /// Gets or sets the stronger control border color pair.
    /// </summary>
    public required XThemeModeColor ControlBorderStrong { get; init; }

    /// <summary>
    /// Gets or sets the panel border color pair.
    /// </summary>
    public required XThemeModeColor PanelBorder { get; init; }

    /// <summary>
    /// Gets or sets the stronger panel border color pair.
    /// </summary>
    public required XThemeModeColor PanelBorderStrong { get; init; }

    /// <summary>
    /// Gets or sets the focus border color pair.
    /// </summary>
    public required XThemeModeColor FocusBorder { get; init; }


    /// <summary>
    /// Gets or sets the selected item background color pair.
    /// </summary>
    public required XThemeModeColor SelectionBackground { get; init; }

    /// <summary>
    /// Gets or sets the selected item border color pair.
    /// </summary>
    public required XThemeModeColor SelectionBorder { get; init; }

    /// <summary>
    /// Gets or sets the selected item foreground color pair.
    /// </summary>
    public required XThemeModeColor SelectionForeground { get; init; }

    /// <summary>
    /// Gets or sets the general hover background color pair.
    /// </summary>
    public required XThemeModeColor HoverBackground { get; init; }

    /// <summary>
    /// Gets or sets the general hover border color pair.
    /// </summary>
    public required XThemeModeColor HoverBorder { get; init; }

    /// <summary>
    /// Gets or sets the general pressed background color pair.
    /// </summary>
    public required XThemeModeColor PressedBackground { get; init; }

    /// <summary>
    /// Gets or sets the general pressed border color pair.
    /// </summary>
    public required XThemeModeColor PressedBorder { get; init; }

    /// <summary>
    /// Gets or sets the disabled background color pair.
    /// </summary>
    public required XThemeModeColor DisabledBackground { get; init; }

    /// <summary>
    /// Gets or sets the disabled foreground color pair.
    /// </summary>
    public required XThemeModeColor DisabledForeground { get; init; }

    /// <summary>
    /// Gets or sets the disabled border color pair.
    /// </summary>
    public required XThemeModeColor DisabledBorder { get; init; }

    /// <summary>
    /// Gets or sets the data grid line color pair.
    /// </summary>
    public required XThemeModeColor GridLine { get; init; }

    /// <summary>
    /// Gets or sets the data grid header background color pair.
    /// </summary>
    public required XThemeModeColor GridHeaderBackground { get; init; }

    /// <summary>
    /// Gets or sets the data grid header foreground color pair.
    /// </summary>
    public required XThemeModeColor GridHeaderForeground { get; init; }

    /// <summary>
    /// Gets or sets the input background color pair.
    /// </summary>
    public required XThemeModeColor InputBackground { get; init; }

    /// <summary>
    /// Gets or sets the input border color pair.
    /// </summary>
    public required XThemeModeColor InputBorder { get; init; }

    /// <summary>
    /// Gets or sets the input placeholder foreground color pair.
    /// </summary>
    public required XThemeModeColor InputPlaceholder { get; init; }

    /// <summary>
    /// Gets or sets the read-only input background color pair.
    /// </summary>
    public required XThemeModeColor InputReadOnlyBackground { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header background color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelHeaderBackground { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header foreground color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelHeaderForeground { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header border color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelHeaderBorder { get; init; }

    /// <summary>
    /// Gets or sets the toolbar background color pair.
    /// </summary>
    public required XThemeModeColor ToolbarBackground { get; init; }

    /// <summary>
    /// Gets or sets the toolbar foreground color pair.
    /// </summary>
    public required XThemeModeColor ToolbarForeground { get; init; }

    /// <summary>
    /// Gets or sets the toolbar secondary foreground color pair.
    /// </summary>
    public required XThemeModeColor ToolbarSecondaryForeground { get; init; }

    /// <summary>
    /// Gets or sets the toolbar border color pair.
    /// </summary>
    public required XThemeModeColor ToolbarBorder { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb background color pair.
    /// </summary>
    public required XThemeModeColor BreadcrumbBackground { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb foreground color pair.
    /// </summary>
    public required XThemeModeColor BreadcrumbForeground { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb secondary foreground color pair.
    /// </summary>
    public required XThemeModeColor BreadcrumbSecondaryForeground { get; init; }

    /// <summary>
    /// Gets or sets the color set for the primary token.
    /// </summary>
    public required XThemeColorSet Primary { get; init; }

    /// <summary>
    /// Gets or sets the color set for the background token.
    /// </summary>
    public required XThemeColorSet Background { get; init; }

    /// <summary>
    /// Gets or sets the color set for the surface token.
    /// </summary>
    public required XThemeColorSet Surface { get; init; }

    /// <summary>
    /// Gets or sets the color set for the border token.
    /// </summary>
    public required XThemeColorSet Border { get; init; }

    /// <summary>
    /// Gets or sets the color set for the accent token.
    /// </summary>
    public required XThemeColorSet Accent { get; init; }

    /// <summary>
    /// Gets or sets the color set for the success token.
    /// </summary>
    public required XThemeColorSet Success { get; init; }

    /// <summary>
    /// Gets or sets the color set for the warning token.
    /// </summary>
    public required XThemeColorSet Warning { get; init; }

    /// <summary>
    /// Gets or sets the color set for the danger token.
    /// </summary>
    public required XThemeColorSet Danger { get; init; }

    /// <summary>
    /// Gets or sets the color set for the info token.
    /// </summary>
    public required XThemeColorSet Info { get; init; }

    /// <summary>
    /// Gets or sets the tab header background color pair.
    /// </summary>
    public required XThemeModeColor TabHeaderBackground { get; init; }

    /// <summary>
    /// Gets or sets the tab header foreground color pair.
    /// </summary>
    public required XThemeModeColor TabHeaderForeground { get; init; }

    /// <summary>
    /// Gets or sets the tab header border color pair.
    /// </summary>
    public required XThemeModeColor TabHeaderBorder { get; init; }

    /// <summary>
    /// Gets or sets the tab item background color pair.
    /// </summary>
    public required XThemeModeColor TabItemBackground { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item background color pair.
    /// </summary>
    public required XThemeModeColor TabItemBackgroundHover { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item background color pair.
    /// </summary>
    public required XThemeModeColor TabItemBackgroundSelected { get; init; }

    /// <summary>
    /// Gets or sets the tab item foreground color pair.
    /// </summary>
    public required XThemeModeColor TabItemForeground { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item foreground color pair.
    /// </summary>
    public required XThemeModeColor TabItemForegroundHover { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item foreground color pair.
    /// </summary>
    public required XThemeModeColor TabItemForegroundSelected { get; init; }

    /// <summary>
    /// Gets or sets the tab item border color pair.
    /// </summary>
    public required XThemeModeColor TabItemBorder { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item border color pair.
    /// </summary>
    public required XThemeModeColor TabItemBorderHover { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item border color pair.
    /// </summary>
    public required XThemeModeColor TabItemBorderSelected { get; init; }

    /// <summary>
    /// Gets or sets the tab item underline color pair.
    /// </summary>
    public required XThemeModeColor TabItemUnderline { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item underline color pair.
    /// </summary>
    public required XThemeModeColor TabItemUnderlineSelected { get; init; }

    /// <summary>
    /// Gets or sets the tab content background color pair.
    /// </summary>
    public required XThemeModeColor TabContentBackground { get; init; }

    /// <summary>
    /// Gets or sets the tab content border color pair.
    /// </summary>
    public required XThemeModeColor TabContentBorder { get; init; }

    /// <summary>
    /// Gets or sets the tab action button foreground color pair.
    /// </summary>
    public required XThemeModeColor TabActionButtonForeground { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button foreground color pair.
    /// </summary>
    public required XThemeModeColor TabActionButtonForegroundHover { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button background color pair.
    /// </summary>
    public required XThemeModeColor TabActionButtonBackgroundHover { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel background color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelBackground { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel foreground color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelForeground { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel border color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelBorder { get; init; }

    /// <summary>
    /// Gets or sets the hovered navigation panel item background color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelItemHoverBackground { get; init; }

    /// <summary>
    /// Gets or sets the selected navigation panel item background color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelItemSelectedBackground { get; init; }

    /// <summary>
    /// Gets or sets the selected navigation panel item foreground color pair.
    /// </summary>
    public required XThemeModeColor NavigationPanelItemSelectedForeground { get; init; }
    #endregion
}
#endregion