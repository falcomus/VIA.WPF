// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRecentItemTree.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XRecentItemTree ###
/// <summary>
/// Represents a composite navigation control that shows a primary <see cref="XTreeView"/> and a separate recent-items area.
/// </summary>
public class XRecentItemTree : Control
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="TreeItemsSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeItemsSourceProperty = DependencyProperty.Register(
        nameof(TreeItemsSource),
        typeof(IEnumerable),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RecentItemsSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemsSourceProperty = DependencyProperty.Register(
        nameof(RecentItemsSource),
        typeof(IEnumerable),
        typeof(XRecentItemTree),
        new PropertyMetadata(null, OnRecentItemsSourceChanged));

    /// <summary>
    /// Identifies the <see cref="TreeItemTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeItemTemplateProperty = DependencyProperty.Register(
        nameof(TreeItemTemplate),
        typeof(DataTemplate),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TreeItemTemplateSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeItemTemplateSelectorProperty = DependencyProperty.Register(
        nameof(TreeItemTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RecentItemTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemTemplateProperty = DependencyProperty.Register(
        nameof(RecentItemTemplate),
        typeof(DataTemplate),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RecentItemTextMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemTextMemberPathProperty = DependencyProperty.Register(
        nameof(RecentItemTextMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("Text"));

    /// <summary>
    /// Identifies the <see cref="RecentItemDescriptionMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemDescriptionMemberPathProperty = DependencyProperty.Register(
        nameof(RecentItemDescriptionMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("Description"));

    /// <summary>
    /// Identifies the <see cref="RecentItemIconMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemIconMemberPathProperty = DependencyProperty.Register(
        nameof(RecentItemIconMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("Icon"));

    /// <summary>
    /// Identifies the <see cref="RecentItemToolTipMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemToolTipMemberPathProperty = DependencyProperty.Register(
        nameof(RecentItemToolTipMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("ToolTip"));

    /// <summary>
    /// Identifies the <see cref="RecentItemIsPinnedMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemIsPinnedMemberPathProperty = DependencyProperty.Register(
        nameof(RecentItemIsPinnedMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("IsPinned"));

    /// <summary>
    /// Identifies the <see cref="SelectedTreeItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedTreeItemProperty = DependencyProperty.Register(
        nameof(SelectedTreeItem),
        typeof(object),
        typeof(XRecentItemTree),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="SelectedRecentItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedRecentItemProperty = DependencyProperty.Register(
        nameof(SelectedRecentItem),
        typeof(object),
        typeof(XRecentItemTree),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="TreeHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeHeaderProperty = DependencyProperty.Register(
        nameof(TreeHeader),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Navigation"));

    /// <summary>
    /// Identifies the <see cref="RecentHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentHeaderProperty = DependencyProperty.Register(
        nameof(RecentHeader),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Zuletzt verwendet"));

    /// <summary>
    /// Identifies the <see cref="TreeHeaderToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeHeaderToolTipProperty = DependencyProperty.Register(
        nameof(TreeHeaderToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Hauptnavigation"));

    /// <summary>
    /// Identifies the <see cref="RecentHeaderToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentHeaderToolTipProperty = DependencyProperty.Register(
        nameof(RecentHeaderToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Zuletzt bearbeitete oder angesehene Elemente"));

    /// <summary>
    /// Identifies the <see cref="ShowRecentItems"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowRecentItemsProperty = DependencyProperty.Register(
        nameof(ShowRecentItems),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="RecentItemsMaxHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RecentItemsMaxHeightProperty = DependencyProperty.Register(
        nameof(RecentItemsMaxHeight),
        typeof(double),
        typeof(XRecentItemTree),
        new PropertyMetadata(220d));

    /// <summary>
    /// Identifies the <see cref="ChildrenMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ChildrenMemberPathProperty = DependencyProperty.Register(
        nameof(ChildrenMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("Children"));

    /// <summary>
    /// Identifies the <see cref="ExpandedMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExpandedMemberPathProperty = DependencyProperty.Register(
        nameof(ExpandedMemberPath),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XRecentItemTree),
        new PropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowRootNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowRootNewButtonProperty = DependencyProperty.Register(
        nameof(ShowRootNewButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ShowExpandAllButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowExpandAllButtonProperty = DependencyProperty.Register(
        nameof(ShowExpandAllButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowCollapseAllButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCollapseAllButtonProperty = DependencyProperty.Register(
        nameof(ShowCollapseAllButton),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="NewItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewItemCommandProperty = DependencyProperty.Register(
        nameof(NewItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditItemCommandProperty = DependencyProperty.Register(
        nameof(EditItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteItemCommandProperty = DependencyProperty.Register(
        nameof(DeleteItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="AllowNodeDragDrop"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AllowNodeDragDropProperty = DependencyProperty.Register(
        nameof(AllowNodeDragDrop),
        typeof(bool),
        typeof(XRecentItemTree),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="NodeDropMode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodeDropModeProperty = DependencyProperty.Register(
        nameof(NodeDropMode),
        typeof(XTreeViewNodeDropMode),
        typeof(XRecentItemTree),
        new PropertyMetadata(XTreeViewNodeDropMode.BeforeAfterInto));

    /// <summary>
    /// Identifies the <see cref="NodeDropHandler"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodeDropHandlerProperty = DependencyProperty.Register(
        nameof(NodeDropHandler),
        typeof(IXTreeViewNodeDropHandler),
        typeof(XRecentItemTree),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="OpenRecentItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OpenRecentItemCommandProperty = DependencyProperty.Register(
        nameof(OpenRecentItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null, OnRecentCommandChanged));

    /// <summary>
    /// Identifies the <see cref="TogglePinRecentItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TogglePinRecentItemCommandProperty = DependencyProperty.Register(
        nameof(TogglePinRecentItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null, OnRecentCommandChanged));

    /// <summary>
    /// Identifies the <see cref="RemoveRecentItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RemoveRecentItemCommandProperty = DependencyProperty.Register(
        nameof(RemoveRecentItemCommand),
        typeof(ICommand),
        typeof(XRecentItemTree),
        new PropertyMetadata(null, OnRecentCommandChanged));

    /// <summary>
    /// Identifies the <see cref="OpenRecentItemToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OpenRecentItemToolTipProperty = DependencyProperty.Register(
        nameof(OpenRecentItemToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Öffnen"));

    /// <summary>
    /// Identifies the <see cref="PinRecentItemToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PinRecentItemToolTipProperty = DependencyProperty.Register(
        nameof(PinRecentItemToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Anheften"));

    /// <summary>
    /// Identifies the <see cref="UnpinRecentItemToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UnpinRecentItemToolTipProperty = DependencyProperty.Register(
        nameof(UnpinRecentItemToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Pin lösen"));

    /// <summary>
    /// Identifies the <see cref="RemoveRecentItemToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RemoveRecentItemToolTipProperty = DependencyProperty.Register(
        nameof(RemoveRecentItemToolTip),
        typeof(object),
        typeof(XRecentItemTree),
        new PropertyMetadata("Aus Verlauf entfernen"));

    /// <summary>
    /// Identifies the <see cref="DropHintBeforeText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintBeforeTextProperty = DependencyProperty.Register(
        nameof(DropHintBeforeText),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("Before"));

    /// <summary>
    /// Identifies the <see cref="DropHintIntoText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintIntoTextProperty = DependencyProperty.Register(
        nameof(DropHintIntoText),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("In"));

    /// <summary>
    /// Identifies the <see cref="DropHintAfterText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintAfterTextProperty = DependencyProperty.Register(
        nameof(DropHintAfterText),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("After"));

    /// <summary>
    /// Identifies the <see cref="DropHintRootText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintRootTextProperty = DependencyProperty.Register(
        nameof(DropHintRootText),
        typeof(string),
        typeof(XRecentItemTree),
        new PropertyMetadata("As Main Category"));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The open recent proxy command.
    /// </summary>
    private readonly RecentItemCommand openRecentItemProxyCommand;

    /// <summary>
    /// The toggle pin recent proxy command.
    /// </summary>
    private readonly RecentItemCommand togglePinRecentItemProxyCommand;

    /// <summary>
    /// The remove recent proxy command.
    /// </summary>
    private readonly RecentItemCommand removeRecentItemProxyCommand;

    /// <summary>
    /// Gets or sets the drop hint prefix for drops before a node.
    /// </summary>
    public string DropHintBeforeText
    {
        get => (string)this.GetValue(DropHintBeforeTextProperty);
        set => this.SetValue(DropHintBeforeTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the drop hint prefix for drops into a node.
    /// </summary>
    public string DropHintIntoText
    {
        get => (string)this.GetValue(DropHintIntoTextProperty);
        set => this.SetValue(DropHintIntoTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the drop hint prefix for drops after a node.
    /// </summary>
    public string DropHintAfterText
    {
        get => (string)this.GetValue(DropHintAfterTextProperty);
        set => this.SetValue(DropHintAfterTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the drop hint text for root drops.
    /// </summary>
    public string DropHintRootText
    {
        get => (string)this.GetValue(DropHintRootTextProperty);
        set => this.SetValue(DropHintRootTextProperty, value);
    }
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XRecentItemTree"/> class.
    /// </summary>
    static XRecentItemTree()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XRecentItemTree),
            new FrameworkPropertyMetadata(typeof(XRecentItemTree)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XRecentItemTree"/> class.
    /// </summary>
    public XRecentItemTree()
    {
        this.openRecentItemProxyCommand = new RecentItemCommand(this.CanOpenRecentItem, this.OpenRecentItem);
        this.togglePinRecentItemProxyCommand = new RecentItemCommand(this.CanTogglePinRecentItem, this.TogglePinRecentItem);
        this.removeRecentItemProxyCommand = new RecentItemCommand(this.CanRemoveRecentItem, this.RemoveRecentItem);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the command used by the template to open a recent item.
    /// </summary>
    public ICommand OpenRecentItemProxyCommand => this.openRecentItemProxyCommand;

    /// <summary>
    /// Gets the command used by the template to toggle the pinned state of a recent item.
    /// </summary>
    public ICommand TogglePinRecentItemProxyCommand => this.togglePinRecentItemProxyCommand;

    /// <summary>
    /// Gets the command used by the template to remove a recent item.
    /// </summary>
    public ICommand RemoveRecentItemProxyCommand => this.removeRecentItemProxyCommand;

    /// <summary>
    /// Gets or sets the primary tree items source.
    /// </summary>
    public IEnumerable? TreeItemsSource
    {
        get => (IEnumerable?)this.GetValue(TreeItemsSourceProperty);
        set => this.SetValue(TreeItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the recent items source.
    /// </summary>
    public IEnumerable? RecentItemsSource
    {
        get => (IEnumerable?)this.GetValue(RecentItemsSourceProperty);
        set => this.SetValue(RecentItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the item template of the primary tree.
    /// </summary>
    public DataTemplate? TreeItemTemplate
    {
        get => (DataTemplate?)this.GetValue(TreeItemTemplateProperty);
        set => this.SetValue(TreeItemTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the item template selector of the primary tree.
    /// </summary>
    public DataTemplateSelector? TreeItemTemplateSelector
    {
        get => (DataTemplateSelector?)this.GetValue(TreeItemTemplateSelectorProperty);
        set => this.SetValue(TreeItemTemplateSelectorProperty, value);
    }

    /// <summary>
    /// Gets or sets the content template used inside recent item rows.
    /// </summary>
    public DataTemplate? RecentItemTemplate
    {
        get => (DataTemplate?)this.GetValue(RecentItemTemplateProperty);
        set => this.SetValue(RecentItemTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path used as recent item display text.
    /// </summary>
    public string RecentItemTextMemberPath
    {
        get => (string)this.GetValue(RecentItemTextMemberPathProperty);
        set => this.SetValue(RecentItemTextMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path used as recent item description.
    /// </summary>
    public string RecentItemDescriptionMemberPath
    {
        get => (string)this.GetValue(RecentItemDescriptionMemberPathProperty);
        set => this.SetValue(RecentItemDescriptionMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path used as recent item icon.
    /// </summary>
    public string RecentItemIconMemberPath
    {
        get => (string)this.GetValue(RecentItemIconMemberPathProperty);
        set => this.SetValue(RecentItemIconMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path used as recent item tooltip.
    /// </summary>
    public string RecentItemToolTipMemberPath
    {
        get => (string)this.GetValue(RecentItemToolTipMemberPathProperty);
        set => this.SetValue(RecentItemToolTipMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path used as recent item pinned state.
    /// </summary>
    public string RecentItemIsPinnedMemberPath
    {
        get => (string)this.GetValue(RecentItemIsPinnedMemberPathProperty);
        set => this.SetValue(RecentItemIsPinnedMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected tree item.
    /// </summary>
    public object? SelectedTreeItem
    {
        get => this.GetValue(SelectedTreeItemProperty);
        set => this.SetValue(SelectedTreeItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected recent item.
    /// </summary>
    public object? SelectedRecentItem
    {
        get => this.GetValue(SelectedRecentItemProperty);
        set => this.SetValue(SelectedRecentItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the tree section header.
    /// </summary>
    public object? TreeHeader
    {
        get => this.GetValue(TreeHeaderProperty);
        set => this.SetValue(TreeHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the recent section header.
    /// </summary>
    public object? RecentHeader
    {
        get => this.GetValue(RecentHeaderProperty);
        set => this.SetValue(RecentHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the tree section header tooltip.
    /// </summary>
    public object? TreeHeaderToolTip
    {
        get => this.GetValue(TreeHeaderToolTipProperty);
        set => this.SetValue(TreeHeaderToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the recent section header tooltip.
    /// </summary>
    public object? RecentHeaderToolTip
    {
        get => this.GetValue(RecentHeaderToolTipProperty);
        set => this.SetValue(RecentHeaderToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the recent-items section is visible.
    /// </summary>
    public bool ShowRecentItems
    {
        get => (bool)this.GetValue(ShowRecentItemsProperty);
        set => this.SetValue(ShowRecentItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum height of the recent-items section.
    /// </summary>
    public double RecentItemsMaxHeight
    {
        get => (double)this.GetValue(RecentItemsMaxHeightProperty);
        set => this.SetValue(RecentItemsMaxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path that contains child nodes in the tree.
    /// </summary>
    public string ChildrenMemberPath
    {
        get => (string)this.GetValue(ChildrenMemberPathProperty);
        set => this.SetValue(ChildrenMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the member path that stores the expanded state in the tree.
    /// </summary>
    public string ExpandedMemberPath
    {
        get => (string)this.GetValue(ExpandedMemberPathProperty);
        set => this.SetValue(ExpandedMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual size of the tree and recent actions.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether new-node buttons are visible in the tree.
    /// </summary>
    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether edit-node buttons are visible in the tree.
    /// </summary>
    public bool ShowEditButton
    {
        get => (bool)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether delete-node buttons are visible in the tree.
    /// </summary>
    public bool ShowDeleteButton
    {
        get => (bool)this.GetValue(ShowDeleteButtonProperty);
        set => this.SetValue(ShowDeleteButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the root new button is visible in the tree.
    /// </summary>
    public bool ShowRootNewButton
    {
        get => (bool)this.GetValue(ShowRootNewButtonProperty);
        set => this.SetValue(ShowRootNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the expand-all button is visible in the tree.
    /// </summary>
    public bool ShowExpandAllButton
    {
        get => (bool)this.GetValue(ShowExpandAllButtonProperty);
        set => this.SetValue(ShowExpandAllButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the collapse-all button is visible in the tree.
    /// </summary>
    public bool ShowCollapseAllButton
    {
        get => (bool)this.GetValue(ShowCollapseAllButtonProperty);
        set => this.SetValue(ShowCollapseAllButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the command used to create a tree item.
    /// </summary>
    public ICommand? NewItemCommand
    {
        get => (ICommand?)this.GetValue(NewItemCommandProperty);
        set => this.SetValue(NewItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command used to edit a tree item.
    /// </summary>
    public ICommand? EditItemCommand
    {
        get => (ICommand?)this.GetValue(EditItemCommandProperty);
        set => this.SetValue(EditItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command used to delete a tree item.
    /// </summary>
    public ICommand? DeleteItemCommand
    {
        get => (ICommand?)this.GetValue(DeleteItemCommandProperty);
        set => this.SetValue(DeleteItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether tree nodes can be reordered by drag-and-drop.
    /// </summary>
    public bool AllowNodeDragDrop
    {
        get => (bool)this.GetValue(AllowNodeDragDropProperty);
        set => this.SetValue(AllowNodeDragDropProperty, value);
    }

    /// <summary>
    /// Gets or sets the node drop mode used by the primary tree.
    /// </summary>
    public XTreeViewNodeDropMode NodeDropMode
    {
        get => (XTreeViewNodeDropMode)this.GetValue(NodeDropModeProperty);
        set => this.SetValue(NodeDropModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the custom node drop handler used by the primary tree.
    /// </summary>
    public IXTreeViewNodeDropHandler? NodeDropHandler
    {
        get => (IXTreeViewNodeDropHandler?)this.GetValue(NodeDropHandlerProperty);
        set => this.SetValue(NodeDropHandlerProperty, value);
    }

    /// <summary>
    /// Gets or sets the external command used to open a recent item.
    /// </summary>
    public ICommand? OpenRecentItemCommand
    {
        get => (ICommand?)this.GetValue(OpenRecentItemCommandProperty);
        set => this.SetValue(OpenRecentItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the external command used to toggle the pinned state of a recent item.
    /// </summary>
    public ICommand? TogglePinRecentItemCommand
    {
        get => (ICommand?)this.GetValue(TogglePinRecentItemCommandProperty);
        set => this.SetValue(TogglePinRecentItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the external command used to remove a recent item.
    /// </summary>
    public ICommand? RemoveRecentItemCommand
    {
        get => (ICommand?)this.GetValue(RemoveRecentItemCommandProperty);
        set => this.SetValue(RemoveRecentItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip for opening a recent item.
    /// </summary>
    public object? OpenRecentItemToolTip
    {
        get => this.GetValue(OpenRecentItemToolTipProperty);
        set => this.SetValue(OpenRecentItemToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip for pinning a recent item.
    /// </summary>
    public object? PinRecentItemToolTip
    {
        get => this.GetValue(PinRecentItemToolTipProperty);
        set => this.SetValue(PinRecentItemToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip for unpinning a recent item.
    /// </summary>
    public object? UnpinRecentItemToolTip
    {
        get => this.GetValue(UnpinRecentItemToolTipProperty);
        set => this.SetValue(UnpinRecentItemToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip for removing a recent item.
    /// </summary>
    public object? RemoveRecentItemToolTip
    {
        get => this.GetValue(RemoveRecentItemToolTipProperty);
        set => this.SetValue(RemoveRecentItemToolTipProperty, value);
    }

    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles command dependency property changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnRecentCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XRecentItemTree tree)
        {
            tree.RaiseRecentCommandCanExecuteChanged();
        }
    }

    /// <summary>
    /// Handles recent item source changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnRecentItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XRecentItemTree tree)
        {
            tree.RaiseRecentCommandCanExecuteChanged();
        }
    }

    /// <summary>
    /// Raises <see cref="ICommand.CanExecuteChanged"/> for all recent proxy commands.
    /// </summary>
    private void RaiseRecentCommandCanExecuteChanged()
    {
        this.openRecentItemProxyCommand.RaiseCanExecuteChanged();
        this.togglePinRecentItemProxyCommand.RaiseCanExecuteChanged();
        this.removeRecentItemProxyCommand.RaiseCanExecuteChanged();
    }

    /// <summary>
    /// Determines whether a recent item can be opened.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    /// <returns><see langword="true"/> if the item can be opened; otherwise <see langword="false"/>.</returns>
    private bool CanOpenRecentItem(object? parameter)
    {
        return parameter is not null && CanExecuteCommand(this.OpenRecentItemCommand, parameter, true);
    }

    /// <summary>
    /// Opens a recent item.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    private void OpenRecentItem(object? parameter)
    {
        if (parameter is null)
        {
            return;
        }

        this.SelectedRecentItem = parameter;
        ExecuteCommand(this.OpenRecentItemCommand, parameter);
    }

    /// <summary>
    /// Determines whether a recent item pin can be toggled.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    /// <returns><see langword="true"/> if the item pin can be toggled; otherwise <see langword="false"/>.</returns>
    private bool CanTogglePinRecentItem(object? parameter)
    {
        return this.TogglePinRecentItemCommand is null
            ? parameter is XRecentItem
            : CanExecuteCommand(this.TogglePinRecentItemCommand, parameter, false);
    }

    /// <summary>
    /// Toggles a recent item pin.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    private void TogglePinRecentItem(object? parameter)
    {
        if (CanExecuteCommand(this.TogglePinRecentItemCommand, parameter, false))
        {
            ExecuteCommand(this.TogglePinRecentItemCommand, parameter);
            this.RefreshRecentItemsView();
            return;
        }

        if (parameter is XRecentItem recentItem)
        {
            recentItem.IsPinned = !recentItem.IsPinned;
            this.RefreshRecentItemsView();
        }
    }

    /// <summary>
    /// Determines whether a recent item can be removed.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    /// <returns><see langword="true"/> if the item can be removed; otherwise <see langword="false"/>.</returns>
    private bool CanRemoveRecentItem(object? parameter)
    {
        if (this.RemoveRecentItemCommand is not null)
        {
            return CanExecuteCommand(this.RemoveRecentItemCommand, parameter, false);
        }

        return parameter is not null && this.RecentItemsSource is IList list && !list.IsReadOnly && list.Contains(parameter);
    }

    /// <summary>
    /// Removes a recent item.
    /// </summary>
    /// <param name="parameter">The recent item.</param>
    private void RemoveRecentItem(object? parameter)
    {
        if (this.RemoveRecentItemCommand is not null)
        {
            ExecuteCommand(this.RemoveRecentItemCommand, parameter);
            this.RefreshRecentItemsView();
            return;
        }

        if (parameter is not null && this.RecentItemsSource is IList list && !list.IsReadOnly && list.Contains(parameter))
        {
            list.Remove(parameter);
            this.RefreshRecentItemsView();
        }
    }

    /// <summary>
    /// Refreshes the current recent items view.
    /// </summary>
    private void RefreshRecentItemsView()
    {
        if (this.RecentItemsSource is not null)
        {
            CollectionViewSource.GetDefaultView(this.RecentItemsSource)?.Refresh();
        }
    }

    /// <summary>
    /// Determines whether a command can be executed.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    /// <param name="defaultWhenCommandMissing">The value returned when the command is missing.</param>
    /// <returns><see langword="true"/> if the command can be executed; otherwise <see langword="false"/>.</returns>
    private static bool CanExecuteCommand(ICommand? command, object? parameter, bool defaultWhenCommandMissing)
    {
        return command is null ? defaultWhenCommandMissing : command.CanExecute(parameter);
    }

    /// <summary>
    /// Executes a command if it can be executed.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    private static void ExecuteCommand(ICommand? command, object? parameter)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command.Execute(parameter);
        }
    }
    #endregion

    #region ### Class RecentItemCommand ###
    /// <summary>
    /// Represents an internal recent item command.
    /// </summary>
    /// <param name="canExecute">The can execute callback.</param>
    /// <param name="execute">The execute callback.</param>
    private sealed class RecentItemCommand(Func<object?, bool> canExecute, Action<object?> execute) : ICommand
    {
        #region ### Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return canExecute(parameter);
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            execute(parameter);
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/>.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
    #endregion
}
#endregion
