// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Extensions;

#region ### Class TreeExtensions ###
/// <summary>
/// Provides generic helper methods for tree-shaped data structures.
/// </summary>
public static class TreeExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Traverses the tree in depth-first order.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="roots">The root nodes.</param>
    /// <param name="childrenSelector">The children selector.</param>
    /// <returns>The traversed nodes.</returns>
    public static IEnumerable<T> TraverseDepthFirst<T>(this IEnumerable<T>? roots, Func<T, IEnumerable<T>?> childrenSelector)
    {
        ArgumentNullException.ThrowIfNull(childrenSelector);

        Stack<T> stack = new();

        foreach (T root in roots.EmptyIfNull().Reverse())
        {
            stack.Push(root);
        }

        while (stack.Count > 0)
        {
            T current = stack.Pop();
            yield return current;

            IEnumerable<T>? children = childrenSelector(current);

            foreach (T child in children.EmptyIfNull().Reverse())
            {
                stack.Push(child);
            }
        }
    }

    /// <summary>
    /// Traverses the tree in breadth-first order.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="roots">The root nodes.</param>
    /// <param name="childrenSelector">The children selector.</param>
    /// <returns>The traversed nodes.</returns>
    public static IEnumerable<T> TraverseBreadthFirst<T>(this IEnumerable<T>? roots, Func<T, IEnumerable<T>?> childrenSelector)
    {
        ArgumentNullException.ThrowIfNull(childrenSelector);

        Queue<T> queue = new();

        foreach (T root in roots.EmptyIfNull())
        {
            queue.Enqueue(root);
        }

        while (queue.Count > 0)
        {
            T current = queue.Dequeue();
            yield return current;

            IEnumerable<T>? children = childrenSelector(current);

            foreach (T child in children.EmptyIfNull())
            {
                queue.Enqueue(child);
            }
        }
    }

    /// <summary>
    /// Finds the first node that matches the specified predicate in depth-first order.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="roots">The root nodes.</param>
    /// <param name="childrenSelector">The children selector.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The matching node, or <c>default</c>.</returns>
    public static T? FindInTree<T>(this IEnumerable<T>? roots, Func<T, IEnumerable<T>?> childrenSelector, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(childrenSelector);
        ArgumentNullException.ThrowIfNull(predicate);

        return roots.TraverseDepthFirst(childrenSelector).FirstOrDefault(item => predicate(item));
    }

    /// <summary>
    /// Returns the specified node and all ancestors by using a parent selector.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="node">The start node.</param>
    /// <param name="parentSelector">The parent selector.</param>
    /// <returns>The node and its ancestors.</returns>
    public static IEnumerable<T> SelfAndAncestors<T>(this T? node, Func<T, T?> parentSelector)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(parentSelector);

        T? current = node;

        while (current is not null)
        {
            yield return current;
            current = parentSelector(current);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the specified item exists in the tree by reference equality.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="roots">The root nodes.</param>
    /// <param name="childrenSelector">The children selector.</param>
    /// <param name="item">The item to find.</param>
    /// <returns><c>true</c> if the item reference exists; otherwise, <c>false</c>.</returns>
    public static bool ContainsReferenceInTree<T>(this IEnumerable<T>? roots, Func<T, IEnumerable<T>?> childrenSelector, T item)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(childrenSelector);

        return roots.TraverseDepthFirst(childrenSelector).Any(current => ReferenceEquals(current, item));
    }
    #endregion
}
#endregion
