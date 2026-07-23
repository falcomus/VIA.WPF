// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XContextMenu.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VIA.WPF.Controls;

#region ### Class XContextMenu ###
/// <summary>
/// Represents a themed context menu that exposes information about its placement target.
/// </summary>
public class XContextMenu : ContextMenu
{
    #region ### Dependency Properties ###
    private static readonly DependencyPropertyKey TargetDataContextPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(TargetDataContext),
            typeof(object),
            typeof(XContextMenu),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="TargetDataContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TargetDataContextProperty =
        TargetDataContextPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey TargetItemPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(TargetItem),
            typeof(object),
            typeof(XContextMenu),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="TargetItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TargetItemProperty =
        TargetItemPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey TargetTagPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(TargetTag),
            typeof(object),
            typeof(XContextMenu),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="TargetTag"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TargetTagProperty =
        TargetTagPropertyKey.DependencyProperty;
    #endregion

    #region ### Constructors ###
    static XContextMenu()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XContextMenu),
            new FrameworkPropertyMetadata(typeof(XContextMenu)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XContextMenu"/> class.
    /// </summary>
    public XContextMenu()
    {
        this.Opened += this.OnMenuOpened;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the data context of the current placement target.
    /// </summary>
    public object? TargetDataContext
    {
        get => this.GetValue(TargetDataContextProperty);
        private set => this.SetValue(TargetDataContextPropertyKey, value);
    }

    /// <summary>
    /// Gets the selected item of a selector placement target.
    /// For other placement targets, the target data context is returned.
    /// </summary>
    public object? TargetItem
    {
        get => this.GetValue(TargetItemProperty);
        private set => this.SetValue(TargetItemPropertyKey, value);
    }

    /// <summary>
    /// Gets the tag value of the current placement target.
    /// </summary>
    public object? TargetTag
    {
        get => this.GetValue(TargetTagProperty);
        private set => this.SetValue(TargetTagPropertyKey, value);
    }
    #endregion

    #region ### Private Methods ###
    private void OnMenuOpened(object sender, RoutedEventArgs e)
    {
        this.UpdateTargetContext();
    }

    private void UpdateTargetContext()
    {
        object? targetDataContext = null;
        object? targetItem = null;
        object? targetTag = null;

        if (this.PlacementTarget is FrameworkElement frameworkElement)
        {
            targetDataContext = frameworkElement.DataContext;
            targetTag = frameworkElement.Tag;
        }

        if (this.PlacementTarget is Selector selector)
        {
            targetItem = selector.SelectedItem;
        }
        else
        {
            targetItem = targetDataContext;
        }

        this.TargetDataContext = targetDataContext;
        this.TargetItem = targetItem;
        this.TargetTag = targetTag;
    }
    #endregion
}
#endregion