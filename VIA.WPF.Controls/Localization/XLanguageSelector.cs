// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLanguageSelector.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Controls;

namespace VIA.WPF.Localization;

#region ### Class XLanguageSelector ###
/// <summary>
/// Represents a VIA.WPF combo box that lists application languages and applies the selected culture.
/// </summary>
public class XLanguageSelector : XComboBox
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="ApplyFormattingCulture"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ApplyFormattingCultureProperty = DependencyProperty.Register(
        nameof(ApplyFormattingCulture),
        typeof(bool),
        typeof(XLanguageSelector),
        new PropertyMetadata(true, OnApplyFormattingCultureChanged));
    #endregion

    #region ### Fields ###
    private static DataTemplate? defaultItemTemplate;
    private static bool hasTriedLoadDefaultItemTemplate;
    private bool isLocalizationServiceSubscribed;
    private bool isSynchronizingSelection;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLanguageSelector"/> class.
    /// </summary>
    public XLanguageSelector()
    {
        this.SetCurrentValue(ItemsSourceProperty, XLanguages.Default);
        this.ApplyDefaultItemPresentation();

        this.Loaded += this.OnLoaded;
        this.Unloaded += this.OnUnloaded;
        this.SelectionChanged += this.OnSelectionChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets whether a language selection also changes number, date and time formatting culture.
    /// </summary>
    public bool ApplyFormattingCulture
    {
        get => (bool)this.GetValue(ApplyFormattingCultureProperty);
        set => this.SetValue(ApplyFormattingCultureProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static DataTemplate? GetDefaultItemTemplate()
    {
        if (hasTriedLoadDefaultItemTemplate)
        {
            return defaultItemTemplate;
        }

        hasTriedLoadDefaultItemTemplate = true;

        try
        {
            ResourceDictionary dictionary = new()
            {
                Source = new Uri(
                    "/VIA.WPF.Controls;component/Themes/XLanguageSelector.xaml",
                    UriKind.Relative)
            };

            defaultItemTemplate = dictionary["XLanguageSelectorItemTemplate"] as DataTemplate;
        }
        catch (InvalidOperationException)
        {
            defaultItemTemplate = null;
        }
        catch (IOException)
        {
            defaultItemTemplate = null;
        }

        return defaultItemTemplate;
    }

    private void ApplyDefaultItemPresentation()
    {
        DataTemplate? itemTemplate = GetDefaultItemTemplate();

        if (itemTemplate is not null)
        {
            this.SetCurrentValue(ItemTemplateProperty, itemTemplate);
            return;
        }

        this.SetCurrentValue(DisplayMemberPathProperty, nameof(XLanguage.DisplayText));
    }

    private static void OnApplyFormattingCultureChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLanguageSelector selector &&
            selector.SelectedItem is XLanguage selectedLanguage)
        {
            XLocalizationService.Current.SetCulture(
                selectedLanguage.Culture,
                selector.ApplyFormattingCulture);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (this.ItemsSource is null)
        {
            this.SetCurrentValue(ItemsSourceProperty, XLanguages.Default);
        }

        this.SynchronizeSelection(XLocalizationService.Current.CurrentUICulture);

        if (this.isLocalizationServiceSubscribed)
        {
            return;
        }

        WeakEventManager<XLocalizationService, XLanguageChangedEventArgs>.AddHandler(
            XLocalizationService.Current,
            nameof(XLocalizationService.LanguageChanged),
            this.OnLanguageChanged);

        this.isLocalizationServiceSubscribed = true;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (!this.isLocalizationServiceSubscribed)
        {
            return;
        }

        WeakEventManager<XLocalizationService, XLanguageChangedEventArgs>.RemoveHandler(
            XLocalizationService.Current,
            nameof(XLocalizationService.LanguageChanged),
            this.OnLanguageChanged);

        this.isLocalizationServiceSubscribed = false;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.isSynchronizingSelection ||
            this.SelectedItem is not XLanguage selectedLanguage)
        {
            return;
        }

        XLocalizationService.Current.SetCulture(
            selectedLanguage.Culture,
            this.ApplyFormattingCulture);
    }

    private void OnLanguageChanged(object? sender, XLanguageChangedEventArgs e)
    {
        if (this.Dispatcher.CheckAccess())
        {
            this.SynchronizeSelection(e.CurrentCulture);
            return;
        }

        if (this.Dispatcher.HasShutdownStarted ||
            this.Dispatcher.HasShutdownFinished)
        {
            return;
        }

        this.Dispatcher.BeginInvoke(
            () => this.SynchronizeSelection(e.CurrentCulture));
    }

    private void SynchronizeSelection(CultureInfo culture)
    {
        IEnumerable<XLanguage> availableLanguages = this.Items
            .OfType<XLanguage>();

        XLanguage? matchingLanguage = XLanguages.FindBestMatch(
            culture,
            availableLanguages);

        if (matchingLanguage is null ||
            Equals(this.SelectedItem, matchingLanguage))
        {
            return;
        }

        this.isSynchronizingSelection = true;

        try
        {
            this.SetCurrentValue(SelectedItemProperty, matchingLanguage);
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }
    #endregion
}
#endregion
