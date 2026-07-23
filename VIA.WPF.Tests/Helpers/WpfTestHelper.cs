// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfTestHelper.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.ExceptionServices;
using System.Windows.Threading;

namespace VIA.WPF.Tests.Helpers;

#region ### Class WpfTestHelper ###
/// <summary>
/// Provides helpers for running WPF tests on an STA dispatcher thread.
/// </summary>
internal static class WpfTestHelper
{
    #region ### Fields ###
    private static readonly Lazy<Dispatcher> TestDispatcher = new(CreateDispatcher, LazyThreadSafetyMode.ExecutionAndPublication);
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Runs the specified action on the shared STA dispatcher and rethrows any exception on the calling thread.
    /// </summary>
    /// <param name="action">The action to run.</param>
    internal static void Run(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Exception? exception = null;

        TestDispatcher.Value.Invoke(
            () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });

        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }

    /// <summary>
    /// Processes pending dispatcher operations for the current dispatcher.
    /// </summary>
    internal static void DoEvents()
    {
        DispatcherFrame frame = new();

        Dispatcher.CurrentDispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new DispatcherOperationCallback(
                _ =>
                {
                    frame.Continue = false;
                    return null;
                }),
            null);

        Dispatcher.PushFrame(frame);
    }
    #endregion

    #region ### Private Methods ###
    private static Dispatcher CreateDispatcher()
    {
        Dispatcher? dispatcher = null;
        using ManualResetEventSlim dispatcherReady = new(false);

        Thread thread = new(
            () =>
            {
                dispatcher = Dispatcher.CurrentDispatcher;
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(dispatcher));
                dispatcherReady.Set();
                Dispatcher.Run();
            })
        {
            IsBackground = true,
            Name = "VIA.WPF test dispatcher"
        };

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        dispatcherReady.Wait();

        return dispatcher!;
    }
    #endregion
}
#endregion
