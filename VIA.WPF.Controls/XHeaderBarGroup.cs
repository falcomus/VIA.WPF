// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHeaderBarGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XHeaderBarGroup ###
/// <summary>
/// Provides compatibility for the former ribbon-like header group. Use <see cref="XHeaderGroup"/> for new views.
/// </summary>
[Obsolete("Use XHeaderGroup instead.")]
public class XHeaderBarGroup : HeaderedContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="HeaderHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
        nameof(HeaderHeight),
        typeof(double),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(18d));

    /// <summary>
    /// Identifies the <see cref="ContentHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
        nameof(ContentHeight),
        typeof(double),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(34d));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(new Thickness(16d, 0d, 16d, 0d)));

    /// <summary>
    /// Identifies the <see cref="ContentPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
        nameof(ContentPadding),
        typeof(Thickness),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(new Thickness(16d, 0d, 16d, 0d)));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(XControlSize.Large));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XHeaderBarGroup),
        new FrameworkPropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XHeaderBarGroup"/> class.
    /// </summary>
    static XHeaderBarGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XHeaderBarGroup), new FrameworkPropertyMetadata(typeof(XHeaderBarGroup)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the height of the group header row.
    /// </summary>
    public double HeaderHeight
    {
        get => (double)GetValue(HeaderHeightProperty);
        set => SetValue(HeaderHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the group content row.
    /// </summary>
    public double ContentHeight
    {
        get => (double)GetValue(ContentHeightProperty);
        set => SetValue(ContentHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the group header padding.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)GetValue(HeaderPaddingProperty);
        set => SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the group content padding.
    /// </summary>
    public Thickness ContentPadding
    {
        get => (Thickness)GetValue(ContentPaddingProperty);
        set => SetValue(ContentPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic group size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic group variant.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
