// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppThemeBindingExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Themes;

#region ### Class AppThemeBindingExtension ###
/// <summary>
/// Provides a value that automatically switches between light and dark theme values.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public class AppThemeBindingExtension : MarkupExtension
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the value used for light theme mode.
    /// </summary>
    public object? Light { get; set; }

    /// <summary>
    /// Gets or sets the value used for dark theme mode.
    /// </summary>
    public object? Dark { get; set; }

    /// <summary>
    /// Gets or sets the fallback value.
    /// </summary>
    public object? Default { get; set; }

    /// <summary>
    /// Gets or sets the optional design-time value.
    /// </summary>
    public object? DesignValue { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        Type targetType = GetTargetType(serviceProvider);

        if (IsInDesignMode())
        {
            object? designValue = this.DesignValue ?? this.Light ?? this.Default ?? this.Dark;

            return designValue is not null
                ? ConvertToTargetType(designValue, targetType, CultureInfo.CurrentCulture)
                : DependencyProperty.UnsetValue;
        }

        Binding binding = new(nameof(XThemeManager.CurrentMode))
        {
            Source = XThemeManager.Current,
            Mode = BindingMode.OneWay,
            Converter = AppThemeBindingConverter.Instance,
            ConverterParameter = new AppThemeBindingOptions(this.Light, this.Dark, this.Default, targetType)
        };

        return binding.ProvideValue(serviceProvider);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Gets the target property type for the markup extension.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The target property type.</returns>
    private static Type GetTargetType(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
        {
            if (provideValueTarget.TargetProperty is DependencyProperty dependencyProperty)
            {
                return dependencyProperty.PropertyType;
            }

            if (provideValueTarget.TargetProperty is PropertyInfo propertyInfo)
            {
                return propertyInfo.PropertyType;
            }
        }

        return typeof(object);
    }

    /// <summary>
    /// Gets a value indicating whether the current process is running in design mode.
    /// </summary>
    /// <returns><c>true</c> if design mode is active; otherwise, <c>false</c>.</returns>
    private static bool IsInDesignMode()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return true;
        }

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return true;
        }

        string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

        return processName.Contains("devenv", StringComparison.OrdinalIgnoreCase) ||
               processName.Contains("Blend", StringComparison.OrdinalIgnoreCase) ||
               processName.Contains("XDesProc", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Converts a value to the requested target type.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="culture">The current culture.</param>
    /// <returns>The converted value.</returns>
    private static object ConvertToTargetType(object? value, Type targetType, CultureInfo culture)
    {
        return AppThemeBindingValueConverter.ConvertToTargetType(value, targetType, culture);
    }
    #endregion
}
#endregion

#region ### Class AppThemeBindingConverter ###
/// <summary>
/// Converts the current theme mode into the configured theme value.
/// </summary>
internal sealed class AppThemeBindingConverter : IValueConverter
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the shared converter instance.
    /// </summary>
    public static AppThemeBindingConverter Instance { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not AppThemeBindingOptions options)
        {
            return Binding.DoNothing;
        }

        XThemeMode mode = value is XThemeMode themeMode
            ? themeMode
            : XThemeMode.Light;

        object? selectedValue = mode == XThemeMode.Dark
            ? options.Dark ?? options.Default ?? options.Light
            : options.Light ?? options.Default ?? options.Dark;

        return AppThemeBindingValueConverter.ConvertToTargetType(selectedValue, options.TargetType, culture);
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    #endregion
}
#endregion

#region ### Class AppThemeBindingValueConverter ###
/// <summary>
/// Provides culture-safe conversion for app theme binding values.
/// </summary>
internal static class AppThemeBindingValueConverter
{
    #region ### Public Methods ###
    /// <summary>
    /// Converts a value to the requested target type.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="culture">The current culture.</param>
    /// <returns>The converted value.</returns>
    public static object ConvertToTargetType(object? value, Type targetType, CultureInfo culture)
    {
        if (value is null)
        {
            return DependencyProperty.UnsetValue;
        }

        Type effectiveTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (effectiveTargetType == typeof(object) || effectiveTargetType.IsInstanceOfType(value))
        {
            return value;
        }

        if (value is string stringValue)
        {
            object? convertedStringValue = ConvertStringToTargetType(stringValue, effectiveTargetType, culture);

            return convertedStringValue ?? DependencyProperty.UnsetValue;
        }

        object? convertedValue = ConvertValueToTargetType(value, effectiveTargetType, culture);

        return convertedValue ?? value;
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Converts a string value to the requested target type.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="culture">The current culture.</param>
    /// <returns>The converted value or <see langword="null"/>.</returns>
    private static object? ConvertStringToTargetType(string value, Type targetType, CultureInfo culture)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(targetType);

        if (converter.CanConvertFrom(typeof(string)))
        {
            object? convertedValue = TryConvertStringWithTypeConverter(converter, value, CultureInfo.InvariantCulture);
            if (convertedValue is not null)
            {
                return convertedValue;
            }

            convertedValue = TryConvertStringWithTypeConverter(converter, value, culture);
            if (convertedValue is not null)
            {
                return convertedValue;
            }
        }

        if (targetType.IsEnum && Enum.TryParse(targetType, value, true, out object? enumValue))
        {
            return enumValue;
        }

        return TryChangeType(value, targetType, CultureInfo.InvariantCulture)
            ?? TryChangeType(value, targetType, culture);
    }

    /// <summary>
    /// Converts a non-string value to the requested target type.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="culture">The current culture.</param>
    /// <returns>The converted value or <see langword="null"/>.</returns>
    private static object? ConvertValueToTargetType(object value, Type targetType, CultureInfo culture)
    {
        return TryChangeType(value, targetType, CultureInfo.InvariantCulture)
            ?? TryChangeType(value, targetType, culture);
    }

    /// <summary>
    /// Tries to convert a string using a type converter.
    /// </summary>
    /// <param name="converter">The type converter.</param>
    /// <param name="value">The source value.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The converted value or <see langword="null"/>.</returns>
    private static object? TryConvertStringWithTypeConverter(TypeConverter converter, string value, CultureInfo culture)
    {
        try
        {
            return converter.ConvertFrom(null, culture, value);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to convert a value using <see cref="System.Convert.ChangeType(object, Type, IFormatProvider)"/>.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The converted value or <see langword="null"/>.</returns>
    private static object? TryChangeType(object value, Type targetType, CultureInfo culture)
    {
        try
        {
            return System.Convert.ChangeType(value, targetType, culture);
        }
        catch
        {
            return null;
        }
    }
    #endregion
}
#endregion

#region ### Class AppThemeBindingOptions ###
/// <summary>
/// Stores app theme binding conversion options.
/// </summary>
internal sealed class AppThemeBindingOptions
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="AppThemeBindingOptions"/> class.
    /// </summary>
    /// <param name="light">The light theme value.</param>
    /// <param name="dark">The dark theme value.</param>
    /// <param name="defaultValue">The fallback value.</param>
    /// <param name="targetType">The target property type.</param>
    public AppThemeBindingOptions(object? light, object? dark, object? defaultValue, Type targetType)
    {
        this.Light = light;
        this.Dark = dark;
        this.Default = defaultValue;
        this.TargetType = targetType;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the light theme value.
    /// </summary>
    public object? Light { get; }

    /// <summary>
    /// Gets the dark theme value.
    /// </summary>
    public object? Dark { get; }

    /// <summary>
    /// Gets the fallback value.
    /// </summary>
    public object? Default { get; }

    /// <summary>
    /// Gets the target property type.
    /// </summary>
    public Type TargetType { get; }
    #endregion
}
#endregion