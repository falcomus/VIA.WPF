// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationPageDefinition.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationPageDefinition ###
/// <summary>
/// Describes one selectable page inside an application navigation.
/// </summary>
public sealed class XNavigationPageDefinition
{
    #region ### Fields ###
    private readonly Func<object?> pageFactory;
    private object? cachedPage;
    private bool hasCachedPage;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationPageDefinition"/> class.
    /// </summary>
    /// <param name="value">The route or value of the page.</param>
    /// <param name="title">The displayed title of the page.</param>
    /// <param name="description">The displayed description of the page.</param>
    /// <param name="pageFactory">The factory used to create the page view model.</param>
    /// <param name="cacheMode">The cache mode used for this page.</param>
    public XNavigationPageDefinition(
        object? value,
        string title,
        string description,
        Func<object?> pageFactory,
        XNavigationCacheMode cacheMode = XNavigationCacheMode.None)
    {
        this.Value = value;
        this.Title = title;
        this.Description = description;
        this.pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
        this.CacheMode = cacheMode;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the route or value of the page.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the displayed title of the page.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the displayed description of the page.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the optional icon.
    /// </summary>
    public object? Icon { get; set; }

    /// <summary>
    /// Gets the cache mode used for this page.
    /// </summary>
    public XNavigationCacheMode CacheMode { get; private set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates or resolves the page represented by this definition.
    /// </summary>
    /// <returns>The created or cached page object.</returns>
    public object? CreatePage()
    {
        if (this.CacheMode != XNavigationCacheMode.PerRoute)
        {
            return this.pageFactory();
        }

        if (!this.hasCachedPage)
        {
            this.cachedPage = this.pageFactory();
            this.hasCachedPage = true;
        }

        return this.cachedPage;
    }

    /// <summary>
    /// Sets the cache mode used for this page.
    /// </summary>
    /// <param name="cacheMode">The new cache mode.</param>
    /// <param name="currentPage">The currently displayed page that should be cached immediately when caching is enabled.</param>
    public void SetCacheMode(XNavigationCacheMode cacheMode, object? currentPage = null)
    {
        if (this.CacheMode == cacheMode)
        {
            if (cacheMode == XNavigationCacheMode.PerRoute && currentPage is not null && !this.hasCachedPage)
            {
                this.cachedPage = currentPage;
                this.hasCachedPage = true;
            }

            return;
        }

        this.CacheMode = cacheMode;

        if (cacheMode == XNavigationCacheMode.PerRoute)
        {
            if (currentPage is not null)
            {
                this.cachedPage = currentPage;
                this.hasCachedPage = true;
            }

            return;
        }

        this.ClearCachedPage();
    }

    /// <summary>
    /// Clears the cached page instance.
    /// </summary>
    public void ClearCachedPage()
    {
        this.cachedPage = null;
        this.hasCachedPage = false;
    }

    /// <summary>
    /// Creates a navigation entry for this page definition.
    /// </summary>
    /// <returns>The navigation entry.</returns>
    public XNavigationEntry ToEntry()
    {
        return new XNavigationEntry(this.Title, this.Value)
        {
            Description = this.Description,
            Icon = this.Icon
        };
    }
    #endregion
}
#endregion
