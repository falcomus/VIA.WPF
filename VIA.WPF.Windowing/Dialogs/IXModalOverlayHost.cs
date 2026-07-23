// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXModalOverlayHost.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Windowing;

#region ### Interface IXModalOverlayHost ###
/// <summary>
/// Defines a window that can expose an owner-local visual overlay while a modal child window is open.
/// </summary>
internal interface IXModalOverlayHost
{
    #region ### Public Methods ###
    /// <summary>
    /// Acquires a modal overlay lease.
    /// </summary>
    /// <returns>A lease that releases the overlay when disposed.</returns>
    IDisposable AcquireModalOverlay();
    #endregion
}
#endregion