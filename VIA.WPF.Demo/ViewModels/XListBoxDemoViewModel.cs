// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XListBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XListBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XListBox showcase page.
/// </summary>
public sealed partial class XListBoxDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    [ObservableProperty]
    private XListBoxDemoItem? _selectedNavigationItem;

    [ObservableProperty]
    private XListBoxDemoItem? _selectedCompactItem;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XListBoxDemoViewModel"/> class.
    /// </summary>
    public XListBoxDemoViewModel()
    {
        this.SelectedNavigationItem = this.NavigationItems[1];
        this.SelectedCompactItem = this.CompactItems[2];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XListBox";

    /// <inheritdoc/>
    public override string Description => "Demonstrates navigation and compact navigation presentation modes, item templates, separators and selected item binding.";

    /// <summary>
    /// Gets the items used by the regular navigation list.
    /// </summary>
    public ObservableCollection<XListBoxDemoItem> NavigationItems { get; } =
    [
        new("Overview", "Entry point and high-level dashboard."),
        new("Controls", "Browse available controls and feature showcases."),
        new("Themes", "Inspect palettes, brushes and application themes."),
        new("Samples", "Practical page layouts and control compositions."),
    ];

    /// <summary>
    /// Gets the items used by the compact navigation list.
    /// </summary>
    public ObservableCollection<XListBoxDemoItem> CompactItems { get; } =
    [
        new("XButton", "Primary action control."),
        new("XTextBox", "Text input with header and icons."),
        new("XListBox", "Navigation list with themed containers."),
        new("XTreeView", "Hierarchical data navigation."),
    ];

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XListBox
    ItemsSource="{Binding NavigationItems}"
    Mode="CompactNavigation"
    SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
    ShowSeparators="True">
    <via:XListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}" />
        </DataTemplate>
    </via:XListBox.ItemTemplate>
</via:XListBox>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XListBox listBox = new()
{
    Mode = XListBoxMode.CompactNavigation,
    ShowSeparators = true,
    ItemsSource = navigationItems,
};

listBox.SetBinding(Selector.SelectedItemProperty, new Binding(nameof(SelectedItem))
{
    Mode = BindingMode.TwoWay,
});
""";
    #endregion
}
#endregion

#region ### Class XListBoxDemoItem ###
/// <summary>
/// Represents a list item in the XListBox demo.
/// </summary>
/// <param name="title">The title.</param>
/// <param name="description">The description.</param>
public sealed class XListBoxDemoItem(string title, string description)
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the title.
    /// </summary>
    public string Title { get; } = title;

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description { get; } = description;
    #endregion
}
#endregion
