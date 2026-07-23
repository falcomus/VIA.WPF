// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewContainer.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VIA.WPF.Controls.Navigation;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

#region ### Class XViewContainer ###
/// <summary>
/// Represents a reusable view container that hosts a list view, an optional tree view and an optional local detail area.
/// </summary>
[TemplatePart(Name = OverlayPartName, Type = typeof(FrameworkElement))]
[TemplatePart(Name = CloseButtonPartName, Type = typeof(ButtonBase))]
[TemplatePart(Name = CancelDetailButtonPartName, Type = typeof(ButtonBase))]
[TemplatePart(Name = DetailLayerPartName, Type = typeof(FrameworkElement))]
[TemplatePart(Name = DetailBorderPartName, Type = typeof(FrameworkElement))]
public class XViewContainer : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the overlay template part.
    /// </summary>
    private const string OverlayPartName = "PART_Overlay";

    /// <summary>
    /// The name of the close button template part.
    /// </summary>
    private const string CloseButtonPartName = "PART_CloseButton";

    /// <summary>
    /// The name of the cancel detail button template part.
    /// </summary>
    private const string CancelDetailButtonPartName = "PART_CancelDetailButton";

    /// <summary>
    /// The name of the detail layer template part.
    /// </summary>
    private const string DetailLayerPartName = "PART_DetailLayer";

    /// <summary>
    /// The name of the detail border template part.
    /// </summary>
    private const string DetailBorderPartName = "PART_DetailBorder";

    /// <summary>
    /// The time window in milliseconds during which overlay clicks are ignored after opening the detail area.
    /// </summary>
    private const double OverlayClickCloseSuppressionMilliseconds = 250d;
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="ListHost"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ListHostProperty = DependencyProperty.Register(
        nameof(ListHost),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ListHostTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ListHostTemplateProperty = DependencyProperty.Register(
        nameof(ListHostTemplate),
        typeof(DataTemplate),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TreeHost"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeHostProperty = DependencyProperty.Register(
        nameof(TreeHost),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnTreeHostChanged));

    /// <summary>
    /// Identifies the <see cref="TreeHostTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TreeHostTemplateProperty = DependencyProperty.Register(
        nameof(TreeHostTemplate),
        typeof(DataTemplate),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ViewMode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(
        nameof(ViewMode),
        typeof(XContentViewMode),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(XContentViewMode.Grid));

    /// <summary>
    /// Identifies the <see cref="CrudContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CrudContextProperty = DependencyProperty.Register(
        nameof(CrudContext),
        typeof(XCrudContext),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnCrudContextChanged));

    /// <summary>
    /// Identifies the <see cref="RequireCrudContext"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RequireCrudContextProperty = DependencyProperty.Register(
        nameof(RequireCrudContext),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="DetailPresentation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailPresentationProperty = DependencyProperty.Register(
        nameof(DetailPresentation),
        typeof(XViewDetailPresentation),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(XViewDetailPresentation.Dialog, OnNavigationLockAffectingPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="UseWindowEditorOverlay" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty UseWindowEditorOverlayProperty = DependencyProperty.Register(
        nameof(UseWindowEditorOverlay),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true, OnWindowEditorOverlayAffectingPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="DetailHost"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHostProperty = DependencyProperty.Register(
        nameof(DetailHost),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnDetailHostChanged));

    /// <summary>
    /// Identifies the <see cref="DetailHostTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHostTemplateProperty = DependencyProperty.Register(
        nameof(DetailHostTemplate),
        typeof(DataTemplate),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHeaderProperty = DependencyProperty.Register(
        nameof(DetailHeader),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnDetailHeaderChanged));

    /// <summary>
    /// Identifies the <see cref="DetailHeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHeaderTemplateProperty = DependencyProperty.Register(
        nameof(DetailHeaderTemplate),
        typeof(DataTemplate),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailFooter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailFooterProperty = DependencyProperty.Register(
        nameof(DetailFooter),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnDetailFooterChanged));

    /// <summary>
    /// Identifies the <see cref="DetailFooterTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailFooterTemplateProperty = DependencyProperty.Register(
        nameof(DetailFooterTemplate),
        typeof(DataTemplate),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsDetailOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDetailOpenProperty = DependencyProperty.Register(
        nameof(IsDetailOpen),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDetailOpenChanged));

    /// <summary>
    /// Identifies the <see cref="DetailPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailPlacementProperty = DependencyProperty.Register(
        nameof(DetailPlacement),
        typeof(XViewFlyoutPlacement),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(XViewFlyoutPlacement.Top));

    /// <summary>
    /// Identifies the <see cref="DetailWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailWidthProperty = DependencyProperty.Register(
        nameof(DetailWidth),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(double.NaN));

    /// <summary>
    /// Identifies the <see cref="DetailMinWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailMinWidthProperty = DependencyProperty.Register(
        nameof(DetailMinWidth),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(520d));

    /// <summary>
    /// Identifies the <see cref="DetailMaxWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailMaxWidthProperty = DependencyProperty.Register(
        nameof(DetailMaxWidth),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Identifies the <see cref="DetailMinHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailMinHeightProperty = DependencyProperty.Register(
        nameof(DetailMinHeight),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(0d));

    /// <summary>
    /// Identifies the <see cref="DetailMaxHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailMaxHeightProperty = DependencyProperty.Register(
        nameof(DetailMaxHeight),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(560d));

    /// <summary>
    /// Identifies the <see cref="DetailMargin"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailMarginProperty = DependencyProperty.Register(
        nameof(DetailMargin),
        typeof(Thickness),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Thickness(24d)));


    /// <summary>
    /// Identifies the <see cref="DetailPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailPaddingProperty = DependencyProperty.Register(
        nameof(DetailPadding),
        typeof(Thickness),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Thickness(18d)));

    /// <summary>
    /// Identifies the <see cref="DetailHeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHeaderPaddingProperty = DependencyProperty.Register(
        nameof(DetailHeaderPadding),
        typeof(Thickness),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Thickness(18d, 14d, 10d, 12d)));

    /// <summary>
    /// Identifies the <see cref="DetailFooterPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailFooterPaddingProperty = DependencyProperty.Register(
        nameof(DetailFooterPadding),
        typeof(Thickness),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Thickness(18d, 12d, 18d, 14d)));

    /// <summary>
    /// Identifies the <see cref="DetailCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailCornerRadiusProperty = DependencyProperty.Register(
        nameof(DetailCornerRadius),
        typeof(CornerRadius),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new CornerRadius(12d)));

    /// <summary>
    /// Identifies the <see cref="DetailBackground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailBackgroundProperty = DependencyProperty.Register(
        nameof(DetailBackground),
        typeof(Brush),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailBorderBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailBorderBrushProperty = DependencyProperty.Register(
        nameof(DetailBorderBrush),
        typeof(Brush),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DetailBorderThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailBorderThicknessProperty = DependencyProperty.Register(
        nameof(DetailBorderThickness),
        typeof(Thickness),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Thickness(1d)));

    /// <summary>
    /// Identifies the <see cref="DetailHorizontalAlignment"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailHorizontalAlignmentProperty = DependencyProperty.Register(
        nameof(DetailHorizontalAlignment),
        typeof(HorizontalAlignment),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(HorizontalAlignment.Center));

    /// <summary>
    /// Identifies the <see cref="OverlayBackground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register(
        nameof(OverlayBackground),
        typeof(Brush),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 10, 30))));

    /// <summary>
    /// Identifies the <see cref="OverlayOpacity"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(
        nameof(OverlayOpacity),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(0.4d));

    /// <summary>
    /// Identifies the <see cref="OverlayCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OverlayCornerRadiusProperty = DependencyProperty.Register(
        nameof(OverlayCornerRadius),
        typeof(CornerRadius),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new CornerRadius(4d)));

    /// <summary>
    /// Identifies the <see cref="ShowDetailCloseButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDetailCloseButtonProperty = DependencyProperty.Register(
        nameof(ShowDetailCloseButton),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="CloseOnOverlayClick"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseOnOverlayClickProperty = DependencyProperty.Register(
        nameof(CloseOnOverlayClick),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsModal"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register(
        nameof(IsModal),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true, OnNavigationLockAffectingPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="EnableDetailAnimation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EnableDetailAnimationProperty = DependencyProperty.Register(
        nameof(EnableDetailAnimation),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="DetailAnimation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailAnimationProperty = DependencyProperty.Register(
        nameof(DetailAnimation),
        typeof(XViewDetailAnimation),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(XViewDetailAnimation.SlideZoom));

    /// <summary>
    /// Identifies the <see cref="DetailAnimationOffset"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailAnimationOffsetProperty = DependencyProperty.Register(
        nameof(DetailAnimationOffset),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(18d));

    /// <summary>
    /// Identifies the <see cref="DetailAnimationScale"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailAnimationScaleProperty = DependencyProperty.Register(
        nameof(DetailAnimationScale),
        typeof(double),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(0.96d));

    /// <summary>
    /// Identifies the <see cref="DetailAnimationDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DetailAnimationDurationProperty = DependencyProperty.Register(
        nameof(DetailAnimationDuration),
        typeof(Duration),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(220d))));

    /// <summary>
    /// Identifies the <see cref="CloseDetailCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseDetailCommandProperty = DependencyProperty.Register(
        nameof(CloseDetailCommand),
        typeof(ICommand),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CloseDetailCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseDetailCommandParameterProperty = DependencyProperty.Register(
        nameof(CloseDetailCommandParameter),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="PrimaryDetailCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PrimaryDetailCommandProperty = DependencyProperty.Register(
        nameof(PrimaryDetailCommand),
        typeof(ICommand),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="PrimaryDetailCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PrimaryDetailCommandParameterProperty = DependencyProperty.Register(
        nameof(PrimaryDetailCommandParameter),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="PrimaryDetailText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PrimaryDetailTextProperty = DependencyProperty.Register(
        nameof(PrimaryDetailText),
        typeof(string),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata("OK"));

    /// <summary>
    /// Identifies the <see cref="CancelDetailText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CancelDetailTextProperty = DependencyProperty.Register(
        nameof(CancelDetailText),
        typeof(string),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata("Cancel"));

    /// <summary>
    /// Identifies the <see cref="ShowDefaultDetailFooter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDefaultDetailFooterProperty = DependencyProperty.Register(
        nameof(ShowDefaultDetailFooter),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ValidationSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValidationSourceProperty = DependencyProperty.Register(
        nameof(ValidationSource),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null, OnValidationSourceChanged));

    /// <summary>
    /// Identifies the <see cref="ShowValidationHint"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowValidationHintProperty = DependencyProperty.Register(
        nameof(ShowValidationHint),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="EffectiveValidationSource"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey EffectiveValidationSourcePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveValidationSource),
        typeof(object),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EffectiveValidationSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveValidationSourceProperty = EffectiveValidationSourcePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="IsDetailHostedByWindow" /> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsDetailHostedByWindowPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsDetailHostedByWindow),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsDetailHostedByWindow" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDetailHostedByWindowProperty = IsDetailHostedByWindowPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasTreeHost"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasTreeHostPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasTreeHost),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasTreeHost"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasTreeHostProperty = HasTreeHostPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasDetailHeader"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasDetailHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasDetailHeader),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasDetailHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasDetailHeaderProperty = HasDetailHeaderPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the read-only <see cref="HasDetailFooter"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey HasDetailFooterPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasDetailFooter),
        typeof(bool),
        typeof(XViewContainer),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasDetailFooter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasDetailFooterProperty = HasDetailFooterPropertyKey.DependencyProperty;
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The current overlay template part.
    /// </summary>
    private FrameworkElement? overlay;

    /// <summary>
    /// The current close button template part.
    /// </summary>
    private ButtonBase? closeButton;

    /// <summary>
    /// The current cancel detail button template part.
    /// </summary>
    private ButtonBase? cancelDetailButton;

    /// <summary>
    /// The dependency properties that are currently bound automatically from <see cref="CrudContext"/>.
    /// </summary>
    private readonly HashSet<DependencyProperty> autoCrudContextBoundProperties = [];

    /// <summary>
    /// The dependency properties that are currently bound automatically from <see cref="IXCrudPageContext"/>.
    /// </summary>
    private readonly HashSet<DependencyProperty> autoCrudPageContextBoundProperties = [];

    /// <summary>
    /// The current detail layer template part.
    /// </summary>
    private FrameworkElement? detailLayer;

    /// <summary>
    /// The current detail border template part.
    /// </summary>
    private FrameworkElement? detailBorder;

    /// <summary>
    /// The UTC timestamp of the last detail area opening.
    /// </summary>
    private DateTime detailOpenedAtUtc = DateTime.MinValue;

    /// <summary>
    /// The window that currently owns the navigation lock requested by this container.
    /// </summary>
    private Window? navigationLockedWindow;

    /// <summary>
    /// The window that currently hosts the detail editor overlay requested by this container.
    /// </summary>
    private Window? editorOverlayHostWindow;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XViewContainer"/> class.
    /// </summary>
    static XViewContainer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XViewContainer),
            new FrameworkPropertyMetadata(typeof(XViewContainer)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XViewContainer"/> class.
    /// </summary>
    public XViewContainer()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);

        this.Loaded += this.OnLoaded;
        this.Unloaded += this.OnUnloaded;
        this.DataContextChanged += this.OnDataContextChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the host content used for the list or grid view.
    /// </summary>
    public object? ListHost
    {
        get => this.GetValue(ListHostProperty);
        set => this.SetValue(ListHostProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display the list host.
    /// </summary>
    public DataTemplate? ListHostTemplate
    {
        get => (DataTemplate?)this.GetValue(ListHostTemplateProperty);
        set => this.SetValue(ListHostTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the host content used for the tree view.
    /// </summary>
    public object? TreeHost
    {
        get => this.GetValue(TreeHostProperty);
        set => this.SetValue(TreeHostProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display the tree host.
    /// </summary>
    public DataTemplate? TreeHostTemplate
    {
        get => (DataTemplate?)this.GetValue(TreeHostTemplateProperty);
        set => this.SetValue(TreeHostTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the current content view mode.
    /// </summary>
    public XContentViewMode ViewMode
    {
        get => (XContentViewMode)this.GetValue(ViewModeProperty);
        set => this.SetValue(ViewModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional CRUD context used to populate the local detail area.
    /// </summary>
    public XCrudContext? CrudContext
    {
        get => (XCrudContext?)this.GetValue(CrudContextProperty);
        set => this.SetValue(CrudContextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the container expects a CRUD context unless detail handling is configured manually.
    /// </summary>
    public bool RequireCrudContext
    {
        get => (bool)this.GetValue(RequireCrudContextProperty);
        set => this.SetValue(RequireCrudContextProperty, value);
    }

    /// <summary>
    /// Gets or sets how the local detail area is presented.
    /// </summary>
    public XViewDetailPresentation DetailPresentation
    {
        get => (XViewDetailPresentation)this.GetValue(DetailPresentationProperty);
        set => this.SetValue(DetailPresentationProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether dialog details should be hosted by the owning <see cref="Window" /> overlay when available.
    /// </summary>
    public bool UseWindowEditorOverlay
    {
        get => (bool)this.GetValue(UseWindowEditorOverlayProperty);
        set => this.SetValue(UseWindowEditorOverlayProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed inside the local detail area.
    /// </summary>
    public object? DetailHost
    {
        get => this.GetValue(DetailHostProperty);
        set => this.SetValue(DetailHostProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display the local detail content.
    /// </summary>
    public DataTemplate? DetailHostTemplate
    {
        get => (DataTemplate?)this.GetValue(DetailHostTemplateProperty);
        set => this.SetValue(DetailHostTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the header displayed above the local detail content.
    /// </summary>
    public object? DetailHeader
    {
        get => this.GetValue(DetailHeaderProperty);
        set => this.SetValue(DetailHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display the local detail header.
    /// </summary>
    public DataTemplate? DetailHeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(DetailHeaderTemplateProperty);
        set => this.SetValue(DetailHeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the footer displayed below the local detail content.
    /// </summary>
    public object? DetailFooter
    {
        get => this.GetValue(DetailFooterProperty);
        set => this.SetValue(DetailFooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used to display the local detail footer.
    /// </summary>
    public DataTemplate? DetailFooterTemplate
    {
        get => (DataTemplate?)this.GetValue(DetailFooterTemplateProperty);
        set => this.SetValue(DetailFooterTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the local detail area is open.
    /// </summary>
    public bool IsDetailOpen
    {
        get => (bool)this.GetValue(IsDetailOpenProperty);
        set => this.SetValue(IsDetailOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets where the local detail area is displayed inside the view area.
    /// </summary>
    public XViewFlyoutPlacement DetailPlacement
    {
        get => (XViewFlyoutPlacement)this.GetValue(DetailPlacementProperty);
        set => this.SetValue(DetailPlacementProperty, value);
    }

    /// <summary>
    /// Gets or sets the explicit detail area width. Use <see cref="double.NaN"/> for automatic stretching.
    /// </summary>
    public double DetailWidth
    {
        get => (double)this.GetValue(DetailWidthProperty);
        set => this.SetValue(DetailWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum detail area width.
    /// </summary>
    public double DetailMinWidth
    {
        get => (double)this.GetValue(DetailMinWidthProperty);
        set => this.SetValue(DetailMinWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum detail area width.
    /// </summary>
    public double DetailMaxWidth
    {
        get => (double)this.GetValue(DetailMaxWidthProperty);
        set => this.SetValue(DetailMaxWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum detail area height.
    /// </summary>
    public double DetailMinHeight
    {
        get => (double)this.GetValue(DetailMinHeightProperty);
        set => this.SetValue(DetailMinHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum detail area height.
    /// </summary>
    public double DetailMaxHeight
    {
        get => (double)this.GetValue(DetailMaxHeightProperty);
        set => this.SetValue(DetailMaxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the outer margin of the detail area inside the view area.
    /// </summary>
    public Thickness DetailMargin
    {
        get => (Thickness)this.GetValue(DetailMarginProperty);
        set => this.SetValue(DetailMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the inner padding of the detail content area.
    /// </summary>
    public Thickness DetailPadding
    {
        get => (Thickness)this.GetValue(DetailPaddingProperty);
        set => this.SetValue(DetailPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the inner padding of the detail header area.
    /// </summary>
    public Thickness DetailHeaderPadding
    {
        get => (Thickness)this.GetValue(DetailHeaderPaddingProperty);
        set => this.SetValue(DetailHeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the inner padding of the detail footer area.
    /// </summary>
    public Thickness DetailFooterPadding
    {
        get => (Thickness)this.GetValue(DetailFooterPaddingProperty);
        set => this.SetValue(DetailFooterPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the detail area.
    /// </summary>
    public CornerRadius DetailCornerRadius
    {
        get => (CornerRadius)this.GetValue(DetailCornerRadiusProperty);
        set => this.SetValue(DetailCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the background brush of the detail area.
    /// </summary>
    public Brush? DetailBackground
    {
        get => (Brush?)this.GetValue(DetailBackgroundProperty);
        set => this.SetValue(DetailBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the border brush of the detail area.
    /// </summary>
    public Brush? DetailBorderBrush
    {
        get => (Brush?)this.GetValue(DetailBorderBrushProperty);
        set => this.SetValue(DetailBorderBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the border thickness of the detail area.
    /// </summary>
    public Thickness DetailBorderThickness
    {
        get => (Thickness)this.GetValue(DetailBorderThicknessProperty);
        set => this.SetValue(DetailBorderThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal detail area alignment.
    /// </summary>
    public HorizontalAlignment DetailHorizontalAlignment
    {
        get => (HorizontalAlignment)this.GetValue(DetailHorizontalAlignmentProperty);
        set => this.SetValue(DetailHorizontalAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the overlay background brush.
    /// </summary>
    public Brush OverlayBackground
    {
        get => (Brush)this.GetValue(OverlayBackgroundProperty);
        set => this.SetValue(OverlayBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the opacity of the overlay background.
    /// </summary>
    public double OverlayOpacity
    {
        get => (double)this.GetValue(OverlayOpacityProperty);
        set => this.SetValue(OverlayOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the overlay background.
    /// </summary>
    public CornerRadius OverlayCornerRadius
    {
        get => (CornerRadius)this.GetValue(OverlayCornerRadiusProperty);
        set => this.SetValue(OverlayCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail close button is visible.
    /// </summary>
    public bool ShowDetailCloseButton
    {
        get => (bool)this.GetValue(ShowDetailCloseButtonProperty);
        set => this.SetValue(ShowDetailCloseButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether clicking the overlay closes the detail area.
    /// </summary>
    public bool CloseOnOverlayClick
    {
        get => (bool)this.GetValue(CloseOnOverlayClickProperty);
        set => this.SetValue(CloseOnOverlayClickProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail area is modal and cannot be closed by clicking the overlay.
    /// </summary>
    public bool IsModal
    {
        get => (bool)this.GetValue(IsModalProperty);
        set => this.SetValue(IsModalProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether opening and closing of the detail area is animated.
    /// </summary>
    public bool EnableDetailAnimation
    {
        get => (bool)this.GetValue(EnableDetailAnimationProperty);
        set => this.SetValue(EnableDetailAnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets the animation style used when the local detail area opens or closes.
    /// </summary>
    public XViewDetailAnimation DetailAnimation
    {
        get => (XViewDetailAnimation)this.GetValue(DetailAnimationProperty);
        set => this.SetValue(DetailAnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical start offset used by slide-based detail animations.
    /// </summary>
    public double DetailAnimationOffset
    {
        get => (double)this.GetValue(DetailAnimationOffsetProperty);
        set => this.SetValue(DetailAnimationOffsetProperty, value);
    }

    /// <summary>
    /// Gets or sets the start scale used by zoom-based detail animations.
    /// </summary>
    public double DetailAnimationScale
    {
        get => (double)this.GetValue(DetailAnimationScaleProperty);
        set => this.SetValue(DetailAnimationScaleProperty, value);
    }

    /// <summary>
    /// Gets or sets the duration used by the detail open/close animation.
    /// </summary>
    public Duration DetailAnimationDuration
    {
        get => (Duration)this.GetValue(DetailAnimationDurationProperty);
        set => this.SetValue(DetailAnimationDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the command that is invoked when the detail area should be closed.
    /// </summary>
    public ICommand? CloseDetailCommand
    {
        get => (ICommand?)this.GetValue(CloseDetailCommandProperty);
        set => this.SetValue(CloseDetailCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used for <see cref="CloseDetailCommand"/>.
    /// </summary>
    public object? CloseDetailCommandParameter
    {
        get => this.GetValue(CloseDetailCommandParameterProperty);
        set => this.SetValue(CloseDetailCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command that is invoked by the primary detail footer button.
    /// </summary>
    public ICommand? PrimaryDetailCommand
    {
        get => (ICommand?)this.GetValue(PrimaryDetailCommandProperty);
        set => this.SetValue(PrimaryDetailCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used for <see cref="PrimaryDetailCommand"/>.
    /// </summary>
    public object? PrimaryDetailCommandParameter
    {
        get => this.GetValue(PrimaryDetailCommandParameterProperty);
        set => this.SetValue(PrimaryDetailCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the text displayed by the primary detail footer button.
    /// </summary>
    public string PrimaryDetailText
    {
        get => (string)this.GetValue(PrimaryDetailTextProperty);
        set => this.SetValue(PrimaryDetailTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text displayed by the cancel detail footer button.
    /// </summary>
    public string CancelDetailText
    {
        get => (string)this.GetValue(CancelDetailTextProperty);
        set => this.SetValue(CancelDetailTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the built-in detail footer with primary and cancel actions is shown.
    /// </summary>
    public bool ShowDefaultDetailFooter
    {
        get => (bool)this.GetValue(ShowDefaultDetailFooterProperty);
        set => this.SetValue(ShowDefaultDetailFooterProperty, value);
    }

    /// <summary>
    /// Gets or sets the validation source used by the detail validation hint. If no value is set, <see cref="DetailHost"/> is used.
    /// </summary>
    public object? ValidationSource
    {
        get => this.GetValue(ValidationSourceProperty);
        set => this.SetValue(ValidationSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a compact validation hint is shown in the detail header.
    /// </summary>
    public bool ShowValidationHint
    {
        get => (bool)this.GetValue(ShowValidationHintProperty);
        set => this.SetValue(ShowValidationHintProperty, value);
    }

    /// <summary>
    /// Gets the effective validation source used by the detail validation hint.
    /// </summary>
    public object? EffectiveValidationSource => this.GetValue(EffectiveValidationSourceProperty);

    /// <summary>
    /// Gets a value indicating whether the current detail area is hosted by the owning window overlay.
    /// </summary>
    public bool IsDetailHostedByWindow => (bool)this.GetValue(IsDetailHostedByWindowProperty);

    /// <summary>
    /// Gets a value indicating whether the container has tree host content.
    /// </summary>
    public bool HasTreeHost => (bool)this.GetValue(HasTreeHostProperty);

    /// <summary>
    /// Gets a value indicating whether the detail area has a visible header.
    /// </summary>
    public bool HasDetailHeader => (bool)this.GetValue(HasDetailHeaderProperty);

    /// <summary>
    /// Gets a value indicating whether the detail area has a visible footer.
    /// </summary>
    public bool HasDetailFooter => (bool)this.GetValue(HasDetailFooterProperty);
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        if (this.overlay is not null)
        {
            this.overlay.MouseLeftButtonUp -= this.OnOverlayMouseLeftButtonUp;
        }

        if (this.closeButton is not null)
        {
            this.closeButton.Click -= this.OnCloseButtonClick;
        }

        if (this.cancelDetailButton is not null)
        {
            this.cancelDetailButton.Click -= this.OnCancelDetailButtonClick;
        }

        this.StopDetailAnimations();

        base.OnApplyTemplate();

        this.overlay = this.GetTemplateChild(OverlayPartName) as FrameworkElement;
        this.closeButton = this.GetTemplateChild(CloseButtonPartName) as ButtonBase;
        this.cancelDetailButton = this.GetTemplateChild(CancelDetailButtonPartName) as ButtonBase;
        this.detailLayer = this.GetTemplateChild(DetailLayerPartName) as FrameworkElement;
        this.detailBorder = this.GetTemplateChild(DetailBorderPartName) as FrameworkElement;

        if (this.overlay is not null)
        {
            this.overlay.MouseLeftButtonUp += this.OnOverlayMouseLeftButtonUp;
        }

        if (this.closeButton is not null)
        {
            this.closeButton.Click += this.OnCloseButtonClick;
        }

        if (this.cancelDetailButton is not null)
        {
            this.cancelDetailButton.Click += this.OnCancelDetailButtonClick;
        }

        this.ApplyResolvedCrudIntegration();
        this.UpdateDetailOpenState(useTransitions: false);
        this.UpdateNavigationLock();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles changes of the <see cref="CrudContext"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnCrudContextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.ApplyResolvedCrudIntegration();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="IsDetailOpen"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIsDetailOpenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            if ((bool)eventArgs.NewValue)
            {
                viewContainer.detailOpenedAtUtc = DateTime.UtcNow;
            }

            viewContainer.ValidateCrudContextRequirement();
            viewContainer.UpdateDetailOpenState(useTransitions: true);
            viewContainer.UpdateNavigationLock();
        }
    }

    /// <summary>
    /// Handles changes of dependency properties that affect the window-wide navigation lock.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnNavigationLockAffectingPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.UpdateNavigationLock();
        }
    }

    /// <summary>
    /// Handles changes of dependency properties that affect the window-hosted editor overlay.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnWindowEditorOverlayAffectingPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.UpdateDetailOpenState(useTransitions: false);
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="DetailHost"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnDetailHostChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.UpdateEffectiveValidationSource();
            viewContainer.UpdateWindowEditorOverlayIfHosted();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="ValidationSource"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnValidationSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.UpdateEffectiveValidationSource();
            viewContainer.UpdateWindowEditorOverlayIfHosted();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="TreeHost"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTreeHostChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.SetValue(HasTreeHostPropertyKey, HasMeaningfulContent(eventArgs.NewValue));
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="DetailHeader"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnDetailHeaderChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.SetValue(HasDetailHeaderPropertyKey, HasMeaningfulContent(eventArgs.NewValue));
            viewContainer.UpdateWindowEditorOverlayIfHosted();
        }
    }

    /// <summary>
    /// Handles changes of the <see cref="DetailFooter"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnDetailFooterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XViewContainer viewContainer)
        {
            viewContainer.SetValue(HasDetailFooterPropertyKey, HasMeaningfulContent(eventArgs.NewValue));
            viewContainer.UpdateWindowEditorOverlayIfHosted();
        }
    }

    /// <summary>
    /// Updates the validation source used by the compact detail validation hint.
    /// </summary>
    private void UpdateEffectiveValidationSource()
    {
        this.SetValue(EffectiveValidationSourcePropertyKey, this.ValidationSource ?? this.DetailHost);
    }

    /// <summary>
    /// Resolves and applies optional CRUD integration from the explicit <see cref="CrudContext"/> or the current data context.
    /// </summary>
    private void ApplyResolvedCrudIntegration()
    {
        XCrudContext? crudContext = this.GetEffectiveCrudContext();

        this.ApplyCrudContextBindings(crudContext);
        this.ApplyCrudPageContextBindings(this.GetEffectiveCrudPageContext());
        this.ValidateCrudContextRequirement();
    }

    /// <summary>
    /// Gets the effective CRUD context from the explicit property or the current data context.
    /// </summary>
    /// <returns>The resolved CRUD context or <see langword="null"/>.</returns>
    private XCrudContext? GetEffectiveCrudContext()
    {
        return this.CrudContext ?? this.GetEffectiveCrudPageContext()?.CrudContext;
    }

    /// <summary>
    /// Gets the effective CRUD page context from the current data context.
    /// </summary>
    /// <returns>The resolved CRUD page context or <see langword="null"/>.</returns>
    private IXCrudPageContext? GetEffectiveCrudPageContext()
    {
        return this.DataContext as IXCrudPageContext;
    }

    /// <summary>
    /// Applies the default bindings backed by the specified CRUD context.
    /// </summary>
    /// <param name="crudContext">The CRUD context or <see langword="null"/>.</param>
    private void ApplyCrudContextBindings(XCrudContext? crudContext)
    {
        if (crudContext is null)
        {
            this.ClearCrudContextBindings();
            return;
        }

        this.SetCrudContextBinding(DetailHostProperty, nameof(XCrudContext.Editor), BindingMode.OneWay, crudContext);
        this.SetCrudContextBinding(DetailHeaderProperty, nameof(XCrudContext.Title), BindingMode.OneWay, crudContext);
        this.SetCrudContextBinding(IsDetailOpenProperty, nameof(XCrudContext.IsOpen), BindingMode.TwoWay, crudContext);
        this.SetCrudContextBinding(CloseDetailCommandProperty, nameof(XCrudContext.CancelCommand), BindingMode.OneWay, crudContext);
        this.UpdateEffectiveValidationSource();
    }

    /// <summary>
    /// Applies default command bindings backed by the specified CRUD page context.
    /// </summary>
    /// <param name="pageContext">The CRUD page context or <see langword="null"/>.</param>
    private void ApplyCrudPageContextBindings(IXCrudPageContext? pageContext)
    {
        if (pageContext is null)
        {
            this.ClearCrudPageContextBindings();
            return;
        }

        this.SetCrudPageContextValue(PrimaryDetailCommandProperty, pageContext.SaveDetailCommand);
    }

    /// <summary>
    /// Clears bindings that were created automatically from <see cref="CrudContext"/>.
    /// </summary>
    private void ClearCrudContextBindings()
    {
        foreach (DependencyProperty dependencyProperty in this.autoCrudContextBoundProperties.ToArray())
        {
            BindingOperations.ClearBinding(this, dependencyProperty);
            this.ClearValue(dependencyProperty);
        }

        this.autoCrudContextBoundProperties.Clear();
        this.UpdateEffectiveValidationSource();
    }

    /// <summary>
    /// Clears bindings that were created automatically from <see cref="IXCrudPageContext"/>.
    /// </summary>
    private void ClearCrudPageContextBindings()
    {
        foreach (DependencyProperty dependencyProperty in this.autoCrudPageContextBoundProperties.ToArray())
        {
            BindingOperations.ClearBinding(this, dependencyProperty);
            this.ClearValue(dependencyProperty);
        }

        this.autoCrudPageContextBoundProperties.Clear();
    }

    /// <summary>
    /// Applies a binding from the current CRUD context unless the property is already explicitly configured.
    /// </summary>
    /// <param name="dependencyProperty">The target dependency property.</param>
    /// <param name="path">The source binding path.</param>
    /// <param name="mode">The binding mode.</param>
    /// <param name="source">The source CRUD context.</param>
    private void SetCrudContextBinding(DependencyProperty dependencyProperty, string path, BindingMode mode, XCrudContext source)
    {
        this.SetAutomaticBinding(dependencyProperty, path, mode, source, this.autoCrudContextBoundProperties);
    }

    /// <summary>
    /// Applies a value from the current CRUD page context unless the property is already explicitly configured.
    /// </summary>
    /// <param name="dependencyProperty">The target dependency property.</param>
    /// <param name="value">The value to apply.</param>
    private void SetCrudPageContextValue(DependencyProperty dependencyProperty, object? value)
    {
        ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this, dependencyProperty);
        bool isDefaultValue = valueSource.BaseValueSource == BaseValueSource.Default && !valueSource.IsExpression;
        bool isAutoBound = this.autoCrudPageContextBoundProperties.Contains(dependencyProperty);

        if (!isDefaultValue && !isAutoBound)
        {
            return;
        }

        this.SetValue(dependencyProperty, value);
        this.autoCrudPageContextBoundProperties.Add(dependencyProperty);
    }

    /// <summary>
    /// Applies an automatic binding unless the target property is already explicitly configured.
    /// </summary>
    /// <param name="dependencyProperty">The target dependency property.</param>
    /// <param name="path">The source binding path.</param>
    /// <param name="mode">The binding mode.</param>
    /// <param name="source">The binding source.</param>
    /// <param name="automaticBindings">The set of automatically managed properties.</param>
    private void SetAutomaticBinding(DependencyProperty dependencyProperty, string path, BindingMode mode, object source, HashSet<DependencyProperty> automaticBindings)
    {
        ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this, dependencyProperty);
        bool isDefaultValue = valueSource.BaseValueSource == BaseValueSource.Default && !valueSource.IsExpression;
        bool isAutoBound = automaticBindings.Contains(dependencyProperty);

        if (!isDefaultValue && !isAutoBound)
        {
            return;
        }

        BindingOperations.SetBinding(
            this,
            dependencyProperty,
            new Binding(path)
            {
                Mode = mode,
                Source = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        automaticBindings.Add(dependencyProperty);
    }

    /// <summary>
    /// Validates whether a required CRUD context is available or detail handling was configured manually.
    /// </summary>
    private void ValidateCrudContextRequirement()
    {
        if (!this.RequireCrudContext || !this.IsDetailOpen || this.GetEffectiveCrudContext() is not null || this.HasManualDetailConfiguration())
        {
            return;
        }

        throw new InvalidOperationException(
            "XViewContainer requires a CrudContext for automatic dialog handling. Set CrudContext, implement IXCrudPageContext on the DataContext, or set RequireCrudContext to false when using fully manual detail bindings.");
    }

    /// <summary>
    /// Gets whether detail handling is explicitly configured without automatic CRUD context bindings.
    /// </summary>
    /// <returns><c>true</c> if manual detail configuration is present; otherwise, <c>false</c>.</returns>
    private bool HasManualDetailConfiguration()
    {
        return this.HasManualValue(DetailHostProperty)
               || this.HasManualValue(DetailHeaderProperty)
               || this.HasManualValue(DetailFooterProperty)
               || this.HasManualValue(IsDetailOpenProperty)
               || this.HasManualValue(CloseDetailCommandProperty)
               || this.HasManualValue(PrimaryDetailCommandProperty);
    }

    /// <summary>
    /// Gets whether the specified dependency property has a non-default value that was not created by automatic CRUD integration.
    /// </summary>
    /// <param name="dependencyProperty">The dependency property.</param>
    /// <returns><c>true</c> if the value is manually configured; otherwise, <c>false</c>.</returns>
    private bool HasManualValue(DependencyProperty dependencyProperty)
    {
        ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this, dependencyProperty);
        bool isDefaultValue = valueSource.BaseValueSource == BaseValueSource.Default && !valueSource.IsExpression;

        return !isDefaultValue
               && !this.autoCrudContextBoundProperties.Contains(dependencyProperty)
               && !this.autoCrudPageContextBoundProperties.Contains(dependencyProperty);
    }

    /// <summary>
    /// Determines whether the specified value should be treated as visible content.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value is meaningful; otherwise, <c>false</c>.</returns>
    private static bool HasMeaningfulContent(object? value)
    {
        return value switch
        {
            null => false,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => true,
        };
    }

    /// <summary>
    /// Handles data context changes and refreshes automatic CRUD integration.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
    {
        this.ApplyResolvedCrudIntegration();
    }

    /// <summary>
    /// Handles loading of the container and synchronizes the navigation lock with the current visual tree.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnLoaded(object sender, RoutedEventArgs eventArgs)
    {
        this.ApplyResolvedCrudIntegration();
        this.UpdateNavigationLock();
    }

    /// <summary>
    /// Handles unloading of the container and releases any active navigation lock.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnUnloaded(object sender, RoutedEventArgs eventArgs)
    {
        this.HideWindowEditorOverlay();
        this.ReleaseNavigationLock();
    }

    /// <summary>
    /// Updates the window-wide navigation lock requested by this container.
    /// </summary>
    private void UpdateNavigationLock()
    {
        this.ReleaseNavigationLock();
    }

    /// <summary>
    /// Releases the window-wide navigation lock requested by this container.
    /// </summary>
    private void ReleaseNavigationLock()
    {
        if (this.navigationLockedWindow is null)
        {
            return;
        }

        XNavigationLock.ReleaseLock(this.navigationLockedWindow);
        this.navigationLockedWindow = null;
    }

    /// <summary>
    /// Updates the visual state of the detail area.
    /// </summary>
    /// <param name="useTransitions">if set to <c>true</c>, opening and closing is animated.</param>
    private void UpdateDetailOpenState(bool useTransitions)
    {
        if (!this.IsDetailOpen)
        {
            this.HideWindowEditorOverlay();
        }
        else if (this.TryShowWindowEditorOverlay())
        {
            this.HideLocalDetailLayer();
            return;
        }

        if (this.detailLayer is null || this.detailBorder is null || this.overlay is null)
        {
            return;
        }

        (TranslateTransform translateTransform, ScaleTransform scaleTransform) = this.EnsureDetailTransforms();
        double offset = this.GetEffectiveAnimationOffset();
        double startScale = this.GetEffectiveAnimationScale();

        if (!useTransitions || !this.ShouldAnimateDetailTransition())
        {
            this.StopDetailAnimations();
            this.detailLayer.Visibility = this.IsDetailOpen ? Visibility.Visible : Visibility.Collapsed;
            this.overlay.Opacity = this.IsDetailOpen ? this.OverlayOpacity : 0d;
            this.detailBorder.Opacity = this.IsDetailOpen ? 1d : 0d;
            translateTransform.Y = this.IsDetailOpen || !this.UsesSlideAnimation() ? 0d : -offset;
            scaleTransform.ScaleX = this.IsDetailOpen || !this.UsesZoomAnimation() ? 1d : startScale;
            scaleTransform.ScaleY = this.IsDetailOpen || !this.UsesZoomAnimation() ? 1d : startScale;
            return;
        }

        if (this.IsDetailOpen)
        {
            this.detailLayer.Visibility = Visibility.Visible;
            this.AnimateOpen(offset, startScale, translateTransform, scaleTransform);
            return;
        }

        this.AnimateClose(offset, startScale, translateTransform, scaleTransform);
    }

    /// <summary>
    /// Hides the local detail layer while the detail editor is hosted by the owning window.
    /// </summary>
    private void HideLocalDetailLayer()
    {
        this.StopDetailAnimations();

        if (this.detailLayer is not null)
        {
            this.detailLayer.Visibility = Visibility.Collapsed;
        }

        if (this.overlay is not null)
        {
            this.overlay.Opacity = 0d;
        }

        if (this.detailBorder is not null)
        {
            this.detailBorder.Opacity = 0d;
        }
    }

    /// <summary>
    /// Tries to display the current detail editor through the owning window overlay host.
    /// </summary>
    /// <returns><c>true</c> if a window overlay host accepted the request; otherwise <c>false</c>.</returns>
    private bool TryShowWindowEditorOverlay()
    {
        if (!this.ShouldUseWindowEditorOverlay())
        {
            this.HideWindowEditorOverlay();
            return false;
        }

        Window? targetWindow = Window.GetWindow(this);
        if (targetWindow is null)
        {
            this.HideWindowEditorOverlay();
            return false;
        }

        this.SetValue(IsDetailHostedByWindowPropertyKey, true);

        ShowEditorOverlayMessage message = new(
            this,
            targetWindow,
            this.DetailHeader,
            this.ResolveContentTemplate(this.DetailHeader, this.DetailHeaderTemplate),
            this.DetailHost,
            this.ResolveContentTemplate(this.DetailHost, this.DetailHostTemplate),
            this.DetailFooter,
            this.ResolveContentTemplate(this.DetailFooter, this.DetailFooterTemplate),
            this.EffectiveValidationSource,
            this.ShowValidationHint,
            this.OverlayBackground,
            this.OverlayOpacity,
            this.OverlayCornerRadius,
            this.DetailBackground,
            this.DetailBorderBrush,
            this.DetailBorderThickness,
            this.DetailCornerRadius,
            this.DetailWidth,
            this.DetailMinWidth,
            this.DetailMaxWidth,
            this.DetailMinHeight,
            this.DetailMaxHeight,
            this.DetailMargin,
            this.DetailPadding,
            this.DetailHeaderPadding,
            this.DetailFooterPadding,
            this.ShowDetailCloseButton,
            this.IsModal,
            this.ShowDefaultDetailFooter && !HasMeaningfulContent(this.DetailFooter),
            this.PrimaryDetailCommand,
            this.PrimaryDetailCommandParameter,
            this.PrimaryDetailText,
            this.CancelDetailText,
            this.CloseDetail,
            this.CanCloseOnWindowOverlayClick);

        WeakReferenceMessenger.Default.Send(message);

        if (!message.Handled)
        {
            this.SetValue(IsDetailHostedByWindowPropertyKey, false);
            this.editorOverlayHostWindow = null;
            return false;
        }

        this.editorOverlayHostWindow = targetWindow;
        return true;
    }

    /// <summary>
    /// Updates the window-hosted overlay if the detail editor is currently hosted by the owning window.
    /// </summary>
    private void UpdateWindowEditorOverlayIfHosted()
    {
        if (this.IsDetailHostedByWindow && this.IsDetailOpen)
        {
            this.TryShowWindowEditorOverlay();
        }
    }

    /// <summary>
    /// Hides a currently hosted window editor overlay.
    /// </summary>
    private void HideWindowEditorOverlay()
    {
        if (!this.IsDetailHostedByWindow && this.editorOverlayHostWindow is null)
        {
            return;
        }

        Window? targetWindow = this.editorOverlayHostWindow ?? Window.GetWindow(this);
        HideEditorOverlayMessage message = new(this, targetWindow);
        WeakReferenceMessenger.Default.Send(message);

        this.editorOverlayHostWindow = null;
        this.SetValue(IsDetailHostedByWindowPropertyKey, false);
    }

    /// <summary>
    /// Gets whether the current detail editor should be shown through the owning window overlay host.
    /// </summary>
    /// <returns><c>true</c> if window-hosting should be attempted; otherwise <c>false</c>.</returns>
    private bool ShouldUseWindowEditorOverlay()
    {
        return this.IsDetailOpen
               && this.UseWindowEditorOverlay
               && this.DetailPresentation == XViewDetailPresentation.Dialog;
    }

    /// <summary>
    /// Gets whether the owning window overlay may currently close the detail editor.
    /// </summary>
    /// <returns><c>true</c> if an overlay click may close the editor; otherwise <c>false</c>.</returns>
    private bool CanCloseOnWindowOverlayClick()
    {
        return this.IsDetailOpen
               && !this.IsModal
               && this.CloseOnOverlayClick
               && !this.ShouldSuppressOverlayClose();
    }

    /// <summary>
    /// Resolves an explicit or local implicit data template for the specified content.
    /// </summary>
    /// <param name="content">The content to display.</param>
    /// <param name="explicitTemplate">The explicitly configured template.</param>
    /// <returns>The resolved template or <see langword="null" />.</returns>
    private DataTemplate? ResolveContentTemplate(object? content, DataTemplate? explicitTemplate)
    {
        if (explicitTemplate is not null || content is null || content is FrameworkElement)
        {
            return explicitTemplate;
        }

        return this.TryFindResource(new DataTemplateKey(content.GetType())) as DataTemplate;
    }

    /// <summary>
    /// Starts the opening animation of the detail area.
    /// </summary>
    /// <param name="offset">The animation start offset.</param>
    /// <param name="startScale">The animation start scale.</param>
    /// <param name="translateTransform">The translate transform of the detail border.</param>
    /// <param name="scaleTransform">The scale transform of the detail border.</param>
    private void AnimateOpen(double offset, double startScale, TranslateTransform translateTransform, ScaleTransform scaleTransform)
    {
        this.StopDetailAnimations();

        bool useSlide = this.UsesSlideAnimation();
        bool useZoom = this.UsesZoomAnimation();
        Duration duration = this.GetEffectiveAnimationDuration();

        if (this.overlay != null)
        {
            this.overlay.Opacity = 0d;
        }

        if (this.detailBorder != null)
        {
            this.detailBorder.Opacity = 0d;
        }

        translateTransform.Y = useSlide ? -offset : 0d;
        scaleTransform.ScaleX = useZoom ? startScale : 1d;
        scaleTransform.ScaleY = useZoom ? startScale : 1d;

        DoubleAnimation overlayAnimation = new()
        {
            From = 0d,
            To = XThemeManager.Current.CurrentMode == XThemeMode.Dark ? this.OverlayOpacity + 0.3d : this.OverlayOpacity,
            Duration = duration,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        DoubleAnimation borderOpacityAnimation = new()
        {
            From = 0d,
            To = 1d,
            Duration = duration,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        this.overlay?.BeginAnimation(UIElement.OpacityProperty, overlayAnimation, HandoffBehavior.SnapshotAndReplace);
        this.detailBorder?.BeginAnimation(UIElement.OpacityProperty, borderOpacityAnimation, HandoffBehavior.SnapshotAndReplace);

        if (useSlide)
        {
            DoubleAnimation translateAnimation = new()
            {
                From = -offset,
                To = 0d,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        if (useZoom)
        {
            DoubleAnimation scaleAnimation = new()
            {
                From = startScale,
                To = 1d,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation, HandoffBehavior.SnapshotAndReplace);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation, HandoffBehavior.SnapshotAndReplace);
        }
    }

    /// <summary>
    /// Starts the closing animation of the detail area.
    /// </summary>
    /// <param name="offset">The animation offset.</param>
    /// <param name="endScale">The animation end scale.</param>
    /// <param name="translateTransform">The translate transform of the detail border.</param>
    /// <param name="scaleTransform">The scale transform of the detail border.</param>
    private void AnimateClose(double offset, double endScale, TranslateTransform translateTransform, ScaleTransform scaleTransform)
    {
        this.StopDetailAnimations();

        bool useSlide = this.UsesSlideAnimation();
        bool useZoom = this.UsesZoomAnimation();
        Duration duration = this.GetEffectiveAnimationDuration();

        DoubleAnimation overlayAnimation = new()
        {
            From = this.overlay?.Opacity ?? 0d,
            To = 0d,
            Duration = duration,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };

        DoubleAnimation borderOpacityAnimation = new()
        {
            From = this.detailBorder?.Opacity ?? 0d,
            To = 0d,
            Duration = duration,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };

        overlayAnimation.Completed += (_, _) =>
        {
            if (!this.IsDetailOpen && this.detailLayer is not null)
            {
                this.detailLayer.Visibility = Visibility.Collapsed;
            }
        };

        this.overlay?.BeginAnimation(UIElement.OpacityProperty, overlayAnimation, HandoffBehavior.SnapshotAndReplace);
        this.detailBorder?.BeginAnimation(UIElement.OpacityProperty, borderOpacityAnimation, HandoffBehavior.SnapshotAndReplace);

        if (useSlide)
        {
            DoubleAnimation translateAnimation = new()
            {
                From = translateTransform.Y,
                To = -offset,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        if (useZoom)
        {
            DoubleAnimation scaleAnimation = new()
            {
                From = scaleTransform.ScaleX,
                To = endScale,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation, HandoffBehavior.SnapshotAndReplace);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation, HandoffBehavior.SnapshotAndReplace);
        }
    }

    /// <summary>
    /// Stops all currently running detail animations.
    /// </summary>
    private void StopDetailAnimations()
    {
        this.overlay?.BeginAnimation(UIElement.OpacityProperty, null);
        this.detailBorder?.BeginAnimation(UIElement.OpacityProperty, null);

        if (this.detailBorder is null)
        {
            return;
        }

        (TranslateTransform translateTransform, ScaleTransform scaleTransform) = this.EnsureDetailTransforms();
        translateTransform.BeginAnimation(TranslateTransform.YProperty, null);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
    }

    /// <summary>
    /// Ensures that the detail border has mutable translate and scale transforms.
    /// </summary>
    /// <returns>The translate and scale transforms used by the detail border.</returns>
    private (TranslateTransform TranslateTransform, ScaleTransform ScaleTransform) EnsureDetailTransforms()
    {
        if (this.detailBorder is null)
        {
            return (new TranslateTransform(), new ScaleTransform(1d, 1d));
        }

        TransformGroup transformGroup = this.EnsureMutableDetailTransformGroup();
        ScaleTransform? scaleTransform = null;
        TranslateTransform? translateTransform = null;

        foreach (Transform transform in transformGroup.Children)
        {
            if (transform is ScaleTransform currentScaleTransform && scaleTransform is null)
            {
                scaleTransform = currentScaleTransform;
                continue;
            }

            if (transform is TranslateTransform currentTranslateTransform && translateTransform is null)
            {
                translateTransform = currentTranslateTransform;
            }
        }

        if (scaleTransform is null)
        {
            scaleTransform = new ScaleTransform(1d, 1d);
            transformGroup.Children.Insert(0, scaleTransform);
        }

        if (translateTransform is null)
        {
            translateTransform = new TranslateTransform();
            transformGroup.Children.Add(translateTransform);
        }

        return (translateTransform, scaleTransform);
    }

    /// <summary>
    /// Ensures that the detail border render transform is a mutable <see cref="TransformGroup"/>.
    /// </summary>
    /// <returns>The mutable transform group.</returns>
    private TransformGroup EnsureMutableDetailTransformGroup()
    {
        if (this.detailBorder?.RenderTransform is TransformGroup existingTransformGroup)
        {
            if (!existingTransformGroup.IsFrozen)
            {
                return existingTransformGroup;
            }

            TransformGroup clonedTransformGroup = existingTransformGroup.Clone();
            this.detailBorder.RenderTransform = clonedTransformGroup;
            return clonedTransformGroup;
        }

        Transform? existingTransform = this.detailBorder?.RenderTransform;
        TransformGroup transformGroup = new();

        if (existingTransform is ScaleTransform scaleTransform && !scaleTransform.IsFrozen)
        {
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(new TranslateTransform());
        }
        else if (existingTransform is TranslateTransform translateTransform && !translateTransform.IsFrozen)
        {
            transformGroup.Children.Add(new ScaleTransform(1d, 1d));
            transformGroup.Children.Add(translateTransform);
        }
        else
        {
            transformGroup.Children.Add(new ScaleTransform(1d, 1d));
            transformGroup.Children.Add(new TranslateTransform());
        }

        if (this.detailBorder is not null)
        {
            this.detailBorder.RenderTransform = transformGroup;
        }

        return transformGroup;
    }

    /// <summary>
    /// Gets a value indicating whether detail transitions should currently be animated.
    /// </summary>
    /// <returns><c>true</c> if transitions should be animated; otherwise, <c>false</c>.</returns>
    private bool ShouldAnimateDetailTransition()
    {
        return this.EnableDetailAnimation &&
               this.DetailAnimation != XViewDetailAnimation.None &&
               this.GetEffectiveAnimationDuration().HasTimeSpan &&
               this.GetEffectiveAnimationDuration().TimeSpan > TimeSpan.Zero;
    }

    /// <summary>
    /// Gets a value indicating whether the selected animation uses a slide transform.
    /// </summary>
    /// <returns><c>true</c> if slide is used; otherwise, <c>false</c>.</returns>
    private bool UsesSlideAnimation()
    {
        return this.DetailAnimation is XViewDetailAnimation.Slide or XViewDetailAnimation.SlideZoom;
    }

    /// <summary>
    /// Gets a value indicating whether the selected animation uses a zoom transform.
    /// </summary>
    /// <returns><c>true</c> if zoom is used; otherwise, <c>false</c>.</returns>
    private bool UsesZoomAnimation()
    {
        return this.DetailAnimation is XViewDetailAnimation.Zoom or XViewDetailAnimation.SlideZoom;
    }

    /// <summary>
    /// Gets the effective vertical animation offset.
    /// </summary>
    /// <returns>The configured animation offset or a safe fallback value.</returns>
    private double GetEffectiveAnimationOffset()
    {
        if (double.IsNaN(this.DetailAnimationOffset) || double.IsInfinity(this.DetailAnimationOffset))
        {
            return 18d;
        }

        return Math.Max(0d, this.DetailAnimationOffset);
    }

    /// <summary>
    /// Gets the effective zoom animation start scale.
    /// </summary>
    /// <returns>The configured animation scale or a safe fallback value.</returns>
    private double GetEffectiveAnimationScale()
    {
        if (double.IsNaN(this.DetailAnimationScale) || double.IsInfinity(this.DetailAnimationScale))
        {
            return 0.96d;
        }

        return Math.Clamp(this.DetailAnimationScale, 0.75d, 1d);
    }

    /// <summary>
    /// Gets the effective animation duration.
    /// </summary>
    /// <returns>The configured animation duration or a fallback duration.</returns>
    private Duration GetEffectiveAnimationDuration()
    {
        if (this.DetailAnimationDuration.HasTimeSpan && this.DetailAnimationDuration.TimeSpan >= TimeSpan.Zero)
        {
            return this.DetailAnimationDuration;
        }

        return new Duration(TimeSpan.FromMilliseconds(220d));
    }

    /// <summary>
    /// Handles clicks on the overlay area.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnOverlayMouseLeftButtonUp(object sender, MouseButtonEventArgs eventArgs)
    {
        if (this.ShouldSuppressOverlayClose())
        {
            eventArgs.Handled = true;
            return;
        }

        if (!this.IsDetailOpen || this.IsModal || !this.CloseOnOverlayClick)
        {
            return;
        }

        this.CloseDetail();
        eventArgs.Handled = true;
    }

    /// <summary>
    /// Determines whether an overlay close click should be suppressed because the detail area has just been opened.
    /// </summary>
    /// <returns><c>true</c> if the overlay close should be suppressed; otherwise, <c>false</c>.</returns>
    private bool ShouldSuppressOverlayClose()
    {
        if (!this.IsDetailOpen || this.detailOpenedAtUtc == DateTime.MinValue)
        {
            return false;
        }

        return (DateTime.UtcNow - this.detailOpenedAtUtc).TotalMilliseconds < OverlayClickCloseSuppressionMilliseconds;
    }

    /// <summary>
    /// Handles clicks on the built-in cancel detail button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnCancelDetailButtonClick(object sender, RoutedEventArgs eventArgs)
    {
        this.CloseDetail();
        eventArgs.Handled = true;
    }

    /// <summary>
    /// Handles clicks on the close button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnCloseButtonClick(object sender, RoutedEventArgs eventArgs)
    {
        this.CloseDetail();
        eventArgs.Handled = true;
    }

    /// <summary>
    /// Closes the detail area directly or delegates the close operation to <see cref="CloseDetailCommand"/>.
    /// </summary>
    private void CloseDetail()
    {
        XCrudContext? crudContext = this.GetEffectiveCrudContext();
        object? commandParameter = this.CloseDetailCommandParameter ?? (object?)crudContext ?? this;

        if (this.CloseDetailCommand is null)
        {
            if (crudContext is not null)
            {
                crudContext.Close();
                return;
            }

            this.IsDetailOpen = false;
            return;
        }

        if (this.CloseDetailCommand.CanExecute(commandParameter))
        {
            this.CloseDetailCommand.Execute(commandParameter);
        }
    }
    #endregion
}
#endregion
