// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMasterDetailSplitViewDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XMasterDetailSplitViewDemoViewModel ###

/// <summary>
/// Represents the demo view model for the XMasterDetailSplitView showcase page.
/// </summary>
public sealed partial class XMasterDetailSplitViewDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###

    [ObservableProperty]
    private DemoArticle? selectedArticle;

    [ObservableProperty]
    private bool isDetailPaneOpen;

    #endregion

    #region ### Constructors ###

    /// <summary>
    /// Initializes a new instance of the <see cref="XMasterDetailSplitViewDemoViewModel"/> class.
    /// </summary>
    public XMasterDetailSplitViewDemoViewModel()
    {
        this.Articles =
        [
            new DemoArticle("AUS00170", "Signalfahne", "Active", 18, "103015", "Safety", "0.2 kg", "Signal flag for warehouse and yard coordination."),
            new DemoArticle("AUS00210", "Warnweste", "Active", 124, "103210", "Safety", "0.18 kg", "High visibility vest for visitors and warehouse staff."),
            new DemoArticle("WER00420", "Akkuschrauber", "Service due", 9, "204420", "Tools", "1.4 kg", "Cordless screwdriver for maintenance teams."),
            new DemoArticle("LAG00910", "Eurobox 600x400", "Active", 312, "304910", "Storage", "1.1 kg", "Reusable storage box for internal logistics."),
        ];

        this.Categories =
        [
            new DemoRelatedItem("Safety", "Main category", "Visible in warehouse reports"),
            new DemoRelatedItem("Visitor equipment", "Sub category", "Used by visitor management"),
        ];

        this.Companies =
        [
            new DemoRelatedItem("Workwear Direkt", "Supplier", "8.50 €"),
            new DemoRelatedItem("Hanse Prüfdienst", "Inspection", "Visual check"),
        ];

        this.SelectedArticle = this.Articles.FirstOrDefault();
        this.IsDetailPaneOpen = true;
    }

    #endregion

    #region ### Public Properties ###

    /// <inheritdoc/>
    public override string Title => "XMasterDetailSplitView";

    /// <inheritdoc/>
    public override string Description => "Demonstrates a reusable master-detail layout with collapsible right detail pane and remaining-width detail area.";

    /// <summary>
    /// Gets the demo articles.
    /// </summary>
    public ObservableCollection<DemoArticle> Articles { get; }

    /// <summary>
    /// Gets the related category rows.
    /// </summary>
    public ObservableCollection<DemoRelatedItem> Categories { get; }

    /// <summary>
    /// Gets the related company rows.
    /// </summary>
    public ObservableCollection<DemoRelatedItem> Companies { get; }

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XMasterDetailSplitView
    MasterTitle="Articles"
    DetailTitle="Article details"
    NewCommand="{Binding NewArticleCommand}"
    DetailCommand="{Binding OpenDetailsCommand}"
    EditCommand="{Binding EditArticleCommand}"
    IsDetailPaneOpen="{Binding IsDetailPaneOpen, Mode=TwoWay}">

    <via:XMasterDetailSplitView.FirstContent>
        <via:XDataGrid ItemsSource="{Binding Articles}" />
    </via:XMasterDetailSplitView.FirstContent>

    <via:XMasterDetailSplitView.SecondContent>
        <via:XTabControl>
            <via:XTabItem Header="Categories">
                <via:XGroup ContentPadding="0" Title="Category records">
                    <via:XGroup.Actions>
                        <via:XButton
                            Command="{Binding NewCategoryCommand}"
                            Content="New"
                            Size="Small" />
                    </via:XGroup.Actions>
                    <via:XDataGrid ItemsSource="{Binding Categories}" />
                </via:XGroup>
            </via:XTabItem>
        </via:XTabControl>
    </via:XMasterDetailSplitView.SecondContent>
</via:XMasterDetailSplitView>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
[ObservableProperty]
private bool isDetailPaneOpen;

[RelayCommand]
private void OpenDetails()
{
    this.IsDetailPaneOpen = this.SelectedArticle is not null;
}
""";

    #endregion

    #region ### Partial Methods ###

    /// <summary>
    /// Opens the detail pane when the selected article changes while the pane is already open.
    /// </summary>
    /// <param name="value">The selected article.</param>
    partial void OnSelectedArticleChanged(DemoArticle? value)
    {
        this.OpenDetailsCommand.NotifyCanExecuteChanged();
        this.EditArticleCommand.NotifyCanExecuteChanged();

        if (value is not null && this.IsDetailPaneOpen)
        {
            this.IsDetailPaneOpen = true;
        }
    }

    #endregion

    #region ### Commands ###

    /// <summary>
    /// Opens the detail pane for the selected article.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanOpenDetails))]
    private void OpenDetails()
    {
        this.IsDetailPaneOpen = this.SelectedArticle is not null;
    }

    /// <summary>
    /// Handles the edit command for the selected article.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanOpenDetails))]
    private void EditArticle()
    {
        this.IsDetailPaneOpen = this.SelectedArticle is not null;
    }

    /// <summary>
    /// Handles the new command for the demo.
    /// </summary>
    [RelayCommand]
    private void NewArticle()
    {
        DemoArticle article = new("NEW00001", "New article", "Draft", 0, "", "General", "0 kg", "New article draft.");
        this.Articles.Insert(0, article);
        this.SelectedArticle = article;
        this.IsDetailPaneOpen = true;
    }

    /// <summary>
    /// Handles the new category command for the demo.
    /// </summary>
    [RelayCommand]
    private void NewCategory()
    {
        this.Categories.Add(new DemoRelatedItem("New category", "Draft", "New category assignment"));
    }

    /// <summary>
    /// Handles the new company command for the demo.
    /// </summary>
    [RelayCommand]
    private void NewCompany()
    {
        this.Companies.Add(new DemoRelatedItem("New company", "Draft", "New company assignment"));
    }

    /// <summary>
    /// Determines whether a detail command can be executed.
    /// </summary>
    /// <returns><see langword="true"/> when an article is selected; otherwise <see langword="false"/>.</returns>
    private bool CanOpenDetails()
    {
        return this.SelectedArticle is not null;
    }

    #endregion
}

#endregion

#region ### Record DemoArticle ###

/// <summary>
/// Represents a demo article row.
/// </summary>
/// <param name="Number">The article number.</param>
/// <param name="Name">The article name.</param>
/// <param name="Status">The article status.</param>
/// <param name="Stock">The stock amount.</param>
/// <param name="Barcode">The barcode.</param>
/// <param name="Group">The article group.</param>
/// <param name="Weight">The article weight.</param>
/// <param name="Description">The article description.</param>
public sealed record DemoArticle(
    string Number,
    string Name,
    string Status,
    int Stock,
    string Barcode,
    string Group,
    string Weight,
    string Description);

#endregion

#region ### Record DemoRelatedItem ###

/// <summary>
/// Represents a related demo row.
/// </summary>
/// <param name="Name">The item name.</param>
/// <param name="Role">The item role.</param>
/// <param name="Info">Additional information.</param>
public sealed record DemoRelatedItem(string Name, string Role, string Info);

#endregion
