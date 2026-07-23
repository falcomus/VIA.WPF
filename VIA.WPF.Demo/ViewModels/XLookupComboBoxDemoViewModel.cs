// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupComboBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VIA.WPF.Controls;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XLookupComboBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XLookupComboBox showcase page.
/// </summary>
public sealed partial class XLookupComboBoxDemoViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupComboBoxDemoViewModel"/> class.
    /// </summary>
    public XLookupComboBoxDemoViewModel()
    {
        this.SelectedProjectId = 3;
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XLookupComboBox";

    /// <inheritdoc/>
    public override string Description => "Demonstrates searchable lookup selection with header, placeholder, icon, empty item, sorting, selected item/value binding and insert requests.";

    /// <summary>
    /// Gets the sample projects.
    /// </summary>
    public ObservableCollection<LookupProjectItem> Projects { get; } =
    [
        new(1, "VIA.WPF Controls", "Component library"),
        new(2, "Theme Designer", "Visual tooling"),
        new(3, "Demo Shell", "Showcase application"),
        new(4, "Documentation", "Public docs"),
        new(5, "Release Pipeline", "Build and publish"),
    ];

    /// <summary>
    /// Gets or sets the selected project.
    /// </summary>
    [ObservableProperty]
    private LookupProjectItem? _selectedProject;

    /// <summary>
    /// Gets or sets the selected project id.
    /// </summary>
    [ObservableProperty]
    private int? _selectedProjectId;

    /// <summary>
    /// Gets or sets the insert request status message.
    /// </summary>
    [ObservableProperty]
    private string _insertStatusMessage = "Type a new project name and press Enter to add it.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XLookupComboBox
    Width="360"
    DisplayMemberPath="Name"
    EmptyOptionText="No project"
    Header="Project"
    Icon="{via:MaterialIcon Kind=FolderOutline}"
    IncludeEmptyOption="True"
    IsEditable="True"
    IsSearchEnabled="True"
    ItemsSource="{Binding Projects}"
    Placeholder="Search project"
    SelectedItem="{Binding SelectedProject, Mode=TwoWay}"
    SelectedValue="{Binding SelectedProjectId, Mode=TwoWay}"
    SelectedValuePath="Id"
    ShowResetButton="True"
    Variant="Outline" />

<via:XLookupComboBox
    Width="360"
    AllowInsertRequest="True"
    DisplayMemberPath="Name"
    Header="Project with insert request"
    Icon="{via:BootstrapIcon Kind=PlusCircleFill}"
    InsertRequestTrigger="EnterOrLostFocus"
    IsEditable="True"
    ItemsSource="{Binding Projects}"
    Placeholder="Type a new project"
    RequestInsertCommand="{Binding RequestProjectInsertCommand}"
    SelectedValue="{Binding SelectedProjectId, Mode=TwoWay}"
    SelectedValuePath="Id" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
[RelayCommand]
private void RequestProjectInsert(XLookupInsertRequest? request)
{
    if (request is null)
    {
        return;
    }

    ProjectItem item = new(nextId, request.Text);
    Projects.Add(item);
    SelectedProject = item;
    SelectedProjectId = item.Id;
}
""";
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Inserts a demo project when the lookup requests a new item.
    /// </summary>
    /// <param name="request">The lookup insert request.</param>
    [RelayCommand]
    private void RequestProjectInsert(XLookupInsertRequest? request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Text))
        {
            return;
        }

        LookupProjectItem? existingProject = this.Projects.FirstOrDefault(project =>
            string.Equals(project.Name, request.Text, StringComparison.CurrentCultureIgnoreCase));

        if (existingProject is not null)
        {
            this.SelectedProject = existingProject;
            this.SelectedProjectId = existingProject.Id;
            this.InsertStatusMessage = $"Selected existing project: {existingProject.Name}";
            return;
        }

        int nextId = this.Projects.Count == 0
            ? 1
            : this.Projects.Max(project => project.Id) + 1;

        LookupProjectItem newProject = new(nextId, request.Text, "Created from insert request");
        this.Projects.Add(newProject);
        this.SelectedProject = newProject;
        this.SelectedProjectId = newProject.Id;
        this.InsertStatusMessage = $"Inserted and selected: {newProject.Name}";
    }
    #endregion
}
#endregion

#region ### Class LookupProjectItem ###
/// <summary>
/// Represents a sample lookup project.
/// </summary>
public sealed class LookupProjectItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="LookupProjectItem"/> class.
    /// </summary>
    /// <param name="id">The project id.</param>
    /// <param name="name">The project name.</param>
    /// <param name="description">The project description.</param>
    public LookupProjectItem(int id, string name, string description)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the project id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the project name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the project description.
    /// </summary>
    public string Description { get; }
    #endregion
}
#endregion
