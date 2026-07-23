// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Extensions;

#region ### Class ObservableCollectionExtensions ###
/// <summary>
/// Provides convenience methods for observable collections.
/// </summary>
public static class ObservableCollectionExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Adds all items from the specified sequence to the collection.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="collection">The observable collection.</param>
    /// <param name="items">The items to add.</param>
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(items);

        foreach (T item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Replaces all items in the collection with the specified items.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="collection">The observable collection.</param>
    /// <param name="items">The replacement items.</param>
    public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(items);

        collection.Clear();

        foreach (T item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Removes all items that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="collection">The observable collection.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The number of removed items.</returns>
    public static int RemoveWhere<T>(this ObservableCollection<T> collection, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        int removedCount = 0;

        for (int index = collection.Count - 1; index >= 0; index--)
        {
            if (predicate(collection[index]))
            {
                collection.RemoveAt(index);
                removedCount++;
            }
        }

        return removedCount;
    }

    /// <summary>
    /// Moves the specified item to the requested index when it exists in the collection.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="collection">The observable collection.</param>
    /// <param name="item">The item to move.</param>
    /// <param name="newIndex">The requested target index.</param>
    /// <returns><c>true</c> if the item was moved; otherwise, <c>false</c>.</returns>
    public static bool MoveItem<T>(this ObservableCollection<T> collection, T item, int newIndex)
    {
        ArgumentNullException.ThrowIfNull(collection);

        int oldIndex = collection.IndexOf(item);

        if (oldIndex < 0)
        {
            return false;
        }

        int safeNewIndex = Math.Clamp(newIndex, 0, collection.Count - 1);

        if (oldIndex == safeNewIndex)
        {
            return true;
        }

        collection.Move(oldIndex, safeNewIndex);

        return true;
    }
    #endregion
}
#endregion
