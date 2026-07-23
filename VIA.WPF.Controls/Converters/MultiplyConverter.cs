// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplyConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class MultiplyConverter ###
/// <summary>
/// Multiplies numeric values by a factor.
/// </summary>
[ValueConversion(typeof(object), typeof(double))]
public sealed class MultiplyConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static MultiplyConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the multiplication factor. The converter parameter overrides this value when supplied.
    /// </summary>
    public double Factor { get; set; } = 1.0;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a numeric value by multiplying it with the configured factor.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double number = ToDouble(value, culture);
        double factor = parameter is null ? this.Factor : ToDouble(parameter, culture);

        return number * factor;
    }

    /// <summary>
    /// Converts a multiplied value back by dividing it by the configured factor.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double factor = parameter is null ? this.Factor : ToDouble(parameter, culture);
        if (Math.Abs(factor) < double.Epsilon)
        {
            return Binding.DoNothing;
        }

        return ToDouble(value, culture) / factor;
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
            return 0.0;
        }
        catch (InvalidCastException)
        {
            return 0.0;
        }
        catch (OverflowException)
        {
            return 0.0;
        }
    }
    #endregion
}
#endregion

#region ### Class MultiplyExtension ###
/// <summary>
/// Provides the shared <see cref="MultiplyConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class MultiplyExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return MultiplyConverter.Instance;
    }
    #endregion
}
#endregion
