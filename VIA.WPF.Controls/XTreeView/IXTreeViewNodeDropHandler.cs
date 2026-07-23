// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXTreeViewNodeDropHandler.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Interface IXTreeViewNodeDropHandler ###
/// <summary>
/// Provides application-specific validation and execution for tree node drag and drop.
/// </summary>
public interface IXTreeViewNodeDropHandler
{
    #region ### Methods ###
    /// <summary>
    /// Gets a value indicating whether the specified drop operation is allowed.
    /// </summary>
    /// <param name="dropInfo">The drop operation information.</param>
    /// <returns><c>true</c> if the operation is allowed; otherwise, <c>false</c>.</returns>
    bool CanDrop(XTreeViewNodeDropInfo dropInfo);

    /// <summary>
    /// Executes the specified drop operation.
    /// </summary>
    /// <param name="dropInfo">The drop operation information.</param>
    void Drop(XTreeViewNodeDropInfo dropInfo);
    #endregion
}
#endregion
