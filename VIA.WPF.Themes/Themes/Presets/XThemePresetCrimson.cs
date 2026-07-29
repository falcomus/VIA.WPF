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
                PrimaryLight = Color.FromRgb(185, 28, 28),
                PrimaryDark = Color.FromRgb(248, 113, 113),
                AccentLight = Color.FromRgb(180, 83, 9),
                AccentDark = Color.FromRgb(251, 191, 36),
                BackgroundLight = Color.FromRgb(248, 243, 242),
                BackgroundDark = Color.FromRgb(29, 23, 23),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(43, 35, 35),
                NavigationLight = Color.FromRgb(84, 17, 17),
                NavigationDark = Color.FromRgb(38, 22, 22),
            });
    }
    #endregion
}
#endregion
