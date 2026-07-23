// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetGraphite.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetGraphite ###
/// <summary>
/// Provides the built-in Graphite theme preset.
/// </summary>
internal static class XThemePresetGraphite
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Graphite theme.
    /// </summary>
    /// <returns>The built-in Graphite theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Graphite",
                PrimaryLight = Color.FromRgb(51, 65, 85),
                PrimaryDark = Color.FromRgb(203, 213, 225),
                AccentLight = Color.FromRgb(14, 165, 233),
                AccentDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(248, 250, 252),
                BackgroundDark = Color.FromRgb(10, 15, 26),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(22, 29, 43),
                NavigationLight = Color.FromRgb(17, 24, 39),
                NavigationDark = Color.FromRgb(11, 17, 29),

                SelectionBorderLight = Colors.DodgerBlue.WithAlpha(100),
                SelectionBorderDark = Colors.DodgerBlue.WithAlpha(60),
                
            });
    }
    #endregion
}
#endregion
