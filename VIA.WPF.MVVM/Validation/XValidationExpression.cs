// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationExpression.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Reflection;

namespace VIA.WPF.MVVM;

#region ### Class XValidationExpression ###
/// <summary>
/// Provides helpers for extracting validation property names from lambda expressions.
/// </summary>
public static class XValidationExpression
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the property name represented by the specified property expression.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TValue">The property value type.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    /// <returns>The property name or dotted property path.</returns>
    public static string GetPropertyName<TModel, TValue>(Expression<Func<TModel, TValue>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        return GetPropertyName((LambdaExpression)propertyExpression);
    }

    /// <summary>
    /// Gets the property name represented by the specified parameterless property expression.
    /// </summary>
    /// <typeparam name="TValue">The property value type.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    /// <returns>The property name or dotted property path.</returns>
    public static string GetPropertyName<TValue>(Expression<Func<TValue>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        return GetPropertyName((LambdaExpression)propertyExpression);
    }

    /// <summary>
    /// Gets the property name represented by the specified lambda expression.
    /// </summary>
    /// <param name="propertyExpression">The property expression.</param>
    /// <returns>The property name or dotted property path.</returns>
    public static string GetPropertyName(LambdaExpression propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        return GetPropertyName(propertyExpression.Body);
    }

    /// <summary>
    /// Gets the property value represented by the specified property expression.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TValue">The property value type.</typeparam>
    /// <param name="model">The model instance.</param>
    /// <param name="propertyExpression">The property expression.</param>
    /// <returns>The property value.</returns>
    public static TValue GetPropertyValue<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        return propertyExpression.Compile().Invoke(model);
    }
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Gets the property name represented by the specified expression body.
    /// </summary>
    /// <param name="expression">The expression body.</param>
    /// <returns>The property name or dotted property path.</returns>
    internal static string GetPropertyName(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        List<string> segments = [];
        Expression? currentExpression = StripConvert(expression);

        while (currentExpression is MemberExpression memberExpression)
        {
            if (memberExpression.Member is not PropertyInfo)
            {
                throw new ArgumentException(
                    $"The expression member '{memberExpression.Member.Name}' is not a property.",
                    nameof(expression));
            }

            segments.Add(memberExpression.Member.Name);
            currentExpression = StripConvert(memberExpression.Expression);
        }

        if (segments.Count == 0)
        {
            throw new ArgumentException("The expression must select a property.", nameof(expression));
        }

        if (currentExpression is not null
            && currentExpression is not ParameterExpression
            && currentExpression is not ConstantExpression)
        {
            throw new ArgumentException("The expression must select a property directly from a model or captured instance.", nameof(expression));
        }

        segments.Reverse();
        return string.Join(".", segments);
    }

    /// <summary>
    /// Compiles the property expression.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TValue">The property value type.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    /// <returns>The compiled property accessor.</returns>
    internal static Func<TModel, TValue> CompilePropertyAccessor<TModel, TValue>(Expression<Func<TModel, TValue>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        _ = GetPropertyName((LambdaExpression)propertyExpression);
        return propertyExpression.Compile();
    }
    #endregion

    #region ### Private Methods ###
    private static Expression? StripConvert(Expression? expression)
    {
        while (expression is UnaryExpression unaryExpression
               && unaryExpression.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
        {
            expression = unaryExpression.Operand;
        }

        return expression;
    }
    #endregion
}
#endregion