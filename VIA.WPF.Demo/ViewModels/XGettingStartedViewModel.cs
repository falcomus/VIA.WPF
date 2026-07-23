// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGettingStartedViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XGettingStartedViewModel ###
/// <summary>
/// Represents the Getting Started landing page.
/// </summary>
public sealed class XGettingStartedViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Getting Started";

    /// <inheritdoc/>
    public override string Description => "Installation, setup and first steps with VIA.WPF.";

    /// <inheritdoc/>
    public override string XamlCode => """
<UserControl
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:via="http://schemas.via.dev/wpf">

    <via:XStackPanel Spacing="14">
        <via:XTextBox
            Header="Project name"
            LeadingIcon="{via:MaterialIcon Kind=FolderOutline}"
            Placeholder="Inventory dashboard" />

        <via:XButton
            Content="Save project"
            Icon="{via:MaterialIcon Kind=ContentSaveOutline}"
            Variant="Primary" />
    </via:XStackPanel>
</UserControl>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
using System.Windows;
using VIA.WPF.Themes;

namespace DemoApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        XThemeService.Initialize();
    }
}
""";
    #endregion
}
#endregion
