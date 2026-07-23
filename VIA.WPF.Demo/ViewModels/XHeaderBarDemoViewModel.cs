// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHeaderBarDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XHeaderBarDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XHeaderBar showcase page.
/// </summary>
public sealed partial class XHeaderBarDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    [ObservableProperty]
    private string _lastToolbarAction = "No command executed.";
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XHeaderBarDemoViewModel"/> class.
    /// </summary>
    public XHeaderBarDemoViewModel()
    {
        this.PrimaryToolbar = XToolbarContext.CreateCrudWithViewMode();
        this.PrimaryToolbar.ShowViewButton = true;
        this.PrimaryToolbar.ShowRememberViewToggle = true;
        this.PrimaryToolbar.NewCommand = new RelayCommand(() => this.LastToolbarAction = "New command executed.");
        this.PrimaryToolbar.ViewCommand = new RelayCommand(() => this.LastToolbarAction = "View command executed.");
        this.PrimaryToolbar.EditCommand = new RelayCommand(() => this.LastToolbarAction = "Edit command executed.");
        this.PrimaryToolbar.DeleteCommand = new RelayCommand(() => this.LastToolbarAction = "Delete command executed.");

        this.CompactToolbar = XToolbarContext.CreateCrudWithViewMode();
        this.CompactToolbar.ShowViewButton = true;
        this.CompactToolbar.ShowRememberViewToggle = true;
        this.CompactToolbar.NewCommand = new RelayCommand(() => this.LastToolbarAction = "Compact new command executed.");
        this.CompactToolbar.ViewCommand = new RelayCommand(() => this.LastToolbarAction = "Compact view command executed.");
        this.CompactToolbar.EditCommand = new RelayCommand(() => this.LastToolbarAction = "Compact edit command executed.");
        this.CompactToolbar.DeleteCommand = new RelayCommand(() => this.LastToolbarAction = "Compact delete command executed.");

        this.SearchContext = new DemoSearchContext("project");
        this.CompactSearchContext = new DemoSearchContext("item");
        this.SearchOnlyContext = new DemoSearchContext("via");
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XHeaderBar";

    /// <inheritdoc/>
    public override string Description => "Demonstrates the dense workbench command bar with optional title, optional overflow menu and labelled command groups.";

    /// <summary>
    /// Gets the primary toolbar context.
    /// </summary>
    public XToolbarContext PrimaryToolbar { get; }

    /// <summary>
    /// Gets the compact toolbar context.
    /// </summary>
    public XToolbarContext CompactToolbar { get; }

    /// <summary>
    /// Gets the primary search context.
    /// </summary>
    public IXSearchContext SearchContext { get; }

    /// <summary>
    /// Gets the compact search context.
    /// </summary>
    public IXSearchContext CompactSearchContext { get; }

    /// <summary>
    /// Gets the search-only context.
    /// </summary>
    public IXSearchContext SearchOnlyContext { get; }

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XHeaderBar TitleWidth="0">
    <via:XHeaderBar.MoreMenu>
        <ContextMenu>
            <MenuItem Header="Runtime diagnostics" />
            <MenuItem Header="Reset workspace" />
        </ContextMenu>
    </via:XHeaderBar.MoreMenu>

    <via:XHeaderBar.Actions>
        <via:XStackPanel Orientation="Horizontal" Spacing="6">
            <via:XButton Content="CPU 6.8 ms" Size="Small" />
            <via:XButton Content="148 FPS" Size="Small" />
        </via:XStackPanel>
    </via:XHeaderBar.Actions>

    <via:XStackPanel Orientation="Horizontal">
        <via:XHeaderGroup Header="View">
            <via:XButtonGroup SelectedValue="Navigator" Size="Small">
                <via:XButtonGroupItem Content="Navigator" Value="Navigator" />
                <via:XButtonGroupItem Content="Inspector" Value="Inspector" />
            </via:XButtonGroup>
        </via:XHeaderGroup>
        <via:XHeaderGroup Header="Edges" IsSeparatorVisible="False">
            <via:XComboBox Width="118" SelectedIndex="0">
                <via:XComboBoxItem Content="Spline" />
                <via:XComboBoxItem Content="Straight" />
            </via:XComboBox>
        </via:XHeaderGroup>
    </via:XStackPanel>
</via:XHeaderBar>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XHeaderBar header = new()
{
    TitleWidth = new GridLength(0),
    Content = CreateWorkbenchGroups(),
    Actions = CreateHeaderActions(),
    MoreMenu = CreateMoreMenu(),
};
""";
    #endregion

    #region ### Private Classes ###
    /// <summary>
    /// Provides a minimal search context for the toolbar preview.
    /// </summary>
    private sealed class DemoSearchContext : ObservableObject, IXSearchContext
    {
        #region ### Private Fields ###
        private string searchTerm;
        #endregion

        #region ### Constructors ###
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoSearchContext"/> class.
        /// </summary>
        /// <param name="initialSearchTerm">The initial search text.</param>
        public DemoSearchContext(string initialSearchTerm)
        {
            this.searchTerm = initialSearchTerm;
            this.ResetSearchCommand = new RelayCommand(() => this.SearchTerm = string.Empty);
        }
        #endregion

        #region ### Public Properties ###
        /// <inheritdoc/>
        public string SearchTerm
        {
            get => this.searchTerm;
            set => this.SetProperty(ref this.searchTerm, value);
        }

        /// <inheritdoc/>
        public ICommand ResetSearchCommand { get; }
        #endregion
    }
    #endregion
}
#endregion
