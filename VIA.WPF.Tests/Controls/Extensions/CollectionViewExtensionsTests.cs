// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionViewExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class CollectionViewExtensionsTests ###
/// <summary>
/// Provides tests for collection view extension helpers.
/// </summary>
public sealed class CollectionViewExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that null collection views can be refreshed and deferred safely.
    /// </summary>
    [Fact]
    public void CollectionViewExtensions_ShouldHandleNullCollectionViewSafely()
    {
        ICollectionView? collectionView = null;

        collectionView.RefreshIfNotNull();
        using IDisposable deferToken = collectionView.DeferRefreshIfNotNull();

        Assert.NotNull(deferToken);
    }

    /// <summary>
    /// Ensures that filters are assigned and immediately affect the collection view.
    /// </summary>
    [Fact]
    public void CollectionViewExtensions_SetFilter_ShouldApplyFilter()
    {
        WpfTestHelper.Run(
            () =>
            {
                ObservableCollection<TestItem> items = CreateItems();
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(items);

                collectionView.SetFilter(item => ((TestItem)item).Age >= 30);

                Assert.Equal(new[] { "Charlie", "Alice" }, collectionView.Cast<TestItem>().Select(item => item.Name));

                collectionView.SetFilter(null);

                Assert.Equal(new[] { "Charlie", "Alice", "Bob" }, collectionView.Cast<TestItem>().Select(item => item.Name));
            });
    }

    /// <summary>
    /// Ensures that sorting can replace or append sort descriptions.
    /// </summary>
    [Fact]
    public void CollectionViewExtensions_SetSort_ShouldApplySortDescriptions()
    {
        WpfTestHelper.Run(
            () =>
            {
                ObservableCollection<TestItem> items = CreateItems();
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(items);

                collectionView.SetSort(nameof(TestItem.Name));

                Assert.Single(collectionView.SortDescriptions);
                Assert.Equal(nameof(TestItem.Name), collectionView.SortDescriptions[0].PropertyName);
                Assert.Equal(ListSortDirection.Ascending, collectionView.SortDescriptions[0].Direction);
                Assert.Equal(new[] { "Alice", "Bob", "Charlie" }, collectionView.Cast<TestItem>().Select(item => item.Name));

                collectionView.SetSort(nameof(TestItem.Age), ListSortDirection.Descending, clearExisting: false);

                Assert.Equal(2, collectionView.SortDescriptions.Count);
                Assert.Equal(nameof(TestItem.Age), collectionView.SortDescriptions[1].PropertyName);
                Assert.Equal(ListSortDirection.Descending, collectionView.SortDescriptions[1].Direction);
            });
    }

    /// <summary>
    /// Ensures that sorting can be cleared.
    /// </summary>
    [Fact]
    public void CollectionViewExtensions_ClearSort_ShouldRemoveSortDescriptions()
    {
        WpfTestHelper.Run(
            () =>
            {
                ObservableCollection<TestItem> items = CreateItems();
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(items);
                collectionView.SetSort(nameof(TestItem.Name));

                collectionView.ClearSort();

                Assert.Empty(collectionView.SortDescriptions);
            });
    }

    /// <summary>
    /// Ensures that collection view helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void CollectionViewExtensions_ShouldRejectNullArguments()
    {
        WpfTestHelper.Run(
            () =>
            {
                ObservableCollection<TestItem> items = CreateItems();
                ICollectionView? nullCollectionView = null;
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(items);

                Assert.Throws<ArgumentNullException>(() => nullCollectionView!.SetFilter(null));
                Assert.Throws<ArgumentNullException>(() => nullCollectionView!.SetSort(nameof(TestItem.Name)));
                Assert.Throws<ArgumentException>(() => collectionView.SetSort(""));
                Assert.Throws<ArgumentNullException>(() => nullCollectionView!.ClearSort());
            });
    }
    #endregion

    #region ### Private Methods ###
    private static ObservableCollection<TestItem> CreateItems()
    {
        return new ObservableCollection<TestItem>
        {
            new TestItem("Charlie", 30),
            new TestItem("Alice", 42),
            new TestItem("Bob", 18)
        };
    }
    #endregion

    #region ### Test Types ###
    private sealed class TestItem
    {
        public TestItem(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        public string Name { get; }

        public int Age { get; }
    }
    #endregion
}
#endregion
