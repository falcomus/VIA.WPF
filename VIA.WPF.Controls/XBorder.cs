// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBorder.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XBorder ###
/// <summary>
/// Represents a themed content border with semantic variants, appearance modes, and elevation.
/// </summary>
public class XBorder : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XBorder),
        new FrameworkPropertyMetadata(new CornerRadius(6d)));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XBorder),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XBorder),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="Elevation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XBorder),
        new FrameworkPropertyMetadata(XElevation.None));

    /// <summary>
    /// Identifies the <see cref="Foreground"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(
        typeof(XBorder),
        new FrameworkPropertyMetadata(default(Brush)));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XBorder"/> class.
    /// </summary>
    static XBorder()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XBorder),
            new FrameworkPropertyMetadata(typeof(XBorder)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XBorder"/> class.
    /// </summary>
    public XBorder()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

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
    /// Gets or sets the elevation level.
    /// </summary>
    public XElevation Elevation
    {
        get => (XElevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground brush.
    /// </summary>
    public new Brush Foreground
    {
        get => (Brush)this.GetValue(ForegroundProperty);
        set => this.SetValue(ForegroundProperty, value);
    }
    #endregion
}
#endregion











////   Copyright (c) VIA.WPF. All rights reserved.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;

//namespace VIA.WPF.Controls;

//#region ### Class XBorder ###
///// <summary>
///// Represents a themed content border with semantic variants, appearance modes, and elevation.
///// </summary>
//public class XBorder : ContentControl
//{
//    #region ### Dependency Properties ###
//    /// <summary>
//    /// Identifies the <see cref="CornerRadius"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
//        nameof(CornerRadius),
//        typeof(CornerRadius),
//        typeof(XBorder),
//        new FrameworkPropertyMetadata(new CornerRadius(6d)));

//    /// <summary>
//    /// Identifies the <see cref="Variant"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
//        nameof(Variant),
//        typeof(XControlVariant),
//        typeof(XBorder),
//        new FrameworkPropertyMetadata(XControlVariant.Default));

//    /// <summary>
//    /// Identifies the <see cref="Appearance"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
//        nameof(Appearance),
//        typeof(XControlAppearance),
//        typeof(XBorder),
//        new FrameworkPropertyMetadata(XControlAppearance.Solid));

//    /// <summary>
//    /// Identifies the <see cref="Elevation"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
//        nameof(Elevation),
//        typeof(XElevation),
//        typeof(XBorder),
//        new FrameworkPropertyMetadata(XElevation.None));

//    /// <summary>
//    /// Identifies the <see cref="Foreground"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ForegroundBrushProperty = Control.ForegroundProperty.AddOwner(
//        typeof(XBorder),
//        new FrameworkPropertyMetadata(default(Brush)));
//    #endregion

//    #region ### Constructors ###
//    /// <summary>
//    /// Initializes static members of the <see cref="XBorder"/> class.
//    /// </summary>
//    static XBorder()
//    {
//        DefaultStyleKeyProperty.OverrideMetadata(
//            typeof(XBorder),
//            new FrameworkPropertyMetadata(typeof(XBorder)));
//    }
//    #endregion

//    #region ### Public Properties ###
//    /// <summary>
//    /// Gets or sets the corner radius.
//    /// </summary>
//    public CornerRadius CornerRadius
//    {
//        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
//        set => this.SetValue(CornerRadiusProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the semantic color variant.
//    /// </summary>
//    public XControlVariant Variant
//    {
//        get => (XControlVariant)this.GetValue(VariantProperty);
//        set => this.SetValue(VariantProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the visual appearance.
//    /// </summary>
//    public XControlAppearance Appearance
//    {
//        get => (XControlAppearance)this.GetValue(AppearanceProperty);
//        set => this.SetValue(AppearanceProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the elevation level.
//    /// </summary>
//    public XElevation Elevation
//    {
//        get => (XElevation)this.GetValue(ElevationProperty);
//        set => this.SetValue(ElevationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the foreground brush.
//    /// </summary>
//    public Brush Foreground
//    {
//        get => (Brush)this.GetValue(ForegroundBrushProperty);
//        set => this.SetValue(ForegroundBrushProperty, value);
//    }
//    #endregion
//}
//#endregion









////// <copyright file="XBorder.cs" company="VIA.WPF">
//////   Copyright (c) VIA.WPF. All rights reserved.
////// </copyright>
////// --------------------------------------------------------------------------------------------------------------------

////using System.Windows;
////using System.Windows.Controls;
////using System.Windows.Media;

////namespace VIA.WPF.Controls;

////#region ### Class XBorder ###
/////// <summary>
/////// Represents a themed content border with semantic variants, appearance modes, and elevation.
/////// </summary>
////public class XBorder : ContentControl
////{
////    #region ### Dependency Properties ###
////    /// <summary>
////    /// Identifies the <see cref="CornerRadius"/> dependency property.
////    /// </summary>
////    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
////        nameof(CornerRadius),
////        typeof(CornerRadius),
////        typeof(XBorder),
////        new FrameworkPropertyMetadata(new CornerRadius(6d)));

////    /// <summary>
////    /// Identifies the <see cref="Variant"/> dependency property.
////    /// </summary>
////    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
////        nameof(Variant),
////        typeof(XControlVariant),
////        typeof(XBorder),
////        new FrameworkPropertyMetadata(XControlVariant.Default));

////    /// <summary>
////    /// Identifies the <see cref="Appearance"/> dependency property.
////    /// </summary>
////    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
////        nameof(Appearance),
////        typeof(XControlAppearance),
////        typeof(XBorder),
////        new FrameworkPropertyMetadata(XControlAppearance.Solid));

////    /// <summary>
////    /// Identifies the <see cref="Elevation"/> dependency property.
////    /// </summary>
////    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
////        nameof(Elevation),
////        typeof(XElevation),
////        typeof(XBorder),
////        new FrameworkPropertyMetadata(XElevation.None));

////    /// <summary>
////    /// Identifies the <see cref="Foreground"/> dependency property.
////    /// </summary>
////    public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(
////        typeof(XBorder),
////        new FrameworkPropertyMetadata(default(Brush)));
////    #endregion

////    #region ### Constructors ###
////    /// <summary>
////    /// Initializes static members of the <see cref="XBorder"/> class.
////    /// </summary>
////    static XBorder()
////    {
////        DefaultStyleKeyProperty.OverrideMetadata(
////            typeof(XBorder),
////            new FrameworkPropertyMetadata(typeof(XBorder)));
////    }
////    #endregion

////    #region ### Public Properties ###
////    /// <summary>
////    /// Gets or sets the corner radius.
////    /// </summary>
////    public CornerRadius CornerRadius
////    {
////        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
////        set => this.SetValue(CornerRadiusProperty, value);
////    }

////    /// <summary>
////    /// Gets or sets the semantic color variant.
////    /// </summary>
////    public XControlVariant Variant
////    {
////        get => (XControlVariant)this.GetValue(VariantProperty);
////        set => this.SetValue(VariantProperty, value);
////    }

////    /// <summary>
////    /// Gets or sets the visual appearance.
////    /// </summary>
////    public XControlAppearance Appearance
////    {
////        get => (XControlAppearance)this.GetValue(AppearanceProperty);
////        set => this.SetValue(AppearanceProperty, value);
////    }

////    /// <summary>
////    /// Gets or sets the elevation level.
////    /// </summary>
////    public XElevation Elevation
////    {
////        get => (XElevation)this.GetValue(ElevationProperty);
////        set => this.SetValue(ElevationProperty, value);
////    }

////    /// <summary>
////    /// Gets or sets the foreground brush.
////    /// </summary>
////    public Brush Foreground
////    {
////        get => (Brush)this.GetValue(ForegroundProperty);
////        set => this.SetValue(ForegroundProperty, value);
////    }
////    #endregion
////}
////#endregion