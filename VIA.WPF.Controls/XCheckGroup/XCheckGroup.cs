// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XCheckGroup ###
/// <summary>
/// Represents a grouped checklist surface based on the VIA.WPF panel visual language.
/// Supports both explicit <see cref="XCheckGroupItem"/> items and data-bound <see cref="ItemsControl.ItemsSource"/> usage.
/// </summary>
public class XCheckGroup : ItemsControl
{
    #region ### Private Fields ###
    private bool _isSynchronizingHeader;
    private bool _isSynchronizingSelectedItems;
    private INotifyCollectionChanged? _selectedItemsNotifier;
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XCheckGroup),
        new PropertyMetadata(null, OnTitleChanged));

    /// <summary>
    /// Identifies the <see cref="Header" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XCheckGroup),
        new PropertyMetadata(null, OnHeaderChanged));

    /// <summary>
    /// Identifies the <see cref="ShowTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle),
        typeof(bool),
        typeof(XCheckGroup),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XCheckGroup),
        new PropertyMetadata(new Thickness(14d, 7d, 7d, 7d)));

    /// <summary>
    /// Identifies the <see cref="ItemsPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemsPaddingProperty = DependencyProperty.Register(
        nameof(ItemsPadding),
        typeof(Thickness),
        typeof(XCheckGroup),
        new PropertyMetadata(new Thickness(12d)));

    /// <summary>
    /// Identifies the <see cref="ItemSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register(
        nameof(ItemSpacing),
        typeof(double),
        typeof(XCheckGroup),
        new PropertyMetadata(8d, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XCheckGroup),
        new PropertyMetadata(Orientation.Vertical, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XCheckGroup),
        new PropertyMetadata(new CornerRadius(6d)));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XCheckGroup),
        new PropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Elevation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XCheckGroup),
        new PropertyMetadata(XElevation.None));

    /// <summary>
    /// Identifies the <see cref="SelectedItems"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        nameof(SelectedItems),
        typeof(IList),
        typeof(XCheckGroup),
        new PropertyMetadata(null, OnSelectedItemsChanged));

    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XCheckGroup"/> class.
    /// </summary>
    static XCheckGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XCheckGroup),
            new FrameworkPropertyMetadata(typeof(XCheckGroup)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XCheckGroup"/> class.
    /// </summary>
    public XCheckGroup()
    {
        this.SelectedItems = new ObservableCollection<object>();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the optional title shown above the checklist items.
    /// </summary>
    public string? Title
    {
        get => (string?)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional header shown above the checklist items.
    /// This is an alias for <see cref="Title" /> and is provided for consistency with other VIA controls.
    /// </summary>
    public string? Header
    {
        get => (string?)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the title area is shown.
    /// </summary>
    public bool ShowTitle
    {
        get => (bool)this.GetValue(ShowTitleProperty);
        set => this.SetValue(ShowTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the header area.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)this.GetValue(HeaderPaddingProperty);
        set => this.SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the items area.
    /// </summary>
    public Thickness ItemsPadding
    {
        get => (Thickness)this.GetValue(ItemsPaddingProperty);
        set => this.SetValue(ItemsPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between checklist items.
    /// </summary>
    public double ItemSpacing
    {
        get => (double)this.GetValue(ItemSpacingProperty);
        set => this.SetValue(ItemSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the layout orientation of the checklist items.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the outer corner radius of the group surface.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size used by the contained selection items.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation level of the group surface.
    /// </summary>
    public XElevation Elevation
    {
        get => (XElevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected items collection.
    /// For explicit item declarations, this collection contains the corresponding <see cref="XCheckGroupItem"/> instances.
    /// For <see cref="ItemsControl.ItemsSource"/> usage, it contains the bound data items.
    /// </summary>
    public IList SelectedItems
    {
        get => (IList)this.GetValue(SelectedItemsProperty);
        set => this.SetValue(SelectedItemsProperty, value);
    }

    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XCheckGroupItem();
    }

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XCheckGroupItem;
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XCheckGroupItem container)
        {
            return;
        }

        container.IsCheckedChanged -= this.OnContainerIsCheckedChanged;
        container.IsCheckedChanged += this.OnContainerIsCheckedChanged;

        this.ApplyContainerMargin(container);

        object selectedValue = GetSelectedValueForItem(container, item);
        container.IsChecked = this.ContainsSelectedItem(selectedValue);
    }

    /// <inheritdoc/>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XCheckGroupItem container)
        {
            container.IsCheckedChanged -= this.OnContainerIsCheckedChanged;
            container.ClearValue(MarginProperty);
        }

        base.ClearContainerForItemOverride(element, item);
    }

    /// <inheritdoc/>
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        this.UpdateContainerMargins();
        this.SyncContainersFromSelectedItems();
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.UpdateContainerMargins();
        this.SyncContainersFromSelectedItems();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles changes of the <see cref="Title" /> property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XCheckGroup group || group._isSynchronizingHeader)
        {
            return;
        }

        group._isSynchronizingHeader = true;
        try
        {
            group.SetCurrentValue(HeaderProperty, e.NewValue);
        }
        finally
        {
            group._isSynchronizingHeader = false;
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="Header" /> property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XCheckGroup group || group._isSynchronizingHeader)
        {
            return;
        }

        group._isSynchronizingHeader = true;
        try
        {
            group.SetCurrentValue(TitleProperty, e.NewValue);
        }
        finally
        {
            group._isSynchronizingHeader = false;
        }
    }

    /// <summary>
    /// Handles layout-related dependency property changes.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XCheckGroup group)
        {
            group.UpdateContainerMargins();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedItems"/> property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XCheckGroup group)
        {
            group.OnSelectedItemsChanged((IList?)e.OldValue, (IList?)e.NewValue);
        }
    }

    /// <summary>
    /// Handles changes of the selected items collection instance.
    /// </summary>
    /// <param name="oldValue">The old collection.</param>
    /// <param name="newValue">The new collection.</param>
    private void OnSelectedItemsChanged(IList? oldValue, IList? newValue)
    {
        if (ReferenceEquals(oldValue, newValue))
        {
            return;
        }

        if (this._selectedItemsNotifier is not null)
        {
            this._selectedItemsNotifier.CollectionChanged -= this.OnSelectedItemsCollectionChanged;
            this._selectedItemsNotifier = null;
        }

        if (newValue is null)
        {
            this.SelectedItems = new ObservableCollection<object>();
            return;
        }

        if (newValue is INotifyCollectionChanged notifier)
        {
            this._selectedItemsNotifier = notifier;
            this._selectedItemsNotifier.CollectionChanged += this.OnSelectedItemsCollectionChanged;
        }

        this.SyncContainersFromSelectedItems();
    }

    /// <summary>
    /// Handles external changes of the selected items collection.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnSelectedItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (this._isSynchronizingSelectedItems)
        {
            return;
        }

        this.SyncContainersFromSelectedItems();
    }

    /// <summary>
    /// Handles check state changes of realized item containers.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnContainerIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (this._isSynchronizingSelectedItems || sender is not XCheckGroupItem container)
        {
            return;
        }

        int index = this.ItemContainerGenerator.IndexFromContainer(container);
        if (index < 0 || index >= this.Items.Count)
        {
            return;
        }

        object item = this.Items[index];
        object selectedValue = GetSelectedValueForItem(container, item);

        this._isSynchronizingSelectedItems = true;
        try
        {
            if (container.IsChecked == true)
            {
                this.AddSelectedItem(selectedValue);
            }
            else
            {
                this.RemoveSelectedItem(selectedValue);
            }
        }
        finally
        {
            this._isSynchronizingSelectedItems = false;
        }
    }

    /// <summary>
    /// Synchronizes the realized containers from the current <see cref="SelectedItems"/> collection.
    /// </summary>
    private void SyncContainersFromSelectedItems()
    {
        this._isSynchronizingSelectedItems = true;
        try
        {
            for (int index = 0; index < this.Items.Count; index++)
            {
                if (this.ItemContainerGenerator.ContainerFromIndex(index) is not XCheckGroupItem container)
                {
                    continue;
                }

                object item = this.Items[index];
                object selectedValue = GetSelectedValueForItem(container, item);
                container.IsChecked = this.ContainsSelectedItem(selectedValue);
            }
        }
        finally
        {
            this._isSynchronizingSelectedItems = false;
        }
    }

    /// <summary>
    /// Updates the margins of all realized item containers.
    /// </summary>
    private void UpdateContainerMargins()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is XCheckGroupItem container)
            {
                this.ApplyContainerMargin(container, index);
            }
        }
    }

    /// <summary>
    /// Applies the correct margin to the specified container.
    /// </summary>
    /// <param name="container">The target container.</param>
    private void ApplyContainerMargin(XCheckGroupItem container)
    {
        int index = this.ItemContainerGenerator.IndexFromContainer(container);
        this.ApplyContainerMargin(container, index);
    }

    /// <summary>
    /// Applies the correct margin to the specified container.
    /// </summary>
    /// <param name="container">The target container.</param>
    /// <param name="index">The container index.</param>
    private void ApplyContainerMargin(XCheckGroupItem container, int index)
    {
        double spacing = Math.Max(0d, this.ItemSpacing);
        bool isLast = index >= 0 && index == this.Items.Count - 1;

        Thickness margin = this.Orientation == Orientation.Horizontal
            ? new Thickness(0d, 0d, isLast ? 0d : spacing, 0d)
            : new Thickness(0d, 0d, 0d, isLast ? 0d : spacing);

        container.Margin = margin;
    }

    /// <summary>
    /// Gets the selected value corresponding to the given item.
    /// </summary>
    /// <param name="container">The realized item container.</param>
    /// <param name="item">The raw item.</param>
    /// <returns>The value used in <see cref="SelectedItems"/>.</returns>
    private static object GetSelectedValueForItem(XCheckGroupItem container, object item)
    {
        return item is XCheckGroupItem ? container : item;
    }

    /// <summary>
    /// Determines whether the selected items collection already contains the specified value.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><see langword="true"/> if the value exists; otherwise <see langword="false"/>.</returns>
    private bool ContainsSelectedItem(object value)
    {
        foreach (object? selectedItem in this.SelectedItems)
        {
            if (Equals(selectedItem, value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Adds the specified value to the selected items collection if it is not present yet.
    /// </summary>
    /// <param name="value">The value to add.</param>
    private void AddSelectedItem(object value)
    {
        if (!this.ContainsSelectedItem(value))
        {
            this.SelectedItems.Add(value);
        }
    }

    /// <summary>
    /// Removes the specified value from the selected items collection if present.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    private void RemoveSelectedItem(object value)
    {
        for (int index = this.SelectedItems.Count - 1; index >= 0; index--)
        {
            if (Equals(this.SelectedItems[index], value))
            {
                this.SelectedItems.RemoveAt(index);
            }
        }
    }
    #endregion
}
#endregion
