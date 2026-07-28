using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Controls;

/// <summary>Compact RGBA color picker with a two-way <see cref="SelectedColor"/> property.</summary>
public class XColorPicker : Control
{
    private bool synchronizing;

    public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
        nameof(SelectedColor),
        typeof(Color),
        typeof(XColorPicker),
        new FrameworkPropertyMetadata(
            Colors.Black,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnColorChanged));

    public static readonly DependencyProperty RedProperty = RegisterChannel(nameof(Red), 0);
    public static readonly DependencyProperty GreenProperty = RegisterChannel(nameof(Green), 0);
    public static readonly DependencyProperty BlueProperty = RegisterChannel(nameof(Blue), 0);
    public static readonly DependencyProperty AlphaProperty = RegisterChannel(nameof(Alpha), byte.MaxValue);

    public static readonly DependencyProperty HexProperty = DependencyProperty.Register(
        nameof(Hex),
        typeof(string),
        typeof(XColorPicker),
        new FrameworkPropertyMetadata(
            "#FF000000",
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnHexChanged));

    static XColorPicker() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(XColorPicker),
        new FrameworkPropertyMetadata(typeof(XColorPicker)));

    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public byte Red
    {
        get => (byte)GetValue(RedProperty);
        set => SetValue(RedProperty, value);
    }

    public byte Green
    {
        get => (byte)GetValue(GreenProperty);
        set => SetValue(GreenProperty, value);
    }

    public byte Blue
    {
        get => (byte)GetValue(BlueProperty);
        set => SetValue(BlueProperty, value);
    }

    public byte Alpha
    {
        get => (byte)GetValue(AlphaProperty);
        set => SetValue(AlphaProperty, value);
    }

    public string Hex
    {
        get => (string)GetValue(HexProperty);
        set => SetValue(HexProperty, value);
    }

    private static DependencyProperty RegisterChannel(string name, byte defaultValue) =>
        DependencyProperty.Register(
            name,
            typeof(byte),
            typeof(XColorPicker),
            new FrameworkPropertyMetadata(defaultValue, OnChannelChanged));

    private static void OnColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        ((XColorPicker)dependencyObject).SynchronizeFromColor((Color)eventArgs.NewValue);
    }

    private static void OnChannelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        XColorPicker picker = (XColorPicker)dependencyObject;
        if (!picker.synchronizing)
        {
            picker.SetCurrentValue(
                SelectedColorProperty,
                Color.FromArgb(picker.Alpha, picker.Red, picker.Green, picker.Blue));
        }
    }

    private static void OnHexChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        XColorPicker picker = (XColorPicker)dependencyObject;
        if (!picker.synchronizing && TryParse((string?)eventArgs.NewValue, out Color color))
        {
            picker.SetCurrentValue(SelectedColorProperty, color);
        }
    }

    private void SynchronizeFromColor(Color color)
    {
        try
        {
            this.synchronizing = true;
            this.SetCurrentValue(RedProperty, color.R);
            this.SetCurrentValue(GreenProperty, color.G);
            this.SetCurrentValue(BlueProperty, color.B);
            this.SetCurrentValue(AlphaProperty, color.A);
            this.SetCurrentValue(HexProperty, color.ToString());
        }
        finally
        {
            this.synchronizing = false;
        }
    }

    private static bool TryParse(string? value, out Color color)
    {
        try
        {
            color = (Color)ColorConverter.ConvertFromString(value ?? string.Empty);
            return true;
        }
        catch (FormatException)
        {
            color = default;
            return false;
        }
        catch (NotSupportedException)
        {
            color = default;
            return false;
        }
    }
}
