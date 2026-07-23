// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationSectionDefinition.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationSectionDefinition ###
/// <summary>
/// Describes one main navigation section of an application shell.
/// </summary>
public sealed class XNavigationSectionDefinition
{
    #region ### Fields ###
    private Func<object?>? defaultPageFactory;
    private Func<object?>? sidePanelFactory;
    private Func<XNavigationSectionWorkspace>? workspaceFactory;
    private object? cachedDefaultPage;
    private object? cachedSidePanel;
    private XNavigationSectionWorkspace? cachedWorkspace;
    private bool hasCachedDefaultPage;
    private bool hasCachedSidePanel;
    private bool hasCachedWorkspace;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationSectionDefinition"/> class.
    /// </summary>
    /// <param name="value">The value represented by this section.</param>
    /// <param name="title">The displayed section title.</param>
    /// <param name="description">The displayed section description.</param>
    public XNavigationSectionDefinition(object? value, string title, string description)
    {
        this.Value = value;
        this.Title = title;
        this.Description = description;
        this.Pages = [];
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the value represented by this section.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the displayed section title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the displayed section description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the selectable pages of the section.
    /// </summary>
    public ObservableCollection<XNavigationPageDefinition> Pages { get; }

    /// <summary>
    /// Gets a value indicating whether this section provides its own side panel.
    /// </summary>
    public bool HasSidePanel => this.sidePanelFactory is not null || this.workspaceFactory is not null;

    /// <summary>
    /// Gets a value indicating whether this section creates a complete custom workspace.
    /// </summary>
    public bool HasWorkspace => this.workspaceFactory is not null;

    /// <summary>
    /// Gets a value indicating whether this section provides its own side content.
    /// </summary>
    public bool HasCustomSideContent => this.HasSidePanel;

    /// <summary>
    /// Gets the cache mode used for the default page.
    /// </summary>
    public XNavigationCacheMode DefaultPageCacheMode { get; private set; }

    /// <summary>
    /// Gets the cache mode used for a custom side panel.
    /// </summary>
    public XNavigationCacheMode SidePanelCacheMode { get; private set; }

    /// <summary>
    /// Gets the cache mode used for a custom section workspace.
    /// </summary>
    public XNavigationCacheMode WorkspaceCacheMode { get; private set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Adds a selectable page to this navigation section.
    /// </summary>
    /// <param name="value">The route or value of the page.</param>
    /// <param name="title">The displayed page title.</param>
    /// <param name="description">The displayed page description.</param>
    /// <param name="pageFactory">The factory used to create the page object.</param>
    /// <param name="cacheMode">The cache mode used for the page.</param>
    /// <returns>This section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition AddPage(
        object? value,
        string title,
        string description,
        Func<object?> pageFactory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        this.Pages.Add(new XNavigationPageDefinition(value, title, description, pageFactory, cacheMode));
        return this;
    }

    /// <summary>
    /// Sets the default page used when the section does not select a specific navigation page.
    /// </summary>
    /// <param name="pageFactory">The default page factory.</param>
    /// <param name="cacheMode">The cache mode used for the default page.</param>
    /// <returns>This section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition WithDefaultPage(
        Func<object?> pageFactory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        this.defaultPageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
        this.DefaultPageCacheMode = cacheMode;
        this.ClearCachedDefaultPage();
        return this;
    }

