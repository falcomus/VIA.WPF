// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBrushKeys.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Themes;

#region ### Class XBrushKeys ###
/// <summary>
/// Provides strongly typed resource keys for VIA.WPF brushes.
/// </summary>
public static class XBrushKeys
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the resource key for the primary brush.
    /// </summary>
    public static ComponentResourceKey Primary { get; } = new(typeof(XBrushKeys), nameof(Primary));

    /// <summary>
    /// Gets the resource key for the primary text brush.
    /// </summary>
    public static ComponentResourceKey PrimaryText { get; } = new(typeof(XBrushKeys), nameof(PrimaryText));

    /// <summary>
    /// Gets the resource key for the primary very light brush.
    /// </summary>
    public static ComponentResourceKey PrimaryVeryLight { get; } = new(typeof(XBrushKeys), nameof(PrimaryVeryLight));

    /// <summary>
    /// Gets the resource key for the primary light brush.
    /// </summary>
    public static ComponentResourceKey PrimaryLight { get; } = new(typeof(XBrushKeys), nameof(PrimaryLight));

    /// <summary>
    /// Gets the resource key for the primary dark brush.
    /// </summary>
    public static ComponentResourceKey PrimaryDark { get; } = new(typeof(XBrushKeys), nameof(PrimaryDark));

    /// <summary>
    /// Gets the resource key for the background brush.
    /// </summary>
    public static ComponentResourceKey Background { get; } = new(typeof(XBrushKeys), nameof(Background));

    /// <summary>
    /// Gets the resource key for the background text brush.
    /// </summary>
    public static ComponentResourceKey BackgroundText { get; } = new(typeof(XBrushKeys), nameof(BackgroundText));

    /// <summary>
    /// Gets the resource key for the background light brush.
    /// </summary>
    public static ComponentResourceKey BackgroundLight { get; } = new(typeof(XBrushKeys), nameof(BackgroundLight));

    /// <summary>
    /// Gets the resource key for the background dark brush.
    /// </summary>
    public static ComponentResourceKey BackgroundDark { get; } = new(typeof(XBrushKeys), nameof(BackgroundDark));

    /// <summary>
    /// Gets the resource key for the surface brush.
    /// </summary>
    public static ComponentResourceKey Surface { get; } = new(typeof(XBrushKeys), nameof(Surface));

    /// <summary>
    /// Gets the resource key for the surface text brush.
    /// </summary>
    public static ComponentResourceKey SurfaceText { get; } = new(typeof(XBrushKeys), nameof(SurfaceText));

    /// <summary>
    /// Gets the resource key for the surface very light brush.
    /// </summary>
    public static ComponentResourceKey SurfaceVeryLight { get; } = new(typeof(XBrushKeys), nameof(SurfaceVeryLight));

    /// <summary>
    /// Gets the resource key for the surface light brush.
    /// </summary>
    public static ComponentResourceKey SurfaceLight { get; } = new(typeof(XBrushKeys), nameof(SurfaceLight));

    /// <summary>
    /// Gets the resource key for the surface dark brush.
    /// </summary>
    public static ComponentResourceKey SurfaceDark { get; } = new(typeof(XBrushKeys), nameof(SurfaceDark));

    /// <summary>
    /// Gets the resource key for the border brush.
    /// </summary>
    public static ComponentResourceKey Border { get; } = new(typeof(XBrushKeys), nameof(Border));

    /// <summary>
    /// Gets the resource key for the border text brush.
    /// </summary>
    public static ComponentResourceKey BorderText { get; } = new(typeof(XBrushKeys), nameof(BorderText));

    /// <summary>
    /// Gets the resource key for the border light brush.
    /// </summary>
    public static ComponentResourceKey BorderLight { get; } = new(typeof(XBrushKeys), nameof(BorderLight));

    /// <summary>
    /// Gets the resource key for the border dark brush.
    /// </summary>
    public static ComponentResourceKey BorderDark { get; } = new(typeof(XBrushKeys), nameof(BorderDark));

    /// <summary>
    /// Gets the resource key for the accent brush.
    /// </summary>
    public static ComponentResourceKey Accent { get; } = new(typeof(XBrushKeys), nameof(Accent));

    /// <summary>
    /// Gets the resource key for the accent text brush.
    /// </summary>
    public static ComponentResourceKey AccentText { get; } = new(typeof(XBrushKeys), nameof(AccentText));

    /// <summary>
    /// Gets the resource key for the accent very light brush.
    /// </summary>
    public static ComponentResourceKey AccentVeryLight { get; } = new(typeof(XBrushKeys), nameof(AccentVeryLight));

    /// <summary>
    /// Gets the resource key for the accent light brush.
    /// </summary>
    public static ComponentResourceKey AccentLight { get; } = new(typeof(XBrushKeys), nameof(AccentLight));

    /// <summary>
    /// Gets the resource key for the accent dark brush.
    /// </summary>
    public static ComponentResourceKey AccentDark { get; } = new(typeof(XBrushKeys), nameof(AccentDark));

    /// <summary>
    /// Gets the resource key for the success brush.
    /// </summary>
    public static ComponentResourceKey Success { get; } = new(typeof(XBrushKeys), nameof(Success));

    /// <summary>
    /// Gets the resource key for the success text brush.
    /// </summary>
    public static ComponentResourceKey SuccessText { get; } = new(typeof(XBrushKeys), nameof(SuccessText));

    /// <summary>
    /// Gets the resource key for the success very light brush.
    /// </summary>
    public static ComponentResourceKey SuccessVeryLight { get; } = new(typeof(XBrushKeys), nameof(SuccessVeryLight));

    /// <summary>
    /// Gets the resource key for the success light brush.
    /// </summary>
    public static ComponentResourceKey SuccessLight { get; } = new(typeof(XBrushKeys), nameof(SuccessLight));

    /// <summary>
    /// Gets the resource key for the success dark brush.
    /// </summary>
    public static ComponentResourceKey SuccessDark { get; } = new(typeof(XBrushKeys), nameof(SuccessDark));

    /// <summary>
    /// Gets the resource key for the warning brush.
    /// </summary>
    public static ComponentResourceKey Warning { get; } = new(typeof(XBrushKeys), nameof(Warning));

    /// <summary>
    /// Gets the resource key for the warning text brush.
    /// </summary>
    public static ComponentResourceKey WarningText { get; } = new(typeof(XBrushKeys), nameof(WarningText));

    /// <summary>
    /// Gets the resource key for the warning very light brush.
    /// </summary>
    public static ComponentResourceKey WarningVeryLight { get; } = new(typeof(XBrushKeys), nameof(WarningVeryLight));

    /// <summary>
    /// Gets the resource key for the warning light brush.
    /// </summary>
    public static ComponentResourceKey WarningLight { get; } = new(typeof(XBrushKeys), nameof(WarningLight));

    /// <summary>
    /// Gets the resource key for the warning dark brush.
    /// </summary>
    public static ComponentResourceKey WarningDark { get; } = new(typeof(XBrushKeys), nameof(WarningDark));

    /// <summary>
    /// Gets the resource key for the danger brush.
    /// </summary>
    public static ComponentResourceKey Danger { get; } = new(typeof(XBrushKeys), nameof(Danger));

    /// <summary>
    /// Gets the resource key for the danger text brush.
    /// </summary>
    public static ComponentResourceKey DangerText { get; } = new(typeof(XBrushKeys), nameof(DangerText));

    /// <summary>
    /// Gets the resource key for the danger very light brush.
    /// </summary>
    public static ComponentResourceKey DangerVeryLight { get; } = new(typeof(XBrushKeys), nameof(DangerVeryLight));

    /// <summary>
    /// Gets the resource key for the danger light brush.
    /// </summary>
    public static ComponentResourceKey DangerLight { get; } = new(typeof(XBrushKeys), nameof(DangerLight));

    /// <summary>
    /// Gets the resource key for the danger dark brush.
    /// </summary>
    public static ComponentResourceKey DangerDark { get; } = new(typeof(XBrushKeys), nameof(DangerDark));

    /// <summary>
    /// Gets the resource key for the info brush.
    /// </summary>
    public static ComponentResourceKey Info { get; } = new(typeof(XBrushKeys), nameof(Info));

    /// <summary>
    /// Gets the resource key for the info text brush.
    /// </summary>
    public static ComponentResourceKey InfoText { get; } = new(typeof(XBrushKeys), nameof(InfoText));

    /// <summary>
    /// Gets the resource key for the info very light brush.
    /// </summary>
    public static ComponentResourceKey InfoVeryLight { get; } = new(typeof(XBrushKeys), nameof(InfoVeryLight));

    /// <summary>
    /// Gets the resource key for the info light brush.
    /// </summary>
    public static ComponentResourceKey InfoLight { get; } = new(typeof(XBrushKeys), nameof(InfoLight));

    /// <summary>
    /// Gets the resource key for the info dark brush.
    /// </summary>
    public static ComponentResourceKey InfoDark { get; } = new(typeof(XBrushKeys), nameof(InfoDark));

    /// <summary>
    /// Gets the resource key for the application canvas brush.
    /// </summary>
    public static ComponentResourceKey Canvas { get; } = new(typeof(XBrushKeys), nameof(Canvas));

    /// <summary>
    /// Gets the resource key for a raised surface brush.
    /// </summary>
    public static ComponentResourceKey SurfaceRaised { get; } = new(typeof(XBrushKeys), nameof(SurfaceRaised));

    /// <summary>
    /// Gets the resource key for a sunken surface brush.
    /// </summary>
    public static ComponentResourceKey SurfaceSunken { get; } = new(typeof(XBrushKeys), nameof(SurfaceSunken));

    /// <summary>
    /// Gets the resource key for primary text.
    /// </summary>
    public static ComponentResourceKey TextPrimary { get; } = new(typeof(XBrushKeys), nameof(TextPrimary));

    /// <summary>
    /// Gets the resource key for secondary text.
    /// </summary>
    public static ComponentResourceKey TextSecondary { get; } = new(typeof(XBrushKeys), nameof(TextSecondary));

    /// <summary>
    /// Gets the resource key for tertiary text and placeholders.
    /// </summary>
    public static ComponentResourceKey TextTertiary { get; } = new(typeof(XBrushKeys), nameof(TextTertiary));

    /// <summary>
    /// Gets the resource key for subtle separators and container borders.
    /// </summary>
    public static ComponentResourceKey BorderSubtle { get; } = new(typeof(XBrushKeys), nameof(BorderSubtle));

    /// <summary>
    /// Gets the resource key for standard control and container borders.
    /// </summary>
    public static ComponentResourceKey BorderDefault { get; } = new(typeof(XBrushKeys), nameof(BorderDefault));

    /// <summary>
    /// Gets the resource key for emphasized control and container borders.
    /// </summary>
    public static ComponentResourceKey BorderStrong { get; } = new(typeof(XBrushKeys), nameof(BorderStrong));

    /// <summary>
    /// Gets the resource key for the outer keyboard focus ring.
    /// </summary>
    public static ComponentResourceKey FocusRing { get; } = new(typeof(XBrushKeys), nameof(FocusRing));

    /// <summary>
    /// Gets the resource key for the contrasting inner keyboard focus ring.
    /// </summary>
    public static ComponentResourceKey FocusRingInner { get; } = new(typeof(XBrushKeys), nameof(FocusRingInner));

    /// <summary>
    /// Gets the resource key for general hover state surfaces.
    /// </summary>
    public static ComponentResourceKey StateHover { get; } = new(typeof(XBrushKeys), nameof(StateHover));

    /// <summary>
    /// Gets the resource key for general pressed state surfaces.
    /// </summary>
    public static ComponentResourceKey StatePressed { get; } = new(typeof(XBrushKeys), nameof(StatePressed));

    /// <summary>
    /// Gets the resource key for selected state surfaces.
    /// </summary>
    public static ComponentResourceKey StateSelected { get; } = new(typeof(XBrushKeys), nameof(StateSelected));

    /// <summary>
    /// Gets the resource key for strong selected state surfaces.
    /// </summary>
    public static ComponentResourceKey StateSelectedStrong { get; } = new(typeof(XBrushKeys), nameof(StateSelectedStrong));

    /// <summary>
    /// Gets the resource key for modal and flyout scrims.
    /// </summary>
    public static ComponentResourceKey Scrim { get; } = new(typeof(XBrushKeys), nameof(Scrim));

    /// <summary>
    /// Gets the resource key for the integrated command-bar background.
    /// </summary>
    public static ComponentResourceKey CommandBarBackground { get; } = new(typeof(XBrushKeys), nameof(CommandBarBackground));

    /// <summary>
    /// Gets the resource key for the integrated command-bar foreground.
    /// </summary>
    public static ComponentResourceKey CommandBarForeground { get; } = new(typeof(XBrushKeys), nameof(CommandBarForeground));

    /// <summary>
    /// Gets the resource key for hovered command-bar actions.
    /// </summary>
    public static ComponentResourceKey CommandBarHoverBackground { get; } = new(typeof(XBrushKeys), nameof(CommandBarHoverBackground));

    /// <summary>
    /// Gets the resource key for pressed command-bar actions.
    /// </summary>
    public static ComponentResourceKey CommandBarPressedBackground { get; } = new(typeof(XBrushKeys), nameof(CommandBarPressedBackground));

    /// <summary>
    /// Gets the resource key for command-bar group header backgrounds.
    /// </summary>
    public static ComponentResourceKey CommandBarGroupHeaderBackground { get; } = new(typeof(XBrushKeys), nameof(CommandBarGroupHeaderBackground));

    /// <summary>
    /// Gets the resource key for the navigation selection indicator.
    /// </summary>
    public static ComponentResourceKey NavigationSelectionIndicator { get; } = new(typeof(XBrushKeys), nameof(NavigationSelectionIndicator));

    /// <summary>
    /// Gets the resource key for the theme mode foreground brush.
    /// </summary>
    public static ComponentResourceKey ThemeModeForeground { get; } = new(typeof(XBrushKeys), nameof(ThemeModeForeground));

    /// <summary>
    /// Gets the resource key for the control border brush.
    /// </summary>
    public static ComponentResourceKey ControlBorder { get; } = new(typeof(XBrushKeys), nameof(ControlBorder));

    /// <summary>
    /// Gets the resource key for the stronger control border brush.
    /// </summary>
    public static ComponentResourceKey ControlBorderStrong { get; } = new(typeof(XBrushKeys), nameof(ControlBorderStrong));

    /// <summary>
    /// Gets the resource key for the panel border brush.
    /// </summary>
    public static ComponentResourceKey PanelBorder { get; } = new(typeof(XBrushKeys), nameof(PanelBorder));

    /// <summary>
    /// Gets the resource key for the stronger panel border brush.
    /// </summary>
    public static ComponentResourceKey PanelBorderStrong { get; } = new(typeof(XBrushKeys), nameof(PanelBorderStrong));

    /// <summary>
    /// Gets the resource key for the focus border brush.
    /// </summary>
    public static ComponentResourceKey FocusBorder { get; } = new(typeof(XBrushKeys), nameof(FocusBorder));



    /// <summary>
    /// Gets the resource key for the selected item background brush.
    /// </summary>
    public static ComponentResourceKey SelectionBackground { get; } = new(typeof(XBrushKeys), nameof(SelectionBackground));

    /// <summary>
    /// Gets the resource key for the selected item border brush.
    /// </summary>
    public static ComponentResourceKey SelectionBorder { get; } = new(typeof(XBrushKeys), nameof(SelectionBorder));

    /// <summary>
    /// Gets the resource key for the selected item foreground brush.
    /// </summary>
    public static ComponentResourceKey SelectionForeground { get; } = new(typeof(XBrushKeys), nameof(SelectionForeground));

    /// <summary>
    /// Gets the resource key for the general hover background brush.
    /// </summary>
    public static ComponentResourceKey HoverBackground { get; } = new(typeof(XBrushKeys), nameof(HoverBackground));

    /// <summary>
    /// Gets the resource key for the general hover border brush.
    /// </summary>
    public static ComponentResourceKey HoverBorder { get; } = new(typeof(XBrushKeys), nameof(HoverBorder));

    /// <summary>
    /// Gets the resource key for the general pressed background brush.
    /// </summary>
    public static ComponentResourceKey PressedBackground { get; } = new(typeof(XBrushKeys), nameof(PressedBackground));

    /// <summary>
    /// Gets the resource key for the general pressed border brush.
    /// </summary>
    public static ComponentResourceKey PressedBorder { get; } = new(typeof(XBrushKeys), nameof(PressedBorder));

    /// <summary>
    /// Gets the resource key for the disabled background brush.
    /// </summary>
    public static ComponentResourceKey DisabledBackground { get; } = new(typeof(XBrushKeys), nameof(DisabledBackground));

    /// <summary>
    /// Gets the resource key for the disabled foreground brush.
    /// </summary>
    public static ComponentResourceKey DisabledForeground { get; } = new(typeof(XBrushKeys), nameof(DisabledForeground));

    /// <summary>
    /// Gets the resource key for the disabled border brush.
    /// </summary>
    public static ComponentResourceKey DisabledBorder { get; } = new(typeof(XBrushKeys), nameof(DisabledBorder));

    /// <summary>
    /// Gets the resource key for the data grid line brush.
    /// </summary>
    public static ComponentResourceKey GridLine { get; } = new(typeof(XBrushKeys), nameof(GridLine));

    /// <summary>
    /// Gets the resource key for the data grid header background brush.
    /// </summary>
    public static ComponentResourceKey GridHeaderBackground { get; } = new(typeof(XBrushKeys), nameof(GridHeaderBackground));

    /// <summary>
    /// Gets the resource key for the data grid header foreground brush.
    /// </summary>
    public static ComponentResourceKey GridHeaderForeground { get; } = new(typeof(XBrushKeys), nameof(GridHeaderForeground));

    /// <summary>
    /// Gets the resource key for the input background brush.
    /// </summary>
    public static ComponentResourceKey InputBackground { get; } = new(typeof(XBrushKeys), nameof(InputBackground));

    /// <summary>
    /// Gets the resource key for the input border brush.
    /// </summary>
    public static ComponentResourceKey InputBorder { get; } = new(typeof(XBrushKeys), nameof(InputBorder));

    /// <summary>
    /// Gets the resource key for the input placeholder foreground brush.
    /// </summary>
    public static ComponentResourceKey InputPlaceholder { get; } = new(typeof(XBrushKeys), nameof(InputPlaceholder));

    /// <summary>
    /// Gets the resource key for the read-only input background brush.
    /// </summary>
    public static ComponentResourceKey InputReadOnlyBackground { get; } = new(typeof(XBrushKeys), nameof(InputReadOnlyBackground));

    /// <summary>
    /// Gets the resource key for the navigation panel header background brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelHeaderBackground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelHeaderBackground));

    /// <summary>
    /// Gets the resource key for the navigation panel header foreground brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelHeaderForeground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelHeaderForeground));

    /// <summary>
    /// Gets the resource key for the navigation panel header border brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelHeaderBorder { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelHeaderBorder));

    /// <summary>
    /// Gets the resource key for the toolbar background brush.
    /// </summary>
    public static ComponentResourceKey ToolbarBackground { get; } = new(typeof(XBrushKeys), nameof(ToolbarBackground));

    /// <summary>
    /// Gets the resource key for the toolbar foreground brush.
    /// </summary>
    public static ComponentResourceKey ToolbarForeground { get; } = new(typeof(XBrushKeys), nameof(ToolbarForeground));

    /// <summary>
    /// Gets the resource key for the toolbar secondary foreground brush.
    /// </summary>
    public static ComponentResourceKey ToolbarSecondaryForeground { get; } = new(typeof(XBrushKeys), nameof(ToolbarSecondaryForeground));

    /// <summary>
    /// Gets the resource key for the toolbar border brush.
    /// </summary>
    public static ComponentResourceKey ToolbarBorder { get; } = new(typeof(XBrushKeys), nameof(ToolbarBorder));

    /// <summary>
    /// Gets the resource key for the breadcrumb background brush.
    /// </summary>
    public static ComponentResourceKey BreadcrumbBackground { get; } = new(typeof(XBrushKeys), nameof(BreadcrumbBackground));

    /// <summary>
    /// Gets the resource key for the breadcrumb foreground brush.
    /// </summary>
    public static ComponentResourceKey BreadcrumbForeground { get; } = new(typeof(XBrushKeys), nameof(BreadcrumbForeground));

    /// <summary>
    /// Gets the resource key for the breadcrumb secondary foreground brush.
    /// </summary>
    public static ComponentResourceKey BreadcrumbSecondaryForeground { get; } = new(typeof(XBrushKeys), nameof(BreadcrumbSecondaryForeground));

    /// <summary>
    /// Gets the resource key for the tab header background brush.
    /// </summary>
    public static ComponentResourceKey TabHeaderBackground { get; } = new(typeof(XBrushKeys), nameof(TabHeaderBackground));

    /// <summary>
    /// Gets the resource key for the tab header foreground brush.
    /// </summary>
    public static ComponentResourceKey TabHeaderForeground { get; } = new(typeof(XBrushKeys), nameof(TabHeaderForeground));

    /// <summary>
    /// Gets the resource key for the tab header border brush.
    /// </summary>
    public static ComponentResourceKey TabHeaderBorder { get; } = new(typeof(XBrushKeys), nameof(TabHeaderBorder));

    /// <summary>
    /// Gets the resource key for the tab item background brush.
    /// </summary>
    public static ComponentResourceKey TabItemBackground { get; } = new(typeof(XBrushKeys), nameof(TabItemBackground));

    /// <summary>
    /// Gets the resource key for the hovered tab item background brush.
    /// </summary>
    public static ComponentResourceKey TabItemBackgroundHover { get; } = new(typeof(XBrushKeys), nameof(TabItemBackgroundHover));

    /// <summary>
    /// Gets the resource key for the selected tab item background brush.
    /// </summary>
    public static ComponentResourceKey TabItemBackgroundSelected { get; } = new(typeof(XBrushKeys), nameof(TabItemBackgroundSelected));

    /// <summary>
    /// Gets the resource key for the tab item foreground brush.
    /// </summary>
    public static ComponentResourceKey TabItemForeground { get; } = new(typeof(XBrushKeys), nameof(TabItemForeground));

    /// <summary>
    /// Gets the resource key for the hovered tab item foreground brush.
    /// </summary>
    public static ComponentResourceKey TabItemForegroundHover { get; } = new(typeof(XBrushKeys), nameof(TabItemForegroundHover));

    /// <summary>
    /// Gets the resource key for the selected tab item foreground brush.
    /// </summary>
    public static ComponentResourceKey TabItemForegroundSelected { get; } = new(typeof(XBrushKeys), nameof(TabItemForegroundSelected));

    /// <summary>
    /// Gets the resource key for the tab item border brush.
    /// </summary>
    public static ComponentResourceKey TabItemBorder { get; } = new(typeof(XBrushKeys), nameof(TabItemBorder));

    /// <summary>
    /// Gets the resource key for the hovered tab item border brush.
    /// </summary>
    public static ComponentResourceKey TabItemBorderHover { get; } = new(typeof(XBrushKeys), nameof(TabItemBorderHover));

    /// <summary>
    /// Gets the resource key for the selected tab item border brush.
    /// </summary>
    public static ComponentResourceKey TabItemBorderSelected { get; } = new(typeof(XBrushKeys), nameof(TabItemBorderSelected));

    /// <summary>
    /// Gets the resource key for the tab item underline brush.
    /// </summary>
    public static ComponentResourceKey TabItemUnderline { get; } = new(typeof(XBrushKeys), nameof(TabItemUnderline));

    /// <summary>
    /// Gets the resource key for the selected tab item underline brush.
    /// </summary>
    public static ComponentResourceKey TabItemUnderlineSelected { get; } = new(typeof(XBrushKeys), nameof(TabItemUnderlineSelected));

    /// <summary>
    /// Gets the resource key for the tab content background brush.
    /// </summary>
    public static ComponentResourceKey TabContentBackground { get; } = new(typeof(XBrushKeys), nameof(TabContentBackground));

    /// <summary>
    /// Gets the resource key for the tab content border brush.
    /// </summary>
    public static ComponentResourceKey TabContentBorder { get; } = new(typeof(XBrushKeys), nameof(TabContentBorder));

    /// <summary>
    /// Gets the resource key for the tab action button foreground brush.
    /// </summary>
    public static ComponentResourceKey TabActionButtonForeground { get; } = new(typeof(XBrushKeys), nameof(TabActionButtonForeground));

    /// <summary>
    /// Gets the resource key for the hovered tab action button foreground brush.
    /// </summary>
    public static ComponentResourceKey TabActionButtonForegroundHover { get; } = new(typeof(XBrushKeys), nameof(TabActionButtonForegroundHover));

    /// <summary>
    /// Gets the resource key for the hovered tab action button background brush.
    /// </summary>
    public static ComponentResourceKey TabActionButtonBackgroundHover { get; } = new(typeof(XBrushKeys), nameof(TabActionButtonBackgroundHover));


    /// <summary>
    /// Gets the resource key for the navigation panel background brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelBackground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelBackground));

    /// <summary>
    /// Gets the resource key for the navigation panel foreground brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelForeground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelForeground));

    /// <summary>
    /// Gets the resource key for the navigation panel border brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelBorder { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelBorder));

    /// <summary>
    /// Gets the resource key for the hovered navigation panel item background brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelItemHoverBackground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelItemHoverBackground));

    /// <summary>
    /// Gets the resource key for the selected navigation panel item background brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelSelectedItemBackground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelSelectedItemBackground));

    /// <summary>
    /// Gets the resource key for the selected navigation panel item foreground brush.
    /// </summary>
    public static ComponentResourceKey NavigationPanelSelectedItemForeground { get; } = new(typeof(XBrushKeys), nameof(NavigationPanelSelectedItemForeground));
    #endregion

}
#endregion
