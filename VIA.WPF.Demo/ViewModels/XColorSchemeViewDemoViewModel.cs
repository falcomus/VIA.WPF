using System.Collections.ObjectModel;
using System.Windows.Media;
using VIA.WPF.Controls;

namespace VIA.WPF.Demo.ViewModels;

public sealed class XColorSchemeViewDemoViewModel : DemoPageViewModel
{
    public XColorSchemeViewDemoViewModel()
    {
        this.Tokens =
        [
            new XColorToken("Primary", "Primary", Color.FromRgb(53, 92, 145)),
            new XColorToken("Accent", "Accent", Color.FromRgb(109, 91, 158)),
            new XColorToken("Info", "Info", Color.FromRgb(0, 124, 131)),
            new XColorToken("Success", "Success", Color.FromRgb(16, 124, 65)),
            new XColorToken("Warning", "Warning", Color.FromRgb(180, 83, 9)),
            new XColorToken("Danger", "Danger", Color.FromRgb(196, 43, 28))
        ];
    }

    public override string Title => "Theme bearbeiten";
    public override string Description => "Bestehende Themes auswählen, Farben für hell und dunkel ändern oder ein neues Theme anlegen.";
    public ObservableCollection<XColorToken> Tokens { get; } = [];
    public Brush Primary => new SolidColorBrush(Color.FromRgb(53, 92, 145));
    public Brush Accent => new SolidColorBrush(Color.FromRgb(109, 91, 158));
    public Brush Info => new SolidColorBrush(Color.FromRgb(0, 124, 131));
    public Brush Success => new SolidColorBrush(Color.FromRgb(16, 124, 65));
    public Brush Warning => new SolidColorBrush(Color.FromRgb(180, 83, 9));
    public Brush Danger => new SolidColorBrush(Color.FromRgb(196, 43, 28));
    public Brush Neutral => new SolidColorBrush(Color.FromRgb(82, 96, 109));
    public Brush Text => new SolidColorBrush(Color.FromRgb(28, 35, 44));
    public Brush ControlBackground => new SolidColorBrush(Color.FromRgb(254, 254, 254));
    public Brush ControlBorder => new SolidColorBrush(Color.FromRgb(166, 174, 184));
    public Brush Border => new SolidColorBrush(Color.FromRgb(197, 204, 213));
    public override string XamlCode => "<via:XColorSchemeEditor ItemsSource=\"{Binding Tokens}\" />";
    public override string CSharpCode => "Create XColorToken entries and bind them to XColorSchemeEditor.ItemsSource.";
}
