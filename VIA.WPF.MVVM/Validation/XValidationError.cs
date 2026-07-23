// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationError.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace VIA.WPF.MVVM;

#region ### Class XValidationError ###
/// <summary>
/// Represents a validation message assigned to one or more properties.
/// </summary>
public sealed class XValidationError
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationError"/> class.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="code">The optional technical validation code.</param>
    public XValidationError(XValidationText text, XValidationSeverity severity, IEnumerable<string>? propertyNames = null, string? code = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.Text = text;
        this.Severity = severity;
        this.PropertyNames = XValidationHelpers.NormalizePropertyNames(propertyNames);
        this.Code = code;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the validation text.
    /// </summary>
    public XValidationText Text { get; }

    /// <summary>
    /// Gets the validation severity.
    /// </summary>
    public XValidationSeverity Severity { get; }

    /// <summary>
    /// Gets the affected property names.
    /// </summary>
    /// <remarks>
    /// Property names are normalized by trimming whitespace, removing empty names, applying ordinal distinctness
    /// and falling back to <see cref="string.Empty"/> for entity-level validation messages.
    /// </remarks>
    public IReadOnlyList<string> PropertyNames { get; }

    /// <summary>
    /// Gets the optional technical validation code.
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Gets the resolved validation message.
    /// </summary>
    public string Message => this.Resolve();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Resolves the validation message for the specified culture.
    /// </summary>
    /// <param name="culture">The optional culture.</param>
    /// <returns>The resolved validation message.</returns>
    public string Resolve(CultureInfo? culture = null)
    {
        return this.Text.Resolve(culture);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return this.Message;
    }
    #endregion
}
#endregion
