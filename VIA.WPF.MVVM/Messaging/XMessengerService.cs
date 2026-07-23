// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMessengerService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Messaging;

namespace VIA.WPF.MVVM;

#region ### Class XMessengerService ###
/// <summary>
/// Provides decoupled MVVM messaging based on <see cref="WeakReferenceMessenger"/>.
/// </summary>
public sealed class XMessengerService : IXMessengerService
{
    #region ### Fields ###
    private readonly IMessenger messenger;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XMessengerService"/> class.
    /// </summary>
    /// <param name="messenger">The underlying messenger.</param>
    public XMessengerService(IMessenger messenger)
    {
        this.messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
    }
    #endregion

    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the default messenger service instance.
    /// </summary>
    public static XMessengerService Default { get; } = new(WeakReferenceMessenger.Default);
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public TMessage Send<TMessage>(TMessage message)
        where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(message);
        return this.messenger.Send(message);
    }

    /// <inheritdoc />
    public void Register<TRecipient, TMessage>(TRecipient recipient, Action<TRecipient, TMessage> handler)
        where TRecipient : class
        where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(handler);

        this.messenger.Register<TRecipient, TMessage>(recipient, handler.Invoke);
    }

    /// <inheritdoc />
    public void Unregister<TMessage>(object recipient)
        where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(recipient);
        this.messenger.Unregister<TMessage>(recipient);
    }

    /// <inheritdoc />
    public void UnregisterAll(object recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);
        this.messenger.UnregisterAll(recipient);
    }
    #endregion
}
#endregion
