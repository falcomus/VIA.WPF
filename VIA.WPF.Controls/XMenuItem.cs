// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMenuItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XMenuItem ###
/// <summary>
/// Represents a themed menu item with semantic coloring and configurable icon sizing.
/// </summary>
public class XMenuItem : MenuItem
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty =
        DependencyProperty.Register(
            nameof(Variant),
            typeof(XControlVariant),
            typeof(XMenuItem),
            new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(XMenuItem),
            new FrameworkPropertyMetadata(16d));
    #endregion

    #region ### Constructors ###
    static XMenuItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XMenuItem),
            new FrameworkPropertyMetadata(typeof(XMenuItem)));
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
    /// Gets or sets the displayed icon size.
    /// </summary>
    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }
    #endregion
}
#endregion