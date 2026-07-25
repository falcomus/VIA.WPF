using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace VIA.WPF.Controls;

/// <summary>Represents one editable, named color token in an application color scheme.</summary>
public class XColorToken : INotifyPropertyChanged
{
    private Color color;
    private string hex;

    public XColorToken(string label, string key, Color color)
    {
        this.Label = label;
        this.Key = key;
        this.color = color;
        this.hex = color.ToString();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Label { get; }
    public string Key { get; }

    public Color Color
    {
        get => this.color;
        set
        {
            if (this.color == value)
            {
                return;
            }

            this.color = value;
            this.hex = value.ToString();
            this.OnPropertyChanged();
            this.OnPropertyChanged(nameof(this.Hex));
            this.OnPropertyChanged(nameof(this.Brush));
        }
    }

    public string Hex
    {
        get => this.hex;
        set
        {
            if (TryParse(value, out Color color))
            {
                this.Color = color;
            }
        }
    }

    public Brush Brush => new SolidColorBrush(this.Color);

    private static bool TryParse(string? value, out Color color)
    {
        try { color = (Color)ColorConverter.ConvertFromString(value ?? string.Empty); return true; }
        catch (FormatException) { color = default; return false; }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
