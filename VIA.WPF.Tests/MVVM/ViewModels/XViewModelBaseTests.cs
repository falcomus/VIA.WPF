// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewModelBaseTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.ViewModels;

#region ### Class XViewModelBaseTests ###
/// <summary>
/// Tests common <see cref="XViewModelBase" /> infrastructure.
/// </summary>
public sealed class XViewModelBaseTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that RunBusyAsync sets and restores busy state and busy text.
    /// </summary>
    [Fact]
    public async Task RunBusyAsync_ShouldSetAndRestoreBusyState()
    {
        TestViewModel viewModel = new(new TrackingMessengerService());

        int result = await viewModel.RunBusyAsync(
            token =>
            {
                _ = token;
                Assert.True(viewModel.IsBusy);
                Assert.Equal("Loading", viewModel.BusyText);
                return Task.FromResult(42);
            },
            "Loading");

        Assert.Equal(42, result);
        Assert.False(viewModel.IsBusy);
        Assert.Null(viewModel.BusyText);
    }

    /// <summary>
    /// Verifies that RunBusyAsync restores a previously active busy state.
    /// </summary>
    [Fact]
    public async Task RunBusyAsync_ShouldRestorePreviousBusyState()
    {
        TestViewModel viewModel = new(new TrackingMessengerService())
        {
            IsBusy = true,
            BusyText = "Old"
        };

        await viewModel.RunBusyAsync(_ => Task.CompletedTask, "New");

        Assert.True(viewModel.IsBusy);
        Assert.Equal("Old", viewModel.BusyText);
    }

    /// <summary>
    /// Verifies that ReloadCommand follows the busy state.
    /// </summary>
    [Fact]
    public void ReloadCommand_ShouldBeDisabledWhileBusy()
    {
        TestViewModel viewModel = new(new TrackingMessengerService());

        Assert.True(viewModel.ReloadCommand.CanExecute(null));

        viewModel.IsBusy = true;

        Assert.False(viewModel.ReloadCommand.CanExecute(null));
    }

    /// <summary>
    /// Verifies that Dispose unregisters all messages once.
    /// </summary>
    [Fact]
    public void Dispose_ShouldUnregisterAllMessagesOnce()
    {
        TrackingMessengerService messengerService = new();
        TestViewModel viewModel = new(messengerService);

        viewModel.Dispose();
        viewModel.Dispose();

        Assert.Equal(1, messengerService.UnregisterAllCallCount);
        Assert.Same(viewModel, messengerService.LastUnregisterAllRecipient);
    }

    /// <summary>
    /// Verifies protected message registration forwards to the messenger service.
    /// </summary>
    [Fact]
    public void RegisterMessage_ShouldForwardRegistrationToMessengerService()
    {
        TrackingMessengerService messengerService = new();
        TestViewModel viewModel = new(messengerService);

        viewModel.RegisterRefreshMessage();

        Assert.Equal(1, messengerService.RegisterCallCount);
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestViewModel : XViewModelBase
    {
        #region ### Constructors ###
        public TestViewModel(IXMessengerService messengerService)
            : base(messengerService)
        {
        }
        #endregion

        #region ### Public Methods ###
        public void RegisterRefreshMessage()
        {
            this.RegisterMessage<TestViewModel, XRefreshRequestedMessage>(this, static (_, _) => { });
        }
        #endregion
    }

    private sealed class TrackingMessengerService : IXMessengerService
    {
        #region ### Public Properties ###
        public int RegisterCallCount { get; private set; }

        public int UnregisterAllCallCount { get; private set; }

        public object? LastUnregisterAllRecipient { get; private set; }
        #endregion

        #region ### Public Methods ###
        public TMessage Send<TMessage>(TMessage message)
            where TMessage : class
        {
            return message;
        }

        public void Register<TRecipient, TMessage>(TRecipient recipient, Action<TRecipient, TMessage> handler)
            where TRecipient : class
            where TMessage : class
        {
            ArgumentNullException.ThrowIfNull(recipient);
            ArgumentNullException.ThrowIfNull(handler);
            this.RegisterCallCount++;
        }

        public void Unregister<TMessage>(object recipient)
            where TMessage : class
        {
            ArgumentNullException.ThrowIfNull(recipient);
        }

        public void UnregisterAll(object recipient)
        {
            ArgumentNullException.ThrowIfNull(recipient);
            this.UnregisterAllCallCount++;
            this.LastUnregisterAllRecipient = recipient;
        }
        #endregion
    }
    #endregion
}
#endregion
