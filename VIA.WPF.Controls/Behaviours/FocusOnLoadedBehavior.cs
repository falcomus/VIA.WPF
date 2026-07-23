// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FocusOnLoadedBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Threading;

namespace VIA.WPF.Behaviors;

#region ### Class FocusOnLoadedBehavior ###
/// <summary>
/// Provides an attached behavior that focuses a framework element after it has been loaded.
/// </summary>
public static class FocusOnLoadedBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(FocusOnLoadedBehavior),
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

        element.Loaded -= OnLoaded;

        if (e.NewValue is true)
        {
            if (element.IsLoaded)
            {
                FocusLater(element);
            }
            else
            {
                element.Loaded += OnLoaded;
            }
        }
    }

    private static void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            element.Loaded -= OnLoaded;
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
