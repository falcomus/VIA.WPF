// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowDialogResultTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Threading;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XWindowDialogResultTests ###
/// <summary>
/// Provides integration tests for modal dialog results and animated <see cref="XWindow"/> closing.
/// </summary>
public sealed class XWindowDialogResultTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that cancelling the first close for the close animation does not discard the modal result.
    /// </summary>
    /// <param name="nativeResult">The result assigned by the dialog.</param>
    /// <param name="expectedOutcome">The expected normalized service outcome.</param>
    [Theory]
    [InlineData(true, XDialogOutcome.Accepted)]
    [InlineData(false, XDialogOutcome.NotAccepted)]
    public void ShowModal_WithAnimatedClose_ShouldPreserveDialogResult(
        bool nativeResult,
        XDialogOutcome expectedOutcome)
    {
        WpfTestHelper.Run(
            () =>
            {
                XWindow dialog = new()
                {
                    Width = 240d,
                    Height = 140d,
                    ShowInTaskbar = false,
                    UseAnimations = true,
                    CloseAnimation = XWindowAnimationMode.Fade,
                    CloseAnimationDuration = new Duration(TimeSpan.FromMilliseconds(20))
                };

                dialog.Loaded += (_, _) =>
                {
                    dialog.Dispatcher.BeginInvoke(
                        DispatcherPriority.ApplicationIdle,
                        new Action(
                            () =>
                            {
                                dialog.DialogResult = nativeResult;

                                // UserFlow dialog handlers currently call Close after assigning DialogResult.
                                // The second close must not bypass the running close animation.
                                dialog.Close();
                            }));
                };

                XDialogResult result = XDialogService.Default.ShowModal(
                    dialog,
                    options: new XDialogOptions
                    {
                        DimOwner = false,
                        RestoreOwnerFocus = false
                    });

                Assert.Equal(expectedOutcome, result.Outcome);
                Assert.Equal(nativeResult, result.NativeResult);
                Assert.Equal(nativeResult, result.IsAccepted);
            });
    }
    #endregion
}
#endregion
