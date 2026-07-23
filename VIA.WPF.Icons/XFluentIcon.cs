// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XFluentIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentIcons.Common;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XFluentIcon ###
/// <summary>
/// Represents a strongly typed Fluent icon control of VIA.WPF.
/// </summary>
public class XFluentIcon : XIconBase<Icon>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(Icon),
        typeof(XFluentIcon),
        new FrameworkPropertyMetadata(Icon.AccessTime));

    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = IconProperty;

    /// <summary>
    /// Identifies the <see cref="IconVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconVariantProperty = DependencyProperty.Register(
        nameof(IconVariant),
        typeof(IconVariant),
        typeof(XFluentIcon),
        new FrameworkPropertyMetadata(IconVariant.Regular));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(IconSize),
        typeof(XFluentIcon),
        new FrameworkPropertyMetadata(IconSize.Size24));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<Icon>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<Icon>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XFluentIcon"/> class.
    /// </summary>
    static XFluentIcon()
    {
        OverrideDefaultStyleKey<XFluentIcon>();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the fluent icon glyph.
    /// </summary>
    public Icon Icon
    {
        get => (Icon)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the fluent icon glyph.
    /// </summary>
    public new Icon Kind
    {
        get => this.Icon;
        set => this.Icon = value;
    }

    /// <summary>
    /// Gets or sets the fluent icon variant.
    /// </summary>
    public IconVariant IconVariant
    {
        get => (IconVariant)this.GetValue(IconVariantProperty);
        set => this.SetValue(IconVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the fluent icon size preset.
    /// </summary>
    public IconSize IconSize
    {
        get => (IconSize)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }
    #endregion
}
#endregion
