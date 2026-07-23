// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectReferenceEqualsConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class ObjectReferenceEqualsConverter ###
/// <summary>
/// Converts values to <see cref="bool"/> values by using reference equality.
/// </summary>
[ValueConversion(typeof(object), typeof(bool))]
public sealed class ObjectReferenceEqualsConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static ObjectReferenceEqualsConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the comparison result should be inverted.
    /// </summary>
    public bool Invert { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a boolean value by comparing the value reference with the converter parameter reference.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEqual = ReferenceEquals(value, parameter);
        return this.Invert ? !isEqual : isEqual;
    }

    /// <summary>
    /// Converts a boolean value back to an object.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class ObjectReferenceEqualsExtension ###
/// <summary>
/// Provides the shared <see cref="ObjectReferenceEqualsConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class ObjectReferenceEqualsExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ObjectReferenceEqualsConverter.Instance;
    }
    #endregion
}
#endregion
