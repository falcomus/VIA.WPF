// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatcherExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Threading;

namespace VIA.WPF.Extensions;

#region ### Class DispatcherExtensions ###
/// <summary>
/// Provides convenience methods for running code on a WPF dispatcher.
/// </summary>
public static class DispatcherExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Invokes the specified action immediately when already on the dispatcher thread; otherwise dispatches it synchronously.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    public static void InvokeIfRequired(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(action);

        if (dispatcher.CheckAccess())
        {
            action();
            return;
        }

        dispatcher.Invoke(action, priority);
    }

    /// <summary>
    /// Invokes the specified function immediately when already on the dispatcher thread; otherwise dispatches it synchronously.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    /// <returns>The function result.</returns>
    public static T InvokeIfRequired<T>(this Dispatcher dispatcher, Func<T> function, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(function);

        return dispatcher.CheckAccess()
            ? function()
            : dispatcher.Invoke(function, priority);
    }

    /// <summary>
    /// Invokes the specified action asynchronously when required.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    /// <returns>The task representing the operation.</returns>
    public static Task InvokeIfRequiredAsync(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(action);

        if (dispatcher.CheckAccess())
        {
            action();
            return Task.CompletedTask;
        }

        return dispatcher.InvokeAsync(action, priority).Task;
    }

    /// <summary>
    /// Invokes the specified function asynchronously when required.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    /// <returns>The task containing the function result.</returns>
    public static Task<T> InvokeIfRequiredAsync<T>(this Dispatcher dispatcher, Func<T> function, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(function);

        if (dispatcher.CheckAccess())
        {
            return Task.FromResult(function());
        }

        return dispatcher.InvokeAsync(function, priority).Task;
    }

    /// <summary>
    /// Invokes the specified asynchronous action on the dispatcher when required.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="action">The asynchronous action to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    /// <returns>The task representing the operation.</returns>
    public static async Task InvokeIfRequiredAsync(this Dispatcher dispatcher, Func<Task> action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(action);

        if (dispatcher.CheckAccess())
        {
            await action().ConfigureAwait(true);
            return;
        }

        await (await dispatcher.InvokeAsync(action, priority).Task.ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Invokes the specified asynchronous function on the dispatcher when required.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="function">The asynchronous function to execute.</param>
    /// <param name="priority">The dispatcher priority.</param>
    /// <returns>The task containing the function result.</returns>
    public static async Task<T> InvokeIfRequiredAsync<T>(this Dispatcher dispatcher, Func<Task<T>> function, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(function);

        if (dispatcher.CheckAccess())
        {
            return await function().ConfigureAwait(true);
        }

        return await (await dispatcher.InvokeAsync(function, priority).Task.ConfigureAwait(false)).ConfigureAwait(false);
    }
    #endregion
}
#endregion
