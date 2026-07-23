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
                PrimaryLight = Color.FromRgb(15, 108, 189),
                PrimaryDark = Color.FromRgb(76, 166, 255),
                AccentLight = Color.FromRgb(15, 108, 189),
                AccentDark = Color.FromRgb(76, 166, 255),
                BackgroundLight = Color.FromRgb(239, 241, 243),
                BackgroundDark = Color.FromRgb(23, 24, 25),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(39, 40, 41),
                NavigationLight = Color.FromRgb(247, 248, 249),
                NavigationDark = Color.FromRgb(21, 22, 23),

                ControlBorderLight = Color.FromRgb(184, 190, 198),
                ControlBorderDark = Color.FromRgb(82, 85, 89),
                ControlBorderStrongLight = Color.FromRgb(133, 141, 151),
                ControlBorderStrongDark = Color.FromRgb(108, 112, 117),
                PanelBorderLight = Color.FromRgb(213, 217, 222),
                PanelBorderStrongLight = Color.FromRgb(190, 196, 203),
                PanelBorderStrongDark = Color.FromRgb(83, 87, 92),
                FocusBorderLight = Color.FromRgb(15, 108, 189),
                FocusBorderDark = Color.FromRgb(76, 166, 255),
                BorderLight = Color.FromRgb(200, 205, 211),
                BorderDark = Color.FromRgb(72, 75, 79),
                SelectionBackgroundLight = Color.FromRgb(220, 235, 250),
                SelectionBackgroundDark = Color.FromRgb(29, 58, 86),
                SelectionBorderLight = Color.FromRgb(15, 108, 189),
                SelectionBorderDark = Color.FromRgb(76, 166, 255),
                HoverBackgroundLight = Color.FromRgb(236, 240, 244),
                HoverBackgroundDark = Color.FromRgb(47, 49, 52),
                HoverBorderLight = Color.FromRgb(158, 166, 175),
                HoverBorderDark = Color.FromRgb(100, 104, 109),
                PressedBackgroundLight = Color.FromRgb(223, 229, 235),
                PressedBackgroundDark = Color.FromRgb(57, 60, 64),
                PressedBorderLight = Color.FromRgb(15, 108, 189),
                PressedBorderDark = Color.FromRgb(76, 166, 255),
                DisabledBackgroundLight = Color.FromRgb(240, 241, 243),
                DisabledBackgroundDark = Color.FromRgb(42, 44, 47),
                DisabledForegroundLight = Color.FromRgb(133, 141, 151),
                DisabledForegroundDark = Color.FromRgb(119, 123, 128),
                DisabledBorderLight = Color.FromRgb(215, 219, 224),
                DisabledBorderDark = Color.FromRgb(61, 64, 68),
                GridLineLight = Color.FromRgb(215, 219, 224),
                GridLineDark = Color.FromRgb(57, 60, 64),
                GridHeaderBackgroundLight = Color.FromRgb(242, 244, 246),
                GridHeaderBackgroundDark = Color.FromRgb(42, 44, 47),
                InputBackgroundDark = Color.FromRgb(43, 45, 48),
                InputBorderLight = Color.FromRgb(184, 190, 198),
                InputBorderDark = Color.FromRgb(82, 85, 89),
                InputReadOnlyBackgroundLight = Color.FromRgb(243, 244, 245),
                InputReadOnlyBackgroundDark = Color.FromRgb(37, 39, 42),
                ToolbarBackgroundLight = Colors.Transparent,
                ToolbarBackgroundDark = Colors.Transparent,
                ToolbarBorderLight = Colors.Transparent,
                ToolbarBorderDark = Colors.Transparent,
                BreadcrumbBackgroundLight = Colors.Transparent,
                BreadcrumbBackgroundDark = Colors.Transparent,
                TabHeaderBorderLight = Color.FromRgb(213, 217, 222),
                TabHeaderBorderDark = Color.FromRgb(60, 63, 67),
                TabItemBackgroundHoverLight = Color.FromRgb(236, 240, 244),
                TabItemBackgroundHoverDark = Color.FromRgb(47, 49, 52),
                TabItemBorderHoverLight = Color.FromRgb(190, 196, 203),
                TabItemBorderHoverDark = Color.FromRgb(83, 87, 92),
                TabItemBorderSelectedLight = Color.FromRgb(184, 190, 198),
                TabItemBorderSelectedDark = Color.FromRgb(82, 85, 89),
                TabItemUnderlineSelectedLight = Color.FromRgb(15, 108, 189),
                TabItemUnderlineSelectedDark = Color.FromRgb(76, 166, 255),
                TabContentBorderLight = Color.FromRgb(213, 217, 222),
                TabContentBorderDark = Color.FromRgb(60, 63, 67),
                NavigationPanelForegroundLight = Color.FromRgb(31, 33, 36),
                NavigationPanelForegroundDark = Color.FromRgb(241, 243, 245),
                NavigationPanelBorderLight = Color.FromRgb(213, 217, 222),
                NavigationPanelBorderDark = Color.FromRgb(60, 63, 67),
                NavigationPanelItemHoverBackgroundLight = Color.FromRgb(233, 237, 242),
                NavigationPanelItemHoverBackgroundDark = Color.FromRgb(43, 45, 48),
                NavigationPanelItemSelectedBackgroundLight = Color.FromRgb(220, 235, 250),
                NavigationPanelItemSelectedBackgroundDark = Color.FromRgb(29, 58, 86),
                NavigationPanelItemSelectedForegroundLight = Color.FromRgb(22, 24, 27),
                NavigationPanelItemSelectedForegroundDark = Color.FromRgb(245, 247, 249)
            });
    }
    #endregion
}
#endregion
