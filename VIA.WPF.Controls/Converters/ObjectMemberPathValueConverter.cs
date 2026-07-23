// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectMemberPathValueConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace VIA.WPF.Converters;

#region ### Class ObjectMemberPathValueConverter ###
/// <summary>
/// Resolves a public member path on an object and returns the resolved value.
/// </summary>
public sealed class ObjectMemberPathValueConverter : IMultiValueConverter
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        object? source = values.Length > 0 ? values[0] : null;
        string? memberPath = values.Length > 1 ? values[1] as string : null;
        bool fallbackToString = string.Equals(parameter as string, "FallbackToString", StringComparison.OrdinalIgnoreCase);

        if (source is null)
        {
            return null;
        }

        object? resolvedValue = string.IsNullOrWhiteSpace(memberPath)
            ? source
            : ResolveMemberPath(source, memberPath);

        if (resolvedValue is null && fallbackToString)
        {
            return source.ToString();
        }

        return resolvedValue;
    }

    /// <inheritdoc />
    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return targetTypes.Select(_ => Binding.DoNothing).ToArray();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Resolves the specified member path.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="memberPath">The public member path.</param>
    /// <returns>The resolved value if available; otherwise <see langword="null"/>.</returns>
    private static object? ResolveMemberPath(object source, string memberPath)
    {
        object? current = source;

        foreach (string memberName in memberPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (current is null)
            {
                return null;
            }

            if (current is IDictionary dictionary && dictionary.Contains(memberName))
            {
                current = dictionary[memberName];
                continue;
            }

            Type currentType = current.GetType();
            PropertyInfo? property = currentType.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public);

            if (property is not null)
            {
                current = property.GetValue(current);
                continue;
            }

            FieldInfo? field = currentType.GetField(memberName, BindingFlags.Instance | BindingFlags.Public);

            if (field is not null)
            {
                current = field.GetValue(current);
                continue;
            }

            return null;
        }

        return current;
    }
    #endregion
}
#endregion
