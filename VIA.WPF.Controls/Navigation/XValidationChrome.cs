// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationChrome.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XValidationChrome ###
/// <summary>
/// Provides validation-related chrome helpers for elements that should not render the default WPF validation adorner.
/// </summary>
public static class XValidationChrome
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the SuppressDefaultErrorAdorner attached dependency property.
    /// </summary>
    public static readonly DependencyProperty SuppressDefaultErrorAdornerProperty = DependencyProperty.RegisterAttached(
        "SuppressDefaultErrorAdorner",
        typeof(bool),
        typeof(XValidationChrome),
        new FrameworkPropertyMetadata(false, OnSuppressDefaultErrorAdornerChanged));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether the default WPF validation error adorner is suppressed for the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the default error adorner is suppressed; otherwise <c>false</c>.</returns>
    public static bool GetSuppressDefaultErrorAdorner(DependencyObject element)
    {
        return (bool)element.GetValue(SuppressDefaultErrorAdornerProperty);
    }

    /// <summary>
    /// Sets whether the default WPF validation error adorner is suppressed for the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetSuppressDefaultErrorAdorner(DependencyObject element, bool value)
    {
        element.SetValue(SuppressDefaultErrorAdornerProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnSuppressDefaultErrorAdornerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is true)
        {
            XValidationAdornerHelper.SuppressDefaultErrorTemplate(dependencyObject);
        }
        else
        {
            XValidationAdornerHelper.ClearErrorTemplate(dependencyObject);
        }
    }
    #endregion
}
#endregion
