// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllTrueToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class AllTrueToVisibilityConverter ###
/// <summary>
/// Converts multiple boolean-like values to <see cref="Visibility"/> depending on whether all values are true.
/// </summary>
public sealed class AllTrueToVisibilityConverter : IMultiValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static AllTrueToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when all input values are true.
    /// </summary>
    public Visibility TrueVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when not all input values are true.
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts multiple values to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        object[] filteredValues = values.Where(value => value != DependencyProperty.UnsetValue).ToArray();
        bool allTrue = filteredValues.Length > 0 && filteredValues.All(XConverterUtility.ToBoolean);
        return allTrue ? this.TrueVisibility : this.FalseVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to source values.
    /// </summary>
    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return targetTypes.Select(_ => Binding.DoNothing).ToArray();
    }
    #endregion
}
#endregion

#region ### Class AllTrueToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="AllTrueToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IMultiValueConverter))]
public sealed class AllTrueToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return AllTrueToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
