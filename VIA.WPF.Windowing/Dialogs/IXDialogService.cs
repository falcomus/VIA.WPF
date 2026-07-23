// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXDialogService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Windowing;

#region ### Interface IXDialogService ###
/// <summary>
/// Defines the service used to display modal WPF dialog windows.
/// </summary>
public interface IXDialogService
{
    #region ### Public Methods ###
    /// <summary>
    /// Displays the specified window as a modal dialog.
    /// </summary>
    /// <param name="dialog">The dialog window to display.</param>
    /// <param name="ownerSource">
    /// An optional dependency object used to resolve the owning window.
    /// </param>
    /// <param name="options">Optional dialog presentation settings.</param>
    /// <returns>The normalized dialog result.</returns>
    XDialogResult ShowModal(
        Window dialog,
        DependencyObject? ownerSource = null,
        XDialogOptions? options = null);
    #endregion
}
#endregion