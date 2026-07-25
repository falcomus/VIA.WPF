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
                AccentLight = Color.FromRgb(180, 83, 9),
                AccentDark = Color.FromRgb(251, 191, 36),
                BackgroundLight = Color.FromRgb(245, 242, 248),
                BackgroundDark = Color.FromRgb(25, 23, 31),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(39, 36, 47),
                NavigationLight = Color.FromRgb(49, 21, 82),
                NavigationDark = Color.FromRgb(24, 21, 36),
            });
    }
    #endregion
}
#endregion
