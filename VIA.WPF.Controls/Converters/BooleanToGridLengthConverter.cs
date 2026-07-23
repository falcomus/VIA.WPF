// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToGridLengthConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanToGridLengthConverter ###
/// <summary>
/// Converts <see cref="bool"/> values to <see cref="GridLength"/> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(GridLength))]
public sealed class BooleanToGridLengthConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanToGridLengthConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the grid length returned when the input value is <see langword="true"/>.
    /// </summary>
    public GridLength TrueLength { get; set; } = new(1, GridUnitType.Star);

    /// <summary>
    /// Gets or sets the grid length returned when the input value is <see langword="false"/>.
    /// </summary>
    public GridLength FalseLength { get; set; } = new(0);
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a <see cref="GridLength"/> value.
    /// </summary>
    /// <remarks>
    /// The optional parameter can override both values using the format <c>trueLength|falseLength</c>, for example <c>280|0</c>.
    /// </remarks>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        (string? trueValue, string? falseValue) = XConverterUtility.ParsePipePair(parameter);
        GridLength trueLength = XConverterUtility.ParseGridLength(trueValue, this.TrueLength, culture);
        GridLength falseLength = XConverterUtility.ParseGridLength(falseValue, this.FalseLength, culture);

        return XConverterUtility.ToBoolean(value) ? trueLength : falseLength;
    }

    /// <summary>
    /// Converts a <see cref="GridLength"/> value back to a boolean value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is GridLength gridLength && gridLength.Value > 0;
    }
    #endregion
}
#endregion

#region ### Class BooleanToGridLengthExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanToGridLengthConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BooleanToGridLengthExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanToGridLengthConverter.Instance;
    }
    #endregion
}
#endregion
