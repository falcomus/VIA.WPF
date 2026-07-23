// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XDialogServiceTests ###
/// <summary>
/// Provides tests for the <see cref="XDialogService"/> class.
/// </summary>
public sealed class XDialogServiceTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that the default service instance is stable.
    /// </summary>
    [Fact]
    public void Default_ShouldReturnStableInstance()
    {
        Assert.Same(XDialogService.Default, XDialogService.Default);
    }

    /// <summary>
    /// Verifies that a resolved owner is assigned and used for default centering.
    /// </summary>
    [Fact]
    public void ShowModal_WithResolvedOwner_ShouldAssignOwnerAndCenterDialog()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window owner = CreateVisibleOwner();
                Window dialog = new();
                Border ownerSource = new();
                StubDialogOwnerResolver resolver = new(owner);
                bool presenterCalled = false;

                try
                {
                    XDialogService service = new(
                        resolver,
                        presentedDialog =>
                        {
                            presenterCalled = true;
                            Assert.Same(dialog, presentedDialog);
                            Assert.Same(owner, presentedDialog.Owner);
                            Assert.Equal(WindowStartupLocation.CenterOwner, presentedDialog.WindowStartupLocation);
                            return true;
                        });

                    XDialogResult result = service.ShowModal(dialog, ownerSource);

                    Assert.True(presenterCalled);
                    Assert.Same(dialog, resolver.LastDialog);
                    Assert.Same(ownerSource, resolver.LastOwnerSource);
                    Assert.Equal(XDialogOutcome.Accepted, result.Outcome);
                    Assert.True(result.IsAccepted);
                    Assert.Equal(true, result.NativeResult);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that a dialog without an owner is centered on the screen.
    /// </summary>
    [Fact]
    public void ShowModal_WithoutOwner_ShouldCenterDialogOnScreen()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                StubDialogOwnerResolver resolver = new(null);

                XDialogService service = new(
                    resolver,
                    presentedDialog =>
                    {
                        Assert.Null(presentedDialog.Owner);
                        Assert.Equal(WindowStartupLocation.CenterScreen, presentedDialog.WindowStartupLocation);
                        return false;
                    });

                XDialogResult result = service.ShowModal(dialog);

                Assert.Equal(XDialogOutcome.NotAccepted, result.Outcome);
                Assert.False(result.IsAccepted);
                Assert.Equal(false, result.NativeResult);
            });
    }

    /// <summary>
    /// Verifies that an explicitly configured startup location is preserved.
    /// </summary>
    [Fact]
    public void ShowModal_WithConfiguredStartupLocation_ShouldPreserveLocation()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window owner = CreateVisibleOwner();
                Window dialog = new()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                try
                {
                    XDialogService service = new(
                        new StubDialogOwnerResolver(owner),
                        presentedDialog =>
                        {
                            Assert.Equal(WindowStartupLocation.CenterScreen, presentedDialog.WindowStartupLocation);
                            return true;
                        });

                    service.ShowModal(dialog);
                }
                finally
                {
                    owner.Close();
                    WpfTestHelper.DoEvents();
                }
            });
    }

    /// <summary>
    /// Verifies that dialog options override the window startup location.
    /// </summary>
    [Fact]
    public void ShowModal_WithStartupLocationOption_ShouldApplyOption()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                XDialogService service = new(
                    new StubDialogOwnerResolver(null),
                    presentedDialog =>
                    {
                        Assert.Equal(WindowStartupLocation.Manual, presentedDialog.WindowStartupLocation);
                        return true;
                    });

                service.ShowModal(
                    dialog,
                    options: new XDialogOptions
                    {
                        StartupLocation = WindowStartupLocation.Manual
                    });
            });
    }

    /// <summary>
    /// Verifies that CenterOwner falls back to CenterScreen without an owner.
    /// </summary>
    [Fact]
    public void ShowModal_WithCenterOwnerButNoOwner_ShouldUseCenterScreen()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                XDialogService service = new(
                    new StubDialogOwnerResolver(null),
                    presentedDialog =>
                    {
                        Assert.Equal(WindowStartupLocation.CenterScreen, presentedDialog.WindowStartupLocation);
                        return true;
                    });

                service.ShowModal(
                    dialog,
                    options: new XDialogOptions
                    {
                        StartupLocation = WindowStartupLocation.CenterOwner
                    });
            });
    }

    /// <summary>
    /// Verifies that nullable WPF dialog results are normalized.
    /// </summary>
    [Theory]
    [InlineData(true, XDialogOutcome.Accepted)]
    [InlineData(false, XDialogOutcome.NotAccepted)]
    [InlineData(null, XDialogOutcome.NoResult)]
    public void ShowModal_ShouldNormalizeNativeResult(
        bool? nativeResult,
        XDialogOutcome expectedOutcome)
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                XDialogService service = new(
                    new StubDialogOwnerResolver(null),
                    _ => nativeResult);

                XDialogResult result = service.ShowModal(dialog);

                Assert.Equal(expectedOutcome, result.Outcome);
                Assert.Equal(nativeResult, result.NativeResult);
                Assert.Equal(nativeResult == true, result.IsAccepted);
            });
    }

    /// <summary>
    /// Verifies that a null dialog is rejected.
    /// </summary>
    [Fact]
    public void ShowModal_WithNullDialog_ShouldThrow()
    {
        XDialogService service = new(new StubDialogOwnerResolver(null));

        Assert.Throws<ArgumentNullException>(() => service.ShowModal(null!));
    }
    #endregion

    #region ### Private Methods ###
    private static Window CreateVisibleOwner()
    {
        Window owner = new()
        {
            Width = 1,
            Height = 1,
            Left = -32000,
            Top = -32000,
            ShowActivated = false,
            ShowInTaskbar = false,
            WindowStyle = WindowStyle.None
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

        internal Window? LastDialog { get; private set; }

        internal DependencyObject? LastOwnerSource { get; private set; }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public Window? ResolveOwner(Window dialog, DependencyObject? ownerSource = null)
        {
            this.LastDialog = dialog;
            this.LastOwnerSource = ownerSource;
            return this.Owner;
        }
        #endregion
    }
    #endregion
}
#endregion