// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMessengerServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Messaging;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Messaging;

#region ### Class XMessengerServiceTests ###
/// <summary>
/// Tests <see cref="XMessengerService" /> message forwarding behavior.
/// </summary>
public sealed class XMessengerServiceTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that sent messages are delivered to registered recipients.
    /// </summary>
    [Fact]
    public void Send_ShouldDeliverMessageToRegisteredRecipient()
    {
        XMessengerService service = new(new WeakReferenceMessenger());
        TestRecipient recipient = new();
        TestMessage? receivedMessage = null;

        service.Register<TestRecipient, TestMessage>(
            recipient,
            (messageRecipient, message) =>
            {
                messageRecipient.ReceiveCount++;
                receivedMessage = message;
            });

        TestMessage sentMessage = new("Banana");
        TestMessage result = service.Send(sentMessage);

        Assert.Same(sentMessage, result);
        Assert.Same(sentMessage, receivedMessage);
        Assert.Equal(1, recipient.ReceiveCount);
    }

    /// <summary>
    /// Verifies that UnregisterAll removes registered recipients.
    /// </summary>
    [Fact]
    public void UnregisterAll_ShouldRemoveRecipientRegistrations()
    {
        XMessengerService service = new(new WeakReferenceMessenger());
        TestRecipient recipient = new();

        service.Register<TestRecipient, TestMessage>(recipient, static (messageRecipient, _) => messageRecipient.ReceiveCount++);
        service.UnregisterAll(recipient);
        service.Send(new TestMessage("Banana"));

        Assert.Equal(0, recipient.ReceiveCount);
    }
    #endregion

    #region ### Private Types ###
    private sealed class TestRecipient
    {
        public int ReceiveCount { get; set; }
    }

    private sealed record TestMessage(string Text);
    #endregion
}
#endregion
