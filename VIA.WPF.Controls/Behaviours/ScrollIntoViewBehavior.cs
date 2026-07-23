// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollIntoViewBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace VIA.WPF.Behaviors;

#region ### Class ScrollIntoViewBehavior ###
/// <summary>
/// Provides an attached behavior that scrolls the selected item into view.
/// </summary>
public static class ScrollIntoViewBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ScrollIntoViewBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether the behavior is enabled for the specified items control.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the behavior is enabled; otherwise <c>false</c>.</returns>
    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// Sets whether the behavior is enabled for the specified items control.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnIsEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is Selector selector)
        {
            selector.SelectionChanged -= OnSelectorSelectionChanged;

            if (e.NewValue is true)
            {
                selector.SelectionChanged += OnSelectorSelectionChanged;
            }
        }
        else if (dependencyObject is TreeView treeView)
        {
            treeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;

            if (e.NewValue is true)
            {
                treeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            }
        }
    }

    private static void OnSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not Selector selector || selector.SelectedItem is null)
        {
            return;
        }

        selector.Dispatcher.BeginInvoke(
            () => ScrollSelectorItemIntoView(selector, selector.SelectedItem),
            DispatcherPriority.Loaded);
    }

    private static void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (sender is not TreeView treeView || e.NewValue is null)
        {
            return;
        }

        treeView.Dispatcher.BeginInvoke(
            () => ScrollTreeViewItemIntoView(treeView, e.NewValue),
            DispatcherPriority.Loaded);
    }

    private static void ScrollSelectorItemIntoView(Selector selector, object selectedItem)
    {
        switch (selector)
        {
            case DataGrid dataGrid:
                dataGrid.ScrollIntoView(selectedItem);
                break;

            case ListBox listBox:
                listBox.ScrollIntoView(selectedItem);
                break;
        }
    }

    private static void ScrollTreeViewItemIntoView(TreeView treeView, object selectedItem)
    {
        TreeViewItem? item = GetTreeViewItem(treeView, selectedItem);
        item?.BringIntoView();
    }

    private static TreeViewItem? GetTreeViewItem(ItemsControl parent, object item)
    {
        parent.UpdateLayout();

        TreeViewItem? directContainer = parent.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

        if (directContainer is not null)
        {
            return directContainer;
        }

        foreach (object childItem in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(childItem) is not TreeViewItem childContainer)
            {
                continue;
            }

            TreeViewItem? result = GetTreeViewItem(childContainer, item);

            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }
    #endregion
}
#endregion
