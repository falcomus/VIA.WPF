// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationBuilder.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace VIA.WPF.MVVM;

#region ### Class XValidationBuilder ###
/// <summary>
/// Creates fluent validation builders.
/// </summary>
public static class XValidationBuilder
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates a validation builder for the specified model type.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <returns>The validation builder.</returns>
    public static XValidationBuilder<TModel> For<TModel>()
        where TModel : notnull
    {
        return new XValidationBuilder<TModel>();
    }
    #endregion
}
#endregion

#region ### Class XValidationBuilder{TModel} ###
/// <summary>
/// Builds reusable validation rule sets for a model type.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
public sealed class XValidationBuilder<TModel>
    where TModel : notnull
{
    #region ### Fields ###
    private readonly List<Func<TModel, XValidationContext, CancellationToken, Task>> rules = [];
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Adds a synchronous validation rule.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    /// <returns>This builder.</returns>
    public XValidationBuilder<TModel> Rule(Action<TModel, XValidationContext> rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        this.rules.Add(
            (model, context, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                rule(model, context);
                return Task.CompletedTask;
            });

        return this;
    }

    /// <summary>
    /// Adds a conditional synchronous validation rule.
    /// </summary>
    /// <param name="condition">The model condition.</param>
    /// <param name="rule">The rule to add when the condition is true.</param>
    /// <returns>This builder.</returns>
    public XValidationBuilder<TModel> RuleIf(Func<TModel, bool> condition, Action<TModel, XValidationContext> rule)
    {
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(rule);

        return this.Rule(
            (model, context) =>
            {
                if (condition(model))
                {
                    rule(model, context);
                }
            });
    }

    /// <summary>
    /// Adds an asynchronous validation rule.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    /// <returns>This builder.</returns>
    public XValidationBuilder<TModel> RuleAsync(Func<TModel, XValidationContext, CancellationToken, Task> rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        this.rules.Add(rule);
        return this;
    }

    /// <summary>
    /// Adds DataAnnotations validation for the model.
    /// </summary>
    /// <param name="validateAllProperties">Whether all properties should be validated.</param>
    /// <returns>This builder.</returns>
    public XValidationBuilder<TModel> DataAnnotations(bool validateAllProperties = true)
    {
        return this.Rule((model, context) => context.ValidateDataAnnotations(model, validateAllProperties));
    }

    /// <summary>
    /// Adds a required-field rule for the selected property.
    /// </summary>
    public XValidationBuilder<TModel> Required<TValue>(Expression<Func<TModel, TValue>> propertyExpression, XValidationText text)
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Required(accessor(model), propertyName, text));
    }

    /// <summary>
    /// Adds a required-field rule for the selected property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Required<TValue>(Expression<Func<TModel, TValue>> propertyExpression, string resourceKey)
    {
        return this.Required(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a conditional required-field rule for the selected property.
    /// </summary>
    public XValidationBuilder<TModel> RequiredIf<TValue>(
        Func<TModel, bool> condition,
        Expression<Func<TModel, TValue>> propertyExpression,
        XValidationText text)
    {
        ArgumentNullException.ThrowIfNull(condition);

        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule(
            (model, context) =>
            {
                if (condition(model))
                {
                    context.Required(accessor(model), propertyName, text);
                }
            });
    }

    /// <summary>
    /// Adds a minimum-length rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> MinLength(Expression<Func<TModel, string?>> propertyExpression, int minimumLength, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.MinLength(accessor(model), minimumLength, propertyName, text));
    }

    /// <summary>
    /// Adds a minimum-length rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> MinLength(Expression<Func<TModel, string?>> propertyExpression, int minimumLength, string resourceKey)
    {
        return this.MinLength(propertyExpression, minimumLength, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a maximum-length rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> MaxLength(Expression<Func<TModel, string?>> propertyExpression, int maximumLength, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.MaxLength(accessor(model), maximumLength, propertyName, text));
    }

    /// <summary>
    /// Adds a maximum-length rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> MaxLength(Expression<Func<TModel, string?>> propertyExpression, int maximumLength, string resourceKey)
    {
        return this.MaxLength(propertyExpression, maximumLength, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> Matches(Expression<Func<TModel, string?>> propertyExpression, string pattern, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Matches(accessor(model), pattern, propertyName, text));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Matches(Expression<Func<TModel, string?>> propertyExpression, string pattern, string resourceKey)
    {
        return this.Matches(propertyExpression, pattern, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> Matches(
        Expression<Func<TModel, string?>> propertyExpression,
        string pattern,
        RegexOptions options,
        XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Matches(accessor(model), pattern, options, propertyName, text));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Matches(
        Expression<Func<TModel, string?>> propertyExpression,
        string pattern,
        RegexOptions options,
        string resourceKey)
    {
        return this.Matches(propertyExpression, pattern, options, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property using a compiled regex.
    /// </summary>
    public XValidationBuilder<TModel> Matches(Expression<Func<TModel, string?>> propertyExpression, Regex regex, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Matches(accessor(model), regex, propertyName, text));
    }

    /// <summary>
    /// Adds a regex rule for the selected string property using a compiled regex and resource key.
    /// </summary>
    public XValidationBuilder<TModel> Matches(Expression<Func<TModel, string?>> propertyExpression, Regex regex, string resourceKey)
    {
        return this.Matches(propertyExpression, regex, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an email-format rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> Email(Expression<Func<TModel, string?>> propertyExpression, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Email(accessor(model), propertyName, text));
    }

    /// <summary>
    /// Adds an email-format rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Email(Expression<Func<TModel, string?>> propertyExpression, string resourceKey)
    {
        return this.Email(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds an absolute URL rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> Url(
        Expression<Func<TModel, string?>> propertyExpression,
        XValidationText text,
        IEnumerable<string>? allowedSchemes = null)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Url(accessor(model), propertyName, text, allowedSchemes));
    }

    /// <summary>
    /// Adds an absolute URL rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Url(
        Expression<Func<TModel, string?>> propertyExpression,
        string resourceKey,
        IEnumerable<string>? allowedSchemes = null)
    {
        return this.Url(propertyExpression, XValidationText.Key(resourceKey), allowedSchemes);
    }

    /// <summary>
    /// Adds a web URL rule for the selected string property.
    /// </summary>
    public XValidationBuilder<TModel> WebUrl(Expression<Func<TModel, string?>> propertyExpression, XValidationText text)
    {
        Func<TModel, string?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.WebUrl(accessor(model), propertyName, text));
    }

    /// <summary>
    /// Adds a web URL rule for the selected string property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> WebUrl(Expression<Func<TModel, string?>> propertyExpression, string resourceKey)
    {
        return this.WebUrl(propertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a range rule for the selected nullable value property.
    /// </summary>
    public XValidationBuilder<TModel> Range<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue minimum,
        TValue maximum,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Range(accessor(model), minimum, maximum, propertyName, text));
    }

    /// <summary>
    /// Adds a range rule for the selected nullable value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Range<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue minimum,
        TValue maximum,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.Range(propertyExpression, minimum, maximum, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a range rule for the selected value property.
    /// </summary>
    public XValidationBuilder<TModel> Range<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue minimum,
        TValue maximum,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.Range(accessor(model), minimum, maximum, propertyName, text));
    }

    /// <summary>
    /// Adds a range rule for the selected value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> Range<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue minimum,
        TValue maximum,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.Range(propertyExpression, minimum, maximum, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a greater-than rule for the selected nullable value property.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThan<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.GreaterThan(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a greater-than rule for the selected nullable value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThan<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.GreaterThan(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a greater-than rule for the selected value property.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThan<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.GreaterThan(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a greater-than rule for the selected value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThan<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.GreaterThan(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a greater-than-or-equal rule for the selected nullable value property.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThanOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.GreaterThanOrEqual(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a greater-than-or-equal rule for the selected nullable value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThanOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.GreaterThanOrEqual(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a greater-than-or-equal rule for the selected value property.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThanOrEqual<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.GreaterThanOrEqual(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a greater-than-or-equal rule for the selected value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> GreaterThanOrEqual<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.GreaterThanOrEqual(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a less-than rule for the selected nullable value property.
    /// </summary>
    public XValidationBuilder<TModel> LessThan<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.LessThan(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a less-than rule for the selected nullable value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> LessThan<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.LessThan(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a less-than rule for the selected value property.
    /// </summary>
    public XValidationBuilder<TModel> LessThan<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.LessThan(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a less-than rule for the selected value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> LessThan<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.LessThan(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a less-than-or-equal rule for the selected nullable value property.
    /// </summary>
    public XValidationBuilder<TModel> LessThanOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.LessThanOrEqual(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a less-than-or-equal rule for the selected nullable value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> LessThanOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.LessThanOrEqual(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a less-than-or-equal rule for the selected value property.
    /// </summary>
    public XValidationBuilder<TModel> LessThanOrEqual<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue> accessor = CreateAccessor(propertyExpression);
        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);

        return this.Rule((model, context) => context.LessThanOrEqual(accessor(model), threshold, propertyName, text));
    }

    /// <summary>
    /// Adds a less-than-or-equal rule for the selected value property using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> LessThanOrEqual<TValue>(
        Expression<Func<TModel, TValue>> propertyExpression,
        TValue threshold,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.LessThanOrEqual(propertyExpression, threshold, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a cross-property order rule for two nullable value properties.
    /// </summary>
    public XValidationBuilder<TModel> MustBeBeforeOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> firstPropertyExpression,
        Expression<Func<TModel, TValue?>> secondPropertyExpression,
        XValidationText text)
        where TValue : struct, IComparable<TValue>
    {
        Func<TModel, TValue?> firstAccessor = CreateAccessor(firstPropertyExpression);
        Func<TModel, TValue?> secondAccessor = CreateAccessor(secondPropertyExpression);
        string firstPropertyName = XValidationExpression.GetPropertyName(firstPropertyExpression);
        string secondPropertyName = XValidationExpression.GetPropertyName(secondPropertyExpression);

        return this.Rule(
            (model, context) => context.MustBeBeforeOrEqual(
                firstAccessor(model),
                firstPropertyName,
                secondAccessor(model),
                secondPropertyName,
                text));
    }

    /// <summary>
    /// Adds a cross-property order rule for two nullable value properties using a resource key.
    /// </summary>
    public XValidationBuilder<TModel> MustBeBeforeOrEqual<TValue>(
        Expression<Func<TModel, TValue?>> firstPropertyExpression,
        Expression<Func<TModel, TValue?>> secondPropertyExpression,
        string resourceKey)
        where TValue : struct, IComparable<TValue>
    {
        return this.MustBeBeforeOrEqual(firstPropertyExpression, secondPropertyExpression, XValidationText.Key(resourceKey));
    }

    /// <summary>
    /// Adds a custom cross-property comparison rule.
    /// </summary>
    public XValidationBuilder<TModel> Compare<TValue>(
        Expression<Func<TModel, TValue>> firstPropertyExpression,
        Expression<Func<TModel, TValue>> secondPropertyExpression,
        Func<TValue?, TValue?, bool> isInvalid,
        XValidationText text,
        bool skipNullValues = true)
    {
        ArgumentNullException.ThrowIfNull(isInvalid);

        Func<TModel, TValue> firstAccessor = CreateAccessor(firstPropertyExpression);
        Func<TModel, TValue> secondAccessor = CreateAccessor(secondPropertyExpression);
        string firstPropertyName = XValidationExpression.GetPropertyName(firstPropertyExpression);
        string secondPropertyName = XValidationExpression.GetPropertyName(secondPropertyExpression);

        return this.Rule(
            (model, context) => context.Compare(
                firstAccessor(model),
                firstPropertyName,
                secondAccessor(model),
                secondPropertyName,
                isInvalid,
                text,
                skipNullValues));
    }

    /// <summary>
    /// Adds an asynchronous boolean rule.
    /// </summary>
    public XValidationBuilder<TModel> MustBeTrueAsync(
        Func<TModel, CancellationToken, Task<bool>> predicate,
        XValidationText text,
        params Expression<Func<TModel, object?>>[] propertyExpressions)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        string[] propertyNames = propertyExpressions.Select(expression => XValidationExpression.GetPropertyName(expression)).ToArray();

        return this.RuleAsync(
            async (model, context, cancellationToken) =>
            {
                bool result = await predicate(model, cancellationToken).ConfigureAwait(false);
                context.ErrorIf(!result, text, propertyNames);
            });
    }

    /// <summary>
    /// Builds an immutable rule set from the current builder state.
    /// </summary>
    /// <returns>The validation rule set.</returns>
    public XValidationRuleSet<TModel> Build()
    {
        return new XValidationRuleSet<TModel>(this.rules);
    }
    #endregion

    #region ### Private Methods ###
    private static Func<TModel, TValue> CreateAccessor<TValue>(Expression<Func<TModel, TValue>> propertyExpression)
    {
        return XValidationExpression.CompilePropertyAccessor(propertyExpression);
    }
    #endregion
}
#endregion
