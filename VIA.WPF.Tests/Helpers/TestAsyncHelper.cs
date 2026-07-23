// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAsyncHelper.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Tests.Helpers;

#region ### Class TestAsyncHelper ###
/// <summary>
/// Provides asynchronous helper methods for tests.
/// </summary>
internal static class TestAsyncHelper
{
    #region ### Internal Methods ###
    /// <summary>
    /// Waits until the specified condition becomes true.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="timeout">The maximum wait time.</param>
    /// <param name="pollInterval">The polling interval.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal static async Task WaitUntilAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollInterval = null)
    {
        ArgumentNullException.ThrowIfNull(condition);

        DateTimeOffset limit = DateTimeOffset.UtcNow.Add(timeout ?? TimeSpan.FromSeconds(2));
        TimeSpan delay = pollInterval ?? TimeSpan.FromMilliseconds(10);

        while (DateTimeOffset.UtcNow <= limit)
        {
            if (condition())
            {
                return;
            }

            await Task.Delay(delay).ConfigureAwait(false);
        }

        Assert.True(condition(), "The expected condition did not become true before the timeout expired.");
    }
    #endregion
}
#endregion
