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
                AccentLight = Color.FromRgb(3, 105, 161),
                AccentDark = Color.FromRgb(125, 211, 252),
                BackgroundLight = Color.FromRgb(242, 244, 247),
                BackgroundDark = Color.FromRgb(21, 24, 29),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(36, 40, 47),
                NavigationLight = Color.FromRgb(17, 24, 39),
                NavigationDark = Color.FromRgb(11, 17, 29),

                SelectionBorderLight = Colors.DodgerBlue.WithAlpha(100),
                SelectionBorderDark = Colors.DodgerBlue.WithAlpha(60),
                
            });
    }
    #endregion
}
#endregion
