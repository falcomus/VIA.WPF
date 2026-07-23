// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetAmber.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetAmber ###
/// <summary>
/// Provides the built-in Amber theme preset.
/// </summary>
internal static class XThemePresetAmber
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Amber theme.
    /// </summary>
    /// <returns>The built-in Amber theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Amber",
                PrimaryLight = Color.FromRgb(180, 83, 9),
                PrimaryDark = Color.FromRgb(251, 191, 36),
                AccentLight = Color.FromRgb(217, 119, 6),
                AccentDark = Color.FromRgb(252, 211, 77),
                BackgroundLight = Color.FromRgb(255, 251, 235),
                BackgroundDark = Color.FromRgb(28, 21, 11),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(43, 34, 22),
                NavigationLight = Color.FromRgb(69, 26, 3),
                NavigationDark = Color.FromRgb(35, 25, 14),
            });
    }
    #endregion
}
#endregion
