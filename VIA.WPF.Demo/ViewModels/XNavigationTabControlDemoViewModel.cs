// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationTabControlDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XNavigationTabControlDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XNavigationTabControl showcase page.
/// </summary>
public sealed partial class XNavigationTabControlDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    [ObservableProperty]
    private int _selectedIndex = 0;
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XNavigationTabControl";

    /// <inheritdoc/>
    public override string Description => "Demonstrates top-level tab navigation without hosted tab content.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XNavigationTabControl SelectedIndex="{Binding SelectedIndex, Mode=TwoWay}">
    <via:XNavigationTabItem Header="Basisdaten" Icon="{via:MaterialIcon Kind=ViewDashboardOutline}" />
    <via:XNavigationTabItem Header="Lagerbuchungen" Icon="{via:MaterialIcon Kind=PackageVariantClosed}" />
    <via:XNavigationTabItem Header="Artikel" Icon="{via:MaterialIcon Kind=InformationOutline}" />
    <via:XNavigationTabItem Header="Berichte" Icon="{via:MaterialIcon Kind=ChartLine}" />
</via:XNavigationTabControl>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XNavigationTabControl navigationTabs = new()
{
    HeaderPadding = new Thickness(14d, 8d, 14d, 8d),
    HeaderSpacing = 10d,
};

navigationTabs.Items.Add(new XNavigationTabItem { Header = "Basisdaten" });
navigationTabs.Items.Add(new XNavigationTabItem { Header = "Artikel" });
""";
    #endregion
}
#endregion
