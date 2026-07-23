// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewReflectionNodeDropHandler.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace VIA.WPF.Controls;

#region ### Class XTreeViewReflectionNodeDropHandler ###
/// <summary>
/// Provides a default reflection-based node drop handler for mutable hierarchical collections.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="XTreeViewReflectionNodeDropHandler"/> class.
/// </remarks>
/// <param name="rootItems">The root collection.</param>
/// <param name="childrenMemberPath">The child collection member path.</param>
internal sealed class XTreeViewReflectionNodeDropHandler(object rootItems, string childrenMemberPath) : IXTreeViewNodeDropHandler
{
    #region ### Fields ###
    /// <summary>
    /// The root collection accessor.
    /// </summary>
    private readonly XMutableListAccessor rootItems = new(rootItems);

    /// <summary>
    /// The child collection member path.
    /// </summary>
    private readonly string childrenMemberPath = childrenMemberPath;

    #endregion
    #region ### Constructors ###
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public bool CanDrop(XTreeViewNodeDropInfo dropInfo)
    {
        object draggedNode = dropInfo.DraggedItem;

        if (!this.rootItems.IsMutable || !this.TryFindOwningList(draggedNode, out _, out _))
        {
            return false;
        }

        if (dropInfo.Position == XTreeViewNodeDropPosition.Root)
        {
            return true;
        }

        return dropInfo.TargetItem is not null && !this.IsDescendantOf(draggedNode, dropInfo.TargetItem);
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
        if (ReferenceEquals(move.SourceItems.Source, move.TargetItems.Source) && move.SourceIndex < targetIndex)
        {
            targetIndex--;
        }

        targetIndex = Math.Max(0, Math.Min(targetIndex, move.TargetItems.Count));
        move.TargetItems.Insert(targetIndex, move.DraggedNode);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Tries to create a concrete collection move.
    /// </summary>
    /// <param name="dropInfo">The drop information.</param>
    /// <param name="move">The created move.</param>
    /// <returns><c>true</c> if a move could be created; otherwise, <c>false</c>.</returns>
    private bool TryCreateMove(XTreeViewNodeDropInfo dropInfo, out XTreeViewNodeMove move)
    {
        move = default;

        object draggedNode = dropInfo.DraggedItem;
        if (!this.TryFindOwningList(draggedNode, out XMutableListAccessor sourceItems, out int sourceIndex))
        {
            return false;
        }

        XMutableListAccessor? targetItems;
        int targetIndex;

        if (dropInfo.Position == XTreeViewNodeDropPosition.Root)
        {
            targetItems = this.rootItems;
            targetIndex = targetItems.Count;
        }
        else
        {
            if (dropInfo.TargetItem is null)
            {
                return false;
            }

            if (dropInfo.Position == XTreeViewNodeDropPosition.Into)
            {
                targetItems = this.GetChildListAccessor(dropInfo.TargetItem);
                targetIndex = targetItems?.Count ?? -1;
            }
            else
            {
                if (!this.TryFindOwningList(dropInfo.TargetItem, out targetItems, out int foundTargetIndex))
                {
                    return false;
                }

                targetIndex = dropInfo.Position == XTreeViewNodeDropPosition.Before
                    ? foundTargetIndex
                    : foundTargetIndex + 1;
            }
        }

        if (targetItems is null || !targetItems.IsMutable || targetIndex < 0)
        {
            return false;
        }

        move = new XTreeViewNodeMove(draggedNode, sourceItems, sourceIndex, targetItems, targetIndex);
        return true;
    }

    /// <summary>
    /// Finds the list owning the specified node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="items">The owning list.</param>
    /// <param name="index">The index in the owning list.</param>
    /// <returns><c>true</c> if the owning list was found; otherwise, <c>false</c>.</returns>
    private bool TryFindOwningList(object node, out XMutableListAccessor items, out int index)
    {
        return this.TryFindOwningList(this.rootItems, node, out items, out index);
    }

    /// <summary>
    /// Finds the list owning the specified node below the specified list.
    /// </summary>
    /// <param name="currentItems">The current list.</param>
    /// <param name="node">The node.</param>
    /// <param name="items">The owning list.</param>
    /// <param name="index">The index in the owning list.</param>
    /// <returns><c>true</c> if the owning list was found; otherwise, <c>false</c>.</returns>
    private bool TryFindOwningList(XMutableListAccessor currentItems, object node, out XMutableListAccessor items, out int index)
    {
        for (int currentIndex = 0; currentIndex < currentItems.Count; currentIndex++)
        {
            object? currentNode = currentItems[currentIndex];
            if (currentNode is null)
            {
                continue;
            }

            if (ReferenceEquals(currentNode, node))
            {
                items = currentItems;
                index = currentIndex;
                return true;
            }

            XMutableListAccessor? childItems = this.GetChildListAccessor(currentNode);
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
    private bool IsDescendantOf(object ancestor, object potentialDescendant)
    {
        XMutableListAccessor? childItems = this.GetChildListAccessor(ancestor);
        if (childItems is null)
        {
            return false;
        }

        foreach (object? childItem in childItems)
        {
            if (childItem is null)
            {
                continue;
            }

            if (ReferenceEquals(childItem, potentialDescendant) || this.IsDescendantOf(childItem, potentialDescendant))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the child list accessor of the specified node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>The child list accessor or <see langword="null"/>.</returns>
    private XMutableListAccessor? GetChildListAccessor(object node)
    {
        PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(node).Find(this.childrenMemberPath, false);
        object? children = propertyDescriptor?.GetValue(node);

        return children is null
            ? null
            : new XMutableListAccessor(children);
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
        object DraggedNode,
        XMutableListAccessor SourceItems,
        int SourceIndex,
        XMutableListAccessor TargetItems,
        int TargetIndex);
    #endregion

    #region ### Class XMutableListAccessor ###
    /// <summary>
    /// Provides non-generic mutable list access by using either <see cref="IList"/> or reflection.
    /// </summary>
    private sealed class XMutableListAccessor : IEnumerable<object?>
    {
        #region ### Fields ###
        /// <summary>
        /// The source collection object.
        /// </summary>
        private readonly object source;

        /// <summary>
        /// The non-generic list, if available.
        /// </summary>
        private readonly IList? list;

        /// <summary>
        /// The count property.
        /// </summary>
        private readonly PropertyInfo? countProperty;

        /// <summary>
        /// The indexer property.
        /// </summary>
        private readonly PropertyInfo? indexerProperty;

        /// <summary>
        /// The insert method.
        /// </summary>
        private readonly MethodInfo? insertMethod;

        /// <summary>
        /// The remove-at method.
        /// </summary>
        private readonly MethodInfo? removeAtMethod;
        #endregion

        #region ### Constructors ###
        /// <summary>
        /// Initializes a new instance of the <see cref="XMutableListAccessor"/> class.
        /// </summary>
        /// <param name="source">The source collection.</param>
        public XMutableListAccessor(object source)
        {
            this.source = source;
            this.list = source as IList;

            Type sourceType = source.GetType();
            this.countProperty = sourceType.GetProperty("Count");
            this.indexerProperty = sourceType
                .GetProperties()
                .FirstOrDefault(property => property.GetIndexParameters().Length == 1);

            this.insertMethod = sourceType
                .GetMethods()
                .FirstOrDefault(method => method.Name == nameof(IList.Insert) && method.GetParameters().Length == 2);

            this.removeAtMethod = sourceType
                .GetMethods()
                .FirstOrDefault(method => method.Name == nameof(IList.RemoveAt) && method.GetParameters().Length == 1);
        }
        #endregion

        #region ### Public Properties ###
        /// <summary>
        /// Gets the source collection object.
        /// </summary>
        public object Source => this.source;

        /// <summary>
        /// Gets a value indicating whether this collection can be mutated.
        /// </summary>
        public bool IsMutable => this.list is not null || this.insertMethod is not null && this.removeAtMethod is not null;

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        public int Count => this.list?.Count ?? Convert.ToInt32(this.countProperty?.GetValue(this.source) ?? 0);

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The item index.</param>
        /// <returns>The item.</returns>
        public object? this[int index] => this.list is not null
            ? this.list[index]
            : this.indexerProperty?.GetValue(this.source, [index]);
        #endregion

        #region ### Public Methods ###
        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The target index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, object item)
        {
            if (this.list is not null)
            {
                this.list.Insert(index, item);
                return;
            }

            this.insertMethod?.Invoke(this.source, [index, item]);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The item index.</param>
        public void RemoveAt(int index)
        {
            if (this.list is not null)
            {
                this.list.RemoveAt(index);
                return;
            }

            this.removeAtMethod?.Invoke(this.source, [index]);
        }

        /// <inheritdoc />
        public IEnumerator<object?> GetEnumerator()
        {
            if (this.source is not IEnumerable enumerable)
            {
                yield break;
            }

            foreach (object? item in enumerable)
            {
                yield return item;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
    #endregion
}
#endregion
