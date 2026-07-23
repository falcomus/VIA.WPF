// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XInputAssist.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XInputAssist ###
/// <summary>
/// Provides attached properties for internal input-control template coordination.
/// </summary>
public static class XInputAssist
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IconSize attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.RegisterAttached(
        "IconSize",
        typeof(double),
        typeof(XInputAssist),
        new FrameworkPropertyMetadata(
            double.NaN,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the HoverBackground attached dependency property.
    /// </summary>
    public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.RegisterAttached(
        "HoverBackground",
        typeof(Brush),
        typeof(XInputAssist),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the FocusedBackground attached dependency property.
    /// </summary>
    public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.RegisterAttached(
        "FocusedBackground",
        typeof(Brush),
        typeof(XInputAssist),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the HoverBorderBrush attached dependency property.
    /// </summary>
    public static readonly DependencyProperty HoverBorderBrushProperty = DependencyProperty.RegisterAttached(
        "HoverBorderBrush",
        typeof(Brush),
        typeof(XInputAssist),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the FocusedBorderBrush attached dependency property.
    /// </summary>
    public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.RegisterAttached(
        "FocusedBorderBrush",
        typeof(Brush),
        typeof(XInputAssist),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the icon size stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured icon size.</returns>
    public static double GetIconSize(DependencyObject element)
    {
        return (double)element.GetValue(IconSizeProperty);
    }

    /// <summary>
    /// Sets the icon size stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The icon size.</param>
    public static void SetIconSize(DependencyObject element, double value)
    {
        element.SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Gets the hover background brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured hover background brush.</returns>
    public static Brush? GetHoverBackground(DependencyObject element)
    {
        return (Brush?)element.GetValue(HoverBackgroundProperty);
    }

    /// <summary>
    /// Sets the hover background brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The hover background brush.</param>
    public static void SetHoverBackground(DependencyObject element, Brush? value)
    {
        element.SetValue(HoverBackgroundProperty, value);
    }

    /// <summary>
    /// Gets the focused background brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured focused background brush.</returns>
    public static Brush? GetFocusedBackground(DependencyObject element)
    {
        return (Brush?)element.GetValue(FocusedBackgroundProperty);
    }

    /// <summary>
    /// Sets the focused background brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The focused background brush.</param>
    public static void SetFocusedBackground(DependencyObject element, Brush? value)
    {
        element.SetValue(FocusedBackgroundProperty, value);
    }

    /// <summary>
    /// Gets the hover border brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured hover border brush.</returns>
    public static Brush? GetHoverBorderBrush(DependencyObject element)
    {
        return (Brush?)element.GetValue(HoverBorderBrushProperty);
    }

    /// <summary>
    /// Sets the hover border brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The hover border brush.</param>
    public static void SetHoverBorderBrush(DependencyObject element, Brush? value)
    {
        element.SetValue(HoverBorderBrushProperty, value);
    }

    /// <summary>
    /// Gets the focused border brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured focused border brush.</returns>
    public static Brush? GetFocusedBorderBrush(DependencyObject element)
    {
        return (Brush?)element.GetValue(FocusedBorderBrushProperty);
    }

    /// <summary>
    /// Sets the focused border brush stored on the specified element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The focused border brush.</param>
    public static void SetFocusedBorderBrush(DependencyObject element, Brush? value)
    {
        element.SetValue(FocusedBorderBrushProperty, value);
    }
    #endregion
}
#endregion
