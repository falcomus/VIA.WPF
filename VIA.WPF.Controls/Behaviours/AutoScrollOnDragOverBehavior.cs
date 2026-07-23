// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoScrollOnDragOverBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace VIA.WPF.Behaviors;

#region ### Class AutoScrollOnDragOverBehavior ###
/// <summary>
/// Provides an attached behavior that scrolls a contained <see cref="ScrollViewer" /> while dragging near its edges.
/// </summary>
public static class AutoScrollOnDragOverBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));

    /// <summary>
    /// Identifies the EdgeThreshold attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EdgeThresholdProperty = DependencyProperty.RegisterAttached(
        "EdgeThreshold",
        typeof(double),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(32d));

    /// <summary>
    /// Identifies the ScrollStep attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ScrollStepProperty = DependencyProperty.RegisterAttached(
        "ScrollStep",
        typeof(double),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(18d));
    #endregion

    #region ### Private Fields ###
    private static readonly DependencyProperty TimerProperty = DependencyProperty.RegisterAttached(
        "Timer",
        typeof(DispatcherTimer),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(null));

    private static readonly DependencyProperty CurrentScrollViewerProperty = DependencyProperty.RegisterAttached(
        "CurrentScrollViewer",
        typeof(ScrollViewer),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(null));

    private static readonly DependencyProperty LastDragPointProperty = DependencyProperty.RegisterAttached(
        "LastDragPoint",
        typeof(Point),
        typeof(AutoScrollOnDragOverBehavior),
        new PropertyMetadata(default(Point)));
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

    /// <summary>
    /// Gets the distance from the edge where automatic scrolling starts.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The edge threshold.</returns>
    public static double GetEdgeThreshold(DependencyObject element)
    {
        return (double)element.GetValue(EdgeThresholdProperty);
    }

    /// <summary>
    /// Sets the distance from the edge where automatic scrolling starts.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The edge threshold.</param>
    public static void SetEdgeThreshold(DependencyObject element, double value)
    {
        element.SetValue(EdgeThresholdProperty, value);
    }

    /// <summary>
    /// Gets the amount by which the scroll viewer scrolls per timer tick.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The scroll step.</returns>
    public static double GetScrollStep(DependencyObject element)
    {
        return (double)element.GetValue(ScrollStepProperty);
    }

    /// <summary>
    /// Sets the amount by which the scroll viewer scrolls per timer tick.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The scroll step.</param>
    public static void SetScrollStep(DependencyObject element, double value)
    {
        element.SetValue(ScrollStepProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnIsEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not FrameworkElement element)
        {
            return;
        }

        element.DragOver -= OnDragOver;
        element.DragLeave -= OnDragLeave;
        element.Drop -= OnDrop;
        StopTimer(element);

        if (e.NewValue is true)
        {
            element.AllowDrop = true;
            element.DragOver += OnDragOver;
            element.DragLeave += OnDragLeave;
            element.Drop += OnDrop;
        }
    }

    private static void OnDragOver(object sender, DragEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        ScrollViewer? scrollViewer = element as ScrollViewer ?? FindVisualChild<ScrollViewer>(element);

        if (scrollViewer is null)
        {
            return;
        }

        element.SetValue(CurrentScrollViewerProperty, scrollViewer);
        element.SetValue(LastDragPointProperty, e.GetPosition(scrollViewer));
        EnsureTimer(element);
    }

    private static void OnDragLeave(object sender, DragEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            StopTimer(element);
        }
    }

    private static void OnDrop(object sender, DragEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            StopTimer(element);
        }
    }

    private static void EnsureTimer(FrameworkElement element)
    {
        DispatcherTimer? timer = (DispatcherTimer?)element.GetValue(TimerProperty);

        if (timer is null)
        {
            timer = new DispatcherTimer(DispatcherPriority.Input, element.Dispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };

            timer.Tick += (_, _) => ScrollIfNeeded(element);
            element.SetValue(TimerProperty, timer);
        }

        if (!timer.IsEnabled)
        {
            timer.Start();
        }
    }

    private static void StopTimer(FrameworkElement element)
    {
        DispatcherTimer? timer = (DispatcherTimer?)element.GetValue(TimerProperty);
        timer?.Stop();
        element.ClearValue(CurrentScrollViewerProperty);
    }

    private static void ScrollIfNeeded(FrameworkElement element)
    {
        ScrollViewer? scrollViewer = (ScrollViewer?)element.GetValue(CurrentScrollViewerProperty);

        if (scrollViewer is null)
        {
            StopTimer(element);
            return;
        }

        Point point = (Point)element.GetValue(LastDragPointProperty);
        double threshold = Math.Max(4d, GetEdgeThreshold(element));
        double step = Math.Max(1d, GetScrollStep(element));

        if (point.Y < threshold)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - step);
        }
        else if (point.Y > scrollViewer.ViewportHeight - threshold)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + step);
        }

        if (point.X < threshold)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - step);
        }
        else if (point.X > scrollViewer.ViewportWidth - threshold)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + step);
        }
    }

    private static T? FindVisualChild<T>(DependencyObject parent)
        where T : DependencyObject
    {
        int count = VisualTreeHelper.GetChildrenCount(parent);

        for (int index = 0; index < count; index++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, index);

            if (child is T typedChild)
            {
                return typedChild;
            }

            T? result = FindVisualChild<T>(child);

            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }
    #endregion
}
#endregion
