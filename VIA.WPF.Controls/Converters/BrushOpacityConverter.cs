// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrushOpacityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace VIA.WPF.Converters;

#region ### Class BrushOpacityConverter ###
/// <summary>
/// Creates a brush copy with a configured opacity.
/// </summary>
[ValueConversion(typeof(Brush), typeof(Brush))]
public sealed class BrushOpacityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BrushOpacityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the opacity applied to the brush. The converter parameter overrides this value when supplied.
    /// </summary>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets a value indicating whether the created brush should be frozen when possible.
    /// </summary>
    public bool FreezeBrush { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a brush to a brush copy with the configured opacity.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Brush sourceBrush)
        {
            return Binding.DoNothing;
        }

        double opacity = parameter is null ? this.Opacity : ToDouble(parameter, culture);
        opacity = Math.Clamp(opacity, 0.0, 1.0);

        Brush brush = sourceBrush.CloneCurrentValue();
        brush.Opacity = opacity;

        if (this.FreezeBrush && brush.CanFreeze)
        {
            brush.Freeze();
        }

        return brush;
    }

    /// <summary>
    /// Converts a brush copy back to the source brush.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion

    #region ### Private Methods ###
    private static double ToDouble(object? value, CultureInfo culture)
    {
        try
        {
            return System.Convert.ToDouble(value, culture);
        }
        catch (FormatException)
        {
            return 1.0;
        }
        catch (InvalidCastException)
        {
            return 1.0;
        }
        catch (OverflowException)
        {
            return 1.0;
        }
    }
    #endregion
}
#endregion

#region ### Class BrushOpacityExtension ###
/// <summary>
/// Provides the shared <see cref="BrushOpacityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BrushOpacityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BrushOpacityConverter.Instance;
    }
    #endregion
}
#endregion
