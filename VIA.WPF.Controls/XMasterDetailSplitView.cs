// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMasterDetailSplitView.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XMasterDetailSplitView ###

/// <summary>
/// Represents a reusable master-detail split view with built-in master and detail headers.
/// </summary>
[TemplatePart(Name = PartCloseButton, Type = typeof(Button))]
public class XMasterDetailSplitView : XSplitView
{
    #region ### Constants ###

    private const string PartCloseButton = "PART_CloseButton";

    #endregion

    #region ### Dependency Properties ###

    /// <summary>
    /// Identifies the <see cref="MasterTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MasterTitleProperty = DependencyProperty.Register(
        nameof(MasterTitle),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MasterTitleTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MasterTitleTemplateProperty = DependencyProperty.Register(
        nameof(MasterTitleTemplate),
        typeof(DataTemplate),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MasterHeaderContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MasterHeaderContentProperty = DependencyProperty.Register(
        nameof(MasterHeaderContent),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MasterHeaderContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MasterHeaderContentTemplateProperty = DependencyProperty.Register(
        nameof(MasterHeaderContentTemplate),
        typeof(DataTemplate),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailTitleProperty = DependencyProperty.Register(
        nameof(DetailTitle),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailTitleTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailTitleTemplateProperty = DependencyProperty.Register(
        nameof(DetailTitleTemplate),
        typeof(DataTemplate),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailHeaderContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHeaderContentProperty = DependencyProperty.Register(
        nameof(DetailHeaderContent),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailHeaderContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHeaderContentTemplateProperty = DependencyProperty.Register(
        nameof(DetailHeaderContentTemplate),
        typeof(DataTemplate),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsDetailPaneOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDetailPaneOpenProperty = DependencyProperty.Register(
        nameof(IsDetailPaneOpen),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(
            false,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnIsDetailPaneOpenChanged));

    /// <summary>
    /// Identifies the <see cref="IsDetailPanePinned"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDetailPanePinnedProperty = DependencyProperty.Register(
        nameof(IsDetailPanePinned),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(
            false,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="ShowMasterHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowMasterHeaderProperty = DependencyProperty.Register(
        nameof(ShowMasterHeader),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowDetailHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDetailHeaderProperty = DependencyProperty.Register(
        nameof(ShowDetailHeader),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="UsePaneChrome"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UsePaneChromeProperty = DependencyProperty.Register(
        nameof(UsePaneChrome),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="NewButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewButtonTextProperty = DependencyProperty.Register(
        nameof(NewButtonText),
        typeof(string),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata("New"));

    /// <summary>
    /// Identifies the <see cref="NewCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(
        nameof(NewCommand),
        typeof(ICommand),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="NewCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandParameterProperty = DependencyProperty.Register(
        nameof(NewCommandParameter),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowDetailButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDetailButtonProperty = DependencyProperty.Register(
        nameof(ShowDetailButton),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="DetailButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailButtonTextProperty = DependencyProperty.Register(
        nameof(DetailButtonText),
        typeof(string),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata("Details"));

    /// <summary>
    /// Identifies the <see cref="DetailCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailCommandProperty = DependencyProperty.Register(
        nameof(DetailCommand),
        typeof(ICommand),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailCommandParameterProperty = DependencyProperty.Register(
        nameof(DetailCommandParameter),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="EditButtonText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditButtonTextProperty = DependencyProperty.Register(
        nameof(EditButtonText),
        typeof(string),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata("Edit"));

    /// <summary>
    /// Identifies the <see cref="EditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
        nameof(EditCommand),
        typeof(ICommand),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandParameterProperty = DependencyProperty.Register(
        nameof(EditCommandParameter),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowCloseButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
        nameof(ShowCloseButton),
        typeof(bool),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="CloseDetailPaneCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseDetailPaneCommandProperty = DependencyProperty.Register(
        nameof(CloseDetailPaneCommand),
        typeof(ICommand),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CloseDetailPaneCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseDetailPaneCommandParameterProperty = DependencyProperty.Register(
        nameof(CloseDetailPaneCommandParameter),
        typeof(object),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MasterContentPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MasterContentPaddingProperty = DependencyProperty.Register(
        nameof(MasterContentPadding),
        typeof(Thickness),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(new Thickness(0d)));

    /// <summary>
    /// Identifies the <see cref="DetailContentPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailContentPaddingProperty = DependencyProperty.Register(
        nameof(DetailContentPadding),
        typeof(Thickness),
        typeof(XMasterDetailSplitView),
        new FrameworkPropertyMetadata(new Thickness(0d)));

    #endregion

    #region ### Fields ###

    private Button? closeButton;

    #endregion

    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XMasterDetailSplitView"/> class.
    /// </summary>
    static XMasterDetailSplitView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XMasterDetailSplitView),
            new FrameworkPropertyMetadata(typeof(XMasterDetailSplitView)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XMasterDetailSplitView"/> class.
    /// </summary>
    public XMasterDetailSplitView()
    {
        this.SetCurrentValue(XSplitView.IsSecondPaneVisibleProperty, this.IsDetailPaneOpen);
    }

    #endregion

    #region ### Public Properties ###

    /// <summary>
    /// Gets or sets the title displayed in the master pane header.
    /// </summary>
    public object? MasterTitle
    {
        get => this.GetValue(MasterTitleProperty);
        set => this.SetValue(MasterTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template displayed in the master pane header.
    /// </summary>
    public DataTemplate? MasterTitleTemplate
    {
        get => (DataTemplate?)this.GetValue(MasterTitleTemplateProperty);
        set => this.SetValue(MasterTitleTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets additional header content displayed in the master pane header.
    /// </summary>
    public object? MasterHeaderContent
    {
        get => this.GetValue(MasterHeaderContentProperty);
        set => this.SetValue(MasterHeaderContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for <see cref="MasterHeaderContent"/>.
    /// </summary>
    public DataTemplate? MasterHeaderContentTemplate
    {
        get => (DataTemplate?)this.GetValue(MasterHeaderContentTemplateProperty);
        set => this.SetValue(MasterHeaderContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the title displayed in the detail pane header.
    /// </summary>
    public object? DetailTitle
    {
        get => this.GetValue(DetailTitleProperty);
        set => this.SetValue(DetailTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template displayed in the detail pane header.
    /// </summary>
    public DataTemplate? DetailTitleTemplate
    {
        get => (DataTemplate?)this.GetValue(DetailTitleTemplateProperty);
        set => this.SetValue(DetailTitleTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets additional header content displayed in the detail pane header.
    /// </summary>
    public object? DetailHeaderContent
    {
        get => this.GetValue(DetailHeaderContentProperty);
        set => this.SetValue(DetailHeaderContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for <see cref="DetailHeaderContent"/>.
    /// </summary>
    public DataTemplate? DetailHeaderContentTemplate
    {
        get => (DataTemplate?)this.GetValue(DetailHeaderContentTemplateProperty);
        set => this.SetValue(DetailHeaderContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail pane is open.
    /// </summary>
    public bool IsDetailPaneOpen
    {
        get => (bool)this.GetValue(IsDetailPaneOpenProperty);
        set => this.SetValue(IsDetailPaneOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail pane is pinned.
    /// </summary>
    public bool IsDetailPanePinned
    {
        get => (bool)this.GetValue(IsDetailPanePinnedProperty);
        set => this.SetValue(IsDetailPanePinnedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the master pane header is visible.
    /// </summary>
    public bool ShowMasterHeader
    {
        get => (bool)this.GetValue(ShowMasterHeaderProperty);
        set => this.SetValue(ShowMasterHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail pane header is visible.
    /// </summary>
    public bool ShowDetailHeader
    {
        get => (bool)this.GetValue(ShowDetailHeaderProperty);
        set => this.SetValue(ShowDetailHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the master and detail panes render their own surface, border and rounded corners.
    /// </summary>
    public bool UsePaneChrome
    {
        get => (bool)this.GetValue(UsePaneChromeProperty);
        set => this.SetValue(UsePaneChromeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the new button is visible.
    /// </summary>
    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the new button text.
    /// </summary>
    public string NewButtonText
    {
        get => (string)this.GetValue(NewButtonTextProperty);
        set => this.SetValue(NewButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the new button.
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
    /// Gets or sets a value indicating whether the detail button is visible.
    /// </summary>
    public bool ShowDetailButton
    {
        get => (bool)this.GetValue(ShowDetailButtonProperty);
        set => this.SetValue(ShowDetailButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the detail button text.
    /// </summary>
    public string DetailButtonText
    {
        get => (string)this.GetValue(DetailButtonTextProperty);
        set => this.SetValue(DetailButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the detail button.
    /// </summary>
    public ICommand? DetailCommand
    {
        get => (ICommand?)this.GetValue(DetailCommandProperty);
        set => this.SetValue(DetailCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by <see cref="DetailCommand"/>.
    /// </summary>
    public object? DetailCommandParameter
    {
        get => this.GetValue(DetailCommandParameterProperty);
        set => this.SetValue(DetailCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit button is visible.
    /// </summary>
    public bool ShowEditButton
    {
        get => (bool)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the edit button text.
    /// </summary>
    public string EditButtonText
    {
        get => (string)this.GetValue(EditButtonTextProperty);
        set => this.SetValue(EditButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the edit button.
    /// </summary>
    public ICommand? EditCommand
    {
        get => (ICommand?)this.GetValue(EditCommandProperty);
        set => this.SetValue(EditCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by <see cref="EditCommand"/>.
    /// </summary>
    public object? EditCommandParameter
    {
        get => this.GetValue(EditCommandParameterProperty);
        set => this.SetValue(EditCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the close button is visible.
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)this.GetValue(ShowCloseButtonProperty);
        set => this.SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the detail pane is closed.
    /// </summary>
    public ICommand? CloseDetailPaneCommand
    {
        get => (ICommand?)this.GetValue(CloseDetailPaneCommandProperty);
        set => this.SetValue(CloseDetailPaneCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by <see cref="CloseDetailPaneCommand"/>.
    /// </summary>
    public object? CloseDetailPaneCommandParameter
    {
        get => this.GetValue(CloseDetailPaneCommandParameterProperty);
        set => this.SetValue(CloseDetailPaneCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to the master pane content.
    /// </summary>
    public Thickness MasterContentPadding
    {
        get => (Thickness)this.GetValue(MasterContentPaddingProperty);
        set => this.SetValue(MasterContentPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to the detail pane content.
    /// </summary>
    public Thickness DetailContentPadding
    {
        get => (Thickness)this.GetValue(DetailContentPaddingProperty);
        set => this.SetValue(DetailContentPaddingProperty, value);
    }

    #endregion

    #region ### Public Methods ###

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        this.DetachTemplateEvents();

        base.OnApplyTemplate();

        this.closeButton = this.GetTemplateChild(PartCloseButton) as Button;

        this.AttachTemplateEvents();
    }

    #endregion

    #region ### Private Methods ###

    /// <summary>
    /// Attaches handlers to template parts.
    /// </summary>
    private void AttachTemplateEvents()
    {
        if (this.closeButton is not null)
        {
            this.closeButton.Click += this.OnCloseButtonClick;
        }
    }

    /// <summary>
    /// Detaches handlers from template parts.
    /// </summary>
    private void DetachTemplateEvents()
    {
        if (this.closeButton is not null)
        {
            this.closeButton.Click -= this.OnCloseButtonClick;
        }

        this.closeButton = null;
    }

    /// <summary>
    /// Handles the close button click.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event arguments.</param>
    private void OnCloseButtonClick(object sender, RoutedEventArgs eventArgs)
    {
        this.SetCurrentValue(IsDetailPanePinnedProperty, false);
        this.SetCurrentValue(IsDetailPaneOpenProperty, false);
        this.SetCurrentValue(XSplitView.IsSecondPaneVisibleProperty, false);

        this.ExecuteCommand(
            this.CloseDetailPaneCommand,
            this.CloseDetailPaneCommandParameter ?? this.DataContext);
    }

    /// <summary>
    /// Executes the specified command when possible.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parameter">The command parameter.</param>
    private void ExecuteCommand(ICommand? command, object? parameter)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command.Execute(parameter);
        }
    }

    /// <summary>
    /// Handles changes of <see cref="IsDetailPaneOpen"/>.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="eventArgs">The event arguments.</param>
    private static void OnIsDetailPaneOpenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XMasterDetailSplitView splitView)
        {
            splitView.SetCurrentValue(XSplitView.IsSecondPaneVisibleProperty, eventArgs.NewValue is true);
        }
    }

    #endregion
}

#endregion
