// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeColorUtility.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemeColorUtility ###
/// <summary>
/// Provides color helpers for VIA.WPF theme presets.
/// </summary>
internal static class XThemeColorUtility
{
    #region ### Private Constants ###
    private const double MinimumReadableContrastRatio = 4.5d;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates a semantic color set with automatically calculated foreground and container colors.
    /// </summary>
    /// <param name="light">The base color for light mode.</param>
    /// <param name="dark">The base color for dark mode.</param>
    /// <param name="lightSurface">The light mode surface color.</param>
    /// <param name="darkSurface">The dark mode surface color.</param>
    /// <returns>The generated color set.</returns>
    public static XThemeColorSet CreateSemanticColorSet(Color light, Color dark, Color lightSurface, Color darkSurface)
    {
        return new XThemeColorSet
        {
            Light = light,
            Dark = dark,
            TextLight = GetReadableForeground(light),
            TextDark = GetReadableForeground(dark),
            VeryLightVariantLight = Mix(lightSurface, light, 0.10d),
            VeryLightVariantDark = Mix(darkSurface, dark, 0.10d),
            LightVariantLight = Mix(lightSurface, light, 0.22d),
            LightVariantDark = Mix(darkSurface, dark, 0.17d),
            DarkVariantLight = DarkenForText(light),
            DarkVariantDark = LightenForText(dark)
        };
    }

    /// <summary>
    /// Creates a neutral color set with explicitly assigned readable text colors.
    /// </summary>
    /// <param name="light">The base color for light mode.</param>
    /// <param name="dark">The base color for dark mode.</param>
    /// <param name="textLight">The text color for light mode.</param>
    /// <param name="textDark">The text color for dark mode.</param>
    /// <returns>The generated neutral color set.</returns>
    public static XThemeColorSet CreateNeutralColorSet(Color light, Color dark, Color textLight, Color textDark)
    {
        return new XThemeColorSet
        {
            Light = light,
            Dark = dark,
            TextLight = textLight,
            TextDark = textDark,
            VeryLightVariantLight = Mix(light, Colors.White, 0.72d),
            VeryLightVariantDark = Mix(dark, Colors.White, 0.04d),
            LightVariantLight = Mix(light, Colors.White, 0.44d),
            LightVariantDark = Mix(dark, Colors.White, 0.07d),
            DarkVariantLight = Mix(light, Colors.Black, 0.08d),
            DarkVariantDark = Mix(dark, Colors.Black, 0.16d)
        };
    }

    /// <summary>
    /// Creates a light and dark color pair.
    /// </summary>
    /// <param name="light">The light mode color.</param>
    /// <param name="dark">The dark mode color.</param>
    /// <returns>The generated color pair.</returns>
    public static XThemeModeColor CreateModeColor(Color light, Color dark)
    {
        return new XThemeModeColor
        {
            Light = light,
            Dark = dark
        };
    }

    /// <summary>
    /// Gets the foreground color with the highest accessible contrast for the specified background color.
    /// </summary>
    /// <param name="background">The background color.</param>
    /// <returns>The readable foreground color.</returns>
    public static Color GetReadableForeground(Color background)
    {
        Color lightText = Colors.White;
        Color darkText = Color.FromRgb(15, 23, 42);

        double lightContrast = GetContrastRatio(background, lightText);
        double darkContrast = GetContrastRatio(background, darkText);

        if (lightContrast >= MinimumReadableContrastRatio && lightContrast >= darkContrast)
        {
            return lightText;
        }

        if (darkContrast >= MinimumReadableContrastRatio)
        {
            return darkText;
        }

        return lightContrast >= darkContrast
            ? lightText
            : darkText;
    }

    /// <summary>
    /// Gets the WCAG contrast ratio between two colors.
    /// </summary>
    /// <param name="first">The first color.</param>
    /// <param name="second">The second color.</param>
    /// <returns>The contrast ratio.</returns>
    public static double GetContrastRatio(Color first, Color second)
    {
        double firstLuminance = GetRelativeLuminance(first);
        double secondLuminance = GetRelativeLuminance(second);
        double lighter = Math.Max(firstLuminance, secondLuminance);
        double darker = Math.Min(firstLuminance, secondLuminance);

        return (lighter + 0.05d) / (darker + 0.05d);
    }

    /// <summary>
    /// Mixes two colors.
    /// </summary>
    /// <param name="from">The source color.</param>
    /// <param name="to">The target color.</param>
    /// <param name="amount">The amount of the target color.</param>
    /// <returns>The mixed color.</returns>
    public static Color Mix(Color from, Color to, double amount)
    {
        amount = Math.Clamp(amount, 0d, 1d);

        return Color.FromArgb(
            MixByte(from.A, to.A, amount),
            MixByte(from.R, to.R, amount),
            MixByte(from.G, to.G, amount),
            MixByte(from.B, to.B, amount));
    }
    #endregion

    #region ### Private Methods ###
    private static Color DarkenForText(Color color)
    {
        Color candidate = Mix(color, Color.FromRgb(15, 23, 42), 0.24d);

        return GetContrastRatio(candidate, Mix(Colors.White, color, 0.14d)) >= MinimumReadableContrastRatio
            ? candidate
            : Mix(color, Color.FromRgb(15, 23, 42), 0.36d);
    }

    private static Color LightenForText(Color color)
    {
        Color candidate = Mix(color, Colors.White, 0.12d);

        return GetContrastRatio(candidate, Color.FromRgb(18, 24, 38)) >= MinimumReadableContrastRatio
            ? candidate
            : Mix(color, Colors.White, 0.24d);
    }

    private static double GetRelativeLuminance(Color color)
    {
        double red = ConvertToLinearRgb(color.R / 255d);
        double green = ConvertToLinearRgb(color.G / 255d);
        double blue = ConvertToLinearRgb(color.B / 255d);

        return (0.2126d * red) + (0.7152d * green) + (0.0722d * blue);
    }

    private static double ConvertToLinearRgb(double value)
    {
        return value <= 0.03928d
            ? value / 12.92d
            : Math.Pow((value + 0.055d) / 1.055d, 2.4d);
    }

    private static byte MixByte(byte from, byte to, double amount)
    {
        return (byte)Math.Round(from + ((to - from) * amount));
    }
    #endregion
}
#endregion
