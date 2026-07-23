// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VIA.WPF.Extensions;

#region ### Class DependencyObjectExtensions ###
/// <summary>
/// Provides helper methods for traversing WPF visual and logical trees.
/// </summary>
public static class DependencyObjectExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Finds the first parent of the specified type by walking the visual tree and then the logical tree.
    /// </summary>
    /// <typeparam name="T">The parent type to search for.</typeparam>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The first matching parent, or <c>null</c>.</returns>
    public static T? FindVisualParent<T>(this DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        DependencyObject? parent = GetParent(dependencyObject);

        while (parent is not null)
        {
            if (parent is T typedParent)
            {
                return typedParent;
            }

            parent = GetParent(parent);
        }

        return null;
    }

    /// <summary>
    /// Finds the specified object itself or the first parent of the specified type.
    /// </summary>
    /// <typeparam name="T">The object type to search for.</typeparam>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The object itself or the first matching parent, or <c>null</c>.</returns>
    public static T? FindVisualAncestorOrSelf<T>(this DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        DependencyObject? current = dependencyObject;

        while (current is not null)
        {
            if (current is T typedCurrent)
            {
                return typedCurrent;
            }

            current = GetParent(current);
        }

        return null;
    }

    /// <summary>
    /// Finds the first visual child of the specified type using depth-first traversal.
    /// </summary>
    /// <typeparam name="T">The child type to search for.</typeparam>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The first matching child, or <c>null</c>.</returns>
    public static T? FindVisualChild<T>(this DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        if (dependencyObject is null)
        {
            return null;
        }

        foreach (DependencyObject child in GetChildren(dependencyObject))
        {
            if (child is T typedChild)
            {
                return typedChild;
            }

            T? nestedChild = child.FindVisualChild<T>();

            if (nestedChild is not null)
            {
                return nestedChild;
            }
        }

        return null;
    }

    /// <summary>
    /// Enumerates all visual and logical children of the specified type using depth-first traversal.
    /// </summary>
    /// <typeparam name="T">The child type to enumerate.</typeparam>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The matching descendants.</returns>
    public static IEnumerable<T> GetVisualDescendants<T>(this DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        if (dependencyObject is null)
        {
            yield break;
        }

        Stack<DependencyObject> stack = new();

        foreach (DependencyObject child in GetChildren(dependencyObject).Reverse())
        {
            stack.Push(child);
        }

        while (stack.Count > 0)
        {
            DependencyObject current = stack.Pop();

            if (current is T typedCurrent)
            {
                yield return typedCurrent;
            }

            foreach (DependencyObject child in GetChildren(current).Reverse())
            {
                stack.Push(child);
            }
        }
    }

    /// <summary>
    /// Enumerates the specified object itself and all visual and logical descendants of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to enumerate.</typeparam>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The matching object and descendants.</returns>
    public static IEnumerable<T> GetVisualSelfAndDescendants<T>(this DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        if (dependencyObject is T typedDependencyObject)
        {
            yield return typedDependencyObject;
        }

        foreach (T descendant in dependencyObject.GetVisualDescendants<T>())
        {
            yield return descendant;
        }
    }

    /// <summary>
    /// Returns the nearest object of the specified type at the given point.
    /// </summary>
    /// <typeparam name="T">The object type to search for.</typeparam>
    /// <param name="root">The hit test root.</param>
    /// <param name="point">The point relative to the root.</param>
    /// <returns>The matching object, or <c>null</c>.</returns>
    public static T? HitTestVisual<T>(this DependencyObject? root, Point point)
    where T : DependencyObject
    {
        if (root is not Visual visual)
        {
            return null;
        }

        HitTestResult? result = VisualTreeHelper.HitTest(visual, point);

        return result?.VisualHit.FindVisualAncestorOrSelf<T>();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Gets the parent of the specified dependency object.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The parent dependency object, or <c>null</c>.</returns>
    private static DependencyObject? GetParent(DependencyObject? dependencyObject)
    {
        if (dependencyObject is null)
        {
            return null;
        }

        DependencyObject? visualParent = dependencyObject is Visual or Visual3D
            ? VisualTreeHelper.GetParent(dependencyObject)
            : null;

        if (visualParent is not null)
        {
            return visualParent;
        }

        if (dependencyObject is FrameworkElement frameworkElement)
        {
            return frameworkElement.Parent;
        }

        if (dependencyObject is FrameworkContentElement frameworkContentElement)
        {
            return frameworkContentElement.Parent;
        }

        return LogicalTreeHelper.GetParent(dependencyObject);
    }

    /// <summary>
    /// Gets the visual and logical children of the specified dependency object.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The child dependency objects.</returns>
    private static IEnumerable<DependencyObject> GetChildren(DependencyObject dependencyObject)
    {
        HashSet<DependencyObject> yieldedChildren = [];

        if (dependencyObject is Visual or Visual3D)
        {
            int visualChildrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            for (int index = 0; index < visualChildrenCount; index++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);

                if (yieldedChildren.Add(child))
                {
                    yield return child;
                }
            }
        }

        foreach (object logicalChild in LogicalTreeHelper.GetChildren(dependencyObject))
        {
            if (logicalChild is DependencyObject dependencyChild && yieldedChildren.Add(dependencyChild))
            {
                yield return dependencyChild;
            }
        }
    }
    #endregion
}
#endregion
