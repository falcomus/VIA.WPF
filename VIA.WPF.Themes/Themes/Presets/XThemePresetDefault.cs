// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetDefault.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetDefault ###
/// <summary>
/// Provides the built-in Default theme preset.
/// </summary>
internal static class XThemePresetDefault
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Default theme.
    /// </summary>
    /// <returns>The built-in Default theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Default",
                PrimaryLight = Color.FromRgb(53, 92, 145),
                PrimaryDark = Color.FromRgb(112, 162, 224),
                AccentLight = Color.FromRgb(109, 91, 158),
                AccentDark = Color.FromRgb(183, 167, 229),
                BackgroundLight = Color.FromRgb(241, 244, 247),
                BackgroundDark = Color.FromRgb(24, 26, 29),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(37, 40, 44),
                NavigationLight = Color.FromRgb(32, 37, 43),
                NavigationDark = Color.FromRgb(21, 24, 28),

                ControlBorderLight = Color.FromRgb(166, 174, 184),
                ControlBorderDark = Color.FromRgb(91, 97, 105),
                ControlBorderStrongLight = Color.FromRgb(112, 122, 134),
                ControlBorderStrongDark = Color.FromRgb(124, 132, 142),
                PanelBorderLight = Color.FromRgb(197, 204, 213),
                PanelBorderDark = Color.FromRgb(69, 75, 83),
                PanelBorderStrongLight = Color.FromRgb(166, 174, 184),
                PanelBorderStrongDark = Color.FromRgb(91, 97, 105),
                FocusBorderLight = Color.FromRgb(53, 92, 145),
                FocusBorderDark = Color.FromRgb(112, 162, 224),
                BorderLight = Color.FromRgb(166, 174, 184),
                BorderDark = Color.FromRgb(91, 97, 105),
                SelectionBackgroundLight = Color.FromRgb(222, 233, 247),
                SelectionBackgroundDark = Color.FromRgb(42, 59, 82),
                SelectionBorderLight = Color.FromRgb(53, 92, 145),
                SelectionBorderDark = Color.FromRgb(112, 162, 224),
                HoverBackgroundLight = Color.FromRgb(232, 236, 241),
                HoverBackgroundDark = Color.FromRgb(48, 52, 58),
                HoverBorderLight = Color.FromRgb(135, 145, 157),
                HoverBorderDark = Color.FromRgb(111, 119, 129),
                PressedBackgroundLight = Color.FromRgb(218, 224, 231),
                PressedBackgroundDark = Color.FromRgb(57, 62, 69),
                PressedBorderLight = Color.FromRgb(53, 92, 145),
                PressedBorderDark = Color.FromRgb(112, 162, 224),
                DisabledBackgroundLight = Color.FromRgb(234, 237, 241),
                DisabledBackgroundDark = Color.FromRgb(42, 46, 51),
                DisabledForegroundLight = Color.FromRgb(122, 131, 142),
                DisabledForegroundDark = Color.FromRgb(132, 139, 148),
                DisabledBorderLight = Color.FromRgb(203, 209, 217),
                DisabledBorderDark = Color.FromRgb(67, 73, 81),
                GridLineLight = Color.FromRgb(203, 209, 217),
                GridLineDark = Color.FromRgb(62, 68, 76),
                GridHeaderBackgroundLight = Color.FromRgb(232, 236, 241),
                GridHeaderBackgroundDark = Color.FromRgb(42, 46, 51),
                InputBackgroundDark = Color.FromRgb(45, 49, 54),
                InputBorderLight = Color.FromRgb(166, 174, 184),
                InputBorderDark = Color.FromRgb(91, 97, 105),
                InputReadOnlyBackgroundLight = Color.FromRgb(235, 238, 242),
                InputReadOnlyBackgroundDark = Color.FromRgb(34, 37, 41),
                ToolbarBackgroundLight = Colors.Transparent,
                ToolbarBackgroundDark = Colors.Transparent,
                ToolbarBorderLight = Colors.Transparent,
                ToolbarBorderDark = Colors.Transparent,
                BreadcrumbBackgroundLight = Colors.Transparent,
                BreadcrumbBackgroundDark = Colors.Transparent,
                TabHeaderBorderLight = Color.FromRgb(197, 204, 213),
                TabHeaderBorderDark = Color.FromRgb(69, 75, 83),
                TabItemBackgroundHoverLight = Color.FromRgb(232, 236, 241),
                TabItemBackgroundHoverDark = Color.FromRgb(48, 52, 58),
                TabItemBorderHoverLight = Color.FromRgb(166, 174, 184),
                TabItemBorderHoverDark = Color.FromRgb(91, 97, 105),
                TabItemBorderSelectedLight = Color.FromRgb(135, 145, 157),
                TabItemBorderSelectedDark = Color.FromRgb(111, 119, 129),
                TabItemUnderlineSelectedLight = Color.FromRgb(53, 92, 145),
                TabItemUnderlineSelectedDark = Color.FromRgb(112, 162, 224),
                TabContentBorderLight = Color.FromRgb(197, 204, 213),
                TabContentBorderDark = Color.FromRgb(69, 75, 83),
                NavigationPanelForegroundLight = Color.FromRgb(226, 231, 237),
                NavigationPanelForegroundDark = Color.FromRgb(241, 243, 245),
                NavigationPanelBorderLight = Color.FromRgb(67, 74, 83),
                NavigationPanelBorderDark = Color.FromRgb(58, 64, 72),
                NavigationPanelItemHoverBackgroundLight = Color.FromRgb(44, 52, 61),
                NavigationPanelItemHoverBackgroundDark = Color.FromRgb(43, 48, 55),
                NavigationPanelItemSelectedBackgroundLight = Color.FromRgb(53, 75, 105),
                NavigationPanelItemSelectedBackgroundDark = Color.FromRgb(42, 59, 82),
                NavigationPanelItemSelectedForegroundLight = Color.FromRgb(255, 255, 255),
                NavigationPanelItemSelectedForegroundDark = Color.FromRgb(245, 247, 249)
            });
    }
    #endregion
}
#endregion
