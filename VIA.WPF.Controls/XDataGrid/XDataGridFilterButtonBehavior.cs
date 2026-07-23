// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridFilterButtonBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XDataGridFilterButtonBehavior ###
/// <summary>
/// Connects a filter button inside a <see cref="DataGridColumnHeader"/> to the owning <see cref="XDataGrid"/>.
/// </summary>
public static class XDataGridFilterButtonBehavior
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the Attach attached property.
    /// </summary>
    public static readonly DependencyProperty AttachProperty =
        DependencyProperty.RegisterAttached(
            "Attach",
            typeof(bool),
            typeof(XDataGridFilterButtonBehavior),
            new PropertyMetadata(false, OnAttachChanged));

    /// <summary>
    /// Identifies the ColumnHeader attached property.
    /// </summary>
    public static readonly DependencyProperty ColumnHeaderProperty =
        DependencyProperty.RegisterAttached(
            "ColumnHeader",
            typeof(DataGridColumnHeader),
            typeof(XDataGridFilterButtonBehavior),
            new PropertyMetadata(null));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Sets whether the behavior is attached.
    /// </summary>
    public static void SetAttach(DependencyObject element, bool value)
    {
        element.SetValue(AttachProperty, value);
    }

    /// <summary>
    /// Gets whether the behavior is attached.
    /// </summary>
    public static bool GetAttach(DependencyObject element)
    {
        return (bool)element.GetValue(AttachProperty);
    }

    /// <summary>
    /// Sets the owning column header.
    /// </summary>
    public static void SetColumnHeader(DependencyObject element, DataGridColumnHeader? value)
    {
        element.SetValue(ColumnHeaderProperty, value);
    }

    /// <summary>
    /// Gets the owning column header.
    /// </summary>
    public static DataGridColumnHeader? GetColumnHeader(DependencyObject element)
    {
        return (DataGridColumnHeader?)element.GetValue(ColumnHeaderProperty);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Button button)
        {
            return;
        }

        button.Click -= OnButtonClick;
        button.Loaded -= OnButtonLoaded;
        button.PreviewKeyDown -= OnButtonPreviewKeyDown;

        if (e.NewValue is true)
        {
            button.Click += OnButtonClick;
            button.Loaded += OnButtonLoaded;
            button.PreviewKeyDown += OnButtonPreviewKeyDown;
        }
    }

    private static void OnButtonLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        DataGridColumnHeader? header = GetColumnHeader(button);
        if (header?.Column is null)
        {
            return;
        }

        XDataGrid? grid = FindVisualParent<XDataGrid>(header);
        grid?.PrepareColumnHeaderFilterButton(header.Column, button);
    }

    private static void OnButtonPreviewKeyDown(object sender, KeyEventArgs e)
    {
        Key key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key != Key.Space && key != Key.Return)
        {
            return;
        }

        if (sender is not Button button)
        {
            return;
        }

        if (TryOpenFilter(button))
        {
            e.Handled = true;
        }
    }

    private static void OnButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (TryOpenFilter(button))
        {
            e.Handled = true;
        }
    }

    private static bool TryOpenFilter(Button button)
    {
        DataGridColumnHeader? header = GetColumnHeader(button) ?? FindVisualParent<DataGridColumnHeader>(button);
        if (header?.Column is null)
        {
            return false;
        }

        XDataGrid? grid = FindVisualParent<XDataGrid>(header) ?? button.Tag as XDataGrid;
        if (grid is null)
        {
            return false;
        }

        header.ApplyTemplate();
        Popup? popup = header.Template.FindName("PART_FilterPopup", header) as Popup;

        grid.HandleColumnHeaderFilterButtonClick(header.Column, popup, button);
        return true;
    }

    private static T? FindVisualParent<T>(DependencyObject? child)
        where T : DependencyObject
    {
        while (child is not null)
        {
            if (child is T result)
            {
                return result;
            }

            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }
    #endregion
}
#endregion
