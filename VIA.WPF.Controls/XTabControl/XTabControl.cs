// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTabControl.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XTabControl ###
/// <summary>
/// Represents the themed tab control of VIA.WPF.
/// </summary>
public class XTabControl : TabControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="HeaderAppearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderAppearanceProperty = DependencyProperty.Register(
        nameof(HeaderAppearance),
        typeof(XTabHeaderAppearance),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(XTabHeaderAppearance.Underlined));

    /// <summary>
    /// Identifies the <see cref="ShowBorder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(
        nameof(ShowBorder),
        typeof(bool),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowTabContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTabContentProperty = DependencyProperty.Register(
        nameof(ShowTabContent),
        typeof(bool),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowAddButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowAddButtonProperty = DependencyProperty.Register(
        nameof(ShowAddButton),
        typeof(bool),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="AddButtonCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AddButtonCommandProperty = DependencyProperty.Register(
        nameof(AddButtonCommand),
        typeof(ICommand),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="AddButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AddButtonContentProperty = DependencyProperty.Register(
        nameof(AddButtonContent),
        typeof(object),
        typeof(XTabControl),
        new FrameworkPropertyMetadata("+"));

    /// <summary>
    /// Identifies the <see cref="ShowTabCloseButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTabCloseButtonProperty = DependencyProperty.Register(
        nameof(ShowTabCloseButton),
        typeof(bool),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(new Thickness(12d, 8d, 12d, 8d)));

    /// <summary>
    /// Identifies the <see cref="HeaderHostPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderHostPaddingProperty = DependencyProperty.Register(
        nameof(HeaderHostPadding),
        typeof(Thickness),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(new Thickness(4d, 2d, 0d, 2d)));

    /// <summary>
    /// Identifies the <see cref="HeaderContentMargin"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentMarginProperty = DependencyProperty.Register(
        nameof(HeaderContentMargin),
        typeof(Thickness),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(new Thickness(4d, 0d, 0d, -2d)));

    /// <summary>
    /// Identifies the <see cref="HeaderUnderlineMargin"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderUnderlineMarginProperty = DependencyProperty.Register(
        nameof(HeaderUnderlineMargin),
        typeof(Thickness),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(new Thickness(4d, 3d, 0d, 0d)));

    /// <summary>
    /// Identifies the <see cref="HeaderSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderSpacingProperty = DependencyProperty.Register(
        nameof(HeaderSpacing),
        typeof(double),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(6d));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(new CornerRadius(8d)));

    /// <summary>
    /// Identifies the <see cref="ItemHeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemHeaderTemplateProperty = DependencyProperty.Register(
        nameof(ItemHeaderTemplate),
        typeof(DataTemplate),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ItemHeaderTemplateSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemHeaderTemplateSelectorProperty = DependencyProperty.Register(
        nameof(ItemHeaderTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(XTabControl),
        new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTabControl"/> class.
    /// </summary>
    static XTabControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTabControl),
            new FrameworkPropertyMetadata(typeof(XTabControl)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the header appearance mode.
    /// </summary>
    public XTabHeaderAppearance HeaderAppearance
    {
        get => (XTabHeaderAppearance)this.GetValue(HeaderAppearanceProperty);
        set => this.SetValue(HeaderAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tab control shows border chrome.
    /// </summary>
    public bool ShowBorder
    {
        get => (bool)this.GetValue(ShowBorderProperty);
        set => this.SetValue(ShowBorderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the selected tab content is shown.
    /// </summary>
    public bool ShowTabContent
    {
        get => (bool)this.GetValue(ShowTabContentProperty);
        set => this.SetValue(ShowTabContentProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the add button is shown.
    /// </summary>
    public bool ShowAddButton
    {
        get => (bool)this.GetValue(ShowAddButtonProperty);
        set => this.SetValue(ShowAddButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the add button.
    /// </summary>
    public ICommand? AddButtonCommand
    {
        get => (ICommand?)this.GetValue(AddButtonCommandProperty);
        set => this.SetValue(AddButtonCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the add button.
    /// </summary>
    public object? AddButtonContent
    {
        get => this.GetValue(AddButtonContentProperty);
        set => this.SetValue(AddButtonContentProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether close buttons are shown on closable tab items.
    /// </summary>
    public bool ShowTabCloseButton
    {
        get => (bool)this.GetValue(ShowTabCloseButtonProperty);
        set => this.SetValue(ShowTabCloseButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to tab headers.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)this.GetValue(HeaderPaddingProperty);
        set => this.SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to the tab header host.
    /// </summary>
    public Thickness HeaderHostPadding
    {
        get => (Thickness)this.GetValue(HeaderHostPaddingProperty);
        set => this.SetValue(HeaderHostPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin applied to the tab header content.
    /// </summary>
    public Thickness HeaderContentMargin
    {
        get => (Thickness)this.GetValue(HeaderContentMarginProperty);
        set => this.SetValue(HeaderContentMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin applied to the selected tab underline.
    /// </summary>
    public Thickness HeaderUnderlineMargin
    {
        get => (Thickness)this.GetValue(HeaderUnderlineMarginProperty);
        set => this.SetValue(HeaderUnderlineMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between tab headers.
    /// </summary>
    public double HeaderSpacing
    {
        get => (double)this.GetValue(HeaderSpacingProperty);
        set => this.SetValue(HeaderSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the control.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the default template used for tab item headers.
    /// </summary>
    public DataTemplate? ItemHeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(ItemHeaderTemplateProperty);
        set => this.SetValue(ItemHeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the selector used to choose a template for tab item headers.
    /// </summary>
    public DataTemplateSelector? ItemHeaderTemplateSelector
    {
        get => (DataTemplateSelector?)this.GetValue(ItemHeaderTemplateSelectorProperty);
        set => this.SetValue(ItemHeaderTemplateSelectorProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Determines whether the specified item is its own container.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns><see langword="true"/> if the item is an <see cref="XTabItem"/>; otherwise <see langword="false"/>.</returns>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XTabItem;
    }

    /// <summary>
    /// Creates or identifies the element that is used to display the specified item.
    /// </summary>
    /// <returns>The container element.</returns>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XTabItem();
    }

    /// <summary>
    /// Prepares the specified element to display the specified item.
    /// </summary>
    /// <param name="element">The element used to display the item.</param>
    /// <param name="item">The item to display.</param>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not XTabItem tabItem)
        {
            return;
        }

        if (tabItem.ReadLocalValue(HeaderedContentControl.HeaderTemplateProperty) == DependencyProperty.UnsetValue)
        {
            BindingOperations.SetBinding(
                tabItem,
                HeaderedContentControl.HeaderTemplateProperty,
                new Binding(nameof(ItemHeaderTemplate))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });
        }

        if (tabItem.ReadLocalValue(HeaderedContentControl.HeaderTemplateSelectorProperty) == DependencyProperty.UnsetValue)
        {
            BindingOperations.SetBinding(
                tabItem,
                HeaderedContentControl.HeaderTemplateSelectorProperty,
                new Binding(nameof(ItemHeaderTemplateSelector))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });
        }
    }

    /// <summary>
    /// Clears all references to the item from the specified element.
    /// </summary>
    /// <param name="element">The container element.</param>
    /// <param name="item">The item that is displayed in the container.</param>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is XTabItem tabItem)
        {
            if (BindingOperations.GetBindingExpression(tabItem, HeaderedContentControl.HeaderTemplateProperty)?.ParentBinding?.Source == this)
            {
                BindingOperations.ClearBinding(tabItem, HeaderedContentControl.HeaderTemplateProperty);
            }

            if (BindingOperations.GetBindingExpression(tabItem, HeaderedContentControl.HeaderTemplateSelectorProperty)?.ParentBinding?.Source == this)
            {
                BindingOperations.ClearBinding(tabItem, HeaderedContentControl.HeaderTemplateSelectorProperty);
            }
        }

        base.ClearContainerForItemOverride(element, item);
    }
    #endregion
}
#endregion
