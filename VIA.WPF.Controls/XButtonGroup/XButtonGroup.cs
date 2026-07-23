


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButtonGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XButtonGroup ###
/// <summary>
/// Represents a segmented button group with single selection support.
/// </summary>
public class XButtonGroup : ItemsControl
{
    #region ### Private Fields ###
    private bool _isSynchronizingSelection;
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="SelectedItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue),
        typeof(object),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedIndex"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
        nameof(SelectedIndex),
        typeof(int),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));

    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(new CornerRadius(4d), FrameworkPropertyMetadataOptions.AffectsRender, OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(XControlSize.Medium, OnItemVisualPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(XControlVariant.Primary, OnItemVisualPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ItemMinWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemMinWidthProperty = DependencyProperty.Register(
        nameof(ItemMinWidth),
        typeof(double),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(34d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnItemVisualPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ItemHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
        nameof(ItemHeight),
        typeof(double),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnItemVisualPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, OnItemVisualPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IconPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(XIconPlacement),
        typeof(XButtonGroup),
        new FrameworkPropertyMetadata(XIconPlacement.Left, OnItemVisualPropertyChanged));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XButtonGroup"/> class.
    /// </summary>
    static XButtonGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XButtonGroup),
            new FrameworkPropertyMetadata(typeof(XButtonGroup)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the selected item.
    /// For explicit item declarations, this is the selected <see cref="XButtonGroupItem"/>.
    /// For data-bound usage, this is the selected data item.
    /// </summary>
    public object? SelectedItem
    {
        get => this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected value.
    /// For explicit item declarations this usually maps to <see cref="XButtonGroupItem.Value"/>.
    /// </summary>
    public object? SelectedValue
    {
        get => this.GetValue(SelectedValueProperty);
        set => this.SetValue(SelectedValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected item index.
    /// </summary>
    public int SelectedIndex
    {
        get => (int)this.GetValue(SelectedIndexProperty);
        set => this.SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// Gets or sets the layout orientation of the group items.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the outer corner radius of the segmented group.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size applied to generated items.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used for the selected item.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum width applied to generated items.
    /// </summary>
    public double ItemMinWidth
    {
        get => (double)this.GetValue(ItemMinWidthProperty);
        set => this.SetValue(ItemMinWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the explicit height applied to generated items.
    /// Use <see cref="double.NaN"/> for automatic height.
    /// </summary>
    public double ItemHeight
    {
        get => (double)this.GetValue(ItemHeightProperty);
        set => this.SetValue(ItemHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon size applied to generated items.
    /// </summary>
    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon placement applied to generated items.
    /// </summary>
    public XIconPlacement IconPlacement
    {
        get => (XIconPlacement)this.GetValue(IconPlacementProperty);
        set => this.SetValue(IconPlacementProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XButtonGroupItem();
    }

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XButtonGroupItem;
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XButtonGroupItem container)
        {
            return;
        }

        container.Click -= this.OnContainerClick;
        container.Click += this.OnContainerClick;

        this.ApplyContainerVisuals(container);
        this.ApplyContainerLayout(container);
        this.SyncContainerSelection(container);
    }

    /// <inheritdoc/>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XButtonGroupItem container)
        {
            container.Click -= this.OnContainerClick;
            container.ClearValue(MarginProperty);
        }

        base.ClearContainerForItemOverride(element, item);
    }

    /// <inheritdoc/>
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        this.UpdateContainerVisuals();
        this.UpdateContainerLayout();
        this.SyncSelectionAfterItemsChanged();
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.UpdateContainerVisuals();
        this.UpdateContainerLayout();
        this.SyncSelectionAfterItemsChanged();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles changes of the <see cref="SelectedItem"/> dependency property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XButtonGroup group && !group._isSynchronizingSelection)
        {
            group.ApplySelectionFromSelectedItem();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedValue"/> dependency property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XButtonGroup group && !group._isSynchronizingSelection)
        {
            group.ApplySelectionFromSelectedValue();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="SelectedIndex"/> dependency property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XButtonGroup group && !group._isSynchronizingSelection)
        {
            group.CommitSelectionFromIndex((int)e.NewValue);
        }
    }

    /// <summary>
    /// Handles layout-related dependency property changes.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XButtonGroup group)
        {
            group.UpdateContainerLayout();
        }
    }

    /// <summary>
    /// Handles visual dependency property changes that are propagated to generated items.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnItemVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XButtonGroup group)
        {
            group.UpdateContainerVisuals();
        }
    }

    /// <summary>
    /// Handles item clicks.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    private void OnContainerClick(object sender, RoutedEventArgs e)
    {
        if (sender is XButtonGroupItem container)
        {
            int index = this.ItemContainerGenerator.IndexFromContainer(container);

            if (index < 0 && this.Items.Contains(container))
            {
                index = this.Items.IndexOf(container);
            }

            this.CommitSelectionFromIndex(index);
        }
    }

    /// <summary>
    /// Applies the current selection after the items collection changed.
    /// </summary>
    private void SyncSelectionAfterItemsChanged()
    {
        if (this.SelectedValue is not null)
        {
            this.ApplySelectionFromSelectedValue();
            return;
        }

        if (this.SelectedItem is not null)
        {
            this.ApplySelectionFromSelectedItem();
            return;
        }

        if (this.SelectedIndex >= 0)
        {
            this.CommitSelectionFromIndex(this.SelectedIndex);
            return;
        }

        this.SyncContainersFromSelectedIndex(-1);
    }

    /// <summary>
    /// Selects the item matching the current <see cref="SelectedValue"/>.
    /// </summary>
    private void ApplySelectionFromSelectedValue()
    {
        int index = this.FindIndexBySelectedValue(this.SelectedValue);

        this._isSynchronizingSelection = true;
        try
        {
            this.SetCurrentValue(SelectedIndexProperty, index);
            this.SetCurrentValue(SelectedItemProperty, index >= 0 ? this.GetSelectedItemForIndex(index) : null);
            this.SyncContainersFromSelectedIndex(index);
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Selects the item matching the current <see cref="SelectedItem"/>.
    /// </summary>
    private void ApplySelectionFromSelectedItem()
    {
        int index = this.FindIndexBySelectedItem(this.SelectedItem);

        this._isSynchronizingSelection = true;
        try
        {
            this.SetCurrentValue(SelectedIndexProperty, index);
            this.SetCurrentValue(SelectedValueProperty, index >= 0 ? this.GetSelectedValueForIndex(index) : null);
            this.SyncContainersFromSelectedIndex(index);
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Commits the selection for the specified index and updates all selection properties.
    /// </summary>
    /// <param name="index">The selected index.</param>
    private void CommitSelectionFromIndex(int index)
    {
        int normalizedIndex = index >= 0 && index < this.Items.Count ? index : -1;

        this._isSynchronizingSelection = true;
        try
        {
            this.SetCurrentValue(SelectedIndexProperty, normalizedIndex);
            this.SetCurrentValue(SelectedItemProperty, normalizedIndex >= 0 ? this.GetSelectedItemForIndex(normalizedIndex) : null);
            this.SetCurrentValue(SelectedValueProperty, normalizedIndex >= 0 ? this.GetSelectedValueForIndex(normalizedIndex) : null);
            this.SyncContainersFromSelectedIndex(normalizedIndex);
        }
        finally
        {
            this._isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Synchronizes all realized containers from the specified selected index.
    /// </summary>
    /// <param name="selectedIndex">The selected index.</param>
    private void SyncContainersFromSelectedIndex(int selectedIndex)
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.GetContainerForIndex(index) is not XButtonGroupItem container)
            {
                continue;
            }

            bool isSelected = index == selectedIndex;
            container.SetCurrentValue(XButtonGroupItem.IsSelectedProperty, isSelected);
            Panel.SetZIndex(container, isSelected ? 10 : 0);
        }
    }

    /// <summary>
    /// Synchronizes a single container from the current selected index.
    /// </summary>
    /// <param name="container">The target container.</param>
    private void SyncContainerSelection(XButtonGroupItem container)
    {
        int index = this.ItemContainerGenerator.IndexFromContainer(container);

        if (index < 0 && this.Items.Contains(container))
        {
            index = this.Items.IndexOf(container);
        }

        bool isSelected = index >= 0 && index == this.SelectedIndex;
        container.SetCurrentValue(XButtonGroupItem.IsSelectedProperty, isSelected);
        Panel.SetZIndex(container, isSelected ? 10 : 0);
    }

    /// <summary>
    /// Updates visual properties for all realized item containers.
    /// </summary>
    private void UpdateContainerVisuals()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.GetContainerForIndex(index) is XButtonGroupItem container)
            {
                this.ApplyContainerVisuals(container);
            }
        }
    }

    /// <summary>
    /// Applies visual properties to the specified item container.
    /// </summary>
    /// <param name="container">The target container.</param>
    private void ApplyContainerVisuals(XButtonGroupItem container)
    {
        this.SetInheritedCurrentValue(container, XButton.SizeProperty, this.Size);
        this.SetInheritedCurrentValue(container, XButton.VariantProperty, this.Variant);
        this.SetInheritedCurrentValue(container, XButton.IconPlacementProperty, this.IconPlacement);
        this.SetInheritedCurrentValue(container, XButton.IconSizeProperty, this.IconSize);
        this.SetInheritedCurrentValue(container, FrameworkElement.MinWidthProperty, Math.Max(0d, this.ItemMinWidth));
        this.SetInheritedCurrentValue(container, FrameworkElement.HeightProperty, this.ItemHeight);
    }

    /// <summary>
    /// Updates layout properties for all realized item containers.
    /// </summary>
    private void UpdateContainerLayout()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.GetContainerForIndex(index) is XButtonGroupItem container)
            {
                this.ApplyContainerLayout(container, index);
            }
        }
    }

    /// <summary>
    /// Applies layout properties to the specified item container.
    /// </summary>
    /// <param name="container">The target container.</param>
    private void ApplyContainerLayout(XButtonGroupItem container)
    {
        int index = this.ItemContainerGenerator.IndexFromContainer(container);

        if (index < 0 && this.Items.Contains(container))
        {
            index = this.Items.IndexOf(container);
        }

        this.ApplyContainerLayout(container, index);
    }

    /// <summary>
    /// Applies layout properties to the specified item container.
    /// </summary>
    /// <param name="container">The target container.</param>
    /// <param name="index">The container index.</param>
    private void ApplyContainerLayout(XButtonGroupItem container, int index)
    {
        container.SetCurrentValue(XButton.CornerRadiusProperty, this.GetCornerRadiusForIndex(index));
        container.SetCurrentValue(BorderThicknessProperty, new Thickness(1d));
        container.Margin = this.GetMarginForIndex(index);
    }

    /// <summary>
    /// Gets the corner radius for the item at the specified index.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The corner radius.</returns>
    private CornerRadius GetCornerRadiusForIndex(int index)
    {
        if (index < 0 || this.Items.Count <= 1)
        {
            return this.CornerRadius;
        }

        bool isFirst = index == 0;
        bool isLast = index == this.Items.Count - 1;

        if (this.Orientation == Orientation.Horizontal)
        {
            return new CornerRadius(
                isFirst ? this.CornerRadius.TopLeft : 0d,
                isLast ? this.CornerRadius.TopRight : 0d,
                isLast ? this.CornerRadius.BottomRight : 0d,
                isFirst ? this.CornerRadius.BottomLeft : 0d);
        }

        return new CornerRadius(
            isFirst ? this.CornerRadius.TopLeft : 0d,
            isFirst ? this.CornerRadius.TopRight : 0d,
            isLast ? this.CornerRadius.BottomRight : 0d,
            isLast ? this.CornerRadius.BottomLeft : 0d);
    }

    /// <summary>
    /// Gets the margin for the item at the specified index.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The margin.</returns>
    private Thickness GetMarginForIndex(int index)
    {
        if (index <= 0)
        {
            return new Thickness(0d);
        }

        return this.Orientation == Orientation.Horizontal
            ? new Thickness(-1d, 0d, 0d, 0d)
            : new Thickness(0d, -1d, 0d, 0d);
    }

    /// <summary>
    /// Finds an item index by selected value.
    /// </summary>
    /// <param name="selectedValue">The selected value.</param>
    /// <returns>The matching index, or -1.</returns>
    private int FindIndexBySelectedValue(object? selectedValue)
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (Equals(this.GetSelectedValueForIndex(index), selectedValue))
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// Finds an item index by selected item.
    /// </summary>
    /// <param name="selectedItem">The selected item.</param>
    /// <returns>The matching index, or -1.</returns>
    private int FindIndexBySelectedItem(object? selectedItem)
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (Equals(this.GetSelectedItemForIndex(index), selectedItem))
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the selection item for the specified index.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The selection item.</returns>
    private object? GetSelectedItemForIndex(int index)
    {
        if (index < 0 || index >= this.Items.Count)
        {
            return null;
        }

        object item = this.Items[index];
        return item is XButtonGroupItem itemContainer ? itemContainer : item;
    }

    /// <summary>
    /// Gets the selection value for the specified index.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The selection value.</returns>
    private object? GetSelectedValueForIndex(int index)
    {
        if (index < 0 || index >= this.Items.Count)
        {
            return null;
        }

        object item = this.Items[index];
        XButtonGroupItem? container = this.GetContainerForIndex(index);

        if (container?.Value is not null)
        {
            return container.Value;
        }

        if (item is XButtonGroupItem itemContainer && itemContainer.Value is not null)
        {
            return itemContainer.Value;
        }

        return item is XButtonGroupItem ? item : item;
    }

    /// <summary>
    /// Gets the realized container for the specified index, or the explicit item if it already is a container.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The matching container, or null.</returns>
    private XButtonGroupItem? GetContainerForIndex(int index)
    {
        if (index < 0 || index >= this.Items.Count)
        {
            return null;
        }

        if (this.ItemContainerGenerator.ContainerFromIndex(index) is XButtonGroupItem container)
        {
            return container;
        }

        return this.Items[index] as XButtonGroupItem;
    }

    /// <summary>
    /// Sets a propagated current value if the target property was not explicitly set on the item.
    /// </summary>
    /// <param name="container">The target container.</param>
    /// <param name="property">The target dependency property.</param>
    /// <param name="value">The propagated value.</param>
    private void SetInheritedCurrentValue(XButtonGroupItem container, DependencyProperty property, object value)
    {
        if (container.ReadLocalValue(property) == DependencyProperty.UnsetValue)
        {
            container.SetCurrentValue(property, value);
        }
    }
    #endregion
}
#endregion
