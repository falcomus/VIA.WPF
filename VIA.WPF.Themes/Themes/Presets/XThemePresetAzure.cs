// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetAzure.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetAzure ###
/// <summary>
/// Provides the built-in Azure theme preset.
/// </summary>
internal static class XThemePresetAzure
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Azure theme.
    /// </summary>
    /// <returns>The built-in Azure theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Azure",
                PrimaryLight = Color.FromRgb(3, 105, 161),
                PrimaryDark = Color.FromRgb(125, 211, 252),
                AccentLight = Color.FromRgb(109, 40, 217),
                AccentDark = Color.FromRgb(196, 181, 253),
                BackgroundLight = Color.FromRgb(241, 245, 248),
                BackgroundDark = Color.FromRgb(19, 26, 32),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(35, 43, 50),
                NavigationLight = Color.FromRgb(12, 50, 76),
                NavigationDark = Color.FromRgb(10, 28, 40),
            });
    }
    #endregion
}
#endregion
