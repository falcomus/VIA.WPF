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
                PrimaryLight = Color.FromRgb(2, 132, 199),
                PrimaryDark = Color.FromRgb(125, 211, 252),
                AccentLight = Color.FromRgb(20, 184, 166),
                AccentDark = Color.FromRgb(94, 234, 212),
                BackgroundLight = Color.FromRgb(240, 249, 255),
                BackgroundDark = Color.FromRgb(8, 22, 32),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(19, 33, 43),
                NavigationLight = Color.FromRgb(12, 50, 76),
                NavigationDark = Color.FromRgb(10, 28, 40),
            });
    }
    #endregion
}
#endregion
