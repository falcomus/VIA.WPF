// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewControlTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.XTreeView;

#region ### Class XTreeViewControlTests ###
/// <summary>
/// Tests stable public contracts of <see cref="VIA.WPF.Controls.XTreeView"/> and <see cref="XTreeViewItem"/>.
/// </summary>
public sealed class XTreeViewControlTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that a new tree view exposes the expected public defaults.
    /// </summary>
    [Fact]
    public void XTreeView_ShouldExposeExpectedDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                global::VIA.WPF.Controls.XTreeView treeView = new();

                Assert.Null(treeView.SelectedDataItem);
                Assert.Null(treeView.HoveredDataItem);
                Assert.False(treeView.IsNodeDragInProgress);
                Assert.True(treeView.ShowNewButton);
                Assert.True(treeView.ShowEditButton);
                Assert.True(treeView.ShowDeleteButton);
                Assert.False(treeView.ShowRootNewButton);
                Assert.Equal("Hauptkategorie", treeView.RootNewButtonText);
                Assert.True(treeView.ShowExpandAllButton);
                Assert.True(treeView.ShowCollapseAllButton);
                Assert.Equal("Alle erweitern", treeView.ExpandAllButtonText);
                Assert.Equal("Alle reduzieren", treeView.CollapseAllButtonText);
                Assert.Null(treeView.NewItemCommand);
                Assert.Null(treeView.EditItemCommand);
                Assert.Null(treeView.DeleteItemCommand);
                Assert.Equal(string.Empty, treeView.ExpandedMemberPath);
                Assert.Equal("Children", treeView.ChildrenMemberPath);
                Assert.Equal(new CornerRadius(4d), treeView.NodeCornerRadius);
                Assert.Equal(new Thickness(6d, 4d, 8d, 4d), treeView.NodePadding);
                Assert.Equal(2d, treeView.ItemSpacing);
                Assert.Equal(XControlSize.Medium, treeView.Size);
                Assert.False(treeView.AllowNodeDragDrop);
                Assert.Equal(XTreeViewNodeDropMode.BeforeAfterInto, treeView.NodeDropMode);
                Assert.Null(treeView.NodeDropHandler);
                Assert.True(treeView.UseDefaultDragAdorner);
                Assert.Equal(0.9d, treeView.DefaultDragAdornerOpacity);
                Assert.Equal(new Point(0d, 0d), treeView.DragMouseAnchorPoint);
                Assert.Equal(new Point(12d, 12d), treeView.DragAdornerTranslation);
                Assert.Equal(new Point(16d, 16d), treeView.EffectAdornerTranslation);
                Assert.Null(treeView.DragAdornerTemplate);
                Assert.Null(treeView.DragAdornerTemplateSelector);
                Assert.True(treeView.ShowAlwaysDropTargetAdorner);
                Assert.Null(treeView.DropTargetAdornerBrush);
                Assert.Null(treeView.DropTargetAdornerPen);
                Assert.Null(treeView.DropTargetHighlightBrush);
                Assert.True(treeView.UseDropTargetHint);
                Assert.Null(treeView.DropHintDataTemplate);
                Assert.NotNull(treeView.NodeDragDropController);
            });
    }

    /// <summary>
    /// Ensures that the bindable selection properties use two-way binding metadata.
    /// </summary>
    [Fact]
    public void SelectionProperties_ShouldBindTwoWayByDefault()
    {
        FrameworkPropertyMetadata selectedMetadata = Assert.IsType<FrameworkPropertyMetadata>(global::VIA.WPF.Controls.XTreeView.SelectedDataItemProperty.GetMetadata(typeof(global::VIA.WPF.Controls.XTreeView)));
        FrameworkPropertyMetadata hoveredMetadata = Assert.IsType<FrameworkPropertyMetadata>(global::VIA.WPF.Controls.XTreeView.HoveredDataItemProperty.GetMetadata(typeof(global::VIA.WPF.Controls.XTreeView)));

        Assert.True(selectedMetadata.BindsTwoWayByDefault);
        Assert.True(hoveredMetadata.BindsTwoWayByDefault);
    }

    /// <summary>
    /// Ensures that public tree view properties round-trip through their dependency properties.
    /// </summary>
    [Fact]
    public void XTreeView_PropertiesShouldRoundTripValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                global::VIA.WPF.Controls.XTreeView treeView = new();
                TestCommand newCommand = new();
                TestCommand editCommand = new();
                TestCommand deleteCommand = new();
                DataTemplate dragTemplate = new();
                DataTemplate dropHintTemplate = new();
                DataTemplateSelector selector = new();
                IXTreeViewNodeDropHandler dropHandler = new TestNodeDropHandler();
                SolidColorBrush brush = new(Colors.Red);
                Pen pen = new(new SolidColorBrush(Colors.Blue), 3d);

                treeView.SelectedDataItem = "Selected";
                treeView.HoveredDataItem = "Hovered";
                treeView.ShowNewButton = false;
                treeView.ShowEditButton = false;
                treeView.ShowDeleteButton = false;
                treeView.ShowRootNewButton = true;
                treeView.RootNewButtonText = "New root";
                treeView.ShowExpandAllButton = false;
                treeView.ShowCollapseAllButton = false;
                treeView.ExpandAllButtonText = "Expand";
                treeView.CollapseAllButtonText = "Collapse";
                treeView.NewItemCommand = newCommand;
                treeView.EditItemCommand = editCommand;
                treeView.DeleteItemCommand = deleteCommand;
                treeView.ExpandedMemberPath = "IsExpanded";
                treeView.ChildrenMemberPath = "Nodes";
                treeView.NodeCornerRadius = new CornerRadius(4d);
                treeView.NodePadding = new Thickness(1d, 2d, 3d, 4d);
                treeView.ItemSpacing = 8d;
                treeView.Size = XControlSize.Large;
                treeView.AllowNodeDragDrop = true;
                treeView.NodeDropMode = XTreeViewNodeDropMode.Into;
                treeView.NodeDropHandler = dropHandler;
                treeView.UseDefaultDragAdorner = false;
                treeView.DefaultDragAdornerOpacity = 0.5d;
                treeView.DragMouseAnchorPoint = new Point(1d, 2d);
                treeView.DragAdornerTranslation = new Point(3d, 4d);
                treeView.EffectAdornerTranslation = new Point(5d, 6d);
                treeView.DragAdornerTemplate = dragTemplate;
                treeView.DragAdornerTemplateSelector = selector;
                treeView.ShowAlwaysDropTargetAdorner = false;
                treeView.DropTargetAdornerBrush = brush;
                treeView.DropTargetAdornerPen = pen;
                treeView.DropTargetHighlightBrush = brush;
                treeView.UseDropTargetHint = false;
                treeView.DropHintDataTemplate = dropHintTemplate;

                Assert.Equal("Selected", treeView.SelectedDataItem);
                Assert.Equal("Hovered", treeView.HoveredDataItem);
                Assert.False(treeView.ShowNewButton);
                Assert.False(treeView.ShowEditButton);
                Assert.False(treeView.ShowDeleteButton);
                Assert.True(treeView.ShowRootNewButton);
                Assert.Equal("New root", treeView.RootNewButtonText);
                Assert.False(treeView.ShowExpandAllButton);
                Assert.False(treeView.ShowCollapseAllButton);
                Assert.Equal("Expand", treeView.ExpandAllButtonText);
                Assert.Equal("Collapse", treeView.CollapseAllButtonText);
                Assert.Same(newCommand, treeView.NewItemCommand);
                Assert.Same(editCommand, treeView.EditItemCommand);
                Assert.Same(deleteCommand, treeView.DeleteItemCommand);
                Assert.Equal("IsExpanded", treeView.ExpandedMemberPath);
                Assert.Equal("Nodes", treeView.ChildrenMemberPath);
                Assert.Equal(new CornerRadius(4d), treeView.NodeCornerRadius);
                Assert.Equal(new Thickness(1d, 2d, 3d, 4d), treeView.NodePadding);
                Assert.Equal(8d, treeView.ItemSpacing);
                Assert.Equal(XControlSize.Large, treeView.Size);
                Assert.True(treeView.AllowNodeDragDrop);
                Assert.Equal(XTreeViewNodeDropMode.Into, treeView.NodeDropMode);
                Assert.Same(dropHandler, treeView.NodeDropHandler);
                Assert.False(treeView.UseDefaultDragAdorner);
                Assert.Equal(0.5d, treeView.DefaultDragAdornerOpacity);
                Assert.Equal(new Point(1d, 2d), treeView.DragMouseAnchorPoint);
                Assert.Equal(new Point(3d, 4d), treeView.DragAdornerTranslation);
                Assert.Equal(new Point(5d, 6d), treeView.EffectAdornerTranslation);
                Assert.Same(dragTemplate, treeView.DragAdornerTemplate);
                Assert.Same(selector, treeView.DragAdornerTemplateSelector);
                Assert.False(treeView.ShowAlwaysDropTargetAdorner);
                Assert.Same(brush, treeView.DropTargetAdornerBrush);
                Assert.Same(pen, treeView.DropTargetAdornerPen);
                Assert.Same(brush, treeView.DropTargetHighlightBrush);
                Assert.False(treeView.UseDropTargetHint);
                Assert.Same(dropHintTemplate, treeView.DropHintDataTemplate);
            });
    }

    /// <summary>
    /// Ensures that command properties are available and executable for empty trees.
    /// </summary>
    [Fact]
    public void ExpandAndCollapseCommands_ShouldBeExecutable()
    {
        WpfTestHelper.Run(
            () =>
            {
                global::VIA.WPF.Controls.XTreeView treeView = new();

                Assert.True(treeView.ExpandAllCommand.CanExecute(null));
                Assert.True(treeView.CollapseAllCommand.CanExecute(null));

                treeView.ExpandAllCommand.Execute(null);
                treeView.CollapseAllCommand.Execute(null);
            });
    }

    /// <summary>
    /// Ensures that a new tree view item exposes the expected public defaults.
    /// </summary>
    [Fact]
    public void XTreeViewItem_ShouldExposeExpectedDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XTreeViewItem item = new();

                Assert.Null(item.ShowNewButton);
                Assert.Null(item.ShowEditButton);
                Assert.Null(item.ShowDeleteButton);
                Assert.Null(item.NewCommand);
                Assert.Null(item.EditCommand);
                Assert.Null(item.DeleteCommand);
                Assert.Null(item.NewCommandParameter);
                Assert.Null(item.EditCommandParameter);
                Assert.Null(item.DeleteCommandParameter);
                Assert.True(item.ResolvedShowNewButton);
                Assert.True(item.ResolvedShowEditButton);
                Assert.True(item.ResolvedShowDeleteButton);
                Assert.Null(item.ResolvedNewCommand);
                Assert.Null(item.ResolvedEditCommand);
                Assert.Null(item.ResolvedDeleteCommand);
                Assert.Null(item.ResolvedNewCommandParameter);
                Assert.Null(item.ResolvedEditCommandParameter);
                Assert.Null(item.ResolvedDeleteCommandParameter);
            });
    }

    /// <summary>
    /// Ensures that tree view item properties round-trip through their dependency properties.
    /// </summary>
    [Fact]
    public void XTreeViewItem_PropertiesShouldRoundTripValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                XTreeViewItem item = new();
                TestCommand newCommand = new();
                TestCommand editCommand = new();
                TestCommand deleteCommand = new();
                object newParameter = new();
                object editParameter = new();
                object deleteParameter = new();

                item.ShowNewButton = false;
                item.ShowEditButton = true;
                item.ShowDeleteButton = false;
                item.NewCommand = newCommand;
                item.EditCommand = editCommand;
                item.DeleteCommand = deleteCommand;
                item.NewCommandParameter = newParameter;
                item.EditCommandParameter = editParameter;
                item.DeleteCommandParameter = deleteParameter;

                Assert.False(item.ShowNewButton);
                Assert.True(item.ShowEditButton);
                Assert.False(item.ShowDeleteButton);
                Assert.Same(newCommand, item.NewCommand);
                Assert.Same(editCommand, item.EditCommand);
                Assert.Same(deleteCommand, item.DeleteCommand);
                Assert.Same(newParameter, item.NewCommandParameter);
                Assert.Same(editParameter, item.EditCommandParameter);
                Assert.Same(deleteParameter, item.DeleteCommandParameter);
            });
    }

    /// <summary>
    /// Ensures that node drop information keeps all supplied values.
    /// </summary>
    [Fact]
    public void XTreeViewNodeDropInfo_ShouldExposeConfiguredValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                object draggedItem = new();
                object targetItem = new();
                global::VIA.WPF.Controls.XTreeView treeView = new();
                XTreeViewItem draggedContainer = new();
                XTreeViewItem targetContainer = new();
                object originalDropInfo = new();

                XTreeViewNodeDropInfo dropInfo = new()
                {
                    DraggedItem = draggedItem,
                    TargetItem = targetItem,
                    Position = XTreeViewNodeDropPosition.After,
                    TreeView = treeView,
                    DraggedContainer = draggedContainer,
                    TargetContainer = targetContainer,
                    OriginalDropInfo = originalDropInfo
                };

                Assert.Same(draggedItem, dropInfo.DraggedItem);
                Assert.Same(targetItem, dropInfo.TargetItem);
                Assert.Equal(XTreeViewNodeDropPosition.After, dropInfo.Position);
                Assert.Same(treeView, dropInfo.TreeView);
                Assert.Same(draggedContainer, dropInfo.DraggedContainer);
                Assert.Same(targetContainer, dropInfo.TargetContainer);
                Assert.Same(originalDropInfo, dropInfo.OriginalDropInfo);
            });
    }
    #endregion

    #region ### Class TestCommand ###
    private sealed class TestCommand : ICommand
    {
        #region ### Events ###
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region ### Public Methods ###
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
        }
        #endregion
    }
    #endregion

    #region ### Class TestNodeDropHandler ###
    private sealed class TestNodeDropHandler : IXTreeViewNodeDropHandler
    {
        #region ### Public Methods ###
        public bool CanDrop(XTreeViewNodeDropInfo dropInfo)
        {
            return true;
        }

        public void Drop(XTreeViewNodeDropInfo dropInfo)
        {
        }
        #endregion
    }
    #endregion
}
#endregion
