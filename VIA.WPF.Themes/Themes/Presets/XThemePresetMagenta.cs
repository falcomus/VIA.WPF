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
                PrimaryLight = Color.FromRgb(192, 38, 211),
                PrimaryDark = Color.FromRgb(240, 171, 252),
                AccentLight = Color.FromRgb(236, 72, 153),
                AccentDark = Color.FromRgb(249, 168, 212),
                BackgroundLight = Color.FromRgb(253, 244, 255),
                BackgroundDark = Color.FromRgb(27, 17, 30),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(42, 30, 47),
                NavigationLight = Color.FromRgb(74, 4, 78),
                NavigationDark = Color.FromRgb(34, 23, 38),
            });
    }
    #endregion
}
#endregion
