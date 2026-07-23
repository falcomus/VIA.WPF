// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationContextParameterizedExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace VIA.WPF.MVVM;

#region ### Class XValidationContextParameterizedExtensions ###
/// <summary>
/// Provides parameterized resource-key helper overloads for <see cref="XValidationContext" />.
/// </summary>
public static class XValidationContextParameterizedExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Adds a validation error using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void AddError(this XValidationContext context, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Error, propertyNames);
    }

    /// <summary>
    /// Adds a validation warning using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void AddWarning(this XValidationContext context, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Warning, propertyNames);
    }

    /// <summary>
    /// Adds a validation information message using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void AddInformation(this XValidationContext context, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Information, propertyNames);
    }

    /// <summary>
    /// Adds an error when the specified condition is true using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void ErrorIf(this XValidationContext context, bool condition, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (condition)
        {
            context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Error, propertyNames);
        }
    }

    /// <summary>
    /// Adds a warning when the specified condition is true using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void WarningIf(this XValidationContext context, bool condition, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (condition)
        {
            context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Warning, propertyNames);
        }
    }

    /// <summary>
    /// Adds an information message when the specified condition is true using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void InformationIf(this XValidationContext context, bool condition, string resourceKey, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (condition)
        {
            context.AddMessage(XValidationText.Resource(resourceKey, arguments), XValidationSeverity.Information, propertyNames);
        }
    }

    /// <summary>
    /// Adds a required field error when the specified value is empty using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Required(this XValidationContext context, object? value, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Required(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds a required field error when the condition is true and the specified value is empty using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void RequiredIf(this XValidationContext context, bool condition, object? value, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.RequiredIf(condition, value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds a required field error when the nullable value has no value using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void RequiredNullable<T>(this XValidationContext context, T? value, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(context);

        context.RequiredNullable(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the specified value is the default value of its type using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void NotDefault<T>(this XValidationContext context, T value, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(context);

        context.NotDefault(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the nullable value has no value or contains the default value of its type using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void NotDefault<T>(this XValidationContext context, T? value, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(context);

        context.NotDefault(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when a selection value has no value or contains the default value of its type using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The selection value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void RequiredSelection<T>(this XValidationContext context, T? value, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(context);

        context.RequiredSelection(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string is shorter than the specified minimum length using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void MinLength(this XValidationContext context, string? value, int minimumLength, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.MinLength(value, minimumLength, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string is shorter than the specified minimum length using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimumLength">The minimum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void MinLengthIf(this XValidationContext context, bool condition, string? value, int minimumLength, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.MinLengthIf(condition, value, minimumLength, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string exceeds the specified maximum length using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void MaxLength(this XValidationContext context, string? value, int maximumLength, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.MaxLength(value, maximumLength, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the condition is true and the string exceeds the specified maximum length using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void MaxLengthIf(this XValidationContext context, bool condition, string? value, int maximumLength, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.MaxLengthIf(condition, value, maximumLength, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Matches(this XValidationContext context, string? value, string pattern, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Matches(value, pattern, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression pattern using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Matches(this XValidationContext context, string? value, string pattern, RegexOptions options, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Matches(value, pattern, options, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string does not match the specified regular expression using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="regex">The regular expression.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Matches(this XValidationContext context, string? value, Regex regex, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Matches(value, regex, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid e-mail address using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Email(this XValidationContext context, string? value, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Email(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Url(this XValidationContext context, string? value, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Url(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute URL with an allowed URI scheme using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="allowedSchemes">The allowed URI schemes. <see langword="null" /> allows every absolute URI scheme.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Url(this XValidationContext context, string? value, string propertyName, IEnumerable<string>? allowedSchemes, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Url(value, propertyName, XValidationText.Resource(resourceKey, arguments), allowedSchemes);
    }

    /// <summary>
    /// Adds an error when the string is not a syntactically valid absolute HTTP or HTTPS URL using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void WebUrl(this XValidationContext context, string? value, string propertyName, string resourceKey, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.WebUrl(value, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the value is outside the specified range using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Range<T>(this XValidationContext context, T? value, T minimum, T maximum, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Range(value, minimum, maximum, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the condition is true and the value is outside the specified range using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">The minimum value.</param>
    /// <param name="maximum">The maximum value.</param>
    /// <param name="propertyName">The affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void RangeIf<T>(this XValidationContext context, bool condition, T? value, T minimum, T maximum, string propertyName, string resourceKey, params object?[] arguments)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(context);

        context.RangeIf(condition, value, minimum, maximum, propertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when the first value is greater than the second value using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void MustBeBeforeOrEqual<T>(this XValidationContext context, T? firstValue, string firstPropertyName, T? secondValue, string secondPropertyName, string resourceKey, params object?[] arguments)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(context);

        context.MustBeBeforeOrEqual(firstValue, firstPropertyName, secondValue, secondPropertyName, XValidationText.Resource(resourceKey, arguments));
    }

    /// <summary>
    /// Adds an error when a custom comparison marks the values as invalid using a parameterized resource key.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="firstValue">The first value.</param>
    /// <param name="firstPropertyName">The first affected property name.</param>
    /// <param name="secondValue">The second value.</param>
    /// <param name="secondPropertyName">The second affected property name.</param>
    /// <param name="isInvalid">The comparison predicate. Returning <see langword="true"/> adds an error.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="skipNullValues">Whether null values should be ignored before the comparison predicate runs.</param>
    /// <param name="arguments">The formatting arguments.</param>
    public static void Compare<T>(
        this XValidationContext context,
        T? firstValue,
        string firstPropertyName,
        T? secondValue,
        string secondPropertyName,
        Func<T?, T?, bool> isInvalid,
        string resourceKey,
        bool skipNullValues = true,
        params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Compare(firstValue, firstPropertyName, secondValue, secondPropertyName, isInvalid, XValidationText.Resource(resourceKey, arguments), skipNullValues);
    }

    /// <summary>
    /// Adds an error when an asynchronous rule returns <see langword="false" /> using a resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task MustBeTrueAsync(this XValidationContext context, Func<CancellationToken, Task<bool>> predicate, string resourceKey, CancellationToken cancellationToken, params string[] propertyNames)
    {
        ArgumentNullException.ThrowIfNull(context);

        await context.MustBeTrueAsync(predicate, XValidationText.Key(resourceKey), cancellationToken, propertyNames).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an error when an asynchronous rule returns <see langword="false" /> using a parameterized resource key.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <param name="arguments">The formatting arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task MustBeTrueAsync(this XValidationContext context, Func<CancellationToken, Task<bool>> predicate, string resourceKey, CancellationToken cancellationToken, IEnumerable<string>? propertyNames, params object?[] arguments)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(predicate);

        bool result = await predicate(cancellationToken).ConfigureAwait(false);
        context.ErrorIf(!result, XValidationText.Resource(resourceKey, arguments), XValidationHelpers.NormalizePropertyNames(propertyNames).ToArray());
    }
    #endregion
}
#endregion
