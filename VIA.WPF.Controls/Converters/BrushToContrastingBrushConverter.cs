using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VIA.WPF.Converters;

public sealed class BrushToContrastingBrushConverter : IValueConverter
{
    public static BrushToContrastingBrushConverter Instance { get; } = new();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush brush) return Brushes.Black;
        Color c = brush.Color;
        double luminance = (0.2126d * c.R) + (0.7152d * c.G) + (0.0722d * c.B);
        return luminance >= 140d ? Brushes.Black : Brushes.White;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
