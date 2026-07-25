namespace VIA.WPF.Demo.ViewModels;

public sealed class XZoomSliderDemoViewModel : DemoPageViewModel
{
    private int zoomPercent = 100;
    public override string Title => "XZoomSlider";
    public override string Description => "Compact workbench zoom control with continuous or discrete movement and a reset action.";
    public int ZoomPercent { get => zoomPercent; set => SetProperty(ref zoomPercent, value); }
    public override string XamlCode => "<via:XZoomSlider MinZoomPercent=\"50\" MaxZoomPercent=\"200\" ShowValueHint=\"True\" ZoomPercent=\"{Binding ZoomPercent, Mode=TwoWay}\" />";
    public override string CSharpCode => "public int ZoomPercent { get => zoomPercent; set => SetProperty(ref zoomPercent, value); }";
}
