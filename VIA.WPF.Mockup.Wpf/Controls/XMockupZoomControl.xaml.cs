// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMockupZoomControl.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Mockup.Wpf.Controls;

/// <summary>
/// Provides the reusable zoom toolbar control used by mockup views.
/// </summary>
public partial class XMockupZoomControl : UserControl
{
    /// <summary>
    /// Identifies the <see cref="Zoom"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
        nameof(Zoom),
        typeof(double),
        typeof(XMockupZoomControl),
        new FrameworkPropertyMetadata(
            100d,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            null,
            CoerceZoom));

    /// <summary>
    /// Identifies the <see cref="Minimum"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum),
        typeof(double),
        typeof(XMockupZoomControl),
        new PropertyMetadata(60d, OnZoomRangeChanged));

    /// <summary>
    /// Identifies the <see cref="Maximum"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(double),
        typeof(XMockupZoomControl),
        new PropertyMetadata(120d, OnZoomRangeChanged));

    /// <summary>
    /// Identifies the <see cref="Step"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
        nameof(Step),
        typeof(double),
        typeof(XMockupZoomControl),
        new PropertyMetadata(5d));

    /// <summary>
    /// Initializes a new instance of the <see cref="XMockupZoomControl"/> class.
    /// </summary>
    public XMockupZoomControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the current zoom percentage.
    /// </summary>
    public double Zoom
    {
        get => (double)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum zoom percentage.
    /// </summary>
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum zoom percentage.
    /// </summary>
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets the zoom increment used by the toolbar buttons.
    /// </summary>
    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    private static object CoerceZoom(DependencyObject dependencyObject, object baseValue)
    {
        XMockupZoomControl control = (XMockupZoomControl)dependencyObject;
        double minimum = Math.Min(control.Minimum, control.Maximum);
        double maximum = Math.Max(control.Minimum, control.Maximum);
        return Math.Clamp((double)baseValue, minimum, maximum);
    }

    private static void OnZoomRangeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        dependencyObject.CoerceValue(ZoomProperty);
    }

    private void OnDecreaseClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(ZoomProperty, Zoom - Math.Max(1d, Step));
    }

    private void OnIncreaseClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(ZoomProperty, Zoom + Math.Max(1d, Step));
    }
}
