// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InverseBooleanConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class InverseBooleanConverter ###
/// <summary>
/// Inverts <see cref="bool"/> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public sealed class InverseBooleanConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static InverseBooleanConverter Instance { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value by inverting its boolean representation.
    /// </summary>
    /// <param name="value">The binding source value.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns><see langword="false"/> if <paramref name="value"/> is <see langword="true"/>; otherwise <see langword="true"/>.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not true;
    }

    /// <summary>
    /// Converts a value back by inverting its boolean representation.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns><see langword="false"/> if <paramref name="value"/> is <see langword="true"/>; otherwise <see langword="true"/>.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not true;
    }
    #endregion
}
#endregion

#region ### Class InverseBooleanExtension ###
/// <summary>
/// Provides the shared <see cref="InverseBooleanConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class InverseBooleanExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return InverseBooleanConverter.Instance;
    }
    #endregion
}
#endregion