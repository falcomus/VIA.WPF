// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetCrimson.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetCrimson ###
/// <summary>
/// Provides the built-in Crimson theme preset.
/// </summary>
internal static class XThemePresetCrimson
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Crimson theme.
    /// </summary>
    /// <returns>The built-in Crimson theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Crimson",
                PrimaryLight = Color.FromRgb(190, 18, 60),
                PrimaryDark = Color.FromRgb(251, 113, 133),
                AccentLight = Color.FromRgb(180, 83, 9),
                AccentDark = Color.FromRgb(251, 191, 36),
                BackgroundLight = Color.FromRgb(247, 242, 243),
                BackgroundDark = Color.FromRgb(28, 23, 25),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(42, 35, 38),
                NavigationLight = Color.FromRgb(76, 5, 25),
                NavigationDark = Color.FromRgb(36, 22, 26),
            });
    }
    #endregion
}
#endregion
