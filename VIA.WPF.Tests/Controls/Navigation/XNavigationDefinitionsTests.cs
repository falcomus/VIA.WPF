// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationDefinitionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class XNavigationDefinitionsTests ###
/// <summary>
/// Provides tests for navigation definitions, workspaces and the navigation manager.
/// </summary>
public sealed class XNavigationDefinitionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that page definitions create new instances when no route cache is enabled.
    /// </summary>
    [Fact]
    public void PageDefinition_ShouldCreateNewPageWhenCacheModeIsNone()
    {
        XNavigationPageDefinition definition = new("route", "Title", "Description", static () => new object());

        object? first = definition.CreatePage();
        object? second = definition.CreatePage();

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotSame(first, second);
        Assert.Equal(XNavigationCacheMode.None, definition.CacheMode);
    }

    /// <summary>
    /// Ensures that page definitions reuse the page when per-route caching is enabled.
    /// </summary>
    [Fact]
    public void PageDefinition_ShouldReusePageWhenCacheModeIsPerRoute()
    {
        XNavigationPageDefinition definition = new(
            "route",
            "Title",
            "Description",
            static () => new object(),
            XNavigationCacheMode.PerRoute);

        object? first = definition.CreatePage();
        object? second = definition.CreatePage();

        Assert.NotNull(first);
        Assert.Same(first, second);
    }

    /// <summary>
    /// Ensures that changing a page definition to per-route caching can seed the current page.
    /// </summary>
    [Fact]
    public void PageDefinition_SetCacheMode_ShouldCacheCurrentPageWhenProvided()
    {
        object currentPage = new();
        XNavigationPageDefinition definition = new("route", "Title", "Description", static () => new object());

        definition.SetCacheMode(XNavigationCacheMode.PerRoute, currentPage);

        Assert.Equal(XNavigationCacheMode.PerRoute, definition.CacheMode);
        Assert.Same(currentPage, definition.CreatePage());
    }

    /// <summary>
    /// Ensures that cached pages can be cleared explicitly.
    /// </summary>
    [Fact]
    public void PageDefinition_ClearCachedPage_ShouldForceNewCachedInstance()
    {
        XNavigationPageDefinition definition = new(
            "route",
            "Title",
            "Description",
            static () => new object(),
            XNavigationCacheMode.PerRoute);
        object? first = definition.CreatePage();

        definition.ClearCachedPage();
        object? second = definition.CreatePage();

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotSame(first, second);
    }

    /// <summary>
    /// Ensures that a page definition can create a matching navigation entry.
    /// </summary>
    [Fact]
    public void PageDefinition_ToEntry_ShouldCopyDisplayValues()
    {
        object icon = new();
        XNavigationPageDefinition definition = new("route", "Title", "Description", static () => new object())
        {
            Icon = icon
        };

        XNavigationEntry entry = definition.ToEntry();

        Assert.Equal("Title", entry.Title);
        Assert.Equal("Description", entry.Description);
        Assert.Equal("route", entry.Value);
        Assert.Same(icon, entry.Icon);
    }

    /// <summary>
    /// Ensures that a null page factory is rejected.
    /// </summary>
    [Fact]
    public void PageDefinition_ShouldRejectNullPageFactory()
    {
        Assert.Throws<ArgumentNullException>(
            "pageFactory",
            () => new XNavigationPageDefinition("route", "Title", "Description", null!));
    }

    /// <summary>
    /// Ensures that section definitions expose constructor values and fluent page registration.
    /// </summary>
    [Fact]
    public void SectionDefinition_AddPage_ShouldRegisterPageAndReturnSection()
    {
        XNavigationSectionDefinition section = new("section", "Section", "Description");

        XNavigationSectionDefinition result = section.AddPage("page", "Page", "Page description", static () => "Page instance");

        Assert.Same(section, result);
        Assert.Equal("section", section.Value);
        Assert.Equal("Section", section.Title);
        Assert.Equal("Description", section.Description);
        Assert.Single(section.Pages);
        Assert.Equal("page", section.Pages[0].Value);
        Assert.Equal("Page", section.Pages[0].Title);
        Assert.Equal("Page description", section.Pages[0].Description);
    }

    /// <summary>
    /// Ensures that default page factories follow their configured cache mode.
    /// </summary>
    [Fact]
    public void SectionDefinition_DefaultPage_ShouldRespectCacheMode()
    {
        XNavigationSectionDefinition uncachedSection = new XNavigationSectionDefinition("section", "Section", "Description")
            .WithDefaultPage(static () => new object());
        XNavigationSectionDefinition cachedSection = new XNavigationSectionDefinition("cached", "Cached", "Description")
            .WithDefaultPage(static () => new object(), XNavigationCacheMode.PerRoute);

        object? firstUncached = uncachedSection.CreateDefaultPage();
        object? secondUncached = uncachedSection.CreateDefaultPage();
        object? firstCached = cachedSection.CreateDefaultPage();
        object? secondCached = cachedSection.CreateDefaultPage();

        Assert.NotSame(firstUncached, secondUncached);
        Assert.Same(firstCached, secondCached);
        Assert.Equal(XNavigationCacheMode.PerRoute, cachedSection.DefaultPageCacheMode);
    }

    /// <summary>
    /// Ensures that side panel factories follow their configured cache mode and side-content aliases.
    /// </summary>
    [Fact]
    public void SectionDefinition_SidePanel_ShouldRespectCacheModeAndSideContentAlias()
    {
        XNavigationSectionDefinition section = new XNavigationSectionDefinition("section", "Section", "Description")
            .WithSidePanel(static () => new object(), XNavigationCacheMode.PerRoute);

        object? first = section.CreateSidePanel();
        object? second = section.CreateSideContent();

        Assert.True(section.HasSidePanel);
        Assert.True(section.HasCustomSideContent);
        Assert.False(section.HasWorkspace);
        Assert.Equal(XNavigationCacheMode.PerRoute, section.SidePanelCacheMode);
        Assert.Same(first, second);
    }

    /// <summary>
    /// Ensures that custom section workspaces follow their configured cache mode.
    /// </summary>
    [Fact]
    public void SectionDefinition_Workspace_ShouldRespectCacheMode()
    {
        XNavigationSectionDefinition section = new XNavigationSectionDefinition("section", "Section", "Description")
            .WithWorkspace(static () => new XNavigationSectionWorkspace("Side", "Page"), XNavigationCacheMode.PerRoute);

        XNavigationSectionWorkspace? first = section.CreateWorkspace();
        XNavigationSectionWorkspace? second = section.CreateWorkspace();

        Assert.True(section.HasWorkspace);
        Assert.True(section.HasSidePanel);
        Assert.Same(first, second);
        Assert.Equal("Side", first?.SidePanel);
        Assert.Equal("Page", first?.Page);
    }

    /// <summary>
    /// Ensures that workspace cache mode changes can seed the current workspace.
    /// </summary>
    [Fact]
    public void SectionDefinition_SetWorkspaceCacheMode_ShouldCacheCurrentWorkspaceWhenProvided()
    {
        XNavigationSectionWorkspace currentWorkspace = new("Side", "Page");
        XNavigationSectionDefinition section = new XNavigationSectionDefinition("section", "Section", "Description")
            .WithWorkspace(static () => new XNavigationSectionWorkspace("Other", "Other"));

        section.SetWorkspaceCacheMode(XNavigationCacheMode.PerRoute, currentWorkspace);

        Assert.Equal(XNavigationCacheMode.PerRoute, section.WorkspaceCacheMode);
        Assert.Same(currentWorkspace, section.CreateWorkspace());
    }

    /// <summary>
    /// Ensures that section factory methods reject null factories.
    /// </summary>
    [Fact]
    public void SectionDefinition_ShouldRejectNullFactories()
    {
        XNavigationSectionDefinition section = new("section", "Section", "Description");

        Assert.Throws<ArgumentNullException>("pageFactory", () => section.AddPage("page", "Page", "Description", null!));
        Assert.Throws<ArgumentNullException>("pageFactory", () => section.WithDefaultPage(null!));
        Assert.Throws<ArgumentNullException>("factory", () => section.WithSidePanel(null!));
        Assert.Throws<ArgumentNullException>("factory", () => section.WithWorkspace(null!));
        Assert.Throws<ArgumentNullException>("factory", () => section.WithSideContent(null!));
    }

    /// <summary>
    /// Ensures that section workspaces expose their constructor values and derived flags.
    /// </summary>
    [Fact]
    public void SectionWorkspace_ShouldExposeConstructorValuesAndDerivedFlags()
    {
        XNavigationEntry first = new("First", "first");
        XNavigationEntry second = new("Second", "second");
        object page = new();

        XNavigationSectionWorkspace workspace = new(
            null,
            page,
            [first, second],
            second);

        Assert.Null(workspace.SidePanel);
        Assert.Same(page, workspace.Page);
        Assert.Equal(2, workspace.PageEntries.Count);
        Assert.Same(second, workspace.SelectedPage);
        Assert.True(workspace.UsesStandardPageNavigation);
        Assert.False(workspace.UsesSidePanel);
    }

    /// <summary>
    /// Ensures that an empty section workspace has no side panel, page or standard navigation.
    /// </summary>
    [Fact]
    public void SectionWorkspace_CreateEmpty_ShouldReturnEmptyWorkspace()
    {
        XNavigationSectionWorkspace workspace = XNavigationSectionWorkspace.CreateEmpty();

        Assert.Null(workspace.SidePanel);
        Assert.Null(workspace.Page);
        Assert.Null(workspace.SelectedPage);
        Assert.Empty(workspace.PageEntries);
        Assert.False(workspace.UsesStandardPageNavigation);
        Assert.False(workspace.UsesSidePanel);
    }

    /// <summary>
    /// Ensures that the navigation manager can register and resolve sections and pages.
    /// </summary>
    [Fact]
    public void NavigationManager_ShouldRegisterAndResolveSectionsAndPages()
    {
        XNavigationManager manager = new();
        XNavigationSectionDefinition firstSection = manager.AddSection("section1", "Section 1", "Description 1");
        XNavigationSectionDefinition secondSection = manager.AddSection("section2", "Section 2", "Description 2");
        firstSection.AddPage("page1", "Page 1", "Page description", static () => "First page");
        secondSection.AddPage("page2", "Page 2", "Page description", static () => "Second page");

        Assert.Same(firstSection, manager.GetFirstSection());
        Assert.Same(secondSection, manager.FindSection("section2"));
        Assert.Same(firstSection.Pages[0], manager.FindPage("page1"));
        Assert.Same(secondSection.Pages[0], manager.FindPage(secondSection, "page2"));
        Assert.Null(manager.FindSection("missing"));
        Assert.Null(manager.FindPage("missing"));
        Assert.Null(manager.FindPage(null, "page1"));
    }

    /// <summary>
    /// Ensures that the navigation manager creates pages by value or entry.
    /// </summary>
    [Fact]
    public void NavigationManager_CreatePage_ShouldCreatePageByValueOrEntry()
    {
        XNavigationManager manager = new();
        manager.AddSection("section", "Section", "Description")
            .AddPage("page", "Page", "Description", static () => "Created page");

        object? byValue = manager.CreatePage("page");
        object? byEntry = manager.CreatePage(new XNavigationEntry("Page", "page"));
        object? missing = manager.CreatePage("missing");
        object? nullEntry = manager.CreatePage((XNavigationEntry?)null);

        Assert.Equal("Created page", byValue);
        Assert.Equal("Created page", byEntry);
        Assert.Null(missing);
        Assert.Null(nullEntry);
    }

    /// <summary>
    /// Ensures that the navigation manager creates a standard page-navigation workspace.
    /// </summary>
    [Fact]
    public void NavigationManager_CreateSectionWorkspace_ShouldCreateStandardWorkspace()
    {
        XNavigationManager manager = new();
        XNavigationSectionDefinition section = manager.AddSection("section", "Section", "Description");
        section.AddPage("first", "First", "First description", static () => "First page");
        section.AddPage("second", "Second", "Second description", static () => "Second page");

        XNavigationSectionWorkspace workspace = manager.CreateSectionWorkspace(section, "second");

        Assert.Null(workspace.SidePanel);
        Assert.Equal("Second page", workspace.Page);
        Assert.Equal(2, workspace.PageEntries.Count);
        Assert.Equal("second", workspace.SelectedPage?.Value);
        Assert.True(workspace.UsesStandardPageNavigation);
    }

    /// <summary>
    /// Ensures that the navigation manager uses the default page when a side panel replaces standard navigation.
    /// </summary>
    [Fact]
    public void NavigationManager_CreateSectionWorkspace_ShouldUseSidePanelAndDefaultPage()
    {
        object sidePanel = new();
        object defaultPage = new();
        XNavigationManager manager = new();
        XNavigationSectionDefinition section = manager.AddSection("section", "Section", "Description")
            .WithSidePanel(() => sidePanel)
            .WithDefaultPage(() => defaultPage);
        section.AddPage("page", "Page", "Description", static () => "Page");

        XNavigationSectionWorkspace workspace = manager.CreateSectionWorkspace(section);

        Assert.Same(sidePanel, workspace.SidePanel);
        Assert.Same(defaultPage, workspace.Page);
        Assert.Single(workspace.PageEntries);
        Assert.Null(workspace.SelectedPage);
        Assert.False(workspace.UsesStandardPageNavigation);
        Assert.True(workspace.UsesSidePanel);
    }

    /// <summary>
    /// Ensures that the navigation manager returns a custom workspace when configured.
    /// </summary>
    [Fact]
    public void NavigationManager_CreateSectionWorkspace_ShouldUseCustomWorkspaceWhenConfigured()
    {
        XNavigationSectionWorkspace customWorkspace = new("Side", "Page");
        XNavigationManager manager = new();
        XNavigationSectionDefinition section = manager.AddSection("section", "Section", "Description")
            .WithWorkspace(() => customWorkspace);

        XNavigationSectionWorkspace workspace = manager.CreateSectionWorkspace(section);

        Assert.Same(customWorkspace, workspace);
    }

    /// <summary>
    /// Ensures that the navigation manager returns an empty workspace for a null section.
    /// </summary>
    [Fact]
    public void NavigationManager_CreateSectionWorkspace_ShouldReturnEmptyWorkspaceForNullSection()
    {
        XNavigationManager manager = new();

        XNavigationSectionWorkspace workspace = manager.CreateSectionWorkspace(null);

        Assert.Null(workspace.SidePanel);
        Assert.Null(workspace.Page);
        Assert.Empty(workspace.PageEntries);
    }
    #endregion
}
#endregion