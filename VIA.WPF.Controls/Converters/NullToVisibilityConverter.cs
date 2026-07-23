// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class NullToVisibilityConverter ###
/// <summary>
/// Converts <see langword="null"/> values to <see cref="Visibility"/> values.
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class NullToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static NullToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is <see langword="null"/>.
    /// </summary>
    public Visibility NullVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is not <see langword="null"/>.
    /// </summary>
    public Visibility NotNullVisibility { get; set; } = Visibility.Visible;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a <see cref="Visibility"/> value.
    /// </summary>
    /// <param name="value">The binding source value.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// <see cref="NullVisibility"/> if <paramref name="value"/> is <see langword="null"/>; otherwise <see cref="NotNullVisibility"/>.
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is null ? this.NullVisibility : this.NotNullVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to an object.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns><see cref="Binding.DoNothing"/> because reverse conversion is not supported.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class NullToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="NullToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class NullToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return NullToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion