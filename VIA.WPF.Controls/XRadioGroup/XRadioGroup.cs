// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRadioGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XRadioGroup ###
/// <summary>
/// Represents a grouped radio selection surface based on the VIA.WPF panel visual language.
/// Supports both explicit <see cref="XRadioGroupItem" /> items and data-bound <see cref="ItemsControl.ItemsSource" /> usage.
/// </summary>
public class XRadioGroup : ItemsControl
{
    #region ### Private Fields ###
    private bool _isSynchronizingHeader;
    private bool _isSynchronizingSelection;
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Title" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XRadioGroup),
        new PropertyMetadata(null, OnTitleChanged));

    /// <summary>
    /// Identifies the <see cref="Header" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XRadioGroup),
        new PropertyMetadata(null, OnHeaderChanged));

    /// <summary>
    /// Identifies the <see cref="ShowTitle" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle),
        typeof(bool),
        typeof(XRadioGroup),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XRadioGroup),
        new PropertyMetadata(new Thickness(14d, 7d, 7d, 7d)));

    /// <summary>
    /// Identifies the <see cref="ItemsPadding" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemsPaddingProperty = DependencyProperty.Register(
        nameof(ItemsPadding),
        typeof(Thickness),
        typeof(XRadioGroup),
        new PropertyMetadata(new Thickness(12d)));

    /// <summary>
    /// Identifies the <see cref="ItemSpacing" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register(
        nameof(ItemSpacing),
        typeof(double),
        typeof(XRadioGroup),
        new PropertyMetadata(15d, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Orientation" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XRadioGroup),
        new PropertyMetadata(Orientation.Vertical, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XRadioGroup),
        new PropertyMetadata(new CornerRadius(6d)));

    /// <summary>
    /// Identifies the <see cref="Size" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XRadioGroup),
        new PropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Elevation" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XRadioGroup),
        new PropertyMetadata(XElevation.None));

    /// <summary>
    /// Identifies the <see cref="SelectedItem" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(XRadioGroup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedValue" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue),
        typeof(object),
        typeof(XRadioGroup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XRadioGroup" /> class.
    /// </summary>
    static XRadioGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XRadioGroup),
            new FrameworkPropertyMetadata(typeof(XRadioGroup)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the optional title shown above the radio items.
    /// </summary>
    public string? Title
    {
        get => (string?)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional header shown above the radio items.
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
    /// Gets or sets the spacing between radio items.
    /// </summary>
    public double ItemSpacing
    {
        get => (double)this.GetValue(ItemSpacingProperty);
        set => this.SetValue(ItemSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the layout orientation of the radio items.
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
    /// Gets or sets the currently selected item.
    /// For explicit item declarations, this is the corresponding <see cref="XRadioGroupItem" />.
    /// For <see cref="ItemsControl.ItemsSource" /> usage, this is the bound data item.
    /// </summary>
    public object? SelectedItem
    {
        get => this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the currently selected value.
    /// For explicit <see cref="XRadioGroupItem" /> items, <see cref="XRadioGroupItem.Value" /> is used when set;
    /// otherwise the item content is used. For data-bound items, the data item itself is used.
    /// </summary>
    public object? SelectedValue
    {
        get => this.GetValue(SelectedValueProperty);
        set => this.SetValue(SelectedValueProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XRadioGroupItem();
    }

    /// <inheritdoc />
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XRadioGroupItem;
    }

    /// <inheritdoc />
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XRadioGroupItem container)
        {
            return;
        }

        container.IsCheckedChanged -= this.OnContainerIsCheckedChanged;
        container.IsCheckedChanged += this.OnContainerIsCheckedChanged;

        this.ApplyContainerMargin(container);

        if (container.IsChecked == true && this.SelectedItem is null && this.SelectedValue is null)
        {
            this.CommitSelection(container, item);
            return;
        }

        this._isSynchronizingSelection = true;
        try
        {
            container.IsChecked = this.IsContainerSelected(container, item);
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <inheritdoc />
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XRadioGroupItem container)
        {
            container.IsCheckedChanged -= this.OnContainerIsCheckedChanged;
            container.ClearValue(MarginProperty);
        }

        base.ClearContainerForItemOverride(element, item);
    }

    /// <inheritdoc />
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        this.UpdateContainerMargins();
        this.ApplySelectionFromCurrentState();
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.UpdateContainerMargins();
        this.ApplySelectionFromCurrentState();
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
        if (d is not XRadioGroup group || group._isSynchronizingHeader)
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
        if (d is not XRadioGroup group || group._isSynchronizingHeader)
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
        if (d is XRadioGroup group)
        {
            group.UpdateContainerMargins();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedItem" /> property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XRadioGroup group && !group._isSynchronizingSelection)
        {
            group.ApplySelectionFromSelectedItem();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedValue" /> property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XRadioGroup group && !group._isSynchronizingSelection)
        {
            group.ApplySelectionFromSelectedValue();
        }
    }

    /// <summary>
    /// Handles check state changes of realized item containers.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnContainerIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (this._isSynchronizingSelection || sender is not XRadioGroupItem container)
        {
            return;
        }

        int index = this.ItemContainerGenerator.IndexFromContainer(container);
        if (index < 0 || index >= this.Items.Count)
        {
            return;
        }

        object item = this.Items[index];

        if (container.IsChecked == true)
        {
            this.CommitSelection(container, item);
        }
        else if (this.IsContainerSelected(container, item))
        {
            this.ClearSelection();
        }
        else
        {
            this.SyncContainersFromCurrentSelection();
        }
    }

    /// <summary>
    /// Applies the selection represented by <see cref="SelectedItem" />.
    /// </summary>
    private void ApplySelectionFromSelectedItem()
    {
        this._isSynchronizingSelection = true;
        try
        {
            object? selectedValue = this.FindSelectedValueFromSelectedItem(this.SelectedItem);

            if (!Equals(this.SelectedValue, selectedValue))
            {
                this.SetCurrentValue(SelectedValueProperty, selectedValue);
            }

            this.SyncContainersFromCurrentSelection();
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Applies the selection represented by <see cref="SelectedValue" />.
    /// </summary>
    private void ApplySelectionFromSelectedValue()
    {
        this._isSynchronizingSelection = true;
        try
        {
            object? selectedItem = this.FindSelectedItemFromSelectedValue(this.SelectedValue);

            if (!Equals(this.SelectedItem, selectedItem))
            {
                this.SetCurrentValue(SelectedItemProperty, selectedItem);
            }

            this.SyncContainersFromCurrentSelection();
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Applies the active selection state after items or templates have changed.
    /// </summary>
    private void ApplySelectionFromCurrentState()
    {
        if (this.SelectedValue is not null)
        {
            this.ApplySelectionFromSelectedValue();
        }
        else
        {
            this.ApplySelectionFromSelectedItem();
        }
    }

    /// <summary>
    /// Commits the selected item and value for the specified container.
    /// </summary>
    /// <param name="container">The selected container.</param>
    /// <param name="item">The raw item.</param>
    private void CommitSelection(XRadioGroupItem container, object item)
    {
        this._isSynchronizingSelection = true;
        try
        {
            this.SetCurrentValue(SelectedItemProperty, GetSelectedItemForItem(container, item));
            this.SetCurrentValue(SelectedValueProperty, GetSelectedValueForItem(container, item));
            this.SyncContainersFromCurrentSelection();
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Clears the current selection.
    /// </summary>
    private void ClearSelection()
    {
        this._isSynchronizingSelection = true;
        try
        {
            this.SetCurrentValue(SelectedItemProperty, null);
            this.SetCurrentValue(SelectedValueProperty, null);
            this.SyncContainersFromCurrentSelection();
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Synchronizes the realized containers from the current selection.
    /// </summary>
    private void SyncContainersFromCurrentSelection()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is not XRadioGroupItem container)
            {
                continue;
            }

            object item = this.Items[index];
            container.IsChecked = this.IsContainerSelected(container, item);
        }
    }

    /// <summary>
    /// Determines whether the specified container represents the active selection.
    /// </summary>
    /// <param name="container">The item container.</param>
    /// <param name="item">The raw item.</param>
    /// <returns><c>true</c> when the container is selected; otherwise <c>false</c>.</returns>
    private bool IsContainerSelected(XRadioGroupItem container, object item)
    {
        return this.SelectedValue is not null
            ? Equals(this.SelectedValue, GetSelectedValueForItem(container, item))
            : Equals(this.SelectedItem, GetSelectedItemForItem(container, item));
    }

    /// <summary>
    /// Finds the selected value that belongs to the specified selected item.
    /// </summary>
    /// <param name="selectedItem">The selected item.</param>
    /// <returns>The corresponding selected value.</returns>
    private object? FindSelectedValueFromSelectedItem(object? selectedItem)
    {
        if (selectedItem is null)
        {
            return null;
        }

        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is not XRadioGroupItem container)
            {
                continue;
            }

            object item = this.Items[index];
            object itemSelection = GetSelectedItemForItem(container, item);

            if (Equals(selectedItem, itemSelection))
            {
                return GetSelectedValueForItem(container, item);
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the selected item that belongs to the specified selected value.
    /// </summary>
    /// <param name="selectedValue">The selected value.</param>
    /// <returns>The corresponding selected item.</returns>
    private object? FindSelectedItemFromSelectedValue(object? selectedValue)
    {
        if (selectedValue is null)
        {
            return null;
        }

        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is not XRadioGroupItem container)
            {
                continue;
            }

            object item = this.Items[index];
            object itemValue = GetSelectedValueForItem(container, item);

            if (Equals(selectedValue, itemValue))
            {
                return GetSelectedItemForItem(container, item);
            }
        }

        return null;
    }

    /// <summary>
    /// Updates the margins of all realized item containers.
    /// </summary>
    private void UpdateContainerMargins()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is XRadioGroupItem container)
            {
                this.ApplyContainerMargin(container, index);
            }
        }
    }

    /// <summary>
    /// Applies the correct margin to the specified container.
    /// </summary>
    /// <param name="container">The target container.</param>
    private void ApplyContainerMargin(XRadioGroupItem container)
    {
        int index = this.ItemContainerGenerator.IndexFromContainer(container);
        this.ApplyContainerMargin(container, index);
    }

    /// <summary>
    /// Applies the correct margin to the specified container.
    /// </summary>
    /// <param name="container">The target container.</param>
    /// <param name="index">The container index.</param>
    private void ApplyContainerMargin(XRadioGroupItem container, int index)
    {
        double spacing = Math.Max(0d, this.ItemSpacing);
        bool isLast = index >= 0 && index == this.Items.Count - 1;

        Thickness margin = this.Orientation == Orientation.Horizontal
            ? new Thickness(0d, 0d, isLast ? 0d : spacing, 0d)
            : new Thickness(0d, 0d, 0d, isLast ? 0d : spacing);

        container.Margin = margin;
    }

    /// <summary>
    /// Gets the selected item corresponding to the given item.
    /// </summary>
    /// <param name="container">The realized item container.</param>
    /// <param name="item">The raw item.</param>
    /// <returns>The value used in <see cref="SelectedItem" />.</returns>
    private static object GetSelectedItemForItem(XRadioGroupItem container, object item)
    {
        return item is XRadioGroupItem ? container : item;
    }

    /// <summary>
    /// Gets the selected value corresponding to the given item.
    /// </summary>
    /// <param name="container">The realized item container.</param>
    /// <param name="item">The raw item.</param>
    /// <returns>The value used in <see cref="SelectedValue" />.</returns>
    private static object GetSelectedValueForItem(XRadioGroupItem container, object item)
    {
        return item is XRadioGroupItem
            ? container.Value ?? container.Content ?? container
            : item;
    }
    #endregion
}
#endregion
