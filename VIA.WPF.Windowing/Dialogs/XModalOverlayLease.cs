// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XModalOverlayLease.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Threading;

namespace VIA.WPF.Windowing;

#region ### Class XModalOverlayLease ###
/// <summary>
/// Releases an owner-local modal overlay exactly once.
/// </summary>
internal sealed class XModalOverlayLease : IDisposable
{
    #region ### Fields ###
    private readonly Dispatcher dispatcher;
    private Action? releaseAction;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XModalOverlayLease"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher that owns the overlay host.</param>
    /// <param name="releaseAction">The action that releases the overlay.</param>
    internal XModalOverlayLease(Dispatcher dispatcher, Action releaseAction)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(releaseAction);

        this.dispatcher = dispatcher;
        this.releaseAction = releaseAction;
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public void Dispose()
    {
        this.dispatcher.VerifyAccess();

        Action? action = Interlocked.Exchange(ref this.releaseAction, null);
        action?.Invoke();
    }
    #endregion
}
#endregion