// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationDataAnnotationExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

using DataAnnotationValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using DataAnnotationValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using DataAnnotationValidator = System.ComponentModel.DataAnnotations.Validator;

namespace VIA.WPF.MVVM;

#region ### Class XValidationDataAnnotationExtensions ###
/// <summary>
/// Provides DataAnnotations integration for <see cref="XValidationContext" />.
/// </summary>
public static class XValidationDataAnnotationExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Adds errors for failed DataAnnotations validation on the context source object.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="validateAllProperties">Whether all properties should be validated.</param>
    public static void ValidateDataAnnotations(this XValidationContext context, bool validateAllProperties = true)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.ValidateDataAnnotations(context.Source, validateAllProperties);
    }

    /// <summary>
    /// Adds errors for failed DataAnnotations validation on the specified instance.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="instance">The instance to validate.</param>
    /// <param name="validateAllProperties">Whether all properties should be validated.</param>
    public static void ValidateDataAnnotations(this XValidationContext context, object instance, bool validateAllProperties = true)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(instance);

        DataAnnotationValidationContext dataAnnotationContext = new(instance);
        List<DataAnnotationValidationResult> results = [];

        if (DataAnnotationValidator.TryValidateObject(instance, dataAnnotationContext, results, validateAllProperties))
        {
            return;
        }

        AddResults(context, results);
    }

    /// <summary>
    /// Adds errors for failed DataAnnotations validation on a single property.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="instance">The instance containing the property.</param>
    /// <param name="propertyName">The property name.</param>
    public static void ValidateDataAnnotationsProperty(this XValidationContext context, object instance, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
            ?? throw new ArgumentException($"The property '{propertyName}' was not found on type '{instance.GetType().FullName}'.", nameof(propertyName));

        object? value = propertyInfo.GetValue(instance);
        DataAnnotationValidationContext dataAnnotationContext = new(instance)
        {
            MemberName = propertyName
        };

        List<DataAnnotationValidationResult> results = [];

        if (DataAnnotationValidator.TryValidateProperty(value, dataAnnotationContext, results))
        {
            return;
        }

        AddResults(context, results, propertyName);
    }

    /// <summary>
    /// Adds errors for failed DataAnnotations validation on a single property selected by expression.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TValue">The property value type.</typeparam>
    /// <param name="context">The validation context.</param>
    /// <param name="model">The model instance.</param>
    /// <param name="propertyExpression">The property expression.</param>
    public static void ValidateDataAnnotationsProperty<TModel, TValue>(
        this XValidationContext context,
        TModel model,
        System.Linq.Expressions.Expression<Func<TModel, TValue>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        string propertyName = XValidationExpression.GetPropertyName(propertyExpression);
        context.ValidateDataAnnotationsProperty(model, propertyName);
    }
    #endregion

    #region ### Private Methods ###
    private static void AddResults(XValidationContext context, IEnumerable<DataAnnotationValidationResult> results, string? fallbackPropertyName = null)
    {
        foreach (DataAnnotationValidationResult result in results)
        {
            if (result == DataAnnotationValidationResult.Success)
            {
                continue;
            }

            string message = string.IsNullOrWhiteSpace(result.ErrorMessage)
                ? "Validation failed."
                : result.ErrorMessage!;

            IEnumerable<string>? propertyNames = result.MemberNames.Any()
                ? result.MemberNames
                : string.IsNullOrWhiteSpace(fallbackPropertyName)
                    ? null
                    : new[] { fallbackPropertyName! };

            context.AddMessage(
                XValidationText.Text(message),
                XValidationSeverity.Error,
                propertyNames,
                result.GetType().Name);
        }
    }
    #endregion
}
#endregion
