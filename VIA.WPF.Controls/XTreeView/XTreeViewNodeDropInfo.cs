// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewNodeDropInfo.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Class XTreeViewNodeDropInfo ###
/// <summary>
/// Contains all relevant information for a tree node drop operation.
/// </summary>
public sealed class XTreeViewNodeDropInfo
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the dragged data item.
    /// </summary>
    public required object DraggedItem { get; init; }

    /// <summary>
    /// Gets the target data item.
    /// </summary>
    public object? TargetItem { get; init; }

    /// <summary>
    /// Gets the requested drop position.
    /// </summary>
    public required XTreeViewNodeDropPosition Position { get; init; }

    /// <summary>
    /// Gets the owning tree view.
    /// </summary>
    public required XTreeView TreeView { get; init; }

    /// <summary>
    /// Gets the realized dragged container, if available.
    /// </summary>
    public XTreeViewItem? DraggedContainer { get; init; }

    /// <summary>
    /// Gets the realized target container, if available.
    /// </summary>
    public XTreeViewItem? TargetContainer { get; init; }

    /// <summary>
    /// Gets the original library-specific drop information object.
    /// </summary>
    public object? OriginalDropInfo { get; init; }
    #endregion
}
#endregion
