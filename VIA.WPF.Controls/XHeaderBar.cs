// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHeaderBar.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XHeaderBar ###
/// <summary>
/// Represents a compact, grouped header bar with a title area and reusable content groups.
/// </summary>
public class XHeaderBar : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="TitleIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(
        nameof(TitleIcon),
        typeof(object),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Subtitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Breadcrumb"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BreadcrumbProperty = DependencyProperty.Register(
        nameof(Breadcrumb),
        typeof(object),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="BreadcrumbTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BreadcrumbTemplateProperty = DependencyProperty.Register(
        nameof(BreadcrumbTemplate),
        typeof(DataTemplate),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Actions"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
        nameof(Actions),
        typeof(object),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ActionsTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionsTemplateProperty = DependencyProperty.Register(
        nameof(ActionsTemplate),
        typeof(DataTemplate),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MoreMenu"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MoreMenuProperty = DependencyProperty.Register(
        nameof(MoreMenu),
        typeof(ContextMenu),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TitleWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
        nameof(TitleWidth),
        typeof(GridLength),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(new GridLength(230d)));

    /// <summary>
    /// Identifies the <see cref="HeaderHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
        nameof(HeaderHeight),
        typeof(double),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(20d));

    /// <summary>
    /// Identifies the <see cref="ContentHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
        nameof(ContentHeight),
        typeof(double),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(34d));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(XControlSize.Large));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XHeaderBar),
        new FrameworkPropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XHeaderBar"/> class.
    /// </summary>
    static XHeaderBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XHeaderBar), new FrameworkPropertyMetadata(typeof(XHeaderBar)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the header bar title.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional title icon.
    /// </summary>
    public object? TitleIcon
    {
        get => GetValue(TitleIconProperty);
        set => SetValue(TitleIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional subtitle.
    /// </summary>
    public string Subtitle
    {
        get => (string)this.GetValue(SubtitleProperty);
        set => this.SetValue(SubtitleProperty, value);
    }

    /// <summary>
    /// Gets or sets optional breadcrumb content shown above the main header row.
    /// </summary>
    public object? Breadcrumb
    {
        get => this.GetValue(BreadcrumbProperty);
        set => this.SetValue(BreadcrumbProperty, value);
    }

    /// <summary>
    /// Gets or sets the breadcrumb content template.
    /// </summary>
    public DataTemplate? BreadcrumbTemplate
    {
        get => (DataTemplate?)this.GetValue(BreadcrumbTemplateProperty);
        set => this.SetValue(BreadcrumbTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets right-aligned header actions.
    /// </summary>
    public object? Actions
    {
        get => this.GetValue(ActionsProperty);
        set => this.SetValue(ActionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the header actions template.
    /// </summary>
    public DataTemplate? ActionsTemplate
    {
        get => (DataTemplate?)this.GetValue(ActionsTemplateProperty);
        set => this.SetValue(ActionsTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional right-aligned overflow menu.
    /// </summary>
    public ContextMenu? MoreMenu
    {
        get => (ContextMenu?)this.GetValue(MoreMenuProperty);
        set => this.SetValue(MoreMenuProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the title area.
    /// </summary>
    public GridLength TitleWidth
    {
        get => (GridLength)GetValue(TitleWidthProperty);
        set => SetValue(TitleWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the optional group header row.
    /// </summary>
    public double HeaderHeight
    {
        get => (double)GetValue(HeaderHeightProperty);
        set => SetValue(HeaderHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the header bar content row.
    /// </summary>
    public double ContentHeight
    {
        get => (double)GetValue(ContentHeightProperty);
        set => SetValue(ContentHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic header bar size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic header bar variant.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
