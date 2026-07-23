// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewContainerDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XViewContainerDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XViewContainer showcase page.
/// </summary>
public sealed partial class XViewContainerDemoViewModel : DemoPageViewModel, IXCrudPageContext
{
    #region ### Private Fields ###
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGridMode))]
    [NotifyPropertyChangedFor(nameof(IsTreeMode))]
    private XContentViewMode _viewMode = XContentViewMode.Grid;

    [ObservableProperty]
    private XViewContainerDemoItem? _selectedItem;

    [ObservableProperty]
    private XViewContainerDemoTreeItem? _selectedTreeItem;

    [ObservableProperty]
    private string _lastAction = "The detail dialog is open. Navigation controls are locked while it is modal.";
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XViewContainerDemoViewModel"/> class.
    /// </summary>
    public XViewContainerDemoViewModel()
    {
        this.SelectedItem = this.Items[0];
        this.SelectedTreeItem = this.TreeItems[0].Children[0];
        this.OpenDetail();
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XViewContainer";

    /// <inheritdoc/>
    public override string Description => "Demonstrates list/tree hosting with a centered modal detail dialog, automatic CRUD context integration and navigation locking.";

    /// <summary>
    /// Gets the CRUD context consumed automatically by the XViewContainer.
    /// </summary>
    public XCrudContext CrudContext { get; } = new();

    /// <inheritdoc />
    ICommand? IXCrudPageContext.SaveDetailCommand => this.SaveDetailCommand;

    /// <summary>
    /// Gets a value indicating whether grid mode is active.
    /// </summary>
    public bool IsGridMode => this.ViewMode == XContentViewMode.Grid;

    /// <summary>
    /// Gets a value indicating whether tree mode is active.
    /// </summary>
    public bool IsTreeMode => this.ViewMode == XContentViewMode.Tree;

    /// <summary>
    /// Gets the list items.
    /// </summary>
    public ObservableCollection<XViewContainerDemoItem> Items { get; } =
    [
        new("Project Alpha", "Customer portal refresh", "In progress"),
        new("Project Beta", "Inventory import tooling", "Review"),
        new("Project Gamma", "Reporting workspace", "Draft"),
        new("Project Delta", "Mobile scanner flow", "Ready"),
    ];

    /// <summary>
    /// Gets the tree items.
    /// </summary>
    public ObservableCollection<XViewContainerDemoTreeItem> TreeItems { get; } =
    [
        new("Customers", true, [new("North"), new("South"), new("West")]),
        new("Orders", true, [new("Open"), new("Completed"), new("Archived")]),
        new("Projects", true, [new("Planned"), new("Active"), new("Closed")]),
    ];

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XViewContainer
    ViewMode="{Binding ViewMode, Mode=TwoWay}"
    DetailWidth="540"
    PrimaryDetailText="Save"
    CancelDetailText="Cancel">
    <via:XViewContainer.ListHost>
        <via:XListBox ItemsSource="{Binding Items}" />
    </via:XViewContainer.ListHost>
    <via:XViewContainer.TreeHost>
        <via:XTreeView ItemsSource="{Binding TreeItems}" ChildrenMemberPath="Children" />
    </via:XViewContainer.TreeHost>
</via:XViewContainer>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public sealed partial class ProjectsViewModel : IXCrudPageContext
{
    public XCrudContext CrudContext { get; } = new();

    ICommand? IXCrudPageContext.SaveDetailCommand => this.SaveDetailCommand;

    [RelayCommand]
    private void Edit(ProjectItem item)
    {
        this.CrudContext.Open(XCrudMode.Edit, new ProjectEditorViewModel(item), "Edit project");
    }

    [RelayCommand]
    private void SaveDetail()
    {
        // Persist the active editor.
        this.CrudContext.Close();
    }
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Opens the detail dialog.
    /// </summary>
    [RelayCommand]
    private void OpenDetail()
    {
        XViewContainerDemoItem item = this.SelectedItem ?? this.Items[0];

        this.CrudContext.Open(
            XCrudMode.Edit,
            new XViewContainerDemoEditor(item.Title, item.Summary, "VIA.WPF", item.Status),
            "Project detail");

        this.LastAction = "Dialog opened. The surrounding navigation is locked automatically.";
    }

    /// <summary>
    /// Saves the detail dialog and closes it.
    /// </summary>
    [RelayCommand]
    private void SaveDetail()
    {
        this.LastAction = $"Saved {this.SelectedItem?.Title ?? "project detail"}.";
        this.CrudContext.Close();
    }

    /// <summary>
    /// Switches to grid mode.
    /// </summary>
    [RelayCommand]
    private void SetGridMode()
    {
        this.ViewMode = XContentViewMode.Grid;
    }

    /// <summary>
    /// Switches to tree mode.
    /// </summary>
    [RelayCommand]
    private void SetTreeMode()
    {
        this.ViewMode = XContentViewMode.Tree;
    }
    #endregion
}
#endregion

#region ### Class XViewContainerDemoEditor ###
/// <summary>
/// Represents the editor payload shown in the XViewContainer detail dialog.
/// </summary>
/// <param name="name">The project name.</param>
/// <param name="summary">The project summary.</param>
/// <param name="owner">The project owner.</param>
/// <param name="status">The project status.</param>
public sealed class XViewContainerDemoEditor(string name, string summary, string owner, string status)
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the project summary.
    /// </summary>
    public string Summary { get; set; } = summary;

    /// <summary>
    /// Gets or sets the project owner.
    /// </summary>
    public string Owner { get; set; } = owner;

    /// <summary>
    /// Gets or sets the project status.
    /// </summary>
    public string Status { get; set; } = status;
    #endregion
}
#endregion

#region ### Class XViewContainerDemoItem ###
/// <summary>
/// Represents one list item in the XViewContainer demo.
/// </summary>
/// <param name="title">The title.</param>
/// <param name="summary">The summary.</param>
/// <param name="status">The status.</param>
public sealed class XViewContainerDemoItem(string title, string summary, string status)
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the title.
    /// </summary>
    public string Title { get; } = title;

    /// <summary>
    /// Gets the summary.
    /// </summary>
    public string Summary { get; } = summary;

    /// <summary>
    /// Gets the status.
    /// </summary>
    public string Status { get; } = status;

    /// <inheritdoc/>
    public override string ToString() => this.Title;
    #endregion
}
#endregion

#region ### Class XViewContainerDemoTreeItem ###
/// <summary>
/// Represents one tree item in the XViewContainer demo.
/// </summary>
public sealed class XViewContainerDemoTreeItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XViewContainerDemoTreeItem"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="isExpanded">A value indicating whether the item is expanded.</param>
    /// <param name="children">The child items.</param>
    public XViewContainerDemoTreeItem(string title, bool isExpanded = false, IEnumerable<XViewContainerDemoTreeItem>? children = null)
    {
        this.Title = title;
        this.IsExpanded = isExpanded;
        this.Children = children is null ? [] : new ObservableCollection<XViewContainerDemoTreeItem>(children);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is expanded.
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Gets the child items.
    /// </summary>
    public ObservableCollection<XViewContainerDemoTreeItem> Children { get; }
    #endregion
}
#endregion
