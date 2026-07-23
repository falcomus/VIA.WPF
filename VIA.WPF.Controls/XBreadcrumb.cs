// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBreadcrumb.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XBreadcrumb ###
/// <summary>
/// Represents a compact breadcrumb path.
/// </summary>
public class XBreadcrumb : ItemsControl
{
    #region ### Dependency Properties ###
    public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register(
        nameof(Separator), typeof(object), typeof(XBreadcrumb), new FrameworkPropertyMetadata("/"));
    #endregion

    #region ### Constructors ###
    static XBreadcrumb()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XBreadcrumb), new FrameworkPropertyMetadata(typeof(XBreadcrumb)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>Gets or sets the separator displayed between path items.</summary>
    public object? Separator
    {
        get => this.GetValue(SeparatorProperty);
        set => this.SetValue(SeparatorProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XBreadcrumbItem;
    }

    /// <inheritdoc />
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XBreadcrumbItem();
    }

    /// <inheritdoc />
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        this.Dispatcher.BeginInvoke(this.UpdateLastItemState);
    }
    #endregion

    #region ### Private Methods ###
    private void UpdateLastItemState()
    {
        for (int index = 0; index < this.Items.Count; index++)
        {
            if (this.ItemContainerGenerator.ContainerFromIndex(index) is XBreadcrumbItem item)
            {
                item.IsLast = index == this.Items.Count - 1;
            }
        }
    }
    #endregion
}
#endregion

#region ### Class XBreadcrumbItem ###
/// <summary>
/// Represents an item inside an <see cref="XBreadcrumb"/>.
/// </summary>
public class XBreadcrumbItem : ContentControl
{
    private static readonly DependencyPropertyKey IsLastPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsLast), typeof(bool), typeof(XBreadcrumbItem), new FrameworkPropertyMetadata(false));

    /// <summary>Identifies the read-only <see cref="IsLast"/> dependency property.</summary>
    public static readonly DependencyProperty IsLastProperty = IsLastPropertyKey.DependencyProperty;

    static XBreadcrumbItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XBreadcrumbItem), new FrameworkPropertyMetadata(typeof(XBreadcrumbItem)));
    }

    /// <summary>Gets a value indicating whether this is the final breadcrumb item.</summary>
    public bool IsLast
    {
        get => (bool)this.GetValue(IsLastProperty);
        internal set => this.SetValue(IsLastPropertyKey, value);
    }
}
#endregion
