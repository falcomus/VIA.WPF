using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Controls;

/// <summary>Displays the semantic colors supplied by an application in a compact, readable palette.</summary>
public class XColorSchemeView : Control
{
    public static readonly DependencyProperty PrimaryBrushProperty = RegisterBrush(nameof(PrimaryBrush));
    public static readonly DependencyProperty AccentBrushProperty = RegisterBrush(nameof(AccentBrush));
    public static readonly DependencyProperty InfoBrushProperty = RegisterBrush(nameof(InfoBrush));
    public static readonly DependencyProperty SuccessBrushProperty = RegisterBrush(nameof(SuccessBrush));
    public static readonly DependencyProperty WarningBrushProperty = RegisterBrush(nameof(WarningBrush));
    public static readonly DependencyProperty DangerBrushProperty = RegisterBrush(nameof(DangerBrush));
    public static readonly DependencyProperty NeutralBrushProperty = RegisterBrush(nameof(NeutralBrush));
    public static readonly DependencyProperty TextBrushProperty = RegisterBrush(nameof(TextBrush));
    public static readonly DependencyProperty ControlBackgroundBrushProperty = RegisterBrush(nameof(ControlBackgroundBrush));
    public static readonly DependencyProperty ControlBorderBrushProperty = RegisterBrush(nameof(ControlBorderBrush));
    public static readonly DependencyProperty SchemeBorderBrushProperty = RegisterBrush(nameof(SchemeBorderBrush));
    static XColorSchemeView() => DefaultStyleKeyProperty.OverrideMetadata(typeof(XColorSchemeView), new FrameworkPropertyMetadata(typeof(XColorSchemeView)));
    public Brush? PrimaryBrush { get => (Brush?)GetValue(PrimaryBrushProperty); set => SetValue(PrimaryBrushProperty, value); }
    public Brush? AccentBrush { get => (Brush?)GetValue(AccentBrushProperty); set => SetValue(AccentBrushProperty, value); }
    public Brush? InfoBrush { get => (Brush?)GetValue(InfoBrushProperty); set => SetValue(InfoBrushProperty, value); }
    public Brush? SuccessBrush { get => (Brush?)GetValue(SuccessBrushProperty); set => SetValue(SuccessBrushProperty, value); }
    public Brush? WarningBrush { get => (Brush?)GetValue(WarningBrushProperty); set => SetValue(WarningBrushProperty, value); }
    public Brush? DangerBrush { get => (Brush?)GetValue(DangerBrushProperty); set => SetValue(DangerBrushProperty, value); }
    public Brush? NeutralBrush { get => (Brush?)GetValue(NeutralBrushProperty); set => SetValue(NeutralBrushProperty, value); }
    public Brush? TextBrush { get => (Brush?)GetValue(TextBrushProperty); set => SetValue(TextBrushProperty, value); }
    public Brush? ControlBackgroundBrush { get => (Brush?)GetValue(ControlBackgroundBrushProperty); set => SetValue(ControlBackgroundBrushProperty, value); }
    public Brush? ControlBorderBrush { get => (Brush?)GetValue(ControlBorderBrushProperty); set => SetValue(ControlBorderBrushProperty, value); }
    public Brush? SchemeBorderBrush { get => (Brush?)GetValue(SchemeBorderBrushProperty); set => SetValue(SchemeBorderBrushProperty, value); }
    private static DependencyProperty RegisterBrush(string name) => DependencyProperty.Register(name, typeof(Brush), typeof(XColorSchemeView), new FrameworkPropertyMetadata(null));
}
