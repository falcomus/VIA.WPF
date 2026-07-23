// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetViolet.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetViolet ###
/// <summary>
/// Provides the built-in Violet theme preset.
/// </summary>
internal static class XThemePresetViolet
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates the built-in Violet theme.
    /// </summary>
    /// <returns>The built-in Violet theme.</returns>
    public static XTheme Create()
    {
        return XThemePresetFactory.Create(
            new XThemePalette
            {
                Name = "Violet",
                PrimaryLight = Color.FromRgb(109, 40, 217),
                PrimaryDark = Color.FromRgb(196, 181, 253),
                AccentLight = Color.FromRgb(147, 51, 234),
                AccentDark = Color.FromRgb(216, 180, 254),
                BackgroundLight = Color.FromRgb(250, 245, 255),
                BackgroundDark = Color.FromRgb(18, 16, 30),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(31, 28, 45),
                NavigationLight = Color.FromRgb(49, 21, 82),
                NavigationDark = Color.FromRgb(24, 21, 36),
            });
    }
    #endregion
}
#endregion
