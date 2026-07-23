// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows.Data;

namespace VIA.WPF.Extensions;

#region ### Class BindingExtensions ###
/// <summary>
/// Provides fluent helper methods for WPF bindings created in code.
/// </summary>
public static class BindingExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Sets the binding mode and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="mode">The binding mode.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithMode(this Binding binding, BindingMode mode)
    {
        ArgumentNullException.ThrowIfNull(binding);

        binding.Mode = mode;

        return binding;
    }

    /// <summary>
    /// Sets the update source trigger and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="updateSourceTrigger">The update source trigger.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithUpdateSourceTrigger(this Binding binding, UpdateSourceTrigger updateSourceTrigger)
    {
        ArgumentNullException.ThrowIfNull(binding);

        binding.UpdateSourceTrigger = updateSourceTrigger;

        return binding;
    }

    /// <summary>
    /// Sets the fallback value and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="fallbackValue">The fallback value.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithFallbackValue(this Binding binding, object? fallbackValue)
    {
        ArgumentNullException.ThrowIfNull(binding);

        binding.FallbackValue = fallbackValue;

        return binding;
    }

    /// <summary>
    /// Sets the target null value and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="targetNullValue">The target null value.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithTargetNullValue(this Binding binding, object? targetNullValue)
    {
        ArgumentNullException.ThrowIfNull(binding);

        binding.TargetNullValue = targetNullValue;

        return binding;
    }

    /// <summary>
    /// Sets the converter and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="converter">The converter.</param>
    /// <param name="parameter">The optional converter parameter.</param>
    /// <param name="culture">The optional converter culture.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithConverter(this Binding binding, IValueConverter converter, object? parameter = null, CultureInfo? culture = null)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(converter);

        binding.Converter = converter;
        binding.ConverterParameter = parameter;
        binding.ConverterCulture = culture;

        return binding;
    }

    /// <summary>
    /// Enables or disables asynchronous binding and returns the same binding instance.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="isAsync">A value indicating whether the binding should be asynchronous.</param>
    /// <returns>The same binding instance.</returns>
    public static Binding WithIsAsync(this Binding binding, bool isAsync = true)
    {
        ArgumentNullException.ThrowIfNull(binding);

        binding.IsAsync = isAsync;

        return binding;
    }
    #endregion
}
#endregion
