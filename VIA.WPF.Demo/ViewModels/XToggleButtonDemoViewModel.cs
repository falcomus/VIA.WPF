// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleButtonDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XToggleButtonDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XToggleButton showcase page.
/// </summary>
public sealed class XToggleButtonDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private bool _isPreviewMode = true;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XToggleButton";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Button-like state control for on/off options, filters, icon states and MVVM-bound boolean values.";

    /// <summary>
    /// Gets or sets a value indicating whether preview mode is active.
    /// </summary>
    public bool IsPreviewMode
    {
        get => _isPreviewMode;
        set
        {
            if (SetProperty(ref _isPreviewMode, value))
            {
                OnPropertyChanged(nameof(PreviewModeSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary for the preview mode sample.
    /// </summary>
    public string PreviewModeSummary => IsPreviewMode
        ? "Preview mode is active."
        : "Preview mode is inactive.";

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<!-- Default unchecked state with a stronger checked state. -->
<via:XToggleButton
    Content="Preview mode"
    IsChecked="{Binding IsPreviewMode, Mode=TwoWay}"
    UncheckedIcon="{via:MaterialIcon Kind=EyeOffOutline}"
    CheckedIcon="{via:MaterialIcon Kind=Eye}" />

<!-- Checked state can use a different semantic variant and appearance. -->
<via:XToggleButton
    Content="Warnings"
    IsChecked="True"
    CheckedVariant="Warning"
    CheckedAppearance="Subtle" />

<!-- Icon-only toggles need a tooltip and an accessible name. -->
<via:XToggleButton
    AutomationProperties.Name="Preview visibility"
    ToolTip="Preview visibility"
    IsChecked="True"
    UncheckedIcon="{via:MaterialIcon Kind=EyeOffOutline}"
    CheckedIcon="{via:MaterialIcon Kind=Eye}" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
private bool _isPreviewMode = true;

public bool IsPreviewMode
{
    get => _isPreviewMode;
    set
    {
        if (SetProperty(ref _isPreviewMode, value))
        {
            OnPropertyChanged(nameof(PreviewModeSummary));
        }
    }
}

public string PreviewModeSummary => IsPreviewMode
    ? "Preview mode is active."
    : "Preview mode is inactive.";
""";
    #endregion
}
#endregion
