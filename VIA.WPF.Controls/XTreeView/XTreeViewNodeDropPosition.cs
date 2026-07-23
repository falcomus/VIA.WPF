// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewNodeDropPosition.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XTreeViewNodeDropPosition ###
/// <summary>
/// Defines the requested drop position for a dragged tree node.
/// </summary>
public enum XTreeViewNodeDropPosition
{
    #region ### Values ###
    /// <summary>
    /// The dragged node should be inserted before the target node.
    /// </summary>
    Before,

    /// <summary>
    /// The dragged node should be inserted after the target node.
    /// </summary>
    After,

    /// <summary>
    /// The dragged node should be inserted into the target node.
    /// </summary>
    Into,

    /// <summary>
    /// The dragged node should be appended to the tree root collection.
    /// </summary>
    Root
    #endregion
}
#endregion
