// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindow.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Messaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VIA.WPF.Controls.Navigation;
using VIA.WPF.Localization;
using VIA.WPF.Themes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using ScrollBar = System.Windows.Controls.Primitives.ScrollBar;
using TextBoxBase = System.Windows.Controls.Primitives.TextBoxBase;

namespace VIA.WPF.Windowing;

#region ### Enum XWindowAnimationMode ###
/// <summary>
/// Defines the supported window startup and close animation modes.
/// </summary>
public enum XWindowAnimationMode
{
    /// <summary>
    /// No animation is applied.
    /// </summary>
    None,

    /// <summary>
    /// Applies a fade animation.
    /// </summary>
    Fade,

    /// <summary>
    /// Applies a fade animation combined with a subtle scale animation.
    /// </summary>
    FadeAndScale
}
#endregion

#region ### Enum XToastPlacement ###
/// <summary>
/// Defines the supported toast placement positions.
/// </summary>
public enum XToastPlacement
{
    /// <summary>
    /// Shows the toast in the top right corner.
    /// </summary>
    TopRight,

    /// <summary>
    /// Shows the toast in the bottom right corner.
    /// </summary>
    BottomRight,

    /// <summary>
    /// Shows the toast centered in the window.
    /// </summary>
    Center
}
#endregion

