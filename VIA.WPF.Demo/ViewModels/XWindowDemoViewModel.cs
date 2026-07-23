// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XWindowDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XWindow showcase page.
/// </summary>
public sealed class XWindowDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XWindow";

    /// <inheritdoc/>
    public override string Description => "Shows how XWindow acts as the application shell with themed chrome, flyouts, navigation and page hosting.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XWindow
    Title="VIA.WPF Demo"
    Width="1440"
    Height="920"
    ShowThemeModeButton="True"
    ShowThemeSelector="True"
    IsLeftFlyoutOpen="{Binding IsLeftFlyoutOpen, Mode=TwoWay}"
    IsRightFlyoutOpen="{Binding IsRightFlyoutOpen, Mode=TwoWay}">

    <via:XWindow.LeftFlyoutContent>
        <via:XBorder Padding="18" CornerRadius="8" Elevation="Low">
            <via:XTextBlock Text="Navigation" TextRole="Heading" />
        </via:XBorder>
    </via:XWindow.LeftFlyoutContent>

    <via:XGrid Columns="280,*">
        <via:XListBox Mode="CompactNavigation" />
        <ContentControl Grid.Column="1" Content="{Binding SelectedPage}" />
    </via:XGrid>
</via:XWindow>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isLeftFlyoutOpen;

    [ObservableProperty]
    private bool _isRightFlyoutOpen;

    [RelayCommand]
    private void ToggleLeftFlyout()
    {
        this.IsLeftFlyoutOpen = !this.IsLeftFlyoutOpen;
    }
}
""";
    #endregion
}
#endregion
