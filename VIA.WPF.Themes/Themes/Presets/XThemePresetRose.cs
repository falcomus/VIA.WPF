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
                PrimaryLight = Color.FromRgb(190, 18, 60),
                PrimaryDark = Color.FromRgb(253, 164, 175),
                AccentLight = Color.FromRgb(79, 70, 229),
                AccentDark = Color.FromRgb(165, 180, 252),
                BackgroundLight = Color.FromRgb(247, 243, 244),
                BackgroundDark = Color.FromRgb(28, 24, 26),
                SurfaceLight = Color.FromRgb(255, 255, 255),
                SurfaceDark = Color.FromRgb(42, 36, 39),
                NavigationLight = Color.FromRgb(76, 29, 49),
                NavigationDark = Color.FromRgb(35, 24, 30),
            });
    }
    #endregion
}
#endregion
