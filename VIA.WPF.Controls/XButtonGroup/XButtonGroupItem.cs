// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButtonGroupItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XButtonGroupItem ###
/// <summary>
/// Represents a single selectable segment inside an <see cref="XButtonGroup"/>.
/// </summary>
public class XButtonGroupItem : XButton
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Value"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(object),
        typeof(XButtonGroupItem),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsSelected"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected),
        typeof(bool),
        typeof(XButtonGroupItem),
        new FrameworkPropertyMetadata(false));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XButtonGroupItem"/> class.
    /// </summary>
    static XButtonGroupItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XButtonGroupItem),
            new FrameworkPropertyMetadata(typeof(XButtonGroupItem)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the value used by <see cref="XButtonGroup.SelectedValue"/> when this item is selected.
    /// </summary>
    public object? Value
    {
        get => this.GetValue(ValueProperty);
        set => this.SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item is currently selected.
    /// </summary>
    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }
    #endregion
}
#endregion
