// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckGroupItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XCheckGroupItem ###
/// <summary>
/// Represents a single selectable checklist item inside an <see cref="XCheckGroup"/>.
/// </summary>
public class XCheckGroupItem : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="IsChecked"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
        nameof(IsChecked),
        typeof(bool?),
        typeof(XCheckGroupItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XCheckGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Command"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(XCheckGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(XCheckGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XCheckGroupItem),
        new PropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XCheckGroupItem"/> class.
    /// </summary>
    static XCheckGroupItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XCheckGroupItem),
            new FrameworkPropertyMetadata(typeof(XCheckGroupItem)));
    }
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when the <see cref="IsChecked"/> value changes.
    /// </summary>
    public event RoutedEventHandler? IsCheckedChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the checklist item is checked.
    /// </summary>
    public bool? IsChecked
    {
        get => (bool?)this.GetValue(IsCheckedProperty);
        set => this.SetValue(IsCheckedProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional secondary description text.
    /// </summary>
    public string? Description
    {
        get => (string?)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the command invoked when the item check state changes through the inner checkbox.
    /// </summary>
    public ICommand? Command
    {
        get => (ICommand?)this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional command parameter.
    /// </summary>
    public object? CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => this.SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic variant used for subtle selection styling.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles changes of the <see cref="IsChecked"/> dependency property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XCheckGroupItem item)
        {
            item.IsCheckedChanged?.Invoke(item, new RoutedEventArgs());
        }
    }
    #endregion
}
#endregion
