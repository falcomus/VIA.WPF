// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeResourceExtensionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions.MarkupExtensions;

#region ### Class ThemeResourceExtensionTests ###
/// <summary>
/// Contains tests for the <see cref="ThemeResourceExtension"/> class.
/// </summary>
public sealed class ThemeResourceExtensionTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that a missing key name returns <see cref="DependencyProperty.UnsetValue"/>.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldReturnUnsetValueWhenKeyNameIsMissing()
    {
        ThemeResourceExtension extension = new();

        object value = extension.ProvideValue(new XamlServiceProviderStub());

        Assert.Same(DependencyProperty.UnsetValue, value);
    }

    /// <summary>
    /// Ensures that property based key providers can be resolved.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldResolveResourceKeyFromProviderProperty()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button target = new();
                target.Resources.Add(TestThemeResourceKeys.PropertyKey, Brushes.Red);
                ThemeResourceExtension extension = new(nameof(TestThemeResourceKeys.PropertyKey))
                {
                    KeyProviderTypeName = typeof(TestThemeResourceKeys).FullName!
                };

                object value = extension.ProvideValue(new XamlServiceProviderStub(target, Control.BackgroundProperty));
                target.SetValue(Control.BackgroundProperty, value);

                Assert.Same(Brushes.Red, target.Background);
            });
    }

    /// <summary>
    /// Ensures that field based key providers can be resolved.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldResolveResourceKeyFromProviderField()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button target = new();
                target.Resources.Add(TestThemeResourceKeys.FieldKey, Brushes.Blue);
                ThemeResourceExtension extension = new(nameof(TestThemeResourceKeys.FieldKey))
                {
                    KeyProviderTypeName = typeof(TestThemeResourceKeys).FullName!
                };

                object value = extension.ProvideValue(new XamlServiceProviderStub(target, Control.BackgroundProperty));
                target.SetValue(Control.BackgroundProperty, value);

                Assert.Same(Brushes.Blue, target.Background);
            });
    }

    /// <summary>
    /// Ensures that an unknown key provider falls back to the key name.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldUseKeyNameWhenProviderTypeCannotBeResolved()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button target = new();
                target.Resources.Add("FallbackKey", Brushes.Green);
                ThemeResourceExtension extension = new("FallbackKey")
                {
                    KeyProviderTypeName = "Missing.Provider.Type"
                };

                object value = extension.ProvideValue(new XamlServiceProviderStub(target, Control.BackgroundProperty));
                target.SetValue(Control.BackgroundProperty, value);

                Assert.Same(Brushes.Green, target.Background);
            });
    }

    /// <summary>
    /// Ensures that a null service provider is rejected.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldThrowWhenServiceProviderIsNull()
    {
        ThemeResourceExtension extension = new("FallbackKey");

        Assert.Throws<ArgumentNullException>(() => extension.ProvideValue(null!));
    }
    #endregion

    #region ### Nested Types ###
    private static class TestThemeResourceKeys
    {
        public static ComponentResourceKey PropertyKey { get; } = new(typeof(TestThemeResourceKeys), nameof(PropertyKey));

        public static readonly ComponentResourceKey FieldKey = new(typeof(TestThemeResourceKeys), nameof(FieldKey));
    }
    #endregion
}
#endregion
