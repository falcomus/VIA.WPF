// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class EnumToVisibilityConverter ###
/// <summary>
/// Converts enum values to <see cref="Visibility"/> values by comparing them with the converter parameter.
/// </summary>
[ValueConversion(typeof(Enum), typeof(Visibility))]
public sealed class EnumToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static EnumToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when the enum value equals the converter parameter.
    /// </summary>
    public Visibility EqualVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when the enum value does not equal the converter parameter.
    /// </summary>
    public Visibility NotEqualVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts an enum value to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEqual = XConverterUtility.AreEqual(value, parameter, culture);
        return isEqual ? this.EqualVisibility : this.NotEqualVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to an enum value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Type enumType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (value is not Visibility.Visible || parameter is null || !enumType.IsEnum)
        {
            return Binding.DoNothing;
        }

        try
        {
            return Enum.Parse(enumType, parameter.ToString() ?? string.Empty, true);
        }
        catch (ArgumentException)
        {
            return Binding.DoNothing;
        }
    }
    #endregion
}
#endregion

#region ### Class EnumToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="EnumToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class EnumToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EnumToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
