// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLocalizationDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows;
using VIA.WPF.Demo.Resources;
using VIA.WPF.Localization;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XLocalizationDemoViewModel ###
/// <summary>
/// Represents the demo view model for the VIA.WPF localization showcase page.
/// </summary>
public sealed partial class XLocalizationDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private readonly XLocalizationService localizationService = XLocalizationService.Current;
    private bool hasCreatedMessage;
    private DateTime lastMessageTime;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLocalizationDemoViewModel"/> class.
    /// </summary>
    public XLocalizationDemoViewModel()
    {
        this.SelectedLanguage = XLanguages.FindBestMatch(
            this.localizationService.CurrentUICulture,
            XLanguages.Default);

        WeakEventManager<XLocalizationService, XLanguageChangedEventArgs>.AddHandler(
            this.localizationService,
            nameof(XLocalizationService.LanguageChanged),
            this.OnLanguageChanged);

        this.UpdateLocalizedPreview();
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Localization";

    /// <inheritdoc/>
    public override string Description => "Demonstrates dynamic DE/EN resource bindings, culture-aware formatting, code messages and the synchronized XWindow language selector.";

    /// <summary>
    /// Gets or sets the selected demo language.
    /// </summary>
    [ObservableProperty]
    private XLanguage? selectedLanguage;

    /// <summary>
    /// Gets the displayed current UI culture.
    /// </summary>
    [ObservableProperty]
    private string currentCultureName = string.Empty;

    /// <summary>
    /// Gets the culture-aware date and time preview.
    /// </summary>
    [ObservableProperty]
    private string datePreview = string.Empty;

    /// <summary>
    /// Gets the culture-aware number preview.
    /// </summary>
    [ObservableProperty]
    private string numberPreview = string.Empty;

    /// <summary>
    /// Gets the localized code-message preview.
    /// </summary>
    [ObservableProperty]
    private string localizedMessage = string.Empty;

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XLanguageSelector
    Width="300"
    Header="{via:XLoc SelectorHeader,
             ResourceManager={x:Static resources:LocalizationDemoResources.ResourceManager}}"
    SelectedItem="{Binding SelectedLanguage, Mode=TwoWay}" />

<via:XTextBlock
    Text="{via:XLoc GreetingTitle,
           ResourceManager={x:Static resources:LocalizationDemoResources.ResourceManager}}" />

<via:XWindow
    ShowLanguageSelector="True"
    AvailableLanguages="{x:Static via:XLanguages.Default}" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XLocalizationService.Current.SetCulture("de-DE");

string message = XLocalizationService.Current.GetString(
    LocalizationDemoResources.ResourceManager,
    "MessageInitial");

string formattedMessage = XLocalizationService.Current.Format(
    LocalizationDemoResources.ResourceManager,
    "MessageCreated",
    fallbackText: null,
    DateTime.Now);
""";
    #endregion

    #region ### Partial Methods ###
    partial void OnSelectedLanguageChanged(XLanguage? value)
    {
        if (value is null)
        {
            return;
        }

        this.localizationService.SetCulture(
            value.Culture,
            applyFormattingCulture: true);
    }
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Creates a localized message through the code-oriented localization API.
    /// </summary>
    [RelayCommand]
    private void CreateMessage()
    {
        this.hasCreatedMessage = true;
        this.lastMessageTime = DateTime.Now;
        this.UpdateLocalizedMessage();
    }
    #endregion

    #region ### Private Methods ###
    private void OnLanguageChanged(object? sender, XLanguageChangedEventArgs e)
    {
        XLanguage? matchingLanguage = XLanguages.FindBestMatch(
            e.CurrentCulture,
            XLanguages.Default);

        if (matchingLanguage is not null &&
            !Equals(this.SelectedLanguage, matchingLanguage))
        {
            this.SelectedLanguage = matchingLanguage;
        }

        this.UpdateLocalizedPreview();
    }

    private void UpdateLocalizedPreview()
    {
        CultureInfo uiCulture = this.localizationService.CurrentUICulture;
        CultureInfo formattingCulture = CultureInfo.CurrentCulture;

        this.CurrentCultureName = $"{uiCulture.DisplayName} ({uiCulture.Name})";
        this.DatePreview = DateTime.Now.ToString("F", formattingCulture);
        this.NumberPreview = 1234567.89m.ToString("N2", formattingCulture);

        this.UpdateLocalizedMessage();
    }

    private void UpdateLocalizedMessage()
    {
        this.LocalizedMessage = this.hasCreatedMessage
            ? this.localizationService.Format(
                LocalizationDemoResources.ResourceManager,
                "MessageCreated",
                fallbackText: null,
                this.lastMessageTime)
            : this.localizationService.GetString(
                LocalizationDemoResources.ResourceManager,
                "MessageInitial");
    }
    #endregion
}
#endregion
