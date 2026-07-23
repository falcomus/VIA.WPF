// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace VIA.WPF.MVVM;

#region ### Class XValidationContext ###
/// <summary>
/// Collects validation messages during validation execution.
/// </summary>
public sealed class XValidationContext
{
    #region ### Fields ###
    private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromMilliseconds(250);

    private static readonly Regex EmailAddressRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
        RegexMatchTimeout);

    private static readonly string[] WebUrlSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps];

    private readonly List<XValidationError> messages = [];
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationContext"/> class.
    /// </summary>
    /// <param name="source">The validated source object.</param>
    public XValidationContext(object source)
    {
        this.Source = source ?? throw new ArgumentNullException(nameof(source));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the validated source object.
    /// </summary>
    public object Source { get; }

    /// <summary>
    /// Gets all collected validation messages.
    /// </summary>
    public IReadOnlyList<XValidationError> Messages => this.messages;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Adds a validation error.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddError(XValidationText text, params string[] propertyNames)
    {
        this.AddMessage(text, XValidationSeverity.Error, propertyNames);
    }

    /// <summary>
    /// Adds a validation error using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddError(string resourceKey, params string[] propertyNames)
    {
        this.AddError(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds a validation warning.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddWarning(XValidationText text, params string[] propertyNames)
    {
        this.AddMessage(text, XValidationSeverity.Warning, propertyNames);
    }

    /// <summary>
    /// Adds a validation warning using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddWarning(string resourceKey, params string[] propertyNames)
    {
        this.AddWarning(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds a validation information message.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddInformation(XValidationText text, params string[] propertyNames)
    {
        this.AddMessage(text, XValidationSeverity.Information, propertyNames);
    }

    /// <summary>
    /// Adds a validation information message using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void AddInformation(string resourceKey, params string[] propertyNames)
    {
        this.AddInformation(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds a validation message.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="code">The optional technical validation code.</param>
    public void AddMessage(XValidationText text, XValidationSeverity severity, IEnumerable<string>? propertyNames = null, string? code = null)
    {
        this.messages.Add(new XValidationError(text, severity, propertyNames, code));
    }

    /// <summary>
    /// Adds an error when the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void ErrorIf(bool condition, XValidationText text, params string[] propertyNames)
    {
        if (condition)
        {
            this.AddError(text, propertyNames);
        }
    }

    /// <summary>
    /// Adds an error when the specified condition is true using a resource key.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void ErrorIf(bool condition, string resourceKey, params string[] propertyNames)
    {
        this.ErrorIf(condition, XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds a warning when the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void WarningIf(bool condition, XValidationText text, params string[] propertyNames)
    {
        if (condition)
        {
            this.AddWarning(text, propertyNames);
        }
    }

    /// <summary>
    /// Adds a warning when the specified condition is true using a resource key.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void WarningIf(bool condition, string resourceKey, params string[] propertyNames)
    {
        this.WarningIf(condition, XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds an information message when the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void InformationIf(bool condition, XValidationText text, params string[] propertyNames)
    {
        if (condition)
        {
            this.AddInformation(text, propertyNames);
        }
    }

    /// <summary>
    /// Adds an information message when the specified condition is true using a resource key.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    public void InformationIf(bool condition, string resourceKey, params string[] propertyNames)
    {
        this.InformationIf(condition, XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Adds a required field error when the specified value is empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Required(object? value, string propertyName, string resourceKey)
    {
        this.Required(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required field error when the specified value is empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Required(object? value, string propertyName, XValidationText text)
    {
        this.ErrorIf(XValidationHelpers.IsEmpty(value), text, propertyName);
    }

    /// <summary>
    /// Adds a required field error when the condition is true and the value is empty.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RequiredIf(bool condition, object? value, string propertyName, string resourceKey)
    {
        this.RequiredIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required field error when the condition is true and the value is empty.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RequiredIf(bool condition, object? value, string propertyName, XValidationText text)
    {
        this.ErrorIf(condition && XValidationHelpers.IsEmpty(value), text, propertyName);
    }

    /// <summary>
    /// Adds a required field error when the nullable value has no value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RequiredNullable<T>(T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.RequiredNullable(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required field error when the nullable value has no value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RequiredNullable<T>(T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(!value.HasValue, text, propertyName);
    }

    /// <summary>
    /// Adds a required field error when the condition is true and the nullable value has no value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RequiredNullableIf<T>(bool condition, T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.RequiredNullableIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required field error when the condition is true and the nullable value has no value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RequiredNullableIf<T>(bool condition, T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(condition && !value.HasValue, text, propertyName);
    }

    /// <summary>
    /// Adds an error when the specified value is the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void NotDefault<T>(T value, string propertyName, string resourceKey)
        where T : struct
    {
        this.NotDefault(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the specified value is the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void NotDefault<T>(T value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(EqualityComparer<T>.Default.Equals(value, default), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the specified value is the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void NotDefaultIf<T>(bool condition, T value, string propertyName, string resourceKey)
        where T : struct
    {
        this.NotDefaultIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the specified value is the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void NotDefaultIf<T>(bool condition, T value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(condition && EqualityComparer<T>.Default.Equals(value, default), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the nullable value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void NotDefault<T>(T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.NotDefault(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the nullable value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void NotDefault<T>(T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(!value.HasValue || EqualityComparer<T>.Default.Equals(value.Value, default), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the nullable value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void NotDefaultIf<T>(bool condition, T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.NotDefaultIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the nullable value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void NotDefaultIf<T>(bool condition, T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.ErrorIf(condition && (!value.HasValue || EqualityComparer<T>.Default.Equals(value.Value, default)), text, propertyName);
    }

    /// <summary>
    /// Adds an error when a selection value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The selection value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RequiredSelection<T>(T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.RequiredSelection(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when a selection value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The selection value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RequiredSelection<T>(T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.NotDefault(value, propertyName, text);
    }

    /// <summary>
    /// Adds an error when a condition is true and a selection value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The selection value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RequiredSelectionIf<T>(bool condition, T? value, string propertyName, string resourceKey)
        where T : struct
    {
        this.RequiredSelectionIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when a condition is true and a selection value has no value or contains the default value of its type.
    /// </summary>
    /// <typeparam name="T">The selection value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RequiredSelectionIf<T>(bool condition, T? value, string propertyName, XValidationText text)
        where T : struct
    {
        this.NotDefaultIf(condition, value, propertyName, text);
    }

    /// <summary>
    /// Adds an error when the string is shorter than the specified minimum length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MinLength(string? value, int minimumLength, string propertyName, string resourceKey)
    {
        this.MinLength(value, minimumLength, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string is shorter than the specified minimum length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MinLength(string? value, int minimumLength, string propertyName, XValidationText text)
    {
        this.ErrorIf(value is not null && value.Length < minimumLength, text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is shorter than the specified minimum length.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MinLengthIf(bool condition, string? value, int minimumLength, string propertyName, string resourceKey)
    {
        this.MinLengthIf(condition, value, minimumLength, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is shorter than the specified minimum length.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MinLengthIf(bool condition, string? value, int minimumLength, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.MinLength(value, minimumLength, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the string exceeds the specified maximum length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MaxLength(string? value, int maximumLength, string propertyName, string resourceKey)
    {
        this.MaxLength(value, maximumLength, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string exceeds the specified maximum length.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MaxLength(string? value, int maximumLength, string propertyName, XValidationText text)
    {
        this.ErrorIf(value?.Length > maximumLength, text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string exceeds the specified maximum length.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MaxLengthIf(bool condition, string? value, int maximumLength, string propertyName, string resourceKey)
    {
        this.MaxLengthIf(condition, value, maximumLength, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string exceeds the specified maximum length.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MaxLengthIf(bool condition, string? value, int maximumLength, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.MaxLength(value, maximumLength, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern.
    /// </summary>
    /// <remarks>
    /// For frequently executed validations, prefer the <see cref="Matches(string?, Regex, string, XValidationText)" /> overload with a cached
    /// <see cref="Regex" /> instance, for example one created through <c>[GeneratedRegex]</c>.
    /// </remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Matches(string? value, string pattern, string propertyName, string resourceKey)
    {
        this.Matches(value, pattern, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern.
    /// </summary>
    /// <remarks>
    /// For frequently executed validations, prefer the <see cref="Matches(string?, Regex, string, XValidationText)" /> overload with a cached
    /// <see cref="Regex" /> instance, for example one created through <c>[GeneratedRegex]</c>.
    /// </remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Matches(string? value, string pattern, RegexOptions options, string propertyName, string resourceKey)
    {
        this.Matches(value, pattern, options, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern.
    /// </summary>
    /// <remarks>
    /// For frequently executed validations, prefer the <see cref="Matches(string?, Regex, string, XValidationText)" /> overload with a cached
    /// <see cref="Regex" /> instance, for example one created through <c>[GeneratedRegex]</c>.
    /// </remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Matches(string? value, string pattern, string propertyName, XValidationText text)
    {
        this.Matches(value, pattern, RegexOptions.None, propertyName, text);
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern.
    /// </summary>
    /// <remarks>
    /// For frequently executed validations, prefer the <see cref="Matches(string?, Regex, string, XValidationText)" /> overload with a cached
    /// <see cref="Regex" /> instance, for example one created through <c>[GeneratedRegex]</c>.
    /// </remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Matches(string? value, string pattern, RegexOptions options, string propertyName, XValidationText text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        this.ErrorIf(value is not null && !Regex.IsMatch(value, pattern, options, RegexMatchTimeout), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="regex">The regular expression.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Matches(string? value, Regex regex, string propertyName, string resourceKey)
    {
        this.Matches(value, regex, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="regex">The regular expression.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Matches(string? value, Regex regex, string propertyName, XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(regex);
        this.ErrorIf(value is not null && !regex.IsMatch(value), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MatchesIf(bool condition, string? value, string pattern, string propertyName, string resourceKey)
    {
        this.MatchesIf(condition, value, pattern, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MatchesIf(bool condition, string? value, string pattern, RegexOptions options, string propertyName, string resourceKey)
    {
        this.MatchesIf(condition, value, pattern, options, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MatchesIf(bool condition, string? value, string pattern, string propertyName, XValidationText text)
    {
        this.MatchesIf(condition, value, pattern, RegexOptions.None, propertyName, text);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MatchesIf(bool condition, string? value, string pattern, RegexOptions options, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.Matches(value, pattern, options, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="regex">The regular expression.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MatchesIf(bool condition, string? value, Regex regex, string propertyName, string resourceKey)
    {
        this.MatchesIf(condition, value, regex, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string does not match the specified regular expression.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="regex">The regular expression.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MatchesIf(bool condition, string? value, Regex regex, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.Matches(value, regex, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid e-mail address.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Email(string? value, string propertyName, string resourceKey)
    {
        this.Email(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid e-mail address.
    /// </summary>
    /// <remarks>
    /// The check is intentionally pragmatic for form validation: it rejects whitespace, display-name forms and addresses without a dotted domain.
    /// </remarks>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Email(string? value, string propertyName, XValidationText text)
    {
        this.ErrorIf(value is not null && !IsEmail(value), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid e-mail address.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void EmailIf(bool condition, string? value, string propertyName, string resourceKey)
    {
        this.EmailIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid e-mail address.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void EmailIf(bool condition, string? value, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.Email(value, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Url(string? value, string propertyName, string resourceKey)
    {
        this.Url(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL with an allowed URI scheme.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="allowedSchemes">The allowed URI schemes. <see langword="null" /> allows every absolute URI scheme.</param>
    public void Url(string? value, string propertyName, string resourceKey, IEnumerable<string>? allowedSchemes)
    {
        this.Url(value, propertyName, XValidationText.Key(resourceKey), allowedSchemes);
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Url(string? value, string propertyName, XValidationText text)
    {
        this.Url(value, propertyName, text, null);
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL with an allowed URI scheme.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="allowedSchemes">The allowed URI schemes. <see langword="null" /> allows every absolute URI scheme.</param>
    public void Url(string? value, string propertyName, XValidationText text, IEnumerable<string>? allowedSchemes)
    {
        this.ErrorIf(value is not null && !IsAbsoluteUri(value, allowedSchemes), text, propertyName);
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void WebUrl(string? value, string propertyName, string resourceKey)
    {
        this.WebUrl(value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void WebUrl(string? value, string propertyName, XValidationText text)
    {
        this.Url(value, propertyName, text, WebUrlSchemes);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute URL.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void UrlIf(bool condition, string? value, string propertyName, string resourceKey)
    {
        this.UrlIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute URL with an allowed URI scheme.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="allowedSchemes">The allowed URI schemes. <see langword="null" /> allows every absolute URI scheme.</param>
    public void UrlIf(bool condition, string? value, string propertyName, string resourceKey, IEnumerable<string>? allowedSchemes)
    {
        this.UrlIf(condition, value, propertyName, XValidationText.Key(resourceKey), allowedSchemes);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute URL.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void UrlIf(bool condition, string? value, string propertyName, XValidationText text)
    {
        this.UrlIf(condition, value, propertyName, text, null);
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute URL with an allowed URI scheme.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="allowedSchemes">The allowed URI schemes. <see langword="null" /> allows every absolute URI scheme.</param>
    public void UrlIf(bool condition, string? value, string propertyName, XValidationText text, IEnumerable<string>? allowedSchemes)
    {
        if (condition)
        {
            this.Url(value, propertyName, text, allowedSchemes);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void WebUrlIf(bool condition, string? value, string propertyName, string resourceKey)
    {
        this.WebUrlIf(condition, value, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is not a syntactically valid absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void WebUrlIf(bool condition, string? value, string propertyName, XValidationText text)
    {
        if (condition)
        {
            this.WebUrl(value, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the value is outside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void Range<T>(T? value, T minimum, T maximum, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.Range(value, minimum, maximum, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the value is outside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void Range<T>(T? value, T minimum, T maximum, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (!value.HasValue)
        {
            return;
        }

        this.ErrorIf(value.Value.CompareTo(minimum) < 0 || value.Value.CompareTo(maximum) > 0, text, propertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is outside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void RangeIf<T>(bool condition, T? value, T minimum, T maximum, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.RangeIf(condition, value, minimum, maximum, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is outside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void RangeIf<T>(bool condition, T? value, T minimum, T maximum, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.Range(value, minimum, maximum, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the value is not greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void GreaterThan<T>(T? value, T threshold, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.GreaterThan(value, threshold, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the value is not greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void GreaterThan<T>(T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (value.HasValue)
        {
            this.ErrorIf(value.Value.CompareTo(threshold) <= 0, text, propertyName);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is not greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void GreaterThanIf<T>(bool condition, T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.GreaterThan(value, threshold, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the value is less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void GreaterThanOrEqual<T>(T? value, T threshold, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.GreaterThanOrEqual(value, threshold, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the value is less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void GreaterThanOrEqual<T>(T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (value.HasValue)
        {
            this.ErrorIf(value.Value.CompareTo(threshold) < 0, text, propertyName);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void GreaterThanOrEqualIf<T>(bool condition, T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.GreaterThanOrEqual(value, threshold, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the value is not less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void LessThan<T>(T? value, T threshold, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.LessThan(value, threshold, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the value is not less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void LessThan<T>(T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (value.HasValue)
        {
            this.ErrorIf(value.Value.CompareTo(threshold) >= 0, text, propertyName);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is not less than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void LessThanIf<T>(bool condition, T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.LessThan(value, threshold, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the value is greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void LessThanOrEqual<T>(T? value, T threshold, string propertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.LessThanOrEqual(value, threshold, propertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the value is greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void LessThanOrEqual<T>(T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (value.HasValue)
        {
            this.ErrorIf(value.Value.CompareTo(threshold) > 0, text, propertyName);
        }
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void LessThanOrEqualIf<T>(bool condition, T? value, T threshold, string propertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.LessThanOrEqual(value, threshold, propertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when the first value is greater than the second value.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    public void MustBeBeforeOrEqual<T>(T? firstValue, string firstPropertyName, T? secondValue, string secondPropertyName, string resourceKey)
        where T : struct, IComparable<T>
    {
        this.MustBeBeforeOrEqual(firstValue, firstPropertyName, secondValue, secondPropertyName, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an error when the first value is greater than the second value.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MustBeBeforeOrEqual<T>(T? firstValue, string firstPropertyName, T? secondValue, string secondPropertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (!firstValue.HasValue || !secondValue.HasValue)
        {
            return;
        }

        this.ErrorIf(firstValue.Value.CompareTo(secondValue.Value) > 0, text, firstPropertyName, secondPropertyName);
    }

    /// <summary>
    /// Adds an error when the condition is true and the first value is greater than the second value.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="text">The validation text.</param>
    public void MustBeBeforeOrEqualIf<T>(bool condition, T? firstValue, string firstPropertyName, T? secondValue, string secondPropertyName, XValidationText text)
        where T : struct, IComparable<T>
    {
        if (condition)
        {
            this.MustBeBeforeOrEqual(firstValue, firstPropertyName, secondValue, secondPropertyName, text);
        }
    }

    /// <summary>
    /// Adds an error when a custom comparison marks the values as invalid.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="isInvalid">The comparison predicate. Returning <see langword="true"/> adds an error.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="skipNullValues">Whether null values should be ignored before the comparison predicate runs.</param>
    public void Compare<T>(
        T? firstValue,
        string firstPropertyName,
        T? secondValue,
        string secondPropertyName,
        Func<T?, T?, bool> isInvalid,
        XValidationText text,
        bool skipNullValues = true)
    {
        ArgumentNullException.ThrowIfNull(isInvalid);

        if (skipNullValues && (firstValue is null || secondValue is null))
        {
            return;
        }

        this.ErrorIf(isInvalid(firstValue, secondValue), text, firstPropertyName, secondPropertyName);
    }

    /// <summary>
    /// Adds an error when a condition is true and a custom comparison marks the values as invalid.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="isInvalid">The comparison predicate. Returning <see langword="true"/> adds an error.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="skipNullValues">Whether null values should be ignored before the comparison predicate runs.</param>
    public void CompareIf<T>(
        bool condition,
        T? firstValue,
        string firstPropertyName,
        T? secondValue,
        string secondPropertyName,
        Func<T?, T?, bool> isInvalid,
        XValidationText text,
        bool skipNullValues = true)
    {
        if (condition)
        {
            this.Compare(firstValue, firstPropertyName, secondValue, secondPropertyName, isInvalid, text, skipNullValues);
        }
    }

    /// <summary>
    /// Adds an error when an asynchronous rule returns <see langword="false"/>.
    /// </summary>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task MustBeTrueAsync(Func<CancellationToken, Task<bool>> predicate, string resourceKey, CancellationToken cancellationToken, params string[] propertyNames)
    {
        await this.MustBeTrueAsync(predicate, XValidationText.Key(resourceKey), cancellationToken, propertyNames).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an error when an asynchronous rule returns <see langword="false"/>.
    /// </summary>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="text">The validation text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task MustBeTrueAsync(Func<CancellationToken, Task<bool>> predicate, XValidationText text, CancellationToken cancellationToken, params string[] propertyNames)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        bool result = await predicate(cancellationToken).ConfigureAwait(false);
        this.ErrorIf(!result, text, propertyNames);
    }
    #endregion

    #region ### Private Methods ###
    private static bool IsEmail(string value)
    {
        return EmailAddressRegex.IsMatch(value);
    }

    private static bool IsAbsoluteUri(string value, IEnumerable<string>? allowedSchemes)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
        {
            return false;
        }

        return IsAllowedScheme(uri, allowedSchemes);
    }

    private static bool IsAllowedScheme(Uri uri, IEnumerable<string>? allowedSchemes)
    {
        if (allowedSchemes is null)
        {
            return true;
        }

        return allowedSchemes
            .Where(scheme => !string.IsNullOrWhiteSpace(scheme))
            .Select(scheme => scheme.Trim())
            .Any(scheme => string.Equals(uri.Scheme, scheme, StringComparison.OrdinalIgnoreCase));
    }
    #endregion
}
#endregion
