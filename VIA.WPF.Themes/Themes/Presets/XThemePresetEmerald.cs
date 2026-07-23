// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetEmerald.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetEmerald ###
/// <summary>
/// Provides the built-in Emerald theme preset.
/// </summary>
internal static class XThemePresetEmerald
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Emerald theme.
    /// </summary>
    /// <returns>The built-in Emerald theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Emerald",
                PrimaryLight = Color.FromRgb(5, 150, 105),
                PrimaryDark = Color.FromRgb(110, 231, 183),
                AccentLight = Color.FromRgb(13, 148, 136),
                AccentDark = Color.FromRgb(94, 234, 212),
                BackgroundLight = Color.FromRgb(240, 253, 250),
                BackgroundDark = Color.FromRgb(7, 24, 22),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(18, 38, 34),
                NavigationLight = Color.FromRgb(6, 78, 59),
                NavigationDark = Color.FromRgb(8, 31, 28),
            });
    }
    #endregion
}
#endregion
