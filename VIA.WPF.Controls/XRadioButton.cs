// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRadioButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XRadioButton ###
/// <summary>
/// Represents the standard radio button control of VIA.WPF.
/// </summary>
public class XRadioButton : RadioButton
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XRadioButton),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XRadioButton),
        new FrameworkPropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XRadioButton"/> class.
    /// </summary>
    static XRadioButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XRadioButton),
            new FrameworkPropertyMetadata(typeof(XRadioButton)));
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
    /// Gets or sets the semantic color variant used for the selected indicator.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
