// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XGroup ###
/// <summary>
/// Represents a composed content group with an optional title, subtitle, icon, actions, and footer.
/// </summary>
public class XGroup : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XGroup),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Subtitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(XGroup),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
        nameof(IconTemplate),
        typeof(DataTemplate),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Actions"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
        nameof(Actions),
        typeof(object),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ActionsTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionsTemplateProperty = DependencyProperty.Register(
        nameof(ActionsTemplate),
        typeof(DataTemplate),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Footer"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate),
        typeof(DataTemplate),
        typeof(XGroup),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XGroup),
        new FrameworkPropertyMetadata(new Thickness(16d, 12d, 16d, 12d)));

    /// <summary>
    /// Identifies the <see cref="ContentPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
        nameof(ContentPadding),
        typeof(Thickness),
        typeof(XGroup),
        new FrameworkPropertyMetadata(new Thickness(16d)));

    /// <summary>
    /// Identifies the <see cref="FooterPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterPaddingProperty = DependencyProperty.Register(
        nameof(FooterPadding),
        typeof(Thickness),
        typeof(XGroup),
        new FrameworkPropertyMetadata(new Thickness(16d, 10d, 16d, 10d)));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XGroup),
        new FrameworkPropertyMetadata(new CornerRadius(4d)));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XGroup"/> class.
    /// </summary>
    static XGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XGroup),
            new FrameworkPropertyMetadata(typeof(XGroup)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the group title.
    /// </summary>
    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional group subtitle.
    /// </summary>
    public string Subtitle
    {
        get => (string)this.GetValue(SubtitleProperty);
        set => this.SetValue(SubtitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional group icon.
    /// </summary>
    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display <see cref="Icon"/>.
    /// </summary>
    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets arbitrary commands or other content displayed in the header.
    /// </summary>
    public object? Actions
    {
        get => this.GetValue(ActionsProperty);
        set => this.SetValue(ActionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display <see cref="Actions"/>.
    /// </summary>
    public DataTemplate? ActionsTemplate
    {
        get => (DataTemplate?)this.GetValue(ActionsTemplateProperty);
        set => this.SetValue(ActionsTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional footer content.
    /// </summary>
    public object? Footer
    {
        get => this.GetValue(FooterProperty);
        set => this.SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display <see cref="Footer"/>.
    /// </summary>
    public DataTemplate? FooterTemplate
    {
        get => (DataTemplate?)this.GetValue(FooterTemplateProperty);
        set => this.SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the header padding.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)this.GetValue(HeaderPaddingProperty);
        set => this.SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the content padding.
    /// </summary>
    public Thickness ContentPadding
    {
        get => (Thickness)this.GetValue(ContentPaddingProperty);
        set => this.SetValue(ContentPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer padding.
    /// </summary>
    public Thickness FooterPadding
    {
        get => (Thickness)this.GetValue(FooterPaddingProperty);
        set => this.SetValue(FooterPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the group corner radius.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }
    #endregion
}
#endregion
