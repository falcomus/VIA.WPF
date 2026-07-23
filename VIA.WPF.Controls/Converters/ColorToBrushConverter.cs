// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorToBrushConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace VIA.WPF.Converters;

#region ### Class ColorToBrushConverter ###
/// <summary>
/// Converts <see cref="Color"/> values to <see cref="SolidColorBrush"/> values.
/// </summary>
[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
public sealed class ColorToBrushConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static ColorToBrushConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the created brush should be frozen when possible.
    /// </summary>
    public bool FreezeBrush { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a <see cref="Color"/> value to a <see cref="SolidColorBrush"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Color color)
        {
            return Binding.DoNothing;
        }

        SolidColorBrush brush = new(color);
        if (this.FreezeBrush && brush.CanFreeze)
        {
            brush.Freeze();
        }

        return brush;
    }

    /// <summary>
    /// Converts a <see cref="SolidColorBrush"/> value back to a <see cref="Color"/> value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is SolidColorBrush brush ? brush.Color : Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class ColorToBrushExtension ###
/// <summary>
/// Provides the shared <see cref="ColorToBrushConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class ColorToBrushExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ColorToBrushConverter.Instance;
    }
    #endregion
}
#endregion
