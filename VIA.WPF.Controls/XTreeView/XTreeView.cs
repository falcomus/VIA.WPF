// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeView.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

#region ### Class XTreeView ###
/// <summary>
/// Represents a themed hierarchical tree view with optional node action commands, bindable selected data item, node preview and optional node drag-and-drop.
/// </summary>
public class XTreeView : TreeView
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="SelectedDataItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedDataItemProperty = DependencyProperty.Register(
        nameof(SelectedDataItem),
        typeof(object),
        typeof(XTreeView),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnSelectedDataItemChanged));

    /// <summary>
    /// Identifies the <see cref="HoveredDataItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HoveredDataItemProperty = DependencyProperty.Register(
        nameof(HoveredDataItem),
        typeof(object),
        typeof(XTreeView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the read-only <see cref="IsNodeDragInProgress"/> dependency property key.
    /// </summary>
    private static readonly DependencyPropertyKey IsNodeDragInProgressPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsNodeDragInProgress),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsNodeDragInProgress"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsNodeDragInProgressProperty = IsNodeDragInProgressPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowRootNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowRootNewButtonProperty = DependencyProperty.Register(
        nameof(ShowRootNewButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="RootNewButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RootNewButtonTextProperty = DependencyProperty.Register(
        nameof(RootNewButtonText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("Hauptkategorie"));

    /// <summary>
    /// Identifies the <see cref="ShowExpandAllButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowExpandAllButtonProperty = DependencyProperty.Register(
        nameof(ShowExpandAllButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowCollapseAllButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCollapseAllButtonProperty = DependencyProperty.Register(
        nameof(ShowCollapseAllButton),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ExpandAllButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExpandAllButtonTextProperty = DependencyProperty.Register(
        nameof(ExpandAllButtonText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("Alle erweitern"));

    /// <summary>
    /// Identifies the <see cref="CollapseAllButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CollapseAllButtonTextProperty = DependencyProperty.Register(
        nameof(CollapseAllButtonText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("Alle reduzieren"));

    /// <summary>
    /// Identifies the <see cref="NewItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewItemCommandProperty = DependencyProperty.Register(
        nameof(NewItemCommand),
        typeof(ICommand),
        typeof(XTreeView),
        new PropertyMetadata(null, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="EditItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditItemCommandProperty = DependencyProperty.Register(
        nameof(EditItemCommand),
        typeof(ICommand),
        typeof(XTreeView),
        new PropertyMetadata(null, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DeleteItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteItemCommandProperty = DependencyProperty.Register(
        nameof(DeleteItemCommand),
        typeof(ICommand),
        typeof(XTreeView),
        new PropertyMetadata(null, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ExpandedMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExpandedMemberPathProperty = DependencyProperty.Register(
        nameof(ExpandedMemberPath),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata(string.Empty, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ChildrenMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ChildrenMemberPathProperty = DependencyProperty.Register(
        nameof(ChildrenMemberPath),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("Children", OnNodeDragDropConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="NodeCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodeCornerRadiusProperty = DependencyProperty.Register(
        nameof(NodeCornerRadius),
        typeof(CornerRadius),
        typeof(XTreeView),
        new PropertyMetadata(new CornerRadius(4d)));

    /// <summary>
    /// Identifies the <see cref="NodePadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodePaddingProperty = DependencyProperty.Register(
        nameof(NodePadding),
        typeof(Thickness),
        typeof(XTreeView),
        new PropertyMetadata(new Thickness(6d, 4d, 8d, 4d)));

    /// <summary>
    /// Identifies the <see cref="ItemSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register(
        nameof(ItemSpacing),
        typeof(double),
        typeof(XTreeView),
        new PropertyMetadata(2d, OnOwnerConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XTreeView),
        new PropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="AllowNodeDragDrop"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AllowNodeDragDropProperty = DependencyProperty.Register(
        nameof(AllowNodeDragDrop),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(false, OnNodeDragDropConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="NodeDropMode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodeDropModeProperty = DependencyProperty.Register(
        nameof(NodeDropMode),
        typeof(XTreeViewNodeDropMode),
        typeof(XTreeView),
        new PropertyMetadata(XTreeViewNodeDropMode.BeforeAfterInto));

    /// <summary>
    /// Identifies the <see cref="NodeDropHandler"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NodeDropHandlerProperty = DependencyProperty.Register(
        nameof(NodeDropHandler),
        typeof(IXTreeViewNodeDropHandler),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="UseDefaultDragAdorner"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UseDefaultDragAdornerProperty = DependencyProperty.Register(
        nameof(UseDefaultDragAdorner),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DefaultDragAdornerOpacity"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DefaultDragAdornerOpacityProperty = DependencyProperty.Register(
        nameof(DefaultDragAdornerOpacity),
        typeof(double),
        typeof(XTreeView),
        new PropertyMetadata(0.9d, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DragMouseAnchorPoint"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DragMouseAnchorPointProperty = DependencyProperty.Register(
        nameof(DragMouseAnchorPoint),
        typeof(Point),
        typeof(XTreeView),
        new PropertyMetadata(new Point(0d, 0d), OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DragAdornerTranslation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DragAdornerTranslationProperty = DependencyProperty.Register(
        nameof(DragAdornerTranslation),
        typeof(Point),
        typeof(XTreeView),
        new PropertyMetadata(new Point(12d, 12d), OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="EffectAdornerTranslation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectAdornerTranslationProperty = DependencyProperty.Register(
        nameof(EffectAdornerTranslation),
        typeof(Point),
        typeof(XTreeView),
        new PropertyMetadata(new Point(16d, 16d), OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DragAdornerTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DragAdornerTemplateProperty = DependencyProperty.Register(
        nameof(DragAdornerTemplate),
        typeof(DataTemplate),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DragAdornerTemplateSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DragAdornerTemplateSelectorProperty = DependencyProperty.Register(
        nameof(DragAdornerTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowAlwaysDropTargetAdorner"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowAlwaysDropTargetAdornerProperty = DependencyProperty.Register(
        nameof(ShowAlwaysDropTargetAdorner),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DropTargetAdornerBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropTargetAdornerBrushProperty = DependencyProperty.Register(
        nameof(DropTargetAdornerBrush),
        typeof(Brush),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DropTargetAdornerPen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropTargetAdornerPenProperty = DependencyProperty.Register(
        nameof(DropTargetAdornerPen),
        typeof(Pen),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DropTargetHighlightBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropTargetHighlightBrushProperty = DependencyProperty.Register(
        nameof(DropTargetHighlightBrush),
        typeof(Brush),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="UseDropTargetHint"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UseDropTargetHintProperty = DependencyProperty.Register(
        nameof(UseDropTargetHint),
        typeof(bool),
        typeof(XTreeView),
        new PropertyMetadata(true, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DropHintDataTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintDataTemplateProperty = DependencyProperty.Register(
        nameof(DropHintDataTemplate),
        typeof(DataTemplate),
        typeof(XTreeView),
        new PropertyMetadata(null, OnNodeDragDropVisualConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="DropHintBeforeText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintBeforeTextProperty = DependencyProperty.Register(
        nameof(DropHintBeforeText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("Before"));

    /// <summary>
    /// Identifies the <see cref="DropHintIntoText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintIntoTextProperty = DependencyProperty.Register(
        nameof(DropHintIntoText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("In"));

    /// <summary>
    /// Identifies the <see cref="DropHintAfterText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintAfterTextProperty = DependencyProperty.Register(
        nameof(DropHintAfterText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("After"));

    /// <summary>
    /// Identifies the <see cref="DropHintRootText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropHintRootTextProperty = DependencyProperty.Register(
        nameof(DropHintRootText),
        typeof(string),
        typeof(XTreeView),
        new PropertyMetadata("As Main Category"));

    #endregion

    #region ### Private Fields ###

    /// <summary>
    /// Indicates whether the selected data item is currently synchronized internally.
    /// </summary>
    private bool isSynchronizingSelectedDataItem;

    /// <summary>
    /// Indicates whether a delayed selection synchronization is already scheduled.
    /// </summary>
    private bool isSelectionSynchronizationScheduled;

    /// <summary>
    /// The internal native node drag-and-drop controller.
    /// </summary>
    private readonly XTreeViewNodeDragDropController nodeDragDropController;

    /// <summary>
    /// The default reflection based node drop handler.
    /// </summary>
    private IXTreeViewNodeDropHandler? defaultNodeDropHandler;

    /// <summary>
    /// The current template scroll viewer if available.
    /// </summary>
    private ScrollViewer? scrollViewer;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTreeView"/> class.
    /// </summary>
    static XTreeView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTreeView),
            new FrameworkPropertyMetadata(typeof(XTreeView)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XTreeView"/> class.
    /// </summary>
    public XTreeView()
    {
        this.nodeDragDropController = new XTreeViewNodeDragDropController(this);
        this.ExpandAllCommand = new XTreeViewCommand(_ => this.ExpandAll());
        this.CollapseAllCommand = new XTreeViewCommand(_ => this.CollapseAll());
        this.ItemContainerGenerator.StatusChanged += this.OnItemContainerGeneratorStatusChanged;
        this.Loaded += this.OnLoaded;
        this.UpdateNodeDragDropConfiguration();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the command that expands all tree nodes.
    /// </summary>
    public ICommand ExpandAllCommand { get; }

    /// <summary>
    /// Gets the command that collapses all tree nodes.
    /// </summary>
    public ICommand CollapseAllCommand { get; }

    /// <summary>
    /// Gets or sets the currently selected data item.
    /// </summary>
    public object? SelectedDataItem
    {
        get => this.GetValue(SelectedDataItemProperty);
        set => this.SetValue(SelectedDataItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the currently hovered data item.
    /// </summary>
    public object? HoveredDataItem
    {
        get => this.GetValue(HoveredDataItemProperty);
        set => this.SetValue(HoveredDataItemProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether a node drag operation is currently active over this tree.
    /// </summary>
    public bool IsNodeDragInProgress => (bool)this.GetValue(IsNodeDragInProgressProperty);

    /// <summary>
    /// Gets or sets a value indicating whether the new button is shown on nodes.
    /// </summary>
    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit button is shown on nodes.
    /// </summary>
    public bool ShowEditButton
    {
        get => (bool)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the delete button is shown on nodes.
    /// </summary>
    public bool ShowDeleteButton
    {
        get => (bool)this.GetValue(ShowDeleteButtonProperty);
        set => this.SetValue(ShowDeleteButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a root-level new button is shown above the tree.
    /// </summary>
    public bool ShowRootNewButton
    {
        get => (bool)this.GetValue(ShowRootNewButtonProperty);
        set => this.SetValue(ShowRootNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the text of the root-level new button.
    /// </summary>
    public string RootNewButtonText
    {
        get => (string)this.GetValue(RootNewButtonTextProperty);
        set => this.SetValue(RootNewButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the expand-all button is shown.
    /// </summary>
    public bool ShowExpandAllButton
    {
        get => (bool)this.GetValue(ShowExpandAllButtonProperty);
        set => this.SetValue(ShowExpandAllButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the collapse-all button is shown.
    /// </summary>
    public bool ShowCollapseAllButton
    {
        get => (bool)this.GetValue(ShowCollapseAllButtonProperty);
        set => this.SetValue(ShowCollapseAllButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the text of the expand-all button.
    /// </summary>
    public string ExpandAllButtonText
    {
        get => (string)this.GetValue(ExpandAllButtonTextProperty);
        set => this.SetValue(ExpandAllButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text of the collapse-all button.
    /// </summary>
    public string CollapseAllButtonText
    {
        get => (string)this.GetValue(CollapseAllButtonTextProperty);
        set => this.SetValue(CollapseAllButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the new action is invoked for a node.
    /// </summary>
    public ICommand? NewItemCommand
    {
        get => (ICommand?)this.GetValue(NewItemCommandProperty);
        set => this.SetValue(NewItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the edit action is invoked for a node.
    /// </summary>
    public ICommand? EditItemCommand
    {
        get => (ICommand?)this.GetValue(EditItemCommandProperty);
        set => this.SetValue(EditItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the delete action is invoked for a node.
    /// </summary>
    public ICommand? DeleteItemCommand
    {
        get => (ICommand?)this.GetValue(DeleteItemCommandProperty);
        set => this.SetValue(DeleteItemCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the data member path used to persist node expansion state.
    /// </summary>
    public string ExpandedMemberPath
    {
        get => (string)this.GetValue(ExpandedMemberPathProperty);
        set => this.SetValue(ExpandedMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the data member path used to read child node collections.
    /// </summary>
    public string ChildrenMemberPath
    {
        get => (string)this.GetValue(ChildrenMemberPathProperty);
        set => this.SetValue(ChildrenMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius applied to node surfaces.
    /// </summary>
    public CornerRadius NodeCornerRadius
    {
        get => (CornerRadius)this.GetValue(NodeCornerRadiusProperty);
        set => this.SetValue(NodeCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to node headers.
    /// </summary>
    public Thickness NodePadding
    {
        get => (Thickness)this.GetValue(NodePaddingProperty);
        set => this.SetValue(NodePaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical spacing between sibling items.
    /// </summary>
    public double ItemSpacing
    {
        get => (double)this.GetValue(ItemSpacingProperty);
        set => this.SetValue(ItemSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the overall control size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether node drag-and-drop is enabled.
    /// </summary>
    public bool AllowNodeDragDrop
    {
        get => (bool)this.GetValue(AllowNodeDragDropProperty);
        set => this.SetValue(AllowNodeDragDropProperty, value);
    }

    /// <summary>
    /// Gets or sets the allowed node drop positions.
    /// </summary>
    public XTreeViewNodeDropMode NodeDropMode
    {
        get => (XTreeViewNodeDropMode)this.GetValue(NodeDropModeProperty);
        set => this.SetValue(NodeDropModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the handler used to validate and execute node drops.
    /// </summary>
    public IXTreeViewNodeDropHandler? NodeDropHandler
    {
        get => (IXTreeViewNodeDropHandler?)this.GetValue(NodeDropHandlerProperty);
        set => this.SetValue(NodeDropHandlerProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the native drag operation should use the default drag feedback.
    /// </summary>
    public bool UseDefaultDragAdorner
    {
        get => (bool)this.GetValue(UseDefaultDragAdornerProperty);
        set => this.SetValue(UseDefaultDragAdornerProperty, value);
    }

    /// <summary>
    /// Gets or sets the opacity used by the default drag adorner.
    /// </summary>
    public double DefaultDragAdornerOpacity
    {
        get => (double)this.GetValue(DefaultDragAdornerOpacityProperty);
        set => this.SetValue(DefaultDragAdornerOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the relative mouse anchor point used by the drag adorner.
    /// </summary>
    public Point DragMouseAnchorPoint
    {
        get => (Point)this.GetValue(DragMouseAnchorPointProperty);
        set => this.SetValue(DragMouseAnchorPointProperty, value);
    }

    /// <summary>
    /// Gets or sets the pixel translation applied to the drag adorner.
    /// </summary>
    public Point DragAdornerTranslation
    {
        get => (Point)this.GetValue(DragAdornerTranslationProperty);
        set => this.SetValue(DragAdornerTranslationProperty, value);
    }

    /// <summary>
    /// Gets or sets the pixel translation reserved for drag effect feedback.
    /// </summary>
    public Point EffectAdornerTranslation
    {
        get => (Point)this.GetValue(EffectAdornerTranslationProperty);
        set => this.SetValue(EffectAdornerTranslationProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template used for the drag adorner.
    /// </summary>
    public DataTemplate? DragAdornerTemplate
    {
        get => (DataTemplate?)this.GetValue(DragAdornerTemplateProperty);
        set => this.SetValue(DragAdornerTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template selector used for the drag adorner.
    /// </summary>
    public DataTemplateSelector? DragAdornerTemplateSelector
    {
        get => (DataTemplateSelector?)this.GetValue(DragAdornerTemplateSelectorProperty);
        set => this.SetValue(DragAdornerTemplateSelectorProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the drop target adorner should also be shown for empty root targets.
    /// </summary>
    public bool ShowAlwaysDropTargetAdorner
    {
        get => (bool)this.GetValue(ShowAlwaysDropTargetAdornerProperty);
        set => this.SetValue(ShowAlwaysDropTargetAdornerProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used by the drop target adorner.
    /// </summary>
    public Brush? DropTargetAdornerBrush
    {
        get => (Brush?)this.GetValue(DropTargetAdornerBrushProperty);
        set => this.SetValue(DropTargetAdornerBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the pen used by the drop target adorner.
    /// </summary>
    public Pen? DropTargetAdornerPen
    {
        get => (Pen?)this.GetValue(DropTargetAdornerPenProperty);
        set => this.SetValue(DropTargetAdornerPenProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used by the drop target highlight adorner.
    /// </summary>
    public Brush? DropTargetHighlightBrush
    {
        get => (Brush?)this.GetValue(DropTargetHighlightBrushProperty);
        set => this.SetValue(DropTargetHighlightBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a drop target hint should be shown.
    /// </summary>
    public bool UseDropTargetHint
    {
        get => (bool)this.GetValue(UseDropTargetHintProperty);
        set => this.SetValue(UseDropTargetHintProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template used by the drop hint.
    /// </summary>
    public DataTemplate? DropHintDataTemplate
    {
        get => (DataTemplate?)this.GetValue(DropHintDataTemplateProperty);
        set => this.SetValue(DropHintDataTemplateProperty, value);
    }

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

    #region ### Internal Properties ###
    /// <summary>
    /// Gets the internal native node drag-and-drop controller.
    /// </summary>
    public object NodeDragDropController => this.nodeDragDropController;

    /// <summary>
    /// Gets the effective node drop handler.
    /// </summary>
    internal IXTreeViewNodeDropHandler? EffectiveNodeDropHandler => this.NodeDropHandler ?? this.defaultNodeDropHandler;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Expands all tree nodes.
    /// </summary>
    public void ExpandAll()
    {
        this.SetExpansionState(true);
    }

    /// <summary>
    /// Collapses all tree nodes.
    /// </summary>
    public void CollapseAll()
    {
        this.SetExpansionState(false);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer ?? this.FindDescendantScrollViewer(this);
        this.UpdateNodeDragDropConfiguration();
        this.RefreshGeneratedItems();
        this.ScheduleSelectedDataItemSynchronization();
    }

    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XTreeViewItem();
    }

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XTreeViewItem;
    }

    /// <inheritdoc />
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is XTreeViewItem treeViewItem)
        {
            treeViewItem.SetOwnerTreeViewInternal(this);
        }
    }

    /// <inheritdoc />
    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

        this.UpdateNodeDragDropConfiguration();
        this.RefreshGeneratedItems();
        this.ScheduleSelectedDataItemSynchronization();
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);
        this.SetCurrentValue(HoveredDataItemProperty, null);
    }

    /// <inheritdoc />
    protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
    {
        base.OnSelectedItemChanged(e);

        if (this.isSynchronizingSelectedDataItem)
        {
            return;
        }

        this.isSynchronizingSelectedDataItem = true;

        try
        {
            this.SetCurrentValue(SelectedDataItemProperty, e.NewValue);
        }
        finally
        {
            this.isSynchronizingSelectedDataItem = false;
        }
    }
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Refreshes all generated item containers after owner-level settings have changed.
    /// </summary>
    internal void RefreshGeneratedItems()
    {
        this.RefreshGeneratedItems(this);
    }

    /// <summary>
    /// Finds the realized container for the specified data item.
    /// </summary>
    /// <param name="dataItem">The data item.</param>
    /// <returns>The realized container or <see langword="null"/>.</returns>
    internal XTreeViewItem? FindContainerForDataItemInternal(object dataItem)
    {
        return this.FindContainerForDataItem(this, dataItem);
    }

    /// <summary>
    /// Sets the current hovered data item from a generated item container.
    /// </summary>
    /// <param name="dataItem">The hovered data item.</param>
    internal void SetHoveredDataItemInternal(object? dataItem)
    {
        this.SetCurrentValue(HoveredDataItemProperty, dataItem);
    }

    /// <summary>
    /// Clears the current hovered data item if it still matches the specified item.
    /// </summary>
    /// <param name="dataItem">The previously hovered data item.</param>
    internal void ClearHoveredDataItemInternal(object? dataItem)
    {
        if (ReferenceEquals(this.HoveredDataItem, dataItem) || this.HoveredDataItem is not null && this.HoveredDataItem.Equals(dataItem))
        {
            this.SetCurrentValue(HoveredDataItemProperty, null);
        }
    }

    /// <summary>
    /// Marks the tree as currently processing a node drag operation for visual state purposes.
    /// </summary>
    internal void BeginNodeDragVisualState()
    {
        if (!this.IsNodeDragInProgress)
        {
            this.SetValue(IsNodeDragInProgressPropertyKey, true);
        }
    }

    /// <summary>
    /// Clears the node drag visual state.
    /// </summary>
    internal void EndNodeDragVisualState()
    {
        if (this.IsNodeDragInProgress)
        {
            this.SetValue(IsNodeDragInProgressPropertyKey, false);
        }
    }

    /// <summary>
    /// Selects the dropped data item after a node drag-and-drop operation.
    /// </summary>
    /// <param name="dataItem">The dropped data item.</param>
    internal void SelectDroppedDataItemInternal(object dataItem)
    {
        this.SetCurrentValue(SelectedDataItemProperty, dataItem);
        this.GetBindingExpression(SelectedDataItemProperty)?.UpdateSource();

        _ = this.Dispatcher.BeginInvoke(
            DispatcherPriority.Loaded,
            new Action(() =>
            {
                this.RefreshGeneratedItems();
                this.ScheduleSelectedDataItemSynchronization();
                this.EnsureSelectedDataItemVisible();
            }));

        _ = this.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                this.RefreshGeneratedItems();
                this.ScheduleSelectedDataItemSynchronization();
                this.EnsureSelectedDataItemVisible();
            }));

        _ = this.Dispatcher.BeginInvoke(
            DispatcherPriority.ApplicationIdle,
            new Action(() =>
            {
                this.RefreshGeneratedItems();
                this.ScheduleSelectedDataItemSynchronization();
                this.EnsureSelectedDataItemVisible();
            }));
    }
    #endregion

    #region ### Private Static Methods ###
    /// <summary>
    /// Handles changes of owner configuration properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnOwnerConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XTreeView treeView)
        {
            return;
        }

        treeView.RefreshGeneratedItems();
        treeView.ScheduleSelectedDataItemSynchronization();
    }

    /// <summary>
    /// Handles changes of node drag-and-drop configuration.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnNodeDragDropConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XTreeView treeView)
        {
            return;
        }

        treeView.UpdateNodeDragDropConfiguration();
        treeView.RefreshGeneratedItems();
    }

    /// <summary>
    /// Handles changes of node drag-and-drop visual configuration.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnNodeDragDropVisualConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XTreeView treeView)
        {
            return;
        }

        treeView.nodeDragDropController.RefreshDropTargetAdorner();
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedDataItem"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnSelectedDataItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XTreeView treeView || treeView.isSynchronizingSelectedDataItem)
        {
            return;
        }

        treeView.ScheduleSelectedDataItemSynchronization();
    }

    /// <summary>
    /// Determines whether two values represent the same tree item.
    /// </summary>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <returns><c>true</c> if both values represent the same item; otherwise, <c>false</c>.</returns>
    private static bool AreSameItem(object? first, object? second)
    {
        return ReferenceEquals(first, second) || first is not null && first.Equals(second);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles the loaded event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnLoaded(object sender, RoutedEventArgs eventArgs)
    {
        this.UpdateNodeDragDropConfiguration();
        this.RefreshGeneratedItems();
        this.ScheduleSelectedDataItemSynchronization();
    }

    /// <summary>
    /// Handles changes of the root item container generator.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnItemContainerGeneratorStatusChanged(object? sender, EventArgs eventArgs)
    {
        if (this.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
        {
            this.RefreshGeneratedItems();
            this.ScheduleSelectedDataItemSynchronization();
        }
    }

    /// <summary>
    /// Applies the current drag-and-drop configuration to the root tree view.
    /// </summary>
    private void UpdateNodeDragDropConfiguration()
    {
        this.defaultNodeDropHandler = this.CreateDefaultNodeDropHandler();
        bool isEnabled = this.AllowNodeDragDrop && this.EffectiveNodeDropHandler is not null;

        this.AllowDrop = isEnabled;
        this.nodeDragDropController.SetEnabled(isEnabled);
    }

    /// <summary>
    /// Applies the current drag-and-drop configuration to a realized item container.
    /// </summary>
    /// <param name="treeViewItem">The item container.</param>
    internal void ApplyNodeDragDropConfigurationInternal(XTreeViewItem treeViewItem)
    {
        if (this.AllowNodeDragDrop && this.EffectiveNodeDropHandler is not null)
        {
            treeViewItem.AllowDrop = true;
            return;
        }

        treeViewItem.ClearValue(UIElement.AllowDropProperty);
    }

    /// <summary>
    /// Creates the default node drop handler when no custom handler is assigned.
    /// </summary>
    /// <returns>The default node drop handler or <see langword="null"/>.</returns>
#pragma warning disable CA1859
    private IXTreeViewNodeDropHandler? CreateDefaultNodeDropHandler()
    {
        object? rootItems = this.ItemsSource;
        if (rootItems is ICollectionView collectionView)
        {
            rootItems = collectionView.SourceCollection;
        }

        return rootItems is not null && !string.IsNullOrWhiteSpace(this.ChildrenMemberPath)
            ? new XTreeViewReflectionNodeDropHandler(rootItems, this.ChildrenMemberPath)
            : null;
    }
#pragma warning restore CA1859

    /// <summary>
    /// Sets the expansion state for all nodes.
    /// </summary>
    /// <param name="isExpanded">The target expansion state.</param>
    private void SetExpansionState(bool isExpanded)
    {
        if (!string.IsNullOrWhiteSpace(this.ExpandedMemberPath))
        {
            foreach (object item in this.GetRootDataItems())
            {
                this.SetExpansionState(item, isExpanded);
            }
        }

        SetVisualExpansionState(this, isExpanded);
        this.RefreshGeneratedItems();
    }

    /// <summary>
    /// Sets the expansion state for a data node and its children.
    /// </summary>
    /// <param name="item">The data item.</param>
    /// <param name="isExpanded">The target expansion state.</param>
    private void SetExpansionState(object item, bool isExpanded)
    {
        this.SetExpandedMemberValue(item, isExpanded);

        foreach (object childItem in this.GetChildDataItems(item))
        {
            this.SetExpansionState(childItem, isExpanded);
        }
    }

    /// <summary>
    /// Sets the expansion state for all realized visual containers.
    /// </summary>
    /// <param name="parent">The parent items control.</param>
    /// <param name="isExpanded">The target expansion state.</param>
    private static void SetVisualExpansionState(ItemsControl parent, bool isExpanded)
    {
        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not TreeViewItem container)
            {
                continue;
            }

            container.IsExpanded = isExpanded;
            SetVisualExpansionState(container, isExpanded);
        }
    }

    /// <summary>
    /// Gets the root data items.
    /// </summary>
    /// <returns>The root data items.</returns>
    private IEnumerable<object> GetRootDataItems()
    {
        IEnumerable? items = this.ItemsSource ?? this.Items;
        if (items is null)
        {
            yield break;
        }

        foreach (object? item in items)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Gets child data items from the configured <see cref="ChildrenMemberPath"/>.
    /// </summary>
    /// <param name="item">The parent data item.</param>
    /// <returns>The child data items.</returns>
    private IEnumerable<object> GetChildDataItems(object item)
    {
        if (string.IsNullOrWhiteSpace(this.ChildrenMemberPath))
        {
            yield break;
        }

        PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.ChildrenMemberPath, false);
        if (propertyDescriptor?.GetValue(item) is not IEnumerable children)
        {
            yield break;
        }

        foreach (object? child in children)
        {
            if (child is not null)
            {
                yield return child;
            }
        }
    }

    /// <summary>
    /// Sets the configured expanded member value if possible.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="isExpanded">The target expansion state.</param>
    private void SetExpandedMemberValue(object item, bool isExpanded)
    {
        PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.ExpandedMemberPath, false);
        if (propertyDescriptor is null || propertyDescriptor.IsReadOnly || propertyDescriptor.PropertyType != typeof(bool))
        {
            return;
        }

        propertyDescriptor.SetValue(item, isExpanded);
    }

    /// <summary>
    /// Schedules synchronization from <see cref="SelectedDataItem"/> to the visual tree selection.
    /// </summary>
    private void ScheduleSelectedDataItemSynchronization()
    {
        if (this.isSelectionSynchronizationScheduled)
        {
            return;
        }

        this.isSelectionSynchronizationScheduled = true;

        this.Dispatcher.BeginInvoke(
            this.SynchronizeSelectedDataItemToVisualSelection,
            DispatcherPriority.Loaded);
    }

    /// <summary>
    /// Synchronizes <see cref="SelectedDataItem"/> to the realized tree view item selection.
    /// </summary>
    private void SynchronizeSelectedDataItemToVisualSelection()
    {
        this.isSelectionSynchronizationScheduled = false;

        if (this.isSynchronizingSelectedDataItem)
        {
            return;
        }

        object? selectedDataItem = this.SelectedDataItem;

        this.isSynchronizingSelectedDataItem = true;

        try
        {
            if (selectedDataItem is null)
            {
                this.ClearCurrentSelection();
                return;
            }

            this.UpdateLayout();
            this.TrySelectDataItem(selectedDataItem);
        }
        finally
        {
            this.isSynchronizingSelectedDataItem = false;
        }
    }

    /// <summary>
    /// Tries to select the specified data item inside the tree view.
    /// </summary>
    /// <param name="dataItem">The data item to select.</param>
    /// <returns><c>true</c> if the item was selected; otherwise, <c>false</c>.</returns>
    private bool TrySelectDataItem(object dataItem)
    {
        return this.TrySelectDataItem(this, dataItem);
    }

    /// <summary>
    /// Tries to select the specified data item inside the specified parent items control.
    /// </summary>
    /// <param name="parent">The parent items control.</param>
    /// <param name="dataItem">The data item to select.</param>
    /// <returns><c>true</c> if the item was selected; otherwise, <c>false</c>.</returns>
    private bool TrySelectDataItem(ItemsControl parent, object dataItem)
    {
        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not TreeViewItem container)
            {
                continue;
            }

            if (container is XTreeViewItem xTreeViewItem)
            {
                xTreeViewItem.SetOwnerTreeViewInternal(this);
            }

            if (AreSameItem(item, dataItem) ||
                AreSameItem(container.DataContext, dataItem) ||
                AreSameItem(container.Header, dataItem))
            {
                this.ClearCurrentSelection();

                container.IsSelected = true;
                container.Focus();
                //container.UpdateLayout();

                _ = container.Dispatcher.BeginInvoke(
                    DispatcherPriority.Loaded,
                    new Action(() => this.ScrollContainerIntoView(container)));

                return true;
            }

            bool wasExpanded = container.IsExpanded;

            container.IsExpanded = true;
            container.UpdateLayout();
            this.UpdateLayout();

            if (this.TrySelectDataItem(container, dataItem))
            {
                return true;
            }

            container.IsExpanded = wasExpanded;
            container.UpdateLayout();
        }

        return false;
    }


    /// <summary>
    /// Scrolls the specified container into the visible viewport of the tree view's scroll viewer.
    /// </summary>
    /// <param name="container"></param>
    private void ScrollContainerIntoView(TreeViewItem container)
    {
        if (container is null)
        {
            return;
        }

        container.UpdateLayout();
        this.UpdateLayout();

        ScrollViewer? scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer
                                     ?? FindDescendant<ScrollViewer>(this);

        if (scrollViewer is null)
        {
            container.BringIntoView();
            return;
        }

        GeneralTransform transform = container.TransformToAncestor(scrollViewer);
        Point topLeft = transform.Transform(new Point(0d, 0d));
        Point bottomLeft = transform.Transform(new Point(0d, container.ActualHeight));

        double itemTop = topLeft.Y + scrollViewer.VerticalOffset;
        double itemBottom = bottomLeft.Y + scrollViewer.VerticalOffset;

        double visibleTop = scrollViewer.VerticalOffset;
        double visibleBottom = visibleTop + scrollViewer.ViewportHeight;

        if (itemTop < visibleTop)
        {
            scrollViewer.ScrollToVerticalOffset(itemTop - 8d);
        }
        else if (itemBottom > visibleBottom)
        {
            double newOffset = itemBottom - scrollViewer.ViewportHeight + 8d;
            scrollViewer.ScrollToVerticalOffset(newOffset);
        }

        container.BringIntoView();
    }

    /// <summary>
    /// Scro
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    private static T? FindDescendant<T>(DependencyObject parent)
    where T : DependencyObject
    {
        if (parent is null)
        {
            return null;
        }

        int childCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T typedChild)
            {
                return typedChild;
            }

            T? descendant = FindDescendant<T>(child);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
    }

    /// <summary>
    /// Clears the current tree view selection if the selected container is currently realized.
    /// </summary>
    private void ClearCurrentSelection()
    {
        this.ClearSelection(this);
    }

    /// <summary>
    /// Clears the current selection below the specified parent items control.
    /// </summary>
    /// <param name="parent">The parent items control.</param>
    /// <returns><c>true</c> if a selected container was found and cleared; otherwise, <c>false</c>.</returns>
    private bool ClearSelection(ItemsControl parent)
    {

        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not TreeViewItem container)
            {
                continue;
            }

            if (container is XTreeViewItem xTreeViewItem)
            {
                xTreeViewItem.SetOwnerTreeViewInternal(this);
            }

            if (container.IsSelected)
            {
                container.IsSelected = false;
                return true;
            }

            if (this.ClearSelection(container))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Ensures that the current selected data item is visible inside the tree viewport.
    /// </summary>
    private void EnsureSelectedDataItemVisible()
    {
        object? selectedDataItem = this.SelectedDataItem;
        if (selectedDataItem is null)
        {
            return;
        }

        this.UpdateLayout();

        if (this.TrySelectDataItem(selectedDataItem))
        {
            XTreeViewItem? container = this.FindContainerForDataItemInternal(selectedDataItem);
            if (container is not null)
            {
                _ = this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => this.ScrollContainerIntoView(container)));
            }
        }
    }

    /// <summary>
    /// Scrolls the specified container into the visible viewport.
    /// </summary>
    /// <param name="container">The realized container.</param>
    private void ScrollContainerIntoView(FrameworkElement container)
    {
        if (container is null)
        {
            return;
        }

        this.scrollViewer ??= this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer ?? this.FindDescendantScrollViewer(this);

        container.UpdateLayout();
        container.BringIntoView();

        ScrollViewer? ownerScrollViewer = this.scrollViewer;
        if (ownerScrollViewer is null)
        {
            return;
        }

        GeneralTransform transform = container.TransformToAncestor(this);
        Rect itemBounds = transform.TransformBounds(new Rect(new Point(0d, 0d), container.RenderSize));

        double viewportTop = ownerScrollViewer.VerticalOffset;
        double viewportBottom = viewportTop + ownerScrollViewer.ViewportHeight;

        if (itemBounds.Top < viewportTop)
        {
            ownerScrollViewer.ScrollToVerticalOffset(Math.Max(0d, itemBounds.Top - 8d));
            return;
        }

        if (itemBounds.Bottom > viewportBottom)
        {
            double targetOffset = itemBounds.Bottom - ownerScrollViewer.ViewportHeight + 8d;
            ownerScrollViewer.ScrollToVerticalOffset(Math.Max(0d, targetOffset));
        }
    }

    /// <summary>
    /// Finds the first descendant scroll viewer below the specified root element.
    /// </summary>
    /// <param name="root">The search root.</param>
    /// <returns>The scroll viewer or <see langword="null" />.</returns>
    private ScrollViewer? FindDescendantScrollViewer(DependencyObject? root)
    {
        if (root is null)
        {
            return null;
        }

        int childCount = VisualTreeHelper.GetChildrenCount(root);
        for (int index = 0; index < childCount; index++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(root, index);
            if (child is ScrollViewer childScrollViewer)
            {
                return childScrollViewer;
            }

            ScrollViewer? nested = this.FindDescendantScrollViewer(child);
            if (nested is not null)
            {
                return nested;
            }
        }

        return null;
    }

    /// <summary>
    /// Refreshes generated item containers recursively.
    /// </summary>
    /// <param name="parent">The parent items control.</param>
    private void RefreshGeneratedItems(ItemsControl parent)
    {
        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not XTreeViewItem container)
            {
                continue;
            }

            container.SetOwnerTreeViewInternal(this);
            this.RefreshGeneratedItems(container);
        }
    }

    /// <summary>
    /// Finds the realized container for the specified data item below the specified parent.
    /// </summary>
    /// <param name="parent">The parent items control.</param>
    /// <param name="dataItem">The data item.</param>
    /// <returns>The realized container or <see langword="null"/>.</returns>
    private XTreeViewItem? FindContainerForDataItem(ItemsControl parent, object dataItem)
    {
        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not XTreeViewItem container)
            {
                continue;
            }

            container.SetOwnerTreeViewInternal(this);

            if (AreSameItem(item, dataItem) || AreSameItem(container.DataContext, dataItem) || AreSameItem(container.Header, dataItem))
            {
                return container;
            }

            XTreeViewItem? childResult = this.FindContainerForDataItem(container, dataItem);
            if (childResult is not null)
            {
                return childResult;
            }
        }

        return null;
    }
    #endregion

    #region ### Class XTreeViewCommand ###
    /// <summary>
    /// Provides a simple command implementation for internal tree view actions.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XTreeViewCommand"/> class.
    /// </remarks>
    /// <param name="execute">The execute delegate.</param>
    private sealed class XTreeViewCommand(Action<object?> execute) : ICommand
    {
        #region ### Fields ###
        /// <summary>
        /// The execute delegate.
        /// </summary>
        private readonly Action<object?> execute = execute;

        #endregion
        #region ### Constructors ###
        #endregion

        #region ### Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.execute(parameter);
        }
        #endregion
    }
    #endregion
}
#endregion
