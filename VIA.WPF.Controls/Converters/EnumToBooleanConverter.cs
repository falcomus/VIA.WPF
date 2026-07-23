// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumToBooleanConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class EnumToBooleanConverter ###
/// <summary>
/// Converts enum values to <see cref="bool"/> values and back.
/// </summary>
[ValueConversion(typeof(Enum), typeof(bool))]
public sealed class EnumToBooleanConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static EnumToBooleanConverter Instance { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts an enum value to a <see cref="bool"/> value by comparing it with the converter parameter.
    /// </summary>
    /// <param name="value">The binding source value.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The enum value to compare with.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> equals <paramref name="parameter"/>; otherwise <see langword="false"/>.
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
        {
            return false;
        }

        string parameterText = parameter.ToString() ?? string.Empty;
        Type enumType = value.GetType();

        if (!enumType.IsEnum)
        {
            return false;
        }

        object parsedValue = Enum.Parse(enumType, parameterText);
        return value.Equals(parsedValue);
    }

    /// <summary>
    /// Converts a <see cref="bool"/> value back to an enum value.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The enum value to return when the boolean is <see langword="true"/>.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// The enum value represented by <paramref name="parameter"/> when <paramref name="value"/> is <see langword="true"/>; otherwise <see cref="Binding.DoNothing"/>.
    /// </returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not true || parameter is null || !targetType.IsEnum)
        {
            return Binding.DoNothing;
        }

        string parameterText = parameter.ToString() ?? string.Empty;
        return Enum.Parse(targetType, parameterText);
    }
    #endregion
}
#endregion

#region ### Class EnumToBooleanExtension ###
/// <summary>
/// Provides the shared <see cref="EnumToBooleanConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class EnumToBooleanExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EnumToBooleanConverter.Instance;
    }
    #endregion
}
#endregion