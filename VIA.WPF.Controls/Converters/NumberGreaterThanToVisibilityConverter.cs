// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberGreaterThanToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class NumberGreaterThanToVisibilityConverter ###
/// <summary>
/// Converts numeric values to <see cref="Visibility"/> values depending on whether they are greater than a threshold.
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class NumberGreaterThanToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static NumberGreaterThanToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the threshold value. The converter parameter overrides this value when supplied.
    /// </summary>
    public double Threshold { get; set; }

    /// <summary>
    /// Gets or sets the visibility returned when the input value is greater than the threshold.
    /// </summary>
    public Visibility GreaterVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when the input value is less than or equal to the threshold.
    /// </summary>
    public Visibility NotGreaterVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a numeric value to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double number = ToDouble(value, culture);
        double threshold = parameter is null ? this.Threshold : ToDouble(parameter, culture);

        return number > threshold ? this.GreaterVisibility : this.NotGreaterVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a number.
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

#region ### Class NumberGreaterThanToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="NumberGreaterThanToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class NumberGreaterThanToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return NumberGreaterThanToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
