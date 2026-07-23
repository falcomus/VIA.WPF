// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogOptions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Windowing;

#region ### Class XDialogOptions ###
/// <summary>
/// Defines optional presentation settings for a modal dialog.
/// </summary>
public sealed class XDialogOptions
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared default dialog options.
    /// </summary>
    public static XDialogOptions Default { get; } = new();
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets a value indicating whether a supported owner window is visually dimmed while the dialog is open.
    /// </summary>
    public bool DimOwner { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the owner and its previously focused element are restored after the dialog closes.
    /// </summary>
    public bool RestoreOwnerFocus { get; init; } = true;

    /// <summary>
    /// Gets the explicit startup location for the dialog.
    /// </summary>
    /// <remarks>
    /// When no value is specified, a dialog with the default
    /// <see cref="WindowStartupLocation.Manual"/> setting is centered on its owner
    /// or on the screen when no owner can be resolved.
    /// </remarks>
    public WindowStartupLocation? StartupLocation { get; init; }
    #endregion
}
#endregion
