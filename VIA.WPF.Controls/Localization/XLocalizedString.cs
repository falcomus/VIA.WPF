// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLocalizedString.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Resources;
using System.Windows;

namespace VIA.WPF.Localization;

#region ### Class XLocalizedString ###
/// <summary>
/// Exposes a localized resource string that refreshes when the active VIA.WPF UI culture changes.
/// </summary>
public sealed class XLocalizedString : INotifyPropertyChanged
{
    #region ### Fields ###
    private readonly XLocalizationService localizationService;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLocalizedString"/> class.
    /// </summary>
    /// <param name="resourceManager">The application resource manager.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="fallbackText">The optional fallback text.</param>
    /// <param name="localizationService">The optional localization service.</param>
    public XLocalizedString(
        ResourceManager resourceManager,
        string key,
        string? fallbackText = null,
        XLocalizationService? localizationService = null)
    {
        ArgumentNullException.ThrowIfNull(resourceManager);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        this.ResourceManager = resourceManager;
        this.Key = key;
        this.FallbackText = fallbackText;
        this.localizationService = localizationService ?? XLocalizationService.Current;

        WeakEventManager<XLocalizationService, XLanguageChangedEventArgs>.AddHandler(
            this.localizationService,
            nameof(XLocalizationService.LanguageChanged),
            this.OnLanguageChanged);
    }
    #endregion

    #region ### Public Events ###
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the resource manager used by this localized string.
    /// </summary>
    public ResourceManager ResourceManager { get; }

    /// <summary>
    /// Gets the resource key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the optional fallback text.
    /// </summary>
    public string? FallbackText { get; }

    /// <summary>
    /// Gets the currently resolved localized text.
    /// </summary>
    public string Value => this.localizationService.GetString(
        this.ResourceManager,
        this.Key,
        this.FallbackText);
    #endregion

    #region ### Private Methods ###
    private void OnLanguageChanged(object? sender, XLanguageChangedEventArgs e)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
    }
    #endregion
}
#endregion
