// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBadge.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XBadge ###
/// <summary>
/// Represents a compact status badge with VIA.WPF variant, appearance and size styling.
/// </summary>
public class XBadge : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XBadge),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XBadge),
        new FrameworkPropertyMetadata(XControlAppearance.VerySubtle));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XBadge),
        new FrameworkPropertyMetadata(XControlSize.Small));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XBadge),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Elevation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XBadge),
        new FrameworkPropertyMetadata(XElevation.None));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XBadge"/> class.
    /// </summary>
    static XBadge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XBadge), new FrameworkPropertyMetadata(typeof(XBadge)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XBadge"/> class.
    /// </summary>
    public XBadge()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the semantic color variant.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    public XControlAppearance Appearance
    {
        get => (XControlAppearance)this.GetValue(AppearanceProperty);
        set => this.SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the badge corner radius.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual elevation shadow.
    /// </summary>
    public XElevation Elevation
    {
        get => (XElevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }
    #endregion
}
#endregion
