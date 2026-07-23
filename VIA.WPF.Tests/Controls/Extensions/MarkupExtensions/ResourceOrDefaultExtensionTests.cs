// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceOrDefaultExtensionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions.MarkupExtensions;

#region ### Class ResourceOrDefaultExtensionTests ###
/// <summary>
/// Contains tests for the <see cref="ResourceOrDefaultExtension"/> class.
/// </summary>
public sealed class ResourceOrDefaultExtensionTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that the default value is returned when no key is configured.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldReturnDefaultValueWhenKeyIsNull()
    {
        ResourceOrDefaultExtension extension = new()
        {
            DefaultValue = "Fallback"
        };

        object? value = extension.ProvideValue(new XamlServiceProviderStub());

        Assert.Equal("Fallback", value);
    }

    /// <summary>
    /// Ensures that resources can be resolved from a framework element target.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldReturnFrameworkElementResourceWhenAvailable()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border target = new();
                target.Resources.Add("ExistingKey", "ResourceValue");
                ResourceOrDefaultExtension extension = new("ExistingKey")
                {
                    DefaultValue = "Fallback"
                };

                object? value = extension.ProvideValue(new XamlServiceProviderStub(target, FrameworkElement.TagProperty));

                Assert.Equal("ResourceValue", value);
            });
    }

    /// <summary>
    /// Ensures that the default value is returned when no matching resource exists.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldReturnDefaultValueWhenResourceIsMissing()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border target = new();
                ResourceOrDefaultExtension extension = new("MissingKey")
                {
                    DefaultValue = "Fallback"
                };

                object? value = extension.ProvideValue(new XamlServiceProviderStub(target, FrameworkElement.TagProperty));

                Assert.Equal("Fallback", value);
            });
    }

    /// <summary>
    /// Ensures that a null service provider is rejected.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldThrowWhenServiceProviderIsNull()
    {
        ResourceOrDefaultExtension extension = new("Key");

        Assert.Throws<ArgumentNullException>(() => extension.ProvideValue(null!));
    }
    #endregion
}
#endregion
