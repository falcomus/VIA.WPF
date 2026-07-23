// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityToBooleanConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class EqualityToBooleanConverter ###
/// <summary>
/// Converts values to <see cref="bool"/> values by comparing them with the converter parameter.
/// </summary>
[ValueConversion(typeof(object), typeof(bool))]
public sealed class EqualityToBooleanConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static EqualityToBooleanConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the comparison result should be inverted.
    /// </summary>
    public bool Invert { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a boolean value by comparing it with the converter parameter.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEqual = XConverterUtility.AreEqual(value, parameter, culture);
        return this.Invert ? !isEqual : isEqual;
    }

    /// <summary>
    /// Converts a boolean value back to the converter parameter if possible.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool shouldSet = value is true;
        if (this.Invert)
        {
            shouldSet = !shouldSet;
        }

        if (!shouldSet)
        {
            return Binding.DoNothing;
        }

        try
        {
            return XConverterUtility.ConvertParameter(parameter, targetType, culture) ?? Binding.DoNothing;
        }
        catch (InvalidOperationException)
        {
            return Binding.DoNothing;
        }
    }
    #endregion
}
#endregion

#region ### Class EqualityToBooleanExtension ###
/// <summary>
/// Provides the shared <see cref="EqualityToBooleanConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class EqualityToBooleanExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EqualityToBooleanConverter.Instance;
    }
    #endregion
}
#endregion
