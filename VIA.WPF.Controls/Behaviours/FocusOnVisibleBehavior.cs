// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FocusOnVisibleBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Threading;

namespace VIA.WPF.Behaviors;

#region ### Class FocusOnVisibleBehavior ###
/// <summary>
/// Provides an attached behavior that focuses a framework element whenever it becomes visible.
/// </summary>
public static class FocusOnVisibleBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(FocusOnVisibleBehavior),
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
        if (dependencyObject is not FrameworkElement element)
        {
            return;
        }

        element.IsVisibleChanged -= OnIsVisibleChanged;

        if (e.NewValue is true)
        {
            element.IsVisibleChanged += OnIsVisibleChanged;

            if (element.IsVisible)
            {
                FocusLater(element);
            }
        }
    }

    private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is FrameworkElement element && e.NewValue is true)
        {
            FocusLater(element);
        }
    }

    private static void FocusLater(FrameworkElement element)
    {
        element.Dispatcher.BeginInvoke(
            () =>
            {
                if (element.IsVisible && element.IsEnabled && element.Focusable)
                {
                    element.Focus();
                }
            },
            DispatcherPriority.Input);
    }
    #endregion
}
#endregion
