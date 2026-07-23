// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XThemePresetsTests ###
/// <summary>
/// Tests built-in theme preset exposure.
/// </summary>
public sealed class XThemePresetsTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that all built-in theme presets are exposed and have unique names.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldExposeUniqueNamedThemes()
    {
        string[] expectedNames =
        [
            "Default",
            "Amber",
            "Azure",
            "Crimson",
            "Emerald",
            "Graphite",
            "Indigo",
            "Magenta",
            "Rose",
            "Sandstone",
            "Teal",
            "Violet"
        ];

        IReadOnlyList<XTheme> themes = XThemePresets.BuiltInThemes;
        string[] actualNames = themes.Select(theme => theme.Name).ToArray();

        Assert.True(themes.Count >= expectedNames.Length);
        Assert.Equal(actualNames.Length, actualNames.Distinct(StringComparer.OrdinalIgnoreCase).Count());

        foreach (string expectedName in expectedNames)
        {
            Assert.Contains(actualNames, name => string.Equals(name, expectedName, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Verifies that the named preset properties are part of the built-in theme collection.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldContainNamedPresetInstances()
    {
        IReadOnlyList<XTheme> themes = XThemePresets.BuiltInThemes;

        Assert.Contains(XThemePresets.Default, themes);
        Assert.Contains(XThemePresets.Amber, themes);
        Assert.Contains(XThemePresets.Azure, themes);
        Assert.Contains(XThemePresets.Crimson, themes);
        Assert.Contains(XThemePresets.Emerald, themes);
        Assert.Contains(XThemePresets.Graphite, themes);
        Assert.Contains(XThemePresets.Indigo, themes);
        Assert.Contains(XThemePresets.Magenta, themes);
        Assert.Contains(XThemePresets.Rose, themes);
        Assert.Contains(XThemePresets.Sandstone, themes);
        Assert.Contains(XThemePresets.Teal, themes);
        Assert.Contains(XThemePresets.Violet, themes);
    }

    /// <summary>
    /// Verifies that every built-in theme contains the core color model data required by the theme manager.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldContainCoreColorData()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            Assert.False(string.IsNullOrWhiteSpace(theme.Name));
            Assert.NotNull(theme.ThemeModeForeground);
            Assert.NotNull(theme.ControlBorder);
            Assert.NotNull(theme.FocusBorder);
            Assert.NotNull(theme.Primary);
            Assert.NotNull(theme.Background);
            Assert.NotNull(theme.Surface);
            Assert.NotNull(theme.Border);
            Assert.NotNull(theme.Accent);
            Assert.NotNull(theme.Success);
            Assert.NotNull(theme.Warning);
            Assert.NotNull(theme.Danger);
            Assert.NotNull(theme.Info);
            Assert.NotNull(theme.NavigationPanelBackground);
            Assert.NotNull(theme.NavigationPanelForeground);
        }
    }

    /// <summary>
    /// Verifies that every built-in theme provides three visually ordered depth levels in both modes.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldProvideLayeredSurfaceDepth()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            double backgroundLight = GetRelativeLuminance(theme.Background.Light);
            double surfaceLight = GetRelativeLuminance(theme.Surface.Light);
            double inputLight = GetRelativeLuminance(theme.InputBackground.Light);

            Assert.True(surfaceLight > backgroundLight, $"{theme.Name} light surface must be lighter than its background.");
            Assert.True(inputLight > surfaceLight, $"{theme.Name} light inputs must be lighter than its surface.");

            double backgroundDark = GetRelativeLuminance(theme.Background.Dark);
            double surfaceDark = GetRelativeLuminance(theme.Surface.Dark);
            double inputDark = GetRelativeLuminance(theme.InputBackground.Dark);

            Assert.True(surfaceDark > backgroundDark, $"{theme.Name} dark surface must be lighter than its background.");
            Assert.True(inputDark > surfaceDark, $"{theme.Name} dark inputs must be lighter than its surface.");
        }
    }
    #endregion

    #region ### Private Methods ###
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
    #endregion
}
#endregion
