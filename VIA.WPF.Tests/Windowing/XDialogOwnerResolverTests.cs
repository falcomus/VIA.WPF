// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogOwnerResolverTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Tests.Helpers;
using VIA.WPF.Windowing;

namespace VIA.WPF.Tests.Windowing;

#region ### Class XDialogOwnerResolverTests ###
/// <summary>
/// Provides tests for the <see cref="XDialogOwnerResolver"/> class.
/// </summary>
public sealed class XDialogOwnerResolverTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that an explicit window source is preferred over automatic candidates.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithExplicitWindowSource_ShouldReturnSourceWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window explicitOwner = new();
                Window automaticOwner = new();
                Window dialog = new();

                XDialogOwnerResolver resolver = CreateResolver(
                    windows: [automaticOwner],
                    mainWindow: automaticOwner,
                    activeWindow: automaticOwner);

                Window? result = resolver.ResolveOwner(dialog, explicitOwner);

                Assert.Same(explicitOwner, result);
            });
    }

    /// <summary>
    /// Verifies that a dependency object resolves its containing window.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithElementSource_ShouldReturnContainingWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border source = new();
                Window owner = new()
                {
                    Content = source
                };

                Window dialog = new();
                XDialogOwnerResolver resolver = CreateResolver();

                Assert.Same(owner, Window.GetWindow(source));
                Assert.Same(owner, resolver.ResolveOwner(dialog, source));
            });
    }

    /// <summary>
    /// Verifies that the active visible application window is used as fallback.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithActiveWindow_ShouldReturnActiveWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window first = new();
                Window active = new();
                Window dialog = new();

                XDialogOwnerResolver resolver = CreateResolver(
                    windows: [first, active],
                    activeWindow: active);

                Window? result = resolver.ResolveOwner(dialog);

                Assert.Same(active, result);
            });
    }

    /// <summary>
    /// Verifies that the visible main window is used when no active window exists.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithoutActiveWindow_ShouldReturnMainWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window candidate = new();
                Window mainWindow = new();
                Window dialog = new();

                XDialogOwnerResolver resolver = CreateResolver(
                    windows: [candidate],
                    mainWindow: mainWindow);

                Window? result = resolver.ResolveOwner(dialog);

                Assert.Same(mainWindow, result);
            });
    }

    /// <summary>
    /// Verifies that no owner is returned when no eligible window exists.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithoutEligibleWindow_ShouldReturnNull()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                XDialogOwnerResolver resolver = CreateResolver();

                Assert.Null(resolver.ResolveOwner(dialog));
            });
    }

    /// <summary>
    /// Verifies that a dialog cannot own itself.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithDialogAsExplicitSource_ShouldThrow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window dialog = new();
                XDialogOwnerResolver resolver = CreateResolver();

                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
                    () => resolver.ResolveOwner(dialog, dialog));

                Assert.Contains("cannot be the dialog itself", exception.Message, StringComparison.Ordinal);
            });
    }

    /// <summary>
    /// Verifies that an explicitly resolved invisible owner is rejected.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithInvisibleExplicitOwner_ShouldThrow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window owner = new();
                Window dialog = new();

                XDialogOwnerResolver resolver = CreateResolver(
                    visibility: window => !ReferenceEquals(window, owner));

                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
                    () => resolver.ResolveOwner(dialog, owner));

                Assert.Contains("is not visible", exception.Message, StringComparison.Ordinal);
            });
    }

    /// <summary>
    /// Verifies that an invisible automatic candidate is skipped.
    /// </summary>
    [Fact]
    public void ResolveOwner_WithInvisibleAutomaticCandidate_ShouldReturnNull()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window candidate = new();
                Window dialog = new();

                XDialogOwnerResolver resolver = CreateResolver(
                    windows: [candidate],
                    activeWindow: candidate,
                    visibility: _ => false);

                Assert.Null(resolver.ResolveOwner(dialog));
            });
    }
    #endregion

    #region ### Private Methods ###
    private static XDialogOwnerResolver CreateResolver(
        IEnumerable<Window>? windows = null,
        Window? mainWindow = null,
        Window? activeWindow = null,
        Func<Window, bool>? visibility = null)
    {
        Window[] windowSnapshot = windows?.ToArray() ?? Array.Empty<Window>();

        return new XDialogOwnerResolver(
            () => windowSnapshot,
            () => mainWindow,
            visibility ?? (_ => true),
            window => ReferenceEquals(window, activeWindow));
    }
    #endregion
}
#endregion