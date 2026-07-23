// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRadioGroupItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XRadioGroupItem ###
/// <summary>
/// Represents a single selectable radio item inside an <see cref="XRadioGroup" />.
/// </summary>
public class XRadioGroupItem : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="IsChecked" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
        nameof(IsChecked),
        typeof(bool?),
        typeof(XRadioGroupItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

    /// <summary>
    /// Identifies the <see cref="Description" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XRadioGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Value" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(object),
        typeof(XRadioGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Command" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(XRadioGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CommandParameter" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(XRadioGroupItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Variant" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XRadioGroupItem),
        new PropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XRadioGroupItem" /> class.
    /// </summary>
    static XRadioGroupItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XRadioGroupItem),
            new FrameworkPropertyMetadata(typeof(XRadioGroupItem)));
    }
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when the <see cref="IsChecked" /> value changes.
    /// </summary>
    public event RoutedEventHandler? IsCheckedChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the item is checked.
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
    /// Gets or sets the value used by <see cref="XRadioGroup.SelectedValue" />.
    /// When this value is not set, the item content is used as the selected value.
    /// </summary>
    public object? Value
    {
        get => this.GetValue(ValueProperty);
        set => this.SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the command invoked when the item is selected.
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
    /// Handles changes of the <see cref="IsChecked" /> dependency property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XRadioGroupItem item)
        {
            item.IsCheckedChanged?.Invoke(item, new RoutedEventArgs());
        }
    }
    #endregion
}
#endregion
