// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToXDataGridColumnDisplayModeConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using VIA.WPF.Controls;

namespace VIA.WPF.Converters;

#region ### Class BooleanToXDataGridColumnDisplayModeConverter ###
/// <summary>
/// Converts <see cref="bool" /> values to <see cref="XDataGridColumnDisplayMode" /> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(XDataGridColumnDisplayMode))]
public sealed class BooleanToXDataGridColumnDisplayModeConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanToXDataGridColumnDisplayModeConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the display mode returned when the input value is <see langword="true" />.
    /// </summary>
    public XDataGridColumnDisplayMode TrueMode { get; set; } = XDataGridColumnDisplayMode.Compact;

    /// <summary>
    /// Gets or sets the display mode returned when the input value is <see langword="false" />.
    /// </summary>
    public XDataGridColumnDisplayMode FalseMode { get; set; } = XDataGridColumnDisplayMode.Full;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a boolean value to a column display mode.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? this.TrueMode : this.FalseMode;
    }

    /// <summary>
    /// Converts a column display mode back to a boolean value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is XDataGridColumnDisplayMode mode && mode == this.TrueMode;
    }
    #endregion
}
#endregion

#region ### Class BooleanToXDataGridColumnDisplayModeExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanToXDataGridColumnDisplayModeConverter" /> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BooleanToXDataGridColumnDisplayModeExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanToXDataGridColumnDisplayModeConverter.Instance;
    }
    #endregion
}
#endregion
