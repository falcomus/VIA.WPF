// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotNullToBooleanConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class NotNullToBooleanConverter ###
/// <summary>
/// Converts non-null values to <see cref="bool"/> values.
/// </summary>
[ValueConversion(typeof(object), typeof(bool))]
public sealed class NotNullToBooleanConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static NotNullToBooleanConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the value returned when the input value is <see langword="null"/>.
    /// </summary>
    public bool NullValue { get; set; }

    /// <summary>
    /// Gets or sets the value returned when the input value is not <see langword="null"/>.
    /// </summary>
    public bool NotNullValue { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a boolean value depending on whether it is not <see langword="null"/>.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is null ? this.NullValue : this.NotNullValue;
    }

    /// <summary>
    /// Converts a boolean value back to an object.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class NotNullToBooleanExtension ###
/// <summary>
/// Provides the shared <see cref="NotNullToBooleanConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class NotNullToBooleanExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return NotNullToBooleanConverter.Instance;
    }
    #endregion
}
#endregion
