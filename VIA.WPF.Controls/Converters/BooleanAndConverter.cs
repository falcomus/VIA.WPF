// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanAndConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanAndConverter ###
/// <summary>
/// Converts multiple boolean-like values to <see langword="true"/> when all values are true.
/// </summary>
public sealed class BooleanAndConverter : IMultiValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanAndConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether unset binding values should be ignored.
    /// </summary>
    public bool IgnoreUnsetValues { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts multiple values to a boolean AND result.
    /// </summary>
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        object?[] filteredValues = this.IgnoreUnsetValues
            ? values.Where(value => value != DependencyProperty.UnsetValue).ToArray()
            : values;

        return filteredValues.Length > 0 && filteredValues.All(XConverterUtility.ToBoolean);
    }

    /// <summary>
    /// Converts a boolean AND result back to source values.
    /// </summary>
    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return targetTypes.Select(_ => Binding.DoNothing).ToArray();
    }
    #endregion
}
#endregion

#region ### Class BooleanAndExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanAndConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IMultiValueConverter))]
public sealed class BooleanAndExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanAndConverter.Instance;
    }
    #endregion
}
#endregion
