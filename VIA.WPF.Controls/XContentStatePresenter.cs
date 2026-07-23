// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XContentStatePresenter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XContentStatePresenter ###
/// <summary>
/// Presents content together with consistent loading, empty, error, and offline states.
/// </summary>
public class XContentStatePresenter : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="State"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        nameof(State),
        typeof(XContentState),
        typeof(XContentStatePresenter),
        new FrameworkPropertyMetadata(XContentState.Content));

    /// <summary>
    /// Identifies the <see cref="LoadingContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register(
        nameof(LoadingContent), typeof(object), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EmptyContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EmptyContentProperty = DependencyProperty.Register(
        nameof(EmptyContent), typeof(object), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ErrorContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ErrorContentProperty = DependencyProperty.Register(
        nameof(ErrorContent), typeof(object), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="OfflineContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OfflineContentProperty = DependencyProperty.Register(
        nameof(OfflineContent), typeof(object), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RetryCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RetryCommandProperty = DependencyProperty.Register(
        nameof(RetryCommand), typeof(ICommand), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RetryCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RetryCommandParameterProperty = DependencyProperty.Register(
        nameof(RetryCommandParameter), typeof(object), typeof(XContentStatePresenter), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RetryText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RetryTextProperty = DependencyProperty.Register(
        nameof(RetryText), typeof(string), typeof(XContentStatePresenter), new FrameworkPropertyMetadata("Retry"));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XContentStatePresenter"/> class.
    /// </summary>
    static XContentStatePresenter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XContentStatePresenter),
            new FrameworkPropertyMetadata(typeof(XContentStatePresenter)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>Gets or sets the active content state.</summary>
    public XContentState State
    {
        get => (XContentState)this.GetValue(StateProperty);
        set => this.SetValue(StateProperty, value);
    }

    /// <summary>Gets or sets custom loading content.</summary>
    public object? LoadingContent
    {
        get => this.GetValue(LoadingContentProperty);
        set => this.SetValue(LoadingContentProperty, value);
    }

    /// <summary>Gets or sets custom empty content.</summary>
    public object? EmptyContent
    {
        get => this.GetValue(EmptyContentProperty);
        set => this.SetValue(EmptyContentProperty, value);
    }

    /// <summary>Gets or sets custom error content.</summary>
    public object? ErrorContent
    {
        get => this.GetValue(ErrorContentProperty);
        set => this.SetValue(ErrorContentProperty, value);
    }

    /// <summary>Gets or sets custom offline content.</summary>
    public object? OfflineContent
    {
        get => this.GetValue(OfflineContentProperty);
        set => this.SetValue(OfflineContentProperty, value);
    }

    /// <summary>Gets or sets the command used to retry an operation.</summary>
    public ICommand? RetryCommand
    {
        get => (ICommand?)this.GetValue(RetryCommandProperty);
        set => this.SetValue(RetryCommandProperty, value);
    }

    /// <summary>Gets or sets the retry command parameter.</summary>
    public object? RetryCommandParameter
    {
        get => this.GetValue(RetryCommandParameterProperty);
        set => this.SetValue(RetryCommandParameterProperty, value);
    }

    /// <summary>Gets or sets the retry button label.</summary>
    public string RetryText
    {
        get => (string)this.GetValue(RetryTextProperty);
        set => this.SetValue(RetryTextProperty, value);
    }
    #endregion
}
#endregion
