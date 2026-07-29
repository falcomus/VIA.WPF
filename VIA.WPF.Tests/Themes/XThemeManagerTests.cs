// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeManagerTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XThemeManagerTests ###
/// <summary>
/// Tests runtime theme manager behavior against isolated resource dictionaries.
/// </summary>
public sealed class XThemeManagerTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the default manager state.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeDefaultState()
    {
        XThemeManager manager = new();

        Assert.Null(manager.CurrentTheme);
        Assert.Equal(XThemeMode.Light, manager.CurrentMode);
        Assert.Equal(TimeSpan.FromMilliseconds(600), manager.TransitionDuration);
    }

    /// <summary>
    /// Verifies that applying a theme to an explicit resource dictionary writes runtime brushes.
    /// </summary>
    [Fact]
    public void ApplyTheme_WithResourceDictionary_ShouldApplyLightModeRuntimeBrushes()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();
                XTheme theme = XThemePresets.Default;

                manager.ApplyTheme(theme, resources);

                Assert.Same(theme, manager.CurrentTheme);
                Assert.Equal(XThemeMode.Light, manager.CurrentMode);

                SolidColorBrush primaryBrush = GetBrush(resources, XBrushKeys.Primary);
                SolidColorBrush primaryTextBrush = GetBrush(resources, XBrushKeys.PrimaryText);
                SolidColorBrush backgroundBrush = GetBrush(resources, XBrushKeys.Background);
                SolidColorBrush canvasBrush = GetBrush(resources, XBrushKeys.Canvas);
                SolidColorBrush raisedSurfaceBrush = GetBrush(resources, XBrushKeys.SurfaceRaised);
                SolidColorBrush focusRingBrush = GetBrush(resources, XBrushKeys.FocusRing);
                SolidColorBrush navigationIndicatorBrush = GetBrush(resources, XBrushKeys.NavigationSelectionIndicator);
                SolidColorBrush successHoverBrush = GetBrush(resources, XBrushKeys.StatusSuccessSubtleHover);
                SolidColorBrush successStrongBrush = GetBrush(resources, XBrushKeys.StatusSuccessStrong);

                Assert.Equal(theme.Primary.Light, primaryBrush.Color);
                Assert.Equal(theme.Primary.TextLight, primaryTextBrush.Color);
                Assert.Equal(theme.Background.Light, backgroundBrush.Color);
                Assert.Equal(theme.Background.Light, canvasBrush.Color);
                Assert.NotEqual(theme.Surface.Light, raisedSurfaceBrush.Color);
                Assert.Equal(theme.FocusBorder.Light, focusRingBrush.Color);
                Assert.Equal(theme.Primary.Light, navigationIndicatorBrush.Color);
                Assert.Equal(theme.Success.LightVariantLight, successHoverBrush.Color);
                Assert.Equal(theme.Success.DarkVariantLight, successStrongBrush.Color);
                Assert.False(primaryBrush.IsFrozen);
            });
    }

    /// <summary>
    /// Verifies that switching mode updates existing runtime resources.
    /// </summary>
    [Fact]
    public void SetMode_ShouldUpdateRuntimeBrushes()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();
                XTheme theme = XThemePresets.Graphite;

                manager.ApplyTheme(theme, resources);
                manager.SetMode(XThemeMode.Dark);

                SolidColorBrush primaryBrush = GetBrush(resources, XBrushKeys.Primary);
                SolidColorBrush primaryTextBrush = GetBrush(resources, XBrushKeys.PrimaryText);
                SolidColorBrush navigationBrush = GetBrush(resources, XBrushKeys.NavigationPanelBackground);
                SolidColorBrush canvasBrush = GetBrush(resources, XBrushKeys.Canvas);
                SolidColorBrush selectedStateBrush = GetBrush(resources, XBrushKeys.StateSelected);
                SolidColorBrush commandBarBrush = GetBrush(resources, XBrushKeys.CommandBarBackground);

                Assert.Equal(XThemeMode.Dark, manager.CurrentMode);
                Assert.Equal(theme.Primary.Dark, primaryBrush.Color);
                Assert.Equal(theme.Primary.TextDark, primaryTextBrush.Color);
                Assert.Equal(theme.NavigationPanelBackground.Dark, navigationBrush.Color);
                Assert.Equal(theme.Background.Dark, canvasBrush.Color);
                Assert.Equal(theme.SelectionBackground.Dark, selectedStateBrush.Color);
                Assert.Equal(theme.ToolbarBackground.Dark, commandBarBrush.Color);
            });
    }

    /// <summary>
    /// Verifies that toggling mode switches between light and dark.
    /// </summary>
    [Fact]
    public void ToggleMode_ShouldSwitchBetweenLightAndDark()
    {
        XThemeManager manager = new();

        manager.ToggleMode();
        Assert.Equal(XThemeMode.Dark, manager.CurrentMode);

        manager.ToggleMode();
        Assert.Equal(XThemeMode.Light, manager.CurrentMode);
    }

    /// <summary>
    /// Verifies that applying several themes reuses the same runtime resource dictionary.
    /// </summary>
    [Fact]
    public void ApplyTheme_ShouldReuseRuntimeResourceDictionary()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();

                manager.ApplyTheme(XThemePresets.Default, resources);
                int dictionaryCountAfterFirstApply = resources.MergedDictionaries.Count;

                manager.ApplyTheme(XThemePresets.Teal, resources);

                Assert.Equal(dictionaryCountAfterFirstApply, resources.MergedDictionaries.Count);
                Assert.Same(XThemePresets.Teal, manager.CurrentTheme);
                Assert.Equal(XThemePresets.Teal.Primary.Light, GetBrush(resources, XBrushKeys.Primary).Color);
            });
    }

    /// <summary>
    /// Verifies that reapplying a theme restores the runtime dictionary as the final resource authority.
    /// </summary>
    [Fact]
    public void ApplyTheme_ShouldKeepRuntimeResourceDictionaryLast()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();

                manager.ApplyTheme(XThemePresets.Default, resources);
                resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        [XBrushKeys.Canvas] = new SolidColorBrush(Colors.Magenta)
                    });

                manager.ApplyTheme(XThemePresets.Graphite, resources);

                ResourceDictionary runtimeDictionary = resources.MergedDictionaries[^1];
                Assert.True(runtimeDictionary.Contains(XBrushKeys.Canvas));
                Assert.Equal(
                    XThemePresets.Graphite.Background.Light,
                    Assert.IsType<SolidColorBrush>(runtimeDictionary[XBrushKeys.Canvas]).Color);
            });
    }

    /// <summary>
    /// Verifies that canonical semantic names use dedicated resource keys.
    /// </summary>
    [Fact]
    public void CanonicalBrushKeys_ShouldUseDedicatedResourceKeys()
    {
        Assert.NotSame(XBrushKeys.PrimaryText, XBrushKeys.PrimaryForeground);
        Assert.NotSame(XBrushKeys.PrimaryVeryLight, XBrushKeys.PrimarySubtle);
        Assert.NotSame(XBrushKeys.AccentText, XBrushKeys.AccentForeground);
        Assert.NotSame(XBrushKeys.Success, XBrushKeys.StatusSuccess);
        Assert.NotSame(XBrushKeys.Warning, XBrushKeys.StatusWarning);
        Assert.NotSame(XBrushKeys.Danger, XBrushKeys.StatusDanger);
        Assert.NotSame(XBrushKeys.Info, XBrushKeys.StatusInfo);
        Assert.NotSame(XBrushKeys.SurfaceRaised, XBrushKeys.SurfaceOverlay);
        Assert.NotSame(XBrushKeys.DisabledForeground, XBrushKeys.TextDisabled);
        Assert.NotSame(XBrushKeys.BorderSubtle, XBrushKeys.Divider);
    }

    /// <summary>
    /// Verifies that raised and overlay containers use progressively stronger tonal elevation.
    /// </summary>
    [Fact]
    public void ApplyTheme_ShouldProvideTonalSurfaceHierarchyForAllBuiltInThemes()
    {
        WpfTestHelper.Run(
            () =>
            {
                foreach (XTheme theme in XThemePresets.BuiltInThemes)
                {
                    ResourceDictionary resources = [];
                    XThemeManager manager = new();
                    manager.ApplyTheme(theme, resources);

                    AssertTonalHierarchy(theme, XThemeMode.Light, manager, resources);
                    AssertTonalHierarchy(theme, XThemeMode.Dark, manager, resources);
                }
            });
    }

    /// <summary>
    /// Verifies that public properties raise property change notifications when changed.
    /// </summary>
    [Fact]
    public void PropertyChanged_ShouldBeRaisedForCurrentThemeAndCurrentMode()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();
                List<string?> changedProperties = [];

                manager.PropertyChanged += OnPropertyChanged;

                manager.ApplyTheme(XThemePresets.Default, resources);
                manager.SetMode(XThemeMode.Dark);

                manager.PropertyChanged -= OnPropertyChanged;

                Assert.Contains(nameof(XThemeManager.CurrentTheme), changedProperties);
                Assert.Contains(nameof(XThemeManager.CurrentMode), changedProperties);
                return;

                void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
                {
                    changedProperties.Add(e.PropertyName);
                }
            });
    }

    /// <summary>
    /// Verifies that observers are notified only after the complete runtime dictionary is active.
    /// </summary>
    [Fact]
    public void PropertyChanged_ShouldObserveCommittedThemeResources()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                XThemeManager manager = new();
                Color? observedPrimary = null;

                manager.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(XThemeManager.CurrentTheme))
                    {
                        observedPrimary = GetBrush(resources, XBrushKeys.Primary).Color;
                    }
                };

                manager.ApplyTheme(XThemePresets.Teal, resources);

                Assert.Equal(XThemePresets.Teal.Primary.Light, observedPrimary);
            });
    }

    /// <summary>
    /// Verifies that dynamic resource consumers follow the atomically replaced runtime dictionary.
    /// </summary>
    [Fact]
    public void ApplyTheme_ShouldRefreshDynamicResourceConsumers()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary resources = [];
                Border consumer = new();
                XThemeManager manager = new();

                consumer.Resources.MergedDictionaries.Add(resources);
                consumer.SetResourceReference(Border.BackgroundProperty, XBrushKeys.Primary);

                manager.ApplyTheme(XThemePresets.Default, resources);
                Assert.Equal(
                    XThemePresets.Default.Primary.Light,
                    Assert.IsType<SolidColorBrush>(consumer.Background).Color);

                manager.ApplyTheme(XThemePresets.Teal, resources);
                Assert.Equal(
                    XThemePresets.Teal.Primary.Light,
                    Assert.IsType<SolidColorBrush>(consumer.Background).Color);
            });
    }

    /// <summary>
    /// Verifies that null arguments are rejected.
    /// </summary>
    [Fact]
    public void ApplyTheme_ShouldValidateArguments()
    {
        XThemeManager manager = new();
        ResourceDictionary resources = [];

        Assert.Throws<ArgumentNullException>(() => manager.ApplyTheme(null!, resources));
        Assert.Throws<ArgumentNullException>(() => manager.ApplyTheme(XThemePresets.Default, null!));
    }
    #endregion

    #region ### Private Methods ###
    private static void AssertTonalHierarchy(
        XTheme theme,
        XThemeMode mode,
        XThemeManager manager,
        ResourceDictionary resources)
    {
        manager.SetMode(mode);

        Color surface = GetBrush(resources, XBrushKeys.Surface).Color;
        Color raised = GetBrush(resources, XBrushKeys.SurfaceRaised).Color;
        Color overlay = GetBrush(resources, XBrushKeys.SurfaceOverlay).Color;

        double raisedDistance = GetColorDistance(surface, raised);
        double overlayDistance = GetColorDistance(surface, overlay);

        Assert.True(
            raisedDistance >= 3d,
            $"{theme.Name} {mode.ToString().ToLowerInvariant()} raised surface must differ from its base surface.");
        Assert.True(
            overlayDistance > raisedDistance,
            $"{theme.Name} {mode.ToString().ToLowerInvariant()} overlay must have more tonal emphasis than a raised surface.");
    }

    private static double GetColorDistance(Color first, Color second)
    {
        double red = first.R - second.R;
        double green = first.G - second.G;
        double blue = first.B - second.B;

        return Math.Sqrt((red * red) + (green * green) + (blue * blue));
    }

    private static SolidColorBrush GetBrush(ResourceDictionary resources, object key)
    {
        object? value = FindResourceValue(resources, key);
        Assert.NotNull(value);
        return Assert.IsType<SolidColorBrush>(value, exactMatch: false);
    }

    private static object? FindResourceValue(ResourceDictionary resources, object key)
    {
        if (resources.Contains(key))
        {
            return resources[key];
        }

        foreach (ResourceDictionary dictionary in resources.MergedDictionaries)
        {
            object? value = FindResourceValue(dictionary, key);

            if (value is not null)
            {
                return value;
            }
        }

        return null;
    }
    #endregion
}
#endregion
