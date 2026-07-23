// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace VIA.WPF.Extensions;

#region ### Class FrameworkElementExtensions ###
/// <summary>
/// Provides convenience methods for WPF framework elements.
/// </summary>
public static class FrameworkElementExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Finds a template part after applying the control template.
    /// </summary>
    /// <typeparam name="T">The expected part type.</typeparam>
    /// <param name="control">The control.</param>
    /// <param name="partName">The template part name.</param>
    /// <returns>The template part, or <c>null</c>.</returns>
    public static T? FindTemplatePart<T>(this Control control, string partName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentException.ThrowIfNullOrWhiteSpace(partName);

        control.ApplyTemplate();

        return control.Template?.FindName(partName, control) as T;
    }

    /// <summary>
    /// Tries to find a resource and cast it to the specified type.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="element">The framework element.</param>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="resource">The found resource.</param>
    /// <returns><c>true</c> if the resource was found and has the specified type; otherwise, <c>false</c>.</returns>
    public static bool TryFindResource<T>(this FrameworkElement element, object resourceKey, out T? resource)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(resourceKey);

        object? value = element.TryFindResource(resourceKey);

        if (value is T typedValue)
        {
            resource = typedValue;
            return true;
        }

        resource = default;
        return false;
    }

    /// <summary>
    /// Runs the specified action immediately if the element is loaded, or once after it has loaded.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <param name="action">The action to execute.</param>
    public static void RunWhenLoaded(this FrameworkElement element, Action action)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(action);

        if (element.IsLoaded)
        {
            action();
            return;
        }

        RoutedEventHandler? loadedHandler = null;
        loadedHandler = (_, _) =>
        {
            element.Loaded -= loadedHandler;
            action();
        };

        element.Loaded += loadedHandler;
    }

    /// <summary>
    /// Returns a task that completes when the element is loaded.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <returns>The task representing the loaded state.</returns>
    public static Task WhenLoadedAsync(this FrameworkElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (element.IsLoaded)
        {
            return Task.CompletedTask;
        }

        TaskCompletionSource<object?> completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        RoutedEventHandler? loadedHandler = null;
        loadedHandler = (_, _) =>
        {
            element.Loaded -= loadedHandler;
            completionSource.TrySetResult(null);
        };

        element.Loaded += loadedHandler;

        return completionSource.Task;
    }

    /// <summary>
    /// Focuses the element later on the dispatcher queue.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <param name="priority">The dispatcher priority.</param>
    public static void FocusLater(this FrameworkElement element, DispatcherPriority priority = DispatcherPriority.Input)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Dispatcher.BeginInvoke(() => element.Focus(), priority);
    }

    /// <summary>
    /// Calls <see cref="FrameworkElement.BringIntoView()"/> later on the dispatcher queue.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <param name="priority">The dispatcher priority.</param>
    public static void BringIntoViewLater(this FrameworkElement element, DispatcherPriority priority = DispatcherPriority.Background)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Dispatcher.BeginInvoke(() => element.BringIntoView(), priority);
    }

    /// <summary>
    /// Gets the data context if it has the specified type.
    /// </summary>
    /// <typeparam name="T">The expected data context type.</typeparam>
    /// <param name="element">The framework element.</param>
    /// <returns>The typed data context, or <c>null</c>.</returns>
    public static T? GetDataContext<T>(this FrameworkElement? element)
        where T : class
    {
        return element?.DataContext as T;
    }
    #endregion
}
#endregion
