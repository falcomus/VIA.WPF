// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToolbarPresenter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Controls;

#region ### Class XToolbarPresenter ###
/// <summary>
/// Presents a reusable page toolbar based on an <see cref="XToolbarContext"/> and an optional search context.
/// </summary>
[Obsolete("Use XHeaderBar with XHeaderGroup actions instead.")]
public class XToolbarPresenter : Control
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Toolbar"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToolbarProperty = DependencyProperty.Register(
        nameof(Toolbar),
        typeof(XToolbarContext),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(null, OnToolbarChanged));

    /// <summary>
    /// Identifies the <see cref="SearchContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchContextProperty = DependencyProperty.Register(
        nameof(SearchContext),
        typeof(object),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(null, OnSearchContextChanged));

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveSearchContext"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveSearchContextPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveSearchContext),
        typeof(IXSearchContext),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EffectiveSearchContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveSearchContextProperty = EffectiveSearchContextPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasSearchContext"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasSearchContextPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasSearchContext),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasSearchContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSearchContextProperty = HasSearchContextPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="ShowToolbar"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowToolbarProperty = DependencyProperty.Register(
        nameof(ShowToolbar),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowSearchBox"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSearchBoxProperty = DependencyProperty.Register(
        nameof(ShowSearchBox),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowSearchSeparator"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSearchSeparatorProperty = DependencyProperty.Register(
        nameof(ShowSearchSeparator),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowViewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowViewButtonProperty = DependencyProperty.Register(
        nameof(ShowViewButton),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowCommandSeparator"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCommandSeparatorProperty = DependencyProperty.Register(
        nameof(ShowCommandSeparator),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowViewModeSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowViewModeSelectorProperty = DependencyProperty.Register(
        nameof(ShowViewModeSelector),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the <see cref="ShowRememberViewToggle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowRememberViewToggleProperty = DependencyProperty.Register(
        nameof(ShowRememberViewToggle),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(true, OnVisibilityInputChanged));

    /// <summary>
    /// Identifies the read-only <see cref="IsSearchBoxVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsSearchBoxVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsSearchBoxVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsSearchBoxVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSearchBoxVisibleProperty = IsSearchBoxVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsSearchSeparatorVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsSearchSeparatorVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsSearchSeparatorVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsSearchSeparatorVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSearchSeparatorVisibleProperty = IsSearchSeparatorVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsNewButtonVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsNewButtonVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsNewButtonVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsNewButtonVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsNewButtonVisibleProperty = IsNewButtonVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsViewButtonVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsViewButtonVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsViewButtonVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsViewButtonVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsViewButtonVisibleProperty = IsViewButtonVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsEditButtonVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsEditButtonVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsEditButtonVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsEditButtonVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEditButtonVisibleProperty = IsEditButtonVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsDeleteButtonVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsDeleteButtonVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsDeleteButtonVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsDeleteButtonVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDeleteButtonVisibleProperty = IsDeleteButtonVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsCommandSeparatorVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsCommandSeparatorVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsCommandSeparatorVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsCommandSeparatorVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCommandSeparatorVisibleProperty = IsCommandSeparatorVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsViewModeSelectorVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsViewModeSelectorVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsViewModeSelectorVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsViewModeSelectorVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsViewModeSelectorVisibleProperty = IsViewModeSelectorVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsRememberViewToggleVisible"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsRememberViewToggleVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsRememberViewToggleVisible),
        typeof(bool),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsRememberViewToggleVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsRememberViewToggleVisibleProperty = IsRememberViewToggleVisiblePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="SearchPlaceholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchPlaceholderProperty = DependencyProperty.Register(
        nameof(SearchPlaceholder),
        typeof(string),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata("Globale Suche"));

    /// <summary>
    /// Identifies the <see cref="ToolbarItemHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToolbarItemHeightProperty = DependencyProperty.Register(
        nameof(ToolbarItemHeight),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(32d, OnDimensionInputChanged));

    /// <summary>
    /// Identifies the <see cref="SearchBoxWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchBoxWidthProperty = DependencyProperty.Register(
        nameof(SearchBoxWidth),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(140d));

    /// <summary>
    /// Identifies the <see cref="SearchBoxHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchBoxHeightProperty = DependencyProperty.Register(
        nameof(SearchBoxHeight),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(double.NaN, OnDimensionInputChanged));

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveSearchBoxHeight"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveSearchBoxHeightPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveSearchBoxHeight),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(30d));

    /// <summary>
    /// Identifies the <see cref="EffectiveSearchBoxHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveSearchBoxHeightProperty = EffectiveSearchBoxHeightPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="SearchBoxPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchBoxPaddingProperty = DependencyProperty.Register(
        nameof(SearchBoxPadding),
        typeof(Thickness),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(new Thickness(3d, 1d, 2d, 1d)));

    /// <summary>
    /// Identifies the <see cref="SearchLeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchLeadingIconSizeProperty = DependencyProperty.Register(
        nameof(SearchLeadingIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(13d));

    /// <summary>
    /// Identifies the <see cref="SearchTrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchTrailingIconSizeProperty = DependencyProperty.Register(
        nameof(SearchTrailingIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(12d));

    /// <summary>
    /// Identifies the <see cref="Spacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(5d));

    /// <summary>
    /// Identifies the <see cref="ButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ButtonIconSizeProperty = DependencyProperty.Register(
        nameof(ButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(16d, OnIconSizeInputChanged));

    /// <summary>
    /// Identifies the <see cref="NewButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewButtonIconSizeProperty = DependencyProperty.Register(
        nameof(NewButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(14d, OnIconSizeInputChanged));

    /// <summary>
    /// Identifies the <see cref="ViewButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewButtonIconSizeProperty = DependencyProperty.Register(
        nameof(ViewButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(18d, OnIconSizeInputChanged));

    /// <summary>
    /// Identifies the <see cref="EditButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditButtonIconSizeProperty = DependencyProperty.Register(
        nameof(EditButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(double.NaN, OnIconSizeInputChanged));

    /// <summary>
    /// Identifies the <see cref="DeleteButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteButtonIconSizeProperty = DependencyProperty.Register(
        nameof(DeleteButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(double.NaN, OnIconSizeInputChanged));

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveNewButtonIconSize"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveNewButtonIconSizePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveNewButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(14d));

    /// <summary>
    /// Identifies the <see cref="EffectiveNewButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveNewButtonIconSizeProperty = EffectiveNewButtonIconSizePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveViewButtonIconSize"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveViewButtonIconSizePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveViewButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(12d));

    /// <summary>
    /// Identifies the <see cref="EffectiveViewButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveViewButtonIconSizeProperty = EffectiveViewButtonIconSizePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveEditButtonIconSize"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveEditButtonIconSizePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveEditButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(16d));

    /// <summary>
    /// Identifies the <see cref="EffectiveEditButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveEditButtonIconSizeProperty = EffectiveEditButtonIconSizePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveDeleteButtonIconSize"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveDeleteButtonIconSizePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveDeleteButtonIconSize),
        typeof(double),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(15d));

    /// <summary>
    /// Identifies the <see cref="EffectiveDeleteButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveDeleteButtonIconSizeProperty = EffectiveDeleteButtonIconSizePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="NewButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewButtonContentProperty = DependencyProperty.Register(
        nameof(NewButtonContent),
        typeof(object),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ViewButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewButtonContentProperty = DependencyProperty.Register(
        nameof(ViewButtonContent),
        typeof(object),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(""));

    /// <summary>
    /// Identifies the <see cref="EditButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditButtonContentProperty = DependencyProperty.Register(
        nameof(EditButtonContent),
        typeof(object),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="DeleteButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteButtonContentProperty = DependencyProperty.Register(
        nameof(DeleteButtonContent),
        typeof(object),
        typeof(XToolbarPresenter),
        new FrameworkPropertyMetadata(""));
    #endregion

    #region ### Private Fields ###
    private INotifyPropertyChanged? toolbarPropertyChangedSource;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToolbarPresenter"/> class.
    /// </summary>
    static XToolbarPresenter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XToolbarPresenter),
            new FrameworkPropertyMetadata(typeof(XToolbarPresenter)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XToolbarPresenter"/> class.
    /// </summary>
    public XToolbarPresenter()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
        ButtonIconSize = 17;
        this.RefreshDimensionStates();
    }
    #endregion

    #region ### Public Properties ###
    public XToolbarContext? Toolbar
    {
        get => (XToolbarContext?)this.GetValue(ToolbarProperty);
        set => this.SetValue(ToolbarProperty, value);
    }

    public object? SearchContext
    {
        get => this.GetValue(SearchContextProperty);
        set => this.SetValue(SearchContextProperty, value);
    }

    public IXSearchContext? EffectiveSearchContext => (IXSearchContext?)this.GetValue(EffectiveSearchContextProperty);

    public bool HasSearchContext => (bool)this.GetValue(HasSearchContextProperty);

    public bool ShowToolbar
    {
        get => (bool)this.GetValue(ShowToolbarProperty);
        set => this.SetValue(ShowToolbarProperty, value);
    }

    public bool ShowSearchBox
    {
        get => (bool)this.GetValue(ShowSearchBoxProperty);
        set => this.SetValue(ShowSearchBoxProperty, value);
    }

    public bool ShowSearchSeparator
    {
        get => (bool)this.GetValue(ShowSearchSeparatorProperty);
        set => this.SetValue(ShowSearchSeparatorProperty, value);
    }

    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    public bool ShowViewButton
    {
        get => (bool)this.GetValue(ShowViewButtonProperty);
        set => this.SetValue(ShowViewButtonProperty, value);
    }

    public bool ShowEditButton
    {
        get => (bool)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    public bool ShowDeleteButton
    {
        get => (bool)this.GetValue(ShowDeleteButtonProperty);
        set => this.SetValue(ShowDeleteButtonProperty, value);
    }

    public bool ShowCommandSeparator
    {
        get => (bool)this.GetValue(ShowCommandSeparatorProperty);
        set => this.SetValue(ShowCommandSeparatorProperty, value);
    }

    public bool ShowViewModeSelector
    {
        get => (bool)this.GetValue(ShowViewModeSelectorProperty);
        set => this.SetValue(ShowViewModeSelectorProperty, value);
    }

    public bool ShowRememberViewToggle
    {
        get => (bool)this.GetValue(ShowRememberViewToggleProperty);
        set => this.SetValue(ShowRememberViewToggleProperty, value);
    }

    public bool IsSearchBoxVisible => (bool)this.GetValue(IsSearchBoxVisibleProperty);

    public bool IsSearchSeparatorVisible => (bool)this.GetValue(IsSearchSeparatorVisibleProperty);

    public bool IsNewButtonVisible => (bool)this.GetValue(IsNewButtonVisibleProperty);

    public bool IsViewButtonVisible => (bool)this.GetValue(IsViewButtonVisibleProperty);

    public bool IsEditButtonVisible => (bool)this.GetValue(IsEditButtonVisibleProperty);

    public bool IsDeleteButtonVisible => (bool)this.GetValue(IsDeleteButtonVisibleProperty);

    public bool IsCommandSeparatorVisible => (bool)this.GetValue(IsCommandSeparatorVisibleProperty);

    public bool IsViewModeSelectorVisible => (bool)this.GetValue(IsViewModeSelectorVisibleProperty);

    public bool IsRememberViewToggleVisible => (bool)this.GetValue(IsRememberViewToggleVisibleProperty);

    public string SearchPlaceholder
    {
        get => (string)this.GetValue(SearchPlaceholderProperty);
        set => this.SetValue(SearchPlaceholderProperty, value);
    }

    public double ToolbarItemHeight
    {
        get => (double)this.GetValue(ToolbarItemHeightProperty);
        set => this.SetValue(ToolbarItemHeightProperty, value);
    }

    public double SearchBoxWidth
    {
        get => (double)this.GetValue(SearchBoxWidthProperty);
        set => this.SetValue(SearchBoxWidthProperty, value);
    }

    public double SearchBoxHeight
    {
        get => (double)this.GetValue(SearchBoxHeightProperty);
        set => this.SetValue(SearchBoxHeightProperty, value);
    }

    public double EffectiveSearchBoxHeight => (double)this.GetValue(EffectiveSearchBoxHeightProperty);

    public Thickness SearchBoxPadding
    {
        get => (Thickness)this.GetValue(SearchBoxPaddingProperty);
        set => this.SetValue(SearchBoxPaddingProperty, value);
    }

    public double SearchLeadingIconSize
    {
        get => (double)this.GetValue(SearchLeadingIconSizeProperty);
        set => this.SetValue(SearchLeadingIconSizeProperty, value);
    }

    public double SearchTrailingIconSize
    {
        get => (double)this.GetValue(SearchTrailingIconSizeProperty);
        set => this.SetValue(SearchTrailingIconSizeProperty, value);
    }

    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }

    public double ButtonIconSize
    {
        get => (double)this.GetValue(ButtonIconSizeProperty);
        set => this.SetValue(ButtonIconSizeProperty, value);
    }

    public double NewButtonIconSize
    {
        get => (double)this.GetValue(NewButtonIconSizeProperty);
        set => this.SetValue(NewButtonIconSizeProperty, value);
    }

    public double ViewButtonIconSize
    {
        get => (double)this.GetValue(ViewButtonIconSizeProperty);
        set => this.SetValue(ViewButtonIconSizeProperty, value);
    }

    public double EditButtonIconSize
    {
        get => (double)this.GetValue(EditButtonIconSizeProperty);
        set => this.SetValue(EditButtonIconSizeProperty, value);
    }

    public double DeleteButtonIconSize
    {
        get => (double)this.GetValue(DeleteButtonIconSizeProperty);
        set => this.SetValue(DeleteButtonIconSizeProperty, value);
    }

    public double EffectiveNewButtonIconSize => (double)this.GetValue(EffectiveNewButtonIconSizeProperty);

    public double EffectiveViewButtonIconSize => (double)this.GetValue(EffectiveViewButtonIconSizeProperty);

    public double EffectiveEditButtonIconSize => (double)this.GetValue(EffectiveEditButtonIconSizeProperty);

    public double EffectiveDeleteButtonIconSize => (double)this.GetValue(EffectiveDeleteButtonIconSizeProperty);

    public object? NewButtonContent
    {
        get => this.GetValue(NewButtonContentProperty);
        set => this.SetValue(NewButtonContentProperty, value);
    }

    public object? ViewButtonContent
    {
        get => this.GetValue(ViewButtonContentProperty);
        set => this.SetValue(ViewButtonContentProperty, value);
    }

    public object? EditButtonContent
    {
        get => this.GetValue(EditButtonContentProperty);
        set => this.SetValue(EditButtonContentProperty, value);
    }

    public object? DeleteButtonContent
    {
        get => this.GetValue(DeleteButtonContentProperty);
        set => this.SetValue(DeleteButtonContentProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.RefreshVisibilityStates();
        this.RefreshIconSizeStates();
        this.RefreshDimensionStates();
    }
    #endregion

    #region ### Private Methods ###
    private static void OnToolbarChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XToolbarPresenter presenter)
        {
            return;
        }

        presenter.DetachToolbarPropertyChangedSource();
        presenter.AttachToolbarPropertyChangedSource(eventArgs.NewValue as INotifyPropertyChanged);
        presenter.RefreshVisibilityStates();
    }

    private static void OnSearchContextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XToolbarPresenter presenter)
        {
            return;
        }

        IXSearchContext? searchContext = eventArgs.NewValue as IXSearchContext;
        presenter.SetValue(EffectiveSearchContextPropertyKey, searchContext);
        presenter.SetValue(HasSearchContextPropertyKey, searchContext is not null);
        presenter.RefreshVisibilityStates();
    }

    private static void OnVisibilityInputChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToolbarPresenter presenter)
        {
            presenter.RefreshVisibilityStates();
        }
    }

    private static void OnDimensionInputChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToolbarPresenter presenter)
        {
            presenter.RefreshDimensionStates();
        }
    }

    private static void OnIconSizeInputChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToolbarPresenter presenter)
        {
            presenter.RefreshIconSizeStates();
        }
    }

    private void AttachToolbarPropertyChangedSource(INotifyPropertyChanged? propertyChangedSource)
    {
        this.toolbarPropertyChangedSource = propertyChangedSource;

        if (this.toolbarPropertyChangedSource is not null)
        {
            this.toolbarPropertyChangedSource.PropertyChanged += this.OnToolbarPropertyChanged;
        }
    }

    private void DetachToolbarPropertyChangedSource()
    {
        if (this.toolbarPropertyChangedSource is not null)
        {
            this.toolbarPropertyChangedSource.PropertyChanged -= this.OnToolbarPropertyChanged;
            this.toolbarPropertyChangedSource = null;
        }
    }

    private void OnToolbarPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
    {
        this.RefreshVisibilityStates();
    }

    private void RefreshVisibilityStates()
    {
        bool showToolbar = this.ShowToolbar;
        bool hasSearchContext = this.HasSearchContext;
        bool hasToolbarItems = this.Toolbar?.HasToolbarItems == true;
        bool isSearchBoxVisible = showToolbar && this.ShowSearchBox && hasSearchContext;

        this.SetValue(IsSearchBoxVisiblePropertyKey, isSearchBoxVisible);
        this.SetValue(IsSearchSeparatorVisiblePropertyKey, showToolbar && this.ShowSearchSeparator && isSearchBoxVisible && hasToolbarItems);
        this.SetValue(IsNewButtonVisiblePropertyKey, showToolbar && this.ShowNewButton && this.Toolbar?.ShowNewButton == true);
        this.SetValue(IsViewButtonVisiblePropertyKey, showToolbar && this.ShowViewButton && this.Toolbar?.ShowViewButton == true);
        this.SetValue(IsEditButtonVisiblePropertyKey, showToolbar && this.ShowEditButton && this.Toolbar?.ShowEditButton == true);
        this.SetValue(IsDeleteButtonVisiblePropertyKey, showToolbar && this.ShowDeleteButton && this.Toolbar?.ShowDeleteButton == true);
        this.SetValue(IsCommandSeparatorVisiblePropertyKey, showToolbar && this.ShowCommandSeparator && this.Toolbar?.ShowCommandSeparator == true);
        this.SetValue(IsRememberViewToggleVisiblePropertyKey, showToolbar && this.ShowRememberViewToggle && this.Toolbar?.ShowRememberViewToggle == true);
        this.SetValue(IsViewModeSelectorVisiblePropertyKey, showToolbar && this.ShowViewModeSelector && this.Toolbar?.ShowViewModeSelector == true);
    }

    private void RefreshDimensionStates()
    {
        double toolbarItemHeight = ResolveDimension(this.ToolbarItemHeight, 30d);
        this.SetValue(EffectiveSearchBoxHeightPropertyKey, ResolveDimension(this.SearchBoxHeight, toolbarItemHeight));
    }

    private void RefreshIconSizeStates()
    {
        double globalIconSize = ResolveIconSize(this.ButtonIconSize, 16d);

        this.SetValue(EffectiveNewButtonIconSizePropertyKey, ResolveIconSize(this.NewButtonIconSize, globalIconSize));
        this.SetValue(EffectiveViewButtonIconSizePropertyKey, ResolveIconSize(this.ViewButtonIconSize, globalIconSize));
        this.SetValue(EffectiveEditButtonIconSizePropertyKey, ResolveIconSize(this.EditButtonIconSize, globalIconSize));
        this.SetValue(EffectiveDeleteButtonIconSizePropertyKey, ResolveIconSize(this.DeleteButtonIconSize, globalIconSize));
    }

    private static double ResolveDimension(double value, double fallbackValue)
    {
        return double.IsNaN(value) || value <= 0d
            ? fallbackValue
            : value;
    }

    private static double ResolveIconSize(double value, double fallbackValue)
    {
        return double.IsNaN(value) || value <= 0d
            ? fallbackValue
            : value;
    }
    #endregion
}
#endregion
