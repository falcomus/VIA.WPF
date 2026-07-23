// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeColorExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemeColorExtensions ###
/// <summary>
/// Provides fluent extension methods for working with WPF colors in VIA.WPF theme definitions.
/// </summary>
public static class XThemeColorExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Returns the specified color with a replaced alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="alpha">The alpha channel value from 0 to 255.</param>
    /// <returns>The color with the specified alpha channel.</returns>
    public static Color WithAlpha(this Color color, byte alpha)
    {
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }

    /// <summary>
    /// Returns the specified color with a replaced alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="alpha">The alpha channel value from 0 to 255.</param>
    /// <returns>The color with the specified alpha channel.</returns>
    public static Color WithAlpha(this Color color, int alpha)
    {
        return color.WithAlpha((byte)Math.Clamp(alpha, 0, 255));
    }

    /// <summary>
    /// Returns the specified color with a replaced red channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="red">The red channel value from 0 to 255.</param>
    /// <returns>The color with the specified red channel.</returns>
    public static Color WithRed(this Color color, int red)
    {
        return Color.FromArgb(color.A, (byte)Math.Clamp(red, 0, 255), color.G, color.B);
    }

    /// <summary>
    /// Returns the specified color with a replaced green channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="green">The green channel value from 0 to 255.</param>
    /// <returns>The color with the specified green channel.</returns>
    public static Color WithGreen(this Color color, int green)
    {
        return Color.FromArgb(color.A, color.R, (byte)Math.Clamp(green, 0, 255), color.B);
    }

    /// <summary>
    /// Returns the specified color with a replaced blue channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="blue">The blue channel value from 0 to 255.</param>
    /// <returns>The color with the specified blue channel.</returns>
    public static Color WithBlue(this Color color, int blue)
    {
        return Color.FromArgb(color.A, color.R, color.G, (byte)Math.Clamp(blue, 0, 255));
    }

    /// <summary>
    /// Returns the specified color with a replaced opacity.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="opacity">The opacity value from 0.0 to 1.0.</param>
    /// <returns>The color with the specified opacity.</returns>
    public static Color WithOpacity(this Color color, double opacity)
    {
        return color.WithAlpha((byte)Math.Round(Math.Clamp(opacity, 0d, 1d) * 255d));
    }

    /// <summary>
    /// Reduces the opacity of the specified color by the specified amount.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="amount">The transparency amount from 0.0 to 1.0.</param>
    /// <returns>The transparentized color.</returns>
    public static Color Transparentize(this Color color, double amount)
    {
        double multiplier = 1d - Math.Clamp(amount, 0d, 1d);

        return color.WithAlpha((byte)Math.Round(color.A * multiplier));
    }

    /// <summary>
    /// Mixes the specified color with another color.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="target">The target color.</param>
    /// <param name="amount">The amount of the target color from 0.0 to 1.0.</param>
    /// <returns>The mixed color.</returns>
    public static Color MixWith(this Color color, Color target, double amount)
    {
        return XThemeColorUtility.Mix(color, target, amount);
    }

    /// <summary>
    /// Lightens the specified color by mixing it with white while preserving the alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="amount">The amount of white from 0.0 to 1.0.</param>
    /// <returns>The lightened color.</returns>
    public static Color Lighten(this Color color, double amount)
    {
        Color target = Color.FromArgb(color.A, 255, 255, 255);

        return color.MixWith(target, amount);
    }

    /// <summary>
    /// Darkens the specified color by mixing it with black while preserving the alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="amount">The amount of black from 0.0 to 1.0.</param>
    /// <returns>The darkened color.</returns>
    public static Color Darken(this Color color, double amount)
    {
        Color target = Color.FromArgb(color.A, 0, 0, 0);

        return color.MixWith(target, amount);
    }

    /// <summary>
    /// Tints the specified color by mixing it with white while preserving the alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="amount">The amount of white from 0.0 to 1.0.</param>
    /// <returns>The tinted color.</returns>
    public static Color Tint(this Color color, double amount)
    {
        return color.Lighten(amount);
    }

    /// <summary>
    /// Shades the specified color by mixing it with black while preserving the alpha channel.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="amount">The amount of black from 0.0 to 1.0.</param>
    /// <returns>The shaded color.</returns>
    public static Color Shade(this Color color, double amount)
    {
        return color.Darken(amount);
    }

    /// <summary>
    /// Alpha-composites the specified foreground color over a background color.
    /// </summary>
    /// <param name="foreground">The foreground color.</param>
    /// <param name="background">The background color.</param>
    /// <returns>The visible color after compositing.</returns>
    public static Color OverlayOn(this Color foreground, Color background)
    {
        double foregroundAlpha = foreground.A / 255d;
        double backgroundAlpha = background.A / 255d;
        double resultAlpha = foregroundAlpha + (backgroundAlpha * (1d - foregroundAlpha));

        if (resultAlpha <= 0d)
        {
            return Colors.Transparent;
        }

        return Color.FromArgb(
            ToByte(resultAlpha * 255d),
            ToByte(((foreground.R * foregroundAlpha) + (background.R * backgroundAlpha * (1d - foregroundAlpha))) / resultAlpha),
            ToByte(((foreground.G * foregroundAlpha) + (background.G * backgroundAlpha * (1d - foregroundAlpha))) / resultAlpha),
            ToByte(((foreground.B * foregroundAlpha) + (background.B * backgroundAlpha * (1d - foregroundAlpha))) / resultAlpha));
    }

    /// <summary>
    /// Adjusts the color toward black or white until the requested contrast ratio on the specified background is reached.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <param name="background">The background color.</param>
    /// <param name="minimumContrastRatio">The minimum WCAG contrast ratio.</param>
    /// <returns>The adjusted color.</returns>
    public static Color EnsureContrastOn(this Color color, Color background, double minimumContrastRatio = 4.5d)
    {
        minimumContrastRatio = Math.Max(1d, minimumContrastRatio);

        if (color.OverlayOn(background).GetContrastRatio(background) >= minimumContrastRatio)
        {
            return color;
        }

        Color target = Colors.Black.GetContrastRatio(background) >= Colors.White.GetContrastRatio(background)
            ? Color.FromArgb(color.A, 0, 0, 0)
            : Color.FromArgb(color.A, 255, 255, 255);

        Color best = color;
        double bestContrast = color.OverlayOn(background).GetContrastRatio(background);

        for (int i = 1; i <= 32; i++)
        {
            Color candidate = color.MixWith(target, i / 32d);
            double candidateContrast = candidate.OverlayOn(background).GetContrastRatio(background);

            if (candidateContrast > bestContrast)
            {
                best = candidate;
                bestContrast = candidateContrast;
            }

            if (candidateContrast >= minimumContrastRatio)
            {
                return candidate;
            }
        }

        return best;
    }

    /// <summary>
    /// Returns a readable foreground color for the specified background color.
    /// </summary>
    /// <param name="background">The background color.</param>
    /// <returns>A readable foreground color.</returns>
    public static Color GetReadableForeground(this Color background)
    {
        return XThemeColorUtility.GetReadableForeground(background);
    }

    /// <summary>
    /// Returns the WCAG contrast ratio between the specified colors.
    /// </summary>
    /// <param name="color">The first color.</param>
    /// <param name="other">The second color.</param>
    /// <returns>The WCAG contrast ratio.</returns>
    public static double GetContrastRatio(this Color color, Color other)
    {
        return XThemeColorUtility.GetContrastRatio(color, other);
    }

    /// <summary>
    /// Gets a value indicating whether the specified color is visually light.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns><c>true</c> if the color is visually light; otherwise, <c>false</c>.</returns>
    public static bool IsLight(this Color color)
    {
        return color.GetContrastRatio(Colors.Black) >= color.GetContrastRatio(Colors.White);
    }

    /// <summary>
    /// Gets a value indicating whether the specified color is visually dark.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns><c>true</c> if the color is visually dark; otherwise, <c>false</c>.</returns>
    public static bool IsDark(this Color color)
    {
        return !color.IsLight();
    }

    /// <summary>
    /// Creates a frozen solid color brush from the specified color.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The generated solid color brush.</returns>
    public static SolidColorBrush ToBrush(this Color color)
    {
        SolidColorBrush brush = new(color);

        if (brush.CanFreeze)
        {
            brush.Freeze();
        }

        return brush;
    }

    /// <summary>
    /// Converts the specified color to a hexadecimal RGB string.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The hexadecimal RGB string.</returns>
    public static string ToHex(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Converts the specified color to a hexadecimal ARGB string.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The hexadecimal ARGB string.</returns>
    public static string ToHexWithAlpha(this Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Converts the specified channel value to a byte.
    /// </summary>
    /// <param name="value">The channel value.</param>
    /// <returns>The byte value.</returns>
    private static byte ToByte(double value)
    {
        return (byte)Math.Clamp(Math.Round(value), 0d, 255d);
    }
    #endregion
}
#endregion
