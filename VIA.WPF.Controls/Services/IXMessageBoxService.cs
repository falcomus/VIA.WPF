// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXMessageBoxService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Services;

#region ### Interface IXMessageBoxService ###
/// <summary>
/// Provides message box operations that can be injected into view models or application services.
/// </summary>
public interface IXMessageBoxService
{
    #region ### Public Methods ###
    /// <summary>
    /// Shows an informational message.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="caption">The caption.</param>
    void ShowInfo(string message, string caption = "Information");

    /// <summary>
    /// Shows a warning message.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="caption">The caption.</param>
    void ShowWarning(string message, string caption = "Warning");

    /// <summary>
    /// Shows an error message.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="caption">The caption.</param>
    void ShowError(string message, string caption = "Error");

    /// <summary>
    /// Shows a confirmation message.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="caption">The caption.</param>
    /// <returns><c>true</c> when the user confirmed; otherwise, <c>false</c>.</returns>
    bool Confirm(string message, string caption = "Confirm");

    /// <summary>
    /// Shows a message box.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="caption">The caption.</param>
    /// <param name="button">The displayed buttons.</param>
    /// <param name="image">The displayed image.</param>
    /// <returns>The selected message box result.</returns>
    MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage image);
    #endregion
}
#endregion
