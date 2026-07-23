// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeModelTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XThemeModelTests ###
/// <summary>
/// Tests the public theme model helpers.
/// </summary>
public sealed class XThemeModelTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that mode colors resolve the correct color for each mode.
    /// </summary>
    [Fact]
    public void XThemeModeColor_GetColor_ShouldReturnModeSpecificColor()
    {
        XThemeModeColor color = new()
        {
            Light = Colors.White,
            Dark = Colors.Black
        };

        Assert.Equal(Colors.White, color.GetColor(XThemeMode.Light));
        Assert.Equal(Colors.Black, color.GetColor(XThemeMode.Dark));
    }

    /// <summary>
    /// Verifies that explicit color sets resolve all light and dark mode colors correctly.
    /// </summary>
    [Fact]
    public void XThemeColorSet_GetColorMethods_ShouldReturnModeSpecificColors()
    {
        XThemeColorSet colorSet = new()
        {
            Light = Color.FromRgb(1, 2, 3),
            Dark = Color.FromRgb(4, 5, 6),
            TextLight = Color.FromRgb(7, 8, 9),
            TextDark = Color.FromRgb(10, 11, 12),
            VeryLightVariantLight = Color.FromRgb(13, 14, 15),
            VeryLightVariantDark = Color.FromRgb(16, 17, 18),
            LightVariantLight = Color.FromRgb(19, 20, 21),
            LightVariantDark = Color.FromRgb(22, 23, 24),
            DarkVariantLight = Color.FromRgb(25, 26, 27),
            DarkVariantDark = Color.FromRgb(28, 29, 30)
        };

        Assert.Equal(Color.FromRgb(1, 2, 3), colorSet.GetBaseColor(XThemeMode.Light));
        Assert.Equal(Color.FromRgb(4, 5, 6), colorSet.GetBaseColor(XThemeMode.Dark));
        Assert.Equal(Color.FromRgb(7, 8, 9), colorSet.GetTextColor(XThemeMode.Light));
        Assert.Equal(Color.FromRgb(10, 11, 12), colorSet.GetTextColor(XThemeMode.Dark));
        Assert.Equal(Color.FromRgb(13, 14, 15), colorSet.GetVeryLightVariantColor(XThemeMode.Light));
        Assert.Equal(Color.FromRgb(16, 17, 18), colorSet.GetVeryLightVariantColor(XThemeMode.Dark));
        Assert.Equal(Color.FromRgb(19, 20, 21), colorSet.GetLightVariantColor(XThemeMode.Light));
        Assert.Equal(Color.FromRgb(22, 23, 24), colorSet.GetLightVariantColor(XThemeMode.Dark));
        Assert.Equal(Color.FromRgb(25, 26, 27), colorSet.GetDarkVariantColor(XThemeMode.Light));
        Assert.Equal(Color.FromRgb(28, 29, 30), colorSet.GetDarkVariantColor(XThemeMode.Dark));
    }

    /// <summary>
    /// Verifies that semantic color sets preserve the supplied base colors and generate readable variants.
    /// </summary>
    [Fact]
    public void XThemeColorSet_CreateSemantic_ShouldUseBaseColorsAndGenerateVariants()
    {
        Color light = Color.FromRgb(37, 99, 235);
        Color dark = Color.FromRgb(147, 197, 253);
        Color lightSurface = Colors.White;
        Color darkSurface = Color.FromRgb(15, 23, 42);

        XThemeColorSet colorSet = XThemeColorSet.CreateSemantic(light, dark, lightSurface, darkSurface);

        Assert.Equal(light, colorSet.Light);
        Assert.Equal(dark, colorSet.Dark);
        Assert.NotEqual(default, colorSet.TextLight);
        Assert.NotEqual(default, colorSet.TextDark);
        Assert.NotEqual(default, colorSet.VeryLightVariantLight);
        Assert.NotEqual(default, colorSet.VeryLightVariantDark);
        Assert.NotEqual(default, colorSet.LightVariantLight);
        Assert.NotEqual(default, colorSet.LightVariantDark);
        Assert.NotEqual(default, colorSet.DarkVariantLight);
        Assert.NotEqual(default, colorSet.DarkVariantDark);
    }

    /// <summary>
    /// Verifies that neutral color sets preserve explicit foreground colors.
    /// </summary>
    [Fact]
    public void XThemeColorSet_CreateNeutral_ShouldPreserveTextColors()
    {
        Color light = Color.FromRgb(226, 232, 240);
        Color dark = Color.FromRgb(30, 41, 59);
        Color textLight = Color.FromRgb(15, 23, 42);
        Color textDark = Colors.White;

        XThemeColorSet colorSet = XThemeColorSet.CreateNeutral(light, dark, textLight, textDark);

        Assert.Equal(light, colorSet.Light);
        Assert.Equal(dark, colorSet.Dark);
        Assert.Equal(textLight, colorSet.TextLight);
        Assert.Equal(textDark, colorSet.TextDark);
    }
    #endregion
}
#endregion
