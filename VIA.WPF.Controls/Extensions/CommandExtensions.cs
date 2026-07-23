// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Extensions;

#region ### Class CommandExtensions ###
/// <summary>
/// Provides convenience methods for WPF commands.
/// </summary>
public static class CommandExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Executes the command when it can be executed with the specified parameter.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    /// <returns><c>true</c> if the command was executed; otherwise, <c>false</c>.</returns>
    public static bool ExecuteIfCan(this ICommand? command, object? parameter = null)
    {
        if (command?.CanExecute(parameter) != true)
        {
            return false;
        }

        command.Execute(parameter);

        return true;
    }

    /// <summary>
    /// Checks whether the command can be executed with the specified parameter.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    public static bool CanExecuteSafe(this ICommand? command, object? parameter = null)
    {
        return command?.CanExecute(parameter) == true;
    }
    #endregion
}
#endregion
