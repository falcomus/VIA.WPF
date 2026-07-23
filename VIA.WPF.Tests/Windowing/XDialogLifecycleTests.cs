// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogLifecycleTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XDialogLifecycleTests ###
/// <summary>
/// Provides lifecycle tests for <see cref="XDialogService"/>.
/// </summary>
public sealed class XDialogLifecycleTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the lifecycle-related default options.
    /// </summary>
    [Fact]
    public void DialogOptions_ShouldEnableOwnerLifecycleDefaults()
    {
        Assert.True(XDialogOptions.Default.DimOwner);
        Assert.True(XDialogOptions.Default.RestoreOwnerFocus);
    }

    /// <summary>
    /// Verifies that the owner-local overlay is active while the dialog presenter executes.
    /// </summary>
    [Fact]
    public void ShowModal_WithXWindowOwner_ShouldHoldOverlayLeaseDuringPresentation()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow owner = CreateVisibleXWindow();
                Window dialog = new();

                try
                {
                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        _ =>
                        {
                            Assert.True(owner.IsModalOverlayOpen);
                            Assert.Equal(1, owner.ModalOverlayDepth);
                            return true;
                        });

                    XDialogResult result = service.ShowModal(dialog);

                    Assert.True(result.IsAccepted);
                    Assert.False(owner.IsModalOverlayOpen);
                    Assert.Equal(0, owner.ModalOverlayDepth);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that disabling owner dimming avoids acquiring an overlay lease.
    /// </summary>
    [Fact]
    public void ShowModal_WithOwnerDimmingDisabled_ShouldNotAcquireOverlayLease()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow owner = CreateVisibleXWindow();
                Window dialog = new();

                try
                {
                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        _ =>
                        {
                            Assert.False(owner.IsModalOverlayOpen);
                            Assert.Equal(0, owner.ModalOverlayDepth);
                            return false;
                        });

                    service.ShowModal(
                        dialog,
                        options: new XDialogOptions
                        {
                            DimOwner = false
                        });

                    Assert.False(owner.IsModalOverlayOpen);
                    Assert.Equal(0, owner.ModalOverlayDepth);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that the overlay is released when dialog presentation throws.
    /// </summary>
    [Fact]
    public void ShowModal_WhenPresentationThrows_ShouldReleaseOverlayLease()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow owner = CreateVisibleXWindow();
                Window dialog = new();
                InvalidOperationException expected = new("Simulated dialog failure.");

                try
                {
                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        _ =>
                        {
                            Assert.True(owner.IsModalOverlayOpen);
                            Assert.Equal(1, owner.ModalOverlayDepth);
                            throw expected;
                        });

                    InvalidOperationException actual = Assert.Throws<InvalidOperationException>(
                        () => service.ShowModal(dialog));

                    Assert.Same(expected, actual);
                    Assert.False(owner.IsModalOverlayOpen);
                    Assert.Equal(0, owner.ModalOverlayDepth);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that a normal WPF owner remains supported without VIA overlay capabilities.
    /// </summary>
    [Fact]
    public void ShowModal_WithNormalWindowOwner_ShouldPresentWithoutOverlayHost()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window owner = CreateVisibleWindow();
                Window dialog = new();

                try
                {
                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        presentedDialog =>
                        {
                            Assert.Same(owner, presentedDialog.Owner);
                            return null;
                        });

                    XDialogResult result = service.ShowModal(dialog);

                    Assert.Equal(XDialogOutcome.NoResult, result.Outcome);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies the complete synchronous WPF modal lifecycle with a real dialog window.
    /// </summary>
    [Fact]
    public void ShowModal_WithRealDialog_ShouldDimOwnerUntilDialogCloses()
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow owner = CreateVisibleXWindow();
                Window dialog = new()
                {
                    Width = 200d,
                    Height = 100d,
                    ShowInTaskbar = false
                };

                try
                {
                    dialog.Loaded += (_, _) =>
                    {
                        Assert.True(owner.IsModalOverlayOpen);
                        Assert.Equal(1, owner.ModalOverlayDepth);
                        dialog.DialogResult = true;
                    };

                    XDialogService service = new(new StubDialogOwnerResolver(owner));
                    XDialogResult result = service.ShowModal(dialog);

                    Assert.True(result.IsAccepted);
                    Assert.False(owner.IsModalOverlayOpen);
                    Assert.Equal(0, owner.ModalOverlayDepth);
                }
                finally
                {
                    if (dialog.IsVisible)
                    {
                        dialog.Close();
                    }

                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that the element focused before opening the dialog is restored.
    /// </summary>
    [Fact]
    public void ShowModal_ShouldRestorePreviousOwnerFocus()
    {
        WpfTestHelper.Run(
            () =>
            {
                TextBox first = new();
                TextBox second = new();
                StackPanel content = new();
                content.Children.Add(first);
                content.Children.Add(second);

                XWindow owner = CreateVisibleXWindow(content);
                Window dialog = new();

                try
                {
                    owner.Activate();
                    first.Focus();
                    Keyboard.Focus(first);
                    WpfTestHelper.DoEvents();

                    Assert.Same(first, Keyboard.FocusedElement);

                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        _ =>
                        {
                            second.Focus();
                            Keyboard.Focus(second);
                            WpfTestHelper.DoEvents();

                            Assert.Same(second, Keyboard.FocusedElement);
                            return true;
                        });

                    service.ShowModal(dialog);

                    Assert.Same(first, Keyboard.FocusedElement);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that focus restoration can be disabled explicitly.
    /// </summary>
    [Fact]
    public void ShowModal_WithFocusRestoreDisabled_ShouldKeepCurrentFocus()
    {
        WpfTestHelper.Run(
            () =>
            {
                TextBox first = new();
                TextBox second = new();
                StackPanel content = new();
                content.Children.Add(first);
                content.Children.Add(second);

                XWindow owner = CreateVisibleXWindow(content);
                Window dialog = new();

                try
                {
                    owner.Activate();
                    first.Focus();
                    Keyboard.Focus(first);
                    WpfTestHelper.DoEvents();

                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        _ =>
                        {
                            second.Focus();
                            Keyboard.Focus(second);
                            WpfTestHelper.DoEvents();
                            return true;
                        });

                    service.ShowModal(
                        dialog,
                        options: new XDialogOptions
                        {
                            RestoreOwnerFocus = false
                        });

                    Assert.Same(second, Keyboard.FocusedElement);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }
    #endregion

    #region ### Private Methods ###
    private static Window CreateVisibleWindow()
    {
        Window owner = new()
        {
            Width = 320d,
            Height = 180d,
            Left = -32000d,
            Top = -32000d,
            ShowActivated = false,
            ShowInTaskbar = false,
            WindowStyle = WindowStyle.None
        };

        owner.Show();
        WpfTestHelper.DoEvents();

        Assert.True(owner.IsVisible);
        return owner;
    }

    private static XWindow CreateVisibleXWindow(object? content = null)
    {
        XWindow owner = new()
        {
            Width = 320d,
            Height = 180d,
            Left = -32000d,
            Top = -32000d,
            ShowActivated = false,
            ShowInTaskbar = false,
            UseAnimations = false,
            Content = content
        };

        owner.Show();
        WpfTestHelper.DoEvents();

        Assert.True(owner.IsVisible);
        return owner;
    }
    #endregion

    #region ### Private Classes ###
    private sealed class StubDialogOwnerResolver : IXDialogOwnerResolver
    {
        #region ### Constructors ###
        internal StubDialogOwnerResolver(Window? owner)
        {
            this.Owner = owner;
        }
        #endregion

        #region ### Internal Properties ###
        internal Window? Owner { get; }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public Window? ResolveOwner(Window dialog, DependencyObject? ownerSource = null)
        {
            return this.Owner;
        }
        #endregion
    }
    #endregion
}
#endregion
