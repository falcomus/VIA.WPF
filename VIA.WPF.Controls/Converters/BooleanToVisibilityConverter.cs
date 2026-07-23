// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class BooleanToVisibilityConverter ###
/// <summary>
/// Converts <see cref="bool"/> values to <see cref="Visibility"/> values.
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BooleanToVisibilityConverter : IValueConverter
{
    #region ### Public Static Fields ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static BooleanToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is <see langword="true"/>.
    /// </summary>
    public Visibility TrueVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility that is returned when the input value is <see langword="false"/>.
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? this.TrueVisibility : this.FalseVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a <see cref="bool"/> value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == this.TrueVisibility;
    }
    #endregion
}
#endregion

#region ### Class BooleanToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="BooleanToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class BooleanToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return BooleanToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion