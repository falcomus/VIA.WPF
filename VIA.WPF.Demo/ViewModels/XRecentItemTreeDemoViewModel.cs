// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRecentItemTreeDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VIA.WPF.Controls;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XRecentItemTreeDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XRecentItemTree showcase page.
/// </summary>
public sealed partial class XRecentItemTreeDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private int createdItemIndex = 1;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XRecentItemTreeDemoViewModel"/> class.
    /// </summary>
    public XRecentItemTreeDemoViewModel()
    {
        this.NodeDropHandler = new XTreeViewCollectionNodeDropHandler<XRecentItemTreeDemoNode>(
            this.NavigationNodes,
            static node => node.Children);

        XRecentItemTreeDemoNode customers = this.NavigationNodes[0].Children[0];
        XRecentItemTreeDemoNode openOrders = this.NavigationNodes[1].Children[0];
        XRecentItemTreeDemoNode stock = this.NavigationNodes[2].Children[1];
        XRecentItemTreeDemoNode reports = this.NavigationNodes[3].Children[0];

        this.AddOrPromoteRecentItem(customers, true);
        this.AddOrPromoteRecentItem(openOrders, false);
        this.AddOrPromoteRecentItem(stock, false);
        this.AddOrPromoteRecentItem(reports, true);

        this.SelectedTreeNode = openOrders;
        this.LastActionText = "Click a recent item to open/select it. Pin keeps a shortcut at the top; remove only removes it from the recent list.";
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XRecentItemTree";

    /// <inheritdoc/>
    public override string Description => "Demonstrates a clean navigation tree with a separate recent-items area, visible pin markers, hover actions and command integration.";

    /// <summary>
    /// Gets the hierarchical navigation nodes.
    /// </summary>
    public ObservableCollection<XRecentItemTreeDemoNode> NavigationNodes { get; } =
    [
        new XRecentItemTreeDemoNode("Customers", "Business partners and contact data", "AccountCircleOutline", true,
        [
            new XRecentItemTreeDemoNode("Customer master data", "Recently edited customer records", "AccountCircleOutline"),
            new XRecentItemTreeDemoNode("Customer groups", "Segments, classifications and account groups", "ShapeOutline"),
            new XRecentItemTreeDemoNode("Contacts", "Contact persons and communication details", "AccountCircleOutline"),
        ]),
        new XRecentItemTreeDemoNode("Orders", "Sales and purchase order workflows", "PackageVariantClosed", true,
        [
            new XRecentItemTreeDemoNode("Open orders", "Orders that still need confirmation or delivery", "CalendarClock"),
            new XRecentItemTreeDemoNode("Invoices", "Invoice and billing documents", "OpenInNew"),
            new XRecentItemTreeDemoNode("Returns", "Return requests and credit notes", "Refresh"),
        ]),
        new XRecentItemTreeDemoNode("Inventory", "Locations, stock and movement data", "PackageVariantClosed", true,
        [
            new XRecentItemTreeDemoNode("Locations", "Physical and logical storage locations", "PackageVariantClosed"),
            new XRecentItemTreeDemoNode("Stock overview", "Current stock by location and product", "ViewGridOutline"),
            new XRecentItemTreeDemoNode("Reservations", "Reserved quantities and allocation state", "CalendarClock"),
        ]),
        new XRecentItemTreeDemoNode("Analytics", "Reports, KPIs and operational dashboards", "ChartLine", true,
        [
            new XRecentItemTreeDemoNode("Sales dashboard", "KPI dashboard opened by the sales team", "ChartLine"),
            new XRecentItemTreeDemoNode("Stock aging", "Inventory aging and slow mover analysis", "ProgressCheck"),
            new XRecentItemTreeDemoNode("Audit log", "Recent system changes and audit events", "ShieldCheckOutline"),
        ]),
    ];

    /// <summary>
    /// Gets the recent item shortcuts.
    /// </summary>
    public ObservableCollection<XRecentItemTreeDemoRecentItem> RecentItems { get; } = [];

    /// <summary>
    /// Gets the drop handler used by the internal XTreeView.
    /// </summary>
    public IXTreeViewNodeDropHandler NodeDropHandler { get; }

    /// <summary>
    /// Gets or sets the selected tree node.
    /// </summary>
    [ObservableProperty]
    private XRecentItemTreeDemoNode? _selectedTreeNode;

    /// <summary>
    /// Gets or sets the selected recent item.
    /// </summary>
    [ObservableProperty]
    private XRecentItemTreeDemoRecentItem? _selectedRecentItem;

    /// <summary>
    /// Gets or sets the last user-visible demo action text.
    /// </summary>
    [ObservableProperty]
    private string _lastActionText = string.Empty;

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XRecentItemTree
    Width="600"
    Height="620"
    AllowNodeDragDrop="True"
    ChildrenMemberPath="Children"
    ExpandedMemberPath="IsExpanded"
    NodeDropHandler="{Binding NodeDropHandler}"
    RecentHeader="Zuletzt verwendet"
    RecentItemDescriptionMemberPath="Description"
    RecentItemIsPinnedMemberPath="IsPinned"
    RecentItemTextMemberPath="Title"
    RecentItemToolTipMemberPath="ToolTip"
    OpenRecentItemToolTip="Öffnen"
    PinRecentItemToolTip="Anheften"
    UnpinRecentItemToolTip="Anheftung lösen"
    RemoveRecentItemToolTip="Aus Verlauf entfernen"
    RecentItemsSource="{Binding RecentItems}"
    RemoveRecentItemCommand="{Binding RemoveRecentItemCommand}"
    OpenRecentItemCommand="{Binding OpenRecentItemCommand}"
    SelectedRecentItem="{Binding SelectedRecentItem, Mode=TwoWay}"
    SelectedTreeItem="{Binding SelectedTreeNode, Mode=TwoWay}"
    ShowRootNewButton="True"
    TogglePinRecentItemCommand="{Binding TogglePinRecentItemCommand}"
    TreeHeader="Navigation"
    TreeItemsSource="{Binding NavigationNodes}">
    <via:XRecentItemTree.TreeItemTemplate>
        <HierarchicalDataTemplate ItemsSource="{Binding Children}">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto" />
                    <ColumnDefinition Width="*" />
                </Grid.ColumnDefinitions>

                <via:XMaterialIcon
                    Grid.Column="0"
                    Margin="0,0,8,0"
                    Kind="{Binding IconKind}"
                    Size="15" />

                <TextBlock
                    Grid.Column="1"
                    Text="{Binding Title}" />
            </Grid>
        </HierarchicalDataTemplate>
    </via:XRecentItemTree.TreeItemTemplate>
</via:XRecentItemTree>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public sealed partial class PageViewModel : ObservableObject
{
    public ObservableCollection<NavigationNode> NavigationNodes { get; } = [];
    public ObservableCollection<RecentShortcut> RecentItems { get; } = [];

    public IXTreeViewNodeDropHandler NodeDropHandler { get; }

    [ObservableProperty]
    private NavigationNode? selectedTreeNode;

    [RelayCommand]
    private void OpenRecentItem(RecentShortcut? item)
    {
        if (item?.Data is NavigationNode node)
        {
            this.SelectedTreeNode = node;
        }
    }

    [RelayCommand]
    private void TogglePinRecentItem(RecentShortcut? item)
    {
        if (item is not null)
        {
            item.IsPinned = !item.IsPinned;
        }
    }
}
""";
    #endregion

    #region ### Partial Methods ###
    /// <summary>
    /// Handles selected tree node changes.
    /// </summary>
    /// <param name="value">The new selected tree node.</param>
    partial void OnSelectedTreeNodeChanged(XRecentItemTreeDemoNode? value)
    {
        if (value is null)
        {
            return;
        }

        this.AddOrPromoteRecentItem(value, false);
        this.LastActionText = $"Selected '{value.Title}' and added it to recent items.";
    }
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Opens a recent shortcut and selects its associated tree node.
    /// </summary>
    /// <param name="item">The recent item.</param>
    [RelayCommand]
    private void OpenRecentItem(XRecentItemTreeDemoRecentItem? item)
    {
        if (item?.Data is null)
        {
            return;
        }

        this.SelectedRecentItem = item;
        this.SelectedTreeNode = item.Data;
        this.MoveRecentItemToDisplayPosition(item);
        this.LastActionText = $"Opened recent item '{item.Title}'.";
    }

    /// <summary>
    /// Toggles the pinned state of a recent shortcut.
    /// </summary>
    /// <param name="item">The recent item.</param>
    [RelayCommand]
    private void TogglePinRecentItem(XRecentItemTreeDemoRecentItem? item)
    {
        if (item is null)
        {
            return;
        }

        item.IsPinned = !item.IsPinned;
        this.MoveRecentItemToDisplayPosition(item);
        this.LastActionText = item.IsPinned
            ? $"Pinned '{item.Title}' to the top of the recent list."
            : $"Unpinned '{item.Title}'.";
    }

    /// <summary>
    /// Removes a recent shortcut.
    /// </summary>
    /// <param name="item">The recent item.</param>
    [RelayCommand]
    private void RemoveRecentItem(XRecentItemTreeDemoRecentItem? item)
    {
        if (item is null)
        {
            return;
        }

        this.RecentItems.Remove(item);

        if (ReferenceEquals(this.SelectedRecentItem, item))
        {
            this.SelectedRecentItem = null;
        }

        this.LastActionText = $"Removed '{item.Title}' from recent items.";
    }

    /// <summary>
    /// Adds a sample child node to the selected parent node or to the root collection.
    /// </summary>
    /// <param name="parentNode">The parent node.</param>
    [RelayCommand]
    private void NewTreeItem(XRecentItemTreeDemoNode? parentNode)
    {
        XRecentItemTreeDemoNode newNode = new(
            $"New item {this.createdItemIndex++}",
            "Created from the forwarded XTreeView node command",
            "Plus");

        if (parentNode is null)
        {
            this.NavigationNodes.Add(newNode);
        }
        else
        {
            parentNode.IsExpanded = true;
            parentNode.Children.Add(newNode);
        }

        this.SelectedTreeNode = newNode;
        this.LastActionText = parentNode is null
            ? $"Created root item '{newNode.Title}'."
            : $"Created '{newNode.Title}' below '{parentNode.Title}'.";
    }

    /// <summary>
    /// Marks a tree node as edited.
    /// </summary>
    /// <param name="node">The tree node.</param>
    [RelayCommand]
    private void EditTreeItem(XRecentItemTreeDemoNode? node)
    {
        if (node is null)
        {
            return;
        }

        if (!node.Title.EndsWith(" • edited", StringComparison.Ordinal))
        {
            node.Title += " • edited";
        }

        this.SelectedTreeNode = node;
        this.LastActionText = $"Edited '{node.Title}'.";
        this.UpdateRecentItemText(node);
    }

    /// <summary>
    /// Removes a tree node and its recent shortcut.
    /// </summary>
    /// <param name="node">The tree node.</param>
    [RelayCommand]
    private void DeleteTreeItem(XRecentItemTreeDemoNode? node)
    {
        if (node is null)
        {
            return;
        }

        if (!this.RemoveNode(this.NavigationNodes, node))
        {
            return;
        }

        foreach (XRecentItemTreeDemoRecentItem recentItem in this.RecentItems.Where(item => ReferenceEquals(item.Data, node)).ToList())
        {
            this.RecentItems.Remove(recentItem);
        }

        if (ReferenceEquals(this.SelectedTreeNode, node))
        {
            this.SelectedTreeNode = null;
        }

        this.LastActionText = $"Deleted '{node.Title}'.";
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Adds or promotes a recent shortcut for the specified node.
    /// </summary>
    /// <param name="node">The tree node.</param>
    /// <param name="isPinned">A value indicating whether the item should be pinned.</param>
    private void AddOrPromoteRecentItem(XRecentItemTreeDemoNode node, bool isPinned)
    {
        XRecentItemTreeDemoRecentItem? existingItem = this.RecentItems.FirstOrDefault(item => ReferenceEquals(item.Data, node));

        if (existingItem is null)
        {
            existingItem = new XRecentItemTreeDemoRecentItem(node, isPinned);
            this.RecentItems.Add(existingItem);
        }
        else if (isPinned && !existingItem.IsPinned)
        {
            existingItem.IsPinned = true;
        }

        this.MoveRecentItemToDisplayPosition(existingItem);
        this.TrimRecentItems();
    }

    /// <summary>
    /// Moves a recent item to the correct visual group.
    /// </summary>
    /// <param name="item">The recent item.</param>
    private void MoveRecentItemToDisplayPosition(XRecentItemTreeDemoRecentItem item)
    {
        int oldIndex = this.RecentItems.IndexOf(item);
        if (oldIndex >= 0)
        {
            this.RecentItems.RemoveAt(oldIndex);
        }

        int insertIndex = item.IsPinned
            ? 0
            : this.RecentItems.TakeWhile(static recentItem => recentItem.IsPinned).Count();

        this.RecentItems.Insert(insertIndex, item);
    }

    /// <summary>
    /// Updates a recent shortcut after the node title changed.
    /// </summary>
    /// <param name="node">The edited node.</param>
    private void UpdateRecentItemText(XRecentItemTreeDemoNode node)
    {
        XRecentItemTreeDemoRecentItem? recentItem = this.RecentItems.FirstOrDefault(item => ReferenceEquals(item.Data, node));
        if (recentItem is not null)
        {
            recentItem.Title = node.Title;
            recentItem.ToolTip = $"{node.Title} öffnen";
        }
    }

    /// <summary>
    /// Keeps the recent list compact while preserving pinned items.
    /// </summary>
    private void TrimRecentItems()
    {
        while (this.RecentItems.Count > 8)
        {
            XRecentItemTreeDemoRecentItem? removableItem = this.RecentItems.LastOrDefault(static item => !item.IsPinned);
            if (removableItem is null)
            {
                return;
            }

            this.RecentItems.Remove(removableItem);
        }
    }

    /// <summary>
    /// Removes a node from a mutable hierarchical collection.
    /// </summary>
    /// <param name="items">The current collection.</param>
    /// <param name="node">The node to remove.</param>
    /// <returns><see langword="true"/> if the node was removed; otherwise <see langword="false"/>.</returns>
    private bool RemoveNode(ObservableCollection<XRecentItemTreeDemoNode> items, XRecentItemTreeDemoNode node)
    {
        if (items.Remove(node))
        {
            return true;
        }

        return items.Any(item => this.RemoveNode(item.Children, node));
    }
    #endregion
}
#endregion

#region ### Class XRecentItemTreeDemoNode ###
/// <summary>
/// Represents one tree node in the XRecentItemTree demo.
/// </summary>
public sealed partial class XRecentItemTreeDemoNode : ObservableObject
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XRecentItemTreeDemoNode"/> class.
    /// </summary>
    /// <param name="title">The node title.</param>
    /// <param name="description">The node description.</param>
    /// <param name="iconKind">The Material icon kind name.</param>
    /// <param name="isExpanded">A value indicating whether the node is expanded.</param>
    /// <param name="children">The child nodes.</param>
    public XRecentItemTreeDemoNode(
        string title,
        string description,
        string iconKind,
        bool isExpanded = false,
        IEnumerable<XRecentItemTreeDemoNode>? children = null)
    {
        this.Title = title;
        this.Description = description;
        this.IconKind = iconKind;
        this.IsExpanded = isExpanded;
        this.Children = children is null ? [] : new ObservableCollection<XRecentItemTreeDemoNode>(children);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the node title.
    /// </summary>
    [ObservableProperty]
    private string _title = string.Empty;

    /// <summary>
    /// Gets the node description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the Material icon kind name.
    /// </summary>
    public string IconKind { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is expanded.
    /// </summary>
    [ObservableProperty]
    private bool _isExpanded;

    /// <summary>
    /// Gets the child nodes.
    /// </summary>
    public ObservableCollection<XRecentItemTreeDemoNode> Children { get; }

    /// <summary>
    /// Gets the node tooltip.
    /// </summary>
    public string ToolTip => $"{this.Title} - {this.Description}";
    #endregion
}
#endregion

#region ### Class XRecentItemTreeDemoRecentItem ###
/// <summary>
/// Represents one recent shortcut in the XRecentItemTree demo.
/// </summary>
public sealed partial class XRecentItemTreeDemoRecentItem : ObservableObject
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XRecentItemTreeDemoRecentItem"/> class.
    /// </summary>
    /// <param name="node">The associated tree node.</param>
    /// <param name="isPinned">A value indicating whether the shortcut is pinned.</param>
    public XRecentItemTreeDemoRecentItem(XRecentItemTreeDemoNode node, bool isPinned)
    {
        this.Data = node;
        this.Title = node.Title;
        this.Description = node.Description;
        this.IconKind = node.IconKind;
        this.ToolTip = $"{node.Title} öffnen";
        this.IsPinned = isPinned;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the associated tree node.
    /// </summary>
    public XRecentItemTreeDemoNode Data { get; }

    /// <summary>
    /// Gets or sets the recent item title.
    /// </summary>
    [ObservableProperty]
    private string _title = string.Empty;

    /// <summary>
    /// Gets the recent item description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the Material icon kind name.
    /// </summary>
    public string IconKind { get; }

    /// <summary>
    /// Gets or sets the recent item tooltip.
    /// </summary>
    [ObservableProperty]
    private string _toolTip = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this recent item is pinned.
    /// </summary>
    [ObservableProperty]
    private bool _isPinned;
    #endregion
}
#endregion
