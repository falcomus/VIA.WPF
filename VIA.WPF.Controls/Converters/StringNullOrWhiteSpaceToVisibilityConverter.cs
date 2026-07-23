// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringNullOrWhiteSpaceToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class StringNullOrWhiteSpaceToVisibilityConverter ###
/// <summary>
/// Converts strings to <see cref="Visibility"/> values depending on whether they are null, empty or white-space-only.
/// </summary>
[ValueConversion(typeof(string), typeof(Visibility))]
public sealed class StringNullOrWhiteSpaceToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static StringNullOrWhiteSpaceToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when the input string is null, empty or white-space-only.
    /// </summary>
    public Visibility NullOrWhiteSpaceVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the visibility returned when the input string contains visible text.
    /// </summary>
    public Visibility NotNullOrWhiteSpaceVisibility { get; set; } = Visibility.Visible;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a string to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value as string)
            ? this.NullOrWhiteSpaceVisibility
            : this.NotNullOrWhiteSpaceVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a string.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class StringNullOrWhiteSpaceToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="StringNullOrWhiteSpaceToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class StringNullOrWhiteSpaceToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return StringNullOrWhiteSpaceToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
