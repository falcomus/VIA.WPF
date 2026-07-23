// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPropertyRow.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XPropertyRow ###
/// <summary>
/// Represents a labelled property value with optional description and actions.
/// </summary>
public class XPropertyRow : ContentControl
{
    #region ### Dependency Properties ###
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label), typeof(string), typeof(XPropertyRow), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(XPropertyRow), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(
        nameof(LabelWidth), typeof(GridLength), typeof(XPropertyRow), new FrameworkPropertyMetadata(new GridLength(160d)));

    public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
        nameof(Actions), typeof(object), typeof(XPropertyRow), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty ActionsTemplateProperty = DependencyProperty.Register(
        nameof(ActionsTemplate), typeof(DataTemplate), typeof(XPropertyRow), new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    static XPropertyRow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XPropertyRow), new FrameworkPropertyMetadata(typeof(XPropertyRow)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>Gets or sets the property label.</summary>
    public string Label
    {
        get => (string)this.GetValue(LabelProperty);
        set => this.SetValue(LabelProperty, value);
    }

    /// <summary>Gets or sets optional helper text.</summary>
    public string Description
    {
        get => (string)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>Gets or sets the width of the label column.</summary>
    public GridLength LabelWidth
    {
        get => (GridLength)this.GetValue(LabelWidthProperty);
        set => this.SetValue(LabelWidthProperty, value);
    }

    /// <summary>Gets or sets optional row actions.</summary>
    public object? Actions
    {
        get => this.GetValue(ActionsProperty);
        set => this.SetValue(ActionsProperty, value);
    }

    /// <summary>Gets or sets the row actions template.</summary>
    public DataTemplate? ActionsTemplate
    {
        get => (DataTemplate?)this.GetValue(ActionsTemplateProperty);
        set => this.SetValue(ActionsTemplateProperty, value);
    }
    #endregion
}
#endregion
