// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringNullOrEmptyToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class StringNullOrEmptyToVisibilityConverter ###
/// <summary>
/// Converts <see cref="string"/> values to <see cref="Visibility"/> values depending on whether the string is null or empty.
/// </summary>
[ValueConversion(typeof(string), typeof(Visibility))]
public sealed class StringNullOrEmptyToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static StringNullOrEmptyToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility that is returned when the input string is null or empty.
    /// </summary>
    public Visibility NullOrEmptyVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the visibility that is returned when the input string is not null or empty.
    /// </summary>
    public Visibility NotNullOrEmptyVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets a value indicating whether white-space-only strings are treated as empty.
    /// </summary>
    public bool TreatWhiteSpaceAsEmpty { get; set; } = true;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a string value to a <see cref="Visibility"/> value.
    /// </summary>
    /// <param name="value">The binding source value.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// <see cref="NullOrEmptyVisibility"/> if the input string is null or empty; otherwise <see cref="NotNullOrEmptyVisibility"/>.
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string? text = value as string;
        bool isEmpty = this.TreatWhiteSpaceAsEmpty
            ? string.IsNullOrWhiteSpace(text)
            : string.IsNullOrEmpty(text);

        return isEmpty ? this.NullOrEmptyVisibility : this.NotNullOrEmptyVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to an object.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns><see cref="Binding.DoNothing"/> because reverse conversion is not supported.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class StringNullOrEmptyToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="StringNullOrEmptyToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class StringNullOrEmptyToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return StringNullOrEmptyToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion