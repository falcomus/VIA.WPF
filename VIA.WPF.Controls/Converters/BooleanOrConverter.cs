// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanOrConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanOrConverter ###
/// <summary>
/// Converts multiple boolean-like values to <see langword="true"/> when at least one value is true.
/// </summary>
public sealed class BooleanOrConverter : IMultiValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanOrConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether unset binding values should be ignored.
    /// </summary>
    public bool IgnoreUnsetValues { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts multiple values to a boolean OR result.
    /// </summary>
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        object?[] filteredValues = this.IgnoreUnsetValues
            ? values.Where(value => value != DependencyProperty.UnsetValue).ToArray()
            : values;

        return filteredValues.Any(XConverterUtility.ToBoolean);
    }

    /// <summary>
    /// Converts a boolean OR result back to source values.
    /// </summary>
    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return targetTypes.Select(_ => Binding.DoNothing).ToArray();
    }
    #endregion
}
#endregion

#region ### Class BooleanOrExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanOrConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IMultiValueConverter))]
public sealed class BooleanOrExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanOrConverter.Instance;
    }
    #endregion
}
#endregion
