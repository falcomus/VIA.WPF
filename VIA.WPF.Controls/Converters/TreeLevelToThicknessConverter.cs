// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeLevelToThicknessConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class TreeLevelToThicknessConverter ###
/// <summary>
/// Converts tree level values to indentation <see cref="Thickness"/> values.
/// </summary>
[ValueConversion(typeof(object), typeof(Thickness))]
public sealed class TreeLevelToThicknessConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static TreeLevelToThicknessConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the indentation size per tree level. The converter parameter overrides this value when supplied.
    /// </summary>
    public double IndentSize { get; set; } = 18.0;

    /// <summary>
    /// Gets or sets the top margin value.
    /// </summary>
    public double Top { get; set; }

    /// <summary>
    /// Gets or sets the right margin value.
    /// </summary>
    public double Right { get; set; }

    /// <summary>
    /// Gets or sets the bottom margin value.
    /// </summary>
    public double Bottom { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a tree level to a left indentation <see cref="Thickness"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int level = ToInt32(value, culture);
        double indentSize = parameter is null ? this.IndentSize : ToDouble(parameter, culture);

        return new Thickness(Math.Max(0, level) * indentSize, this.Top, this.Right, this.Bottom);
    }

    /// <summary>
    /// Converts a <see cref="Thickness"/> value back to a tree level.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion

    #region ### Private Methods ###
    private static int ToInt32(object? value, CultureInfo culture)
    {
        try
        {
            return System.Convert.ToInt32(value, culture);
        }
        catch (FormatException)
        {
            return 0;
        }
        catch (InvalidCastException)
        {
            return 0;
        }
        catch (OverflowException)
        {
            return 0;
        }
    }

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

#region ### Class TreeLevelToThicknessExtension ###
/// <summary>
/// Provides the shared <see cref="TreeLevelToThicknessConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class TreeLevelToThicknessExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return TreeLevelToThicknessConverter.Instance;
    }
    #endregion
}
#endregion
