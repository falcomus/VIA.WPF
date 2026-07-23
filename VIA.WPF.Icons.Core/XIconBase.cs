// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Icons;

#region ### Class XIconBase ###
/// <summary>
/// Provides the shared base implementation for strongly typed VIA.WPF icon controls.
/// </summary>
/// <typeparam name="TKind">The enum type used by the icon control.</typeparam>
public abstract class XIconBase<TKind> : Control
    where TKind : struct, Enum
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
        nameof(Kind),
        typeof(TKind),
        typeof(XIconBase<TKind>),
        new FrameworkPropertyMetadata(default(TKind)));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(double),
        typeof(XIconBase<TKind>),
        new FrameworkPropertyMetadata(16d));

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
        nameof(Stretch),
        typeof(Stretch),
        typeof(XIconBase<TKind>),
        new FrameworkPropertyMetadata(Stretch.Uniform));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XIconBase{TKind}"/> class.
    /// </summary>
    static XIconBase()
    {
        ForegroundProperty.OverrideMetadata(
            typeof(XIconBase<TKind>),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XIconBase{TKind}"/> class.
    /// </summary>
    protected XIconBase()
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the strongly typed icon kind.
    /// </summary>
    public TKind Kind
    {
        get => (TKind)this.GetValue(KindProperty);
        set => this.SetValue(KindProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double Size
    {
        get => (double)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon stretch mode.
    /// </summary>
    public Stretch Stretch
    {
        get => (Stretch)this.GetValue(StretchProperty);
        set => this.SetValue(StretchProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Overrides the default style key for the specified icon control type.
    /// </summary>
    /// <typeparam name="TIcon">The icon control type.</typeparam>
    protected static void OverrideDefaultStyleKey<TIcon>()
        where TIcon : XIconBase<TKind>
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TIcon),
            new FrameworkPropertyMetadata(typeof(TIcon)));
    }
    #endregion
}
#endregion
