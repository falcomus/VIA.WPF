// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataSection.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XDataSection ###

/// <summary>
/// Represents a reusable data section with a compact header, optional toolbar content and a primary new command.
/// </summary>
[Obsolete("Use XGroup with XHeaderGroup actions and XContentStatePresenter instead.")]
public class XDataSection : ContentControl
{
    #region ### Dependency Properties ###

    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ShowTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle),
        typeof(bool),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="HeaderContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
        nameof(HeaderContent),
        typeof(object),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentTemplateProperty = DependencyProperty.Register(
        nameof(HeaderContentTemplate),
        typeof(DataTemplate),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ToolbarContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToolbarContentProperty = DependencyProperty.Register(
        nameof(ToolbarContent),
        typeof(object),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ToolbarContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToolbarContentTemplateProperty = DependencyProperty.Register(
        nameof(ToolbarContentTemplate),
        typeof(DataTemplate),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="NewButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewButtonTextProperty = DependencyProperty.Register(
        nameof(NewButtonText),
        typeof(string),
        typeof(XDataSection),
        new FrameworkPropertyMetadata("New"));

    /// <summary>
    /// Identifies the <see cref="NewCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(
        nameof(NewCommand),
        typeof(ICommand),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="NewCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandParameterProperty = DependencyProperty.Register(
        nameof(NewCommandParameter),
        typeof(object),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderOverlayMargin"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderOverlayMarginProperty = DependencyProperty.Register(
        nameof(HeaderOverlayMargin),
        typeof(Thickness),
        typeof(XDataSection),
        new FrameworkPropertyMetadata(new Thickness(0d, -35d, 0d, 0d)));

    #endregion

    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XDataSection"/> class.
    /// </summary>
    static XDataSection()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XDataSection),
            new FrameworkPropertyMetadata(typeof(XDataSection)));
    }

    #endregion

    #region ### Public Properties ###

    /// <summary>
    /// Gets or sets the section title.
    /// </summary>
    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the title is visible.
    /// </summary>
    public bool ShowTitle
    {
        get => (bool)this.GetValue(ShowTitleProperty);
        set => this.SetValue(ShowTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets optional content displayed next to the title.
    /// </summary>
    public object? HeaderContent
    {
        get => this.GetValue(HeaderContentProperty);
        set => this.SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for <see cref="HeaderContent"/>.
    /// </summary>
    public DataTemplate? HeaderContentTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderContentTemplateProperty);
        set => this.SetValue(HeaderContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets additional toolbar content displayed before the built-in action buttons.
    /// </summary>
    public object? ToolbarContent
    {
        get => this.GetValue(ToolbarContentProperty);
        set => this.SetValue(ToolbarContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for <see cref="ToolbarContent"/>.
    /// </summary>
    public DataTemplate? ToolbarContentTemplate
    {
        get => (DataTemplate?)this.GetValue(ToolbarContentTemplateProperty);
        set => this.SetValue(ToolbarContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the built-in new button is visible.
    /// </summary>
    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the built-in new button text.
    /// </summary>
    public string NewButtonText
    {
        get => (string)this.GetValue(NewButtonTextProperty);
        set => this.SetValue(NewButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the built-in new button.
    /// </summary>
    public ICommand? NewCommand
    {
        get => (ICommand?)this.GetValue(NewCommandProperty);
        set => this.SetValue(NewCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by <see cref="NewCommand"/>.
    /// </summary>
    public object? NewCommandParameter
    {
        get => this.GetValue(NewCommandParameterProperty);
        set => this.SetValue(NewCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin used for the overlay header/action area.
    /// </summary>
    public Thickness HeaderOverlayMargin
    {
        get => (Thickness)this.GetValue(HeaderOverlayMarginProperty);
        set => this.SetValue(HeaderOverlayMarginProperty, value);
    }

    #endregion
}

#endregion
