// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationTextTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Resources;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationTextTests ###
/// <summary>
/// Tests validation text resolution and localization fallback behavior.
/// </summary>
public sealed class XValidationTextTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that literal validation text resolves unchanged.
    /// </summary>
    [Fact]
    public void Text_ShouldResolveLiteralText()
    {
        XValidationText text = XValidationText.Text("Literal message");

        Assert.False(text.IsResourceKey);
        Assert.Equal("Literal message", text.Resolve());
        Assert.Equal("Literal message", text.ToString());
    }

    /// <summary>
    /// Verifies that a resource key falls back to the supplied fallback text.
    /// </summary>
    [Fact]
    public void Key_ShouldUseFallbackTextWhenResourceIsMissing()
    {
        PreserveLocalizationSettings(() =>
        {
            XValidationText text = XValidationText.Key("MissingKey", "Fallback message");

            XValidationLocalization.ResourceManager = null;
            XValidationLocalization.ThrowOnMissingResource = false;

            Assert.True(text.IsResourceKey);
            Assert.Equal("MissingKey", text.ResourceKey);
            Assert.Equal("Fallback message", text.Resolve());
        });
    }

    /// <summary>
    /// Verifies that a missing resource key resolves to the key when no fallback is configured.
    /// </summary>
    [Fact]
    public void Key_ShouldUseResourceKeyWhenResourceAndFallbackAreMissing()
    {
        PreserveLocalizationSettings(() =>
        {
            XValidationLocalization.ResourceManager = null;
            XValidationLocalization.ThrowOnMissingResource = false;

            Assert.Equal("MissingKey", XValidationText.Key("MissingKey").Resolve());
        });
    }

    /// <summary>
    /// Verifies that missing resources can be configured to throw.
    /// </summary>
    [Fact]
    public void Key_ShouldThrowWhenMissingResourceThrowModeIsEnabled()
    {
        PreserveLocalizationSettings(() =>
        {
            XValidationLocalization.ResourceManager = null;
            XValidationLocalization.ThrowOnMissingResource = true;

            Assert.Throws<MissingManifestResourceException>(() => XValidationText.Key("MissingKey").Resolve());
        });
    }
    #endregion

    #region ### Private Methods ###
    private static void PreserveLocalizationSettings(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        ResourceManager? oldResourceManager = XValidationLocalization.ResourceManager;
        System.Globalization.CultureInfo? oldCulture = XValidationLocalization.Culture;
        bool oldThrowOnMissingResource = XValidationLocalization.ThrowOnMissingResource;

        try
        {
            action();
        }
        finally
        {
            XValidationLocalization.ResourceManager = oldResourceManager;
            XValidationLocalization.Culture = oldCulture;
            XValidationLocalization.ThrowOnMissingResource = oldThrowOnMissingResource;
        }
    }
    #endregion
}
#endregion
