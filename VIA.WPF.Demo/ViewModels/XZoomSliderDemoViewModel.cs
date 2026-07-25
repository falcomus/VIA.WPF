namespace VIA.WPF.Demo.ViewModels;

public sealed class XZoomSliderDemoViewModel : DemoPageViewModel
{
    private int zoomPercent = 100;
    public override string Title => "XZoomSlider";
    public override string Description => "Compact workbench zoom control with snapping and a reset action.";
    public int ZoomPercent { get => zoomPercent; set => SetProperty(ref zoomPercent, value); }
    public override string XamlCode => "<via:XZoomSlider Minimum=\"50\" Maximum=\"200\" ZoomPercent=\"{Binding ZoomPercent, Mode=TwoWay}\" />";
    public override string CSharpCode => "public int ZoomPercent { get => zoomPercent; set => SetProperty(ref zoomPercent, value); }";
}
