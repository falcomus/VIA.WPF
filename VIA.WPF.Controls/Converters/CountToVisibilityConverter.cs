// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountToVisibilityConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Converters;

#region ### Class CountToVisibilityConverter ###
/// <summary>
/// Converts numeric count values or collection-like values to <see cref="Visibility"/> values.
/// </summary>
[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class CountToVisibilityConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static CountToVisibilityConverter Instance { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visibility returned when the count is zero.
    /// </summary>
    public Visibility ZeroVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the visibility returned when the count is greater than zero.
    /// </summary>
    public Visibility NonZeroVisibility { get; set; } = Visibility.Visible;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Converts a count value to a <see cref="Visibility"/> value.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int count = GetCount(value, culture);
        return count == 0 ? this.ZeroVisibility : this.NonZeroVisibility;
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to a count value.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion

    #region ### Private Methods ###
    private static int GetCount(object? value, CultureInfo culture)
    {
        int? collectionCount = XConverterUtility.GetCount(value);
        if (collectionCount.HasValue)
        {
            return collectionCount.Value;
        }

        try
        {
            return System.Convert.ToInt32(value, culture);
        }
        catch (FormatException)
        {
            return 0;
        }
        catch (InvalidCastException)
        {
            return 0;
        }
        catch (OverflowException)
        {
            return 0;
        }
    }
    #endregion
}
#endregion

#region ### Class CountToVisibilityExtension ###
/// <summary>
/// Provides the shared <see cref="CountToVisibilityConverter"/> through XAML.
/// </summary>
[MarkupExtensionReturnType(typeof(IValueConverter))]
public sealed class CountToVisibilityExtension : MarkupExtension
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return CountToVisibilityConverter.Instance;
    }
    #endregion
}
#endregion
