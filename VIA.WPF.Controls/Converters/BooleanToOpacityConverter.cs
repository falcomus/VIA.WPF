// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToOpacityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanToOpacityConverter ###
/// <summary>
/// Converts <see cref="bool"/> values to opacity values.
/// </summary>
[ValueConversion(typeof(bool), typeof(double))]
public sealed class BooleanToOpacityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanToOpacityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the opacity returned when the input value is <see langword="true"/>.
    /// </summary>
    public double TrueOpacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the opacity returned when the input value is <see langword="false"/>.
    /// </summary>
    public double FalseOpacity { get; set; } = 0.45;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to an opacity value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return XConverterUtility.ToBoolean(value) ? this.TrueOpacity : this.FalseOpacity;
    }

    /// <summary>
    /// Converts an opacity value back to a boolean value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is double opacity && opacity >= this.TrueOpacity;
    }
    #endregion
}
#endregion

#region ### Class BooleanToOpacityExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanToOpacityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BooleanToOpacityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanToOpacityConverter.Instance;
    }
    #endregion
}
#endregion
