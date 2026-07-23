// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionViewExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace VIA.WPF.Extensions;

#region ### Class CollectionViewExtensions ###
/// <summary>
/// Provides convenience methods for WPF collection views.
/// </summary>
public static class CollectionViewExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Refreshes the collection view when it is not null.
    /// </summary>
    /// <param name="collectionView">The collection view.</param>
    public static void RefreshIfNotNull(this ICollectionView? collectionView)
    {
        collectionView?.Refresh();
    }

    /// <summary>
    /// Defers refresh when the collection view is not null.
    /// </summary>
    /// <param name="collectionView">The collection view.</param>
    /// <returns>The defer token or an empty disposable token.</returns>
    public static IDisposable DeferRefreshIfNotNull(this ICollectionView? collectionView)
    {
        return collectionView?.DeferRefresh() ?? EmptyDisposable.Instance;
    }

    /// <summary>
    /// Replaces the filter of the collection view.
    /// </summary>
    /// <param name="collectionView">The collection view.</param>
    /// <param name="filter">The filter predicate.</param>
    public static void SetFilter(this ICollectionView collectionView, Predicate<object>? filter)
    {
        ArgumentNullException.ThrowIfNull(collectionView);

        collectionView.Filter = filter;
        collectionView.Refresh();
    }

    /// <summary>
    /// Sets a single sort description on the collection view.
    /// </summary>
    /// <param name="collectionView">The collection view.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="direction">The sort direction.</param>
    /// <param name="clearExisting">A value indicating whether existing sort descriptions should be removed.</param>
    public static void SetSort(this ICollectionView collectionView, string propertyName, ListSortDirection direction = ListSortDirection.Ascending, bool clearExisting = true)
    {
        ArgumentNullException.ThrowIfNull(collectionView);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        using (collectionView.DeferRefresh())
        {
            if (clearExisting)
            {
                collectionView.SortDescriptions.Clear();
            }

            collectionView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }
    }

    /// <summary>
    /// Clears sorting on the collection view.
    /// </summary>
    /// <param name="collectionView">The collection view.</param>
    public static void ClearSort(this ICollectionView collectionView)
    {
        ArgumentNullException.ThrowIfNull(collectionView);

        using (collectionView.DeferRefresh())
        {
            collectionView.SortDescriptions.Clear();
        }
    }
    #endregion

    #region ### Class EmptyDisposable ###
    /// <summary>
    /// Provides a disposable object that does nothing.
    /// </summary>
    private sealed class EmptyDisposable : IDisposable
    {
        #region ### Public Properties ###
        /// <summary>
        /// Gets the shared disposable instance.
        /// </summary>
        public static EmptyDisposable Instance { get; } = new();
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public void Dispose()
        {
        }
        #endregion
    }
    #endregion
}
#endregion
