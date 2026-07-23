// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSeparator.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XSeparator ###
/// <summary>
/// Represents a simple themed separator that can be displayed horizontally or vertically.
/// </summary>
public class XSeparator : Control
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XSeparator),
        new FrameworkPropertyMetadata(
            Orientation.Vertical,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XSeparator),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="LineThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
        nameof(LineThickness),
        typeof(double),
        typeof(XSeparator),
        new FrameworkPropertyMetadata(
            1d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XSeparator"/> class.
    /// </summary>
    static XSeparator()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XSeparator),
            new FrameworkPropertyMetadata(typeof(XSeparator)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XSeparator"/> class.
    /// </summary>
    public XSeparator()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the separator orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used by the separator line.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual thickness of the separator line.
    /// </summary>
    public double LineThickness
    {
        get => (double)this.GetValue(LineThicknessProperty);
        set => this.SetValue(LineThicknessProperty, value);
    }
    #endregion
}
#endregion
