// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XExternalValidationError.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Class XExternalValidationError ###
/// <summary>
/// Represents a validation message supplied by an external system, for example an API or persistence layer.
/// </summary>
public sealed class XExternalValidationError
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XExternalValidationError" /> class.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="code">The optional technical validation code.</param>
    public XExternalValidationError(
        XValidationText text,
        IEnumerable<string>? propertyNames = null,
        XValidationSeverity severity = XValidationSeverity.Error,
        string? code = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.Text = text;
        this.PropertyNames = XValidationHelpers.NormalizePropertyNames(propertyNames);
        this.Severity = severity;
        this.Code = code;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the validation text.
    /// </summary>
    public XValidationText Text { get; }

    /// <summary>
    /// Gets the affected property names.
    /// </summary>
    public IReadOnlyList<string> PropertyNames { get; }

    /// <summary>
    /// Gets the validation severity.
    /// </summary>
    public XValidationSeverity Severity { get; }

    /// <summary>
    /// Gets the optional technical validation code.
    /// </summary>
    public string? Code { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates an external validation error using a literal text.
    /// </summary>
    /// <param name="message">The validation message.</param>
    /// <param name="propertyName">The optional affected property name.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="code">The optional technical validation code.</param>
    /// <returns>The external validation error.</returns>
    public static XExternalValidationError FromText(
        string message,
        string? propertyName = null,
        XValidationSeverity severity = XValidationSeverity.Error,
        string? code = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new XExternalValidationError(XValidationText.Text(message), ToPropertyNames(propertyName), severity, code);
    }

    /// <summary>
    /// Creates an external validation error using a literal text.
    /// </summary>
    /// <param name="message">The validation message.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="code">The optional technical validation code.</param>
    /// <returns>The external validation error.</returns>
    public static XExternalValidationError FromText(
        string message,
        IEnumerable<string>? propertyNames,
        XValidationSeverity severity = XValidationSeverity.Error,
        string? code = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new XExternalValidationError(XValidationText.Text(message), propertyNames, severity, code);
    }

    /// <summary>
    /// Creates an external validation error using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyName">The optional affected property name.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="code">The optional technical validation code.</param>
    /// <returns>The external validation error.</returns>
    public static XExternalValidationError FromResourceKey(
        string resourceKey,
        string? propertyName = null,
        XValidationSeverity severity = XValidationSeverity.Error,
        string? code = null)
    {
        return new XExternalValidationError(XValidationText.Key(resourceKey), ToPropertyNames(propertyName), severity, code);
    }

    /// <summary>
    /// Converts this external validation error to a validation error instance.
    /// </summary>
    /// <returns>The validation error.</returns>
    public XValidationError ToValidationError()
    {
        return new XValidationError(this.Text, this.Severity, this.PropertyNames, this.Code);
    }
    #endregion

    #region ### Private Methods ###
    private static IEnumerable<string>? ToPropertyNames(string? propertyName)
    {
        return string.IsNullOrWhiteSpace(propertyName) ? null : [propertyName];
    }
    #endregion
}
#endregion
