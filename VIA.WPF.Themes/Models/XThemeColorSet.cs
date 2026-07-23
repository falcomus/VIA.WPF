// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeColorSet.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemeColorSet ###
/// <summary>
/// Describes a cohesive set of theme colors for light and dark modes.
/// </summary>
public sealed class XThemeColorSet
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the primary color for light mode.
    /// </summary>
    public required Color Light { get; init; }

    /// <summary>
    /// Gets or sets the primary color for dark mode.
    /// </summary>
    public required Color Dark { get; init; }

    /// <summary>
    /// Gets or sets the text color for the primary color in light mode.
    /// </summary>
    public required Color TextLight { get; init; }

    /// <summary>
    /// Gets or sets the text color for the primary color in dark mode.
    /// </summary>
    public required Color TextDark { get; init; }

    /// <summary>
    /// Gets or sets the very light variant color for light mode.
    /// </summary>
    public required Color VeryLightVariantLight { get; init; }

    /// <summary>
    /// Gets or sets the very light variant color for dark mode.
    /// </summary>
    public required Color VeryLightVariantDark { get; init; }

    /// <summary>
    /// Gets or sets the light variant color for light mode.
    /// </summary>
    public required Color LightVariantLight { get; init; }

    /// <summary>
    /// Gets or sets the light variant color for dark mode.
    /// </summary>
    public required Color LightVariantDark { get; init; }

    /// <summary>
    /// Gets or sets the dark variant color for light mode.
    /// </summary>
    public required Color DarkVariantLight { get; init; }

    /// <summary>
    /// Gets or sets the dark variant color for dark mode.
    /// </summary>
    public required Color DarkVariantDark { get; init; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates a semantic color set and automatically calculates readable foreground and variant colors.
    /// </summary>
    /// <param name="light">The base color for light mode.</param>
    /// <param name="dark">The base color for dark mode.</param>
    /// <param name="lightSurface">The surface color used to generate light mode container colors.</param>
    /// <param name="darkSurface">The surface color used to generate dark mode container colors.</param>
    /// <returns>The generated color set.</returns>
    public static XThemeColorSet CreateSemantic(Color light, Color dark, Color lightSurface, Color darkSurface)
    {
        return XThemeColorUtility.CreateSemanticColorSet(light, dark, lightSurface, darkSurface);
    }

    /// <summary>
    /// Creates a neutral color set with explicit foreground colors and generated variants.
    /// </summary>
    /// <param name="light">The base color for light mode.</param>
    /// <param name="dark">The base color for dark mode.</param>
    /// <param name="textLight">The text color for light mode.</param>
    /// <param name="textDark">The text color for dark mode.</param>
    /// <returns>The generated color set.</returns>
    public static XThemeColorSet CreateNeutral(Color light, Color dark, Color textLight, Color textDark)
    {
        return XThemeColorUtility.CreateNeutralColorSet(light, dark, textLight, textDark);
    }

    /// <summary>
    /// Gets a readable foreground color for the specified background color.
    /// </summary>
    /// <param name="background">The background color.</param>
    /// <returns>A readable foreground color.</returns>
    public static Color GetReadableTextColor(Color background)
    {
        return XThemeColorUtility.GetReadableForeground(background);
    }

    /// <summary>
    /// Gets the primary color for the specified mode.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    /// <returns>The corresponding color.</returns>
    public Color GetBaseColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark ? this.Dark : this.Light;
    }

    /// <summary>
    /// Gets the text color for the specified mode.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    /// <returns>The corresponding color.</returns>
    public Color GetTextColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark ? this.TextDark : this.TextLight;
    }

    /// <summary>
    /// Gets the very light variant color for the specified mode.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    /// <returns>The corresponding color.</returns>
    public Color GetVeryLightVariantColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark ? this.VeryLightVariantDark : this.VeryLightVariantLight;
    }

    /// <summary>
    /// Gets the light variant color for the specified mode.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    /// <returns>The corresponding color.</returns>
    public Color GetLightVariantColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark ? this.LightVariantDark : this.LightVariantLight;
    }

    /// <summary>
    /// Gets the dark variant color for the specified mode.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    /// <returns>The corresponding color.</returns>
    public Color GetDarkVariantColor(XThemeMode mode)
    {
        return mode == XThemeMode.Dark ? this.DarkVariantDark : this.DarkVariantLight;
    }
    #endregion
}
#endregion