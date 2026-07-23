// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewCollectionNodeDropHandlerTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.XTreeView;

#region ### Class XTreeViewCollectionNodeDropHandlerTests ###
/// <summary>
/// Tests the collection based tree node drop handler.
/// </summary>
public sealed class XTreeViewCollectionNodeDropHandlerTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that non-matching dragged items are rejected.
    /// </summary>
    [Fact]
    public void CanDrop_ShouldReturnFalseWhenDraggedItemHasWrongType()
    {
        WpfTestHelper.Run(
            () =>
            {
                List<TestNode> nodes = [new("A")];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                bool result = handler.CanDrop(CreateDropInfo("Wrong", null, XTreeViewNodeDropPosition.Root));

                Assert.False(result);
            });
    }

    /// <summary>
    /// Ensures that dragged items outside of the configured root hierarchy are rejected.
    /// </summary>
    [Fact]
    public void CanDrop_ShouldReturnFalseWhenDraggedItemIsNotOwnedByTree()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode knownNode = new("A");
                TestNode unknownNode = new("B");
                List<TestNode> nodes = [knownNode];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                bool result = handler.CanDrop(CreateDropInfo(unknownNode, null, XTreeViewNodeDropPosition.Root));

                Assert.False(result);
            });
    }

    /// <summary>
    /// Ensures that root drops are allowed for known dragged nodes.
    /// </summary>
    [Fact]
    public void CanDrop_ShouldAllowRootDropForOwnedDraggedItem()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode node = new("A");
                List<TestNode> nodes = [node];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                bool result = handler.CanDrop(CreateDropInfo(node, null, XTreeViewNodeDropPosition.Root));

                Assert.True(result);
            });
    }

    /// <summary>
    /// Ensures that moving a node into one of its descendants is rejected.
    /// </summary>
    [Fact]
    public void CanDrop_ShouldRejectDroppingAncestorIntoDescendant()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode parent = new("Parent");
                TestNode child = new("Child");
                TestNode grandChild = new("GrandChild");
                parent.Children.Add(child);
                child.Children.Add(grandChild);
                List<TestNode> nodes = [parent];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                bool result = handler.CanDrop(CreateDropInfo(parent, grandChild, XTreeViewNodeDropPosition.Into));

                Assert.False(result);
            });
    }

    /// <summary>
    /// Ensures that dropping before a sibling reorders the owning collection.
    /// </summary>
    [Fact]
    public void Drop_ShouldMoveDraggedNodeBeforeTargetSibling()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode first = new("A");
                TestNode second = new("B");
                TestNode third = new("C");
                List<TestNode> nodes = [first, second, third];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                handler.Drop(CreateDropInfo(third, first, XTreeViewNodeDropPosition.Before));

                Assert.Equal([third, first, second], nodes);
            });
    }

    /// <summary>
    /// Ensures that dropping after a sibling reorders the owning collection.
    /// </summary>
    [Fact]
    public void Drop_ShouldMoveDraggedNodeAfterTargetSibling()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode first = new("A");
                TestNode second = new("B");
                TestNode third = new("C");
                List<TestNode> nodes = [first, second, third];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                handler.Drop(CreateDropInfo(first, third, XTreeViewNodeDropPosition.After));

                Assert.Equal([second, third, first], nodes);
            });
    }

    /// <summary>
    /// Ensures that dropping into a target appends the dragged node to the target children.
    /// </summary>
    [Fact]
    public void Drop_ShouldMoveDraggedNodeIntoTargetChildren()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode parent = new("Parent");
                TestNode existingChild = new("ExistingChild");
                TestNode dragged = new("Dragged");
                parent.Children.Add(existingChild);
                List<TestNode> nodes = [parent, dragged];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                handler.Drop(CreateDropInfo(dragged, parent, XTreeViewNodeDropPosition.Into));

                Assert.Equal([parent], nodes);
                Assert.Equal([existingChild, dragged], parent.Children);
            });
    }

    /// <summary>
    /// Ensures that root drops append the dragged node to the root collection.
    /// </summary>
    [Fact]
    public void Drop_ShouldMoveNestedNodeToRootEnd()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode parent = new("Parent");
                TestNode child = new("Child");
                TestNode root = new("Root");
                parent.Children.Add(child);
                List<TestNode> nodes = [parent, root];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                handler.Drop(CreateDropInfo(child, null, XTreeViewNodeDropPosition.Root));

                Assert.Empty(parent.Children);
                Assert.Equal([parent, root, child], nodes);
            });
    }

    /// <summary>
    /// Ensures that invalid targets do not mutate the hierarchy.
    /// </summary>
    [Fact]
    public void Drop_ShouldIgnoreInvalidTargetItem()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode first = new("A");
                TestNode second = new("B");
                List<TestNode> nodes = [first, second];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = CreateHandler(nodes);

                handler.Drop(CreateDropInfo(first, "Wrong", XTreeViewNodeDropPosition.Before));

                Assert.Equal([first, second], nodes);
            });
    }

    /// <summary>
    /// Ensures that a missing child collection prevents invalid into moves.
    /// </summary>
    [Fact]
    public void Drop_ShouldIgnoreIntoDropWhenTargetChildrenCollectionIsMissing()
    {
        WpfTestHelper.Run(
            () =>
            {
                TestNode target = new("Target");
                TestNode dragged = new("Dragged");
                List<TestNode> nodes = [target, dragged];
                XTreeViewCollectionNodeDropHandler<TestNode> handler = new(nodes, node => node.Name == "Target" ? null : node.Children);

                handler.Drop(CreateDropInfo(dragged, target, XTreeViewNodeDropPosition.Into));

                Assert.Equal([target, dragged], nodes);
                Assert.Empty(target.Children);
            });
    }
    #endregion

    #region ### Private Methods ###
    private static XTreeViewCollectionNodeDropHandler<TestNode> CreateHandler(IList<TestNode> nodes)
    {
        return new XTreeViewCollectionNodeDropHandler<TestNode>(nodes, node => node.Children);
    }

    private static XTreeViewNodeDropInfo CreateDropInfo(object draggedItem, object? targetItem, XTreeViewNodeDropPosition position)
    {
        return new XTreeViewNodeDropInfo
        {
            DraggedItem = draggedItem,
            TargetItem = targetItem,
            Position = position,
            TreeView = new global::VIA.WPF.Controls.XTreeView()
        };
    }
    #endregion

    #region ### Class TestNode ###
    private sealed class TestNode(string name)
    {
        #region ### Public Properties ###
        public string Name { get; } = name;

        public List<TestNode> Children { get; } = [];
        #endregion

        #region ### Public Methods ###
        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }
    #endregion
}
#endregion
