// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogOwnerResolver.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Windowing;

#region ### Class XDialogOwnerResolver ###
/// <summary>
/// Resolves dialog owners using deterministic WPF window rules.
/// </summary>
public sealed class XDialogOwnerResolver : IXDialogOwnerResolver
{
    #region ### Fields ###
    private readonly Func<IEnumerable<Window>> windowsProvider;
    private readonly Func<Window?> mainWindowProvider;
    private readonly Func<Window, bool> isWindowVisible;
    private readonly Func<Window, bool> isWindowActive;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogOwnerResolver"/> class.
    /// </summary>
    public XDialogOwnerResolver()
        : this(
            GetApplicationWindows,
            GetApplicationMainWindow,
            static window => window.IsVisible,
            static window => window.IsActive)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogOwnerResolver"/> class
    /// with deterministic environment providers.
    /// </summary>
    internal XDialogOwnerResolver(
        Func<IEnumerable<Window>> windowsProvider,
        Func<Window?> mainWindowProvider,
        Func<Window, bool> isWindowVisible,
        Func<Window, bool> isWindowActive)
    {
        ArgumentNullException.ThrowIfNull(windowsProvider);
        ArgumentNullException.ThrowIfNull(mainWindowProvider);
        ArgumentNullException.ThrowIfNull(isWindowVisible);
        ArgumentNullException.ThrowIfNull(isWindowActive);

        this.windowsProvider = windowsProvider;
        this.mainWindowProvider = mainWindowProvider;
        this.isWindowVisible = isWindowVisible;
        this.isWindowActive = isWindowActive;
    }
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the shared stateless owner resolver.
    /// </summary>
    public static XDialogOwnerResolver Default { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public Window? ResolveOwner(Window dialog, DependencyObject? ownerSource = null)
    {
        ArgumentNullException.ThrowIfNull(dialog);
        dialog.Dispatcher.VerifyAccess();

        if (dialog.Owner is Window configuredOwner)
        {
            return this.ValidateExplicitOwner(dialog, configuredOwner, "dialog.Owner");
        }

        if (ownerSource is not null)
        {
            if (!ReferenceEquals(ownerSource.Dispatcher, dialog.Dispatcher))
            {
                throw new InvalidOperationException(
                    "The dialog and owner source must belong to the same dispatcher.");
            }

            Window? sourceOwner = ownerSource as Window ?? Window.GetWindow(ownerSource);

            if (sourceOwner is not null)
            {
                return this.ValidateExplicitOwner(dialog, sourceOwner, "ownerSource");
            }
        }

        Window[] windows = this.windowsProvider()
            .Distinct()
            .ToArray();

        Window? activeWindow = windows.FirstOrDefault(
            window => this.IsAutomaticCandidate(dialog, window) && this.isWindowActive(window));

        if (activeWindow is not null)
        {
            return activeWindow;
        }

        Window? mainWindow = this.mainWindowProvider();

        if (mainWindow is not null && this.IsAutomaticCandidate(dialog, mainWindow))
        {
            return mainWindow;
        }

        return null;
    }
    #endregion

    #region ### Private Methods ###
    private static IEnumerable<Window> GetApplicationWindows()
    {
        Application? application = Application.Current;

        return application is null
            ? Array.Empty<Window>()
            : application.Windows.Cast<Window>().ToArray();
    }

    private static Window? GetApplicationMainWindow()
    {
        return Application.Current?.MainWindow;
    }

    private Window ValidateExplicitOwner(Window dialog, Window owner, string sourceName)
    {
        if (ReferenceEquals(dialog, owner))
        {
            throw new InvalidOperationException(
                $"The resolved dialog owner from {sourceName} cannot be the dialog itself.");
        }

        if (!ReferenceEquals(dialog.Dispatcher, owner.Dispatcher))
        {
            throw new InvalidOperationException(
                $"The resolved dialog owner from {sourceName} belongs to a different dispatcher.");
        }

        if (!this.isWindowVisible(owner))
        {
            throw new InvalidOperationException(
                $"The resolved dialog owner from {sourceName} is not visible.");
        }

        return owner;
    }

    private bool IsAutomaticCandidate(Window dialog, Window candidate)
    {
        return !ReferenceEquals(dialog, candidate)
            && ReferenceEquals(dialog.Dispatcher, candidate.Dispatcher)
            && this.isWindowVisible(candidate);
    }
    #endregion
}
#endregion