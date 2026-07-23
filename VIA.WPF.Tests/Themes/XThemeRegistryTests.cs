// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeRegistryTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XThemeRegistryTests ###
/// <summary>
/// Tests the global theme registry.
/// </summary>
public sealed class XThemeRegistryTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that registering a theme makes it discoverable by name.
    /// </summary>
    [Fact]
    public void Register_ShouldMakeThemeAvailableByName()
    {
        XThemeRegistry registry = XThemeRegistry.Current;

        registry.Register(XThemePresets.Default);

        Assert.True(registry.Contains(XThemePresets.Default.Name));
        Assert.Same(XThemePresets.Default, registry.GetByName(XThemePresets.Default.Name));
    }

    /// <summary>
    /// Verifies that name lookup is case-insensitive.
    /// </summary>
    [Fact]
    public void GetByName_ShouldUseCaseInsensitiveNameComparison()
    {
        XThemeRegistry registry = XThemeRegistry.Current;

        registry.Register(XThemePresets.Graphite);

        XTheme? theme = registry.GetByName(XThemePresets.Graphite.Name.ToUpperInvariant());

        Assert.Same(XThemePresets.Graphite, theme);
        Assert.True(registry.Contains(XThemePresets.Graphite.Name.ToLowerInvariant()));
    }

    /// <summary>
    /// Verifies that registering the same theme name more than once does not add duplicates.
    /// </summary>
    [Fact]
    public void Register_ShouldIgnoreDuplicateNames()
    {
        XThemeRegistry registry = XThemeRegistry.Current;

        registry.Register(XThemePresets.Azure);
        int countAfterFirstRegistration = registry.Themes.Count;

        registry.Register(XThemePresets.Azure);

        Assert.Equal(countAfterFirstRegistration, registry.Themes.Count);
    }

    /// <summary>
    /// Verifies that registering a range registers all supplied themes and remains idempotent.
    /// </summary>
    [Fact]
    public void RegisterRange_ShouldRegisterBuiltInThemesIdempotently()
    {
        XThemeRegistry registry = XThemeRegistry.Current;

        registry.RegisterRange(XThemePresets.BuiltInThemes);
        int countAfterFirstRegistration = registry.Themes.Count;

        registry.RegisterRange(XThemePresets.BuiltInThemes);

        Assert.Equal(countAfterFirstRegistration, registry.Themes.Count);

        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            Assert.True(registry.Contains(theme.Name));
        }
    }

    /// <summary>
    /// Verifies null and blank argument handling.
    /// </summary>
    [Fact]
    public void PublicMethods_ShouldValidateArguments()
    {
        XThemeRegistry registry = XThemeRegistry.Current;

        Assert.Throws<ArgumentNullException>(() => registry.Register(null!));
        Assert.Throws<ArgumentNullException>(() => registry.RegisterRange(null!));
        Assert.Throws<ArgumentException>(() => registry.GetByName(string.Empty));
        Assert.Throws<ArgumentException>(() => registry.Contains("   "));
    }
    #endregion
}
#endregion
