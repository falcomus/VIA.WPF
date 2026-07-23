// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FirstValidationErrorConverter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace VIA.WPF.Converters;

#region ### Class FirstValidationErrorConverter ###
/// <summary>
/// Safely extracts the error content of the first validation error from a
/// <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection{T}"/> of <see cref="ValidationError"/>.
/// Returns an empty string when the collection is null or empty, preventing the
/// <see cref="System.ArgumentOutOfRangeException"/> that occurs when binding directly
/// to <c>(Validation.Errors)[0].ErrorContent</c> on controls without errors.
/// </summary>
/// <remarks>
/// Use this converter instead of the direct index binding pattern:
/// <code>
/// <!-- Instead of this (crashes when Validation.Errors is empty): -->
/// Text="{Binding (Validation.Errors)[0].ErrorContent, RelativeSource=...}"
///
/// <!-- Use this (safe): -->
/// Text="{Binding (Validation.Errors), RelativeSource=..., Converter={StaticResource FirstValidationErrorConverter}}"
/// </code>
/// </remarks>
[ValueConversion(typeof(IEnumerable), typeof(string))]
public sealed class FirstValidationErrorConverter : IValueConverter
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static FirstValidationErrorConverter Instance { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            foreach (object? item in enumerable)
            {
                if (item is ValidationError error)
                {
                    return error.ErrorContent?.ToString() ?? string.Empty;
                }
            }
        }

        return string.Empty;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
    #endregion
}
#endregion
