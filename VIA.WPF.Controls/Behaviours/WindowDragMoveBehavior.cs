// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowDragMoveBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class WindowDragMoveBehavior ###
/// <summary>
/// Provides an attached behavior that allows a framework element to drag its containing window.
/// </summary>
public static class WindowDragMoveBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(WindowDragMoveBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether the behavior is enabled for the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the behavior is enabled; otherwise <c>false</c>.</returns>
    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// Sets whether the behavior is enabled for the specified element.
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
        if (dependencyObject is not UIElement element)
        {
            return;
        }

        element.MouseLeftButtonDown -= OnMouseLeftButtonDown;

        if (e.NewValue is true)
        {
            element.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not DependencyObject dependencyObject || e.ButtonState != MouseButtonState.Pressed || e.ClickCount != 1)
        {
            return;
        }

        Window? window = Window.GetWindow(dependencyObject);

        if (window is null)
        {
            return;
        }

        try
        {
            window.DragMove();
            e.Handled = true;
        }
        catch (InvalidOperationException)
        {
            // DragMove can throw when the mouse button state changed while the event was processed.
        }
    }
    #endregion
}
#endregion
