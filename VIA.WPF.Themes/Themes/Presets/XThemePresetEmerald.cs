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
                PrimaryLight = Color.FromRgb(21, 128, 61),
                PrimaryDark = Color.FromRgb(134, 239, 172),
                AccentLight = Color.FromRgb(79, 70, 229),
                AccentDark = Color.FromRgb(165, 180, 252),
                InfoLight = Color.FromRgb(3, 105, 161),
                InfoDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(241, 247, 242),
                BackgroundDark = Color.FromRgb(19, 28, 22),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(34, 45, 38),
                NavigationLight = Color.FromRgb(20, 83, 45),
                NavigationDark = Color.FromRgb(10, 34, 24),
            });
    }
    #endregion
}
#endregion
