// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XModalOverlayTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XModalOverlayTests ###
/// <summary>
/// Provides tests for the owner-local modal overlay capability of <see cref="XWindow"/>.
/// </summary>
public sealed class XModalOverlayTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the initial modal overlay state and brush configuration.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeModalOverlayDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.False(window.IsModalOverlayOpen);
                Assert.Equal(0, window.ModalOverlayDepth);
                Assert.NotNull(window.ModalOverlayBrush);
                Assert.False(window.UseWindowShadow);

                window.ModalOverlayBrush = Brushes.CadetBlue;

                Assert.Same(Brushes.CadetBlue, window.ModalOverlayBrush);
            });
    }

    /// <summary>
    /// Verifies that dialog windows opt in to elevation without affecting application windows.
    /// </summary>
    [Fact]
    public void DialogWindow_ShouldEnableElevationShadowByDefault()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDialogWindow dialog = new();

                Assert.True(dialog.UseWindowShadow);
            });
    }

    /// <summary>
    /// Verifies that nested leases keep the owner overlay open until the final lease is released.
    /// </summary>
    [Fact]
    public void AcquireModalOverlay_WithNestedLeases_ShouldReferenceCountPerWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();
                IXModalOverlayHost host = window;

                IDisposable outerLease = host.AcquireModalOverlay();

                Assert.True(window.IsModalOverlayOpen);
                Assert.Equal(1, window.ModalOverlayDepth);

                IDisposable innerLease = host.AcquireModalOverlay();

                Assert.True(window.IsModalOverlayOpen);
                Assert.Equal(2, window.ModalOverlayDepth);

                innerLease.Dispose();

                Assert.True(window.IsModalOverlayOpen);
                Assert.Equal(1, window.ModalOverlayDepth);

                outerLease.Dispose();

                Assert.False(window.IsModalOverlayOpen);
                Assert.Equal(0, window.ModalOverlayDepth);
            });
    }

    /// <summary>
    /// Verifies that disposing the same lease more than once does not underflow the owner state.
    /// </summary>
    [Fact]
    public void ModalOverlayLease_WhenDisposedTwice_ShouldReleaseOnlyOnce()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();
                IDisposable lease = ((IXModalOverlayHost)window).AcquireModalOverlay();

                lease.Dispose();
                lease.Dispose();

                Assert.False(window.IsModalOverlayOpen);
                Assert.Equal(0, window.ModalOverlayDepth);
            });
    }

    /// <summary>
    /// Verifies that modal overlay state is isolated between window instances.
    /// </summary>
    [Fact]
    public void AcquireModalOverlay_OnDifferentWindows_ShouldKeepStateIndependent()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow firstWindow = new();
                XWindow secondWindow = new();

                using IDisposable lease = ((IXModalOverlayHost)firstWindow).AcquireModalOverlay();

                Assert.True(firstWindow.IsModalOverlayOpen);
                Assert.Equal(1, firstWindow.ModalOverlayDepth);
                Assert.False(secondWindow.IsModalOverlayOpen);
                Assert.Equal(0, secondWindow.ModalOverlayDepth);
            });
    }

    /// <summary>
    /// Verifies that the themed template materializes the modal overlay and reacts to lease state.
    /// </summary>
    [Fact]
    public void Template_ShouldMaterializeAndToggleModalOverlay()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new()
                {
                    Width = 320d,
                    Height = 180d,
                    Left = -32000d,
                    Top = -32000d,
                    ShowActivated = false,
                    ShowInTaskbar = false,
                    UseAnimations = false
                };

                try
                {
                    window.Show();
                    window.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                    Border overlay = Assert.IsType<Border>(
                        window.Template.FindName("PART_ModalOverlay", window));

                    Assert.Equal(Visibility.Collapsed, overlay.Visibility);
                    Assert.False(overlay.IsVisible);

                    using (((IXModalOverlayHost)window).AcquireModalOverlay())
                    {
                        window.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                        Assert.Equal(Visibility.Visible, overlay.Visibility);
                        Assert.True(overlay.IsVisible);
                    }

                    window.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                    Assert.Equal(Visibility.Collapsed, overlay.Visibility);
                    Assert.False(overlay.IsVisible);
                }
                finally
                {
                    window.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that consumers cannot set the read-only modal overlay state directly.
    /// </summary>
    [Fact]
    public void IsModalOverlayOpen_ShouldBeReadOnly()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow window = new();

                Assert.Throws<InvalidOperationException>(
                    () => window.SetValue(XWindow.IsModalOverlayOpenProperty, true));

                Assert.False(window.IsModalOverlayOpen);
            });
    }
    #endregion
}
#endregion
