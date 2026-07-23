// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeModeColor.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemeModeColor ###
/// <summary>
/// Represents a simple light and dark color pair.
/// </summary>
public sealed class XThemeModeColor
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the color for light mode.
    /// </summary>
    public required Color Light { get; init; }

    /// <summary>
    /// Gets or sets the color for dark mode.
    /// </summary>
    public required Color Dark { get; init; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the color for the specified theme mode.
    /// </summary>
    /// <param name="mode">The target theme mode.</param>
    /// <returns>The color for the specified mode.</returns>
    public Color GetColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark
            ? this.Dark
            : this.Light;
    }
    #endregion
}
#endregion