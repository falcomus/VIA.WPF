// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VIA.WPF.Controls;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTreeViewDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XTreeView showcase page.
/// </summary>
public sealed partial class XTreeViewDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    [ObservableProperty]
    private XTreeViewDemoNode? _selectedNavigationNode;

    [ObservableProperty]
    private XTreeViewDemoNode? _selectedDragDropNode;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTreeViewDemoViewModel"/> class.
    /// </summary>
    public XTreeViewDemoViewModel()
    {
        this.NavigationNodeDropHandler = new XTreeViewCollectionNodeDropHandler<XTreeViewDemoNode>(
            this.NavigationNodes,
            static node => node.Children);

        this.DragDropNodeDropHandler = new XTreeViewCollectionNodeDropHandler<XTreeViewDemoNode>(
            this.DragDropNodes,
            static node => node.Children);

        this.SelectedNavigationNode = this.NavigationNodes[0].Children[0].Children[0];
        this.SelectedDragDropNode = this.DragDropNodes[0].Children[0].Children[0];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XTreeView";

    /// <inheritdoc/>
    public override string Description => "Demonstrates hierarchical data, selected data item binding, node actions, expand/collapse commands and native drag/drop.";

    /// <summary>
    /// Gets the drop handler used by the navigation preview.
    /// </summary>
    public IXTreeViewNodeDropHandler NavigationNodeDropHandler { get; }

    /// <summary>
    /// Gets the drop handler used by the drag/drop preview.
    /// </summary>
    public IXTreeViewNodeDropHandler DragDropNodeDropHandler { get; }

    /// <summary>
    /// Gets the nodes used by the navigation preview.
    /// </summary>
    public ObservableCollection<XTreeViewDemoNode> NavigationNodes { get; } =
    [
        new XTreeViewDemoNode("Controls", true,
        [
            new XTreeViewDemoNode("Input", true,
            [
                new XTreeViewDemoNode("XButton"),
                new XTreeViewDemoNode("XTextBox"),
                new XTreeViewDemoNode("XComboBox"),
            ]),
            new XTreeViewDemoNode("Layout", false,
            [
                new XTreeViewDemoNode("XGrid"),
                new XTreeViewDemoNode("XGroup"),
                new XTreeViewDemoNode("XSplitView"),
            ]),
        ]),
        new XTreeViewDemoNode("Themes", true,
        [
            new XTreeViewDemoNode("Default"),
            new XTreeViewDemoNode("Graphite"),
            new XTreeViewDemoNode("Ocean"),
        ]),
    ];

    /// <summary>
    /// Gets the nodes used by the drag and drop preview.
    /// </summary>
    public ObservableCollection<XTreeViewDemoNode> DragDropNodes { get; } =
    [
        new XTreeViewDemoNode("Warehouse North", true,
        [
            new XTreeViewDemoNode("Aisle A", true,
            [
                new XTreeViewDemoNode("Bin A-01"),
                new XTreeViewDemoNode("Bin A-02"),
                new XTreeViewDemoNode("Bin A-03"),
            ]),
            new XTreeViewDemoNode("Aisle B", true,
            [
                new XTreeViewDemoNode("Bin B-01"),
                new XTreeViewDemoNode("Bin B-02"),
                new XTreeViewDemoNode("Bin B-03"),
            ]),
        ]),
        new XTreeViewDemoNode("Warehouse South", true,
        [
            new XTreeViewDemoNode("Receiving", true,
            [
                new XTreeViewDemoNode("Incoming pallets"),
                new XTreeViewDemoNode("Quality check"),
                new XTreeViewDemoNode("Returns"),
            ]),
            new XTreeViewDemoNode("Shipping", true,
            [
                new XTreeViewDemoNode("Picking"),
                new XTreeViewDemoNode("Packing"),
                new XTreeViewDemoNode("Dispatch"),
            ]),
        ]),
    ];

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XTreeView
    AllowNodeDragDrop="True"
    ChildrenMemberPath="Children"
    ExpandedMemberPath="IsExpanded"
    ItemsSource="{Binding DragDropNodes}"
    NodeDropHandler="{Binding DragDropNodeDropHandler}"
    NodeDropMode="BeforeAfterInto"
    SelectedDataItem="{Binding SelectedDragDropNode, Mode=TwoWay}"
    ShowCollapseAllButton="True"
    ShowExpandAllButton="True">
    <via:XTreeView.ItemTemplate>
        <HierarchicalDataTemplate ItemsSource="{Binding Children}">
            <TextBlock Text="{Binding Title}" />
        </HierarchicalDataTemplate>
    </via:XTreeView.ItemTemplate>
</via:XTreeView>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
ObservableCollection<TreeNode> nodes =
[
    new("Warehouse North", true,
    [
        new("Aisle A", true, [new("Bin A-01"), new("Bin A-02")]),
        new("Aisle B", true, [new("Bin B-01"), new("Bin B-02")]),
    ]),
];

IXTreeViewNodeDropHandler nodeDropHandler =
    new XTreeViewCollectionNodeDropHandler<TreeNode>(
        nodes,
        static node => node.Children);

XTreeView treeView = new()
{
    ItemsSource = nodes,
    AllowNodeDragDrop = true,
    ChildrenMemberPath = nameof(TreeNode.Children),
    ExpandedMemberPath = nameof(TreeNode.IsExpanded),
    NodeDropHandler = nodeDropHandler,
    NodeDropMode = XTreeViewNodeDropMode.BeforeAfterInto,
};
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Handles the new node command.
    /// </summary>
    /// <param name="node">The command parameter.</param>
    [RelayCommand]
    private void NewNode(object? node)
    {
        this.SelectedNavigationNode = node as XTreeViewDemoNode ?? this.NavigationNodes[0];
    }

    /// <summary>
    /// Handles the edit node command.
    /// </summary>
    /// <param name="node">The command parameter.</param>
    [RelayCommand]
    private void EditNode(object? node)
    {
        this.SelectedNavigationNode = node as XTreeViewDemoNode;
    }

    /// <summary>
    /// Handles the delete node command.
    /// </summary>
    /// <param name="node">The command parameter.</param>
    [RelayCommand]
    private void DeleteNode(object? node)
    {
        this.SelectedNavigationNode = node as XTreeViewDemoNode;
    }
    #endregion
}
#endregion

#region ### Class XTreeViewDemoNode ###
/// <summary>
/// Represents one tree node in the XTreeView demo.
/// </summary>
public sealed class XTreeViewDemoNode : ObservableObject
{
    #region ### Private Fields ###
    private bool isExpanded;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTreeViewDemoNode"/> class.
    /// </summary>
    /// <param name="title">The node title.</param>
    /// <param name="isExpanded">A value indicating whether the node is expanded.</param>
    /// <param name="children">The child nodes.</param>
    public XTreeViewDemoNode(string title, bool isExpanded = false, IEnumerable<XTreeViewDemoNode>? children = null)
    {
        this.Title = title;
        this.isExpanded = isExpanded;
        this.Children = children is null ? [] : new ObservableCollection<XTreeViewDemoNode>(children);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the node title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => this.isExpanded;
        set => this.SetProperty(ref this.isExpanded, value);
    }

    /// <summary>
    /// Gets the child nodes.
    /// </summary>
    public ObservableCollection<XTreeViewDemoNode> Children { get; }
    #endregion
}
#endregion
