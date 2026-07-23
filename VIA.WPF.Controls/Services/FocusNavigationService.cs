// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FocusNavigationService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Services;

#region ### Class FocusNavigationService ###
/// <summary>
/// Provides focus navigation helpers for WPF dialogs, forms and custom controls.
/// </summary>
public static class FocusNavigationService
{
    #region ### Public Methods ###
    /// <summary>
    /// Focuses the first focusable element in the specified root.
    /// </summary>
    /// <param name="root">The root element.</param>
    /// <returns><c>true</c> if an element was focused; otherwise, <c>false</c>.</returns>
    public static bool FocusFirstInput(DependencyObject? root)
    {
        UIElement? target = FindFocusableChildren(root).FirstOrDefault();
        return FocusElement(target);
    }

    /// <summary>
    /// Moves focus to the next focus target from the specified root element.
    /// </summary>
    /// <param name="root">The root element.</param>
    /// <returns><c>true</c> if focus was moved; otherwise, <c>false</c>.</returns>
    public static bool FocusNext(DependencyObject? root)
    {
        if (root is not UIElement element)
        {
            return false;
        }

        TraversalRequest request = new(FocusNavigationDirection.Next);
        return element.MoveFocus(request);
    }

    /// <summary>
    /// Moves focus to the previous focus target from the specified root element.
    /// </summary>
    /// <param name="root">The root element.</param>
    /// <returns><c>true</c> if focus was moved; otherwise, <c>false</c>.</returns>
    public static bool FocusPrevious(DependencyObject? root)
    {
        if (root is not UIElement element)
        {
            return false;
        }

        TraversalRequest request = new(FocusNavigationDirection.Previous);
        return element.MoveFocus(request);
    }

    /// <summary>
    /// Returns all focusable child elements of the specified root.
    /// </summary>
    /// <param name="root">The root element.</param>
    /// <returns>The focusable child elements.</returns>
    public static IEnumerable<UIElement> FindFocusableChildren(DependencyObject? root)
    {
        if (root is null)
        {
            yield break;
        }

        foreach (DependencyObject child in EnumerateVisualChildren(root))
        {
            if (child is UIElement element && IsFocusableInput(element))
            {
                yield return element;
            }

            foreach (UIElement descendant in FindFocusableChildren(child))
            {
                yield return descendant;
            }
        }
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Focuses the specified element.
    /// </summary>
    /// <param name="element">The element to focus.</param>
    /// <returns><c>true</c> if the element was focused; otherwise, <c>false</c>.</returns>
    private static bool FocusElement(UIElement? element)
    {
        if (element is null)
        {
            return false;
        }

        element.Focus();
        return Keyboard.Focus(element) == element || element.IsKeyboardFocusWithin;
    }

    /// <summary>
    /// Gets a value indicating whether the specified element can receive focus.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the element can receive focus; otherwise, <c>false</c>.</returns>
    private static bool IsFocusableInput(UIElement element)
    {
        if (!element.Focusable || !element.IsEnabled || !element.IsVisible)
        {
            return false;
        }

        if (element is Control control && !control.IsTabStop)
        {
            return false;
        }

        return element is TextBoxBase or PasswordBox or ComboBox or Button or CheckBox or RadioButton or Slider or ListBox or DataGrid;
    }

    /// <summary>
    /// Enumerates visual children of a dependency object.
    /// </summary>
    /// <param name="root">The root object.</param>
    /// <returns>The visual children.</returns>
    private static IEnumerable<DependencyObject> EnumerateVisualChildren(DependencyObject root)
    {
        int childCount;

        try
        {
            childCount = VisualTreeHelper.GetChildrenCount(root);
        }
        catch (InvalidOperationException)
        {
            yield break;
        }

        for (int index = 0; index < childCount; index++)
        {
            yield return VisualTreeHelper.GetChild(root, index);
        }
    }
    #endregion
}
#endregion
