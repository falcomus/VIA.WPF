// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationList.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XNavigationList ###
/// <summary>
/// Represents a selectable navigation list.
/// </summary>
public class XNavigationList : ListBox
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty =
        DependencyProperty.Register(
            nameof(Variant),
            typeof(XNavigationListVariant),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(XNavigationListVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(XNavigationList),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(XNavigationList),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Footer"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterProperty =
        DependencyProperty.Register(
            nameof(Footer),
            typeof(object),
            typeof(XNavigationList),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterTemplateProperty =
        DependencyProperty.Register(
            nameof(FooterTemplate),
            typeof(DataTemplate),
            typeof(XNavigationList),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="HeaderSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderSpacingProperty =
        DependencyProperty.Register(
            nameof(HeaderSpacing),
            typeof(Thickness),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 16), FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="FooterSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterSpacingProperty =
        DependencyProperty.Register(
            nameof(FooterSpacing),
            typeof(Thickness),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new Thickness(0, 16, 0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ItemPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemPaddingProperty =
        DependencyProperty.Register(
            nameof(ItemPadding),
            typeof(Thickness),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new Thickness(20, 14, 20, 14), FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ItemMargin"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemMarginProperty =
        DependencyProperty.Register(
            nameof(ItemMargin),
            typeof(Thickness),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 8), FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ItemCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemCornerRadiusProperty =
        DependencyProperty.Register(
            nameof(ItemCornerRadius),
            typeof(CornerRadius),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new CornerRadius(10), FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="SelectedItemBackground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemBackgroundProperty =
        DependencyProperty.Register(
            nameof(SelectedItemBackground),
            typeof(Brush),
            typeof(XNavigationList),
            new PropertyMetadata(Brushes.Transparent));

    /// <summary>
    /// Identifies the <see cref="SelectedItemForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemForegroundProperty =
        DependencyProperty.Register(
            nameof(SelectedItemForeground),
            typeof(Brush),
            typeof(XNavigationList),
            new PropertyMetadata(Brushes.White));

    /// <summary>
    /// Identifies the <see cref="ItemHoverBackground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemHoverBackgroundProperty =
        DependencyProperty.Register(
            nameof(ItemHoverBackground),
            typeof(Brush),
            typeof(XNavigationList),
            new PropertyMetadata(Brushes.Transparent));

    /// <summary>
    /// Identifies the <see cref="ShowSelectionIndicator"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSelectionIndicatorProperty =
        DependencyProperty.Register(
            nameof(ShowSelectionIndicator),
            typeof(bool),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorBrushProperty =
        DependencyProperty.Register(
            nameof(SelectionIndicatorBrush),
            typeof(Brush),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorWidthProperty =
        DependencyProperty.Register(
            nameof(SelectionIndicatorWidth),
            typeof(double),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(3d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectionIndicatorCornerRadiusProperty =
        DependencyProperty.Register(
            nameof(SelectionIndicatorCornerRadius),
            typeof(CornerRadius),
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(new CornerRadius(0d, 2d, 2d, 0d), FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XNavigationList"/> class.
    /// </summary>
    static XNavigationList()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XNavigationList),
            new FrameworkPropertyMetadata(typeof(XNavigationList)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visual surface variant.
    /// </summary>
    public XNavigationListVariant Variant
    {
        get => (XNavigationListVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional header content.
    /// </summary>
    public object? Header
    {
        get => this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional header template.
    /// </summary>
    public DataTemplate? HeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderTemplateProperty);
        set => this.SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional footer content.
    /// </summary>
    public object? Footer
    {
        get => this.GetValue(FooterProperty);
        set => this.SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional footer template.
    /// </summary>
    public DataTemplate? FooterTemplate
    {
        get => (DataTemplate?)this.GetValue(FooterTemplateProperty);
        set => this.SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the list background.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the header spacing.
    /// </summary>
    public Thickness HeaderSpacing
    {
        get => (Thickness)this.GetValue(HeaderSpacingProperty);
        set => this.SetValue(HeaderSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer spacing.
    /// </summary>
    public Thickness FooterSpacing
    {
        get => (Thickness)this.GetValue(FooterSpacingProperty);
        set => this.SetValue(FooterSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to list items.
    /// </summary>
    public Thickness ItemPadding
    {
        get => (Thickness)this.GetValue(ItemPaddingProperty);
        set => this.SetValue(ItemPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin applied to list items.
    /// </summary>
    public Thickness ItemMargin
    {
        get => (Thickness)this.GetValue(ItemMarginProperty);
        set => this.SetValue(ItemMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius applied to list items.
    /// </summary>
    public CornerRadius ItemCornerRadius
    {
        get => (CornerRadius)this.GetValue(ItemCornerRadiusProperty);
        set => this.SetValue(ItemCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected item background brush.
    /// </summary>
    public Brush? SelectedItemBackground
    {
        get => (Brush?)this.GetValue(SelectedItemBackgroundProperty);
        set => this.SetValue(SelectedItemBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected item foreground brush.
    /// </summary>
    public Brush? SelectedItemForeground
    {
        get => (Brush?)this.GetValue(SelectedItemForegroundProperty);
        set => this.SetValue(SelectedItemForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the item hover background brush.
    /// </summary>
    public Brush? ItemHoverBackground
    {
        get => (Brush?)this.GetValue(ItemHoverBackgroundProperty);
        set => this.SetValue(ItemHoverBackgroundProperty, value);
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
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XNavigationListItem;
    }

    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XNavigationListItem();
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XNavigationListItem listItem)
        {
            return;
        }

        if (ReferenceEquals(element, item))
        {
            return;
        }

        this.BindListItem(listItem, item);
    }

    /// <inheritdoc/>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XNavigationListItem listItem && !ReferenceEquals(element, item))
        {
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.TitleProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.SubTitleProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.IconProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.ShowBadgeProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.BadgeContentProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.BadgeVariantProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.ShowEditProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.EditCommandProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.EditCommandParameterProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.ShowDeleteProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.DeleteCommandProperty, item);
            ClearBindingIfOwnedByItem(listItem, XNavigationListItem.DeleteCommandParameterProperty, item);
            ClearBindingIfOwnedByItem(listItem, IsEnabledProperty, item);
            ClearBindingIfOwnedByItem(listItem, VisibilityProperty, item);
        }

        base.ClearContainerForItemOverride(element, item);
    }
    #endregion

    #region ### Private Methods ###
    private void BindListItem(XNavigationListItem listItem, object item)
    {
        if (!HasExplicitValueOrBinding(listItem, XNavigationListItem.TitleProperty))
        {
            string? titlePath = GetFirstReadableProperty(item, "Title", "DisplayName", "Name");
            BindingOperations.SetBinding(
                listItem,
                XNavigationListItem.TitleProperty,
                CreateItemBinding(item, titlePath ?? ".", item.ToString() ?? string.Empty));
        }

        if (!HasExplicitValueOrBinding(listItem, XNavigationListItem.SubTitleProperty))
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
                    XNavigationListItem.SubTitleProperty,
                    CreateItemBinding(item, subTitlePath, null));
            }
        }

        BindIfReadable(listItem, XNavigationListItem.IconProperty, item, "Icon", null);
        BindIfReadable(listItem, XNavigationListItem.ShowBadgeProperty, item, "ShowBadge", false);
        BindIfReadable(listItem, XNavigationListItem.BadgeContentProperty, item, "BadgeContent", null);
        BindIfReadable(listItem, XNavigationListItem.BadgeVariantProperty, item, "BadgeVariant", XControlVariant.Accent);
        BindIfReadable(listItem, XNavigationListItem.ShowEditProperty, item, "ShowEdit", false);
        BindIfReadable(listItem, XNavigationListItem.EditCommandProperty, item, "EditCommand", null);
        BindIfReadable(listItem, XNavigationListItem.EditCommandParameterProperty, item, "EditCommandParameter", null);
        BindIfReadable(listItem, XNavigationListItem.ShowDeleteProperty, item, "ShowDelete", false);
        BindIfReadable(listItem, XNavigationListItem.DeleteCommandProperty, item, "DeleteCommand", null);
        BindIfReadable(listItem, XNavigationListItem.DeleteCommandParameterProperty, item, "DeleteCommandParameter", null);
        BindIfReadable(listItem, IsEnabledProperty, item, "IsEnabled", true);

        if (!HasExplicitValueOrBinding(listItem, VisibilityProperty)
            && HasReadableProperty(item, "IsVisible"))
        {
            BindingOperations.SetBinding(
                listItem,
                VisibilityProperty,
                new Binding("IsVisible")
                {
                    Source = item,
                    Mode = BindingMode.OneWay,
                    Converter = new BooleanToVisibilityConverter(),
                    FallbackValue = Visibility.Visible,
                    TargetNullValue = Visibility.Visible,
                });
        }
    }

    private static void BindIfReadable(
        DependencyObject target,
        DependencyProperty dependencyProperty,
        object item,
        string propertyName,
        object? fallbackValue)
    {
        if (HasExplicitValueOrBinding(target, dependencyProperty)
            || !HasReadableProperty(item, propertyName))
        {
            return;
        }

        BindingOperations.SetBinding(
            target,
            dependencyProperty,
            CreateItemBinding(item, propertyName, fallbackValue));
    }

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

    private static bool HasExplicitValueOrBinding(DependencyObject target, DependencyProperty dependencyProperty)
    {
        return target.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue
            || BindingOperations.IsDataBound(target, dependencyProperty);
    }

    private static bool HasReadableProperty(object item, string propertyName)
    {
        return item.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
            ?.CanRead == true;
    }

    private static string? GetFirstReadableProperty(object item, params string[] propertyNames)
    {
        return propertyNames.FirstOrDefault(propertyName => HasReadableProperty(item, propertyName));
    }

    private static void ClearBindingIfOwnedByItem(DependencyObject dependencyObject, DependencyProperty dependencyProperty, object item)
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
