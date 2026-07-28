// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Localization;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XWindowTests ###
/// <summary>
/// Provides tests for the <see cref="XWindow" /> class.
/// </summary>
public sealed class XWindowTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that a new window uses the expected shell defaults.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeWindowShellDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.Equal(WindowStyle.None, window.WindowStyle);
                Assert.True(window.AllowsTransparency);
                Assert.Same(Brushes.Transparent, window.Background);
                Assert.Equal(ResizeMode.CanResize, window.ResizeMode);
                Assert.Equal(WindowStartupLocation.CenterScreen, window.WindowStartupLocation);
                Assert.Equal(WindowState.Normal, window.WindowState);
                Assert.True(window.ShowInTaskbar);
            });
    }

    /// <summary>
    /// Verifies default dependency property values that define the visible window chrome.
    /// </summary>
    [Fact]
    public void DependencyProperties_ShouldExposeExpectedChromeDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.Null(window.TitleBarContent);
                Assert.Null(window.TitleBarContentTemplate);
                Assert.Equal(string.Empty, window.Subtitle);
                Assert.Equal(35d, window.TitleBarHeight);
                Assert.Equal(new CornerRadius(6d, 6d, 4d, 4d), window.CornerRadius);
                Assert.Equal(new CornerRadius(6d, 6d, 0d, 0d), window.TopCornerRadius);
                Assert.False(window.ShowThemeSelector);
                Assert.False(window.ShowLanguageSelector);
                Assert.Same(XLanguages.Default, window.AvailableLanguages);
                Assert.Null(window.SelectedLanguage);
                Assert.True(window.ApplyLanguageFormattingCulture);
                Assert.True(window.ShowThemeModeButton);
                Assert.True(window.ShowMinimizeButton);
                Assert.True(window.ShowMaximizeButton);
                Assert.True(window.ShowCloseButton);
                Assert.False(window.ShowResizeGrip);
                Assert.Null(window.StatusBarContent);
                Assert.Null(window.StatusBarContentTemplate);
            });
    }

    /// <summary>
    /// Verifies default dependency property values for busy, flyout, toast and animation state.
    /// </summary>
    [Fact]
    public void DependencyProperties_ShouldExposeExpectedStateDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.False(window.IsBusy);
                Assert.Equal("Bitte warten...", window.BusyContent);
                Assert.Null(window.BusyContentTemplate);
                Assert.True(window.CloseFlyoutsOnOverlayClick);
                Assert.False(window.IsLeftFlyoutOpen);
                Assert.False(window.IsRightFlyoutOpen);
                Assert.False(window.IsTopFlyoutOpen);
                Assert.False(window.IsBottomFlyoutOpen);
                Assert.Equal(320d, window.LeftFlyoutWidth);
                Assert.Equal(320d, window.RightFlyoutWidth);
                Assert.Equal(240d, window.TopFlyoutHeight);
                Assert.Equal(240d, window.BottomFlyoutHeight);
                Assert.Null(window.ToastContent);
                Assert.Null(window.ToastContentTemplate);
                Assert.False(window.IsToastOpen);
                Assert.Equal(XToastPlacement.TopRight, window.ToastPlacement);
                Assert.True(window.UseAnimations);
                Assert.Equal(new Duration(TimeSpan.FromMilliseconds(220)), window.FlyoutAnimationDuration);
                Assert.Equal(24d, window.FlyoutAnimationOffset);
                Assert.True(window.FlyoutUseFade);
                Assert.Equal(new Duration(TimeSpan.FromMilliseconds(180)), window.ToastAnimationDuration);
                Assert.Equal(18d, window.ToastAnimationOffset);
                Assert.True(window.ToastUseFade);
                Assert.Equal(XWindowAnimationMode.None, window.StartupAnimation);
                Assert.Equal(XWindowAnimationMode.None, window.CloseAnimation);
            });
    }

    /// <summary>
    /// Verifies that all brush dependency properties expose non-null defaults and support round-tripping.
    /// </summary>
    [Fact]
    public void BrushProperties_ShouldExposeDefaultsAndSupportRoundTrips()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.NotNull(window.WindowBackgroundBrush);
                Assert.NotNull(window.WindowForegroundBrush);
                Assert.NotNull(window.WindowBorderBrush);
                Assert.NotNull(window.TitleBarBackgroundBrush);
                Assert.NotNull(window.TitleBarForegroundBrush);
                Assert.NotNull(window.CaptionButtonHoverBrush);
                Assert.NotNull(window.CaptionButtonPressedBrush);
                Assert.NotNull(window.CloseButtonHoverBrush);
                Assert.NotNull(window.CloseButtonPressedBrush);
                Assert.NotNull(window.CloseButtonForegroundBrush);
                Assert.NotNull(window.BusyOverlayBrush);
                Assert.NotNull(window.FlyoutOverlayBrush);

                window.WindowBackgroundBrush = Brushes.Red;
                window.WindowForegroundBrush = Brushes.Green;
                window.WindowBorderBrush = Brushes.Blue;
                window.TitleBarBackgroundBrush = Brushes.Yellow;
                window.TitleBarForegroundBrush = Brushes.Orange;
                window.CaptionButtonHoverBrush = Brushes.Purple;
                window.CaptionButtonPressedBrush = Brushes.Pink;
                window.CloseButtonHoverBrush = Brushes.Brown;
                window.CloseButtonPressedBrush = Brushes.Gray;
                window.CloseButtonForegroundBrush = Brushes.White;
                window.BusyOverlayBrush = Brushes.Black;
                window.FlyoutOverlayBrush = Brushes.Cyan;

                Assert.Same(Brushes.Red, window.WindowBackgroundBrush);
                Assert.Same(Brushes.Green, window.WindowForegroundBrush);
                Assert.Same(Brushes.Blue, window.WindowBorderBrush);
                Assert.Same(Brushes.Yellow, window.TitleBarBackgroundBrush);
                Assert.Same(Brushes.Orange, window.TitleBarForegroundBrush);
                Assert.Same(Brushes.Purple, window.CaptionButtonHoverBrush);
                Assert.Same(Brushes.Pink, window.CaptionButtonPressedBrush);
                Assert.Same(Brushes.Brown, window.CloseButtonHoverBrush);
                Assert.Same(Brushes.Gray, window.CloseButtonPressedBrush);
                Assert.Same(Brushes.White, window.CloseButtonForegroundBrush);
                Assert.Same(Brushes.Black, window.BusyOverlayBrush);
                Assert.Same(Brushes.Cyan, window.FlyoutOverlayBrush);
            });
    }

    /// <summary>
    /// Verifies that content and template dependency properties support round-tripping.
    /// </summary>
    [Fact]
    public void ContentProperties_ShouldSupportRoundTrips()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();
                object titleBarContent = new();
                object statusBarContent = new();
                object busyContent = new();
                object leftFlyoutContent = new();
                object rightFlyoutContent = new();
                object topFlyoutContent = new();
                object bottomFlyoutContent = new();
                object toastContent = new();
                DataTemplate template = new(typeof(TextBlock));

                window.TitleBarContent = titleBarContent;
                window.TitleBarContentTemplate = template;
                window.StatusBarContent = statusBarContent;
                window.StatusBarContentTemplate = template;
                window.BusyContent = busyContent;
                window.BusyContentTemplate = template;
                window.LeftFlyoutContent = leftFlyoutContent;
                window.RightFlyoutContent = rightFlyoutContent;
                window.TopFlyoutContent = topFlyoutContent;
                window.BottomFlyoutContent = bottomFlyoutContent;
                window.ToastContent = toastContent;
                window.ToastContentTemplate = template;

                Assert.Same(titleBarContent, window.TitleBarContent);
                Assert.Same(template, window.TitleBarContentTemplate);
                Assert.Same(statusBarContent, window.StatusBarContent);
                Assert.Same(template, window.StatusBarContentTemplate);
                Assert.Same(busyContent, window.BusyContent);
                Assert.Same(template, window.BusyContentTemplate);
                Assert.Same(leftFlyoutContent, window.LeftFlyoutContent);
                Assert.Same(rightFlyoutContent, window.RightFlyoutContent);
                Assert.Same(topFlyoutContent, window.TopFlyoutContent);
                Assert.Same(bottomFlyoutContent, window.BottomFlyoutContent);
                Assert.Same(toastContent, window.ToastContent);
                Assert.Same(template, window.ToastContentTemplate);
            });
    }

    /// <summary>
    /// Verifies that primitive dependency properties support round-tripping.
    /// </summary>
    [Fact]
    public void PrimitiveProperties_ShouldSupportRoundTrips()
    {
        WpfTestHelper.Run(
            () =>
            {
                XLocalizationService localizationService = XLocalizationService.Current;
                CultureInfo previousServiceUICulture = localizationService.CurrentUICulture;
                bool previousApplyFormattingCulture = localizationService.ApplyFormattingCulture;
                CultureInfo previousCurrentCulture = CultureInfo.CurrentCulture;
                CultureInfo previousCurrentUICulture = CultureInfo.CurrentUICulture;
                CultureInfo? previousDefaultThreadCulture = CultureInfo.DefaultThreadCurrentCulture;
                CultureInfo? previousDefaultThreadUICulture = CultureInfo.DefaultThreadCurrentUICulture;

                try
                {
                    XWindow window = new();

                    window.Subtitle = "Details";
                    window.TitleBarHeight = 48d;
                    window.ShowThemeSelector = true;
                    window.ShowLanguageSelector = true;
                    window.AvailableLanguages = new[] { XLanguages.English };
                    window.ApplyLanguageFormattingCulture = false;
                    window.SelectedLanguage = XLanguages.English;
                    window.ShowThemeModeButton = false;
                    window.ShowMinimizeButton = false;
                    window.ShowMaximizeButton = false;
                    window.ShowCloseButton = false;
                    window.ShowResizeGrip = true;
                    window.IsBusy = true;
                    window.CloseFlyoutsOnOverlayClick = false;
                    window.IsToastOpen = true;
                    window.ToastPlacement = XToastPlacement.Center;
                    window.UseAnimations = false;
                    window.LeftFlyoutWidth = 400d;
                    window.RightFlyoutWidth = 420d;
                    window.TopFlyoutHeight = 260d;
                    window.BottomFlyoutHeight = 280d;
                    window.FlyoutAnimationDuration = new Duration(TimeSpan.FromMilliseconds(10));
                    window.FlyoutAnimationOffset = 12d;
                    window.FlyoutUseFade = false;
                    window.ToastAnimationDuration = new Duration(TimeSpan.FromMilliseconds(11));
                    window.ToastAnimationOffset = 13d;
                    window.ToastUseFade = false;
                    window.StartupAnimation = XWindowAnimationMode.Fade;
                    window.StartupAnimationDuration = new Duration(TimeSpan.FromMilliseconds(14));
                    window.StartupAnimationScaleDuration = new Duration(TimeSpan.FromMilliseconds(15));
                    window.StartupAnimationInitialScale = 0.9d;
                    window.CloseAnimation = XWindowAnimationMode.FadeAndScale;
                    window.CloseAnimationDuration = new Duration(TimeSpan.FromMilliseconds(16));
                    window.CloseAnimationScaleDuration = new Duration(TimeSpan.FromMilliseconds(17));
                    window.CloseAnimationTargetScale = 0.8d;

                    Assert.Equal("Details", window.Subtitle);
                    Assert.Equal(48d, window.TitleBarHeight);
                    Assert.True(window.ShowThemeSelector);
                    Assert.True(window.ShowLanguageSelector);
                    Assert.Single(window.AvailableLanguages!);
                    Assert.Same(XLanguages.English, window.SelectedLanguage);
                    Assert.False(window.ApplyLanguageFormattingCulture);
                    Assert.False(window.ShowThemeModeButton);
                    Assert.False(window.ShowMinimizeButton);
                    Assert.False(window.ShowMaximizeButton);
                    Assert.False(window.ShowCloseButton);
                    Assert.True(window.ShowResizeGrip);
                    Assert.True(window.IsBusy);
                    Assert.False(window.CloseFlyoutsOnOverlayClick);
                    Assert.True(window.IsToastOpen);
                    Assert.Equal(XToastPlacement.Center, window.ToastPlacement);
                    Assert.False(window.UseAnimations);
                    Assert.Equal(400d, window.LeftFlyoutWidth);
                    Assert.Equal(420d, window.RightFlyoutWidth);
                    Assert.Equal(260d, window.TopFlyoutHeight);
                    Assert.Equal(280d, window.BottomFlyoutHeight);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(10)), window.FlyoutAnimationDuration);
                    Assert.Equal(12d, window.FlyoutAnimationOffset);
                    Assert.False(window.FlyoutUseFade);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(11)), window.ToastAnimationDuration);
                    Assert.Equal(13d, window.ToastAnimationOffset);
                    Assert.False(window.ToastUseFade);
                    Assert.Equal(XWindowAnimationMode.Fade, window.StartupAnimation);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(14)), window.StartupAnimationDuration);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(15)), window.StartupAnimationScaleDuration);
                    Assert.Equal(0.9d, window.StartupAnimationInitialScale);
                    Assert.Equal(XWindowAnimationMode.FadeAndScale, window.CloseAnimation);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(16)), window.CloseAnimationDuration);
                    Assert.Equal(new Duration(TimeSpan.FromMilliseconds(17)), window.CloseAnimationScaleDuration);
                    Assert.Equal(0.8d, window.CloseAnimationTargetScale);
                }
                finally
                {
                    localizationService.SetCulture(
                        previousServiceUICulture,
                        previousApplyFormattingCulture);

                    CultureInfo.CurrentCulture = previousCurrentCulture;
                    CultureInfo.CurrentUICulture = previousCurrentUICulture;
                    CultureInfo.DefaultThreadCurrentCulture = previousDefaultThreadCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = previousDefaultThreadUICulture;
                }

            });
    }

    /// <summary>
    /// Verifies that the selected language controls inherited WPF binding culture and VIA.WPF input formatting.
    /// </summary>
    [Fact]
    public void SelectedLanguage_ShouldApplyFormattingCultureToVisualTree()
    {
        WpfTestHelper.Run(
            () =>
            {
                XLocalizationService localizationService = XLocalizationService.Current;
                CultureInfo previousServiceUICulture = localizationService.CurrentUICulture;
                bool previousApplyFormattingCulture = localizationService.ApplyFormattingCulture;
                CultureInfo previousCurrentCulture = CultureInfo.CurrentCulture;
                CultureInfo previousCurrentUICulture = CultureInfo.CurrentUICulture;
                CultureInfo? previousDefaultThreadCulture = CultureInfo.DefaultThreadCurrentCulture;
                CultureInfo? previousDefaultThreadUICulture = CultureInfo.DefaultThreadCurrentUICulture;

                try
                {
                    const double Value = 1234.5d;
                    XNumberBox numberBox = new()
                    {
                        FormatString = "N2",
                        Maximum = 10000d,
                        Value = Value
                    };
                    TextBlock formattedText = new();
                    BindingOperations.SetBinding(
                        formattedText,
                        TextBlock.TextProperty,
                        new Binding
                        {
                            Source = Value,
                            StringFormat = "N2"
                        });

                    StackPanel panel = new();
                    panel.Children.Add(numberBox);
                    panel.Children.Add(formattedText);

                    XWindow window = new()
                    {
                        Content = panel,
                        ApplyLanguageFormattingCulture = true,
                        SelectedLanguage = XLanguages.German
                    };

                    formattedText.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();

                    CultureInfo germanCulture = CultureInfo.GetCultureInfo("de-DE");
                    Assert.Equal(germanCulture.IetfLanguageTag, window.Language.IetfLanguageTag, ignoreCase: true);
                    Assert.Equal(germanCulture.IetfLanguageTag, numberBox.Language.IetfLanguageTag, ignoreCase: true);
                    Assert.Equal(Value.ToString("N2", germanCulture), numberBox.Text);
                    Assert.Equal(Value.ToString("N2", germanCulture), formattedText.Text);

                    window.SelectedLanguage = XLanguages.English;
                    formattedText.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();

                    CultureInfo englishCulture = XLanguages.English.Culture;
                    Assert.Equal(englishCulture.IetfLanguageTag, window.Language.IetfLanguageTag, ignoreCase: true);
                    Assert.Equal(Value.ToString("N2", englishCulture), numberBox.Text);
                    Assert.Equal(Value.ToString("N2", englishCulture), formattedText.Text);
                }
                finally
                {
                    localizationService.SetCulture(
                        previousServiceUICulture,
                        previousApplyFormattingCulture);

                    CultureInfo.CurrentCulture = previousCurrentCulture;
                    CultureInfo.CurrentUICulture = previousCurrentUICulture;
                    CultureInfo.DefaultThreadCurrentCulture = previousDefaultThreadCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = previousDefaultThreadUICulture;
                }
            });
    }

    /// <summary>
    /// Verifies that changing the main corner radius updates the derived top corner radius.
    /// </summary>
    [Fact]
    public void CornerRadius_ShouldUpdateDerivedTopCornerRadius()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                window.CornerRadius = new CornerRadius(1d, 2d, 3d, 4d);

                Assert.Equal(new CornerRadius(1d, 2d, 0d, 0d), window.TopCornerRadius);
            });
    }

    /// <summary>
    /// Verifies that flyouts are closed when their content is cleared.
    /// </summary>
    [Fact]
    public void ClearingFlyoutContent_ShouldCloseMatchingFlyout()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new()
                {
                    LeftFlyoutContent = new object(),
                    RightFlyoutContent = new object(),
                    TopFlyoutContent = new object(),
                    BottomFlyoutContent = new object(),
                    IsLeftFlyoutOpen = true,
                    IsRightFlyoutOpen = true,
                    IsTopFlyoutOpen = true,
                    IsBottomFlyoutOpen = true
                };

                window.LeftFlyoutContent = null;
                window.RightFlyoutContent = null;
                window.TopFlyoutContent = null;
                window.BottomFlyoutContent = null;

                Assert.False(window.IsLeftFlyoutOpen);
                Assert.False(window.IsRightFlyoutOpen);
                Assert.False(window.IsTopFlyoutOpen);
                Assert.False(window.IsBottomFlyoutOpen);
            });
    }

    /// <summary>
    /// Verifies that the constructor registers the expected command bindings.
    /// </summary>
    [Fact]
    public void Constructor_ShouldRegisterExpectedCommandBindings()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();
                IEnumerable<ICommand> commands = window.CommandBindings.OfType<CommandBinding>().Select(binding => binding.Command);

                Assert.Contains(XWindowCommands.ToggleThemeMode, commands);
                Assert.Contains(XWindowCommands.Minimize, commands);
                Assert.Contains(XWindowCommands.MaximizeRestore, commands);
                Assert.Contains(XWindowCommands.Close, commands);
            });
    }

    /// <summary>
    /// Verifies the public enum values used by the window API.
    /// </summary>
    [Fact]
    public void Enums_ShouldExposeExpectedValues()
    {
        Assert.Equal(new[] { XWindowAnimationMode.None, XWindowAnimationMode.Fade, XWindowAnimationMode.FadeAndScale }, Enum.GetValues<XWindowAnimationMode>());
        Assert.Equal(new[] { XToastPlacement.TopRight, XToastPlacement.BottomRight, XToastPlacement.Center }, Enum.GetValues<XToastPlacement>());
    }
    #endregion
}
#endregion
