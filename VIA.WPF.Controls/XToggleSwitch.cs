// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleSwitch.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls.Primitives;

namespace VIA.WPF.Controls;

#region ### Class XToggleSwitch ###
/// <summary>
/// Represents the standard toggle switch control of VIA.WPF.
/// </summary>
public class XToggleSwitch : ToggleButton
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XToggleSwitch),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XToggleSwitch),
        new FrameworkPropertyMetadata(XControlVariant.Primary));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToggleSwitch"/> class.
    /// </summary>
    static XToggleSwitch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XToggleSwitch),
            new FrameworkPropertyMetadata(typeof(XToggleSwitch)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the semantic size of the control.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used for the checked switch state.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
