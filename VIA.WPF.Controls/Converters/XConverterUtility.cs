// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XConverterUtility.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace VIA.WPF.Converters;

#region ### Class XConverterUtility ###
/// <summary>
/// Provides helper methods for VIA.WPF converters.
/// </summary>
internal static class XConverterUtility
{
    #region ### Internal Methods ###
    /// <summary>
    /// Compares two converter values using a robust string and value comparison fallback.
    /// </summary>
    /// <param name="left">The first value.</param>
    /// <param name="right">The second value.</param>
    /// <param name="culture">The culture used for conversion.</param>
    /// <returns><see langword="true"/> if both values are considered equal; otherwise <see langword="false"/>.</returns>
    internal static bool AreEqual(object? left, object? right, CultureInfo culture)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        if (left.Equals(right))
        {
            return true;
        }

        Type leftType = Nullable.GetUnderlyingType(left.GetType()) ?? left.GetType();

        try
        {
            object? convertedRight = ConvertParameter(right, leftType, culture);
            if (left.Equals(convertedRight))
            {
                return true;
            }
        }
        catch (InvalidOperationException)
        {
            // Ignore and fall back to string comparison.
        }

        return string.Equals(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Converts a converter parameter to a target type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The desired target type.</param>
    /// <param name="culture">The culture used for conversion.</param>
    /// <returns>The converted value.</returns>
    internal static object? ConvertParameter(object? value, Type targetType, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        Type nonNullableTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (nonNullableTargetType.IsInstanceOfType(value))
        {
            return value;
        }

        if (nonNullableTargetType.IsEnum)
        {
            return Enum.Parse(nonNullableTargetType, value.ToString() ?? string.Empty, true);
        }

        TypeConverter converter = TypeDescriptor.GetConverter(nonNullableTargetType);
        if (converter.CanConvertFrom(value.GetType()))
        {
            return converter.ConvertFrom(null, culture, value);
        }

        if (value is string text && converter.CanConvertFrom(typeof(string)))
        {
            return converter.ConvertFrom(null, culture, text);
        }

        if (value is IConvertible)
        {
            return Convert.ChangeType(value, nonNullableTargetType, culture);
        }

        throw new InvalidOperationException($"Value '{value}' cannot be converted to '{nonNullableTargetType}'.");
    }

    /// <summary>
    /// Gets a count for common collection-like values.
    /// </summary>
    /// <param name="value">The value to inspect.</param>
    /// <returns>The count if one can be determined; otherwise <see langword="null"/>.</returns>
    internal static int? GetCount(object? value)
    {
        return value switch
        {
            null => null,
            ICollection collection => collection.Count,
            IEnumerable enumerable => CountEnumerable(enumerable),
            _ => null
        };
    }

    /// <summary>
    /// Converts a value to a boolean value using common WPF converter rules.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted boolean value.</returns>
    internal static bool ToBoolean(object? value)
    {
        return value switch
        {
            bool booleanValue => booleanValue,
            Visibility visibility => visibility == Visibility.Visible,
            string text => bool.TryParse(text, out bool parsed) && parsed,
            int integer => integer != 0,
            long longValue => longValue != 0,
            double doubleValue => Math.Abs(doubleValue) > double.Epsilon,
            decimal decimalValue => decimalValue != 0m,
            _ => value is not null
        };
    }

    /// <summary>
    /// Parses a <see cref="GridLength"/> value.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="fallback">The fallback value.</param>
    /// <param name="culture">The culture used for parsing.</param>
    /// <returns>The parsed <see cref="GridLength"/> value.</returns>
    internal static GridLength ParseGridLength(object? value, GridLength fallback, CultureInfo culture)
    {
        if (value is GridLength gridLength)
        {
            return gridLength;
        }

        if (value is null)
        {
            return fallback;
        }

        GridLengthConverter converter = new();
        if (converter.CanConvertFrom(value.GetType()))
        {
            return (GridLength)(converter.ConvertFrom(null, culture, value) ?? fallback);
        }

        string? text = value.ToString();
        if (!string.IsNullOrWhiteSpace(text) && converter.CanConvertFrom(typeof(string)))
        {
            return (GridLength)(converter.ConvertFrom(null, culture, text) ?? fallback);
        }

        return fallback;
    }

    /// <summary>
    /// Parses a <see cref="Thickness"/> value.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="fallback">The fallback value.</param>
    /// <param name="culture">The culture used for parsing.</param>
    /// <returns>The parsed <see cref="Thickness"/> value.</returns>
    internal static Thickness ParseThickness(object? value, Thickness fallback, CultureInfo culture)
    {
        if (value is Thickness thickness)
        {
            return thickness;
        }

        if (value is null)
        {
            return fallback;
        }

        ThicknessConverter converter = new();
        if (converter.CanConvertFrom(value.GetType()))
        {
            return (Thickness)(converter.ConvertFrom(null, culture, value) ?? fallback);
        }

        string? text = value.ToString();
        if (!string.IsNullOrWhiteSpace(text) && converter.CanConvertFrom(typeof(string)))
        {
            return (Thickness)(converter.ConvertFrom(null, culture, text) ?? fallback);
        }

        return fallback;
    }

    /// <summary>
    /// Parses a converter parameter pair split by a pipe character.
    /// </summary>
    /// <param name="parameter">The converter parameter.</param>
    /// <returns>The parsed pair.</returns>
    internal static (string? TrueValue, string? FalseValue) ParsePipePair(object? parameter)
    {
        string? parameterText = parameter?.ToString();
        if (string.IsNullOrWhiteSpace(parameterText))
        {
            return (null, null);
        }

        string[] parts = parameterText.Split('|', 2, StringSplitOptions.TrimEntries);
        return parts.Length == 1 ? (parts[0], null) : (parts[0], parts[1]);
    }
    #endregion

    #region ### Private Methods ###
    private static int CountEnumerable(IEnumerable enumerable)
    {
        int count = 0;
        IEnumerator enumerator = enumerable.GetEnumerator();

        while (enumerator.MoveNext())
        {
            count++;
        }

        return count;
    }
    #endregion
}
#endregion
