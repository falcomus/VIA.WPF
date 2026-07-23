// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XProgressBarDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XProgressBarDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XProgressBar showcase page.
/// </summary>
public sealed class XProgressBarDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private double _exportProgress = 45d;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XProgressBarDemoViewModel"/> class.
    /// </summary>
    public XProgressBarDemoViewModel()
    {
        IncreaseProgressCommand = new RelayCommand(IncreaseProgress);
        DecreaseProgressCommand = new RelayCommand(DecreaseProgress);
        ResetProgressCommand = new RelayCommand(ResetProgress);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XProgressBar";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates XProgressBar header, description, value display, semantic variants, size presets, indeterminate state and MVVM updates.";

    /// <summary>
    /// Gets or sets the export progress sample value.
    /// </summary>
    public double ExportProgress
    {
        get => _exportProgress;
        set
        {
            if (SetProperty(ref _exportProgress, Math.Clamp(value, 0d, 100d)))
            {
                OnPropertyChanged(nameof(ExportProgressSummary));
            }
        }
    }

    /// <summary>
    /// Gets the export progress summary text.
    /// </summary>
    public string ExportProgressSummary => $"Export is {ExportProgress:F0}% complete.";

    /// <summary>
    /// Gets the command that increases the progress value.
    /// </summary>
    public IRelayCommand IncreaseProgressCommand { get; }

    /// <summary>
    /// Gets the command that decreases the progress value.
    /// </summary>
    public IRelayCommand DecreaseProgressCommand { get; }

    /// <summary>
    /// Gets the command that resets the progress value.
    /// </summary>
    public IRelayCommand ResetProgressCommand { get; }

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<via:XProgressBar
    Header="Export progress"
    Description="The value is bound to the view model and can be changed with commands."
    Minimum="0"
    Maximum="100"
    ShowValue="True"
    Value="{Binding ExportProgress, Mode=TwoWay}"
    ValueFormatString="F0"
    Variant="Primary" />

<via:XProgressBar
    Header="Background operation"
    IsIndeterminate="True"
    ShowValue="False"
    Variant="Info" />

<via:XProgressBar Header="Success" ShowValue="True" Value="84" Variant="Success" />
<via:XProgressBar Header="Warning" ShowValue="True" Value="48" Variant="Warning" />
<via:XProgressBar Header="Danger" ShowValue="True" Value="24" Variant="Danger" />

<via:XProgressBar
    Header="Custom brush override"
    ProgressBrush="{DynamicResource {x:Static via:XBrushKeys.Accent}}"
    TrackBrush="{DynamicResource {x:Static via:XBrushKeys.SurfaceLight}}"
    ShowValue="True"
    Value="72" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
public XProgressBarDemoViewModel()
{
    IncreaseProgressCommand = new RelayCommand(IncreaseProgress);
    DecreaseProgressCommand = new RelayCommand(DecreaseProgress);
    ResetProgressCommand = new RelayCommand(ResetProgress);
}

private double _exportProgress = 45d;

public double ExportProgress
{
    get => _exportProgress;
    set
    {
        if (SetProperty(ref _exportProgress, Math.Clamp(value, 0d, 100d)))
        {
            OnPropertyChanged(nameof(ExportProgressSummary));
        }
    }
}

public string ExportProgressSummary => $"Export is {ExportProgress:F0}% complete.";

public IRelayCommand IncreaseProgressCommand { get; }
public IRelayCommand DecreaseProgressCommand { get; }
public IRelayCommand ResetProgressCommand { get; }

private void IncreaseProgress() => ExportProgress += 10d;
private void DecreaseProgress() => ExportProgress -= 10d;
private void ResetProgress() => ExportProgress = 0d;
""";
    #endregion

    #region ### Private Methods ###
    private void IncreaseProgress()
    {
        ExportProgress += 10d;
    }

    private void DecreaseProgress()
    {
        ExportProgress -= 10d;
    }

    private void ResetProgress()
    {
        ExportProgress = 0d;
    }
    #endregion
}
#endregion
