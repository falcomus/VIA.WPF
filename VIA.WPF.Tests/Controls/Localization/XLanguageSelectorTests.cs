// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLanguageSelectorTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Localization;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Localization;

#region ### Class XLanguageSelectorTests ###
/// <summary>
/// Tests the VIA.WPF language selector and built-in language catalog.
/// </summary>
public sealed class XLanguageSelectorTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the built-in German and English language definitions.
    /// </summary>
    [Fact]
    public void DefaultLanguages_ShouldContainGermanAndEnglish()
    {
        Assert.Collection(
            XLanguages.Default,
            language =>
            {
                Assert.Same(XLanguages.German, language);
                Assert.Equal("🇩🇪 Deutsch", language.DisplayText);
                Assert.Equal("🇩🇪 DE", language.CompactDisplayText);
            },
            language =>
            {
                Assert.Same(XLanguages.English, language);
                Assert.Equal("🇬🇧 English", language.DisplayText);
                Assert.Equal("🇬🇧 EN", language.CompactDisplayText);
            });
    }

    /// <summary>
    /// Verifies exact, neutral and fallback language matching.
    /// </summary>
    [Fact]
    public void FindBestMatch_ShouldUseExactNeutralAndEnglishFallback()
    {
        Assert.Same(
            XLanguages.German,
            XLanguages.FindBestMatch(
                CultureInfo.GetCultureInfo("de-DE"),
                XLanguages.Default));

        Assert.Same(
            XLanguages.German,
            XLanguages.FindBestMatch(
                CultureInfo.GetCultureInfo("de-AT"),
                XLanguages.Default));

        Assert.Same(
            XLanguages.English,
            XLanguages.FindBestMatch(
                CultureInfo.GetCultureInfo("fr-FR"),
                XLanguages.Default));
    }

    /// <summary>
    /// Verifies the selector defaults and display configuration.
    /// </summary>
    [Fact]
    public void Constructor_ShouldExposeLanguageSelectorDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XLanguageSelector selector = new();

                Assert.Same(XLanguages.Default, selector.ItemsSource);
                Assert.Equal(string.Empty, selector.DisplayMemberPath);
                Assert.True(selector.ApplyFormattingCulture);
            });
    }

    /// <summary>
    /// Verifies that the selector theme exposes the vector-flag item template.
    /// </summary>
    [Fact]
    public void ThemeDictionary_ShouldProvideVectorFlagItemTemplate()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary dictionary = new()
                {
                    Source = new Uri(
                        "/VIA.WPF.Controls;component/Themes/XLanguageSelector.xaml",
                        UriKind.Relative)
                };

                DataTemplate itemTemplate = Assert.IsType<DataTemplate>(
                    dictionary["XLanguageSelectorItemTemplate"]);

                Assert.NotNull(itemTemplate);
            });
    }

    /// <summary>
    /// Verifies that selecting a language changes the shared UI culture.
    /// </summary>
    [Fact]
    public void Selection_ShouldChangeLocalizationCulture()
    {
        WpfTestHelper.Run(
            () =>
            {
                XLocalizationService service = XLocalizationService.Current;
                CultureInfo originalUICulture = service.CurrentUICulture;
                bool originalFormattingMode = service.ApplyFormattingCulture;
                CultureInfo originalCulture = CultureInfo.CurrentCulture;
                CultureInfo originalThreadUICulture = CultureInfo.CurrentUICulture;
                CultureInfo? originalDefaultCulture = CultureInfo.DefaultThreadCurrentCulture;
                CultureInfo? originalDefaultUICulture = CultureInfo.DefaultThreadCurrentUICulture;

                try
                {
                    XLanguageSelector selector = new()
                    {
                        ApplyFormattingCulture = false,
                        SelectedItem = XLanguages.German
                    };

                    Assert.Equal("de-DE", service.CurrentUICulture.Name);
                }
                finally
                {
                    service.SetCulture(originalUICulture, originalFormattingMode);
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalThreadUICulture;
                    CultureInfo.DefaultThreadCurrentCulture = originalDefaultCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = originalDefaultUICulture;
                }
            });
    }
    #endregion
}
#endregion
