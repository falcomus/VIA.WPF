// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewModelBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.MVVM;

#region ### Class XViewModelBase ###
/// <summary>
/// Provides a reusable base class for application view models.
/// </summary>
public abstract class XViewModelBase : XValidatableObject, IDisposable
{
    #region ### Fields ###
    private string? title;
    private string? description;
    private bool isBusy;
    private string? busyText;
    private bool isDisposed;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XViewModelBase"/> class.
    /// </summary>
    protected XViewModelBase()
        : this(XMessengerService.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XViewModelBase"/> class.
    /// </summary>
    /// <param name="messengerService">The messenger service.</param>
    protected XViewModelBase(IXMessengerService messengerService)
    {
        this.MessengerService = messengerService ?? throw new ArgumentNullException(nameof(messengerService));
        this.ReloadCommand = new AsyncRelayCommand(this.ReloadAsync, this.CanReload);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the view model title.
    /// </summary>
    public string? Title
    {
        get => this.title;
        set => this.SetProperty(ref this.title, value);
    }

    /// <summary>
    /// Gets or sets the view model description.
    /// </summary>
    public string? Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the view model is busy.
    /// </summary>
    public bool IsBusy
    {
        get => this.isBusy;
        set
        {
            if (this.SetProperty(ref this.isBusy, value))
            {
                this.ReloadCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the optional busy text.
    /// </summary>
    public string? BusyText
    {
        get => this.busyText;
        set => this.SetProperty(ref this.busyText, value);
    }

    /// <summary>
    /// Gets the reload command.
    /// </summary>
    public IAsyncRelayCommand ReloadCommand { get; }

    /// <summary>
    /// Gets the messenger service.
    /// </summary>
    public IXMessengerService MessengerService { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Runs an asynchronous operation while the busy state is active.
    /// </summary>
    /// <param name="operation">The operation.</param>
    /// <param name="busyText">The optional busy text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RunBusyAsync(Func<CancellationToken, Task> operation, string? busyText = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        await this.RunBusyAsync(
            async token =>
            {
                await operation(token);
                return true;
            },
            busyText,
            cancellationToken);
    }

    /// <summary>
    /// Runs an asynchronous operation while the busy state is active.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="operation">The operation.</param>
    /// <param name="busyText">The optional busy text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The operation result.</returns>
    public async Task<TResult> RunBusyAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, string? busyText = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        bool oldIsBusy = this.IsBusy;
        string? oldBusyText = this.BusyText;

        try
        {
            this.IsBusy = true;
            this.BusyText = busyText;
            return await operation(cancellationToken);
        }
        finally
        {
            this.BusyText = oldBusyText;
            this.IsBusy = oldIsBusy;
        }
    }

    /// <summary>
    /// Releases message registrations held by this view model.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Sends a message through the configured messenger service.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="message">The message.</param>
    /// <returns>The sent message.</returns>
    protected TMessage SendMessage<TMessage>(TMessage message)
        where TMessage : class
    {
        return this.MessengerService.Send(message);
    }

    /// <summary>
    /// Registers the current view model for a message type.
    /// </summary>
    /// <typeparam name="TRecipient">The recipient type.</typeparam>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="recipient">The recipient.</param>
    /// <param name="handler">The message handler.</param>
    protected void RegisterMessage<TRecipient, TMessage>(TRecipient recipient, Action<TRecipient, TMessage> handler)
        where TRecipient : class
        where TMessage : class
    {
        this.MessengerService.Register(recipient, handler);
    }

    /// <summary>
    /// Unregisters the current view model from all messages.
    /// </summary>
    protected void UnregisterAllMessages()
    {
        this.MessengerService.UnregisterAll(this);
    }

    /// <summary>
    /// Releases managed resources used by this view model.
    /// </summary>
    /// <param name="disposing"><c>true</c> when managed resources should be released; otherwise <c>false</c>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.UnregisterAllMessages();
        }

        this.isDisposed = true;
    }

    /// <summary>
    /// Determines whether reload can execute.
    /// </summary>
    /// <returns><c>true</c> when reload can execute; otherwise <c>false</c>.</returns>
    protected virtual bool CanReload()
    {
        return !this.IsBusy;
    }

    /// <summary>
    /// Executes reload logic.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task ReloadCoreAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return base.ShouldValidateAfterPropertyChanged(propertyName)
            && propertyName is not nameof(this.Title)
            && propertyName is not nameof(this.Description)
            && propertyName is not nameof(this.IsBusy)
            && propertyName is not nameof(this.BusyText);
    }
    #endregion

    #region ### Private Methods ###
    private async Task ReloadAsync()
    {
        await this.RunBusyAsync(this.ReloadCoreAsync);
    }
    #endregion
}
#endregion
