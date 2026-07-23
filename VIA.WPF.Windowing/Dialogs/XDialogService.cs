// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace VIA.WPF.Windowing;

#region ### Class XDialogService ###
/// <summary>
/// Provides deterministic modal WPF dialog presentation.
/// </summary>
public sealed class XDialogService : IXDialogService
{
    #region ### Fields ###
    private readonly IXDialogOwnerResolver ownerResolver;
    private readonly Func<Window, bool?> showDialog;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogService"/> class.
    /// </summary>
    public XDialogService()
        : this(XDialogOwnerResolver.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogService"/> class.
    /// </summary>
    /// <param name="ownerResolver">The owner resolver to use.</param>
    public XDialogService(IXDialogOwnerResolver ownerResolver)
        : this(ownerResolver, static dialog => dialog.ShowDialog())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogService"/> class
    /// with a deterministic dialog presenter.
    /// </summary>
    internal XDialogService(
        IXDialogOwnerResolver ownerResolver,
        Func<Window, bool?> showDialog)
    {
        ArgumentNullException.ThrowIfNull(ownerResolver);
        ArgumentNullException.ThrowIfNull(showDialog);

        this.ownerResolver = ownerResolver;
        this.showDialog = showDialog;
    }
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared stateless dialog service.
    /// </summary>
    public static XDialogService Default { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public XDialogResult ShowModal(
        Window dialog,
        DependencyObject? ownerSource = null,
        XDialogOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(dialog);
        dialog.Dispatcher.VerifyAccess();

        if (dialog.IsVisible)
        {
            throw new InvalidOperationException(
                "A visible window cannot be shown as a new modal dialog.");
        }

        options ??= XDialogOptions.Default;

        Window? resolvedOwner = this.ownerResolver.ResolveOwner(dialog, ownerSource);

        if (dialog.Owner is Window configuredOwner)
        {
            if (resolvedOwner is not null && !ReferenceEquals(configuredOwner, resolvedOwner))
            {
                throw new InvalidOperationException(
                    "The configured dialog owner differs from the owner returned by the resolver.");
            }

            resolvedOwner = configuredOwner;
        }
        else if (resolvedOwner is not null)
        {
            dialog.Owner = resolvedOwner;
        }

        ConfigureStartupLocation(dialog, resolvedOwner, options);

        IInputElement? previousFocus = CaptureOwnerFocus(resolvedOwner, options);
        IDisposable? overlayLease = null;

        try
        {
            if (options.DimOwner && resolvedOwner is IXModalOverlayHost overlayHost)
            {
                overlayLease = overlayHost.AcquireModalOverlay();
            }

            bool? nativeResult = this.showDialog(dialog);
            return XDialogResult.FromNativeResult(nativeResult);
        }
        finally
        {
            try
            {
                overlayLease?.Dispose();
            }
            finally
            {
                RestoreOwnerFocus(resolvedOwner, previousFocus, options);
            }
        }
    }
    #endregion

    #region ### Private Methods ###
    private static IInputElement? CaptureOwnerFocus(
        Window? owner,
        XDialogOptions options)
    {
        if (!options.RestoreOwnerFocus ||
            owner is null ||
            !owner.IsKeyboardFocusWithin)
        {
            return null;
        }

        return Keyboard.FocusedElement;
    }

    private static void ConfigureStartupLocation(
        Window dialog,
        Window? owner,
        XDialogOptions options)
    {
        if (options.StartupLocation is WindowStartupLocation requestedLocation)
        {
            dialog.WindowStartupLocation = NormalizeStartupLocation(requestedLocation, owner);
            return;
        }

        if (dialog.WindowStartupLocation == WindowStartupLocation.CenterOwner && owner is null)
        {
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            return;
        }

        if (dialog.WindowStartupLocation != WindowStartupLocation.Manual)
        {
            return;
        }

        dialog.WindowStartupLocation = owner is null
            ? WindowStartupLocation.CenterScreen
            : WindowStartupLocation.CenterOwner;
    }

    private static bool CanRestoreFocus(
        Window owner,
        IInputElement? previousFocus)
    {
        return previousFocus is UIElement element &&
            element.IsVisible &&
            element.IsEnabled &&
            element.Focusable &&
            ReferenceEquals(Window.GetWindow(element), owner);
    }

    private static WindowStartupLocation NormalizeStartupLocation(
        WindowStartupLocation startupLocation,
        Window? owner)
    {
        return startupLocation == WindowStartupLocation.CenterOwner && owner is null
            ? WindowStartupLocation.CenterScreen
            : startupLocation;
    }

    private static void RestoreOwnerFocus(
        Window? owner,
        IInputElement? previousFocus,
        XDialogOptions options)
    {
        if (!options.RestoreOwnerFocus ||
            owner?.IsVisible != true ||
            owner.Dispatcher.HasShutdownStarted)
        {
            return;
        }

        try
        {
            if (!owner.IsActive)
            {
                owner.Activate();
            }

            if (CanRestoreFocus(owner, previousFocus))
            {
                Keyboard.Focus(previousFocus);
            }
            else if (owner.IsEnabled && owner.Focusable)
            {
                owner.Focus();
            }
        }
        catch (InvalidOperationException)
        {
            // The owner may be closing while the modal dialog is unwinding.
        }
    }
    #endregion
}
#endregion
