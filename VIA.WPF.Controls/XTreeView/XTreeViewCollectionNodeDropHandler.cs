// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewCollectionNodeDropHandler.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Class XTreeViewCollectionNodeDropHandler<TNode> ###
/// <summary>
/// Provides a default node drop handler for mutable hierarchical collections.
/// </summary>
/// <typeparam name="TNode">The node type.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="XTreeViewCollectionNodeDropHandler{TNode}"/> class.
/// </remarks>
/// <param name="rootItems">The root node collection.</param>
/// <param name="childItemsSelector">The child collection selector.</param>
public sealed class XTreeViewCollectionNodeDropHandler<TNode>(IList<TNode> rootItems, Func<TNode, IList<TNode>?> childItemsSelector) : IXTreeViewNodeDropHandler
{
    #region ### Private Fields ###
    /// <summary>
    /// The root node collection.
    /// </summary>
    private readonly IList<TNode> rootItems = rootItems;

    /// <summary>
    /// Gets the mutable child collection for a node.
    /// </summary>
    private readonly Func<TNode, IList<TNode>?> childItemsSelector = childItemsSelector;

    #endregion
    #region ### Constructors ###
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public bool CanDrop(XTreeViewNodeDropInfo dropInfo)
    {
        if (dropInfo.DraggedItem is not TNode draggedNode)
        {
            return false;
        }

        if (!this.TryFindOwningList(draggedNode, out _, out _))
        {
            return false;
        }

        if (dropInfo.Position == XTreeViewNodeDropPosition.Root)
        {
            return true;
        }

        return dropInfo.TargetItem is TNode targetNode && !this.IsDescendantOf(draggedNode, targetNode);
    }

    /// <inheritdoc />
    public void Drop(XTreeViewNodeDropInfo dropInfo)
    {
        if (!this.TryCreateMove(dropInfo, out XTreeViewNodeMove move))
        {
            return;
        }

        move.SourceItems.RemoveAt(move.SourceIndex);

        int targetIndex = move.TargetIndex;
        if (ReferenceEquals(move.SourceItems, move.TargetItems) && move.SourceIndex < targetIndex)
        {
            targetIndex--;
        }

        targetIndex = Math.Max(0, Math.Min(targetIndex, move.TargetItems.Count));
        move.TargetItems.Insert(targetIndex, move.DraggedNode);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Tries to create a concrete collection move description.
    /// </summary>
    /// <param name="dropInfo">The drop information.</param>
    /// <param name="move">The created move description.</param>
    /// <returns><c>true</c> if a move could be created; otherwise, <c>false</c>.</returns>
    private bool TryCreateMove(XTreeViewNodeDropInfo dropInfo, out XTreeViewNodeMove move)
    {
        move = default;

        if (dropInfo.DraggedItem is not TNode draggedNode ||
            !this.TryFindOwningList(draggedNode, out IList<TNode>? sourceItems, out int sourceIndex))
        {
            return false;
        }

        IList<TNode>? targetItems;
        int targetIndex;

        if (dropInfo.Position == XTreeViewNodeDropPosition.Root)
        {
            targetItems = this.rootItems;
            targetIndex = targetItems.Count;
        }
        else
        {
            if (dropInfo.TargetItem is not TNode targetNode)
            {
                return false;
            }

            if (dropInfo.Position == XTreeViewNodeDropPosition.Into)
            {
                targetItems = this.childItemsSelector(targetNode);
                targetIndex = targetItems?.Count ?? -1;
            }
            else
            {
                if (!this.TryFindOwningList(targetNode, out targetItems, out int foundTargetIndex))
                {
                    return false;
                }

                targetIndex = dropInfo.Position == XTreeViewNodeDropPosition.Before
                    ? foundTargetIndex
                    : foundTargetIndex + 1;
            }
        }

        if (targetItems is null || targetIndex < 0)
        {
            return false;
        }

        move = new XTreeViewNodeMove(draggedNode, sourceItems, sourceIndex, targetItems, targetIndex);
        return true;
    }

    /// <summary>
    /// Finds the mutable list that currently owns the specified node.
    /// </summary>
    /// <param name="node">The node to find.</param>
    /// <param name="items">The owning list.</param>
    /// <param name="index">The index inside the owning list.</param>
    /// <returns><c>true</c> if the owning list was found; otherwise, <c>false</c>.</returns>
    private bool TryFindOwningList(TNode node, out IList<TNode> items, out int index)
    {
        return this.TryFindOwningList(this.rootItems, node, out items, out index);
    }

    /// <summary>
    /// Finds the mutable list that currently owns the specified node below the specified list.
    /// </summary>
    /// <param name="currentItems">The current list.</param>
    /// <param name="node">The node to find.</param>
    /// <param name="items">The owning list.</param>
    /// <param name="index">The index inside the owning list.</param>
    /// <returns><c>true</c> if the owning list was found; otherwise, <c>false</c>.</returns>
    private bool TryFindOwningList(IList<TNode> currentItems, TNode node, out IList<TNode> items, out int index)
    {
        for (int currentIndex = 0; currentIndex < currentItems.Count; currentIndex++)
        {
            TNode currentNode = currentItems[currentIndex];

            if (AreSameNode(currentNode, node))
            {
                items = currentItems;
                index = currentIndex;
                return true;
            }

            IList<TNode>? childItems = this.childItemsSelector(currentNode);
            if (childItems is not null && this.TryFindOwningList(childItems, node, out items, out index))
            {
                return true;
            }
        }

        items = this.rootItems;
        index = -1;
        return false;
    }

    /// <summary>
    /// Gets a value indicating whether the potential descendant is below the specified ancestor.
    /// </summary>
    /// <param name="ancestor">The potential ancestor.</param>
    /// <param name="potentialDescendant">The potential descendant.</param>
    /// <returns><c>true</c> if the descendant is below the ancestor; otherwise, <c>false</c>.</returns>
    private bool IsDescendantOf(TNode ancestor, TNode potentialDescendant)
    {
        IList<TNode>? childItems = this.childItemsSelector(ancestor);
        if (childItems is null)
        {
            return false;
        }

        foreach (TNode childItem in childItems)
        {
            if (AreSameNode(childItem, potentialDescendant) || this.IsDescendantOf(childItem, potentialDescendant))
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Gets a value indicating whether two nodes are the same move node.
    /// </summary>
    /// <param name="first">The first node.</param>
    /// <param name="second">The second node.</param>
    /// <returns><c>true</c> if both values represent the same move node; otherwise, <c>false</c>.</returns>
    private static bool AreSameNode(TNode first, TNode second)
    {
        return typeof(TNode).IsValueType
            ? EqualityComparer<TNode>.Default.Equals(first, second)
            : ReferenceEquals(first, second);
    }
    #endregion

    #region ### Struct XTreeViewNodeMove ###
    /// <summary>
    /// Represents a concrete collection move.
    /// </summary>
    /// <param name="DraggedNode">The dragged node.</param>
    /// <param name="SourceItems">The source list.</param>
    /// <param name="SourceIndex">The source index.</param>
    /// <param name="TargetItems">The target list.</param>
    /// <param name="TargetIndex">The target index.</param>
    private readonly record struct XTreeViewNodeMove(
        TNode DraggedNode,
        IList<TNode> SourceItems,
        int SourceIndex,
        IList<TNode> TargetItems,
        int TargetIndex);
    #endregion
}
#endregion
