// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationText.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace VIA.WPF.MVVM;

#region ### Class XValidationText ###
/// <summary>
/// Represents a validation text that can either be literal text or a localization resource key.
/// </summary>
public sealed class XValidationText
{
    #region ### Constructors ###
    private XValidationText(string? resourceKey, string? fallbackText, bool isResourceKey, object?[]? arguments)
    {
        this.ResourceKey = resourceKey;
        this.FallbackText = fallbackText;
        this.IsResourceKey = isResourceKey;
        this.Arguments = arguments?.ToArray() ?? [];
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the localization resource key.
    /// </summary>
    public string? ResourceKey { get; }

    /// <summary>
    /// Gets the fallback text used when no localized value can be resolved.
    /// </summary>
    public string? FallbackText { get; }

    /// <summary>
    /// Gets a value indicating whether this instance represents a localization resource key.
    /// </summary>
    public bool IsResourceKey { get; }

    /// <summary>
    /// Gets the formatting arguments used for localized or fallback validation text.
    /// </summary>
    public IReadOnlyList<object?> Arguments { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates a literal validation text.
    /// </summary>
    /// <param name="text">The literal text.</param>
    /// <param name="arguments">The optional formatting arguments.</param>
    /// <returns>The validation text.</returns>
    public static XValidationText Text(string text, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(text);
        return new XValidationText(null, text, false, arguments);
    }

    /// <summary>
    /// Creates a resource based validation text.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="fallbackText">The optional fallback text.</param>
    /// <param name="arguments">The optional formatting arguments.</param>
    /// <returns>The validation text.</returns>
    public static XValidationText Key(string resourceKey, string? fallbackText = null, params object?[] arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceKey);
        return new XValidationText(resourceKey, fallbackText, true, arguments);
    }

    /// <summary>
    /// Creates a resource based validation text without a fallback text.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The optional formatting arguments.</param>
    /// <returns>The validation text.</returns>
    public static XValidationText Resource(string resourceKey, params object?[] arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceKey);
        return new XValidationText(resourceKey, null, true, arguments);
    }

    /// <summary>
    /// Creates a resource based validation text with a fallback text.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="fallbackText">The optional fallback text.</param>
    /// <param name="arguments">The optional formatting arguments.</param>
    /// <returns>The validation text.</returns>
    public static XValidationText Resource(string resourceKey, string? fallbackText, params object?[] arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceKey);
        return new XValidationText(resourceKey, fallbackText, true, arguments);
    }

    /// <summary>
    /// Resolves the validation text for the specified culture.
    /// </summary>
    /// <param name="culture">The optional culture.</param>
    /// <returns>The resolved validation text.</returns>
    public string Resolve(CultureInfo? culture = null)
    {
        return XValidationLocalization.Resolve(this, culture);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return this.Resolve();
    }
    #endregion
}
#endregion
