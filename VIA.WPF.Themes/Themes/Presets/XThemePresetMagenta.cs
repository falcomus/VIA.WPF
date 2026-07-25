// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetMagenta.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetMagenta ###
/// <summary>
/// Provides the built-in Magenta theme preset.
/// </summary>
internal static class XThemePresetMagenta
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Magenta theme.
    /// </summary>
    /// <returns>The built-in Magenta theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Magenta",
                PrimaryLight = Color.FromRgb(162, 28, 175),
                PrimaryDark = Color.FromRgb(240, 171, 252),
                AccentLight = Color.FromRgb(3, 105, 161),
                AccentDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(247, 242, 247),
                BackgroundDark = Color.FromRgb(27, 23, 29),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(41, 35, 43),
                NavigationLight = Color.FromRgb(74, 4, 78),
                NavigationDark = Color.FromRgb(34, 23, 38),
            });
    }
    #endregion
}
#endregion
