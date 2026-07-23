// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPage.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XPage ###
/// <summary>
/// Represents the standard page composition with optional header and footer slots.
/// </summary>
public class XPage : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(XPage),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(XPage),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Footer"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(XPage),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate),
        typeof(DataTemplate),
        typeof(XPage),
        new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XPage"/> class.
    /// </summary>
    static XPage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XPage), new FrameworkPropertyMetadata(typeof(XPage)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the page header content.
    /// </summary>
    public object? Header
    {
        get => this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the page header template.
    /// </summary>
    public DataTemplate? HeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderTemplateProperty);
        set => this.SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the page footer content.
    /// </summary>
    public object? Footer
    {
        get => this.GetValue(FooterProperty);
        set => this.SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the page footer template.
    /// </summary>
    public DataTemplate? FooterTemplate
    {
        get => (DataTemplate?)this.GetValue(FooterTemplateProperty);
        set => this.SetValue(FooterTemplateProperty, value);
    }
    #endregion
}
#endregion
