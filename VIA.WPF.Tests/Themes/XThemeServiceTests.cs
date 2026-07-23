// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XThemeServiceTests ###
/// <summary>
/// Tests global theme service registry behavior that does not require a WPF application instance.
/// </summary>
public sealed class XThemeServiceTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that service properties expose the global manager and registry instances.
    /// </summary>
    [Fact]
    public void Properties_ShouldExposeGlobalInstances()
    {
        Assert.Same(XThemeManager.Current, XThemeService.Manager);
        Assert.Same(XThemeRegistry.Current, XThemeService.Registry);
    }

    /// <summary>
    /// Verifies that built-in themes are registered once and remain available.
    /// </summary>
    [Fact]
    public void EnsureBuiltInThemesRegistered_ShouldRegisterBuiltInThemesIdempotently()
    {
        XThemeService.EnsureBuiltInThemesRegistered();
        int countAfterFirstCall = XThemeService.Registry.Themes.Count;

        XThemeService.EnsureBuiltInThemesRegistered();

        Assert.Equal(countAfterFirstCall, XThemeService.Registry.Themes.Count);

        foreach (XTheme theme in XThemePresets.BuiltInThemes)
        {
            Assert.True(XThemeService.Registry.Contains(theme.Name));
        }
    }

    /// <summary>
    /// Verifies that changing to an unknown theme name returns false without requiring a WPF application instance.
    /// </summary>
    [Fact]
    public void ChangeTheme_WithUnknownName_ShouldReturnFalse()
    {
        bool result = XThemeService.ChangeTheme($"Unknown-{Guid.NewGuid():N}");

        Assert.False(result);
    }

    /// <summary>
    /// Verifies that changing to a blank theme name throws.
    /// </summary>
    [Fact]
    public void ChangeTheme_WithBlankName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => XThemeService.ChangeTheme("   "));
    }
    #endregion
}
#endregion
