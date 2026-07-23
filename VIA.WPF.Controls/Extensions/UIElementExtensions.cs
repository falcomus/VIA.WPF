// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIElementExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Extensions;

#region ### Class UIElementExtensions ###
/// <summary>
/// Provides convenience methods for WPF UI elements.
/// </summary>
public static class UIElementExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Sets the visibility to <see cref="Visibility.Visible"/> or <see cref="Visibility.Collapsed"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="element">The UI element.</param>
    /// <param name="isVisible">A value indicating whether the element should be visible.</param>
    /// <returns>The same element instance.</returns>
    public static T SetVisible<T>(this T element, bool isVisible)
        where T : UIElement
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

        return element;
    }

    /// <summary>
    /// Sets the visibility to <see cref="Visibility.Visible"/> or <see cref="Visibility.Hidden"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="element">The UI element.</param>
    /// <param name="isVisible">A value indicating whether the element should be visible.</param>
    /// <returns>The same element instance.</returns>
    public static T SetHiddenWhenInvisible<T>(this T element, bool isVisible)
        where T : UIElement
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;

        return element;
    }

    /// <summary>
    /// Sets hit testing on the element and returns the same instance.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="element">The UI element.</param>
    /// <param name="isHitTestVisible">A value indicating whether hit testing should be enabled.</param>
    /// <returns>The same element instance.</returns>
    public static T SetHitTestVisible<T>(this T element, bool isHitTestVisible)
        where T : UIElement
    {
        ArgumentNullException.ThrowIfNull(element);

        element.IsHitTestVisible = isHitTestVisible;

        return element;
    }

    /// <summary>
    /// Gets the bounds of the element relative to another visual.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <param name="relativeTo">The visual that defines the target coordinate system.</param>
    /// <returns>The transformed bounds, or <see cref="Rect.Empty"/> if they cannot be calculated.</returns>
    public static Rect GetBoundsRelativeTo(this FrameworkElement element, Visual relativeTo)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(relativeTo);

        if (element.ActualWidth <= 0d || element.ActualHeight <= 0d)
        {
            return Rect.Empty;
        }

        try
        {
            GeneralTransform transform = element.TransformToAncestor(relativeTo);

            return transform.TransformBounds(new Rect(0d, 0d, element.ActualWidth, element.ActualHeight));
        }
        catch (InvalidOperationException)
        {
            return Rect.Empty;
        }
    }

    /// <summary>
    /// Releases mouse capture when the element currently owns it.
    /// </summary>
    /// <param name="element">The UI element.</param>
    public static void ReleaseMouseCaptureIfCaptured(this UIElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (element.IsMouseCaptured)
        {
            element.ReleaseMouseCapture();
        }
    }
    #endregion
}
#endregion
