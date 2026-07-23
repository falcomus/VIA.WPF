// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToThicknessConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanToThicknessConverter ###
/// <summary>
/// Converts <see cref="bool"/> values to <see cref="Thickness"/> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(Thickness))]
public sealed class BooleanToThicknessConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanToThicknessConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the thickness returned when the input value is <see langword="true"/>.
    /// </summary>
    public Thickness TrueThickness { get; set; } = new(0);

    /// <summary>
    /// Gets or sets the thickness returned when the input value is <see langword="false"/>.
    /// </summary>
    public Thickness FalseThickness { get; set; } = new(0);
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a <see cref="Thickness"/> value.
    /// </summary>
    /// <remarks>
    /// The optional parameter can override both values using the format <c>trueThickness|falseThickness</c>.
    /// </remarks>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        (string? trueValue, string? falseValue) = XConverterUtility.ParsePipePair(parameter);
        Thickness trueThickness = XConverterUtility.ParseThickness(trueValue, this.TrueThickness, culture);
        Thickness falseThickness = XConverterUtility.ParseThickness(falseValue, this.FalseThickness, culture);

        return XConverterUtility.ToBoolean(value) ? trueThickness : falseThickness;
    }

    /// <summary>
    /// Converts a <see cref="Thickness"/> value back to a boolean value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class BooleanToThicknessExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanToThicknessConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BooleanToThicknessExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanToThicknessConverter.Instance;
    }
    #endregion
}
#endregion
