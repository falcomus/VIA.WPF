// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FocusNavigationServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Services;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class FocusNavigationServiceTests ###
/// <summary>
/// Provides tests for focus navigation service helpers.
/// </summary>
public sealed class FocusNavigationServiceTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that focus navigation helpers handle null roots safely.
    /// </summary>
    [Fact]
    public void FocusNavigationService_ShouldHandleNullRootsSafely()
    {
        Assert.False(FocusNavigationService.FocusFirstInput(null));
        Assert.False(FocusNavigationService.FocusNext(null));
        Assert.False(FocusNavigationService.FocusPrevious(null));
        Assert.Empty(FocusNavigationService.FindFocusableChildren(null));
    }

    /// <summary>
    /// Ensures that focusable child enumeration ignores disabled, hidden and non-tab-stop controls.
    /// </summary>
    [Fact]
    public void FocusNavigationService_FindFocusableChildren_ShouldIgnoreUnavailableControls()
    {
        WpfTestHelper.Run(
            () =>
            {
                StackPanel root = new();
                TextBox disabledTextBox = new()
                {
                    Focusable = true,
                    IsEnabled = false
                };
                Button hiddenButton = new()
                {
                    Focusable = true,
                    Visibility = Visibility.Collapsed
                };
                Button nonTabStopButton = new()
                {
                    Focusable = true,
                    IsTabStop = false
                };
                root.Children.Add(disabledTextBox);
                root.Children.Add(hiddenButton);
                root.Children.Add(nonTabStopButton);

                Assert.Empty(FocusNavigationService.FindFocusableChildren(root));
                Assert.False(FocusNavigationService.FocusFirstInput(root));
            });
    }
    #endregion
}
#endregion
