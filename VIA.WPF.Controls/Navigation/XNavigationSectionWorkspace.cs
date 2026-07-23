// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationSectionWorkspace.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationSectionWorkspace ###
/// <summary>
/// Describes the runtime objects created for a navigation section.
/// </summary>
public sealed class XNavigationSectionWorkspace
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationSectionWorkspace"/> class.
    /// </summary>
    /// <param name="sidePanel">The custom side panel object.</param>
    /// <param name="page">The page object displayed in the main content area.</param>
    public XNavigationSectionWorkspace(object? sidePanel, object? page)
        : this(sidePanel, page, [], selectedPage: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationSectionWorkspace"/> class.
    /// </summary>
    /// <param name="sidePanel">The custom side panel object.</param>
    /// <param name="page">The page object displayed in the main content area.</param>
    /// <param name="pageEntries">The standard page navigation entries.</param>
    /// <param name="selectedPage">The selected standard page navigation entry.</param>
    public XNavigationSectionWorkspace(
        object? sidePanel,
        object? page,
        IEnumerable<XNavigationEntry> pageEntries,
        XNavigationEntry? selectedPage)
    {
        this.SidePanel = sidePanel;
        this.Page = page;
        this.PageEntries = new ObservableCollection<XNavigationEntry>(pageEntries);
        this.SelectedPage = selectedPage;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the standard page navigation entries displayed in the section side area.
    /// </summary>
    public ObservableCollection<XNavigationEntry> PageEntries { get; }

    /// <summary>
    /// Gets the selected standard page navigation entry.
    /// </summary>
    public XNavigationEntry? SelectedPage { get; }

    /// <summary>
    /// Gets the custom side panel object displayed instead of the standard section page navigation.
    /// </summary>
    public object? SidePanel { get; }

    /// <summary>
    /// Gets the page object displayed in the main content area.
    /// </summary>
    public object? Page { get; }

    /// <summary>
    /// Gets a value indicating whether the workspace uses standard page navigation.
    /// </summary>
    public bool UsesStandardPageNavigation => this.SidePanel is null && this.PageEntries.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the workspace uses a custom side panel.
    /// </summary>
    public bool UsesSidePanel => this.SidePanel is not null;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates an empty workspace.
    /// </summary>
    /// <returns>The empty workspace.</returns>
    public static XNavigationSectionWorkspace CreateEmpty()
    {
        return new XNavigationSectionWorkspace(sidePanel: null, page: null);
    }
    #endregion
}
#endregion
