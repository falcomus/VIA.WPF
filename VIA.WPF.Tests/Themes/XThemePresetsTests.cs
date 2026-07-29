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

    /// <summary>
    /// Verifies that text placed on semantic colors meets WCAG AA contrast in both modes.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldProvideReadableSemanticForegrounds()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            XThemeColorSet[] semanticSets =
            [
                theme.Primary,
                theme.Accent,
                theme.Success,
                theme.Warning,
                theme.Danger,
                theme.Info
            ];

            foreach (XThemeColorSet colorSet in semanticSets)
            {
                Assert.True(
                    GetContrastRatio(colorSet.Light, colorSet.TextLight) >= 4.5d,
                    $"{theme.Name} light semantic foreground must meet WCAG AA contrast.");
                Assert.True(
                    GetContrastRatio(colorSet.Dark, colorSet.TextDark) >= 4.5d,
                    $"{theme.Name} dark semantic foreground must meet WCAG AA contrast.");
            }
        }
    }

    /// <summary>
    /// Verifies that primary, accent, and informational colors remain visually distinct.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldSeparatePrimaryAccentAndInfoColors()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            AssertColorDistance(theme, XThemeMode.Light, theme.Primary.Light, theme.Accent.Light, "primary", "accent");
            AssertColorDistance(theme, XThemeMode.Light, theme.Primary.Light, theme.Info.Light, "primary", "info");
            AssertColorDistance(theme, XThemeMode.Light, theme.Accent.Light, theme.Info.Light, "accent", "info");

            AssertColorDistance(theme, XThemeMode.Dark, theme.Primary.Dark, theme.Accent.Dark, "primary", "accent");
            AssertColorDistance(theme, XThemeMode.Dark, theme.Primary.Dark, theme.Info.Dark, "primary", "info");
            AssertColorDistance(theme, XThemeMode.Dark, theme.Accent.Dark, theme.Info.Dark, "accent", "info");
        }
    }

    /// <summary>
    /// Verifies that workbench surfaces and their borders are visibly separated.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldProvideVisibleSurfaceAndBorderSeparation()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            Assert.True(
                GetContrastRatio(theme.Background.Light, theme.Surface.Light) >= 1.07d,
                $"{theme.Name} light surface must be visibly separated from its canvas.");
            Assert.True(
                GetContrastRatio(theme.Surface.Light, theme.PanelBorder.Light) >= 1.55d,
                $"{theme.Name} light panel border must be visible on its surface.");

            Assert.True(
                GetContrastRatio(theme.Background.Dark, theme.Surface.Dark) >= 1.12d,
                $"{theme.Name} dark surface must be visibly separated from its canvas.");
            Assert.True(
                GetContrastRatio(theme.Surface.Dark, theme.PanelBorder.Dark) >= 1.65d,
                $"{theme.Name} dark panel border must be visible on its surface.");
        }
    }

    /// <summary>
    /// Verifies that editable and selected areas remain distinct from their containing surfaces.
    /// </summary>
    [Fact]
    public void BuiltInThemes_ShouldSeparateInteractiveAreasFromContainers()
    {
        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            Assert.True(
                GetContrastRatio(theme.Surface.Dark, theme.InputBackground.Dark) >= 1.10d,
                $"{theme.Name} dark input must be separated from its containing surface.");

            Assert.True(
                GetContrastRatio(theme.InputBackground.Light, theme.ControlBorder.Light) >= 1.75d,
                $"{theme.Name} light control border must be visible on an input.");
            Assert.True(
                GetContrastRatio(theme.InputBackground.Dark, theme.ControlBorder.Dark) >= 1.75d,
                $"{theme.Name} dark control border must be visible on an input.");

            Assert.True(
                GetContrastRatio(theme.SelectionBackground.Light, theme.SelectionBorder.Light) >= 1.35d,
                $"{theme.Name} light selection border must be visible on a selection.");
            Assert.True(
                GetContrastRatio(theme.SelectionBackground.Dark, theme.SelectionBorder.Dark) >= 1.35d,
                $"{theme.Name} dark selection border must be visible on a selection.");

            Assert.Equal(byte.MaxValue, theme.SelectionBorder.Light.A);
            Assert.Equal(byte.MaxValue, theme.SelectionBorder.Dark.A);

            Assert.True(
                GetContrastRatio(theme.NavigationPanelBackground.Light, theme.NavigationPanelBorder.Light) >= 1.40d,
                $"{theme.Name} light navigation border must be visible on its panel.");
            Assert.True(
                GetContrastRatio(theme.NavigationPanelBackground.Dark, theme.NavigationPanelBorder.Dark) >= 1.40d,
                $"{theme.Name} dark navigation border must be visible on its panel.");
        }
    }
    #endregion

    #region ### Private Methods ###
    private static void AssertColorDistance(
        XTheme theme,
        XThemeMode mode,
        Color first,
        Color second,
        string firstRole,
        string secondRole)
    {
        double red = first.R - second.R;
        double green = first.G - second.G;
        double blue = first.B - second.B;
        double distance = Math.Sqrt((red * red) + (green * green) + (blue * blue));

        Assert.True(
            distance >= 28d,
            $"{theme.Name} {mode.ToString().ToLowerInvariant()} {firstRole} and {secondRole} colors must remain distinct.");
    }

    private static double GetContrastRatio(Color first, Color second)
    {
        double firstLuminance = GetRelativeLuminance(first);
        double secondLuminance = GetRelativeLuminance(second);
        double lighter = Math.Max(firstLuminance, secondLuminance);
        double darker = Math.Min(firstLuminance, secondLuminance);

        return (lighter + 0.05d) / (darker + 0.05d);
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
    #endregion
}
#endregion
