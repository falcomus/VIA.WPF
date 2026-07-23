// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWelcomeDashboardViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XWelcomeDashboardViewModel ###
/// <summary>
/// Represents the welcome dashboard page.
/// </summary>
public sealed class XWelcomeDashboardViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XWelcomeDashboardViewModel"/> class.
    /// </summary>
    public XWelcomeDashboardViewModel()
    {
        this.WorkItems =
        [
            new WelcomeWorkItem("VIA-1042", "Modernize ProjectPlanner shell", "In progress", "High", "Claus"),
            new WelcomeWorkItem("VIA-1038", "Review XDataGrid row states", "Ready", "Medium", "UI Kit"),
            new WelcomeWorkItem("VIA-1027", "Ship calendar popup template", "Done", "High", "Controls"),
            new WelcomeWorkItem("VIA-1019", "Audit commandbar composition", "Review", "Medium", "Workbench"),
        ];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Welcome";

    /// <inheritdoc/>
    public override string Description => "A compact workbench dashboard showing how VIA.WPF controls compose into a production-style application.";

    /// <summary>
    /// Gets the sample work items.
    /// </summary>
    public ObservableCollection<WelcomeWorkItem> WorkItems { get; }

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XHeaderBar TitleWidth="0">
    <via:XHeaderGroup Header="View">
        <via:XButtonGroup SelectedValue="Dashboard" Size="Small">
            <via:XButtonGroupItem Content="Dashboard" Value="Dashboard" />
            <via:XButtonGroupItem Content="Planner" Value="Planner" />
        </via:XButtonGroup>
    </via:XHeaderGroup>
    <via:XHeaderGroup Header="Filter" IsSeparatorVisible="False">
        <via:XSearchBox Width="240" Placeholder="Search work items" />
    </via:XHeaderGroup>
</via:XHeaderBar>

<via:XGroup Title="Current work">
    <via:XDataGrid ItemsSource="{Binding WorkItems}" />
</via:XGroup>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public sealed class WorkbenchViewModel
{
    public ObservableCollection<WorkItem> WorkItems { get; } = [];
}
""";
    #endregion
}
#endregion

#region ### Record WelcomeWorkItem ###
/// <summary>
/// Represents a sample dashboard work item.
/// </summary>
public sealed record WelcomeWorkItem(string Id, string Title, string Status, string Priority, string Owner);
#endregion
