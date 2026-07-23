// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleSwitchDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XToggleSwitchDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XToggleSwitch showcase page.
/// </summary>
public sealed class XToggleSwitchDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private bool _autoSaveEnabled = true;
    private bool _advancedOptionsVisible;
    private bool _cloudBackupEnabled = true;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XToggleSwitch";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates switch-style Boolean settings with size variants, disabled states, a sliding thumb and MVVM binding.";

    /// <summary>
    /// Gets or sets a value indicating whether auto-save is enabled.
    /// </summary>
    public bool AutoSaveEnabled
    {
        get => _autoSaveEnabled;
        set
        {
            if (SetProperty(ref _autoSaveEnabled, value))
            {
                OnPropertyChanged(nameof(AutoSaveSummary));
                OnPropertyChanged(nameof(SettingsSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether advanced options are visible.
    /// </summary>
    public bool AdvancedOptionsVisible
    {
        get => _advancedOptionsVisible;
        set
        {
            if (SetProperty(ref _advancedOptionsVisible, value))
            {
                OnPropertyChanged(nameof(AdvancedOptionsSummary));
                OnPropertyChanged(nameof(SettingsSummary));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether cloud backup is enabled.
    /// </summary>
    public bool CloudBackupEnabled
    {
        get => _cloudBackupEnabled;
        set
        {
            if (SetProperty(ref _cloudBackupEnabled, value))
            {
                OnPropertyChanged(nameof(SettingsSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary for the auto-save sample.
    /// </summary>
    public string AutoSaveSummary => AutoSaveEnabled
        ? "Auto-save is enabled."
        : "Auto-save is disabled.";

    /// <summary>
    /// Gets a short summary for the advanced options sample.
    /// </summary>
    public string AdvancedOptionsSummary => AdvancedOptionsVisible
        ? "Advanced options are visible."
        : "Advanced options are hidden.";

    /// <summary>
    /// Gets a compact summary for the settings panel sample.
    /// </summary>
    public string SettingsSummary => $"Cloud backup: {FormatState(CloudBackupEnabled)}, Auto-save: {FormatState(AutoSaveEnabled)}, Advanced mode: {FormatState(AdvancedOptionsVisible)}.";

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<!-- Basic states -->
<via:XToggleSwitch Content="Enable sync" IsChecked="True" />
<via:XToggleSwitch Content="Send usage diagnostics" />
<via:XToggleSwitch Content="Disabled on" IsChecked="True" IsEnabled="False" />

<!-- Sizes -->
<via:XToggleSwitch Content="Small" Size="Small" />
<via:XToggleSwitch Content="Medium" Size="Medium" />
<via:XToggleSwitch Content="Large" Size="Large" />

<!-- MVVM binding -->
<via:XToggleSwitch
    Content="Auto-save documents"
    IsChecked="{Binding AutoSaveEnabled, Mode=TwoWay}" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
private bool _autoSaveEnabled = true;

public bool AutoSaveEnabled
{
    get => _autoSaveEnabled;
    set
    {
        if (SetProperty(ref _autoSaveEnabled, value))
        {
            OnPropertyChanged(nameof(AutoSaveSummary));
        }
    }
}

public string AutoSaveSummary => AutoSaveEnabled
    ? "Auto-save is enabled."
    : "Auto-save is disabled.";
""";
    #endregion

    #region ### Private Methods ###
    private static string FormatState(bool value)
    {
        return value ? "on" : "off";
    }
    #endregion
}
#endregion
