// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXMessengerService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Interface IXMessengerService ###
/// <summary>
/// Provides a thin abstraction for decoupled MVVM messaging.
/// </summary>
public interface IXMessengerService
{
    #region ### Methods ###
    /// <summary>
    /// Sends a message to registered recipients.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="message">The message.</param>
    /// <returns>The sent message.</returns>
    TMessage Send<TMessage>(TMessage message)
        where TMessage : class;

    /// <summary>
    /// Registers a recipient for a message type.
    /// </summary>
    /// <typeparam name="TRecipient">The recipient type.</typeparam>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="recipient">The recipient.</param>
    /// <param name="handler">The message handler.</param>
    void Register<TRecipient, TMessage>(TRecipient recipient, Action<TRecipient, TMessage> handler)
        where TRecipient : class
        where TMessage : class;

    /// <summary>
    /// Unregisters a recipient from a message type.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="recipient">The recipient.</param>
    void Unregister<TMessage>(object recipient)
        where TMessage : class;

    /// <summary>
    /// Unregisters a recipient from all message types.
    /// </summary>
    /// <param name="recipient">The recipient.</param>
    void UnregisterAll(object recipient);
    #endregion
}
#endregion
