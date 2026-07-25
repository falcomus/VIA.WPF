using System.Windows.Media;
namespace VIA.WPF.Demo.ViewModels;
public sealed class XColorSchemeViewDemoViewModel : DemoPageViewModel
{
 public override string Title => "XColorSchemeView"; public override string Description => "Semantic palette preview for application-defined color schemes.";
 public Brush Primary => new SolidColorBrush(Color.FromRgb(53,92,145)); public Brush Accent => new SolidColorBrush(Color.FromRgb(109,91,158)); public Brush Info => new SolidColorBrush(Color.FromRgb(0,124,131)); public Brush Success => new SolidColorBrush(Color.FromRgb(16,124,65)); public Brush Warning => new SolidColorBrush(Color.FromRgb(180,83,9)); public Brush Danger => new SolidColorBrush(Color.FromRgb(196,43,28)); public Brush Neutral => new SolidColorBrush(Color.FromRgb(82,96,109)); public Brush Text => new SolidColorBrush(Color.FromRgb(28,35,44)); public Brush ControlBackground => new SolidColorBrush(Color.FromRgb(254,254,254)); public Brush ControlBorder => new SolidColorBrush(Color.FromRgb(166,174,184)); public Brush Border => new SolidColorBrush(Color.FromRgb(197,204,213)); public override string XamlCode => "<via:XColorSchemeView PrimaryBrush=\"{Binding Primary}\" AccentBrush=\"{Binding Accent}\" />"; public override string CSharpCode => "Provide application semantic brushes through the XColorSchemeView properties.";
}
