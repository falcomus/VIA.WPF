// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

#region ### Class XTreeViewItem ###
/// <summary>
/// Represents a themed hierarchical tree view item with optional node action commands.
/// </summary>
public class XTreeViewItem : TreeViewItem
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool?),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool?),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool?),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="NewCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(
        nameof(NewCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
        nameof(EditCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
        nameof(DeleteCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="NewCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandParameterProperty = DependencyProperty.Register(
        nameof(NewCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandParameterProperty = DependencyProperty.Register(
        nameof(EditCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandParameterProperty = DependencyProperty.Register(
        nameof(DeleteCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    private static readonly DependencyPropertyKey ResolvedShowNewButtonPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedShowNewButton),
        typeof(bool),
        typeof(XTreeViewItem),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedShowNewButtonProperty = ResolvedShowNewButtonPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedShowEditButtonPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedShowEditButton),
        typeof(bool),
        typeof(XTreeViewItem),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedShowEditButtonProperty = ResolvedShowEditButtonPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedShowDeleteButtonPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedShowDeleteButton),
        typeof(bool),
        typeof(XTreeViewItem),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedShowDeleteButtonProperty = ResolvedShowDeleteButtonPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedNewCommandPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedNewCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedNewCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedNewCommandProperty = ResolvedNewCommandPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedEditCommandPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedEditCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedEditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedEditCommandProperty = ResolvedEditCommandPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedDeleteCommandPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedDeleteCommand),
        typeof(ICommand),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedDeleteCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedDeleteCommandProperty = ResolvedDeleteCommandPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedNewCommandParameterPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedNewCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedNewCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedNewCommandParameterProperty = ResolvedNewCommandParameterPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedEditCommandParameterPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedEditCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedEditCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedEditCommandParameterProperty = ResolvedEditCommandParameterPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResolvedDeleteCommandParameterPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResolvedDeleteCommandParameter),
        typeof(object),
        typeof(XTreeViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="ResolvedDeleteCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResolvedDeleteCommandParameterProperty = ResolvedDeleteCommandParameterPropertyKey.DependencyProperty;
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The owning tree view.
    /// </summary>
    private XTreeView? ownerTreeView;

    /// <summary>
    /// Indicates whether owner-dependent properties are currently refreshed.
    /// </summary>
    private bool isRefreshingOwnerDependentProperties;

    /// <summary>
    /// Indicates whether an owner-dependent refresh is already scheduled.
    /// </summary>
    private bool isOwnerDependentRefreshScheduled;

    /// <summary>
    /// Indicates whether an owner-dependent refresh was requested before the item was fully loaded or while another refresh was running.
    /// </summary>
    private bool isOwnerDependentRefreshPending;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTreeViewItem"/> class.
    /// </summary>
    static XTreeViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTreeViewItem),
            new FrameworkPropertyMetadata(typeof(XTreeViewItem)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XTreeViewItem"/> class.
    /// </summary>
    public XTreeViewItem()
    {
        this.Loaded += this.OnLoaded;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the new button is shown for this node.
    /// </summary>
    public bool? ShowNewButton
    {
        get => (bool?)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit button is shown for this node.
    /// </summary>
    public bool? ShowEditButton
    {
        get => (bool?)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the delete button is shown for this node.
    /// </summary>
    public bool? ShowDeleteButton
    {
        get => (bool?)this.GetValue(ShowDeleteButtonProperty);
        set => this.SetValue(ShowDeleteButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the new action is invoked for this node.
    /// </summary>
    public ICommand? NewCommand
    {
        get => (ICommand?)this.GetValue(NewCommandProperty);
        set => this.SetValue(NewCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the edit action is invoked for this node.
    /// </summary>
    public ICommand? EditCommand
    {
        get => (ICommand?)this.GetValue(EditCommandProperty);
        set => this.SetValue(EditCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the delete action is invoked for this node.
    /// </summary>
    public ICommand? DeleteCommand
    {
        get => (ICommand?)this.GetValue(DeleteCommandProperty);
        set => this.SetValue(DeleteCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used for the new action.
    /// </summary>
    public object? NewCommandParameter
    {
        get => this.GetValue(NewCommandParameterProperty);
        set => this.SetValue(NewCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used for the edit action.
    /// </summary>
    public object? EditCommandParameter
    {
        get => this.GetValue(EditCommandParameterProperty);
        set => this.SetValue(EditCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used for the delete action.
    /// </summary>
    public object? DeleteCommandParameter
    {
        get => this.GetValue(DeleteCommandParameterProperty);
        set => this.SetValue(DeleteCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the new button is effectively shown.
    /// </summary>
    public bool ResolvedShowNewButton
    {
        get => (bool)this.GetValue(ResolvedShowNewButtonProperty);
        private set => this.SetValue(ResolvedShowNewButtonPropertyKey, value);
    }

    /// <summary>
    /// Gets a value indicating whether the edit button is effectively shown.
    /// </summary>
    public bool ResolvedShowEditButton
    {
        get => (bool)this.GetValue(ResolvedShowEditButtonProperty);
        private set => this.SetValue(ResolvedShowEditButtonPropertyKey, value);
    }

    /// <summary>
    /// Gets a value indicating whether the delete button is effectively shown.
    /// </summary>
    public bool ResolvedShowDeleteButton
    {
        get => (bool)this.GetValue(ResolvedShowDeleteButtonProperty);
        private set => this.SetValue(ResolvedShowDeleteButtonPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective new command.
    /// </summary>
    public ICommand? ResolvedNewCommand
    {
        get => (ICommand?)this.GetValue(ResolvedNewCommandProperty);
        private set => this.SetValue(ResolvedNewCommandPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective edit command.
    /// </summary>
    public ICommand? ResolvedEditCommand
    {
        get => (ICommand?)this.GetValue(ResolvedEditCommandProperty);
        private set => this.SetValue(ResolvedEditCommandPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective delete command.
    /// </summary>
    public ICommand? ResolvedDeleteCommand
    {
        get => (ICommand?)this.GetValue(ResolvedDeleteCommandProperty);
        private set => this.SetValue(ResolvedDeleteCommandPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective new command parameter.
    /// </summary>
    public object? ResolvedNewCommandParameter
    {
        get => this.GetValue(ResolvedNewCommandParameterProperty);
        private set => this.SetValue(ResolvedNewCommandParameterPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective edit command parameter.
    /// </summary>
    public object? ResolvedEditCommandParameter
    {
        get => this.GetValue(ResolvedEditCommandParameterProperty);
        private set => this.SetValue(ResolvedEditCommandParameterPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective delete command parameter.
    /// </summary>
    public object? ResolvedDeleteCommandParameter
    {
        get => this.GetValue(ResolvedDeleteCommandParameterProperty);
        private set => this.SetValue(ResolvedDeleteCommandParameterPropertyKey, value);
    }
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Sets the owning tree view and schedules refresh of all owner-dependent values.
    /// </summary>
    /// <param name="owner">The owning tree view.</param>
    internal void SetOwnerTreeViewInternal(XTreeView owner)
    {
        this.ownerTreeView = owner;
        this.ScheduleOwnerDependentPropertiesRefresh();
    }

    /// <summary>
    /// Refreshes owner-dependent properties and bindings.
    /// </summary>
    internal void RefreshOwnerDependentProperties()
    {
        if (!this.IsLoaded)
        {
            this.isOwnerDependentRefreshPending = true;
            return;
        }

        if (this.isRefreshingOwnerDependentProperties)
        {
            this.isOwnerDependentRefreshPending = true;
            return;
        }

        this.isRefreshingOwnerDependentProperties = true;

        try
        {
            this.isOwnerDependentRefreshPending = false;
            this.UpdateResolvedValues();
            this.UpdateExpandedBinding();
            this.UpdateNodeDragDropConfiguration();
        }
        finally
        {
            this.isRefreshingOwnerDependentProperties = false;
        }

        if (this.isOwnerDependentRefreshPending)
        {
            this.ScheduleOwnerDependentPropertiesRefresh();
        }
    }
    #endregion

    #region ### Protected Methods ###
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

        if (element is XTreeViewItem treeViewItem && this.ownerTreeView is not null)
        {
            treeViewItem.SetOwnerTreeViewInternal(this.ownerTreeView);
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.ScheduleOwnerDependentPropertiesRefresh();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == ShowNewButtonProperty ||
            e.Property == ShowEditButtonProperty ||
            e.Property == ShowDeleteButtonProperty ||
            e.Property == NewCommandProperty ||
            e.Property == EditCommandProperty ||
            e.Property == DeleteCommandProperty ||
            e.Property == NewCommandParameterProperty ||
            e.Property == EditCommandParameterProperty ||
            e.Property == DeleteCommandParameterProperty ||
            e.Property == DataContextProperty)
        {
            this.ScheduleOwnerDependentPropertiesRefresh();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        XTreeView? owner = this.ownerTreeView ?? this.GetOwningTreeView();
        object? dataItem = this.DataContext ?? this.Header;

        if (owner is not null && !owner.IsNodeDragInProgress && dataItem is not null)
        {
            owner.SetCurrentValue(XTreeView.HoveredDataItemProperty, dataItem);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        XTreeView? owner = this.ownerTreeView ?? this.GetOwningTreeView();
        object? dataItem = this.DataContext ?? this.Header;

        if (owner is not null && !owner.IsNodeDragInProgress && ReferenceEquals(owner.HoveredDataItem, dataItem))
        {
            owner.SetCurrentValue(XTreeView.HoveredDataItemProperty, null);
        }
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
        this.ownerTreeView ??= this.GetOwningTreeView();
        this.isOwnerDependentRefreshPending = true;
        this.ScheduleOwnerDependentPropertiesRefresh();
    }

    /// <summary>
    /// Schedules a deferred refresh of owner-dependent properties and bindings.
    /// </summary>
    private void ScheduleOwnerDependentPropertiesRefresh()
    {
        if (!this.IsLoaded)
        {
            this.isOwnerDependentRefreshPending = true;
            return;
        }

        if (this.isOwnerDependentRefreshScheduled)
        {
            this.isOwnerDependentRefreshPending = true;
            return;
        }

        this.isOwnerDependentRefreshScheduled = true;

        this.Dispatcher.BeginInvoke(
            () =>
            {
                this.isOwnerDependentRefreshScheduled = false;
                this.RefreshOwnerDependentProperties();
            },
            DispatcherPriority.ContextIdle);
    }

    /// <summary>
    /// Updates all resolved values from local and owning tree view properties.
    /// </summary>
    private void UpdateResolvedValues()
    {
        XTreeView? owner = this.ownerTreeView ?? this.GetOwningTreeView();

        this.ResolvedShowNewButton = this.ShowNewButton ?? owner?.ShowNewButton ?? true;
        this.ResolvedShowEditButton = this.ShowEditButton ?? owner?.ShowEditButton ?? true;
        this.ResolvedShowDeleteButton = this.ShowDeleteButton ?? owner?.ShowDeleteButton ?? true;

        this.ResolvedNewCommand = this.NewCommand ?? owner?.NewItemCommand;
        this.ResolvedEditCommand = this.EditCommand ?? owner?.EditItemCommand;
        this.ResolvedDeleteCommand = this.DeleteCommand ?? owner?.DeleteItemCommand;

        object fallbackParameter = this.DataContext ?? this;

        this.ResolvedNewCommandParameter = this.NewCommandParameter ?? fallbackParameter;
        this.ResolvedEditCommandParameter = this.EditCommandParameter ?? fallbackParameter;
        this.ResolvedDeleteCommandParameter = this.DeleteCommandParameter ?? fallbackParameter;
    }

    /// <summary>
    /// Updates the expansion-state binding from the owning tree view.
    /// </summary>
    private void UpdateExpandedBinding()
    {
        XTreeView? owner = this.ownerTreeView ?? this.GetOwningTreeView();

        if (owner is null || string.IsNullOrWhiteSpace(owner.ExpandedMemberPath))
        {
            BindingOperations.ClearBinding(this, IsExpandedProperty);
            return;
        }

        BindingOperations.SetBinding(
            this,
            IsExpandedProperty,
            new Binding(owner.ExpandedMemberPath)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
    }

    /// <summary>
    /// Gets the owning <see cref="XTreeView"/> if available.
    /// </summary>
    /// <returns>The owning tree view or <see langword="null"/>.</returns>
    private XTreeView? GetOwningTreeView()
    {
        ItemsControl? current = ItemsControl.ItemsControlFromItemContainer(this);

        while (current is not null)
        {
            if (current is XTreeView treeView)
            {
                return treeView;
            }

            current = ItemsControl.ItemsControlFromItemContainer(current);
        }

        return null;
    }

    /// <summary>
    /// Applies the current drag-and-drop configuration to this tree view item.
    /// </summary>
    private void UpdateNodeDragDropConfiguration()
    {
        XTreeView? owner = this.ownerTreeView ?? this.GetOwningTreeView();

        if (!this.IsLoaded || owner is null)
        {
            return;
        }

        owner.ApplyNodeDragDropConfigurationInternal(this);
    }

    #endregion
}
#endregion
