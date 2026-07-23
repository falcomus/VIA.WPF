// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectAllTextBoxBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class SelectAllTextBoxBehavior ###
/// <summary>
/// Provides an attached behavior that selects all text when a text box receives keyboard focus.
/// </summary>
public static class SelectAllTextBoxBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(SelectAllTextBoxBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether the behavior is enabled for the specified text box.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the behavior is enabled; otherwise <c>false</c>.</returns>
    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// Sets whether the behavior is enabled for the specified text box.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnIsEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not TextBox textBox)
        {
            return;
        }

        textBox.GotKeyboardFocus -= OnGotKeyboardFocus;
        textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;

        if (e.NewValue is true)
        {
            textBox.GotKeyboardFocus += OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
    }

    private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.SelectAll();
        }
    }

    private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is TextBox textBox && !textBox.IsKeyboardFocusWithin)
        {
            e.Handled = true;
            textBox.Focus();
        }
    }
    #endregion
}
#endregion
