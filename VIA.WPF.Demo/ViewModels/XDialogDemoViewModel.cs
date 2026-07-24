// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using VIA.WPF.Demo.Views;
using VIA.WPF.Windowing;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XDialogDemoViewModel ###
/// <summary>
/// Represents the view model for the modal dialog showcase page.
/// </summary>
public sealed partial class XDialogDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    [ObservableProperty]
    private string _lastOutcome = "No dialog has been opened yet.";

    [ObservableProperty]
    private string _savedProfile = "No profile has been saved yet.";
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XDialog";

    /// <inheritdoc />
    public override string Description => "Demonstrates service-managed modal XWindow dialogs with deterministic owner resolution, owner dimming, normalized results and focus restoration.";

    /// <inheritdoc />
    public override string XamlCode => """
<via:XButton
    Command="{Binding OpenProfileDialogCommand}"
    CommandParameter="{Binding RelativeSource={RelativeSource Self}}"
    Content="Open profile dialog"
    Icon="{via:MaterialIcon Kind=OpenInNew}"
    Variant="Primary" />

<via:XWindow
    x:Class="MyApp.Views.ProfileDialog"
    Title="Edit profile"
    Width="640"
    ResizeMode="NoResize"
    ShowInTaskbar="False"
    SizeToContent="Height">
    <via:XGrid Rows="*,Auto">
        <!-- Dialog content -->
    </via:XGrid>
</via:XWindow>
""";

    /// <inheritdoc />
    public override string CSharpCode => """
private readonly IXDialogService dialogService = XDialogService.Default;

[RelayCommand]
private void OpenProfileDialog(DependencyObject? ownerSource)
{
    ProfileEditorViewModel editor = new();
    ProfileDialog dialog = new(editor);

    XDialogResult result = this.dialogService.ShowModal(
        dialog,
        ownerSource,
        new XDialogOptions
        {
            DimOwner = true,
            RestoreOwnerFocus = true
        });

    if (result.IsAccepted)
    {
        Save(editor);
    }
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Opens the profile editor with the recommended modal presentation settings.
    /// </summary>
    /// <param name="ownerSource">An element associated with the desired owner window.</param>
    [RelayCommand]
    private void OpenProfileDialog(DependencyObject? ownerSource)
    {
        this.ShowProfileDialog(ownerSource, dimOwner: true);
    }

    /// <summary>
    /// Opens the same editor without dimming the owner.
    /// </summary>
    /// <param name="ownerSource">An element associated with the desired owner window.</param>
    [RelayCommand]
    private void OpenUndimmedDialog(DependencyObject? ownerSource)
    {
        this.ShowProfileDialog(ownerSource, dimOwner: false);
    }
    #endregion

    #region ### Private Methods ###
    private void ShowProfileDialog(DependencyObject? ownerSource, bool dimOwner)
    {
        XDialogProfile profile = new()
        {
            DisplayName = "Alex Morgan",
            Email = "alex.morgan@example.com",
            Role = "Product designer",
            Notes = "Owns the design-system rollout and review workflow."
        };

        XDialogSampleWindow dialog = new(profile);

        XDialogResult result = XDialogService.Default.ShowModal(
            dialog,
            ownerSource,
            new XDialogOptions
            {
                DimOwner = dimOwner,
                RestoreOwnerFocus = true,
                StartupLocation = WindowStartupLocation.CenterOwner
            });

        this.LastOutcome = result.Outcome switch
        {
            XDialogOutcome.Accepted => "Accepted — the dialog returned a normalized success result.",
            XDialogOutcome.NotAccepted => "Cancelled — no edited values were applied.",
            _ => "Closed without a boolean dialog result."
        };

        if (result.IsAccepted)
        {
            this.SavedProfile = $"{profile.DisplayName} · {profile.Role} · {profile.Email}";
        }
    }
    #endregion
}
#endregion

#region ### Class XDialogProfile ###
/// <summary>
/// Represents the editable data used by the modal dialog sample.
/// </summary>
public sealed partial class XDialogProfile : ObservableObject
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [ObservableProperty]
    private string _displayName = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [ObservableProperty]
    private string _email = string.Empty;

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    [ObservableProperty]
    private string _role = string.Empty;

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    [ObservableProperty]
    private string _notes = string.Empty;
    #endregion
}
#endregion