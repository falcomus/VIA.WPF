using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

/// <summary>Provides a compact workbench zoom control with a slider and reset action.</summary>
public class XZoomSlider : Control
{
    public static readonly DependencyProperty ZoomPercentProperty = DependencyProperty.Register(nameof(ZoomPercent), typeof(int), typeof(XZoomSlider), new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public static readonly DependencyProperty MinZoomPercentProperty = DependencyProperty.Register(nameof(MinZoomPercent), typeof(int), typeof(XZoomSlider), new FrameworkPropertyMetadata(20));
    public static readonly DependencyProperty MaxZoomPercentProperty = DependencyProperty.Register(nameof(MaxZoomPercent), typeof(int), typeof(XZoomSlider), new FrameworkPropertyMetadata(200));
    public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(nameof(TickFrequency), typeof(double), typeof(XZoomSlider), new FrameworkPropertyMetadata(5d));
    public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register(nameof(IsSnapToTickEnabled), typeof(bool), typeof(XZoomSlider), new FrameworkPropertyMetadata(false));
    public static readonly DependencyProperty ShowValueHintProperty = DependencyProperty.Register(nameof(ShowValueHint), typeof(bool), typeof(XZoomSlider), new FrameworkPropertyMetadata(false));
    public static readonly DependencyProperty ResetValueProperty = DependencyProperty.Register(nameof(ResetValue), typeof(int), typeof(XZoomSlider), new FrameworkPropertyMetadata(100));
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(nameof(ShowResetButton), typeof(bool), typeof(XZoomSlider), new FrameworkPropertyMetadata(true));
    public static readonly DependencyProperty ResetButtonToolTipProperty = DependencyProperty.Register(nameof(ResetButtonToolTip), typeof(object), typeof(XZoomSlider), new FrameworkPropertyMetadata("Reset zoom"));

    public static readonly RoutedUICommand ResetZoomCommand = new("Reset zoom", nameof(ResetZoomCommand), typeof(XZoomSlider));

    static XZoomSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XZoomSlider), new FrameworkPropertyMetadata(typeof(XZoomSlider)));
        CommandManager.RegisterClassCommandBinding(typeof(XZoomSlider), new CommandBinding(ResetZoomCommand, static (sender, _) => ((XZoomSlider)sender).ResetZoom()));
    }

    public int ZoomPercent { get => (int)GetValue(ZoomPercentProperty); set => SetValue(ZoomPercentProperty, value); }
    public int MinZoomPercent { get => (int)GetValue(MinZoomPercentProperty); set => SetValue(MinZoomPercentProperty, value); }
    public int MaxZoomPercent { get => (int)GetValue(MaxZoomPercentProperty); set => SetValue(MaxZoomPercentProperty, value); }
    public double TickFrequency { get => (double)GetValue(TickFrequencyProperty); set => SetValue(TickFrequencyProperty, value); }
    public bool IsSnapToTickEnabled { get => (bool)GetValue(IsSnapToTickEnabledProperty); set => SetValue(IsSnapToTickEnabledProperty, value); }
    public bool ShowValueHint { get => (bool)GetValue(ShowValueHintProperty); set => SetValue(ShowValueHintProperty, value); }
    public int ResetValue { get => (int)GetValue(ResetValueProperty); set => SetValue(ResetValueProperty, value); }
    public bool ShowResetButton { get => (bool)GetValue(ShowResetButtonProperty); set => SetValue(ShowResetButtonProperty, value); }
    public object ResetButtonToolTip { get => GetValue(ResetButtonToolTipProperty); set => SetValue(ResetButtonToolTipProperty, value); }

    private void ResetZoom() => ZoomPercent = Math.Clamp(ResetValue, MinZoomPercent, MaxZoomPercent);
}
