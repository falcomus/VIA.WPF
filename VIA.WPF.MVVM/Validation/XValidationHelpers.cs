// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationHelpers.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

namespace VIA.WPF.MVVM;

#region ### Class XValidationHelpers ###
/// <summary>
/// Provides shared helper methods for the validation infrastructure.
/// </summary>
internal static class XValidationHelpers
{
    #region ### Internal Methods ###
    /// <summary>
    /// Determines whether a value should be treated as empty by required-field validation.
    /// </summary>
    /// <param name="value">The value to inspect.</param>
    /// <returns><c>true</c> when the value is empty; otherwise <c>false</c>.</returns>
    internal static bool IsEmpty(object? value)
    {
        return value switch
        {
            null => true,
            string text => string.IsNullOrWhiteSpace(text),
            Guid guid => guid == Guid.Empty,
            IEnumerable enumerable => !enumerable.Cast<object?>().Any(),
            _ => false
        };
    }

    /// <summary>
    /// Normalizes validation property names.
    /// </summary>
    /// <param name="propertyNames">The property names to normalize.</param>
    /// <returns>The normalized property names. An empty property name represents entity-level validation.</returns>
    internal static IReadOnlyList<string> NormalizePropertyNames(IEnumerable<string>? propertyNames)
    {
        string[] names = propertyNames?
            .Where(propertyName => !string.IsNullOrWhiteSpace(propertyName))
            .Select(propertyName => propertyName.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray() ?? [];

        return names.Length == 0 ? [string.Empty] : names;
    }

    /// <summary>
    /// Normalizes validation property names and removes the entity-level placeholder.
    /// </summary>
    /// <param name="propertyNames">The property names to normalize.</param>
    /// <returns>The normalized property names without entity-level entries.</returns>
    internal static string[] NormalizeExplicitPropertyNames(IEnumerable<string>? propertyNames)
    {
        return NormalizePropertyNames(propertyNames)
            .Where(propertyName => !string.IsNullOrEmpty(propertyName))
            .ToArray();
    }

    /// <summary>
    /// Compares two validation message sequences for semantic equality.
    /// </summary>
    /// <param name="first">The first sequence.</param>
    /// <param name="second">The second sequence.</param>
    /// <returns><c>true</c> when both sequences contain equivalent validation messages; otherwise <c>false</c>.</returns>
    internal static bool ValidationMessagesEqual(IReadOnlyList<XValidationError> first, IReadOnlyList<XValidationError> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        if (first.Count != second.Count)
        {
            return false;
        }

        for (int index = 0; index < first.Count; index++)
        {
            if (!ValidationMessagesEqual(first[index], second[index]))
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region ### Private Methods ###
    private static bool ValidationMessagesEqual(XValidationError first, XValidationError second)
    {
        return first.Severity == second.Severity
            && string.Equals(first.Code, second.Code, StringComparison.Ordinal)
            && ValidationTextsEqual(first.Text, second.Text)
            && first.PropertyNames.SequenceEqual(second.PropertyNames, StringComparer.Ordinal);
    }

    private static bool ValidationTextsEqual(XValidationText first, XValidationText second)
    {
        return string.Equals(first.ResourceKey, second.ResourceKey, StringComparison.Ordinal)
            && string.Equals(first.FallbackText, second.FallbackText, StringComparison.Ordinal)
            && first.IsResourceKey == second.IsResourceKey
            && first.Arguments.SequenceEqual(second.Arguments);
    }
    #endregion
}
#endregion
