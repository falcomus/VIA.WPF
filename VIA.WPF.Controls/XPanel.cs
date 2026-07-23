// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPanel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XPanel ###
/// <summary>
/// Represents a reusable container panel with optional header, footer, and action buttons.
/// </summary>
[Obsolete("Use XGroup with arbitrary Actions and Footer content instead.")]
public class XPanel : ContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XPanel),
        new FrameworkPropertyMetadata(string.Empty, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="TitleTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
        nameof(TitleTemplate),
        typeof(DataTemplate),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
        nameof(HeaderContent),
        typeof(object),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HeaderContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentTemplateProperty = DependencyProperty.Register(
        nameof(HeaderContentTemplate),
        typeof(DataTemplate),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TitleFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
        nameof(TitleFontSize),
        typeof(double),
        typeof(XPanel),
        new FrameworkPropertyMetadata(14d));

    /// <summary>
    /// Identifies the <see cref="TitleFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register(
        nameof(TitleFontWeight),
        typeof(FontWeight),
        typeof(XPanel),
        new FrameworkPropertyMetadata(FontWeights.SemiBold));

    /// <summary>
    /// Identifies the <see cref="HeaderContentFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderContentFontSize),
        typeof(double),
        typeof(XPanel),
        new FrameworkPropertyMetadata(13d));

    /// <summary>
    /// Identifies the <see cref="HeaderContentFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderContentFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderContentFontWeight),
        typeof(FontWeight),
        typeof(XPanel),
        new FrameworkPropertyMetadata(FontWeights.Normal));

    /// <summary>
    /// Identifies the <see cref="Footer"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer),
        typeof(object),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null, OnFooterStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="FooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate),
        typeof(DataTemplate),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XPanel),
        new FrameworkPropertyMetadata(new CornerRadius(8d), OnCornerRadiusChanged));

    /// <summary>
    /// Identifies the read-only <see cref="TopCornerRadius"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey TopCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(TopCornerRadius),
        typeof(CornerRadius),
        typeof(XPanel),
        new FrameworkPropertyMetadata(default(CornerRadius)));

    /// <summary>
    /// Identifies the <see cref="TopCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TopCornerRadiusProperty = TopCornerRadiusPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="BottomCornerRadius"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey BottomCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(BottomCornerRadius),
        typeof(CornerRadius),
        typeof(XPanel),
        new FrameworkPropertyMetadata(default(CornerRadius)));

    /// <summary>
    /// Identifies the <see cref="BottomCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BottomCornerRadiusProperty = BottomCornerRadiusPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasHeader"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasHeader),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasHeaderProperty = HasHeaderPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasFooter"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasFooterPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasFooter),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasFooter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasFooterProperty = HasFooterPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XPanel),
        new FrameworkPropertyMetadata(new Thickness(16d, 12d, 4d, 12d)));

    /// <summary>
    /// Identifies the <see cref="FooterPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FooterPaddingProperty = DependencyProperty.Register(
        nameof(FooterPadding),
        typeof(Thickness),
        typeof(XPanel),
        new FrameworkPropertyMetadata(new Thickness(16d, 10d, 16d, 12d)));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XPanel),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XPanel),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="HeaderAppearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderAppearanceProperty = DependencyProperty.Register(
        nameof(HeaderAppearance),
        typeof(XControlAppearance),
        typeof(XPanel),
        new FrameworkPropertyMetadata(XControlAppearance.Subtle));

    /// <summary>
    /// Identifies the <see cref="HeaderBackgroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderBackgroundBrushProperty = DependencyProperty.Register(
        nameof(HeaderBackgroundBrush),
        typeof(Brush),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderForegroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderForegroundBrushProperty = DependencyProperty.Register(
        nameof(HeaderForegroundBrush),
        typeof(Brush),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Elevation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XPanel),
        new FrameworkPropertyMetadata(XElevation.Low));

    /// <summary>
    /// Identifies the <see cref="ShowNewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowNewButtonProperty = DependencyProperty.Register(
        nameof(ShowNewButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty = DependencyProperty.Register(
        nameof(ShowEditButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty = DependencyProperty.Register(
        nameof(ShowDeleteButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowRefreshButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowRefreshButtonProperty = DependencyProperty.Register(
        nameof(ShowRefreshButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowSettingsButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSettingsButtonProperty = DependencyProperty.Register(
        nameof(ShowSettingsButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowSearchButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSearchButtonProperty = DependencyProperty.Register(
        nameof(ShowSearchButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowFilterButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowFilterButtonProperty = DependencyProperty.Register(
        nameof(ShowFilterButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ShowMenuButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowMenuButtonProperty = DependencyProperty.Register(
        nameof(ShowMenuButton),
        typeof(bool),
        typeof(XPanel),
        new FrameworkPropertyMetadata(false, OnHeaderStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="NewCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(
        nameof(NewCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
        nameof(EditCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
        nameof(DeleteCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RefreshCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(
        nameof(RefreshCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SettingsCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SettingsCommandProperty = DependencyProperty.Register(
        nameof(SettingsCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SearchCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
        nameof(SearchCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FilterCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FilterCommandProperty = DependencyProperty.Register(
        nameof(FilterCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="MenuCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MenuCommandProperty = DependencyProperty.Register(
        nameof(MenuCommand),
        typeof(ICommand),
        typeof(XPanel),
        new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XPanel"/> class.
    /// </summary>
    static XPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XPanel),
            new FrameworkPropertyMetadata(typeof(XPanel)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XPanel"/> class.
    /// </summary>
    public XPanel()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
        this.UpdateCornerRadii();
        this.UpdateHeaderState();
        this.UpdateFooterState();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the panel title.
    /// </summary>
    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the panel title.
    /// </summary>
    public DataTemplate? TitleTemplate
    {
        get => (DataTemplate?)this.GetValue(TitleTemplateProperty);
        set => this.SetValue(TitleTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the custom header content.
    /// </summary>
    public object? HeaderContent
    {
        get => this.GetValue(HeaderContentProperty);
        set => this.SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the custom header content.
    /// </summary>
    public DataTemplate? HeaderContentTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderContentTemplateProperty);
        set => this.SetValue(HeaderContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size used for the panel title.
    /// </summary>
    public double TitleFontSize
    {
        get => (double)this.GetValue(TitleFontSizeProperty);
        set => this.SetValue(TitleFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font weight used for the panel title.
    /// </summary>
    public FontWeight TitleFontWeight
    {
        get => (FontWeight)this.GetValue(TitleFontWeightProperty);
        set => this.SetValue(TitleFontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size used for the custom header content.
    /// </summary>
    public double HeaderContentFontSize
    {
        get => (double)this.GetValue(HeaderContentFontSizeProperty);
        set => this.SetValue(HeaderContentFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font weight used for the custom header content.
    /// </summary>
    public FontWeight HeaderContentFontWeight
    {
        get => (FontWeight)this.GetValue(HeaderContentFontWeightProperty);
        set => this.SetValue(HeaderContentFontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer content.
    /// </summary>
    public object? Footer
    {
        get => this.GetValue(FooterProperty);
        set => this.SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the footer content.
    /// </summary>
    public DataTemplate? FooterTemplate
    {
        get => (DataTemplate?)this.GetValue(FooterTemplateProperty);
        set => this.SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the panel corner radius.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets the top-only corner radius.
    /// </summary>
    public CornerRadius TopCornerRadius => (CornerRadius)this.GetValue(TopCornerRadiusProperty);

    /// <summary>
    /// Gets the bottom-only corner radius.
    /// </summary>
    public CornerRadius BottomCornerRadius => (CornerRadius)this.GetValue(BottomCornerRadiusProperty);

    /// <summary>
    /// Gets a value indicating whether the panel has a visible header.
    /// </summary>
    public bool HasHeader => (bool)this.GetValue(HasHeaderProperty);

    /// <summary>
    /// Gets a value indicating whether the panel has a visible footer.
    /// </summary>
    public bool HasFooter => (bool)this.GetValue(HasFooterProperty);

    /// <summary>
    /// Gets or sets the padding of the header area.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)this.GetValue(HeaderPaddingProperty);
        set => this.SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the footer area.
    /// </summary>
    public Thickness FooterPadding
    {
        get => (Thickness)this.GetValue(FooterPaddingProperty);
        set => this.SetValue(FooterPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    public XControlAppearance Appearance
    {
        get => (XControlAppearance)this.GetValue(AppearanceProperty);
        set => this.SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual appearance of the header area.
    /// </summary>
    public XControlAppearance HeaderAppearance
    {
        get => (XControlAppearance)this.GetValue(HeaderAppearanceProperty);
        set => this.SetValue(HeaderAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the explicit header background brush.
    /// </summary>
    public Brush? HeaderBackgroundBrush
    {
        get => (Brush?)this.GetValue(HeaderBackgroundBrushProperty);
        set => this.SetValue(HeaderBackgroundBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the explicit header foreground brush.
    /// </summary>
    public Brush? HeaderForegroundBrush
    {
        get => (Brush?)this.GetValue(HeaderForegroundBrushProperty);
        set => this.SetValue(HeaderForegroundBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation of the panel.
    /// </summary>
    public XElevation Elevation
    {
        get => (XElevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the new button is shown.
    /// </summary>
    public bool ShowNewButton
    {
        get => (bool)this.GetValue(ShowNewButtonProperty);
        set => this.SetValue(ShowNewButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit button is shown.
    /// </summary>
    public bool ShowEditButton
    {
        get => (bool)this.GetValue(ShowEditButtonProperty);
        set => this.SetValue(ShowEditButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the delete button is shown.
    /// </summary>
    public bool ShowDeleteButton
    {
        get => (bool)this.GetValue(ShowDeleteButtonProperty);
        set => this.SetValue(ShowDeleteButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the refresh button is shown.
    /// </summary>
    public bool ShowRefreshButton
    {
        get => (bool)this.GetValue(ShowRefreshButtonProperty);
        set => this.SetValue(ShowRefreshButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the settings button is shown.
    /// </summary>
    public bool ShowSettingsButton
    {
        get => (bool)this.GetValue(ShowSettingsButtonProperty);
        set => this.SetValue(ShowSettingsButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the search button is shown.
    /// </summary>
    public bool ShowSearchButton
    {
        get => (bool)this.GetValue(ShowSearchButtonProperty);
        set => this.SetValue(ShowSearchButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the filter button is shown.
    /// </summary>
    public bool ShowFilterButton
    {
        get => (bool)this.GetValue(ShowFilterButtonProperty);
        set => this.SetValue(ShowFilterButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the menu button is shown.
    /// </summary>
    public bool ShowMenuButton
    {
        get => (bool)this.GetValue(ShowMenuButtonProperty);
        set => this.SetValue(ShowMenuButtonProperty, value);
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
    /// Gets or sets the command executed by the edit button.
    /// </summary>
    public ICommand? EditCommand
    {
        get => (ICommand?)this.GetValue(EditCommandProperty);
        set => this.SetValue(EditCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the delete button.
    /// </summary>
    public ICommand? DeleteCommand
    {
        get => (ICommand?)this.GetValue(DeleteCommandProperty);
        set => this.SetValue(DeleteCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the refresh button.
    /// </summary>
    public ICommand? RefreshCommand
    {
        get => (ICommand?)this.GetValue(RefreshCommandProperty);
        set => this.SetValue(RefreshCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the settings button.
    /// </summary>
    public ICommand? SettingsCommand
    {
        get => (ICommand?)this.GetValue(SettingsCommandProperty);
        set => this.SetValue(SettingsCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the search button.
    /// </summary>
    public ICommand? SearchCommand
    {
        get => (ICommand?)this.GetValue(SearchCommandProperty);
        set => this.SetValue(SearchCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the filter button.
    /// </summary>
    public ICommand? FilterCommand
    {
        get => (ICommand?)this.GetValue(FilterCommandProperty);
        set => this.SetValue(FilterCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the menu button.
    /// </summary>
    public ICommand? MenuCommand
    {
        get => (ICommand?)this.GetValue(MenuCommandProperty);
        set => this.SetValue(MenuCommandProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnCornerRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XPanel panel)
        {
            panel.UpdateCornerRadii();
        }
    }

    private static void OnHeaderStatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XPanel panel)
        {
            panel.UpdateHeaderState();
        }
    }

    private static void OnFooterStatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XPanel panel)
        {
            panel.UpdateFooterState();
        }
    }

    private void UpdateCornerRadii()
    {
        this.SetValue(
            TopCornerRadiusPropertyKey,
            new CornerRadius(this.CornerRadius.TopLeft, this.CornerRadius.TopRight, 0d, 0d));

        this.SetValue(
            BottomCornerRadiusPropertyKey,
            new CornerRadius(0d, 0d, this.CornerRadius.BottomRight, this.CornerRadius.BottomLeft));
    }

    private void UpdateHeaderState()
    {
        bool hasHeader =
            !string.IsNullOrWhiteSpace(this.Title) ||
            this.HeaderContent is not null ||
            this.ShowNewButton ||
            this.ShowEditButton ||
            this.ShowDeleteButton ||
            this.ShowRefreshButton ||
            this.ShowSettingsButton ||
            this.ShowSearchButton ||
            this.ShowFilterButton ||
            this.ShowMenuButton;

        this.SetValue(HasHeaderPropertyKey, hasHeader);
    }

    private void UpdateFooterState()
    {
        this.SetValue(HasFooterPropertyKey, this.Footer is not null);
    }
    #endregion
}
#endregion
