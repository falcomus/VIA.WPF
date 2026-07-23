// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMetric.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XMetric ###
/// <summary>
/// Represents a neutral metric with optional supporting text and semantic emphasis.
/// </summary>
public class XMetric : Control
{
    #region ### Dependency Properties ###
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label), typeof(string), typeof(XMetric), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(XMetric), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty ValueTemplateProperty = DependencyProperty.Register(
        nameof(ValueTemplate), typeof(DataTemplate), typeof(XMetric), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty SupportingTextProperty = DependencyProperty.Register(
        nameof(SupportingText), typeof(string), typeof(XMetric), new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(object), typeof(XMetric), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant), typeof(XControlVariant), typeof(XMetric), new FrameworkPropertyMetadata(XControlVariant.Default));
    #endregion

    #region ### Constructors ###
    static XMetric()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XMetric), new FrameworkPropertyMetadata(typeof(XMetric)));
    }
    #endregion

    #region ### Public Properties ###
    public string Label
    {
        get => (string)this.GetValue(LabelProperty);
        set => this.SetValue(LabelProperty, value);
    }

    public object? Value
    {
        get => this.GetValue(ValueProperty);
        set => this.SetValue(ValueProperty, value);
    }

    public DataTemplate? ValueTemplate
    {
        get => (DataTemplate?)this.GetValue(ValueTemplateProperty);
        set => this.SetValue(ValueTemplateProperty, value);
    }

    public string SupportingText
    {
        get => (string)this.GetValue(SupportingTextProperty);
        set => this.SetValue(SupportingTextProperty, value);
    }

    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }
    #endregion
}
#endregion
