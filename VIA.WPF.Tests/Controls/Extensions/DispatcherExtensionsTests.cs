// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatcherExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Threading;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class DispatcherExtensionsTests ###
/// <summary>
/// Provides tests for dispatcher extension helpers.
/// </summary>
public sealed class DispatcherExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that dispatcher helpers execute synchronously when already on the dispatcher thread.
    /// </summary>
    [Fact]
    public void DispatcherExtensions_ShouldInvokeImmediatelyWhenAccessIsAvailable()
    {
        WpfTestHelper.Run(
            () =>
            {
                Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
                int actionCount = 0;

                dispatcher.InvokeIfRequired(() => actionCount++);
                int value = dispatcher.InvokeIfRequired(static () => 42);

                Assert.Equal(1, actionCount);
                Assert.Equal(42, value);
            });
    }

    /// <summary>
    /// Ensures that asynchronous dispatcher helpers execute immediately when already on the dispatcher thread.
    /// </summary>
    [Fact]
    public void DispatcherExtensions_ShouldInvokeAsyncImmediatelyWhenAccessIsAvailable()
    {
        WpfTestHelper.Run(
            () =>
            {
                Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
                int actionCount = 0;

                dispatcher.InvokeIfRequiredAsync(() => actionCount++).GetAwaiter().GetResult();
                int value = dispatcher.InvokeIfRequiredAsync(static () => 42).GetAwaiter().GetResult();
                dispatcher.InvokeIfRequiredAsync(
                    () =>
                    {
                        actionCount++;
                        return Task.CompletedTask;
                    }).GetAwaiter().GetResult();
                string text = dispatcher.InvokeIfRequiredAsync(static () => Task.FromResult("Value")).GetAwaiter().GetResult();

                Assert.Equal(2, actionCount);
                Assert.Equal(42, value);
                Assert.Equal("Value", text);
            });
    }

    /// <summary>
    /// Ensures that dispatcher helpers reject null arguments.
    /// </summary>
    [Fact]
    public void DispatcherExtensions_ShouldRejectNullArguments()
    {
        WpfTestHelper.Run(
async () =>
            {
                Dispatcher? dispatcher = null;
                Dispatcher validDispatcher = Dispatcher.CurrentDispatcher;

                Assert.Throws<ArgumentNullException>(() => dispatcher!.InvokeIfRequired(() => { }));
                Assert.Throws<ArgumentNullException>(() => validDispatcher.InvokeIfRequired((Action)null!));
                Assert.Throws<ArgumentNullException>(() => validDispatcher.InvokeIfRequired((Func<int>)null!));
                await Assert.ThrowsAsync<ArgumentNullException>(() => dispatcher!.InvokeIfRequiredAsync(() => { }));
                await Assert.ThrowsAsync<ArgumentNullException>(() => validDispatcher.InvokeIfRequiredAsync((Action)null!));
                await Assert.ThrowsAsync<ArgumentNullException>(() => validDispatcher.InvokeIfRequiredAsync((Func<int>)null!));
                await Assert.ThrowsAsync<ArgumentNullException>(() => validDispatcher.InvokeIfRequiredAsync((Func<Task>)null!));
                await Assert.ThrowsAsync<ArgumentNullException>(() => validDispatcher.InvokeIfRequiredAsync((Func<Task<int>>)null!));
            });
    }
    #endregion
}
#endregion
