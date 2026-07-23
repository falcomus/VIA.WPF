// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSliderDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XSliderDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XSlider showcase page.
/// </summary>
public sealed class XSliderDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private double _balance;
    private double _brightness = 72d;
    private double _precision = 0.42d;
    private double _stepValue = 40d;
    private double _temperature = 58d;
    private double _volume = 64d;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XSlider";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates XSlider header, description, value display, value hint, variants, size presets, ticks, snapping, orientation, disabled state and track customization.";

    /// <summary>
    /// Gets or sets the demo volume value.
    /// </summary>
    public double Volume
    {
        get => _volume;
        set
        {
            if (SetProperty(ref _volume, value))
            {
                OnPropertyChanged(nameof(VolumeSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets the precision sample value.
    /// </summary>
    public double Precision
    {
        get => _precision;
        set => SetProperty(ref _precision, value);
    }

    /// <summary>
    /// Gets or sets the balance sample value.
    /// </summary>
    public double Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    /// <summary>
    /// Gets or sets the brightness sample value.
    /// </summary>
    public double Brightness
    {
        get => _brightness;
        set => SetProperty(ref _brightness, value);
    }

    /// <summary>
    /// Gets or sets the temperature sample value.
    /// </summary>
    public double Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    /// <summary>
    /// Gets or sets the step control sample value.
    /// </summary>
    public double StepValue
    {
        get => _stepValue;
        set => SetProperty(ref _stepValue, value);
    }

    /// <summary>
    /// Gets a short summary for the volume sample.
    /// </summary>
    public string VolumeSummary => $"Volume: {Volume:F0}";

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<via:XSlider
    Header="Volume"
    Description="The value is bound and shown in the header row."
    Minimum="0"
    Maximum="100"
    ShowValue="True"
    Value="{Binding Volume, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    ValueFormatString="F0" />

<via:XSlider
    Header="Brightness"
    Description="Drag the thumb to see the badge hint."
    Minimum="0"
    Maximum="100"
    ShowValue="True"
    ShowValueHint="True"
    ValueHintVariant="Info"
    ValueHintAppearance="Solid"
    Variant="Info"
    Value="{Binding Brightness, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<via:XSlider
    Header="Zoom"
    Minimum="50"
    Maximum="200"
    TickFrequency="25"
    TickPlacement="BottomRight"
    IsSnapToTickEnabled="True"
    ShowValue="True"
    Value="125" />

<via:XSlider Header="Small" Size="Small" ShowValue="True" Value="35" />
<via:XSlider Header="Medium" Size="Medium" ShowValue="True" Value="55" />
<via:XSlider Header="Large" Size="Large" ShowValue="True" Value="75" />

<via:XSlider
    Header="Primary track"
    ActiveTrackBrush="{DynamicResource {x:Static via:XBrushKeys.Primary}}"
    InactiveTrackBrush="{DynamicResource {x:Static via:XBrushKeys.SurfaceLight}}"
    ThumbBrush="{DynamicResource {x:Static via:XBrushKeys.Primary}}"
    TrackThickness="7"
    ThumbSize="22"
    ShowValue="True"
    Value="62" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
private double _volume = 64d;
private double _brightness = 72d;

public double Volume
{
    get => _volume;
    set
    {
        if (SetProperty(ref _volume, value))
        {
            OnPropertyChanged(nameof(VolumeSummary));
        }
    }
}

public double Brightness
{
    get => _brightness;
    set => SetProperty(ref _brightness, value);
}

public string VolumeSummary => $"Volume: {Volume:F0}";
""";
    #endregion
}
#endregion
