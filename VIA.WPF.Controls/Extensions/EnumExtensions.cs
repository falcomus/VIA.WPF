// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VIA.WPF.Extensions;

#region ### Class EnumExtensions ###
/// <summary>
/// Provides convenience methods for enum values.
/// </summary>
public static class EnumExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the display name from <see cref="DisplayAttribute"/> or <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name.</returns>
    public static string GetDisplayName(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        DisplayAttribute? displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

        if (!string.IsNullOrWhiteSpace(displayAttribute?.GetName()))
        {
            return displayAttribute.GetName()!;
        }

        DescriptionAttribute? descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

        return !string.IsNullOrWhiteSpace(descriptionAttribute?.Description)
            ? descriptionAttribute.Description
            : value.ToString();
    }

    /// <summary>
    /// Gets the description from <see cref="DescriptionAttribute"/> or <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description.</returns>
    public static string GetDescription(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        DescriptionAttribute? descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();

        if (!string.IsNullOrWhiteSpace(descriptionAttribute?.Description))
        {
            return descriptionAttribute.Description;
        }

        DisplayAttribute? displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

        return !string.IsNullOrWhiteSpace(displayAttribute?.Description)
            ? displayAttribute.Description!
            : value.GetDisplayName();
    }

    /// <summary>
    /// Parses the specified text as an enum value and returns a fallback value when parsing fails.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The text value.</param>
    /// <param name="fallback">The fallback value.</param>
    /// <param name="ignoreCase">A value indicating whether the parse should ignore case.</param>
    /// <returns>The parsed enum value or the fallback value.</returns>
    public static TEnum ToEnumOrDefault<TEnum>(this string? value, TEnum fallback, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase, out TEnum result)
            ? result
            : fallback;
    }

    /// <summary>
    /// Gets all values of the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>The enum values.</returns>
    public static IReadOnlyList<TEnum> GetValues<TEnum>()
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>();
    }
    #endregion
}
#endregion
