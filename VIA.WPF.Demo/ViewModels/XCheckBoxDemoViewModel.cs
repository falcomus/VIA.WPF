// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XCheckBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XCheckBox showcase page.
/// </summary>
public sealed class XCheckBoxDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private bool _notificationsEnabled = true;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XCheckBox";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates the themed XCheckBox with shared sizing, checked, unchecked, indeterminate and disabled states, and MVVM binding.";

    /// <summary>
    /// Gets or sets a value indicating whether notifications are enabled.
    /// </summary>
    public bool NotificationsEnabled
    {
        get => _notificationsEnabled;
        set
        {
            if (SetProperty(ref _notificationsEnabled, value))
            {
                OnPropertyChanged(nameof(NotificationSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary for the live binding sample.
    /// </summary>
    public string NotificationSummary => NotificationsEnabled
        ? "Notifications are enabled."
        : "Notifications are disabled.";

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<!-- Basic states -->
<via:XCheckBox Content="Enable notifications" />
<via:XCheckBox Content="Remember filters" IsChecked="True" />
<via:XCheckBox Content="Partial selection" IsThreeState="True" IsChecked="{x:Null}" />
<via:XCheckBox Content="Disabled checked" IsChecked="True" IsEnabled="False" />

<!-- Sizes -->
<via:XCheckBox Content="Small option" Size="Small" />
<via:XCheckBox Content="Medium option" Size="Medium" />
<via:XCheckBox Content="Large option" Size="Large" />

<!-- Shape -->
<via:XCheckBox Content="Sharp indicator" CornerRadius="0" />
<via:XCheckBox Content="Rounded indicator" CornerRadius="8" />

<!-- MVVM binding -->
<via:XCheckBox
    Content="Notifications enabled"
    IsChecked="{Binding NotificationsEnabled, Mode=TwoWay}" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
private bool _notificationsEnabled = true;

public bool NotificationsEnabled
{
    get => _notificationsEnabled;
    set
    {
        if (SetProperty(ref _notificationsEnabled, value))
        {
            OnPropertyChanged(nameof(NotificationSummary));
        }
    }
}

public string NotificationSummary => NotificationsEnabled
    ? "Notifications are enabled."
    : "Notifications are disabled.";
""";
    #endregion
}
#endregion
