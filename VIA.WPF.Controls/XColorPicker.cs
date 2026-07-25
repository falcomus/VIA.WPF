using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Controls;

/// <summary>Compact RGBA color picker with a two-way <see cref="SelectedColor"/> property.</summary>
public class XColorPicker : Control
{
    private bool synchronizing;

    public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
        nameof(SelectedColor), typeof(Color), typeof(XColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));
    public static readonly DependencyProperty RedProperty = RegisterChannel(nameof(Red));
    public static readonly DependencyProperty GreenProperty = RegisterChannel(nameof(Green));
    public static readonly DependencyProperty BlueProperty = RegisterChannel(nameof(Blue));
    public static readonly DependencyProperty AlphaProperty = RegisterChannel(nameof(Alpha));
    public static readonly DependencyProperty HexProperty = DependencyProperty.Register(
        nameof(Hex), typeof(string), typeof(XColorPicker), new FrameworkPropertyMetadata("#00000000", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHexChanged));

    static XColorPicker() => DefaultStyleKeyProperty.OverrideMetadata(typeof(XColorPicker), new FrameworkPropertyMetadata(typeof(XColorPicker)));

    public Color SelectedColor { get => (Color)GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }
    public byte Red { get => (byte)GetValue(RedProperty); set => SetValue(RedProperty, value); }
    public byte Green { get => (byte)GetValue(GreenProperty); set => SetValue(GreenProperty, value); }
    public byte Blue { get => (byte)GetValue(BlueProperty); set => SetValue(BlueProperty, value); }
    public byte Alpha { get => (byte)GetValue(AlphaProperty); set => SetValue(AlphaProperty, value); }
    public string Hex { get => (string)GetValue(HexProperty); set => SetValue(HexProperty, value); }

    private static DependencyProperty RegisterChannel(string name) => DependencyProperty.Register(name, typeof(byte), typeof(XColorPicker), new FrameworkPropertyMetadata((byte)0, OnChannelChanged));

    private static void OnColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        ((XColorPicker)dependencyObject).SynchronizeFromColor((Color)eventArgs.NewValue);
    }

    private static void OnChannelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        XColorPicker picker = (XColorPicker)dependencyObject;
        if (!picker.synchronizing)
        {
            picker.SelectedColor = Color.FromArgb(picker.Alpha, picker.Red, picker.Green, picker.Blue);
        }
    }

    private static void OnHexChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        XColorPicker picker = (XColorPicker)dependencyObject;
        if (!picker.synchronizing && TryParse((string?)eventArgs.NewValue, out Color color))
        {
            picker.SelectedColor = color;
        }
    }

    private void SynchronizeFromColor(Color color)
    {
        this.synchronizing = true;
        SetCurrentValue(RedProperty, color.R);
        SetCurrentValue(GreenProperty, color.G);
        SetCurrentValue(BlueProperty, color.B);
        SetCurrentValue(AlphaProperty, color.A);
        SetCurrentValue(HexProperty, color.ToString());
        this.synchronizing = false;
    }

    private static bool TryParse(string? value, out Color color)
    {
        try { color = (Color)ColorConverter.ConvertFromString(value ?? string.Empty); return true; }
        catch (FormatException) { color = default; return false; }
    }
}