    /// <summary>
    /// Sets a custom side panel used by this section instead of the standard page navigation list.
    /// </summary>
    /// <param name="factory">The side panel factory.</param>
    /// <param name="cacheMode">The cache mode used for the side panel.</param>
    /// <returns>This section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition WithSidePanel(
        Func<object?> factory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        this.sidePanelFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.SidePanelCacheMode = cacheMode;
        this.ClearCachedSidePanel();
        return this;
    }

    /// <summary>
    /// Sets a custom workspace used by this section.
    /// </summary>
    /// <param name="factory">The workspace factory.</param>
    /// <param name="cacheMode">The cache mode used for the workspace.</param>
    /// <returns>This section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition WithWorkspace(
        Func<XNavigationSectionWorkspace> factory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        this.workspaceFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.WorkspaceCacheMode = cacheMode;
        this.ClearCachedWorkspace();
        return this;
    }

    /// <summary>
    /// Sets custom side content used by this section instead of the standard navigation panel.
    /// </summary>
    /// <param name="factory">The side content factory.</param>
    /// <param name="cacheMode">The cache mode used for the side content.</param>
    /// <returns>This section definition for fluent configuration.</returns>
    public XNavigationSectionDefinition WithSideContent(
        Func<object?> factory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        return this.WithSidePanel(factory, cacheMode);
    }

    /// <summary>
    /// Sets the cache mode used for the custom workspace.
    /// </summary>
    /// <param name="cacheMode">The new cache mode.</param>
    /// <param name="currentWorkspace">The currently displayed workspace that should be cached immediately when caching is enabled.</param>
    public void SetWorkspaceCacheMode(XNavigationCacheMode cacheMode, XNavigationSectionWorkspace? currentWorkspace = null)
    {
        if (this.WorkspaceCacheMode == cacheMode)
        {
            if (cacheMode == XNavigationCacheMode.PerRoute && currentWorkspace is not null && !this.hasCachedWorkspace)
            {
                this.cachedWorkspace = currentWorkspace;
                this.hasCachedWorkspace = true;
            }

            return;
        }

        this.WorkspaceCacheMode = cacheMode;

        if (cacheMode == XNavigationCacheMode.PerRoute)
        {
            if (currentWorkspace is not null)
            {
                this.cachedWorkspace = currentWorkspace;
                this.hasCachedWorkspace = true;
            }

            return;
        }

        this.ClearCachedWorkspace();
    }

    /// <summary>
    /// Creates the custom workspace for this section.
    /// </summary>
    /// <returns>The workspace object, or <see langword="null"/> when no workspace is configured.</returns>
    public XNavigationSectionWorkspace? CreateWorkspace()
    {
        if (this.workspaceFactory is null)
        {
            return null;
        }

        if (this.WorkspaceCacheMode != XNavigationCacheMode.PerRoute)
        {
            return this.workspaceFactory.Invoke();
        }

        if (!this.hasCachedWorkspace)
        {
            this.cachedWorkspace = this.workspaceFactory.Invoke();
            this.hasCachedWorkspace = true;
        }

        return this.cachedWorkspace;
    }

    /// <summary>
    /// Creates the custom side panel for this section.
    /// </summary>
    /// <returns>The side panel object, or <see langword="null"/> when no custom side panel is configured.</returns>
    public object? CreateSidePanel()
    {
        if (this.sidePanelFactory is null)
        {
            return null;
        }

        if (this.SidePanelCacheMode != XNavigationCacheMode.PerRoute)
        {
            return this.sidePanelFactory.Invoke();
        }

        if (!this.hasCachedSidePanel)
        {
            this.cachedSidePanel = this.sidePanelFactory.Invoke();
            this.hasCachedSidePanel = true;
        }

        return this.cachedSidePanel;
    }

    /// <summary>
    /// Creates the custom side content for this section.
    /// </summary>
    /// <returns>The side content object, or <see langword="null"/> when no custom side content is configured.</returns>
    public object? CreateSideContent()
    {
        return this.CreateSidePanel();
    }

    /// <summary>
    /// Creates the default page for this section.
    /// </summary>
    /// <returns>The page object, or <see langword="null"/> when no default page is configured.</returns>
    public object? CreateDefaultPage()
    {
        if (this.defaultPageFactory is null)
        {
            return null;
        }

        if (this.DefaultPageCacheMode != XNavigationCacheMode.PerRoute)
        {
            return this.defaultPageFactory.Invoke();
        }

        if (!this.hasCachedDefaultPage)
        {
            this.cachedDefaultPage = this.defaultPageFactory.Invoke();
            this.hasCachedDefaultPage = true;
        }

        return this.cachedDefaultPage;
    }
    #endregion

    #region ### Private Methods ###
    private void ClearCachedDefaultPage()
    {
        this.cachedDefaultPage = null;
        this.hasCachedDefaultPage = false;
    }

    private void ClearCachedSidePanel()
    {
        this.cachedSidePanel = null;
        this.hasCachedSidePanel = false;
    }

    private void ClearCachedWorkspace()
    {
        this.cachedWorkspace = null;
        this.hasCachedWorkspace = false;
    }
    #endregion
}
#endregion
