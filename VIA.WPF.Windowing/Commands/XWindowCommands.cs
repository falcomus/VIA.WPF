// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowCommands.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Windowing;

#region ### Class XWindowCommands ###
/// <summary>
/// Provides standard commands for VIA.WPF windows.
/// </summary>
public static class XWindowCommands
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the command that minimizes the window.
    /// </summary>
    public static RoutedUICommand Minimize { get; } = new(
        "Minimize",
        nameof(Minimize),
        typeof(XWindowCommands));

    /// <summary>
    /// Gets the command that toggles maximize and restore.
    /// </summary>
    public static RoutedUICommand MaximizeRestore { get; } = new(
        "MaximizeRestore",
        nameof(MaximizeRestore),
        typeof(XWindowCommands));

    /// <summary>
    /// Gets the command that closes the window.
    /// </summary>
    public static RoutedUICommand Close { get; } = new(
        "Close",
        nameof(Close),
        typeof(XWindowCommands));

    /// <summary>
    /// Gets the command that toggles the current theme mode.
    /// </summary>
    public static RoutedUICommand ToggleThemeMode { get; } = new(
        "ToggleThemeMode",
        nameof(ToggleThemeMode),
        typeof(XWindowCommands));
    #endregion
}
#endregion
