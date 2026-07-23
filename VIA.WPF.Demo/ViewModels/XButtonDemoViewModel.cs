// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButtonDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XButtonDemoViewModel ###
/// <summary>
/// Represents the view model for the XButton showcase page.
/// </summary>
public sealed partial class XButtonDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    [ObservableProperty]
    private string _lastAction = "No command executed yet.";
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XButton";

    /// <inheritdoc />
    public override string Description => "Demonstrates variants, appearances, sizes, icons, loading state, elevation, layout helpers and command binding for XButton.";

    /// <inheritdoc />
    public override string XamlCode => """
<via:XButton
    Content="Primary"
    Variant="Primary" />

<via:XButton
    Appearance="Outline"
    Content="Download"
    IconPlacement="Left"
    IconSize="16"
    Variant="Info">
    <via:XButton.Icon>
        <via:XIcon Kind="Download" />
    </via:XButton.Icon>
</via:XButton>

<via:XButton
    Appearance="Outline"
    Content="Next step"
    IconPlacement="Right"
    Variant="Primary">
    <via:XButton.Icon>
        <via:XIcon Kind="ArrowRight" Pack="BootstrapIcons" />
    </via:XButton.Icon>
</via:XButton>

<via:XButton
    Width="34"
    Height="34"
    Content=""
    ToolTip="Search"
    Variant="Default">
    <via:XButton.Icon>
        <via:XIcon Kind="Search" />
    </via:XButton.Icon>
</via:XButton>

<via:XButton
    Content="Saving"
    IsLoading="True"
    LoadingContent="Saving..."
    Variant="Primary" />

<via:XButton
    Content="Refresh"
    Icon="{via:MaterialIcon Kind=Refresh}"
    IconSize="16"
    via:XIconAssist.IsRotationAnimated="True"
    via:XIconAssist.RotationAnimationDuration="0:0:0.9"
    Variant="Primary" />

<via:XButton
    Command="{Binding RunPrimaryActionCommand}"
    Content="Run command"
    Elevation="Medium"
    Variant="Primary" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class XButtonDemoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _lastAction = "No command executed yet.";

    [RelayCommand]
    private void RunPrimaryAction()
    {
        this.LastAction = "Primary command executed.";
    }

    [RelayCommand]
    private void RunParameterizedAction(string? action)
    {
        this.LastAction = $"{action ?? "Button"} command executed.";
    }
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Executes the primary demo action.
    /// </summary>
    [RelayCommand]
    private void RunPrimaryAction()
    {
        this.LastAction = "Primary command executed.";
    }

    /// <summary>
    /// Executes a parameterized demo action.
    /// </summary>
    /// <param name="action">The action name.</param>
    [RelayCommand]
    private void RunParameterizedAction(string? action)
    {
        this.LastAction = $"{action ?? "Button"} command executed.";
    }
    #endregion
}
#endregion