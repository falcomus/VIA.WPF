// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Icons;
using VIA.WPF.Tests.Helpers;
using FluentIconKind = FluentIcons.Common.Icon;
using FluentIconSize = FluentIcons.Common.IconSize;
using FluentIconVariant = FluentIcons.Common.IconVariant;

namespace VIA.WPF.Tests.Icons;

#region ### Class XIconTests ###
/// <summary>
/// Provides tests for VIA.WPF icon controls and markup extensions.
/// </summary>
public sealed class XIconTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that the generic icon control exposes stable default values.
    /// </summary>
    [Fact]
    public void XIcon_ShouldExposeDefaultValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                XIcon icon = new();

                Assert.Equal(XIconPack.MaterialDesign, icon.Pack);
                Assert.Null(icon.Kind);
                Assert.Equal(16d, icon.Size);
                Assert.Equal(Stretch.Uniform, icon.Stretch);
                Assert.NotNull(icon.Foreground);
            });
    }

    /// <summary>
    /// Ensures that the generic icon control supports property roundtrips.
    /// </summary>
    [Fact]
    public void XIcon_ShouldSupportPropertyRoundtrips()
    {
        WpfTestHelper.Run(
            () =>
            {
                Brush foreground = Brushes.CornflowerBlue;
                XIcon icon = new()
                {
                    Pack = XIconPack.Material,
                    Kind = PackIconMaterialKind.None,
                    Size = 24d,
                    Stretch = Stretch.Fill,
                    Foreground = foreground
                };

                Assert.Equal(XIconPack.Material, icon.Pack);
                Assert.Equal(PackIconMaterialKind.None, icon.Kind);
                Assert.Equal(24d, icon.Size);
                Assert.Equal(Stretch.Fill, icon.Stretch);
                Assert.Same(foreground, icon.Foreground);
            });
    }

    /// <summary>
    /// Ensures that the generic icon markup extension creates a configured icon instance.
    /// </summary>
    [Fact]
    public void XIconExtension_ShouldCreateConfiguredXIcon()
    {
        WpfTestHelper.Run(
            () =>
            {
                Brush foreground = Brushes.OrangeRed;
                XIconExtension extension = new(PackIconMaterialDesignKind.None)
                {
                    Size = 28d,
                    Foreground = foreground
                };

                XIcon icon = Assert.IsType<XIcon>(extension.ProvideValue(null!));

                Assert.Equal(PackIconMaterialDesignKind.None, icon.Kind);
                Assert.Equal(28d, icon.Size);
                Assert.Same(foreground, icon.Foreground);
            });
    }

    /// <summary>
    /// Ensures that the namespace smoke extension returns its configured value.
    /// </summary>
    [Fact]
    public void IconTestExtension_ShouldReturnDiscoveryValue()
    {
        IconTestExtension extension = new();

        Assert.Equal("IconTest1", extension.ProvideValue(null!));
    }

    /// <summary>
    /// Ensures that strongly typed MahApps icon controls expose default values.
    /// </summary>
    [Fact]
    public void StronglyTypedMahAppsIconControls_ShouldExposeDefaultValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                AssertMahAppsIconDefaults(new XMaterialDesignIcon(), PackIconMaterialDesignKind.None);
                AssertMahAppsIconDefaults(new XMaterialIcon(), PackIconMaterialKind.None);
                AssertMahAppsIconDefaults(new XBootstrapIcon(), PackIconBootstrapIconsKind.None);
                AssertMahAppsIconDefaults(new XFontAwesomeIcon(), PackIconFontAwesome6Kind.None);
                AssertMahAppsIconDefaults(new XModernIcon(), PackIconModernKind.None);
                AssertMahAppsIconDefaults(new XPhosphorIcon(), PackIconPhosphorIconsKind.None);
                AssertMahAppsIconDefaults(new XFileIcon(), PackIconFileIconsKind.None);
            });
    }

    /// <summary>
    /// Ensures that strongly typed MahApps icon markup extensions create configured controls.
    /// </summary>
    [Fact]
    public void StronglyTypedMahAppsIconExtensions_ShouldCreateConfiguredControls()
    {
        WpfTestHelper.Run(
            () =>
            {
                XMaterialDesignIcon materialDesignIcon = Assert.IsType<XMaterialDesignIcon>(new MaterialDesignIconExtension(PackIconMaterialDesignKind.None)
                {
                    Size = 20d,
                    Stretch = Stretch.Fill
                }.ProvideValue(null!));
                AssertMahAppsIconValues(materialDesignIcon, PackIconMaterialDesignKind.None, 20d, Stretch.Fill);

                XMaterialIcon materialIcon = Assert.IsType<XMaterialIcon>(new MaterialIconExtension(PackIconMaterialKind.None)
                {
                    Size = 21d,
                    Stretch = Stretch.None
                }.ProvideValue(null!));
                AssertMahAppsIconValues(materialIcon, PackIconMaterialKind.None, 21d, Stretch.None);

                XBootstrapIcon bootstrapIcon = Assert.IsType<XBootstrapIcon>(new BootstrapIconExtension(PackIconBootstrapIconsKind.None)
                {
                    Size = 22d,
                    Stretch = Stretch.UniformToFill
                }.ProvideValue(null!));
                AssertMahAppsIconValues(bootstrapIcon, PackIconBootstrapIconsKind.None, 22d, Stretch.UniformToFill);

                XFontAwesomeIcon fontAwesomeIcon = Assert.IsType<XFontAwesomeIcon>(new FontAwesomeIconExtension(PackIconFontAwesome6Kind.None)
                {
                    Size = 23d,
                    Stretch = Stretch.Fill
                }.ProvideValue(null!));
                AssertMahAppsIconValues(fontAwesomeIcon, PackIconFontAwesome6Kind.None, 23d, Stretch.Fill);

                XModernIcon modernIcon = Assert.IsType<XModernIcon>(new ModernIconExtension(PackIconModernKind.None)
                {
                    Size = 24d,
                    Stretch = Stretch.None
                }.ProvideValue(null!));
                AssertMahAppsIconValues(modernIcon, PackIconModernKind.None, 24d, Stretch.None);

                XPhosphorIcon phosphorIcon = Assert.IsType<XPhosphorIcon>(new PhosphorIconExtension(PackIconPhosphorIconsKind.None)
                {
                    Size = 25d,
                    Stretch = Stretch.UniformToFill
                }.ProvideValue(null!));
                AssertMahAppsIconValues(phosphorIcon, PackIconPhosphorIconsKind.None, 25d, Stretch.UniformToFill);

                XFileIcon fileIcon = Assert.IsType<XFileIcon>(new FileIconExtension(PackIconFileIconsKind.None)
                {
                    Size = 26d,
                    Stretch = Stretch.Fill
                }.ProvideValue(null!));
                AssertMahAppsIconValues(fileIcon, PackIconFileIconsKind.None, 26d, Stretch.Fill);
            });
    }

    /// <summary>
    /// Ensures that the Fluent icon control exposes default values.
    /// </summary>
    [Fact]
    public void XFluentIcon_ShouldExposeDefaultValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                XFluentIcon icon = new();

                Assert.Equal(FluentIconKind.AccessTime, icon.Icon);
                Assert.Equal(FluentIconKind.AccessTime, icon.Kind);
                Assert.Equal(FluentIconVariant.Regular, icon.IconVariant);
                Assert.Equal(FluentIconSize.Size24, icon.IconSize);
                Assert.Equal(16d, icon.Size);
                Assert.Equal(Stretch.Uniform, icon.Stretch);
            });
    }

    /// <summary>
    /// Ensures that the Fluent icon markup extension creates a configured control.
    /// </summary>
    [Fact]
    public void FluentIconExtension_ShouldCreateConfiguredControl()
    {
        WpfTestHelper.Run(
            () =>
            {
                XFluentIcon icon = Assert.IsType<XFluentIcon>(new FluentIconExtension(FluentIconKind.AccessTime)
                {
                    IconVariant = FluentIconVariant.Filled,
                    IconSize = FluentIconSize.Size20,
                    Size = 30d,
                    Stretch = Stretch.Fill
                }.ProvideValue(null!));

                Assert.Equal(FluentIconKind.AccessTime, icon.Icon);
                Assert.Equal(FluentIconKind.AccessTime, icon.Kind);
                Assert.Equal(FluentIconVariant.Filled, icon.IconVariant);
                Assert.Equal(FluentIconSize.Size20, icon.IconSize);
                Assert.Equal(30d, icon.Size);
                Assert.Equal(Stretch.Fill, icon.Stretch);
            });
    }
    #endregion

    #region ### Private Methods ###
    private static void AssertMahAppsIconDefaults<TIcon, TKind>(TIcon icon, TKind expectedKind)
        where TIcon : Control
        where TKind : struct, Enum
    {
        AssertMahAppsIconValues(icon, expectedKind, 16d, Stretch.Uniform);
    }

    private static void AssertMahAppsIconValues<TIcon, TKind>(TIcon icon, TKind expectedKind, double expectedSize, Stretch expectedStretch)
        where TIcon : Control
        where TKind : struct, Enum
    {
        object? actualKind = icon.GetValue((System.Windows.DependencyProperty)typeof(TIcon).GetField("KindProperty")!.GetValue(null)!);
        object? actualSize = icon.GetValue((System.Windows.DependencyProperty)typeof(TIcon).GetField("SizeProperty")!.GetValue(null)!);
        object? actualStretch = icon.GetValue((System.Windows.DependencyProperty)typeof(TIcon).GetField("StretchProperty")!.GetValue(null)!);

        Assert.Equal(expectedKind, actualKind);
        Assert.Equal(expectedSize, actualSize);
        Assert.Equal(expectedStretch, actualStretch);
    }
    #endregion
}
#endregion
