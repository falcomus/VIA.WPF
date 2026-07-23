// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace VIA.WPF.Extensions;

#region ### Class StringExtensions ###
/// <summary>
/// Provides convenience methods for working with strings.
/// </summary>
public static class StringExtensions
{
    #region ### Fields ###
    /// <summary>
    /// Matches multiple whitespace characters.
    /// </summary>
    private static readonly Regex MultiWhitespaceRegex = new(@"\s+", RegexOptions.Compiled);
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets a value indicating whether the string is null, empty, or contains only whitespace.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns><c>true</c> if the string is null, empty, or whitespace; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Returns <c>null</c> when the string is null, empty, or whitespace; otherwise returns the trimmed value.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>The trimmed value, or <c>null</c>.</returns>
    public static string? NullIfWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    /// <summary>
    /// Compares two strings using ordinal ignore-case comparison.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="other">The value to compare with.</param>
    /// <returns><c>true</c> if both strings are equal; otherwise, <c>false</c>.</returns>
    public static bool EqualsIgnoreCase(this string? value, string? other)
    {
        return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks whether the source string contains the specified value using ordinal ignore-case comparison.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="searchValue">The search value.</param>
    /// <returns><c>true</c> if the search value is contained in the source value; otherwise, <c>false</c>.</returns>
    public static bool ContainsIgnoreCase(this string? value, string? searchValue)
    {
        if (value is null || searchValue is null)
        {
            return false;
        }

        return value.Contains(searchValue, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks whether the source string starts with the specified value using ordinal ignore-case comparison.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="searchValue">The search value.</param>
    /// <returns><c>true</c> if the source value starts with the search value; otherwise, <c>false</c>.</returns>
    public static bool StartsWithIgnoreCase(this string? value, string? searchValue)
    {
        if (value is null || searchValue is null)
        {
            return false;
        }

        return value.StartsWith(searchValue, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Normalizes text for search and filtering by trimming, lower-casing, removing diacritics, and collapsing whitespace.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <returns>The normalized search text.</returns>
    public static string NormalizeSearchText(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        string normalized = value.Trim().Normalize(NormalizationForm.FormD);
        StringBuilder builder = new(normalized.Length);

        foreach (char character in normalized)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);

            if (category != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(char.ToLowerInvariant(character));
            }
        }

        return MultiWhitespaceRegex.Replace(builder.ToString().Normalize(NormalizationForm.FormC), " ");
    }

    /// <summary>
    /// Limits the string to the specified maximum length and appends an ellipsis when truncated.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="maximumLength">The maximum length.</param>
    /// <param name="ellipsis">The ellipsis text.</param>
    /// <returns>The limited string.</returns>
    public static string LimitLength(this string? value, int maximumLength, string ellipsis = "…")
    {
        if (string.IsNullOrEmpty(value) || maximumLength <= 0)
        {
            return string.Empty;
        }

        if (value.Length <= maximumLength)
        {
            return value;
        }

        if (string.IsNullOrEmpty(ellipsis) || ellipsis.Length >= maximumLength)
        {
            return value[..maximumLength];
        }

        return value[..(maximumLength - ellipsis.Length)] + ellipsis;
    }
    #endregion
}
#endregion
