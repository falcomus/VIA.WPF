// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdornerLayerExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace VIA.WPF.Extensions;

#region ### Class AdornerLayerExtensions ###
/// <summary>
/// Provides helper methods for working with WPF adorners.
/// </summary>
public static class AdornerLayerExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the nearest adorner layer for the specified element.
    /// </summary>
    /// <param name="element">The adorned element.</param>
    /// <returns>The adorner layer or <see langword="null"/>.</returns>
    public static AdornerLayer? GetAdornerLayerSafe(this UIElement? element)
    {
        return element is null ? null : AdornerLayer.GetAdornerLayer(element);
    }

    /// <summary>
    /// Adds the specified adorner to the nearest adorner layer.
    /// </summary>
    /// <param name="element">The adorned element.</param>
    /// <param name="adorner">The adorner to add.</param>
    /// <returns><c>true</c> if the adorner was added; otherwise, <c>false</c>.</returns>
    public static bool AddAdorner(this UIElement? element, Adorner adorner)
    {
        ArgumentNullException.ThrowIfNull(adorner);

        AdornerLayer? layer = element.GetAdornerLayerSafe();
        if (layer is null)
        {
            return false;
        }

        layer.Add(adorner);
        return true;
    }

    /// <summary>
    /// Gets all adorners of the requested type for the specified element.
    /// </summary>
    /// <typeparam name="T">The adorner type.</typeparam>
    /// <param name="element">The adorned element.</param>
    /// <returns>The matching adorners.</returns>
    public static IReadOnlyList<T> GetAdorners<T>(this UIElement? element)
        where T : Adorner
    {
        if (element is null)
        {
            return [];
        }

        AdornerLayer? layer = element.GetAdornerLayerSafe();
        Adorner[]? adorners = layer?.GetAdorners(element);

        return adorners?.OfType<T>().ToArray() ?? [];
    }

    /// <summary>
    /// Removes all adorners of the requested type from the specified element.
    /// </summary>
    /// <typeparam name="T">The adorner type.</typeparam>
    /// <param name="element">The adorned element.</param>
    /// <returns>The number of removed adorners.</returns>
    public static int RemoveAdorners<T>(this UIElement? element)
        where T : Adorner
    {
        if (element is null)
        {
            return 0;
        }

        AdornerLayer? layer = element.GetAdornerLayerSafe();
        if (layer is null)
        {
            return 0;
        }

        int removedCount = 0;

        foreach (T adorner in element.GetAdorners<T>())
        {
            layer.Remove(adorner);
            removedCount++;
        }

        return removedCount;
    }

    /// <summary>
    /// Invalidates all adorners assigned to the specified element.
    /// </summary>
    /// <param name="element">The adorned element.</param>
    public static void InvalidateAdorners(this UIElement? element)
    {
        if (element is null)
        {
            return;
        }

        AdornerLayer? layer = element.GetAdornerLayerSafe();
        Adorner[]? adorners = layer?.GetAdorners(element);

        if (adorners is null)
        {
            return;
        }

        foreach (Adorner adorner in adorners)
        {
            adorner.InvalidateVisual();
            adorner.InvalidateMeasure();
            adorner.InvalidateArrange();
        }
    }

    /// <summary>
    /// Calculates the bounds of an element relative to another visual.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="relativeTo">The target visual.</param>
    /// <returns>The calculated bounds or <see cref="Rect.Empty"/>.</returns>
    public static Rect GetBoundsRelativeTo(this FrameworkElement? element, Visual? relativeTo)
    {
        if (element is null || relativeTo is null || element.ActualWidth <= 0 || element.ActualHeight <= 0)
        {
            return Rect.Empty;
        }

        GeneralTransform transform = element.TransformToAncestor(relativeTo);
        return transform.TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));
    }
    #endregion
}
#endregion
