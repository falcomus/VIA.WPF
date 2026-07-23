// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationContextExpressionExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace VIA.WPF.MVVM;

#region ### Class XValidationContextExpressionExtensions ###
/// <summary>
/// Provides expression-based validation overloads for <see cref="XValidationContext" />.
/// </summary>
public static class XValidationContextExpressionExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Adds a validation error for the selected property.
    /// </summary>
    public static void AddError<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.AddError(text, XValidationExpression.GetPropertyName(propertyExpression));
    }

    /// <summary>
    /// Adds a validation error for the selected property using a resource key.
    /// </summary>
    public static void AddError<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        string resourceKey)
    {
        context.AddError(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a validation warning for the selected property.
    /// </summary>
    public static void AddWarning<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.AddWarning(text, XValidationExpression.GetPropertyName(propertyExpression));
    }

    /// <summary>
    /// Adds a validation warning for the selected property using a resource key.
    /// </summary>
    public static void AddWarning<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        string resourceKey)
    {
        context.AddWarning(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an informational validation message for the selected property.
    /// </summary>
    public static void AddInformation<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.AddInformation(text, XValidationExpression.GetPropertyName(propertyExpression));
    }

    /// <summary>
    /// Adds an informational validation message for the selected property using a resource key.
    /// </summary>
    public static void AddInformation<TModel, TValue>(
        this XValidationContext context,
        Expression<Func<TModel, TValue>> propertyExpression,
        string resourceKey)
    {
        context.AddInformation(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required-field error when the selected property is empty.
    /// </summary>
    public static void Required<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Required(GetValue(model, propertyExpression), XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a required-field error when the selected property is empty using a resource key.
    /// </summary>
    public static void Required<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue>> propertyExpression,
        string resourceKey)
    {
        context.Required(model, propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a required-field error when the condition is true and the selected property is empty.
    /// </summary>
    public static void RequiredIf<TModel, TValue>(
        this XValidationContext context,
        bool condition,
        TModel model,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        if (condition)
        {
            context.Required(model, propertyExpression, text);
        }
    }

    /// <summary>
    /// Adds a minimum-length error when the selected string property is too short.
    /// </summary>
    public static void MinLength<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        int minimumLength,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.MinLength(GetValue(model, propertyExpression), minimumLength, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a minimum-length error when the selected string property is too short using a resource key.
    /// </summary>
    public static void MinLength<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        int minimumLength,
        string resourceKey)
    {
        context.MinLength(model, propertyExpression, minimumLength, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a maximum-length error when the selected string property is too long.
    /// </summary>
    public static void MaxLength<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        int maximumLength,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.MaxLength(GetValue(model, propertyExpression), maximumLength, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a maximum-length error when the selected string property is too long using a resource key.
    /// </summary>
    public static void MaxLength<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        int maximumLength,
        string resourceKey)
    {
        context.MaxLength(model, propertyExpression, maximumLength, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a regular-expression error when the selected string property does not match the pattern.
    /// </summary>
    public static void Matches<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        string pattern,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Matches(GetValue(model, propertyExpression), pattern, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a regular-expression error when the selected string property does not match the pattern.
    /// </summary>
    public static void Matches<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        string pattern,
        RegexOptions options,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Matches(GetValue(model, propertyExpression), pattern, options, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a regular-expression error when the selected string property does not match the compiled regex.
    /// </summary>
    public static void Matches<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        Regex regex,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Matches(GetValue(model, propertyExpression), regex, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds an email-format error when the selected string property is not a valid email address.
    /// </summary>
    public static void Email<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Email(GetValue(model, propertyExpression), XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds an email-format error when the selected string property is not a valid email address using a resource key.
    /// </summary>
    public static void Email<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        string resourceKey)
    {
        context.Email(model, propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an absolute-URL error when the selected string property is not a valid URL.
    /// </summary>
    public static void Url<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        XValidationText text,
        IEnumerable<string>? allowedSchemes = null)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Url(GetValue(model, propertyExpression), XValidationExpression.GetPropertyName(propertyExpression), text, allowedSchemes);
    }

    /// <summary>
    /// Adds an absolute-URL error when the selected string property is not a valid URL using a resource key.
    /// </summary>
    public static void Url<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        string resourceKey,
        IEnumerable<string>? allowedSchemes = null)
    {
        context.Url(model, propertyExpression, XValidationText.Key(resourceKey), allowedSchemes);
    }

    /// <summary>
    /// Adds a web-URL error when the selected string property is not a valid HTTP or HTTPS URL.
    /// </summary>
    public static void WebUrl<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.WebUrl(GetValue(model, propertyExpression), XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a web-URL error when the selected string property is not a valid HTTP or HTTPS URL using a resource key.
    /// </summary>
    public static void WebUrl<TModel>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, string?>> propertyExpression,
        string resourceKey)
    {
        context.WebUrl(model, propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a range error when the selected nullable value property is outside the specified range.
    /// </summary>
    public static void Range<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue minimum,
        TValue maximum,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Range(GetValue(model, propertyExpression), minimum, maximum, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a range error when the selected value property is outside the specified range.
    /// </summary>
    public static void Range<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue minimum,
        TValue maximum,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.Range(GetValue(model, propertyExpression), minimum, maximum, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a greater-than error when the selected nullable value property is not greater than the threshold.
    /// </summary>
    public static void GreaterThan<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.GreaterThan(GetValue(model, propertyExpression), threshold, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds a less-than error when the selected nullable value property is not less than the threshold.
    /// </summary>
    public static void LessThan<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        context.LessThan(GetValue(model, propertyExpression), threshold, XValidationExpression.GetPropertyName(propertyExpression), text);
    }

    /// <summary>
    /// Adds an error when the first nullable value property is greater than the second.
    /// </summary>
    public static void MustBeBeforeOrEqual<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue?>> firstPropertyExpression,
        Expression<Func<TModel, TValue?>> secondPropertyExpression,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(firstPropertyExpression);
        ArgumentNullException.ThrowIfNull(secondPropertyExpression);

        context.MustBeBeforeOrEqual(
            GetValue(model, firstPropertyExpression),
            XValidationExpression.GetPropertyName(firstPropertyExpression),
            GetValue(model, secondPropertyExpression),
            XValidationExpression.GetPropertyName(secondPropertyExpression),
            text);
    }

    /// <summary>
    /// Adds an error when a custom comparison marks the selected properties as invalid.
    /// </summary>
    public static void Compare<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        Expression<Func<TModel, TValue>> firstPropertyExpression,
        Expression<Func<TModel, TValue>> secondPropertyExpression,
        Func<TValue?, TValue?, bool> isInvalid,
        XValidationText text,
        bool skipNullValues = true)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(firstPropertyExpression);
        ArgumentNullException.ThrowIfNull(secondPropertyExpression);

        context.Compare(
            GetValue(model, firstPropertyExpression),
            XValidationExpression.GetPropertyName(firstPropertyExpression),
            GetValue(model, secondPropertyExpression),
            XValidationExpression.GetPropertyName(secondPropertyExpression),
            isInvalid,
            text,
            skipNullValues);
    }
    #endregion

    #region ### Private Methods ###
    private static TValue GetValue<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression)
    {
        return XValidationExpression.GetPropertyValue(model, propertyExpression);
    }
    #endregion
}
#endregion
