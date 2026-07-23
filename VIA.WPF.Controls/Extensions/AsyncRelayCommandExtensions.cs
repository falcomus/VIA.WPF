// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncRelayCommandExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// <remarks>
//   This optional file requires a reference to CommunityToolkit.Mvvm.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.Extensions;

#region ### Class AsyncRelayCommandExtensions ###
/// <summary>
/// Provides safe helper methods for CommunityToolkit async relay commands.
/// </summary>
public static class AsyncRelayCommandExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Executes an async relay command when it can currently execute.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    /// <returns><c>true</c> if the command was executed; otherwise, <c>false</c>.</returns>
    public static async Task<bool> ExecuteIfCanAsync(this IAsyncRelayCommand? command, object? parameter = null)
    {
        if (command?.CanExecute(parameter) != true)
        {
            return false;
        }

        await command.ExecuteAsync(parameter).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    /// Executes an async relay command and catches any thrown exception.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    /// <returns>The command execution result.</returns>
    public static async Task<XCommandExecutionResult> TryExecuteAsync(this IAsyncRelayCommand? command, object? parameter = null)
    {
        try
        {
            bool executed = await command.ExecuteIfCanAsync(parameter).ConfigureAwait(false);
            return executed ? XCommandExecutionResult.Executed() : XCommandExecutionResult.NotExecuted();
        }
        catch (Exception exception)
        {
            return XCommandExecutionResult.Failed(exception);
        }
    }
    #endregion
}
#endregion

#region ### Class XCommandExecutionResult ###
/// <summary>
/// Represents the result of a command execution attempt.
/// </summary>
public sealed class XCommandExecutionResult
{
    #region ### Constructors ###
    private XCommandExecutionResult(bool wasExecuted, Exception? exception)
    {
        this.WasExecuted = wasExecuted;
        this.Exception = exception;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets a value indicating whether the command was executed.
    /// </summary>
    public bool WasExecuted { get; }

    /// <summary>
    /// Gets the exception that occurred during command execution.
    /// </summary>
    public Exception? Exception { get; }

    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates a successful command execution result.
    /// </summary>
    /// <returns>The command execution result.</returns>
    public static XCommandExecutionResult Executed()
    {
        return new XCommandExecutionResult(true, null);
    }

    /// <summary>
    /// Creates a skipped command execution result.
    /// </summary>
    /// <returns>The command execution result.</returns>
    public static XCommandExecutionResult NotExecuted()
    {
        return new XCommandExecutionResult(false, null);
    }

    /// <summary>
    /// Creates a failed command execution result.
    /// </summary>
    /// <param name="exception">The thrown exception.</param>
    /// <returns>The command execution result.</returns>
    public static XCommandExecutionResult Failed(Exception exception)
    {
        return new XCommandExecutionResult(false, exception);
    }
    #endregion
}
#endregion
