// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetRose.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetRose ###
/// <summary>
/// Provides the built-in Rose theme preset.
/// </summary>
internal static class XThemePresetRose
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Rose theme.
    /// </summary>
    /// <returns>The built-in Rose theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Rose",
                PrimaryLight = Color.FromRgb(225, 29, 72),
                PrimaryDark = Color.FromRgb(253, 164, 175),
                AccentLight = Color.FromRgb(217, 70, 239),
                AccentDark = Color.FromRgb(240, 171, 252),
                BackgroundLight = Color.FromRgb(255, 247, 248),
                BackgroundDark = Color.FromRgb(28, 18, 23),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(43, 31, 37),
                NavigationLight = Color.FromRgb(76, 29, 49),
                NavigationDark = Color.FromRgb(35, 24, 30),
            });
    }
    #endregion
}
#endregion
