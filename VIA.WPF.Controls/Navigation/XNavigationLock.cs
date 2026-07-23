// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationLock.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationLock ###
/// <summary>
/// Coordinates window-wide navigation locking while a modal VIA.WPF view container dialog is open.
/// </summary>
public static class XNavigationLock
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsNavigationLocked attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsNavigationLockedProperty = DependencyProperty.RegisterAttached(
        "IsNavigationLocked",
        typeof(bool),
        typeof(XNavigationLock),
        new FrameworkPropertyMetadata(
            false,
            FrameworkPropertyMetadataOptions.Inherits,
            OnNavigationLockStateChanged));

    /// <summary>
    /// Identifies the DisableWhenNavigationLocked attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DisableWhenNavigationLockedProperty = DependencyProperty.RegisterAttached(
        "DisableWhenNavigationLocked",
        typeof(bool),
        typeof(XNavigationLock),
        new FrameworkPropertyMetadata(
            true,
            FrameworkPropertyMetadataOptions.Inherits,
            OnNavigationLockStateChanged));
    #endregion

    #region ### Private Fields ###
    private static readonly DependencyProperty NavigationLockCountProperty = DependencyProperty.RegisterAttached(
        "NavigationLockCount",
        typeof(int),
        typeof(XNavigationLock),
        new FrameworkPropertyMetadata(0));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether navigation is currently locked for the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if navigation is locked; otherwise, <c>false</c>.</returns>
    public static bool GetIsNavigationLocked(DependencyObject element)
    {
        return (bool)element.GetValue(IsNavigationLockedProperty);
    }

    /// <summary>
    /// Sets whether navigation is currently locked for the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsNavigationLocked(DependencyObject element, bool value)
    {
        element.SetValue(IsNavigationLockedProperty, value);
    }

    /// <summary>
    /// Gets whether the specified element should be disabled when navigation is locked.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the element reacts to navigation locks; otherwise, <c>false</c>.</returns>
    public static bool GetDisableWhenNavigationLocked(DependencyObject element)
    {
        return (bool)element.GetValue(DisableWhenNavigationLockedProperty);
    }

    /// <summary>
    /// Sets whether the specified element should be disabled when navigation is locked.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetDisableWhenNavigationLocked(DependencyObject element, bool value)
    {
        element.SetValue(DisableWhenNavigationLockedProperty, value);
    }

    /// <summary>
    /// Determines whether navigation is currently locked for the specified element or its owning window.
    /// </summary>
    /// <param name="element">The element to inspect.</param>
    /// <returns><c>true</c> if navigation is locked; otherwise <c>false</c>.</returns>
    public static bool IsNavigationLockedFor(DependencyObject element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (GetIsNavigationLocked(element))
        {
            return true;
        }

        Window? window = element as Window ?? Window.GetWindow(element);

        return window is not null && GetIsNavigationLocked(window);
    }
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Adds a navigation lock request to the specified window.
    /// </summary>
    /// <param name="window">The target window.</param>
    internal static void PushLock(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        int lockCount = GetNavigationLockCount(window) + 1;

        SetNavigationLockCount(window, lockCount);
        SetIsNavigationLocked(window, lockCount > 0);
        CoerceLockAwareElements(window);
    }

    /// <summary>
    /// Removes a navigation lock request from the specified window.
    /// </summary>
    /// <param name="window">The target window.</param>
    internal static void ReleaseLock(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        int lockCount = Math.Max(0, GetNavigationLockCount(window) - 1);

        SetNavigationLockCount(window, lockCount);
        SetIsNavigationLocked(window, lockCount > 0);
        CoerceLockAwareElements(window);
    }
    #endregion

    #region ### Private Methods ###
    private static void CoerceLockAwareElements(DependencyObject root)
    {
        if (root is UIElement element)
        {
            element.CoerceValue(UIElement.IsHitTestVisibleProperty);
            element.CoerceValue(UIElement.OpacityProperty);
        }

        int childCount;

        try
        {
            childCount = VisualTreeHelper.GetChildrenCount(root);
        }
        catch (InvalidOperationException)
        {
            return;
        }

        for (int index = 0; index < childCount; index++)
        {
            CoerceLockAwareElements(VisualTreeHelper.GetChild(root, index));
        }
    }

    private static int GetNavigationLockCount(DependencyObject element)
    {
        return (int)element.GetValue(NavigationLockCountProperty);
    }

    private static void SetNavigationLockCount(DependencyObject element, int value)
    {
        element.SetValue(NavigationLockCountProperty, value);
    }

    private static void OnNavigationLockStateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not UIElement element)
        {
            return;
        }

        element.CoerceValue(UIElement.IsHitTestVisibleProperty);
        element.CoerceValue(UIElement.OpacityProperty);
    }
    #endregion
}
#endregion
