// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLocalizationServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Resources;
using VIA.WPF.Localization;

namespace VIA.WPF.Tests.Controls.Localization;

#region ### Class XLocalizationServiceTests ###
/// <summary>
/// Tests the shared VIA.WPF localization infrastructure.
/// </summary>
public sealed class XLocalizationServiceTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the language metadata used by a future language selector.
    /// </summary>
    [Fact]
    public void XLanguage_ShouldExposeConfiguredMetadata()
    {
        XLanguage language = new("de-DE", "Deutsch", "DE", "🇩🇪");

        Assert.Equal("de-DE", language.CultureName);
        Assert.Equal("Deutsch", language.DisplayName);
        Assert.Equal("DE", language.ShortName);
        Assert.Equal("🇩🇪", language.FlagGlyph);
        Assert.Equal("Deutsch", language.ToString());
    }

    /// <summary>
    /// Verifies that resource lookup uses the active UI culture.
    /// </summary>
    [Fact]
    public void GetString_ShouldUseActiveUICulture()
    {
        XLocalizationService service = XLocalizationService.Current;
        CultureState state = CaptureCultureState(service);

        try
        {
            service.SetCulture("de-DE", applyFormattingCulture: false);

            TestResourceManager resourceManager = new();
            string value = service.GetString(resourceManager, "Save");

            Assert.Equal("de-DE:Save", value);
        }
        finally
        {
            RestoreCultureState(service, state);
        }
    }

    /// <summary>
    /// Verifies that a language change updates bindable localized strings.
    /// </summary>
    [Fact]
    public void XLocalizedString_ShouldNotifyAfterLanguageChange()
    {
        XLocalizationService service = XLocalizationService.Current;
        CultureState state = CaptureCultureState(service);

        try
        {
            service.SetCulture("en-US", applyFormattingCulture: false);

            XLocalizedString localizedString = new(new TestResourceManager(), "Open", localizationService: service);
            List<string?> changedProperties = new();

            localizedString.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

            service.SetCulture("de-DE", applyFormattingCulture: false);

            Assert.Equal("de-DE:Open", localizedString.Value);
            Assert.Contains(nameof(XLocalizedString.Value), changedProperties);
        }
        finally
        {
            RestoreCultureState(service, state);
        }
    }

    /// <summary>
    /// Verifies fallback behavior for missing resource manifests.
    /// </summary>
    [Fact]
    public void GetString_ShouldUseFallbackWhenManifestIsMissing()
    {
        XLocalizationService service = XLocalizationService.Current;
        bool originalThrowSetting = service.ThrowOnMissingResource;

        try
        {
            service.ThrowOnMissingResource = false;

            string value = service.GetString(
                new MissingManifestTestResourceManager(),
                "Missing",
                "Fallback");

            Assert.Equal("Fallback", value);
        }
        finally
        {
            service.ThrowOnMissingResource = originalThrowSetting;
        }
    }
    #endregion

    #region ### Private Methods ###
    private static CultureState CaptureCultureState(XLocalizationService service)
    {
        return new CultureState(
            service.CurrentUICulture,
            service.ApplyFormattingCulture,
            CultureInfo.CurrentCulture,
            CultureInfo.CurrentUICulture,
            CultureInfo.DefaultThreadCurrentCulture,
            CultureInfo.DefaultThreadCurrentUICulture);
    }

    private static void RestoreCultureState(XLocalizationService service, CultureState state)
    {
        service.SetCulture(state.ServiceUICulture, state.ApplyFormattingCulture);

        CultureInfo.CurrentCulture = state.ThreadCulture;
        CultureInfo.CurrentUICulture = state.ThreadUICulture;
        CultureInfo.DefaultThreadCurrentCulture = state.DefaultThreadCulture;
        CultureInfo.DefaultThreadCurrentUICulture = state.DefaultThreadUICulture;
    }
    #endregion

    #region ### Class TestResourceManager ###
    private sealed class TestResourceManager : ResourceManager
    {
        #region ### Public Methods ###
        public override string? GetString(string name, CultureInfo? culture)
        {
            return $"{culture?.Name}:{name}";
        }
        #endregion
    }
    #endregion

    #region ### Class MissingManifestTestResourceManager ###
    private sealed class MissingManifestTestResourceManager : ResourceManager
    {
        #region ### Public Methods ###
        public override string? GetString(string name, CultureInfo? culture)
        {
            throw new MissingManifestResourceException();
        }
        #endregion
    }
    #endregion

    #region ### Record CultureState ###
    private sealed record CultureState(
        CultureInfo ServiceUICulture,
        bool ApplyFormattingCulture,
        CultureInfo ThreadCulture,
        CultureInfo ThreadUICulture,
        CultureInfo? DefaultThreadCulture,
        CultureInfo? DefaultThreadUICulture);
    #endregion
}
#endregion
