// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLocalizationService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Resources;

namespace VIA.WPF.Localization;

#region ### Class XLocalizationService ###
/// <summary>
/// Provides the process-wide UI culture used by VIA.WPF localization bindings and application messages.
/// </summary>
/// <remarks>
/// The service does not own application resource files. Applications keep their own strongly typed resources and pass
/// the corresponding <see cref="ResourceManager"/> to <see cref="GetString"/> or <see cref="Format"/>.
/// </remarks>
public sealed class XLocalizationService
{
    #region ### Fields ###
    private readonly object syncRoot = new();
    private CultureInfo currentUICulture;
    private bool applyFormattingCulture = true;
    private bool throwOnMissingResource;
    #endregion

    #region ### Constructors ###
    private XLocalizationService()
    {
        this.currentUICulture = CultureInfo.CurrentUICulture;
    }
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared application localization service.
    /// </summary>
    public static XLocalizationService Current { get; } = new();
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs after the active UI culture changed.
    /// </summary>
    public event EventHandler<XLanguageChangedEventArgs>? LanguageChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the culture currently used to resolve localized UI strings.
    /// </summary>
    public CultureInfo CurrentUICulture
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.currentUICulture;
            }
        }
    }

    /// <summary>
    /// Gets whether a language change also changes number, date and time formatting culture.
    /// </summary>
    public bool ApplyFormattingCulture
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.applyFormattingCulture;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether missing resource manifests should be rethrown instead of using the fallback text.
    /// </summary>
    public bool ThrowOnMissingResource
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.throwOnMissingResource;
            }
        }

        set
        {
            lock (this.syncRoot)
            {
                this.throwOnMissingResource = value;
            }
        }
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Changes the active UI culture.
    /// </summary>
    /// <param name="cultureName">The culture name, for example <c>de-DE</c> or <c>en-US</c>.</param>
    /// <param name="applyFormattingCulture">
    /// <see langword="true"/> to also use the culture for number, date and time formatting.
    /// </param>
    public void SetCulture(string cultureName, bool applyFormattingCulture = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureName);
        this.SetCulture(CultureInfo.GetCultureInfo(cultureName), applyFormattingCulture);
    }

    /// <summary>
    /// Changes the active UI culture.
    /// </summary>
    /// <param name="culture">The culture to activate.</param>
    /// <param name="applyFormattingCulture">
    /// <see langword="true"/> to also use the culture for number, date and time formatting.
    /// </param>
    public void SetCulture(CultureInfo culture, bool applyFormattingCulture = true)
    {
        ArgumentNullException.ThrowIfNull(culture);

        CultureInfo previousCulture;
        bool cultureChanged;
        bool formattingModeChanged;

        lock (this.syncRoot)
        {
            previousCulture = this.currentUICulture;
            cultureChanged = !StringComparer.OrdinalIgnoreCase.Equals(previousCulture.Name, culture.Name);
            formattingModeChanged = this.applyFormattingCulture != applyFormattingCulture;

            this.currentUICulture = culture;
            this.applyFormattingCulture = applyFormattingCulture;
        }

        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        if (applyFormattingCulture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
        }

        if (cultureChanged || formattingModeChanged)
        {
            this.LanguageChanged?.Invoke(this, new XLanguageChangedEventArgs(previousCulture, culture));
        }
    }

    /// <summary>
    /// Resolves a localized resource string with the active UI culture.
    /// </summary>
    /// <param name="resourceManager">The application resource manager.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="fallbackText">The optional fallback text. The key itself is used when omitted.</param>
    /// <param name="culture">An optional explicit culture.</param>
    /// <returns>The resolved localized text.</returns>
    public string GetString(
        ResourceManager resourceManager,
        string key,
        string? fallbackText = null,
        CultureInfo? culture = null)
    {
        ArgumentNullException.ThrowIfNull(resourceManager);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        CultureInfo effectiveCulture = culture ?? this.CurrentUICulture;
        bool shouldThrow;

        lock (this.syncRoot)
        {
            shouldThrow = this.throwOnMissingResource;
        }

        try
        {
            return resourceManager.GetString(key, effectiveCulture)
                ?? fallbackText
                ?? key;
        }
        catch (MissingManifestResourceException) when (!shouldThrow)
        {
            return fallbackText ?? key;
        }
    }

    /// <summary>
    /// Resolves and formats a localized resource string with the active UI culture.
    /// </summary>
    /// <param name="resourceManager">The application resource manager.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="fallbackText">The optional fallback format text.</param>
    /// <param name="arguments">The format arguments.</param>
    /// <returns>The formatted localized text.</returns>
    public string Format(
        ResourceManager resourceManager,
        string key,
        string? fallbackText,
        params object?[] arguments)
    {
        string format = this.GetString(resourceManager, key, fallbackText);
        return string.Format(this.CurrentUICulture, format, arguments ?? Array.Empty<object?>());
    }
    #endregion
}
#endregion
