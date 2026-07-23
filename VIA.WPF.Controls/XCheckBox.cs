// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XCheckBox ###
/// <summary>
/// Represents the standard check box control of VIA.WPF.
/// </summary>
public class XCheckBox : CheckBox
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XCheckBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallCornerRadius));
    //new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XCheckBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XCheckBox),
        new FrameworkPropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XCheckBox"/> class.
    /// </summary>
    static XCheckBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XCheckBox),
            new FrameworkPropertyMetadata(typeof(XCheckBox)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius of the check indicator.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size of the control.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used for the checked indicator.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
