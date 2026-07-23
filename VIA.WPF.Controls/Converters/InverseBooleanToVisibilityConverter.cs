// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InverseBooleanToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class InverseBooleanToVisibilityConverter ###
/// <summary>
/// Converts <see cref="bool"/> values to inverted <see cref="Visibility"/> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class InverseBooleanToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static InverseBooleanToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is <see langword="true"/>.
    /// </summary>
    public Visibility TrueVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is <see langword="false"/>.
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Visible;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to an inverted <see cref="Visibility"/> value.
    /// </summary>
    /// <param name="value">The binding source value.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// <see cref="TrueVisibility"/> if <paramref name="value"/> represents <see langword="true"/>; otherwise <see cref="FalseVisibility"/>.
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? this.TrueVisibility : this.FalseVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns><see langword="true"/> if the visibility is equal to <see cref="TrueVisibility"/>; otherwise <see langword="false"/>.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == this.TrueVisibility;
    }
    #endregion
}
#endregion

#region ### Class InverseBooleanToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="InverseBooleanToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class InverseBooleanToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return InverseBooleanToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion