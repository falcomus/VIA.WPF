// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXDialogOwnerResolver.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Windowing;

#region ### Interface IXDialogOwnerResolver ###
/// <summary>
/// Defines the strategy used to resolve the owner of a modal dialog window.
/// </summary>
public interface IXDialogOwnerResolver
{
    #region ### Public Methods ###
    /// <summary>
    /// Resolves the owner for the specified dialog.
    /// </summary>
    /// <param name="dialog">The dialog whose owner is required.</param>
    /// <param name="ownerSource">
    /// An optional dependency object associated with the desired owner window.
    /// </param>
    /// <returns>The resolved owner, or <see langword="null"/> when no owner is available.</returns>
    Window? ResolveOwner(Window dialog, DependencyObject? ownerSource = null);
    #endregion
}
#endregion