// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Extensions;

#region ### Class TaskExtensions ###
/// <summary>
/// Provides convenience methods for tasks.
/// </summary>
public static class TaskExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Observes a fire-and-forget task and forwards exceptions to the optional error handler.
    /// </summary>
    /// <param name="task">The task to observe.</param>
    /// <param name="errorHandler">The optional error handler.</param>
    public static void Forget(this Task task, Action<Exception>? errorHandler = null)
    {
        ArgumentNullException.ThrowIfNull(task);

        _ = ForgetAsync(task, errorHandler);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Awaits the specified task and handles exceptions.
    /// </summary>
    /// <param name="task">The task to await.</param>
    /// <param name="errorHandler">The optional error handler.</param>
    /// <returns>The task representing the operation.</returns>
    private static async Task ForgetAsync(Task task, Action<Exception>? errorHandler)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception exception) when (errorHandler is not null)
        {
            errorHandler(exception);
        }
    }
    #endregion
}
#endregion
