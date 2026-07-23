// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationLocalization.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Resources;

namespace VIA.WPF.MVVM;

#region ### Class XValidationLocalization ###
/// <summary>
/// Provides global localization settings for VIA.WPF validation messages.
/// </summary>
/// <remarks>
/// These settings are process-wide by design. They are intended for application-level startup configuration.
/// Tests or host scenarios that temporarily change validation localization should capture the current
/// <see cref="XValidationLocalizationSettings"/> with <see cref="Capture"/> and restore it with
/// <see cref="Restore(XValidationLocalizationSettings)"/> afterwards. Access to the settings is synchronized
/// so callers always observe a consistent settings snapshot.
/// </remarks>
public static class XValidationLocalization
{
    #region ### Fields ###
    private static readonly Lock SyncRoot = new();
    private static ResourceManager? resourceManager;
    private static CultureInfo? culture;
    private static bool throwOnMissingResource;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the resource manager used to resolve validation text resource keys.
    /// </summary>
    /// <remarks>
    /// The value is global for the current process. Configure it once during application startup whenever possible.
    /// </remarks>
    public static ResourceManager? ResourceManager
    {
        get
        {
            lock (SyncRoot)
            {
                return resourceManager;
            }
        }

        set
        {
            lock (SyncRoot)
            {
                resourceManager = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the culture used to resolve validation texts when no explicit culture is supplied.
    /// </summary>
    /// <remarks>
    /// The value is global for the current process. When this value is <see langword="null"/>,
    /// <see cref="CultureInfo.CurrentUICulture"/> is used.
    /// </remarks>
    public static CultureInfo? Culture
    {
        get
        {
            lock (SyncRoot)
            {
                return culture;
            }
        }

        set
        {
            lock (SyncRoot)
            {
                culture = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether missing resources should throw an exception.
    /// </summary>
    /// <remarks>
    /// The value is global for the current process. Library users usually keep this disabled in production
    /// and enable it in tests or development builds when missing resource keys should fail fast.
    /// </remarks>
    public static bool ThrowOnMissingResource
    {
        get
        {
            lock (SyncRoot)
            {
                return throwOnMissingResource;
            }
        }

        set
        {
            lock (SyncRoot)
            {
                throwOnMissingResource = value;
            }
        }
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Captures the current global validation localization settings.
    /// </summary>
    /// <returns>The captured settings.</returns>
    public static XValidationLocalizationSettings Capture()
    {
        lock (SyncRoot)
        {
            return new XValidationLocalizationSettings(resourceManager, culture, throwOnMissingResource);
        }
    }

    /// <summary>
    /// Restores previously captured global validation localization settings.
    /// </summary>
    /// <param name="settings">The settings to restore.</param>
    public static void Restore(XValidationLocalizationSettings settings)
    {
        lock (SyncRoot)
        {
            resourceManager = settings.ResourceManager;
            culture = settings.Culture;
            throwOnMissingResource = settings.ThrowOnMissingResource;
        }
    }

    /// <summary>
    /// Resolves a validation text using the configured resource manager.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="culture">The optional culture.</param>
    /// <returns>The resolved text.</returns>
    public static string Resolve(XValidationText text, CultureInfo? culture = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        ResourceManager? resourceManagerSnapshot;
        CultureInfo? configuredCultureSnapshot;
        bool throwOnMissingResourceSnapshot;

        lock (SyncRoot)
        {
            resourceManagerSnapshot = resourceManager;
            configuredCultureSnapshot = XValidationLocalization.culture;
            throwOnMissingResourceSnapshot = throwOnMissingResource;
        }

        CultureInfo effectiveCulture = culture ?? configuredCultureSnapshot ?? CultureInfo.CurrentUICulture;

        if (!text.IsResourceKey)
        {
            return FormatText(text.FallbackText ?? string.Empty, text.Arguments, effectiveCulture);
        }

        string? resourceKey = text.ResourceKey;
        if (string.IsNullOrWhiteSpace(resourceKey))
        {
            return FormatText(text.FallbackText ?? string.Empty, text.Arguments, effectiveCulture);
        }

        string? resolvedText = ResolveResource(resourceManagerSnapshot, resourceKey, effectiveCulture, throwOnMissingResourceSnapshot);
        if (!string.IsNullOrWhiteSpace(resolvedText))
        {
            return FormatText(resolvedText, text.Arguments, effectiveCulture);
        }

        if (!string.IsNullOrWhiteSpace(text.FallbackText))
        {
            return FormatText(text.FallbackText, text.Arguments, effectiveCulture);
        }

        if (throwOnMissingResourceSnapshot)
        {
            throw new MissingManifestResourceException($"The validation resource key '{resourceKey}' could not be resolved.");
        }

        return FormatText(resourceKey, text.Arguments, effectiveCulture);
    }
    #endregion

    #region ### Private Methods ###
    private static string FormatText(string text, IReadOnlyList<object?> arguments, CultureInfo culture)
    {
        return arguments.Count == 0
            ? text
            : string.Format(culture, text, arguments.ToArray());
    }

    private static string? ResolveResource(ResourceManager? resourceManager, string resourceKey, CultureInfo culture, bool throwOnMissingResource)
    {
        if (resourceManager is null)
        {
            return null;
        }

        try
        {
            return resourceManager.GetString(resourceKey, culture);
        }
        catch (MissingManifestResourceException)
        {
            if (throwOnMissingResource)
            {
                throw;
            }

            return null;
        }
    }
    #endregion
}
#endregion

#region ### Struct XValidationLocalizationSettings ###
/// <summary>
/// Represents a snapshot of the global <see cref="XValidationLocalization"/> settings.
/// </summary>
/// <param name="ResourceManager">The captured resource manager.</param>
/// <param name="Culture">The captured culture.</param>
/// <param name="ThrowOnMissingResource">The captured missing-resource behavior.</param>
public readonly record struct XValidationLocalizationSettings(
    ResourceManager? ResourceManager,
    CultureInfo? Culture,
    bool ThrowOnMissingResource);
#endregion
