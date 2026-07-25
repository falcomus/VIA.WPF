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
                PrimaryLight = Color.FromRgb(4, 120, 87),
                PrimaryDark = Color.FromRgb(110, 231, 183),
                AccentLight = Color.FromRgb(79, 70, 229),
                AccentDark = Color.FromRgb(165, 180, 252),
                InfoLight = Color.FromRgb(3, 105, 161),
                InfoDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(241, 246, 244),
                BackgroundDark = Color.FromRgb(19, 27, 24),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(34, 44, 40),
                NavigationLight = Color.FromRgb(6, 78, 59),
                NavigationDark = Color.FromRgb(8, 31, 28),
            });
    }
    #endregion
}
#endregion
