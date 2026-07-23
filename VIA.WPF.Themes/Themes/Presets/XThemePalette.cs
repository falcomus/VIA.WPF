// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePalette.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePalette ###
/// <summary>
/// Describes the seed colors and optional detail color overrides used to generate a complete VIA.WPF theme preset.
/// </summary>
internal sealed class XThemePalette
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the theme name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the primary light mode color.
    /// </summary>
    public required Color PrimaryLight { get; init; }

    /// <summary>
    /// Gets or sets the primary dark mode color.
    /// </summary>
    public required Color PrimaryDark { get; init; }

    /// <summary>
    /// Gets or sets the accent light mode color.
    /// </summary>
    public required Color AccentLight { get; init; }

    /// <summary>
    /// Gets or sets the accent dark mode color.
    /// </summary>
    public required Color AccentDark { get; init; }

    /// <summary>
    /// Gets or sets the success light mode color.
    /// </summary>
    public Color SuccessLight { get; init; } = Color.FromRgb(21, 128, 61);

    /// <summary>
    /// Gets or sets the success dark mode color.
    /// </summary>
    public Color SuccessDark { get; init; } = Color.FromRgb(134, 239, 172);

    /// <summary>
    /// Gets or sets the warning light mode color.
    /// </summary>
    public Color WarningLight { get; init; } = Color.FromRgb(245, 158, 11);

    /// <summary>
    /// Gets or sets the warning dark mode color.
    /// </summary>
    public Color WarningDark { get; init; } = Color.FromRgb(252, 211, 77);

    /// <summary>
    /// Gets or sets the danger light mode color.
    /// </summary>
    public Color DangerLight { get; init; } = Color.FromRgb(180, 35, 24);

    /// <summary>
    /// Gets or sets the danger dark mode color.
    /// </summary>
    public Color DangerDark { get; init; } = Color.FromRgb(252, 165, 165);

    /// <summary>
    /// Gets or sets the info light mode color.
    /// </summary>
    public Color InfoLight { get; init; } = Color.FromRgb(3, 105, 161);

    /// <summary>
    /// Gets or sets the info dark mode color.
    /// </summary>
    public Color InfoDark { get; init; } = Color.FromRgb(125, 211, 252);

    /// <summary>
    /// Gets or sets the background light mode color.
    /// </summary>
    public Color BackgroundLight { get; init; } = Color.FromRgb(238, 241, 245);

    /// <summary>
    /// Gets or sets the background dark mode color.
    /// </summary>
    public Color BackgroundDark { get; init; } = Color.FromRgb(15, 23, 42);

    /// <summary>
    /// Gets or sets the surface light mode color.
    /// </summary>
    public Color SurfaceLight { get; init; } = Colors.White;

    /// <summary>
    /// Gets or sets the surface dark mode color.
    /// </summary>
    public Color SurfaceDark { get; init; } = Color.FromRgb(30, 41, 59);

    /// <summary>
    /// Gets or sets the navigation panel light mode color.
    /// </summary>
    public Color NavigationLight { get; init; } = Color.FromRgb(15, 23, 42);

    /// <summary>
    /// Gets or sets the navigation panel dark mode color.
    /// </summary>
    public Color NavigationDark { get; init; } = Color.FromRgb(15, 23, 42);

    /// <summary>
    /// Gets or sets the theme mode foreground light mode override.
    /// </summary>
    public Color? ThemeModeForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the theme mode foreground dark mode override.
    /// </summary>
    public Color? ThemeModeForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the control border light mode override.
    /// </summary>
    public Color? ControlBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the control border dark mode override.
    /// </summary>
    public Color? ControlBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the strong control border light mode override.
    /// </summary>
    public Color? ControlBorderStrongLight { get; init; }

    /// <summary>
    /// Gets or sets the strong control border dark mode override.
    /// </summary>
    public Color? ControlBorderStrongDark { get; init; }

    /// <summary>
    /// Gets or sets the panel border light mode override.
    /// </summary>
    public Color? PanelBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the panel border dark mode override.
    /// </summary>
    public Color? PanelBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the strong panel border light mode override.
    /// </summary>
    public Color? PanelBorderStrongLight { get; init; }

    /// <summary>
    /// Gets or sets the strong panel border dark mode override.
    /// </summary>
    public Color? PanelBorderStrongDark { get; init; }

    /// <summary>
    /// Gets or sets the focus border light mode override.
    /// </summary>
    public Color? FocusBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the focus border dark mode override.
    /// </summary>
    public Color? FocusBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the neutral border color-set base light mode override.
    /// </summary>
    public Color? BorderLight { get; init; }

    /// <summary>
    /// Gets or sets the neutral border color-set base dark mode override.
    /// </summary>
    public Color? BorderDark { get; init; }

    /// <summary>
    /// Gets or sets the selection background light mode override.
    /// </summary>
    public Color? SelectionBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the selection background dark mode override.
    /// </summary>
    public Color? SelectionBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the selection border light mode override.
    /// </summary>
    public Color? SelectionBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the selection border dark mode override.
    /// </summary>
    public Color? SelectionBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the selection foreground light mode override.
    /// </summary>
    public Color? SelectionForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the selection foreground dark mode override.
    /// </summary>
    public Color? SelectionForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the hover background light mode override.
    /// </summary>
    public Color? HoverBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the hover background dark mode override.
    /// </summary>
    public Color? HoverBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the hover border light mode override.
    /// </summary>
    public Color? HoverBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the hover border dark mode override.
    /// </summary>
    public Color? HoverBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the pressed background light mode override.
    /// </summary>
    public Color? PressedBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the pressed background dark mode override.
    /// </summary>
    public Color? PressedBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the pressed border light mode override.
    /// </summary>
    public Color? PressedBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the pressed border dark mode override.
    /// </summary>
    public Color? PressedBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the disabled background light mode override.
    /// </summary>
    public Color? DisabledBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the disabled background dark mode override.
    /// </summary>
    public Color? DisabledBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the disabled foreground light mode override.
    /// </summary>
    public Color? DisabledForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the disabled foreground dark mode override.
    /// </summary>
    public Color? DisabledForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the disabled border light mode override.
    /// </summary>
    public Color? DisabledBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the disabled border dark mode override.
    /// </summary>
    public Color? DisabledBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the grid line light mode override.
    /// </summary>
    public Color? GridLineLight { get; init; }

    /// <summary>
    /// Gets or sets the grid line dark mode override.
    /// </summary>
    public Color? GridLineDark { get; init; }

    /// <summary>
    /// Gets or sets the grid header background light mode override.
    /// </summary>
    public Color? GridHeaderBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the grid header background dark mode override.
    /// </summary>
    public Color? GridHeaderBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the grid header foreground light mode override.
    /// </summary>
    public Color? GridHeaderForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the grid header foreground dark mode override.
    /// </summary>
    public Color? GridHeaderForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the input background light mode override.
    /// </summary>
    public Color? InputBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the input background dark mode override.
    /// </summary>
    public Color? InputBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the input border light mode override.
    /// </summary>
    public Color? InputBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the input border dark mode override.
    /// </summary>
    public Color? InputBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the input placeholder light mode override.
    /// </summary>
    public Color? InputPlaceholderLight { get; init; }

    /// <summary>
    /// Gets or sets the input placeholder dark mode override.
    /// </summary>
    public Color? InputPlaceholderDark { get; init; }

    /// <summary>
    /// Gets or sets the read-only input background light mode override.
    /// </summary>
    public Color? InputReadOnlyBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the read-only input background dark mode override.
    /// </summary>
    public Color? InputReadOnlyBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header background light mode override.
    /// </summary>
    public Color? NavigationPanelHeaderBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header background dark mode override.
    /// </summary>
    public Color? NavigationPanelHeaderBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header foreground light mode override.
    /// </summary>
    public Color? NavigationPanelHeaderForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header foreground dark mode override.
    /// </summary>
    public Color? NavigationPanelHeaderForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header border light mode override.
    /// </summary>
    public Color? NavigationPanelHeaderBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel header border dark mode override.
    /// </summary>
    public Color? NavigationPanelHeaderBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the toolbar background light mode override.
    /// </summary>
    public Color? ToolbarBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the toolbar background dark mode override.
    /// </summary>
    public Color? ToolbarBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the toolbar foreground light mode override.
    /// </summary>
    public Color? ToolbarForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the toolbar foreground dark mode override.
    /// </summary>
    public Color? ToolbarForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the toolbar secondary foreground light mode override.
    /// </summary>
    public Color? ToolbarSecondaryForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the toolbar secondary foreground dark mode override.
    /// </summary>
    public Color? ToolbarSecondaryForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the toolbar border light mode override.
    /// </summary>
    public Color? ToolbarBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the toolbar border dark mode override.
    /// </summary>
    public Color? ToolbarBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb background light mode override.
    /// </summary>
    public Color? BreadcrumbBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb background dark mode override.
    /// </summary>
    public Color? BreadcrumbBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb foreground light mode override.
    /// </summary>
    public Color? BreadcrumbForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb foreground dark mode override.
    /// </summary>
    public Color? BreadcrumbForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb secondary foreground light mode override.
    /// </summary>
    public Color? BreadcrumbSecondaryForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the breadcrumb secondary foreground dark mode override.
    /// </summary>
    public Color? BreadcrumbSecondaryForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the tab header background light mode override.
    /// </summary>
    public Color? TabHeaderBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab header background dark mode override.
    /// </summary>
    public Color? TabHeaderBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the tab header foreground light mode override.
    /// </summary>
    public Color? TabHeaderForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab header foreground dark mode override.
    /// </summary>
    public Color? TabHeaderForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the tab header border light mode override.
    /// </summary>
    public Color? TabHeaderBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the tab header border dark mode override.
    /// </summary>
    public Color? TabHeaderBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the tab item background light mode override.
    /// </summary>
    public Color? TabItemBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab item background dark mode override.
    /// </summary>
    public Color? TabItemBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item background light mode override.
    /// </summary>
    public Color? TabItemBackgroundHoverLight { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item background dark mode override.
    /// </summary>
    public Color? TabItemBackgroundHoverDark { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item background light mode override.
    /// </summary>
    public Color? TabItemBackgroundSelectedLight { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item background dark mode override.
    /// </summary>
    public Color? TabItemBackgroundSelectedDark { get; init; }

    /// <summary>
    /// Gets or sets the tab item foreground light mode override.
    /// </summary>
    public Color? TabItemForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab item foreground dark mode override.
    /// </summary>
    public Color? TabItemForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item foreground light mode override.
    /// </summary>
    public Color? TabItemForegroundHoverLight { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item foreground dark mode override.
    /// </summary>
    public Color? TabItemForegroundHoverDark { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item foreground light mode override.
    /// </summary>
    public Color? TabItemForegroundSelectedLight { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item foreground dark mode override.
    /// </summary>
    public Color? TabItemForegroundSelectedDark { get; init; }

    /// <summary>
    /// Gets or sets the tab item border light mode override.
    /// </summary>
    public Color? TabItemBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the tab item border dark mode override.
    /// </summary>
    public Color? TabItemBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item border light mode override.
    /// </summary>
    public Color? TabItemBorderHoverLight { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab item border dark mode override.
    /// </summary>
    public Color? TabItemBorderHoverDark { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item border light mode override.
    /// </summary>
    public Color? TabItemBorderSelectedLight { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item border dark mode override.
    /// </summary>
    public Color? TabItemBorderSelectedDark { get; init; }

    /// <summary>
    /// Gets or sets the tab item underline light mode override.
    /// </summary>
    public Color? TabItemUnderlineLight { get; init; }

    /// <summary>
    /// Gets or sets the tab item underline dark mode override.
    /// </summary>
    public Color? TabItemUnderlineDark { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item underline light mode override.
    /// </summary>
    public Color? TabItemUnderlineSelectedLight { get; init; }

    /// <summary>
    /// Gets or sets the selected tab item underline dark mode override.
    /// </summary>
    public Color? TabItemUnderlineSelectedDark { get; init; }

    /// <summary>
    /// Gets or sets the tab content background light mode override.
    /// </summary>
    public Color? TabContentBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab content background dark mode override.
    /// </summary>
    public Color? TabContentBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the tab content border light mode override.
    /// </summary>
    public Color? TabContentBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the tab content border dark mode override.
    /// </summary>
    public Color? TabContentBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the tab action button foreground light mode override.
    /// </summary>
    public Color? TabActionButtonForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the tab action button foreground dark mode override.
    /// </summary>
    public Color? TabActionButtonForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button foreground light mode override.
    /// </summary>
    public Color? TabActionButtonForegroundHoverLight { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button foreground dark mode override.
    /// </summary>
    public Color? TabActionButtonForegroundHoverDark { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button background light mode override.
    /// </summary>
    public Color? TabActionButtonBackgroundHoverLight { get; init; }

    /// <summary>
    /// Gets or sets the hovered tab action button background dark mode override.
    /// </summary>
    public Color? TabActionButtonBackgroundHoverDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel background light mode override.
    /// </summary>
    public Color? NavigationPanelBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel background dark mode override.
    /// </summary>
    public Color? NavigationPanelBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel foreground light mode override.
    /// </summary>
    public Color? NavigationPanelForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel foreground dark mode override.
    /// </summary>
    public Color? NavigationPanelForegroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel border light mode override.
    /// </summary>
    public Color? NavigationPanelBorderLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel border dark mode override.
    /// </summary>
    public Color? NavigationPanelBorderDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item hover background light mode override.
    /// </summary>
    public Color? NavigationPanelItemHoverBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item hover background dark mode override.
    /// </summary>
    public Color? NavigationPanelItemHoverBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item selected background light mode override.
    /// </summary>
    public Color? NavigationPanelItemSelectedBackgroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item selected background dark mode override.
    /// </summary>
    public Color? NavigationPanelItemSelectedBackgroundDark { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item selected foreground light mode override.
    /// </summary>
    public Color? NavigationPanelItemSelectedForegroundLight { get; init; }

    /// <summary>
    /// Gets or sets the navigation panel item selected foreground dark mode override.
    /// </summary>
    public Color? NavigationPanelItemSelectedForegroundDark { get; init; }
    #endregion
}
#endregion
