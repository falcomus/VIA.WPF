// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionEmptyToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class CollectionEmptyToVisibilityConverter ###
/// <summary>
/// Converts collection-like values to <see cref="Visibility"/> values depending on whether they are empty.
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class CollectionEmptyToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static CollectionEmptyToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when the input collection is null or empty.
    /// </summary>
    public Visibility EmptyVisibility { get; set; } = Visibility.Visible;

    /// <summary>
    /// Gets or sets the visibility returned when the input collection contains at least one item.
    /// </summary>
    public Visibility NotEmptyVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a collection-like value to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int count = XConverterUtility.GetCount(value) ?? 0;
        return count == 0 ? this.EmptyVisibility : this.NotEmptyVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a collection-like value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion

#region ### Class CollectionEmptyToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="CollectionEmptyToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class CollectionEmptyToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return CollectionEmptyToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
