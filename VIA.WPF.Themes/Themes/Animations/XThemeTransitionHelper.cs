// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeTransitionHelper.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VIA.WPF.Themes;

#region ### Class XThemeTransitionHelper ###
/// <summary>
/// Provides visual theme transition animations.
/// </summary>
public static class XThemeTransitionHelper
{
    #region ### Public Methods ###
    /// <summary>
    /// Animates a theme change by placing a snapshot of the previous visual state above the window
    /// and revealing the changed theme below it.
    /// </summary>
    /// <param name="window">The target window.</param>
    /// <param name="themeChangeAction">The action that applies the theme change.</param>
    /// <returns>A task that completes when the transition has finished.</returns>
    public static async Task AnimateThemeChangeAsync(Window window, Action themeChangeAction)
    {
        await AnimateThemeChangeAsync(
            window,
            themeChangeAction,
            XThemeTransitionDirection.TopToBottom,
            XThemeManager.Current.TransitionDuration);
    }

    /// <summary>
    /// Animates a theme change by placing a snapshot of the previous visual state above the window
    /// and revealing the changed theme below it.
    /// </summary>
    /// <param name="window">The target window.</param>
    /// <param name="themeChangeAction">The action that applies the theme change.</param>
    /// <param name="direction">The reveal direction.</param>
    /// <param name="duration">The transition duration.</param>
    /// <returns>A task that completes when the transition has finished.</returns>
    public static async Task AnimateThemeChangeAsync(
        Window window,
        Action themeChangeAction,
        XThemeTransitionDirection direction,
        TimeSpan duration)
    {
        ArgumentNullException.ThrowIfNull(window);
        ArgumentNullException.ThrowIfNull(themeChangeAction);

        if (!window.IsLoaded || window.ActualWidth <= 0d || window.ActualHeight <= 0d || duration <= TimeSpan.Zero)
        {
            themeChangeAction();
            return;
        }

        if (!window.Dispatcher.CheckAccess())
        {
            await window.Dispatcher.InvokeAsync(
                async () => await AnimateThemeChangeAsync(window, themeChangeAction, direction, duration),
                DispatcherPriority.Render);

            return;
        }

        Cursor? previousCursor = Mouse.OverrideCursor;
        Mouse.OverrideCursor = Cursors.Wait;

        Popup? popup = null;

        try
        {
            RenderTargetBitmap? snapshot = CreateWindowSnapshot(window);
            if (snapshot is null)
            {
                themeChangeAction();
                return;
            }

            popup = CreateSnapshotPopup(window, snapshot);
            Image image = (Image)popup.Child;
            RectangleGeometry clipGeometry = new(new Rect(0d, 0d, window.ActualWidth, window.ActualHeight));
            image.Clip = clipGeometry;

            popup.IsOpen = true;

            await window.Dispatcher.InvokeAsync(static () => { }, DispatcherPriority.Render);

            themeChangeAction();

            await window.Dispatcher.InvokeAsync(static () => { }, DispatcherPriority.Render);

            await AnimateSnapshotOutAsync(
                popup,
                image,
                clipGeometry,
                direction,
                window.ActualWidth,
                window.ActualHeight,
                duration);
        }
        finally
        {
            if (popup is not null)
            {
                popup.IsOpen = false;
                popup.Child = null;
            }

            Mouse.OverrideCursor = previousCursor;
        }
    }
    #endregion

    #region ### Private Methods ###
    private static RenderTargetBitmap? CreateWindowSnapshot(Window window)
    {
        PresentationSource? source = PresentationSource.FromVisual(window);
        Matrix transformToDevice = source?.CompositionTarget?.TransformToDevice ?? Matrix.Identity;

        int pixelWidth = Math.Max(1, (int)Math.Round(window.ActualWidth * transformToDevice.M11));
        int pixelHeight = Math.Max(1, (int)Math.Round(window.ActualHeight * transformToDevice.M22));
        double dpiX = 96d * transformToDevice.M11;
        double dpiY = 96d * transformToDevice.M22;

        RenderTargetBitmap bitmap = new(pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Pbgra32);

        try
        {
            bitmap.Render(window);
            bitmap.Freeze();

            return bitmap;
        }
        catch
        {
            return null;
        }
    }

    private static Popup CreateSnapshotPopup(Window window, ImageSource snapshot)
    {
        Image image = new()
        {
            Source = snapshot,
            Width = window.ActualWidth,
            Height = window.ActualHeight,
            Stretch = Stretch.Fill,
            SnapsToDevicePixels = true,
            IsHitTestVisible = false
        };

        RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

        return new Popup
        {
            AllowsTransparency = true,
            Focusable = false,
            IsHitTestVisible = false,
            Placement = PlacementMode.Relative,
            PlacementTarget = window,
            HorizontalOffset = 0d,
            VerticalOffset = 0d,
            StaysOpen = true,
            Child = image
        };
    }

    private static Task AnimateSnapshotOutAsync(
        Popup popup,
        Image image,
        RectangleGeometry clipGeometry,
        XThemeTransitionDirection direction,
        double width,
        double height,
        TimeSpan duration)
    {
        TaskCompletionSource<object?> completionSource = new();

        Rect targetRect = direction switch
        {
            XThemeTransitionDirection.TopToBottom => new Rect(0d, height, width, 0d),
            XThemeTransitionDirection.BottomToTop => new Rect(0d, 0d, width, 0d),
            XThemeTransitionDirection.LeftToRight => new Rect(width, 0d, 0d, height),
            XThemeTransitionDirection.RightToLeft => new Rect(0d, 0d, 0d, height),
            _ => new Rect(0d, height, width, 0d)
        };

        RectAnimation clipAnimation = new()
        {
            From = new Rect(0d, 0d, width, height),
            To = targetRect,
            Duration = new Duration(duration),
            EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseInOut
            }
        };

        DoubleAnimation opacityAnimation = new()
        {
            From = 1d,
            To = 0d,
            Duration = new Duration(TimeSpan.FromMilliseconds(Math.Min(duration.TotalMilliseconds, 180d))),
            BeginTime = TimeSpan.FromMilliseconds(Math.Max(0d, duration.TotalMilliseconds - 180d)),
            EasingFunction = new QuadraticEase
            {
                EasingMode = EasingMode.EaseOut
            }
        };

        clipAnimation.Completed += (_, _) =>
        {
            image.BeginAnimation(UIElement.OpacityProperty, null);
            clipGeometry.BeginAnimation(RectangleGeometry.RectProperty, null);

            popup.IsOpen = false;
            popup.Child = null;

            completionSource.TrySetResult(null);
        };

        clipGeometry.BeginAnimation(RectangleGeometry.RectProperty, clipAnimation, HandoffBehavior.SnapshotAndReplace);
        image.BeginAnimation(UIElement.OpacityProperty, opacityAnimation, HandoffBehavior.SnapshotAndReplace);

        return completionSource.Task;
    }
    #endregion
}
#endregion