// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationManager.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationManager ###
/// <summary>
/// Provides a reusable navigation definition and page factory registry.
/// </summary>
public sealed class XNavigationManager
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationManager"/> class.
    /// </summary>
    public XNavigationManager()
    {
        this.Sections = [];
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the registered main navigation sections.
    /// </summary>
    public ObservableCollection<XNavigationSectionDefinition> Sections { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Adds a main navigation section to the manager.
    /// </summary>
    /// <param name="value">The value represented by the section.</param>
    /// <param name="title">The displayed section title.</param>
    /// <param name="description">The displayed section description.</param>
    /// <returns>The created section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition AddSection(object? value, string title, string description)
    {
        XNavigationSectionDefinition definition = new(value, title, description);
        this.Sections.Add(definition);
        return definition;
    }

    /// <summary>
    /// Gets the first registered navigation section.
    /// </summary>
    /// <returns>The first navigation section, or <see langword="null"/> when no section exists.</returns>
    public XNavigationSectionDefinition? GetFirstSection()
    {
        return this.Sections.FirstOrDefault();
    }

    /// <summary>
    /// Finds a registered section by value.
    /// </summary>
    /// <param name="value">The requested section value.</param>
    /// <returns>The section definition, or <see langword="null"/> when the value is unknown.</returns>
    public XNavigationSectionDefinition? FindSection(object? value)
    {
        return this.Sections.FirstOrDefault(section => Equals(section.Value, value));
    }

    /// <summary>
    /// Finds a registered page by route or value.
    /// </summary>
    /// <param name="value">The requested page value.</param>
    /// <returns>The page definition, or <see langword="null"/> when the value is unknown.</returns>
    public XNavigationPageDefinition? FindPage(object? value)
    {
        return this.Sections
            .SelectMany(static section => section.Pages)
            .FirstOrDefault(page => Equals(page.Value, value));
    }

    /// <summary>
    /// Finds a registered page by route or value inside the specified section.
    /// </summary>
    /// <param name="section">The section to search in.</param>
    /// <param name="value">The requested page value.</param>
    /// <returns>The page definition, or <see langword="null"/> when the value is unknown.</returns>
    public XNavigationPageDefinition? FindPage(XNavigationSectionDefinition? section, object? value)
    {
        return section?.Pages.FirstOrDefault(page => Equals(page.Value, value));
    }

    /// <summary>
    /// Creates a page object for the requested route or value.
    /// </summary>
    /// <param name="value">The requested page value.</param>
    /// <returns>The created page object, or <see langword="null"/> when the value is unknown.</returns>
    public object? CreatePage(object? value)
    {
        return this.FindPage(value)?.CreatePage();
    }

    /// <summary>
    /// Creates a page object for the specified navigation entry.
    /// </summary>
    /// <param name="entry">The navigation entry.</param>
    /// <returns>The created page object, or <see langword="null"/> when no page can be resolved.</returns>
    public object? CreatePage(XNavigationEntry? entry)
    {
        return entry is null
            ? null
            : this.CreatePage(entry.Value);
    }

    /// <summary>
    /// Creates the runtime workspace for the specified section.
    /// </summary>
    /// <param name="section">The selected section.</param>
    /// <param name="selectedPageValue">The preferred page value that should be selected in standard page navigation.</param>
    /// <returns>The created section workspace.</returns>
    public XNavigationSectionWorkspace CreateSectionWorkspace(XNavigationSectionDefinition? section, object? selectedPageValue = null)
    {
        if (section is null)
        {
            return XNavigationSectionWorkspace.CreateEmpty();
        }

        XNavigationSectionWorkspace? customWorkspace = section.CreateWorkspace();
        if (customWorkspace is not null)
        {
            return customWorkspace;
        }

        ObservableCollection<XNavigationEntry> pageEntries = new(
            section.Pages.Select(static page => page.ToEntry()));

        object? sidePanel = section.CreateSidePanel();
        if (sidePanel is not null)
        {
            return new XNavigationSectionWorkspace(
                sidePanel,
                section.CreateDefaultPage(),
                pageEntries,
                selectedPage: null);
        }

        XNavigationEntry? selectedPage = pageEntries.FirstOrDefault(page => Equals(page.Value, selectedPageValue))
            ?? pageEntries.FirstOrDefault();
        object? page = selectedPage is not null
            ? this.CreatePage(selectedPage)
            : section.CreateDefaultPage();

        return new XNavigationSectionWorkspace(
            sidePanel: null,
            page,
            pageEntries,
            selectedPage);
    }
    #endregion
}
#endregion
