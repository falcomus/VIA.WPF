// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationListDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XNavigationListDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XNavigationList showcase page.
/// </summary>
public sealed partial class XNavigationListDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    [ObservableProperty]
    private int _selectedPrimaryIndex = 1;

    [ObservableProperty]
    private int _selectedSecondaryIndex = 0;
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XNavigationList";

    /// <inheritdoc/>
    public override string Description => "Demonstrates a dedicated navigation list with header, footer, icon items and configurable item layout.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XNavigationList
    Header="Application"
    Footer="Signed in as demo.user"
    ItemCornerRadius="10"
    ItemMargin="0,0,0,6"
    ItemPadding="14,11"
    SelectedIndex="{Binding SelectedIndex, Mode=TwoWay}">
    <via:XNavigationListItem Icon="{via:MaterialIcon Kind=ViewDashboardOutline}" Title="Dashboard" />
    <via:XNavigationListItem Icon="{via:MaterialIcon Kind=AccountGroupOutline}" Title="Customers" />
    <via:XNavigationListItem Icon="{via:MaterialIcon Kind=PackageVariantClosed}" Title="Orders" />
</via:XNavigationList>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XNavigationList navigationList = new()
{
    Header = "Application",
    Footer = "Signed in as demo.user",
    ItemPadding = new Thickness(14d, 11d, 14d, 11d),
    ItemMargin = new Thickness(0d, 0d, 0d, 6d),
    ItemCornerRadius = new CornerRadius(10d),
};

navigationList.Items.Add(new XNavigationListItem
{
    Title = "Dashboard",
    Icon = new MaterialIconExtension { Kind = PackIconMaterialKind.ViewDashboardOutline }.ProvideValue(null),
});
""";
    #endregion
}
#endregion
