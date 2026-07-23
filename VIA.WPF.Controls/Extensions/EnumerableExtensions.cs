// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Extensions;

#region ### Class EnumerableExtensions ###
/// <summary>
/// Provides convenience methods for working with enumerable sequences.
/// </summary>
public static class EnumerableExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Returns an empty sequence when the source sequence is null.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>The source sequence or an empty sequence.</returns>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
    {
        return source ?? Enumerable.Empty<T>();
    }

    /// <summary>
    /// Executes the specified action for every item in the sequence.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="action">The action to execute.</param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (T item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// Filters out null values from a nullable reference sequence.
    /// </summary>
    /// <typeparam name="T">The non-null item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>The non-null items.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (T? item in source)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Gets the zero-based index of the first item that matches the specified predicate.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The item index, or -1.</returns>
    public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        int index = 0;

        foreach (T item in source)
        {
            if (predicate(item))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    /// Gets a value indicating whether the sequence contains the exact reference instance.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="item">The reference item.</param>
    /// <returns><c>true</c> if the reference is contained; otherwise, <c>false</c>.</returns>
    public static bool ContainsReference<T>(this IEnumerable<T> source, T item)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Any(current => ReferenceEquals(current, item));
    }

    /// <summary>
    /// Converts the sequence to an observable collection.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>The observable collection.</returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ObservableCollection<T>(source);
    }
    #endregion
}
#endregion
