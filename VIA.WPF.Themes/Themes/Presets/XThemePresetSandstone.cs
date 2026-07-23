// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetSandstone.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetSandstone ###
/// <summary>
/// Provides the built-in Sandstone theme preset.
/// </summary>
internal static class XThemePresetSandstone
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Sandstone theme.
    /// </summary>
    /// <returns>The built-in Sandstone theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Sandstone",
                PrimaryLight = Color.FromRgb(120, 53, 15),
                PrimaryDark = Color.FromRgb(253, 186, 116),
                AccentLight = Color.FromRgb(101, 163, 13),
                AccentDark = Color.FromRgb(190, 242, 100),
                BackgroundLight = Color.FromRgb(250, 248, 241),
                BackgroundDark = Color.FromRgb(25, 22, 17),
                SurfaceLight = Color.FromRgb(255, 255, 252),
                SurfaceDark = Color.FromRgb(38, 34, 27),
                NavigationLight = Color.FromRgb(68, 41, 22),
                NavigationDark = Color.FromRgb(31, 27, 22),
            });
    }
    #endregion
}
#endregion
