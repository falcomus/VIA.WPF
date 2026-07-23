// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetTeal.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetTeal ###
/// <summary>
/// Provides the built-in Teal theme preset.
/// </summary>
internal static class XThemePresetTeal
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Teal theme.
    /// </summary>
    /// <returns>The built-in Teal theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Teal",
                PrimaryLight = Color.FromRgb(15, 118, 110),
                PrimaryDark = Color.FromRgb(94, 234, 212),
                AccentLight = Color.FromRgb(14, 165, 233),
                AccentDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(240, 253, 250),
                BackgroundDark = Color.FromRgb(7, 25, 28),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(18, 39, 43),
                NavigationLight = Color.FromRgb(19, 78, 74),
                NavigationDark = Color.FromRgb(9, 32, 35),
            });
    }
    #endregion
}
#endregion
