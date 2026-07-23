// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnyTrueToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class AnyTrueToVisibilityConverter ###
/// <summary>
/// Converts multiple boolean-like values to <see cref="Visibility"/> depending on whether at least one value is true.
/// </summary>
public sealed class AnyTrueToVisibilityConverter : IMultiValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static AnyTrueToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when at least one input value is true.
    /// </summary>
    public Visibility TrueVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when no input value is true.
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts multiple values to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        bool anyTrue = values.Where(value => value != DependencyProperty.UnsetValue).Any(XConverterUtility.ToBoolean);
        return anyTrue ? this.TrueVisibility : this.FalseVisibility;
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

#region ### Class AnyTrueToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="AnyTrueToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IMultiValueConverter))]
public sealed class AnyTrueToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return AnyTrueToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