#region ### Class XWindow ###
/// <summary>
/// Represents the base window of VIA.WPF.
/// </summary>
public class XWindow : Window, IXModalOverlayHost
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="TitleBarContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
        nameof(TitleBarContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TitleBarContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarContentTemplateProperty = DependencyProperty.Register(
        nameof(TitleBarContentTemplate),
        typeof(DataTemplate),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Subtitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(XWindow),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="TitleBarHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
        nameof(TitleBarHeight),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(35d));

    /// <summary>
    /// Identifies the <see cref="TitleFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
        nameof(TitleFontSize),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(13d));

    /// <summary>
    /// Identifies the <see cref="TitleBarPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarPaddingProperty = DependencyProperty.Register(
        nameof(TitleBarPadding),
        typeof(Thickness),
        typeof(XWindow),
        new PropertyMetadata(new Thickness(12d, 0d, 0d, 0d)));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XWindow),
        new PropertyMetadata(new CornerRadius(6d, 6d, 4d, 4d), OnCornerRadiusChanged));

    /// <summary>
    /// Identifies the read-only <see cref="TopCornerRadius"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey TopCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(TopCornerRadius),
        typeof(CornerRadius),
        typeof(XWindow),
        new PropertyMetadata(default(CornerRadius)));

    /// <summary>
    /// Identifies the <see cref="TopCornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TopCornerRadiusProperty = TopCornerRadiusPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="ShowThemeSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowThemeSelectorProperty = DependencyProperty.Register(
        nameof(ShowThemeSelector),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ShowLanguageSelector"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowLanguageSelectorProperty = DependencyProperty.Register(
        nameof(ShowLanguageSelector),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="AvailableLanguages"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AvailableLanguagesProperty = DependencyProperty.Register(
        nameof(AvailableLanguages),
        typeof(IEnumerable<XLanguage>),
        typeof(XWindow),
        new PropertyMetadata(XLanguages.Default));

    /// <summary>
    /// Identifies the <see cref="SelectedLanguage"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedLanguageProperty = DependencyProperty.Register(
        nameof(SelectedLanguage),
        typeof(XLanguage),
        typeof(XWindow),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnLanguageConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ApplyLanguageFormattingCulture"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ApplyLanguageFormattingCultureProperty = DependencyProperty.Register(
        nameof(ApplyLanguageFormattingCulture),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true, OnLanguageConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowThemeModeButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowThemeModeButtonProperty = DependencyProperty.Register(
        nameof(ShowThemeModeButton),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowMinimizeButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(
        nameof(ShowMinimizeButton),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowMaximizeButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(
        nameof(ShowMaximizeButton),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowCloseButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
        nameof(ShowCloseButton),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowResizeGrip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResizeGripProperty = DependencyProperty.Register(
        nameof(ShowResizeGrip),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="WindowBackgroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowBackgroundBrushProperty = DependencyProperty.Register(
        nameof(WindowBackgroundBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(245, 247, 250)));

    /// <summary>
    /// Identifies the <see cref="WindowForegroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowForegroundBrushProperty = DependencyProperty.Register(
        nameof(WindowForegroundBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(17, 24, 39)));

    /// <summary>
    /// Identifies the <see cref="WindowBorderBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowBorderBrushProperty = DependencyProperty.Register(
        nameof(WindowBorderBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(203, 213, 225)));

    /// <summary>
    /// Identifies the <see cref="TitleBarBackgroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarBackgroundBrushProperty = DependencyProperty.Register(
        nameof(TitleBarBackgroundBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(Brushes.White));

    /// <summary>
    /// Identifies the <see cref="TitleBarForegroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleBarForegroundBrushProperty = DependencyProperty.Register(
        nameof(TitleBarForegroundBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(17, 24, 39)));

    /// <summary>
    /// Identifies the <see cref="CaptionButtonHoverBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaptionButtonHoverBrushProperty = DependencyProperty.Register(
        nameof(CaptionButtonHoverBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(241, 245, 249)));

    /// <summary>
    /// Identifies the <see cref="CaptionButtonPressedBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaptionButtonPressedBrushProperty = DependencyProperty.Register(
        nameof(CaptionButtonPressedBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(226, 232, 240)));

    /// <summary>
    /// Identifies the <see cref="CloseButtonHoverBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseButtonHoverBrushProperty = DependencyProperty.Register(
        nameof(CloseButtonHoverBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(232, 17, 35)));

    /// <summary>
    /// Identifies the <see cref="CloseButtonPressedBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseButtonPressedBrushProperty = DependencyProperty.Register(
        nameof(CloseButtonPressedBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(197, 15, 31)));

    /// <summary>
    /// Identifies the <see cref="CloseButtonForegroundBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseButtonForegroundBrushProperty = DependencyProperty.Register(
        nameof(CloseButtonForegroundBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(Brushes.Black));

    /// <summary>
    /// Identifies the <see cref="StatusBarContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StatusBarContentProperty = DependencyProperty.Register(
        nameof(StatusBarContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="StatusBarContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StatusBarContentTemplateProperty = DependencyProperty.Register(
        nameof(StatusBarContentTemplate),
        typeof(DataTemplate),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsBusy"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
        nameof(IsBusy),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="BusyContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
        nameof(BusyContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata("Bitte warten..."));

    /// <summary>
    /// Identifies the <see cref="BusyContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
        nameof(BusyContentTemplate),
        typeof(DataTemplate),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="BusyOverlayBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BusyOverlayBrushProperty = DependencyProperty.Register(
        nameof(BusyOverlayBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(140, 15, 23, 42)));

    /// <summary>
    /// Identifies the <see cref="ModalOverlayBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModalOverlayBrushProperty = DependencyProperty.Register(
        nameof(ModalOverlayBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(120, 15, 23, 42)));

    /// <summary>
    /// Identifies the read-only <see cref="IsModalOverlayOpen"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey IsModalOverlayOpenPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsModalOverlayOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="IsModalOverlayOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsModalOverlayOpenProperty =
        IsModalOverlayOpenPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="FlyoutOverlayBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FlyoutOverlayBrushProperty = DependencyProperty.Register(
        nameof(FlyoutOverlayBrush),
        typeof(Brush),
        typeof(XWindow),
        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(120, 15, 23, 42)));

    /// <summary>
    /// Identifies the <see cref="CloseFlyoutsOnOverlayClick"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseFlyoutsOnOverlayClickProperty = DependencyProperty.Register(
        nameof(CloseFlyoutsOnOverlayClick),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="LeftFlyoutContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeftFlyoutContentProperty = DependencyProperty.Register(
        nameof(LeftFlyoutContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="RightFlyoutContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RightFlyoutContentProperty = DependencyProperty.Register(
        nameof(RightFlyoutContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="TopFlyoutContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TopFlyoutContentProperty = DependencyProperty.Register(
        nameof(TopFlyoutContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="BottomFlyoutContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BottomFlyoutContentProperty = DependencyProperty.Register(
        nameof(BottomFlyoutContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsLeftFlyoutOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsLeftFlyoutOpenProperty = DependencyProperty.Register(
        nameof(IsLeftFlyoutOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsRightFlyoutOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsRightFlyoutOpenProperty = DependencyProperty.Register(
        nameof(IsRightFlyoutOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsTopFlyoutOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsTopFlyoutOpenProperty = DependencyProperty.Register(
        nameof(IsTopFlyoutOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsBottomFlyoutOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsBottomFlyoutOpenProperty = DependencyProperty.Register(
        nameof(IsBottomFlyoutOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="LeftFlyoutWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeftFlyoutWidthProperty = DependencyProperty.Register(
        nameof(LeftFlyoutWidth),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(320d));

    /// <summary>
    /// Identifies the <see cref="RightFlyoutWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RightFlyoutWidthProperty = DependencyProperty.Register(
        nameof(RightFlyoutWidth),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(320d));

    /// <summary>
    /// Identifies the <see cref="TopFlyoutHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TopFlyoutHeightProperty = DependencyProperty.Register(
        nameof(TopFlyoutHeight),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(240d));

    /// <summary>
    /// Identifies the <see cref="BottomFlyoutHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BottomFlyoutHeightProperty = DependencyProperty.Register(
        nameof(BottomFlyoutHeight),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(240d));

    /// <summary>
    /// Identifies the <see cref="ToastContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastContentProperty = DependencyProperty.Register(
        nameof(ToastContent),
        typeof(object),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ToastContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastContentTemplateProperty = DependencyProperty.Register(
        nameof(ToastContentTemplate),
        typeof(DataTemplate),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsToastOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsToastOpenProperty = DependencyProperty.Register(
        nameof(IsToastOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ToastPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastPlacementProperty = DependencyProperty.Register(
        nameof(ToastPlacement),
        typeof(XToastPlacement),
        typeof(XWindow),
        new PropertyMetadata(XToastPlacement.TopRight));

    /// <summary>
    /// Identifies the <see cref="UseAnimations"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UseAnimationsProperty = DependencyProperty.Register(
        nameof(UseAnimations),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="FlyoutAnimationDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FlyoutAnimationDurationProperty = DependencyProperty.Register(
        nameof(FlyoutAnimationDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(220))));

    /// <summary>
    /// Identifies the <see cref="FlyoutAnimationOffset"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FlyoutAnimationOffsetProperty = DependencyProperty.Register(
        nameof(FlyoutAnimationOffset),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(24d));

    /// <summary>
    /// Identifies the <see cref="FlyoutUseFade"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FlyoutUseFadeProperty = DependencyProperty.Register(
        nameof(FlyoutUseFade),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ToastAnimationDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastAnimationDurationProperty = DependencyProperty.Register(
        nameof(ToastAnimationDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(180))));

    /// <summary>
    /// Identifies the <see cref="ToastAnimationOffset"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastAnimationOffsetProperty = DependencyProperty.Register(
        nameof(ToastAnimationOffset),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(18d));

    /// <summary>
    /// Identifies the <see cref="ToastUseFade"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastUseFadeProperty = DependencyProperty.Register(
        nameof(ToastUseFade),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="StartupAnimation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StartupAnimationProperty = DependencyProperty.Register(
        nameof(StartupAnimation),
        typeof(XWindowAnimationMode),
        typeof(XWindow),
        new PropertyMetadata(XWindowAnimationMode.None));

    /// <summary>
    /// Identifies the <see cref="StartupAnimationDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StartupAnimationDurationProperty = DependencyProperty.Register(
        nameof(StartupAnimationDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(180))));

    /// <summary>
    /// Identifies the <see cref="StartupAnimationScaleDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StartupAnimationScaleDurationProperty = DependencyProperty.Register(
        nameof(StartupAnimationScaleDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(Duration.Automatic));

    /// <summary>
    /// Identifies the <see cref="StartupAnimationInitialScale"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StartupAnimationInitialScaleProperty = DependencyProperty.Register(
        nameof(StartupAnimationInitialScale),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(0.985d));

    /// <summary>
    /// Identifies the <see cref="CloseAnimation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseAnimationProperty = DependencyProperty.Register(
        nameof(CloseAnimation),
        typeof(XWindowAnimationMode),
        typeof(XWindow),
        new PropertyMetadata(XWindowAnimationMode.None));

    /// <summary>
    /// Identifies the <see cref="CloseAnimationDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseAnimationDurationProperty = DependencyProperty.Register(
        nameof(CloseAnimationDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(140))));

    /// <summary>
    /// Identifies the <see cref="CloseAnimationScaleDuration"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseAnimationScaleDurationProperty = DependencyProperty.Register(
        nameof(CloseAnimationScaleDuration),
        typeof(Duration),
        typeof(XWindow),
        new PropertyMetadata(Duration.Automatic));

    /// <summary>
    /// Identifies the <see cref="CloseAnimationTargetScale"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseAnimationTargetScaleProperty = DependencyProperty.Register(
        nameof(CloseAnimationTargetScale),
        typeof(double),
        typeof(XWindow),
        new PropertyMetadata(0.99d));


    /// <summary>
    /// Identifies the <see cref="EditorOverlayMessage" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditorOverlayMessageProperty = DependencyProperty.Register(
        nameof(EditorOverlayMessage),
        typeof(ShowEditorOverlayMessage),
        typeof(XWindow),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsEditorOverlayOpen" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEditorOverlayOpenProperty = DependencyProperty.Register(
        nameof(IsEditorOverlayOpen),
        typeof(bool),
        typeof(XWindow),
        new PropertyMetadata(false));
    #endregion

    #region ### Private Fields ###
    private FrameworkElement? _titleBarElement;
    private Thumb? _resizeGripThumb;
    private Thumb? _leftResizeThumb;
    private Thumb? _topResizeThumb;
    private Thumb? _rightResizeThumb;
    private Thumb? _bottomResizeThumb;
    private Thumb? _topLeftResizeThumb;
    private Thumb? _topRightResizeThumb;
    private Thumb? _bottomRightResizeThumb;
    private Thumb? _bottomLeftResizeThumb;
    private HwndSource? _hwndSource;
    private Grid? _flyoutLayer;
    private Border? _flyoutOverlay;
    private Border? _leftFlyoutElement;
    private Border? _rightFlyoutElement;
    private Border? _topFlyoutElement;
    private Border? _bottomFlyoutElement;
    private TranslateTransform? _leftFlyoutTransform;
    private TranslateTransform? _rightFlyoutTransform;
    private TranslateTransform? _topFlyoutTransform;
    private TranslateTransform? _bottomFlyoutTransform;
    private Border? _windowFrameElement;
    private Border? _editorOverlayOverlay;
    private ButtonBase? _editorOverlayCloseButton;
    private ButtonBase? _editorOverlayCancelButton;
    private ScaleTransform? _windowFrameScaleTransform;
    private bool _isStartupAnimationPrepared;
    private bool _isCloseAnimationRunning;
    private bool _isCloseAnimationCompleted;
    private bool? _pendingDialogResult;
    private int _modalOverlayDepth;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XWindow"/> class.
    /// </summary>
    static XWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XWindow),
            new FrameworkPropertyMetadata(typeof(XWindow)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XWindow"/> class.
    /// </summary>
    public XWindow()
    {
        this.WindowStyle = WindowStyle.None;
        this.AllowsTransparency = true;
        this.Background = Brushes.Transparent;
        this.ResizeMode = ResizeMode.CanResize;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.WindowState = WindowState.Normal;
        this.ShowInTaskbar = true;

        this.CommandBindings.Add(new CommandBinding(XWindowCommands.ToggleThemeMode, OnToggleThemeModeExecuted, OnToggleThemeModeCanExecute));
        this.CommandBindings.Add(new CommandBinding(XWindowCommands.Minimize, OnMinimizeExecuted, OnMinimizeCanExecute));
        this.CommandBindings.Add(new CommandBinding(XWindowCommands.MaximizeRestore, OnMaximizeRestoreExecuted, OnMaximizeRestoreCanExecute));
        this.CommandBindings.Add(new CommandBinding(XWindowCommands.Close, OnCloseExecuted, OnCloseCanExecute));

        this.SourceInitialized += OnWindowSourceInitialized;
        this.Loaded += OnWindowLoaded;
        this.Closing += OnWindowClosing;
        this.Closed += OnWindowClosed;

        WeakReferenceMessenger.Default.Register<ShowEditorOverlayMessage>(this, static (recipient, message) => ((XWindow)recipient).OnShowEditorOverlayMessage(message));
        WeakReferenceMessenger.Default.Register<HideEditorOverlayMessage>(this, static (recipient, message) => ((XWindow)recipient).OnHideEditorOverlayMessage(message));

        this.UpdateDerivedCornerRadii();
    }
    #endregion

    #region ### Public Properties ###
    public object? TitleBarContent
    {
        get => this.GetValue(TitleBarContentProperty);
        set => this.SetValue(TitleBarContentProperty, value);
    }

    public DataTemplate? TitleBarContentTemplate
    {
        get => (DataTemplate?)this.GetValue(TitleBarContentTemplateProperty);
        set => this.SetValue(TitleBarContentTemplateProperty, value);
    }

    public string Subtitle
    {
        get => (string)this.GetValue(SubtitleProperty);
        set => this.SetValue(SubtitleProperty, value);
    }

    public double TitleBarHeight
    {
        get => (double)this.GetValue(TitleBarHeightProperty);
        set => this.SetValue(TitleBarHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the title in the window title bar.
    /// </summary>
    public double TitleFontSize
    {
        get => (double)this.GetValue(TitleFontSizeProperty);
        set => this.SetValue(TitleFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the window title bar.
    /// </summary>
    public Thickness TitleBarPadding
    {
        get => (Thickness)this.GetValue(TitleBarPaddingProperty);
        set => this.SetValue(TitleBarPaddingProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    public CornerRadius TopCornerRadius => (CornerRadius)this.GetValue(TopCornerRadiusProperty);

    public bool ShowThemeSelector
    {
        get => (bool)this.GetValue(ShowThemeSelectorProperty);
        set => this.SetValue(ShowThemeSelectorProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the language selector is visible in the title bar.
    /// </summary>
    public bool ShowLanguageSelector
    {
        get => (bool)this.GetValue(ShowLanguageSelectorProperty);
        set => this.SetValue(ShowLanguageSelectorProperty, value);
    }

    /// <summary>
    /// Gets or sets the languages displayed by the title bar language selector.
    /// </summary>
    public IEnumerable<XLanguage>? AvailableLanguages
    {
        get => (IEnumerable<XLanguage>?)this.GetValue(AvailableLanguagesProperty);
        set => this.SetValue(AvailableLanguagesProperty, value);
    }

    /// <summary>
    /// Gets or sets the language selected in the title bar language selector.
    /// </summary>
    public XLanguage? SelectedLanguage
    {
        get => (XLanguage?)this.GetValue(SelectedLanguageProperty);
        set => this.SetValue(SelectedLanguageProperty, value);
    }

    /// <summary>
    /// Gets or sets whether language selection also changes number, date and time formatting culture.
    /// </summary>
    public bool ApplyLanguageFormattingCulture
    {
        get => (bool)this.GetValue(ApplyLanguageFormattingCultureProperty);
        set => this.SetValue(ApplyLanguageFormattingCultureProperty, value);
    }

    public bool ShowThemeModeButton
    {
        get => (bool)this.GetValue(ShowThemeModeButtonProperty);
        set => this.SetValue(ShowThemeModeButtonProperty, value);
    }

    public bool ShowMinimizeButton
    {
        get => (bool)this.GetValue(ShowMinimizeButtonProperty);
        set => this.SetValue(ShowMinimizeButtonProperty, value);
    }

    public bool ShowMaximizeButton
    {
        get => (bool)this.GetValue(ShowMaximizeButtonProperty);
        set => this.SetValue(ShowMaximizeButtonProperty, value);
    }

    public bool ShowCloseButton
    {
        get => (bool)this.GetValue(ShowCloseButtonProperty);
        set => this.SetValue(ShowCloseButtonProperty, value);
    }

    public bool ShowResizeGrip
    {
        get => (bool)this.GetValue(ShowResizeGripProperty);
        set => this.SetValue(ShowResizeGripProperty, value);
    }

    public Brush WindowBackgroundBrush
    {
        get => (Brush)this.GetValue(WindowBackgroundBrushProperty);
        set => this.SetValue(WindowBackgroundBrushProperty, value);
    }

    public Brush WindowForegroundBrush
    {
        get => (Brush)this.GetValue(WindowForegroundBrushProperty);
        set => this.SetValue(WindowForegroundBrushProperty, value);
    }

    public Brush WindowBorderBrush
    {
        get => (Brush)this.GetValue(WindowBorderBrushProperty);
        set => this.SetValue(WindowBorderBrushProperty, value);
    }

    public Brush TitleBarBackgroundBrush
    {
        get => (Brush)this.GetValue(TitleBarBackgroundBrushProperty);
        set => this.SetValue(TitleBarBackgroundBrushProperty, value);
    }

    public Brush TitleBarForegroundBrush
    {
        get => (Brush)this.GetValue(TitleBarForegroundBrushProperty);
        set => this.SetValue(TitleBarForegroundBrushProperty, value);
    }

    public Brush CaptionButtonHoverBrush
    {
        get => (Brush)this.GetValue(CaptionButtonHoverBrushProperty);
        set => this.SetValue(CaptionButtonHoverBrushProperty, value);
    }

    public Brush CaptionButtonPressedBrush
    {
        get => (Brush)this.GetValue(CaptionButtonPressedBrushProperty);
        set => this.SetValue(CaptionButtonPressedBrushProperty, value);
    }

    public Brush CloseButtonHoverBrush
    {
        get => (Brush)this.GetValue(CloseButtonHoverBrushProperty);
        set => this.SetValue(CloseButtonHoverBrushProperty, value);
    }

    public Brush CloseButtonPressedBrush
    {
        get => (Brush)this.GetValue(CloseButtonPressedBrushProperty);
        set => this.SetValue(CloseButtonPressedBrushProperty, value);
    }

    public Brush CloseButtonForegroundBrush
    {
        get => (Brush)this.GetValue(CloseButtonForegroundBrushProperty);
        set => this.SetValue(CloseButtonForegroundBrushProperty, value);
    }

    public object? StatusBarContent
    {
        get => this.GetValue(StatusBarContentProperty);
        set => this.SetValue(StatusBarContentProperty, value);
    }

    public DataTemplate? StatusBarContentTemplate
    {
        get => (DataTemplate?)this.GetValue(StatusBarContentTemplateProperty);
        set => this.SetValue(StatusBarContentTemplateProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)this.GetValue(IsBusyProperty);
        set => this.SetValue(IsBusyProperty, value);
    }

    public object? BusyContent
    {
        get => this.GetValue(BusyContentProperty);
        set => this.SetValue(BusyContentProperty, value);
    }

    public DataTemplate? BusyContentTemplate
    {
        get => (DataTemplate?)this.GetValue(BusyContentTemplateProperty);
        set => this.SetValue(BusyContentTemplateProperty, value);
    }

    public Brush BusyOverlayBrush
    {
        get => (Brush)this.GetValue(BusyOverlayBrushProperty);
        set => this.SetValue(BusyOverlayBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used to dim this window while an owned modal dialog is open.
    /// </summary>
    public Brush ModalOverlayBrush
    {
        get => (Brush)this.GetValue(ModalOverlayBrushProperty);
        set => this.SetValue(ModalOverlayBrushProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether this window currently hosts one or more modal overlay leases.
    /// </summary>
    public bool IsModalOverlayOpen => (bool)this.GetValue(IsModalOverlayOpenProperty);

    public Brush FlyoutOverlayBrush
    {
        get => (Brush)this.GetValue(FlyoutOverlayBrushProperty);
        set => this.SetValue(FlyoutOverlayBrushProperty, value);
    }

    public bool CloseFlyoutsOnOverlayClick
    {
        get => (bool)this.GetValue(CloseFlyoutsOnOverlayClickProperty);
        set => this.SetValue(CloseFlyoutsOnOverlayClickProperty, value);
    }

    public object? LeftFlyoutContent
    {
        get => this.GetValue(LeftFlyoutContentProperty);
        set => this.SetValue(LeftFlyoutContentProperty, value);
    }

    public object? RightFlyoutContent
    {
        get => this.GetValue(RightFlyoutContentProperty);
        set => this.SetValue(RightFlyoutContentProperty, value);
    }

    public object? TopFlyoutContent
    {
        get => this.GetValue(TopFlyoutContentProperty);
        set => this.SetValue(TopFlyoutContentProperty, value);
    }

    public object? BottomFlyoutContent
    {
        get => this.GetValue(BottomFlyoutContentProperty);
        set => this.SetValue(BottomFlyoutContentProperty, value);
    }

    public bool IsLeftFlyoutOpen
    {
        get => (bool)this.GetValue(IsLeftFlyoutOpenProperty);
        set => this.SetValue(IsLeftFlyoutOpenProperty, value);
    }

    public bool IsRightFlyoutOpen
    {
        get => (bool)this.GetValue(IsRightFlyoutOpenProperty);
        set => this.SetValue(IsRightFlyoutOpenProperty, value);
    }

    public bool IsTopFlyoutOpen
    {
        get => (bool)this.GetValue(IsTopFlyoutOpenProperty);
        set => this.SetValue(IsTopFlyoutOpenProperty, value);
    }

    public bool IsBottomFlyoutOpen
    {
        get => (bool)this.GetValue(IsBottomFlyoutOpenProperty);
        set => this.SetValue(IsBottomFlyoutOpenProperty, value);
    }

    public double LeftFlyoutWidth
    {
        get => (double)this.GetValue(LeftFlyoutWidthProperty);
        set => this.SetValue(LeftFlyoutWidthProperty, value);
    }

    public double RightFlyoutWidth
    {
        get => (double)this.GetValue(RightFlyoutWidthProperty);
        set => this.SetValue(RightFlyoutWidthProperty, value);
    }

    public double TopFlyoutHeight
    {
        get => (double)this.GetValue(TopFlyoutHeightProperty);
        set => this.SetValue(TopFlyoutHeightProperty, value);
    }

    public double BottomFlyoutHeight
    {
        get => (double)this.GetValue(BottomFlyoutHeightProperty);
        set => this.SetValue(BottomFlyoutHeightProperty, value);
    }

    public object? ToastContent
    {
        get => this.GetValue(ToastContentProperty);
        set => this.SetValue(ToastContentProperty, value);
    }

    public DataTemplate? ToastContentTemplate
    {
        get => (DataTemplate?)this.GetValue(ToastContentTemplateProperty);
        set => this.SetValue(ToastContentTemplateProperty, value);
    }

    public bool IsToastOpen
    {
        get => (bool)this.GetValue(IsToastOpenProperty);
        set => this.SetValue(IsToastOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the toast placement.
    /// </summary>
    public XToastPlacement ToastPlacement
    {
        get => (XToastPlacement)this.GetValue(ToastPlacementProperty);
        set => this.SetValue(ToastPlacementProperty, value);
    }

    public bool UseAnimations
    {
        get => (bool)this.GetValue(UseAnimationsProperty);
        set => this.SetValue(UseAnimationsProperty, value);
    }

    public Duration FlyoutAnimationDuration
    {
        get => (Duration)this.GetValue(FlyoutAnimationDurationProperty);
        set => this.SetValue(FlyoutAnimationDurationProperty, value);
    }

    public double FlyoutAnimationOffset
    {
        get => (double)this.GetValue(FlyoutAnimationOffsetProperty);
        set => this.SetValue(FlyoutAnimationOffsetProperty, value);
    }

    public bool FlyoutUseFade
    {
        get => (bool)this.GetValue(FlyoutUseFadeProperty);
        set => this.SetValue(FlyoutUseFadeProperty, value);
    }

    public Duration ToastAnimationDuration
    {
        get => (Duration)this.GetValue(ToastAnimationDurationProperty);
        set => this.SetValue(ToastAnimationDurationProperty, value);
    }

    public double ToastAnimationOffset
    {
        get => (double)this.GetValue(ToastAnimationOffsetProperty);
        set => this.SetValue(ToastAnimationOffsetProperty, value);
    }

    public bool ToastUseFade
    {
        get => (bool)this.GetValue(ToastUseFadeProperty);
        set => this.SetValue(ToastUseFadeProperty, value);
    }

    /// <summary>
    /// Gets or sets the startup animation mode.
    /// </summary>
    public XWindowAnimationMode StartupAnimation
    {
        get => (XWindowAnimationMode)this.GetValue(StartupAnimationProperty);
        set => this.SetValue(StartupAnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets the startup fade animation duration.
    /// </summary>
    public Duration StartupAnimationDuration
    {
        get => (Duration)this.GetValue(StartupAnimationDurationProperty);
        set => this.SetValue(StartupAnimationDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional startup scale animation duration.
    /// If not set explicitly, a shorter duration is derived automatically from <see cref="StartupAnimationDuration"/>.
    /// </summary>
    public Duration StartupAnimationScaleDuration
    {
        get => (Duration)this.GetValue(StartupAnimationScaleDurationProperty);
        set => this.SetValue(StartupAnimationScaleDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the initial startup scale that is used for <see cref="XWindowAnimationMode.FadeAndScale"/>.
    /// </summary>
    public double StartupAnimationInitialScale
    {
        get => (double)this.GetValue(StartupAnimationInitialScaleProperty);
        set => this.SetValue(StartupAnimationInitialScaleProperty, value);
    }

    /// <summary>
    /// Gets or sets the close animation mode.
    /// </summary>
    public XWindowAnimationMode CloseAnimation
    {
        get => (XWindowAnimationMode)this.GetValue(CloseAnimationProperty);
        set => this.SetValue(CloseAnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets the close fade animation duration.
    /// </summary>
    public Duration CloseAnimationDuration
    {
        get => (Duration)this.GetValue(CloseAnimationDurationProperty);
        set => this.SetValue(CloseAnimationDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional close scale animation duration.
    /// If not set explicitly, a shorter duration is derived automatically from <see cref="CloseAnimationDuration"/>.
    /// </summary>
    public Duration CloseAnimationScaleDuration
    {
        get => (Duration)this.GetValue(CloseAnimationScaleDurationProperty);
        set => this.SetValue(CloseAnimationScaleDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets the close animation target scale that is used for <see cref="XWindowAnimationMode.FadeAndScale"/>.
    /// </summary>
    public double CloseAnimationTargetScale
    {
        get => (double)this.GetValue(CloseAnimationTargetScaleProperty);
        set => this.SetValue(CloseAnimationTargetScaleProperty, value);
    }


    /// <summary>
    /// Gets or sets the current editor overlay message.
    /// </summary>
    public ShowEditorOverlayMessage? EditorOverlayMessage
    {
        get => (ShowEditorOverlayMessage?)this.GetValue(EditorOverlayMessageProperty);
        set => this.SetValue(EditorOverlayMessageProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editor overlay is open.
    /// </summary>
    public bool IsEditorOverlayOpen
    {
        get => (bool)this.GetValue(IsEditorOverlayOpenProperty);
        set => this.SetValue(IsEditorOverlayOpenProperty, value);
    }
    #endregion

    #region ### Internal Properties ###
    /// <summary>
    /// Gets the current number of modal overlay leases held by this window.
    /// </summary>
    internal int ModalOverlayDepth => this._modalOverlayDepth;
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Acquires an owner-local modal overlay lease.
    /// </summary>
    /// <returns>A lease that releases the overlay when disposed.</returns>
    internal IDisposable AcquireModalOverlay()
    {
        this.Dispatcher.VerifyAccess();

        if (this._modalOverlayDepth == int.MaxValue)
        {
            throw new InvalidOperationException("The modal overlay lease depth exceeded its supported range.");
        }

        this._modalOverlayDepth++;

        if (this._modalOverlayDepth == 1)
        {
            this.SetValue(IsModalOverlayOpenPropertyKey, true);
        }

        return new XModalOverlayLease(this.Dispatcher, this.ReleaseModalOverlay);
    }

    IDisposable IXModalOverlayHost.AcquireModalOverlay()
    {
        return this.AcquireModalOverlay();
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Applies the control template and connects template parts.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (this._titleBarElement is not null)
        {
            this._titleBarElement.MouseLeftButtonDown -= OnTitleBarMouseLeftButtonDown;
        }

        DetachResizeThumb(this._resizeGripThumb, OnResizeGripDragDelta);
        DetachResizeThumb(this._leftResizeThumb, OnLeftResizeThumbDragDelta);
        DetachResizeThumb(this._topResizeThumb, OnTopResizeThumbDragDelta);
        DetachResizeThumb(this._rightResizeThumb, OnRightResizeThumbDragDelta);
        DetachResizeThumb(this._bottomResizeThumb, OnBottomResizeThumbDragDelta);
        DetachResizeThumb(this._topLeftResizeThumb, OnTopLeftResizeThumbDragDelta);
        DetachResizeThumb(this._topRightResizeThumb, OnTopRightResizeThumbDragDelta);
        DetachResizeThumb(this._bottomRightResizeThumb, OnBottomRightResizeThumbDragDelta);
        DetachResizeThumb(this._bottomLeftResizeThumb, OnBottomLeftResizeThumbDragDelta);

        if (this._flyoutOverlay is not null)
        {
            this._flyoutOverlay.MouseLeftButtonDown -= OnFlyoutOverlayMouseLeftButtonDown;
        }

        if (this._editorOverlayOverlay is not null)
        {
            this._editorOverlayOverlay.MouseLeftButtonUp -= this.OnEditorOverlayMouseLeftButtonUp;
        }

        if (this._editorOverlayCloseButton is not null)
        {
            this._editorOverlayCloseButton.Click -= this.OnEditorOverlayCloseButtonClick;
        }

        if (this._editorOverlayCancelButton is not null)
        {
            this._editorOverlayCancelButton.Click -= this.OnEditorOverlayCancelButtonClick;
        }

        this._titleBarElement = this.GetTemplateChild("PART_TitleBar") as FrameworkElement;
        this._resizeGripThumb = this.GetTemplateChild("PART_ResizeGrip") as Thumb;
        this._leftResizeThumb = this.GetTemplateChild("PART_LeftResizeThumb") as Thumb;
        this._topResizeThumb = this.GetTemplateChild("PART_TopResizeThumb") as Thumb;
        this._rightResizeThumb = this.GetTemplateChild("PART_RightResizeThumb") as Thumb;
        this._bottomResizeThumb = this.GetTemplateChild("PART_BottomResizeThumb") as Thumb;
        this._topLeftResizeThumb = this.GetTemplateChild("PART_TopLeftResizeThumb") as Thumb;
        this._topRightResizeThumb = this.GetTemplateChild("PART_TopRightResizeThumb") as Thumb;
        this._bottomRightResizeThumb = this.GetTemplateChild("PART_BottomRightResizeThumb") as Thumb;
        this._bottomLeftResizeThumb = this.GetTemplateChild("PART_BottomLeftResizeThumb") as Thumb;
        this._flyoutLayer = this.GetTemplateChild("PART_FlyoutLayer") as Grid;
        this._flyoutOverlay = this.GetTemplateChild("PART_FlyoutOverlay") as Border;
        this._leftFlyoutElement = this.GetTemplateChild("PART_LeftFlyout") as Border;
        this._rightFlyoutElement = this.GetTemplateChild("PART_RightFlyout") as Border;
        this._topFlyoutElement = this.GetTemplateChild("PART_TopFlyout") as Border;
        this._bottomFlyoutElement = this.GetTemplateChild("PART_BottomFlyout") as Border;
        this._leftFlyoutTransform = this.GetTemplateChild("PART_LeftFlyoutTransform") as TranslateTransform;
        this._rightFlyoutTransform = this.GetTemplateChild("PART_RightFlyoutTransform") as TranslateTransform;
        this._topFlyoutTransform = this.GetTemplateChild("PART_TopFlyoutTransform") as TranslateTransform;
        this._bottomFlyoutTransform = this.GetTemplateChild("PART_BottomFlyoutTransform") as TranslateTransform;
        this._windowFrameElement = this.GetTemplateChild("PART_WindowFrame") as Border;
        this._editorOverlayOverlay = this.GetTemplateChild("PART_EditorOverlayOverlay") as Border;
        this._editorOverlayCloseButton = this.GetTemplateChild("PART_EditorOverlayCloseButton") as ButtonBase;
        this._editorOverlayCancelButton = this.GetTemplateChild("PART_EditorOverlayCancelButton") as ButtonBase;

        this.EnsureWindowFrameAnimationTransform();
        this.PrepareStartupAnimationState();

        if (this._titleBarElement is not null)
        {
            this._titleBarElement.MouseLeftButtonDown += OnTitleBarMouseLeftButtonDown;
        }

        if (this._flyoutOverlay is not null)
        {
            this._flyoutOverlay.MouseLeftButtonDown += OnFlyoutOverlayMouseLeftButtonDown;
        }

        if (this._editorOverlayOverlay is not null)
        {
            this._editorOverlayOverlay.MouseLeftButtonUp += this.OnEditorOverlayMouseLeftButtonUp;
        }

        if (this._editorOverlayCloseButton is not null)
        {
            this._editorOverlayCloseButton.Click += this.OnEditorOverlayCloseButtonClick;
        }

        if (this._editorOverlayCancelButton is not null)
        {
            this._editorOverlayCancelButton.Click += this.OnEditorOverlayCancelButtonClick;
        }

        AttachResizeThumb(this._resizeGripThumb, OnResizeGripDragDelta);
        AttachResizeThumb(this._leftResizeThumb, OnLeftResizeThumbDragDelta);
        AttachResizeThumb(this._topResizeThumb, OnTopResizeThumbDragDelta);
        AttachResizeThumb(this._rightResizeThumb, OnRightResizeThumbDragDelta);
        AttachResizeThumb(this._bottomResizeThumb, OnBottomResizeThumbDragDelta);
        AttachResizeThumb(this._topLeftResizeThumb, OnTopLeftResizeThumbDragDelta);
        AttachResizeThumb(this._topRightResizeThumb, OnTopRightResizeThumbDragDelta);
        AttachResizeThumb(this._bottomRightResizeThumb, OnBottomRightResizeThumbDragDelta);
        AttachResizeThumb(this._bottomLeftResizeThumb, OnBottomLeftResizeThumbDragDelta);

        this.UpdateAllFlyoutStates(false);
    }
    #endregion

    #region ### Private Methods ###
    private void ReleaseModalOverlay()
    {
        this.Dispatcher.VerifyAccess();

        if (this._modalOverlayDepth <= 0)
        {
            throw new InvalidOperationException("A modal overlay lease was released without a matching acquisition.");
        }

        this._modalOverlayDepth--;

        if (this._modalOverlayDepth == 0)
        {
            this.SetValue(IsModalOverlayOpenPropertyKey, false);
        }
    }

    private void OnWindowSourceInitialized(object? sender, EventArgs e)
    {
        this.AttachWindowHook();
        this.PrepareStartupAnimationState();
    }


    private void AttachWindowHook()
    {
        if (this._hwndSource is not null)
        {
            return;
        }

        IntPtr handle = new WindowInteropHelper(this).Handle;
        if (handle == IntPtr.Zero)
        {
            return;
        }

        this._hwndSource = HwndSource.FromHwnd(handle);
        this._hwndSource?.AddHook(this.WndProc);
    }

    private void DetachWindowHook()
    {
        if (this._hwndSource is null)
        {
            return;
        }

        this._hwndSource.RemoveHook(this.WndProc);
        this._hwndSource = null;
    }

    private IntPtr WndProc(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (message == NativeMethods.WmGetMinMaxInfo)
        {
            handled = NativeMethods.TryAdjustMaximizedWindowSize(hwnd, lParam);
        }

        return IntPtr.Zero;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        this.BeginStartupAnimation();
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        if (this._isCloseAnimationCompleted)
        {
            return;
        }

        if (this._isCloseAnimationRunning)
        {
            e.Cancel = true;
            return;
        }

        if (!this.ShouldAnimateClose())
        {
            return;
        }

        this._pendingDialogResult = this.DialogResult;
        e.Cancel = true;
        this.BeginCloseAnimation();
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        this._pendingDialogResult = null;
        this.DetachWindowHook();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        this.EditorOverlayMessage = null;
        this.IsEditorOverlayOpen = false;
    }

    private void OnShowEditorOverlayMessage(ShowEditorOverlayMessage message)
    {
        if (!ReferenceEquals(message.TargetWindow, this))
        {
            return;
        }

        this.EditorOverlayMessage = message;
        this.IsEditorOverlayOpen = true;
        message.Handled = true;
    }

    private void OnHideEditorOverlayMessage(HideEditorOverlayMessage message)
    {
        if (message.TargetWindow is not null && !ReferenceEquals(message.TargetWindow, this))
        {
            return;
        }

        if (!ReferenceEquals(this.EditorOverlayMessage?.Owner, message.Owner))
        {
            return;
        }

        this.IsEditorOverlayOpen = false;
        this.EditorOverlayMessage = null;
        message.Handled = true;
    }

    private void OnEditorOverlayMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        ShowEditorOverlayMessage? message = this.EditorOverlayMessage;
        if (message?.CanCloseOnOverlayClick != true)
        {
            return;
        }

        message.Close();
        e.Handled = true;
    }

    private void OnEditorOverlayCloseButtonClick(object sender, RoutedEventArgs e)
    {
        this.EditorOverlayMessage?.Close();
        e.Handled = true;
    }

    private void OnEditorOverlayCancelButtonClick(object sender, RoutedEventArgs e)
    {
        this.EditorOverlayMessage?.Close();
        e.Handled = true;
    }

    private bool ShouldAnimateStartup()
    {
        return this.UseAnimations
               && this.StartupAnimation != XWindowAnimationMode.None
               && !DesignerProperties.GetIsInDesignMode(this)
               && !this._isCloseAnimationRunning;
    }

    private bool ShouldAnimateClose()
    {
        return this.UseAnimations
               && this.CloseAnimation != XWindowAnimationMode.None
               && !DesignerProperties.GetIsInDesignMode(this)
               && this.IsLoaded
               && !this._isCloseAnimationRunning
               && !this._isCloseAnimationCompleted;
    }

    private void PrepareStartupAnimationState()
    {
        if (this._isStartupAnimationPrepared || !this.ShouldAnimateStartup())
        {
            return;
        }

        this.Opacity = 0.62d;
        this.EnsureWindowFrameAnimationTransform();

        if (this.StartupAnimation == XWindowAnimationMode.FadeAndScale && this._windowFrameScaleTransform is not null)
        {
            this._windowFrameScaleTransform.ScaleX = this.StartupAnimationInitialScale;
            this._windowFrameScaleTransform.ScaleY = this.StartupAnimationInitialScale;
        }

        this._isStartupAnimationPrepared = true;
    }

    private void BeginStartupAnimation()
    {
        if (!this._isStartupAnimationPrepared || !this.ShouldAnimateStartup())
        {
            return;
        }

        this.BeginAnimation(Window.OpacityProperty, null);
        Storyboard? storyboard = this.CreateWindowAnimationStoryboard(
            this.StartupAnimation,
            this.StartupAnimationDuration,
            ResolveScaleAnimationDuration(this.StartupAnimationDuration, this.StartupAnimationScaleDuration, true),
            this.Opacity,
            1d,
            this.StartupAnimationInitialScale,
            1d);

        if (storyboard is null)
        {
            this.ResetAnimationState();
            return;
        }

        storyboard.Completed += (_, _) =>
        {
            this.Opacity = 1d;
            this.ResetAnimationState();
        };

        storyboard.Begin();
    }

    private void BeginCloseAnimation()
    {
        if (!this.ShouldAnimateClose())
        {
            this.CompleteAnimatedClose();
            return;
        }

        this._isCloseAnimationRunning = true;
        this.BeginAnimation(Window.OpacityProperty, null);

        Storyboard? storyboard = this.CreateWindowAnimationStoryboard(
            this.CloseAnimation,
            this.CloseAnimationDuration,
            ResolveScaleAnimationDuration(this.CloseAnimationDuration, this.CloseAnimationScaleDuration, false),
            this.Opacity,
            0d,
            1d,
            this.CloseAnimationTargetScale);

        if (storyboard is null)
        {
            this.CompleteAnimatedClose();
            return;
        }

        storyboard.Completed += (_, _) =>
        {
            this.BeginAnimation(Window.OpacityProperty, null);
            this.Opacity = 0d;
            this.CompleteAnimatedClose();
        };

        storyboard.Begin();
    }

    private void CompleteAnimatedClose()
    {
        bool? pendingDialogResult = this._pendingDialogResult;

        this._pendingDialogResult = null;
        this._isCloseAnimationRunning = false;
        this._isCloseAnimationCompleted = true;

        if (pendingDialogResult.HasValue)
        {
            this.DialogResult = pendingDialogResult.Value;
            return;
        }

        this.Close();
    }

    private static Duration ResolveScaleAnimationDuration(Duration opacityDuration, Duration configuredScaleDuration, bool isStartup)
    {
        if (configuredScaleDuration.HasTimeSpan)
        {
            return configuredScaleDuration;
        }

        if (!opacityDuration.HasTimeSpan)
        {
            return opacityDuration;
        }

        double opacityMilliseconds = opacityDuration.TimeSpan.TotalMilliseconds;
        double minimumMilliseconds = isStartup ? 120d : 100d;
        double maximumMilliseconds = isStartup ? 180d : 160d;
        double preferredMilliseconds = Math.Max(opacityMilliseconds * 0.6d, minimumMilliseconds);
        double scaleMilliseconds = Math.Min(opacityMilliseconds, Math.Min(preferredMilliseconds, maximumMilliseconds));

        return new Duration(TimeSpan.FromMilliseconds(scaleMilliseconds));
    }

    private Storyboard? CreateWindowAnimationStoryboard(
        XWindowAnimationMode animationMode,
        Duration opacityDuration,
        Duration scaleDuration,
        double fromOpacity,
        double toOpacity,
        double fromScale,
        double toScale)
    {
        Storyboard storyboard = new();

        DoubleAnimation opacityAnimation = new()
        {
            From = fromOpacity,
            To = toOpacity,
            Duration = opacityDuration,
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        Storyboard.SetTarget(opacityAnimation, this);
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Window.OpacityProperty));
        storyboard.Children.Add(opacityAnimation);

        if (animationMode == XWindowAnimationMode.FadeAndScale)
        {
            this.EnsureWindowFrameAnimationTransform();

            if (this._windowFrameScaleTransform is null)
            {
                return storyboard;
            }

            DoubleAnimation scaleXAnimation = new()
            {
                From = fromScale,
                To = toScale,
                Duration = scaleDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            DoubleAnimation scaleYAnimation = new()
            {
                From = fromScale,
                To = toScale,
                Duration = scaleDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(scaleXAnimation, this._windowFrameScaleTransform);
            Storyboard.SetTarget(scaleYAnimation, this._windowFrameScaleTransform);
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath(ScaleTransform.ScaleYProperty));

            storyboard.Children.Add(scaleXAnimation);
            storyboard.Children.Add(scaleYAnimation);
        }

        return storyboard;
    }

    private void EnsureWindowFrameAnimationTransform()
    {
        if (this._windowFrameElement is null)
        {
            return;
        }

        this._windowFrameElement.RenderTransformOrigin = new Point(0.5d, 0.5d);

        if (this._windowFrameElement.RenderTransform is ScaleTransform scaleTransform)
        {
            this._windowFrameScaleTransform = scaleTransform;
            return;
        }

        if (this._windowFrameElement.RenderTransform is TransformGroup transformGroup)
        {
            ScaleTransform? existingScaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault();
            if (existingScaleTransform is not null)
            {
                this._windowFrameScaleTransform = existingScaleTransform;
                return;
            }

            this._windowFrameScaleTransform = new ScaleTransform(1d, 1d);
            transformGroup.Children.Insert(0, this._windowFrameScaleTransform);
            return;
        }

        this._windowFrameScaleTransform = new ScaleTransform(1d, 1d);
        this._windowFrameElement.RenderTransform = this._windowFrameScaleTransform;
    }

    private void ResetAnimationState()
    {
        this._isStartupAnimationPrepared = false;

        if (this._windowFrameScaleTransform is not null)
        {
            this._windowFrameScaleTransform.ScaleX = 1d;
            this._windowFrameScaleTransform.ScaleY = 1d;
        }
    }

    private static void OnFlyoutStatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XWindow window)
        {
            window.UpdateAllFlyoutStates(true);
        }
    }

    private static void OnFlyoutContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not XWindow window)
        {
            return;
        }

        if (ReferenceEquals(e.Property, LeftFlyoutContentProperty) && e.NewValue is null)
        {
            window.IsLeftFlyoutOpen = false;
        }
        else if (ReferenceEquals(e.Property, RightFlyoutContentProperty) && e.NewValue is null)
        {
            window.IsRightFlyoutOpen = false;
        }
        else if (ReferenceEquals(e.Property, TopFlyoutContentProperty) && e.NewValue is null)
        {
            window.IsTopFlyoutOpen = false;
        }
        else if (ReferenceEquals(e.Property, BottomFlyoutContentProperty) && e.NewValue is null)
        {
            window.IsBottomFlyoutOpen = false;
        }

        window.UpdateAllFlyoutStates(false);
    }

    private static void OnCornerRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XWindow window)
        {
            window.UpdateDerivedCornerRadii();
        }
    }

    private static void OnLanguageConfigurationChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XWindow window &&
            window.SelectedLanguage is XLanguage selectedLanguage)
        {
            XLocalizationService.Current.SetCulture(
                selectedLanguage.Culture,
                window.ApplyLanguageFormattingCulture);

            CultureInfo formattingCulture = window.ApplyLanguageFormattingCulture
                ? selectedLanguage.Culture
                : CultureInfo.CurrentCulture;

            window.SetCurrentValue(
                LanguageProperty,
                XmlLanguage.GetLanguage(formattingCulture.IetfLanguageTag));
        }
    }

    private static void OnToggleThemeModeCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        e.Handled = true;
    }

    private static async void OnToggleThemeModeExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is XWindow window)
        {
            await XThemeTransitionHelper.AnimateThemeChangeAsync(
                window,
                XThemeManager.Current.ToggleMode);
        }
        else
        {
            XThemeManager.Current.ToggleMode();
        }

        e.Handled = true;
    }

    private static void OnMinimizeCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (sender is XWindow window)
        {
            e.CanExecute = window.ResizeMode is not ResizeMode.NoResize;
            e.Handled = true;
        }
    }

    private static void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is XWindow window)
        {
            SystemCommands.MinimizeWindow(window);
            e.Handled = true;
        }
    }

    private static void OnMaximizeRestoreCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (sender is XWindow window)
        {
            e.CanExecute = window.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
            e.Handled = true;
        }
    }

    private static void OnMaximizeRestoreExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is not XWindow window)
        {
            return;
        }

        if (window.WindowState == WindowState.Maximized)
        {
            SystemCommands.RestoreWindow(window);
        }
        else
        {
            SystemCommands.MaximizeWindow(window);
        }

        e.Handled = true;
    }

    private static void OnCloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        e.Handled = true;
    }

    private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is XWindow window)
        {
            SystemCommands.CloseWindow(window);
            e.Handled = true;
        }
    }

    private void OnTitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left)
        {
            return;
        }

        if (IsInsideInteractiveElement(e.OriginalSource as DependencyObject))
        {
            return;
        }

        if (e.ClickCount == 2 && this.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
            }
            else
            {
                SystemCommands.MaximizeWindow(this);
            }

            return;
        }

        try
        {
            this.DragMove();
        }
        catch
        {
        }
    }

    private void OnFlyoutOverlayMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!this.CloseFlyoutsOnOverlayClick)
        {
            return;
        }

        this.CloseAllFlyouts();
        e.Handled = true;
    }

    private void CloseAllFlyouts()
    {
        this.IsLeftFlyoutOpen = false;
        this.IsRightFlyoutOpen = false;
        this.IsTopFlyoutOpen = false;
        this.IsBottomFlyoutOpen = false;
    }

    private void UpdateAllFlyoutStates(bool useTransitions)
    {
        this.UpdateFlyoutState(this._leftFlyoutElement, this._leftFlyoutTransform, this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null, FlyoutPlacement.Left, useTransitions);
        this.UpdateFlyoutState(this._rightFlyoutElement, this._rightFlyoutTransform, this.IsRightFlyoutOpen && this.RightFlyoutContent is not null, FlyoutPlacement.Right, useTransitions);
        this.UpdateFlyoutState(this._topFlyoutElement, this._topFlyoutTransform, this.IsTopFlyoutOpen && this.TopFlyoutContent is not null, FlyoutPlacement.Top, useTransitions);
        this.UpdateFlyoutState(this._bottomFlyoutElement, this._bottomFlyoutTransform, this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null, FlyoutPlacement.Bottom, useTransitions);

        bool isAnyFlyoutOpen = (this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null)
                               || (this.IsRightFlyoutOpen && this.RightFlyoutContent is not null)
                               || (this.IsTopFlyoutOpen && this.TopFlyoutContent is not null)
                               || (this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null);

        this.UpdateFlyoutOverlayState(isAnyFlyoutOpen, useTransitions);

        if (this._flyoutLayer is not null)
        {
            this._flyoutLayer.IsHitTestVisible = isAnyFlyoutOpen;
        }
    }

    private void UpdateFlyoutOverlayState(bool isVisible, bool useTransitions)
    {
        if (this._flyoutOverlay is null)
        {
            return;
        }

        this._flyoutOverlay.BeginAnimation(UIElement.OpacityProperty, null);

        if (!useTransitions || !this.UseAnimations)
        {
            this._flyoutOverlay.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            this._flyoutOverlay.Opacity = isVisible ? 1d : 0d;
            return;
        }

        Duration duration = this.FlyoutAnimationDuration;

        if (isVisible)
        {
            this._flyoutOverlay.Visibility = Visibility.Visible;
            this._flyoutOverlay.BeginAnimation(
                UIElement.OpacityProperty,
                new DoubleAnimation
                {
                    From = this._flyoutOverlay.Opacity,
                    To = 1d,
                    Duration = duration
                });
        }
        else
        {
            DoubleAnimation animation = new()
            {
                From = this._flyoutOverlay.Opacity,
                To = 0d,
                Duration = duration
            };

            animation.Completed += (_, _) =>
            {
                if (!this.HasAnyFlyoutOpen() && this._flyoutOverlay is not null)
                {
                    this._flyoutOverlay.Visibility = Visibility.Collapsed;
                    this._flyoutOverlay.Opacity = 0d;
                }
            };

            this._flyoutOverlay.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }

    private void UpdateFlyoutState(Border? flyoutElement, TranslateTransform? transform, bool isOpen, FlyoutPlacement placement, bool useTransitions)
    {
        if (flyoutElement is null || transform is null)
        {
            return;
        }

        double closedOffset = placement switch
        {
            FlyoutPlacement.Left => -this.FlyoutAnimationOffset,
            FlyoutPlacement.Right => this.FlyoutAnimationOffset,
            FlyoutPlacement.Top => -this.FlyoutAnimationOffset,
            _ => this.FlyoutAnimationOffset,
        };

        transform.BeginAnimation(placement is FlyoutPlacement.Left or FlyoutPlacement.Right ? TranslateTransform.XProperty : TranslateTransform.YProperty, null);
        flyoutElement.BeginAnimation(UIElement.OpacityProperty, null);

        if (!useTransitions || !this.UseAnimations)
        {
            flyoutElement.Visibility = isOpen ? Visibility.Visible : Visibility.Collapsed;
            flyoutElement.Opacity = isOpen || !this.FlyoutUseFade ? 1d : 0d;

            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
            {
                transform.X = isOpen ? 0d : closedOffset;
            }
            else
            {
                transform.Y = isOpen ? 0d : closedOffset;
            }

            return;
        }

        Duration duration = this.FlyoutAnimationDuration;
        IEasingFunction easing = new QuadraticEase { EasingMode = EasingMode.EaseOut };

        if (isOpen)
        {
            flyoutElement.Visibility = Visibility.Visible;

            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
            {
                transform.X = closedOffset;
                transform.BeginAnimation(
                    TranslateTransform.XProperty,
                    new DoubleAnimation
                    {
                        From = closedOffset,
                        To = 0d,
                        Duration = duration,
                        EasingFunction = easing
                    });
            }
            else
            {
                transform.Y = closedOffset;
                transform.BeginAnimation(
                    TranslateTransform.YProperty,
                    new DoubleAnimation
                    {
                        From = closedOffset,
                        To = 0d,
                        Duration = duration,
                        EasingFunction = easing
                    });
            }

            if (this.FlyoutUseFade)
            {
                flyoutElement.Opacity = 0d;
                flyoutElement.BeginAnimation(
                    UIElement.OpacityProperty,
                    new DoubleAnimation
                    {
                        From = 0d,
                        To = 1d,
                        Duration = duration
                    });
            }
            else
            {
                flyoutElement.Opacity = 1d;
            }
        }
        else
        {
            if (flyoutElement.Visibility != Visibility.Visible)
            {
                flyoutElement.Visibility = Visibility.Collapsed;

                if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
                {
                    transform.X = closedOffset;
                }
                else
                {
                    transform.Y = closedOffset;
                }

                return;
            }

            DoubleAnimation offsetAnimation = new()
            {
                To = closedOffset,
                Duration = duration,
                EasingFunction = easing
            };

            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
            {
                transform.BeginAnimation(TranslateTransform.XProperty, offsetAnimation);
            }
            else
            {
                transform.BeginAnimation(TranslateTransform.YProperty, offsetAnimation);
            }

            DoubleAnimation opacityAnimation = new()
            {
                To = this.FlyoutUseFade ? 0d : 1d,
                Duration = duration
            };

            opacityAnimation.Completed += (_, _) =>
            {
                if (!this.IsFlyoutOpenForPlacement(placement))
                {
                    flyoutElement.Visibility = Visibility.Collapsed;
                    flyoutElement.Opacity = this.FlyoutUseFade ? 0d : 1d;

                    if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
                    {
                        transform.X = closedOffset;
                    }
                    else
                    {
                        transform.Y = closedOffset;
                    }
                }
            };

            flyoutElement.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }
    }

    private bool HasAnyFlyoutOpen()
    {
        return (this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null)
               || (this.IsRightFlyoutOpen && this.RightFlyoutContent is not null)
               || (this.IsTopFlyoutOpen && this.TopFlyoutContent is not null)
               || (this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null);
    }

    private bool IsFlyoutOpenForPlacement(FlyoutPlacement placement)
    {
        return placement switch
        {
            FlyoutPlacement.Left => this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null,
            FlyoutPlacement.Right => this.IsRightFlyoutOpen && this.RightFlyoutContent is not null,
            FlyoutPlacement.Top => this.IsTopFlyoutOpen && this.TopFlyoutContent is not null,
            FlyoutPlacement.Bottom => this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null,
            _ => false,
        };
    }

    private void OnResizeGripDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromRight(e.HorizontalChange);
        this.ResizeFromBottom(e.VerticalChange);
    }

    private void OnLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromLeft(e.HorizontalChange);
    }

    private void OnTopResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromTop(e.VerticalChange);
    }

    private void OnRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromRight(e.HorizontalChange);
    }

    private void OnBottomResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromBottom(e.VerticalChange);
    }

    private void OnTopLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromLeft(e.HorizontalChange);
        this.ResizeFromTop(e.VerticalChange);
    }

    private void OnTopRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromRight(e.HorizontalChange);
        this.ResizeFromTop(e.VerticalChange);
    }

    private void OnBottomRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromRight(e.HorizontalChange);
        this.ResizeFromBottom(e.VerticalChange);
    }

    private void OnBottomLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!this.CanResizeWindow())
        {
            return;
        }

        this.ResizeFromLeft(e.HorizontalChange);
        this.ResizeFromBottom(e.VerticalChange);
    }

    private bool CanResizeWindow()
    {
        return this.WindowState == WindowState.Normal
               && this.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
    }

    private void ResizeFromLeft(double horizontalChange)
    {
        double currentWidth = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
        double newWidth = Math.Max(this.MinWidth, currentWidth - horizontalChange);
        double appliedDelta = currentWidth - newWidth;

        if (!double.IsInfinity(this.MaxWidth))
        {
            newWidth = Math.Min(this.MaxWidth, newWidth);
            appliedDelta = currentWidth - newWidth;
        }

        this.Left += appliedDelta;
        this.Width = newWidth;
    }

    private void ResizeFromTop(double verticalChange)
    {
        double currentHeight = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
        double newHeight = Math.Max(this.MinHeight, currentHeight - verticalChange);
        double appliedDelta = currentHeight - newHeight;

        if (!double.IsInfinity(this.MaxHeight))
        {
            newHeight = Math.Min(this.MaxHeight, newHeight);
            appliedDelta = currentHeight - newHeight;
        }

        this.Top += appliedDelta;
        this.Height = newHeight;
    }

    private void ResizeFromRight(double horizontalChange)
    {
        double currentWidth = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
        double newWidth = Math.Max(this.MinWidth, currentWidth + horizontalChange);

        if (!double.IsInfinity(this.MaxWidth))
        {
            newWidth = Math.Min(this.MaxWidth, newWidth);
        }

        this.Width = newWidth;
    }

    private void ResizeFromBottom(double verticalChange)
    {
        double currentHeight = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
        double newHeight = Math.Max(this.MinHeight, currentHeight + verticalChange);

        if (!double.IsInfinity(this.MaxHeight))
        {
            newHeight = Math.Min(this.MaxHeight, newHeight);
        }

        this.Height = newHeight;
    }

    private static void AttachResizeThumb(Thumb? thumb, DragDeltaEventHandler handler)
    {
        if (thumb is not null)
        {
            thumb.DragDelta += handler;
        }
    }

    private static void DetachResizeThumb(Thumb? thumb, DragDeltaEventHandler handler)
    {
        if (thumb is not null)
        {
            thumb.DragDelta -= handler;
        }
    }

    private static bool IsInsideInteractiveElement(DependencyObject? source)
    {
        DependencyObject? current = source;

        while (current is not null)
        {
            if (current is ButtonBase or TextBoxBase or Selector or ScrollBar or Thumb)
            {
                return true;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return false;
    }

    private void UpdateDerivedCornerRadii()
    {
        this.SetValue(
            TopCornerRadiusPropertyKey,
            new CornerRadius(
                this.CornerRadius.TopLeft,
                this.CornerRadius.TopRight,
                0d,
                0d));
    }


    private static class NativeMethods
    {
        public const int WmGetMinMaxInfo = 0x0024;
        private const int MonitorDefaultToNearest = 0x00000002;

        public static bool TryAdjustMaximizedWindowSize(IntPtr hwnd, IntPtr lParam)
        {
            IntPtr monitor = MonitorFromWindow(hwnd, MonitorDefaultToNearest);
            if (monitor == IntPtr.Zero)
            {
                return false;
            }

            MonitorInfo monitorInfo = new()
            {
                cbSize = Marshal.SizeOf<MonitorInfo>()
            };

            if (!GetMonitorInfo(monitor, ref monitorInfo))
            {
                return false;
            }

            MinMaxInfo minMaxInfo = Marshal.PtrToStructure<MinMaxInfo>(lParam);
            Rect monitorArea = monitorInfo.rcMonitor;
            Rect workArea = monitorInfo.rcWork;

            minMaxInfo.ptMaxPosition.x = workArea.left - monitorArea.left;
            minMaxInfo.ptMaxPosition.y = workArea.top - monitorArea.top;
            minMaxInfo.ptMaxSize.x = workArea.right - workArea.left;
            minMaxInfo.ptMaxSize.y = workArea.bottom - workArea.top;
            minMaxInfo.ptMaxTrackSize.x = minMaxInfo.ptMaxSize.x;
            minMaxInfo.ptMaxTrackSize.y = minMaxInfo.ptMaxSize.y;

            Marshal.StructureToPtr(minMaxInfo, lParam, false);
            return true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr monitor, ref MonitorInfo monitorInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MinMaxInfo
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MonitorInfo
        {
            public int cbSize;
            public Rect rcMonitor;
            public Rect rcWork;
            public uint dwFlags;
        }
    }

    private enum FlyoutPlacement
    {
        Left,
        Right,
        Top,
        Bottom,
    }
    #endregion
}
#endregion









//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="XWindow.cs" company="VIA.WPF">
////   Copyright (c) VIA.WPF. All rights reserved.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//using CommunityToolkit.Mvvm.Messaging;
//using System.ComponentModel;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using VIA.WPF.Controls.Navigation;
//using VIA.WPF.Themes;
//using Brush = System.Windows.Media.Brush;
//using Brushes = System.Windows.Media.Brushes;
//using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
//using ScrollBar = System.Windows.Controls.Primitives.ScrollBar;
//using TextBoxBase = System.Windows.Controls.Primitives.TextBoxBase;

//namespace VIA.WPF.Windowing;

//#region ### Enum XWindowAnimationMode ###
///// <summary>
///// Defines the supported window startup and close animation modes.
///// </summary>
//public enum XWindowAnimationMode
//{
//    /// <summary>
//    /// No animation is applied.
//    /// </summary>
//    None,

//    /// <summary>
//    /// Applies a fade animation.
//    /// </summary>
//    Fade,

//    /// <summary>
//    /// Applies a fade animation combined with a subtle scale animation.
//    /// </summary>
//    FadeAndScale
//}
//#endregion

//#region ### Enum XToastPlacement ###
///// <summary>
///// Defines the supported toast placement positions.
///// </summary>
//public enum XToastPlacement
//{
//    /// <summary>
//    /// Shows the toast in the top right corner.
//    /// </summary>
//    TopRight,

//    /// <summary>
//    /// Shows the toast in the bottom right corner.
//    /// </summary>
//    BottomRight,

//    /// <summary>
//    /// Shows the toast centered in the window.
//    /// </summary>
//    Center
//}
//#endregion

//#region ### Class XWindow ###
///// <summary>
///// Represents the base window of VIA.WPF.
///// </summary>
//public class XWindow : Window
//{
//    #region ### Dependency Properties ###
//    /// <summary>
//    /// Identifies the <see cref="TitleBarContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
//        nameof(TitleBarContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="TitleBarContentTemplate"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TitleBarContentTemplateProperty = DependencyProperty.Register(
//        nameof(TitleBarContentTemplate),
//        typeof(DataTemplate),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="Subtitle"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
//        nameof(Subtitle),
//        typeof(string),
//        typeof(XWindow),
//        new PropertyMetadata(string.Empty));

//    /// <summary>
//    /// Identifies the <see cref="TitleBarHeight"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
//        nameof(TitleBarHeight),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(35d));

//    /// <summary>
//    /// Identifies the <see cref="CornerRadius"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
//        nameof(CornerRadius),
//        typeof(CornerRadius),
//        typeof(XWindow),
//        new PropertyMetadata(new CornerRadius(6d, 6d, 4d, 4d), OnCornerRadiusChanged));

//    /// <summary>
//    /// Identifies the read-only <see cref="TopCornerRadius"/> dependency property.
//    /// </summary>
//    private static readonly DependencyPropertyKey TopCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
//        nameof(TopCornerRadius),
//        typeof(CornerRadius),
//        typeof(XWindow),
//        new PropertyMetadata(default(CornerRadius)));

//    /// <summary>
//    /// Identifies the <see cref="TopCornerRadius"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TopCornerRadiusProperty = TopCornerRadiusPropertyKey.DependencyProperty;

//    /// <summary>
//    /// Identifies the <see cref="ShowThemeSelector"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowThemeSelectorProperty = DependencyProperty.Register(
//        nameof(ShowThemeSelector),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false));

//    /// <summary>
//    /// Identifies the <see cref="ShowThemeModeButton"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowThemeModeButtonProperty = DependencyProperty.Register(
//        nameof(ShowThemeModeButton),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="ShowMinimizeButton"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(
//        nameof(ShowMinimizeButton),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="ShowMaximizeButton"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(
//        nameof(ShowMaximizeButton),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="ShowCloseButton"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
//        nameof(ShowCloseButton),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="ShowResizeGrip"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ShowResizeGripProperty = DependencyProperty.Register(
//        nameof(ShowResizeGrip),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false));

//    /// <summary>
//    /// Identifies the <see cref="WindowBackgroundBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty WindowBackgroundBrushProperty = DependencyProperty.Register(
//        nameof(WindowBackgroundBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(245, 247, 250)));

//    /// <summary>
//    /// Identifies the <see cref="WindowForegroundBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty WindowForegroundBrushProperty = DependencyProperty.Register(
//        nameof(WindowForegroundBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(17, 24, 39)));

//    /// <summary>
//    /// Identifies the <see cref="WindowBorderBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty WindowBorderBrushProperty = DependencyProperty.Register(
//        nameof(WindowBorderBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(203, 213, 225)));

//    /// <summary>
//    /// Identifies the <see cref="TitleBarBackgroundBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TitleBarBackgroundBrushProperty = DependencyProperty.Register(
//        nameof(TitleBarBackgroundBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(Brushes.White));

//    /// <summary>
//    /// Identifies the <see cref="TitleBarForegroundBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TitleBarForegroundBrushProperty = DependencyProperty.Register(
//        nameof(TitleBarForegroundBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(17, 24, 39)));

//    /// <summary>
//    /// Identifies the <see cref="CaptionButtonHoverBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CaptionButtonHoverBrushProperty = DependencyProperty.Register(
//        nameof(CaptionButtonHoverBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(241, 245, 249)));

//    /// <summary>
//    /// Identifies the <see cref="CaptionButtonPressedBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CaptionButtonPressedBrushProperty = DependencyProperty.Register(
//        nameof(CaptionButtonPressedBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(226, 232, 240)));

//    /// <summary>
//    /// Identifies the <see cref="CloseButtonHoverBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseButtonHoverBrushProperty = DependencyProperty.Register(
//        nameof(CloseButtonHoverBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(232, 17, 35)));

//    /// <summary>
//    /// Identifies the <see cref="CloseButtonPressedBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseButtonPressedBrushProperty = DependencyProperty.Register(
//        nameof(CloseButtonPressedBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(197, 15, 31)));

//    /// <summary>
//    /// Identifies the <see cref="CloseButtonForegroundBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseButtonForegroundBrushProperty = DependencyProperty.Register(
//        nameof(CloseButtonForegroundBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(Brushes.Black));

//    /// <summary>
//    /// Identifies the <see cref="StatusBarContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StatusBarContentProperty = DependencyProperty.Register(
//        nameof(StatusBarContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="StatusBarContentTemplate"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StatusBarContentTemplateProperty = DependencyProperty.Register(
//        nameof(StatusBarContentTemplate),
//        typeof(DataTemplate),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="IsBusy"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
//        nameof(IsBusy),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false));

//    /// <summary>
//    /// Identifies the <see cref="BusyContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
//        nameof(BusyContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata("Bitte warten..."));

//    /// <summary>
//    /// Identifies the <see cref="BusyContentTemplate"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
//        nameof(BusyContentTemplate),
//        typeof(DataTemplate),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="BusyOverlayBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty BusyOverlayBrushProperty = DependencyProperty.Register(
//        nameof(BusyOverlayBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(140, 15, 23, 42)));

//    /// <summary>
//    /// Identifies the <see cref="FlyoutOverlayBrush"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty FlyoutOverlayBrushProperty = DependencyProperty.Register(
//        nameof(FlyoutOverlayBrush),
//        typeof(Brush),
//        typeof(XWindow),
//        new PropertyMetadata(XBrushFactory.CreateFrozenBrush(120, 15, 23, 42)));

//    /// <summary>
//    /// Identifies the <see cref="CloseFlyoutsOnOverlayClick"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseFlyoutsOnOverlayClickProperty = DependencyProperty.Register(
//        nameof(CloseFlyoutsOnOverlayClick),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="LeftFlyoutContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty LeftFlyoutContentProperty = DependencyProperty.Register(
//        nameof(LeftFlyoutContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="RightFlyoutContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty RightFlyoutContentProperty = DependencyProperty.Register(
//        nameof(RightFlyoutContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="TopFlyoutContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TopFlyoutContentProperty = DependencyProperty.Register(
//        nameof(TopFlyoutContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="BottomFlyoutContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty BottomFlyoutContentProperty = DependencyProperty.Register(
//        nameof(BottomFlyoutContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null, OnFlyoutContentPropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="IsLeftFlyoutOpen"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsLeftFlyoutOpenProperty = DependencyProperty.Register(
//        nameof(IsLeftFlyoutOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="IsRightFlyoutOpen"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsRightFlyoutOpenProperty = DependencyProperty.Register(
//        nameof(IsRightFlyoutOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="IsTopFlyoutOpen"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsTopFlyoutOpenProperty = DependencyProperty.Register(
//        nameof(IsTopFlyoutOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="IsBottomFlyoutOpen"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsBottomFlyoutOpenProperty = DependencyProperty.Register(
//        nameof(IsBottomFlyoutOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false, OnFlyoutStatePropertyChanged));

//    /// <summary>
//    /// Identifies the <see cref="LeftFlyoutWidth"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty LeftFlyoutWidthProperty = DependencyProperty.Register(
//        nameof(LeftFlyoutWidth),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(320d));

//    /// <summary>
//    /// Identifies the <see cref="RightFlyoutWidth"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty RightFlyoutWidthProperty = DependencyProperty.Register(
//        nameof(RightFlyoutWidth),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(320d));

//    /// <summary>
//    /// Identifies the <see cref="TopFlyoutHeight"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty TopFlyoutHeightProperty = DependencyProperty.Register(
//        nameof(TopFlyoutHeight),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(240d));

//    /// <summary>
//    /// Identifies the <see cref="BottomFlyoutHeight"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty BottomFlyoutHeightProperty = DependencyProperty.Register(
//        nameof(BottomFlyoutHeight),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(240d));

//    /// <summary>
//    /// Identifies the <see cref="ToastContent"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastContentProperty = DependencyProperty.Register(
//        nameof(ToastContent),
//        typeof(object),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="ToastContentTemplate"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastContentTemplateProperty = DependencyProperty.Register(
//        nameof(ToastContentTemplate),
//        typeof(DataTemplate),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="IsToastOpen"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsToastOpenProperty = DependencyProperty.Register(
//        nameof(IsToastOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false));

//    /// <summary>
//    /// Identifies the <see cref="ToastPlacement"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastPlacementProperty = DependencyProperty.Register(
//        nameof(ToastPlacement),
//        typeof(XToastPlacement),
//        typeof(XWindow),
//        new PropertyMetadata(XToastPlacement.TopRight));

//    /// <summary>
//    /// Identifies the <see cref="UseAnimations"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty UseAnimationsProperty = DependencyProperty.Register(
//        nameof(UseAnimations),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="FlyoutAnimationDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty FlyoutAnimationDurationProperty = DependencyProperty.Register(
//        nameof(FlyoutAnimationDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(220))));

//    /// <summary>
//    /// Identifies the <see cref="FlyoutAnimationOffset"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty FlyoutAnimationOffsetProperty = DependencyProperty.Register(
//        nameof(FlyoutAnimationOffset),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(24d));

//    /// <summary>
//    /// Identifies the <see cref="FlyoutUseFade"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty FlyoutUseFadeProperty = DependencyProperty.Register(
//        nameof(FlyoutUseFade),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="ToastAnimationDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastAnimationDurationProperty = DependencyProperty.Register(
//        nameof(ToastAnimationDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(180))));

//    /// <summary>
//    /// Identifies the <see cref="ToastAnimationOffset"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastAnimationOffsetProperty = DependencyProperty.Register(
//        nameof(ToastAnimationOffset),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(18d));

//    /// <summary>
//    /// Identifies the <see cref="ToastUseFade"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty ToastUseFadeProperty = DependencyProperty.Register(
//        nameof(ToastUseFade),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(true));

//    /// <summary>
//    /// Identifies the <see cref="StartupAnimation"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StartupAnimationProperty = DependencyProperty.Register(
//        nameof(StartupAnimation),
//        typeof(XWindowAnimationMode),
//        typeof(XWindow),
//        new PropertyMetadata(XWindowAnimationMode.None));

//    /// <summary>
//    /// Identifies the <see cref="StartupAnimationDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StartupAnimationDurationProperty = DependencyProperty.Register(
//        nameof(StartupAnimationDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(180))));

//    /// <summary>
//    /// Identifies the <see cref="StartupAnimationScaleDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StartupAnimationScaleDurationProperty = DependencyProperty.Register(
//        nameof(StartupAnimationScaleDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(Duration.Automatic));

//    /// <summary>
//    /// Identifies the <see cref="StartupAnimationInitialScale"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty StartupAnimationInitialScaleProperty = DependencyProperty.Register(
//        nameof(StartupAnimationInitialScale),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(0.985d));

//    /// <summary>
//    /// Identifies the <see cref="CloseAnimation"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseAnimationProperty = DependencyProperty.Register(
//        nameof(CloseAnimation),
//        typeof(XWindowAnimationMode),
//        typeof(XWindow),
//        new PropertyMetadata(XWindowAnimationMode.None));

//    /// <summary>
//    /// Identifies the <see cref="CloseAnimationDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseAnimationDurationProperty = DependencyProperty.Register(
//        nameof(CloseAnimationDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(140))));

//    /// <summary>
//    /// Identifies the <see cref="CloseAnimationScaleDuration"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseAnimationScaleDurationProperty = DependencyProperty.Register(
//        nameof(CloseAnimationScaleDuration),
//        typeof(Duration),
//        typeof(XWindow),
//        new PropertyMetadata(Duration.Automatic));

//    /// <summary>
//    /// Identifies the <see cref="CloseAnimationTargetScale"/> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty CloseAnimationTargetScaleProperty = DependencyProperty.Register(
//        nameof(CloseAnimationTargetScale),
//        typeof(double),
//        typeof(XWindow),
//        new PropertyMetadata(0.99d));


//    /// <summary>
//    /// Identifies the <see cref="EditorOverlayMessage" /> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty EditorOverlayMessageProperty = DependencyProperty.Register(
//        nameof(EditorOverlayMessage),
//        typeof(ShowEditorOverlayMessage),
//        typeof(XWindow),
//        new PropertyMetadata(null));

//    /// <summary>
//    /// Identifies the <see cref="IsEditorOverlayOpen" /> dependency property.
//    /// </summary>
//    public static readonly DependencyProperty IsEditorOverlayOpenProperty = DependencyProperty.Register(
//        nameof(IsEditorOverlayOpen),
//        typeof(bool),
//        typeof(XWindow),
//        new PropertyMetadata(false));
//    #endregion

//    #region ### Private Fields ###
//    private FrameworkElement? _titleBarElement;
//    private Thumb? _resizeGripThumb;
//    private Thumb? _leftResizeThumb;
//    private Thumb? _topResizeThumb;
//    private Thumb? _rightResizeThumb;
//    private Thumb? _bottomResizeThumb;
//    private Thumb? _topLeftResizeThumb;
//    private Thumb? _topRightResizeThumb;
//    private Thumb? _bottomRightResizeThumb;
//    private Thumb? _bottomLeftResizeThumb;
//    private Grid? _flyoutLayer;
//    private Border? _flyoutOverlay;
//    private Border? _leftFlyoutElement;
//    private Border? _rightFlyoutElement;
//    private Border? _topFlyoutElement;
//    private Border? _bottomFlyoutElement;
//    private TranslateTransform? _leftFlyoutTransform;
//    private TranslateTransform? _rightFlyoutTransform;
//    private TranslateTransform? _topFlyoutTransform;
//    private TranslateTransform? _bottomFlyoutTransform;
//    private Border? _windowFrameElement;
//    private Border? _editorOverlayOverlay;
//    private ButtonBase? _editorOverlayCloseButton;
//    private ButtonBase? _editorOverlayCancelButton;
//    private ScaleTransform? _windowFrameScaleTransform;
//    private bool _isStartupAnimationPrepared;
//    private bool _isCloseAnimationRunning;
//    private bool _isCloseAnimationCompleted;
//    #endregion

//    #region ### Constructors ###
//    /// <summary>
//    /// Initializes static members of the <see cref="XWindow"/> class.
//    /// </summary>
//    static XWindow()
//    {
//        DefaultStyleKeyProperty.OverrideMetadata(
//            typeof(XWindow),
//            new FrameworkPropertyMetadata(typeof(XWindow)));
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="XWindow"/> class.
//    /// </summary>
//    public XWindow()
//    {
//        this.WindowStyle = WindowStyle.None;
//        this.AllowsTransparency = true;
//        this.Background = Brushes.Transparent;
//        this.ResizeMode = ResizeMode.CanResize;
//        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
//        this.WindowState = WindowState.Normal;
//        this.ShowInTaskbar = true;

//        this.CommandBindings.Add(new CommandBinding(XWindowCommands.ToggleThemeMode, OnToggleThemeModeExecuted, OnToggleThemeModeCanExecute));
//        this.CommandBindings.Add(new CommandBinding(XWindowCommands.Minimize, OnMinimizeExecuted, OnMinimizeCanExecute));
//        this.CommandBindings.Add(new CommandBinding(XWindowCommands.MaximizeRestore, OnMaximizeRestoreExecuted, OnMaximizeRestoreCanExecute));
//        this.CommandBindings.Add(new CommandBinding(XWindowCommands.Close, OnCloseExecuted, OnCloseCanExecute));

//        this.SourceInitialized += OnWindowSourceInitialized;
//        this.Loaded += OnWindowLoaded;
//        this.Closing += OnWindowClosing;
//        this.Closed += OnWindowClosed;

//        WeakReferenceMessenger.Default.Register<ShowEditorOverlayMessage>(this, static (recipient, message) => ((XWindow)recipient).OnShowEditorOverlayMessage(message));
//        WeakReferenceMessenger.Default.Register<HideEditorOverlayMessage>(this, static (recipient, message) => ((XWindow)recipient).OnHideEditorOverlayMessage(message));

//        this.UpdateDerivedCornerRadii();
//    }
//    #endregion

//    #region ### Public Properties ###
//    public object? TitleBarContent
//    {
//        get => this.GetValue(TitleBarContentProperty);
//        set => this.SetValue(TitleBarContentProperty, value);
//    }

//    public DataTemplate? TitleBarContentTemplate
//    {
//        get => (DataTemplate?)this.GetValue(TitleBarContentTemplateProperty);
//        set => this.SetValue(TitleBarContentTemplateProperty, value);
//    }

//    public string Subtitle
//    {
//        get => (string)this.GetValue(SubtitleProperty);
//        set => this.SetValue(SubtitleProperty, value);
//    }

//    public double TitleBarHeight
//    {
//        get => (double)this.GetValue(TitleBarHeightProperty);
//        set => this.SetValue(TitleBarHeightProperty, value);
//    }

//    public CornerRadius CornerRadius
//    {
//        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
//        set => this.SetValue(CornerRadiusProperty, value);
//    }

//    public CornerRadius TopCornerRadius => (CornerRadius)this.GetValue(TopCornerRadiusProperty);

//    public bool ShowThemeSelector
//    {
//        get => (bool)this.GetValue(ShowThemeSelectorProperty);
//        set => this.SetValue(ShowThemeSelectorProperty, value);
//    }

//    public bool ShowThemeModeButton
//    {
//        get => (bool)this.GetValue(ShowThemeModeButtonProperty);
//        set => this.SetValue(ShowThemeModeButtonProperty, value);
//    }

//    public bool ShowMinimizeButton
//    {
//        get => (bool)this.GetValue(ShowMinimizeButtonProperty);
//        set => this.SetValue(ShowMinimizeButtonProperty, value);
//    }

//    public bool ShowMaximizeButton
//    {
//        get => (bool)this.GetValue(ShowMaximizeButtonProperty);
//        set => this.SetValue(ShowMaximizeButtonProperty, value);
//    }

//    public bool ShowCloseButton
//    {
//        get => (bool)this.GetValue(ShowCloseButtonProperty);
//        set => this.SetValue(ShowCloseButtonProperty, value);
//    }

//    public bool ShowResizeGrip
//    {
//        get => (bool)this.GetValue(ShowResizeGripProperty);
//        set => this.SetValue(ShowResizeGripProperty, value);
//    }

//    public Brush WindowBackgroundBrush
//    {
//        get => (Brush)this.GetValue(WindowBackgroundBrushProperty);
//        set => this.SetValue(WindowBackgroundBrushProperty, value);
//    }

//    public Brush WindowForegroundBrush
//    {
//        get => (Brush)this.GetValue(WindowForegroundBrushProperty);
//        set => this.SetValue(WindowForegroundBrushProperty, value);
//    }

//    public Brush WindowBorderBrush
//    {
//        get => (Brush)this.GetValue(WindowBorderBrushProperty);
//        set => this.SetValue(WindowBorderBrushProperty, value);
//    }

//    public Brush TitleBarBackgroundBrush
//    {
//        get => (Brush)this.GetValue(TitleBarBackgroundBrushProperty);
//        set => this.SetValue(TitleBarBackgroundBrushProperty, value);
//    }

//    public Brush TitleBarForegroundBrush
//    {
//        get => (Brush)this.GetValue(TitleBarForegroundBrushProperty);
//        set => this.SetValue(TitleBarForegroundBrushProperty, value);
//    }

//    public Brush CaptionButtonHoverBrush
//    {
//        get => (Brush)this.GetValue(CaptionButtonHoverBrushProperty);
//        set => this.SetValue(CaptionButtonHoverBrushProperty, value);
//    }

//    public Brush CaptionButtonPressedBrush
//    {
//        get => (Brush)this.GetValue(CaptionButtonPressedBrushProperty);
//        set => this.SetValue(CaptionButtonPressedBrushProperty, value);
//    }

//    public Brush CloseButtonHoverBrush
//    {
//        get => (Brush)this.GetValue(CloseButtonHoverBrushProperty);
//        set => this.SetValue(CloseButtonHoverBrushProperty, value);
//    }

//    public Brush CloseButtonPressedBrush
//    {
//        get => (Brush)this.GetValue(CloseButtonPressedBrushProperty);
//        set => this.SetValue(CloseButtonPressedBrushProperty, value);
//    }

//    public Brush CloseButtonForegroundBrush
//    {
//        get => (Brush)this.GetValue(CloseButtonForegroundBrushProperty);
//        set => this.SetValue(CloseButtonForegroundBrushProperty, value);
//    }

//    public object? StatusBarContent
//    {
//        get => this.GetValue(StatusBarContentProperty);
//        set => this.SetValue(StatusBarContentProperty, value);
//    }

//    public DataTemplate? StatusBarContentTemplate
//    {
//        get => (DataTemplate?)this.GetValue(StatusBarContentTemplateProperty);
//        set => this.SetValue(StatusBarContentTemplateProperty, value);
//    }

//    public bool IsBusy
//    {
//        get => (bool)this.GetValue(IsBusyProperty);
//        set => this.SetValue(IsBusyProperty, value);
//    }

//    public object? BusyContent
//    {
//        get => this.GetValue(BusyContentProperty);
//        set => this.SetValue(BusyContentProperty, value);
//    }

//    public DataTemplate? BusyContentTemplate
//    {
//        get => (DataTemplate?)this.GetValue(BusyContentTemplateProperty);
//        set => this.SetValue(BusyContentTemplateProperty, value);
//    }

//    public Brush BusyOverlayBrush
//    {
//        get => (Brush)this.GetValue(BusyOverlayBrushProperty);
//        set => this.SetValue(BusyOverlayBrushProperty, value);
//    }

//    public Brush FlyoutOverlayBrush
//    {
//        get => (Brush)this.GetValue(FlyoutOverlayBrushProperty);
//        set => this.SetValue(FlyoutOverlayBrushProperty, value);
//    }

//    public bool CloseFlyoutsOnOverlayClick
//    {
//        get => (bool)this.GetValue(CloseFlyoutsOnOverlayClickProperty);
//        set => this.SetValue(CloseFlyoutsOnOverlayClickProperty, value);
//    }

//    public object? LeftFlyoutContent
//    {
//        get => this.GetValue(LeftFlyoutContentProperty);
//        set => this.SetValue(LeftFlyoutContentProperty, value);
//    }

//    public object? RightFlyoutContent
//    {
//        get => this.GetValue(RightFlyoutContentProperty);
//        set => this.SetValue(RightFlyoutContentProperty, value);
//    }

//    public object? TopFlyoutContent
//    {
//        get => this.GetValue(TopFlyoutContentProperty);
//        set => this.SetValue(TopFlyoutContentProperty, value);
//    }

//    public object? BottomFlyoutContent
//    {
//        get => this.GetValue(BottomFlyoutContentProperty);
//        set => this.SetValue(BottomFlyoutContentProperty, value);
//    }

//    public bool IsLeftFlyoutOpen
//    {
//        get => (bool)this.GetValue(IsLeftFlyoutOpenProperty);
//        set => this.SetValue(IsLeftFlyoutOpenProperty, value);
//    }

//    public bool IsRightFlyoutOpen
//    {
//        get => (bool)this.GetValue(IsRightFlyoutOpenProperty);
//        set => this.SetValue(IsRightFlyoutOpenProperty, value);
//    }

//    public bool IsTopFlyoutOpen
//    {
//        get => (bool)this.GetValue(IsTopFlyoutOpenProperty);
//        set => this.SetValue(IsTopFlyoutOpenProperty, value);
//    }

//    public bool IsBottomFlyoutOpen
//    {
//        get => (bool)this.GetValue(IsBottomFlyoutOpenProperty);
//        set => this.SetValue(IsBottomFlyoutOpenProperty, value);
//    }

//    public double LeftFlyoutWidth
//    {
//        get => (double)this.GetValue(LeftFlyoutWidthProperty);
//        set => this.SetValue(LeftFlyoutWidthProperty, value);
//    }

//    public double RightFlyoutWidth
//    {
//        get => (double)this.GetValue(RightFlyoutWidthProperty);
//        set => this.SetValue(RightFlyoutWidthProperty, value);
//    }

//    public double TopFlyoutHeight
//    {
//        get => (double)this.GetValue(TopFlyoutHeightProperty);
//        set => this.SetValue(TopFlyoutHeightProperty, value);
//    }

//    public double BottomFlyoutHeight
//    {
//        get => (double)this.GetValue(BottomFlyoutHeightProperty);
//        set => this.SetValue(BottomFlyoutHeightProperty, value);
//    }

//    public object? ToastContent
//    {
//        get => this.GetValue(ToastContentProperty);
//        set => this.SetValue(ToastContentProperty, value);
//    }

//    public DataTemplate? ToastContentTemplate
//    {
//        get => (DataTemplate?)this.GetValue(ToastContentTemplateProperty);
//        set => this.SetValue(ToastContentTemplateProperty, value);
//    }

//    public bool IsToastOpen
//    {
//        get => (bool)this.GetValue(IsToastOpenProperty);
//        set => this.SetValue(IsToastOpenProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the toast placement.
//    /// </summary>
//    public XToastPlacement ToastPlacement
//    {
//        get => (XToastPlacement)this.GetValue(ToastPlacementProperty);
//        set => this.SetValue(ToastPlacementProperty, value);
//    }

//    public bool UseAnimations
//    {
//        get => (bool)this.GetValue(UseAnimationsProperty);
//        set => this.SetValue(UseAnimationsProperty, value);
//    }

//    public Duration FlyoutAnimationDuration
//    {
//        get => (Duration)this.GetValue(FlyoutAnimationDurationProperty);
//        set => this.SetValue(FlyoutAnimationDurationProperty, value);
//    }

//    public double FlyoutAnimationOffset
//    {
//        get => (double)this.GetValue(FlyoutAnimationOffsetProperty);
//        set => this.SetValue(FlyoutAnimationOffsetProperty, value);
//    }

//    public bool FlyoutUseFade
//    {
//        get => (bool)this.GetValue(FlyoutUseFadeProperty);
//        set => this.SetValue(FlyoutUseFadeProperty, value);
//    }

//    public Duration ToastAnimationDuration
//    {
//        get => (Duration)this.GetValue(ToastAnimationDurationProperty);
//        set => this.SetValue(ToastAnimationDurationProperty, value);
//    }

//    public double ToastAnimationOffset
//    {
//        get => (double)this.GetValue(ToastAnimationOffsetProperty);
//        set => this.SetValue(ToastAnimationOffsetProperty, value);
//    }

//    public bool ToastUseFade
//    {
//        get => (bool)this.GetValue(ToastUseFadeProperty);
//        set => this.SetValue(ToastUseFadeProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the startup animation mode.
//    /// </summary>
//    public XWindowAnimationMode StartupAnimation
//    {
//        get => (XWindowAnimationMode)this.GetValue(StartupAnimationProperty);
//        set => this.SetValue(StartupAnimationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the startup fade animation duration.
//    /// </summary>
//    public Duration StartupAnimationDuration
//    {
//        get => (Duration)this.GetValue(StartupAnimationDurationProperty);
//        set => this.SetValue(StartupAnimationDurationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the optional startup scale animation duration.
//    /// If not set explicitly, a shorter duration is derived automatically from <see cref="StartupAnimationDuration"/>.
//    /// </summary>
//    public Duration StartupAnimationScaleDuration
//    {
//        get => (Duration)this.GetValue(StartupAnimationScaleDurationProperty);
//        set => this.SetValue(StartupAnimationScaleDurationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the initial startup scale that is used for <see cref="XWindowAnimationMode.FadeAndScale"/>.
//    /// </summary>
//    public double StartupAnimationInitialScale
//    {
//        get => (double)this.GetValue(StartupAnimationInitialScaleProperty);
//        set => this.SetValue(StartupAnimationInitialScaleProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the close animation mode.
//    /// </summary>
//    public XWindowAnimationMode CloseAnimation
//    {
//        get => (XWindowAnimationMode)this.GetValue(CloseAnimationProperty);
//        set => this.SetValue(CloseAnimationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the close fade animation duration.
//    /// </summary>
//    public Duration CloseAnimationDuration
//    {
//        get => (Duration)this.GetValue(CloseAnimationDurationProperty);
//        set => this.SetValue(CloseAnimationDurationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the optional close scale animation duration.
//    /// If not set explicitly, a shorter duration is derived automatically from <see cref="CloseAnimationDuration"/>.
//    /// </summary>
//    public Duration CloseAnimationScaleDuration
//    {
//        get => (Duration)this.GetValue(CloseAnimationScaleDurationProperty);
//        set => this.SetValue(CloseAnimationScaleDurationProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets the close animation target scale that is used for <see cref="XWindowAnimationMode.FadeAndScale"/>.
//    /// </summary>
//    public double CloseAnimationTargetScale
//    {
//        get => (double)this.GetValue(CloseAnimationTargetScaleProperty);
//        set => this.SetValue(CloseAnimationTargetScaleProperty, value);
//    }


//    /// <summary>
//    /// Gets or sets the current editor overlay message.
//    /// </summary>
//    public ShowEditorOverlayMessage? EditorOverlayMessage
//    {
//        get => (ShowEditorOverlayMessage?)this.GetValue(EditorOverlayMessageProperty);
//        set => this.SetValue(EditorOverlayMessageProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets a value indicating whether the editor overlay is open.
//    /// </summary>
//    public bool IsEditorOverlayOpen
//    {
//        get => (bool)this.GetValue(IsEditorOverlayOpenProperty);
//        set => this.SetValue(IsEditorOverlayOpenProperty, value);
//    }
//    #endregion

//    #region ### Public Methods ###
//    /// <summary>
//    /// Applies the control template and connects template parts.
//    /// </summary>
//    public override void OnApplyTemplate()
//    {
//        base.OnApplyTemplate();

//        if (this._titleBarElement is not null)
//        {
//            this._titleBarElement.MouseLeftButtonDown -= OnTitleBarMouseLeftButtonDown;
//        }

//        DetachResizeThumb(this._resizeGripThumb, OnResizeGripDragDelta);
//        DetachResizeThumb(this._leftResizeThumb, OnLeftResizeThumbDragDelta);
//        DetachResizeThumb(this._topResizeThumb, OnTopResizeThumbDragDelta);
//        DetachResizeThumb(this._rightResizeThumb, OnRightResizeThumbDragDelta);
//        DetachResizeThumb(this._bottomResizeThumb, OnBottomResizeThumbDragDelta);
//        DetachResizeThumb(this._topLeftResizeThumb, OnTopLeftResizeThumbDragDelta);
//        DetachResizeThumb(this._topRightResizeThumb, OnTopRightResizeThumbDragDelta);
//        DetachResizeThumb(this._bottomRightResizeThumb, OnBottomRightResizeThumbDragDelta);
//        DetachResizeThumb(this._bottomLeftResizeThumb, OnBottomLeftResizeThumbDragDelta);

//        if (this._flyoutOverlay is not null)
//        {
//            this._flyoutOverlay.MouseLeftButtonDown -= OnFlyoutOverlayMouseLeftButtonDown;
//        }

//        if (this._editorOverlayOverlay is not null)
//        {
//            this._editorOverlayOverlay.MouseLeftButtonUp -= this.OnEditorOverlayMouseLeftButtonUp;
//        }

//        if (this._editorOverlayCloseButton is not null)
//        {
//            this._editorOverlayCloseButton.Click -= this.OnEditorOverlayCloseButtonClick;
//        }

//        if (this._editorOverlayCancelButton is not null)
//        {
//            this._editorOverlayCancelButton.Click -= this.OnEditorOverlayCancelButtonClick;
//        }

//        this._titleBarElement = this.GetTemplateChild("PART_TitleBar") as FrameworkElement;
//        this._resizeGripThumb = this.GetTemplateChild("PART_ResizeGrip") as Thumb;
//        this._leftResizeThumb = this.GetTemplateChild("PART_LeftResizeThumb") as Thumb;
//        this._topResizeThumb = this.GetTemplateChild("PART_TopResizeThumb") as Thumb;
//        this._rightResizeThumb = this.GetTemplateChild("PART_RightResizeThumb") as Thumb;
//        this._bottomResizeThumb = this.GetTemplateChild("PART_BottomResizeThumb") as Thumb;
//        this._topLeftResizeThumb = this.GetTemplateChild("PART_TopLeftResizeThumb") as Thumb;
//        this._topRightResizeThumb = this.GetTemplateChild("PART_TopRightResizeThumb") as Thumb;
//        this._bottomRightResizeThumb = this.GetTemplateChild("PART_BottomRightResizeThumb") as Thumb;
//        this._bottomLeftResizeThumb = this.GetTemplateChild("PART_BottomLeftResizeThumb") as Thumb;
//        this._flyoutLayer = this.GetTemplateChild("PART_FlyoutLayer") as Grid;
//        this._flyoutOverlay = this.GetTemplateChild("PART_FlyoutOverlay") as Border;
//        this._leftFlyoutElement = this.GetTemplateChild("PART_LeftFlyout") as Border;
//        this._rightFlyoutElement = this.GetTemplateChild("PART_RightFlyout") as Border;
//        this._topFlyoutElement = this.GetTemplateChild("PART_TopFlyout") as Border;
//        this._bottomFlyoutElement = this.GetTemplateChild("PART_BottomFlyout") as Border;
//        this._leftFlyoutTransform = this.GetTemplateChild("PART_LeftFlyoutTransform") as TranslateTransform;
//        this._rightFlyoutTransform = this.GetTemplateChild("PART_RightFlyoutTransform") as TranslateTransform;
//        this._topFlyoutTransform = this.GetTemplateChild("PART_TopFlyoutTransform") as TranslateTransform;
//        this._bottomFlyoutTransform = this.GetTemplateChild("PART_BottomFlyoutTransform") as TranslateTransform;
//        this._windowFrameElement = this.GetTemplateChild("PART_WindowFrame") as Border;
//        this._editorOverlayOverlay = this.GetTemplateChild("PART_EditorOverlayOverlay") as Border;
//        this._editorOverlayCloseButton = this.GetTemplateChild("PART_EditorOverlayCloseButton") as ButtonBase;
//        this._editorOverlayCancelButton = this.GetTemplateChild("PART_EditorOverlayCancelButton") as ButtonBase;

//        this.EnsureWindowFrameAnimationTransform();
//        this.PrepareStartupAnimationState();

//        if (this._titleBarElement is not null)
//        {
//            this._titleBarElement.MouseLeftButtonDown += OnTitleBarMouseLeftButtonDown;
//        }

//        if (this._flyoutOverlay is not null)
//        {
//            this._flyoutOverlay.MouseLeftButtonDown += OnFlyoutOverlayMouseLeftButtonDown;
//        }

//        if (this._editorOverlayOverlay is not null)
//        {
//            this._editorOverlayOverlay.MouseLeftButtonUp += this.OnEditorOverlayMouseLeftButtonUp;
//        }

//        if (this._editorOverlayCloseButton is not null)
//        {
//            this._editorOverlayCloseButton.Click += this.OnEditorOverlayCloseButtonClick;
//        }

//        if (this._editorOverlayCancelButton is not null)
//        {
//            this._editorOverlayCancelButton.Click += this.OnEditorOverlayCancelButtonClick;
//        }

//        AttachResizeThumb(this._resizeGripThumb, OnResizeGripDragDelta);
//        AttachResizeThumb(this._leftResizeThumb, OnLeftResizeThumbDragDelta);
//        AttachResizeThumb(this._topResizeThumb, OnTopResizeThumbDragDelta);
//        AttachResizeThumb(this._rightResizeThumb, OnRightResizeThumbDragDelta);
//        AttachResizeThumb(this._bottomResizeThumb, OnBottomResizeThumbDragDelta);
//        AttachResizeThumb(this._topLeftResizeThumb, OnTopLeftResizeThumbDragDelta);
//        AttachResizeThumb(this._topRightResizeThumb, OnTopRightResizeThumbDragDelta);
//        AttachResizeThumb(this._bottomRightResizeThumb, OnBottomRightResizeThumbDragDelta);
//        AttachResizeThumb(this._bottomLeftResizeThumb, OnBottomLeftResizeThumbDragDelta);

//        this.UpdateAllFlyoutStates(false);
//    }
//    #endregion

//    #region ### Private Methods ###
//    private void OnWindowSourceInitialized(object? sender, EventArgs e)
//    {
//        this.PrepareStartupAnimationState();
//    }

//    private void OnWindowLoaded(object sender, RoutedEventArgs e)
//    {
//        this.BeginStartupAnimation();
//    }

//    private void OnWindowClosing(object? sender, CancelEventArgs e)
//    {
//        if (this._isCloseAnimationCompleted)
//        {
//            return;
//        }

//        if (!this.ShouldAnimateClose())
//        {
//            return;
//        }

//        e.Cancel = true;
//        this.BeginCloseAnimation();
//    }

//    private void OnWindowClosed(object? sender, EventArgs e)
//    {
//        WeakReferenceMessenger.Default.UnregisterAll(this);
//        this.EditorOverlayMessage = null;
//        this.IsEditorOverlayOpen = false;
//    }

//    private void OnShowEditorOverlayMessage(ShowEditorOverlayMessage message)
//    {
//        if (!ReferenceEquals(message.TargetWindow, this))
//        {
//            return;
//        }

//        this.EditorOverlayMessage = message;
//        this.IsEditorOverlayOpen = true;
//        message.Handled = true;
//    }

//    private void OnHideEditorOverlayMessage(HideEditorOverlayMessage message)
//    {
//        if (message.TargetWindow is not null && !ReferenceEquals(message.TargetWindow, this))
//        {
//            return;
//        }

//        if (!ReferenceEquals(this.EditorOverlayMessage?.Owner, message.Owner))
//        {
//            return;
//        }

//        this.IsEditorOverlayOpen = false;
//        this.EditorOverlayMessage = null;
//        message.Handled = true;
//    }

//    private void OnEditorOverlayMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//    {
//        ShowEditorOverlayMessage? message = this.EditorOverlayMessage;
//        if (message?.CanCloseOnOverlayClick != true)
//        {
//            return;
//        }

//        message.Close();
//        e.Handled = true;
//    }

//    private void OnEditorOverlayCloseButtonClick(object sender, RoutedEventArgs e)
//    {
//        this.EditorOverlayMessage?.Close();
//        e.Handled = true;
//    }

//    private void OnEditorOverlayCancelButtonClick(object sender, RoutedEventArgs e)
//    {
//        this.EditorOverlayMessage?.Close();
//        e.Handled = true;
//    }

//    private bool ShouldAnimateStartup()
//    {
//        return this.UseAnimations
//               && this.StartupAnimation != XWindowAnimationMode.None
//               && !DesignerProperties.GetIsInDesignMode(this)
//               && !this._isCloseAnimationRunning;
//    }

//    private bool ShouldAnimateClose()
//    {
//        return this.UseAnimations
//               && this.CloseAnimation != XWindowAnimationMode.None
//               && !DesignerProperties.GetIsInDesignMode(this)
//               && this.IsLoaded
//               && !this._isCloseAnimationRunning
//               && !this._isCloseAnimationCompleted;
//    }

//    private void PrepareStartupAnimationState()
//    {
//        if (this._isStartupAnimationPrepared || !this.ShouldAnimateStartup())
//        {
//            return;
//        }

//        this.Opacity = 0.62d;
//        this.EnsureWindowFrameAnimationTransform();

//        if (this.StartupAnimation == XWindowAnimationMode.FadeAndScale && this._windowFrameScaleTransform is not null)
//        {
//            this._windowFrameScaleTransform.ScaleX = this.StartupAnimationInitialScale;
//            this._windowFrameScaleTransform.ScaleY = this.StartupAnimationInitialScale;
//        }

//        this._isStartupAnimationPrepared = true;
//    }

//    private void BeginStartupAnimation()
//    {
//        if (!this._isStartupAnimationPrepared || !this.ShouldAnimateStartup())
//        {
//            return;
//        }

//        this.BeginAnimation(Window.OpacityProperty, null);
//        Storyboard? storyboard = this.CreateWindowAnimationStoryboard(
//            this.StartupAnimation,
//            this.StartupAnimationDuration,
//            ResolveScaleAnimationDuration(this.StartupAnimationDuration, this.StartupAnimationScaleDuration, true),
//            this.Opacity,
//            1d,
//            this.StartupAnimationInitialScale,
//            1d);

//        if (storyboard is null)
//        {
//            this.ResetAnimationState();
//            return;
//        }

//        storyboard.Completed += (_, _) =>
//        {
//            this.Opacity = 1d;
//            this.ResetAnimationState();
//        };

//        storyboard.Begin();
//    }

//    private void BeginCloseAnimation()
//    {
//        if (!this.ShouldAnimateClose())
//        {
//            this.Close();
//            return;
//        }

//        this._isCloseAnimationRunning = true;
//        this.BeginAnimation(Window.OpacityProperty, null);

//        Storyboard? storyboard = this.CreateWindowAnimationStoryboard(
//            this.CloseAnimation,
//            this.CloseAnimationDuration,
//            ResolveScaleAnimationDuration(this.CloseAnimationDuration, this.CloseAnimationScaleDuration, false),
//            this.Opacity,
//            0d,
//            1d,
//            this.CloseAnimationTargetScale);

//        if (storyboard is null)
//        {
//            this._isCloseAnimationRunning = false;
//            this._isCloseAnimationCompleted = true;
//            this.Close();
//            return;
//        }

//        storyboard.Completed += (_, _) =>
//        {
//            this.BeginAnimation(Window.OpacityProperty, null);
//            this.Opacity = 0d;
//            this._isCloseAnimationRunning = false;
//            this._isCloseAnimationCompleted = true;
//            this.Close();
//        };

//        storyboard.Begin();
//    }

//    private static Duration ResolveScaleAnimationDuration(Duration opacityDuration, Duration configuredScaleDuration, bool isStartup)
//    {
//        if (configuredScaleDuration.HasTimeSpan)
//        {
//            return configuredScaleDuration;
//        }

//        if (!opacityDuration.HasTimeSpan)
//        {
//            return opacityDuration;
//        }

//        double opacityMilliseconds = opacityDuration.TimeSpan.TotalMilliseconds;
//        double minimumMilliseconds = isStartup ? 120d : 100d;
//        double maximumMilliseconds = isStartup ? 180d : 160d;
//        double preferredMilliseconds = Math.Max(opacityMilliseconds * 0.6d, minimumMilliseconds);
//        double scaleMilliseconds = Math.Min(opacityMilliseconds, Math.Min(preferredMilliseconds, maximumMilliseconds));

//        return new Duration(TimeSpan.FromMilliseconds(scaleMilliseconds));
//    }

//    private Storyboard? CreateWindowAnimationStoryboard(
//        XWindowAnimationMode animationMode,
//        Duration opacityDuration,
//        Duration scaleDuration,
//        double fromOpacity,
//        double toOpacity,
//        double fromScale,
//        double toScale)
//    {
//        Storyboard storyboard = new();

//        DoubleAnimation opacityAnimation = new()
//        {
//            From = fromOpacity,
//            To = toOpacity,
//            Duration = opacityDuration,
//            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
//        };

//        Storyboard.SetTarget(opacityAnimation, this);
//        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Window.OpacityProperty));
//        storyboard.Children.Add(opacityAnimation);

//        if (animationMode == XWindowAnimationMode.FadeAndScale)
//        {
//            this.EnsureWindowFrameAnimationTransform();

//            if (this._windowFrameScaleTransform is null)
//            {
//                return storyboard;
//            }

//            DoubleAnimation scaleXAnimation = new()
//            {
//                From = fromScale,
//                To = toScale,
//                Duration = scaleDuration,
//                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
//            };

//            DoubleAnimation scaleYAnimation = new()
//            {
//                From = fromScale,
//                To = toScale,
//                Duration = scaleDuration,
//                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
//            };

//            Storyboard.SetTarget(scaleXAnimation, this._windowFrameScaleTransform);
//            Storyboard.SetTarget(scaleYAnimation, this._windowFrameScaleTransform);
//            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));
//            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath(ScaleTransform.ScaleYProperty));

//            storyboard.Children.Add(scaleXAnimation);
//            storyboard.Children.Add(scaleYAnimation);
//        }

//        return storyboard;
//    }

//    private void EnsureWindowFrameAnimationTransform()
//    {
//        if (this._windowFrameElement is null)
//        {
//            return;
//        }

//        this._windowFrameElement.RenderTransformOrigin = new Point(0.5d, 0.5d);

//        if (this._windowFrameElement.RenderTransform is ScaleTransform scaleTransform)
//        {
//            this._windowFrameScaleTransform = scaleTransform;
//            return;
//        }

//        if (this._windowFrameElement.RenderTransform is TransformGroup transformGroup)
//        {
//            ScaleTransform? existingScaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault();
//            if (existingScaleTransform is not null)
//            {
//                this._windowFrameScaleTransform = existingScaleTransform;
//                return;
//            }

//            this._windowFrameScaleTransform = new ScaleTransform(1d, 1d);
//            transformGroup.Children.Insert(0, this._windowFrameScaleTransform);
//            return;
//        }

//        this._windowFrameScaleTransform = new ScaleTransform(1d, 1d);
//        this._windowFrameElement.RenderTransform = this._windowFrameScaleTransform;
//    }

//    private void ResetAnimationState()
//    {
//        this._isStartupAnimationPrepared = false;

//        if (this._windowFrameScaleTransform is not null)
//        {
//            this._windowFrameScaleTransform.ScaleX = 1d;
//            this._windowFrameScaleTransform.ScaleY = 1d;
//        }
//    }

//    private static void OnFlyoutStatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
//    {
//        if (dependencyObject is XWindow window)
//        {
//            window.UpdateAllFlyoutStates(true);
//        }
//    }

//    private static void OnFlyoutContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
//    {
//        if (dependencyObject is not XWindow window)
//        {
//            return;
//        }

//        if (ReferenceEquals(e.Property, LeftFlyoutContentProperty) && e.NewValue is null)
//        {
//            window.IsLeftFlyoutOpen = false;
//        }
//        else if (ReferenceEquals(e.Property, RightFlyoutContentProperty) && e.NewValue is null)
//        {
//            window.IsRightFlyoutOpen = false;
//        }
//        else if (ReferenceEquals(e.Property, TopFlyoutContentProperty) && e.NewValue is null)
//        {
//            window.IsTopFlyoutOpen = false;
//        }
//        else if (ReferenceEquals(e.Property, BottomFlyoutContentProperty) && e.NewValue is null)
//        {
//            window.IsBottomFlyoutOpen = false;
//        }

//        window.UpdateAllFlyoutStates(false);
//    }

//    private static void OnCornerRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
//    {
//        if (dependencyObject is XWindow window)
//        {
//            window.UpdateDerivedCornerRadii();
//        }
//    }

//    private static void OnToggleThemeModeCanExecute(object sender, CanExecuteRoutedEventArgs e)
//    {
//        e.CanExecute = true;
//        e.Handled = true;
//    }

//    private static async void OnToggleThemeModeExecuted(object sender, ExecutedRoutedEventArgs e)
//    {
//        if (sender is XWindow window)
//        {
//            await XThemeTransitionHelper.AnimateThemeChangeAsync(
//                window,
//                XThemeManager.Current.ToggleMode);
//        }
//        else
//        {
//            XThemeManager.Current.ToggleMode();
//        }

//        e.Handled = true;
//    }

//    private static void OnMinimizeCanExecute(object sender, CanExecuteRoutedEventArgs e)
//    {
//        if (sender is XWindow window)
//        {
//            e.CanExecute = window.ResizeMode is not ResizeMode.NoResize;
//            e.Handled = true;
//        }
//    }

//    private static void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
//    {
//        if (sender is XWindow window)
//        {
//            SystemCommands.MinimizeWindow(window);
//            e.Handled = true;
//        }
//    }

//    private static void OnMaximizeRestoreCanExecute(object sender, CanExecuteRoutedEventArgs e)
//    {
//        if (sender is XWindow window)
//        {
//            e.CanExecute = window.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
//            e.Handled = true;
//        }
//    }

//    private static void OnMaximizeRestoreExecuted(object sender, ExecutedRoutedEventArgs e)
//    {
//        if (sender is not XWindow window)
//        {
//            return;
//        }

//        if (window.WindowState == WindowState.Maximized)
//        {
//            SystemCommands.RestoreWindow(window);
//        }
//        else
//        {
//            SystemCommands.MaximizeWindow(window);
//        }

//        e.Handled = true;
//    }

//    private static void OnCloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
//    {
//        e.CanExecute = true;
//        e.Handled = true;
//    }

//    private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
//    {
//        if (sender is XWindow window)
//        {
//            SystemCommands.CloseWindow(window);
//            e.Handled = true;
//        }
//    }

//    private void OnTitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//    {
//        if (e.ChangedButton != MouseButton.Left)
//        {
//            return;
//        }

//        if (IsInsideInteractiveElement(e.OriginalSource as DependencyObject))
//        {
//            return;
//        }

//        if (e.ClickCount == 2 && this.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip)
//        {
//            if (this.WindowState == WindowState.Maximized)
//            {
//                SystemCommands.RestoreWindow(this);
//            }
//            else
//            {
//                SystemCommands.MaximizeWindow(this);
//            }

//            return;
//        }

//        try
//        {
//            this.DragMove();
//        }
//        catch
//        {
//        }
//    }

//    private void OnFlyoutOverlayMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//    {
//        if (!this.CloseFlyoutsOnOverlayClick)
//        {
//            return;
//        }

//        this.CloseAllFlyouts();
//        e.Handled = true;
//    }

//    private void CloseAllFlyouts()
//    {
//        this.IsLeftFlyoutOpen = false;
//        this.IsRightFlyoutOpen = false;
//        this.IsTopFlyoutOpen = false;
//        this.IsBottomFlyoutOpen = false;
//    }

//    private void UpdateAllFlyoutStates(bool useTransitions)
//    {
//        this.UpdateFlyoutState(this._leftFlyoutElement, this._leftFlyoutTransform, this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null, FlyoutPlacement.Left, useTransitions);
//        this.UpdateFlyoutState(this._rightFlyoutElement, this._rightFlyoutTransform, this.IsRightFlyoutOpen && this.RightFlyoutContent is not null, FlyoutPlacement.Right, useTransitions);
//        this.UpdateFlyoutState(this._topFlyoutElement, this._topFlyoutTransform, this.IsTopFlyoutOpen && this.TopFlyoutContent is not null, FlyoutPlacement.Top, useTransitions);
//        this.UpdateFlyoutState(this._bottomFlyoutElement, this._bottomFlyoutTransform, this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null, FlyoutPlacement.Bottom, useTransitions);

//        bool isAnyFlyoutOpen = (this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null)
//                               || (this.IsRightFlyoutOpen && this.RightFlyoutContent is not null)
//                               || (this.IsTopFlyoutOpen && this.TopFlyoutContent is not null)
//                               || (this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null);

//        this.UpdateFlyoutOverlayState(isAnyFlyoutOpen, useTransitions);

//        if (this._flyoutLayer is not null)
//        {
//            this._flyoutLayer.IsHitTestVisible = isAnyFlyoutOpen;
//        }
//    }

//    private void UpdateFlyoutOverlayState(bool isVisible, bool useTransitions)
//    {
//        if (this._flyoutOverlay is null)
//        {
//            return;
//        }

//        this._flyoutOverlay.BeginAnimation(UIElement.OpacityProperty, null);

//        if (!useTransitions || !this.UseAnimations)
//        {
//            this._flyoutOverlay.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
//            this._flyoutOverlay.Opacity = isVisible ? 1d : 0d;
//            return;
//        }

//        Duration duration = this.FlyoutAnimationDuration;

//        if (isVisible)
//        {
//            this._flyoutOverlay.Visibility = Visibility.Visible;
//            this._flyoutOverlay.BeginAnimation(
//                UIElement.OpacityProperty,
//                new DoubleAnimation
//                {
//                    From = this._flyoutOverlay.Opacity,
//                    To = 1d,
//                    Duration = duration
//                });
//        }
//        else
//        {
//            DoubleAnimation animation = new()
//            {
//                From = this._flyoutOverlay.Opacity,
//                To = 0d,
//                Duration = duration
//            };

//            animation.Completed += (_, _) =>
//            {
//                if (!this.HasAnyFlyoutOpen() && this._flyoutOverlay is not null)
//                {
//                    this._flyoutOverlay.Visibility = Visibility.Collapsed;
//                    this._flyoutOverlay.Opacity = 0d;
//                }
//            };

//            this._flyoutOverlay.BeginAnimation(UIElement.OpacityProperty, animation);
//        }
//    }

//    private void UpdateFlyoutState(Border? flyoutElement, TranslateTransform? transform, bool isOpen, FlyoutPlacement placement, bool useTransitions)
//    {
//        if (flyoutElement is null || transform is null)
//        {
//            return;
//        }

//        double closedOffset = placement switch
//        {
//            FlyoutPlacement.Left => -this.FlyoutAnimationOffset,
//            FlyoutPlacement.Right => this.FlyoutAnimationOffset,
//            FlyoutPlacement.Top => -this.FlyoutAnimationOffset,
//            _ => this.FlyoutAnimationOffset,
//        };

//        transform.BeginAnimation(placement is FlyoutPlacement.Left or FlyoutPlacement.Right ? TranslateTransform.XProperty : TranslateTransform.YProperty, null);
//        flyoutElement.BeginAnimation(UIElement.OpacityProperty, null);

//        if (!useTransitions || !this.UseAnimations)
//        {
//            flyoutElement.Visibility = isOpen ? Visibility.Visible : Visibility.Collapsed;
//            flyoutElement.Opacity = isOpen || !this.FlyoutUseFade ? 1d : 0d;

//            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
//            {
//                transform.X = isOpen ? 0d : closedOffset;
//            }
//            else
//            {
//                transform.Y = isOpen ? 0d : closedOffset;
//            }

//            return;
//        }

//        Duration duration = this.FlyoutAnimationDuration;
//        IEasingFunction easing = new QuadraticEase { EasingMode = EasingMode.EaseOut };

//        if (isOpen)
//        {
//            flyoutElement.Visibility = Visibility.Visible;

//            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
//            {
//                transform.X = closedOffset;
//                transform.BeginAnimation(
//                    TranslateTransform.XProperty,
//                    new DoubleAnimation
//                    {
//                        From = closedOffset,
//                        To = 0d,
//                        Duration = duration,
//                        EasingFunction = easing
//                    });
//            }
//            else
//            {
//                transform.Y = closedOffset;
//                transform.BeginAnimation(
//                    TranslateTransform.YProperty,
//                    new DoubleAnimation
//                    {
//                        From = closedOffset,
//                        To = 0d,
//                        Duration = duration,
//                        EasingFunction = easing
//                    });
//            }

//            if (this.FlyoutUseFade)
//            {
//                flyoutElement.Opacity = 0d;
//                flyoutElement.BeginAnimation(
//                    UIElement.OpacityProperty,
//                    new DoubleAnimation
//                    {
//                        From = 0d,
//                        To = 1d,
//                        Duration = duration
//                    });
//            }
//            else
//            {
//                flyoutElement.Opacity = 1d;
//            }
//        }
//        else
//        {
//            if (flyoutElement.Visibility != Visibility.Visible)
//            {
//                flyoutElement.Visibility = Visibility.Collapsed;

//                if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
//                {
//                    transform.X = closedOffset;
//                }
//                else
//                {
//                    transform.Y = closedOffset;
//                }

//                return;
//            }

//            DoubleAnimation offsetAnimation = new()
//            {
//                To = closedOffset,
//                Duration = duration,
//                EasingFunction = easing
//            };

//            if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
//            {
//                transform.BeginAnimation(TranslateTransform.XProperty, offsetAnimation);
//            }
//            else
//            {
//                transform.BeginAnimation(TranslateTransform.YProperty, offsetAnimation);
//            }

//            DoubleAnimation opacityAnimation = new()
//            {
//                To = this.FlyoutUseFade ? 0d : 1d,
//                Duration = duration
//            };

//            opacityAnimation.Completed += (_, _) =>
//            {
//                if (!this.IsFlyoutOpenForPlacement(placement))
//                {
//                    flyoutElement.Visibility = Visibility.Collapsed;
//                    flyoutElement.Opacity = this.FlyoutUseFade ? 0d : 1d;

//                    if (placement is FlyoutPlacement.Left or FlyoutPlacement.Right)
//                    {
//                        transform.X = closedOffset;
//                    }
//                    else
//                    {
//                        transform.Y = closedOffset;
//                    }
//                }
//            };

//            flyoutElement.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
//        }
//    }

//    private bool HasAnyFlyoutOpen()
//    {
//        return (this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null)
//               || (this.IsRightFlyoutOpen && this.RightFlyoutContent is not null)
//               || (this.IsTopFlyoutOpen && this.TopFlyoutContent is not null)
//               || (this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null);
//    }

//    private bool IsFlyoutOpenForPlacement(FlyoutPlacement placement)
//    {
//        return placement switch
//        {
//            FlyoutPlacement.Left => this.IsLeftFlyoutOpen && this.LeftFlyoutContent is not null,
//            FlyoutPlacement.Right => this.IsRightFlyoutOpen && this.RightFlyoutContent is not null,
//            FlyoutPlacement.Top => this.IsTopFlyoutOpen && this.TopFlyoutContent is not null,
//            FlyoutPlacement.Bottom => this.IsBottomFlyoutOpen && this.BottomFlyoutContent is not null,
//            _ => false,
//        };
//    }

//    private void OnResizeGripDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromRight(e.HorizontalChange);
//        this.ResizeFromBottom(e.VerticalChange);
//    }

//    private void OnLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromLeft(e.HorizontalChange);
//    }

//    private void OnTopResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromTop(e.VerticalChange);
//    }

//    private void OnRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromRight(e.HorizontalChange);
//    }

//    private void OnBottomResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromBottom(e.VerticalChange);
//    }

//    private void OnTopLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromLeft(e.HorizontalChange);
//        this.ResizeFromTop(e.VerticalChange);
//    }

//    private void OnTopRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromRight(e.HorizontalChange);
//        this.ResizeFromTop(e.VerticalChange);
//    }

//    private void OnBottomRightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromRight(e.HorizontalChange);
//        this.ResizeFromBottom(e.VerticalChange);
//    }

//    private void OnBottomLeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
//    {
//        if (!this.CanResizeWindow())
//        {
//            return;
//        }

//        this.ResizeFromLeft(e.HorizontalChange);
//        this.ResizeFromBottom(e.VerticalChange);
//    }

//    private bool CanResizeWindow()
//    {
//        return this.WindowState == WindowState.Normal
//               && this.ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
//    }

//    private void ResizeFromLeft(double horizontalChange)
//    {
//        double currentWidth = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
//        double newWidth = Math.Max(this.MinWidth, currentWidth - horizontalChange);
//        double appliedDelta = currentWidth - newWidth;

//        if (!double.IsInfinity(this.MaxWidth))
//        {
//            newWidth = Math.Min(this.MaxWidth, newWidth);
//            appliedDelta = currentWidth - newWidth;
//        }

//        this.Left += appliedDelta;
//        this.Width = newWidth;
//    }

//    private void ResizeFromTop(double verticalChange)
//    {
//        double currentHeight = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
//        double newHeight = Math.Max(this.MinHeight, currentHeight - verticalChange);
//        double appliedDelta = currentHeight - newHeight;

//        if (!double.IsInfinity(this.MaxHeight))
//        {
//            newHeight = Math.Min(this.MaxHeight, newHeight);
//            appliedDelta = currentHeight - newHeight;
//        }

//        this.Top += appliedDelta;
//        this.Height = newHeight;
//    }

//    private void ResizeFromRight(double horizontalChange)
//    {
//        double currentWidth = double.IsNaN(this.Width) ? this.ActualWidth : this.Width;
//        double newWidth = Math.Max(this.MinWidth, currentWidth + horizontalChange);

//        if (!double.IsInfinity(this.MaxWidth))
//        {
//            newWidth = Math.Min(this.MaxWidth, newWidth);
//        }

//        this.Width = newWidth;
//    }

//    private void ResizeFromBottom(double verticalChange)
//    {
//        double currentHeight = double.IsNaN(this.Height) ? this.ActualHeight : this.Height;
//        double newHeight = Math.Max(this.MinHeight, currentHeight + verticalChange);

//        if (!double.IsInfinity(this.MaxHeight))
//        {
//            newHeight = Math.Min(this.MaxHeight, newHeight);
//        }

//        this.Height = newHeight;
//    }

//    private static void AttachResizeThumb(Thumb? thumb, DragDeltaEventHandler handler)
//    {
//        if (thumb is not null)
//        {
//            thumb.DragDelta += handler;
//        }
//    }

//    private static void DetachResizeThumb(Thumb? thumb, DragDeltaEventHandler handler)
//    {
//        if (thumb is not null)
//        {
//            thumb.DragDelta -= handler;
//        }
//    }

//    private static bool IsInsideInteractiveElement(DependencyObject? source)
//    {
//        DependencyObject? current = source;

//        while (current is not null)
//        {
//            if (current is ButtonBase or TextBoxBase or Selector or ScrollBar or Thumb)
//            {
//                return true;
//            }

//            current = VisualTreeHelper.GetParent(current);
//        }

//        return false;
//    }

//    private void UpdateDerivedCornerRadii()
//    {
//        this.SetValue(
//            TopCornerRadiusPropertyKey,
//            new CornerRadius(
//                this.CornerRadius.TopLeft,
//                this.CornerRadius.TopRight,
//                0d,
//                0d));
//    }

//    private enum FlyoutPlacement
//    {
//        Left,
//        Right,
//        Top,
//        Bottom,
//    }
//    #endregion
//}
//#endregion
