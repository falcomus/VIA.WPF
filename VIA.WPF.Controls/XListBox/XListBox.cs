// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XListBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XListBox ###
/// <summary>
/// Represents a themed list box control of VIA.WPF.
/// </summary>
public class XListBox : ListBox
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Mode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
        nameof(Mode),
        typeof(XListBoxMode),
        typeof(XListBox),
        new FrameworkPropertyMetadata(XListBoxMode.Navigation));

    /// <summary>
    /// Identifies the <see cref="ShowSeparators"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSeparatorsProperty = DependencyProperty.Register(
        nameof(ShowSeparators),
        typeof(bool),
        typeof(XListBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XListBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="ShowSelectionIndicator"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSelectionIndicatorProperty = DependencyProperty.Register(
        nameof(ShowSelectionIndicator),
        typeof(bool),
        typeof(XListBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorBrushProperty = DependencyProperty.Register(
        nameof(SelectionIndicatorBrush),
        typeof(Brush),
        typeof(XListBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorWidthProperty = DependencyProperty.Register(
        nameof(SelectionIndicatorWidth),
        typeof(double),
        typeof(XListBox),
        new FrameworkPropertyMetadata(3d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorCornerRadiusProperty = DependencyProperty.Register(
        nameof(SelectionIndicatorCornerRadius),
        typeof(CornerRadius),
        typeof(XListBox),
        new FrameworkPropertyMetadata(new CornerRadius(0d, 2d, 2d, 0d), FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XListBox"/> class.
    /// </summary>
    static XListBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XListBox),
            new FrameworkPropertyMetadata(typeof(XListBox)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the presentation mode of the list box.
    /// </summary>
    public XListBoxMode Mode
    {
        get => (XListBoxMode)this.GetValue(ModeProperty);
        set => this.SetValue(ModeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether compact navigation items show separators.
    /// </summary>
    public bool ShowSeparators
    {
        get => (bool)this.GetValue(ShowSeparatorsProperty);
        set => this.SetValue(ShowSeparatorsProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual size of the list box items.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether selected items show a leading selection indicator.
    /// </summary>
    public bool ShowSelectionIndicator
    {
        get => (bool)this.GetValue(ShowSelectionIndicatorProperty);
        set => this.SetValue(ShowSelectionIndicatorProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used by the selection indicator.
    /// </summary>
    public Brush? SelectionIndicatorBrush
    {
        get => (Brush?)this.GetValue(SelectionIndicatorBrushProperty);
        set => this.SetValue(SelectionIndicatorBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the selection indicator.
    /// </summary>
    public double SelectionIndicatorWidth
    {
        get => (double)this.GetValue(SelectionIndicatorWidthProperty);
        set => this.SetValue(SelectionIndicatorWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the selection indicator.
    /// </summary>
    public CornerRadius SelectionIndicatorCornerRadius
    {
        get => (CornerRadius)this.GetValue(SelectionIndicatorCornerRadiusProperty);
        set => this.SetValue(SelectionIndicatorCornerRadiusProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XListBoxItem();
    }

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XListBoxItem;
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XListBoxItem listItem || ReferenceEquals(element, item))
        {
            return;
        }

        this.BindListItem(listItem, item);
    }

    /// <inheritdoc/>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XListBoxItem listItem && !ReferenceEquals(element, item))
        {
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.TitleProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.SubTitleProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.ShowBadgeProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.BadgeContentProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.BadgeVariantProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.ShowEditProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.EditCommandProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.EditCommandParameterProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.ShowDeleteProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.DeleteCommandProperty, item);
            ClearBindingIfOwnedByItem(listItem, XListBoxItem.DeleteCommandParameterProperty, item);
        }

        base.ClearContainerForItemOverride(element, item);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Applies conventional bindings to a generated item container.
    /// </summary>
    /// <param name="listItem">The generated item container.</param>
    /// <param name="item">The source item.</param>
    private void BindListItem(XListBoxItem listItem, object item)
    {
        if (listItem.ReadLocalValue(XListBoxItem.TitleProperty) == DependencyProperty.UnsetValue)
        {
            string titlePath = HasReadableProperty(item, "Title")
                ? "Title"
                : HasReadableProperty(item, "Name")
                    ? "Name"
                    : ".";

            BindingOperations.SetBinding(
                listItem,
                XListBoxItem.TitleProperty,
                CreateItemBinding(item, titlePath, item.ToString() ?? string.Empty));
        }

        if (listItem.ReadLocalValue(XListBoxItem.SubTitleProperty) == DependencyProperty.UnsetValue)
        {
            string? subTitlePath = HasReadableProperty(item, "SubTitle")
                ? "SubTitle"
                : HasReadableProperty(item, "Description")
                    ? "Description"
                    : null;

            if (subTitlePath is not null)
            {
                BindingOperations.SetBinding(
                    listItem,
                    XListBoxItem.SubTitleProperty,
                    CreateItemBinding(item, subTitlePath, null));
            }
        }

        BindIfReadable(listItem, XListBoxItem.ShowBadgeProperty, item, "ShowBadge", false);
        BindIfReadable(listItem, XListBoxItem.BadgeContentProperty, item, "BadgeContent", null);
        BindIfReadable(listItem, XListBoxItem.BadgeVariantProperty, item, "BadgeVariant", XControlVariant.Accent);
        BindIfReadable(listItem, XListBoxItem.ShowEditProperty, item, "ShowEdit", false);
        BindIfReadable(listItem, XListBoxItem.EditCommandProperty, item, "EditCommand", null);
        BindIfReadable(listItem, XListBoxItem.EditCommandParameterProperty, item, "EditCommandParameter", null);
        BindIfReadable(listItem, XListBoxItem.ShowDeleteProperty, item, "ShowDelete", false);
        BindIfReadable(listItem, XListBoxItem.DeleteCommandProperty, item, "DeleteCommand", null);
        BindIfReadable(listItem, XListBoxItem.DeleteCommandParameterProperty, item, "DeleteCommandParameter", null);
    }

    /// <summary>
    /// Binds a readable item property when the container has no local value.
    /// </summary>
    private static void BindIfReadable(
        DependencyObject target,
        DependencyProperty dependencyProperty,
        object item,
        string propertyName,
        object? fallbackValue)
    {
        if (target.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue
            || !HasReadableProperty(item, propertyName))
        {
            return;
        }

        BindingOperations.SetBinding(
            target,
            dependencyProperty,
            CreateItemBinding(item, propertyName, fallbackValue));
    }

    /// <summary>
    /// Creates a one-way binding to an item property.
    /// </summary>
    private static Binding CreateItemBinding(object item, string path, object? fallbackValue)
    {
        return new Binding(path)
        {
            Source = item,
            Mode = BindingMode.OneWay,
            FallbackValue = fallbackValue,
            TargetNullValue = fallbackValue,
        };
    }

    /// <summary>
    /// Gets a value indicating whether the item exposes a readable public property.
    /// </summary>
    private static bool HasReadableProperty(object item, string propertyName)
    {
        return item.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
            ?.CanRead == true;
    }

    /// <summary>
    /// Clears a binding only when it was created for the specified source item.
    /// </summary>
    private static void ClearBindingIfOwnedByItem(
        DependencyObject dependencyObject,
        DependencyProperty dependencyProperty,
        object item)
    {
        BindingExpression? bindingExpression = BindingOperations.GetBindingExpression(
            dependencyObject,
            dependencyProperty);

        if (bindingExpression?.ParentBinding.Source is object source && ReferenceEquals(source, item))
        {
            BindingOperations.ClearBinding(dependencyObject, dependencyProperty);
        }
    }
    #endregion
}
#endregion
