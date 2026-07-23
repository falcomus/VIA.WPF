// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class EqualityToVisibilityConverter ###
/// <summary>
/// Converts values to <see cref="Visibility"/> values by comparing them with the converter parameter.
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class EqualityToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static EqualityToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when the values are equal.
    /// </summary>
    public Visibility EqualVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when the values are not equal.
    /// </summary>
    public Visibility NotEqualVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets a value indicating whether the comparison result should be inverted.
    /// </summary>
    public bool Invert { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a <see cref="Visibility"/> value by comparing it with the converter parameter.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEqual = XConverterUtility.AreEqual(value, parameter, culture);
        if (this.Invert)
        {
            isEqual = !isEqual;
        }

        return isEqual ? this.EqualVisibility : this.NotEqualVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to an object.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class EqualityToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="EqualityToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class EqualityToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EqualityToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
