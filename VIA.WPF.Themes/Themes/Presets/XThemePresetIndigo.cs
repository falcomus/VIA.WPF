// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetIndigo.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetIndigo ###
/// <summary>
/// Provides the built-in Indigo theme preset.
/// </summary>
internal static class XThemePresetIndigo
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Indigo theme.
    /// </summary>
    /// <returns>The built-in Indigo theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Indigo",
                PrimaryLight = Color.FromRgb(79, 70, 229),
                PrimaryDark = Color.FromRgb(165, 180, 252),
                AccentLight = Color.FromRgb(124, 58, 237),
                AccentDark = Color.FromRgb(196, 181, 253),
                BackgroundLight = Color.FromRgb(245, 247, 255),
                BackgroundDark = Color.FromRgb(14, 18, 34),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(27, 32, 50),
                NavigationLight = Color.FromRgb(30, 41, 93),
                NavigationDark = Color.FromRgb(18, 23, 42),
            });
    }
    #endregion
}
#endregion
