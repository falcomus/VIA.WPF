// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidatableObject.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VIA.WPF.MVVM;

#region ### Class XValidatableObject ###
/// <summary>
/// Provides a validation-capable observable object based on <see cref="INotifyDataErrorInfo"/>.
/// </summary>
/// <remarks>
/// <para>
/// Instances are intended to be owned by the WPF UI thread or another clearly defined owner thread.
/// Asynchronous validation protects against stale validation results by using cancellation and version checks,
/// but the class does not provide a general-purpose, freely parallel multi-threaded collection API.
/// </para>
/// <para>
/// Subclasses may fire secondary <see cref="OnPropertyChanged"/> notifications from within their own
/// <see cref="OnPropertyChanged"/> override, for example to update derived display properties.
/// Display-only properties should be excluded from automatic validation by overriding
/// <see cref="ShouldValidateAfterPropertyChanged"/>.
/// </para>
/// </remarks>
public abstract class XValidatableObject : XObservableObject, INotifyDataErrorInfo
{
    #region ### Fields ###
    private readonly Lock syncRoot = new();
    private readonly ConcurrentDictionary<string, IReadOnlyList<XValidationError>> errorsByPropertyName = new(StringComparer.Ordinal);
    private CancellationTokenSource? pendingValidationCancellationTokenSource;
    private IReadOnlyList<XValidationError> validationMessages = [];
    private IReadOnlyList<XValidationError> validationErrors = [];
    private IReadOnlyList<XValidationError> validationWarnings = [];
    private IReadOnlyList<XValidationError> validationInformationMessages = [];
    private string validationSummaryText = string.Empty;
    private bool isValidationEnabled = true;
    private bool validateOnPropertyChanged = true;
    private bool validationTraceEnabled;
    private TimeSpan validationDelay = TimeSpan.FromMilliseconds(150);
    private bool isValidating;
    private int validationVersion;
    private int runningValidationCount;
    private int onPropertyChangedDepth;
    private int invalidatingPropertiesDepth;

#if DEBUG
    private readonly bool verifyOwnerThread = SynchronizationContext.Current is not null;
    private readonly int ownerManagedThreadId = Environment.CurrentManagedThreadId;
#endif
    #endregion

    #region ### Events ###
    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    #endregion

    #region ### Public Constants ###
    /// <summary>
    /// Provides the property name used for <see cref="IsValidationEnabled" /> change notifications.
    /// </summary>
    public const string IsValidationEnabledPropertyName = nameof(IsValidationEnabled);

    /// <summary>
    /// Provides the property name used for <see cref="ValidateOnPropertyChanged" /> change notifications.
    /// </summary>
    public const string ValidateOnPropertyChangedPropertyName = nameof(ValidateOnPropertyChanged);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationDelay" /> change notifications.
    /// </summary>
    public const string ValidationDelayPropertyName = nameof(ValidationDelay);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationTraceEnabled" /> change notifications.
    /// </summary>
    public const string ValidationTraceEnabledPropertyName = nameof(ValidationTraceEnabled);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationMessages" /> change notifications.
    /// </summary>
    public const string ValidationMessagesPropertyName = nameof(ValidationMessages);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationErrors" /> change notifications.
    /// </summary>
    public const string ValidationErrorsPropertyName = nameof(ValidationErrors);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationWarnings" /> change notifications.
    /// </summary>
    public const string ValidationWarningsPropertyName = nameof(ValidationWarnings);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationInformationMessages" /> change notifications.
    /// </summary>
    public const string ValidationInformationMessagesPropertyName = nameof(ValidationInformationMessages);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationSummaryText" /> change notifications.
    /// </summary>
    public const string ValidationSummaryTextPropertyName = nameof(ValidationSummaryText);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationErrorCount" /> change notifications.
    /// </summary>
    public const string ValidationErrorCountPropertyName = nameof(ValidationErrorCount);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationWarningCount" /> change notifications.
    /// </summary>
    public const string ValidationWarningCountPropertyName = nameof(ValidationWarningCount);

    /// <summary>
    /// Provides the property name used for <see cref="ValidationInformationCount" /> change notifications.
    /// </summary>
    public const string ValidationInformationCountPropertyName = nameof(ValidationInformationCount);

    /// <summary>
    /// Provides the property name used for <see cref="HasValidationMessages" /> change notifications.
    /// </summary>
    public const string HasValidationMessagesPropertyName = nameof(HasValidationMessages);

    /// <summary>
    /// Provides the property name used for <see cref="HasValidationWarnings" /> change notifications.
    /// </summary>
    public const string HasValidationWarningsPropertyName = nameof(HasValidationWarnings);

    /// <summary>
    /// Provides the property name used for <see cref="HasValidationInformation" /> change notifications.
    /// </summary>
    public const string HasValidationInformationPropertyName = nameof(HasValidationInformation);

    /// <summary>
    /// Provides the property name used for <see cref="HasErrors" /> change notifications.
    /// </summary>
    public const string HasErrorsPropertyName = nameof(HasErrors);

    /// <summary>
    /// Provides the property name used for <see cref="IsValid" /> change notifications.
    /// </summary>
    public const string IsValidPropertyName = nameof(IsValid);

    /// <summary>
    /// Provides the property name used for <see cref="IsValidating" /> change notifications.
    /// </summary>
    public const string IsValidatingPropertyName = nameof(IsValidating);
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether validation is enabled.
    /// </summary>
    /// <remarks>
    /// Setting this property to <see langword="false"/> cancels pending validation, invalidates stale async results
    /// and clears all current validation messages.
    /// </remarks>
    public bool IsValidationEnabled
    {
        get => this.isValidationEnabled;
        set
        {
            this.VerifyOwnerThread();

            if (!this.SetProperty(ref this.isValidationEnabled, value))
            {
                return;
            }

            if (!value)
            {
                this.CancelPendingValidation();
                Interlocked.Increment(ref this.validationVersion);
                this.ClearValidation();
                this.IsValidating = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether validation runs automatically after property changes.
    /// </summary>
    public bool ValidateOnPropertyChanged
    {
        get => this.validateOnPropertyChanged;
        set
        {
            this.VerifyOwnerThread();
            this.SetProperty(ref this.validateOnPropertyChanged, value);
        }
    }

    /// <summary>
    /// Gets or sets the delay used for automatic validation after property changes.
    /// </summary>
    public TimeSpan ValidationDelay
    {
        get => this.validationDelay;
        set
        {
            this.VerifyOwnerThread();
            this.SetProperty(ref this.validationDelay, value < TimeSpan.Zero ? TimeSpan.Zero : value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether validation trace messages are written through <see cref="OnValidationTrace"/>.
    /// </summary>
    public bool ValidationTraceEnabled
    {
        get => this.validationTraceEnabled;
        set
        {
            this.VerifyOwnerThread();
            this.SetProperty(ref this.validationTraceEnabled, value);
        }
    }

    /// <summary>
    /// Gets all current validation messages.
    /// </summary>
    public IReadOnlyList<XValidationError> ValidationMessages
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.validationMessages;
            }
        }
    }

    /// <summary>
    /// Gets all current validation errors.
    /// </summary>
    public IReadOnlyList<XValidationError> ValidationErrors
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.validationErrors;
            }
        }
    }

    /// <summary>
    /// Gets all current validation warnings.
    /// </summary>
    public IReadOnlyList<XValidationError> ValidationWarnings
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.validationWarnings;
            }
        }
    }

    /// <summary>
    /// Gets all current informational validation messages.
    /// </summary>
    public IReadOnlyList<XValidationError> ValidationInformationMessages
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.validationInformationMessages;
            }
        }
    }

    /// <summary>
    /// Gets a compact validation summary for status bars, tooltips or headers.
    /// </summary>
    public string ValidationSummaryText
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.validationSummaryText;
            }
        }
    }

    /// <summary>
    /// Gets the current number of validation error messages.
    /// </summary>
    public int ValidationErrorCount => this.ValidationErrors.Count;

    /// <summary>
    /// Gets the current number of validation warning messages.
    /// </summary>
    public int ValidationWarningCount => this.ValidationWarnings.Count;

    /// <summary>
    /// Gets the current number of informational validation messages.
    /// </summary>
    public int ValidationInformationCount => this.ValidationInformationMessages.Count;

    /// <summary>
    /// Gets a value indicating whether any validation message exists.
    /// </summary>
    public bool HasValidationMessages => this.ValidationMessages.Count > 0;

    /// <summary>
    /// Gets a value indicating whether validation warnings exist.
    /// </summary>
    public bool HasValidationWarnings => this.ValidationWarningCount > 0;

    /// <summary>
    /// Gets a value indicating whether informational validation messages exist.
    /// </summary>
    public bool HasValidationInformation => this.ValidationInformationCount > 0;

    /// <summary>
    /// Gets a value indicating whether validation is currently running.
    /// </summary>
    public bool IsValidating
    {
        get => this.isValidating;
        private set => this.SetProperty(ref this.isValidating, value);
    }

    /// <summary>
    /// Gets a value indicating whether the object has validation errors.
    /// </summary>
    public bool HasErrors
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.errorsByPropertyName.Any(pair => pair.Value.Count > 0);
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the object is currently valid.
    /// </summary>
    public bool IsValid => !this.HasErrors;
    #endregion

    #region ### Protected Properties ###
    /// <summary>
    /// Gets a value indicating whether property notifications are currently raised for validation invalidation.
    /// </summary>
    protected bool IsInvalidatingProperties => Volatile.Read(ref this.invalidatingPropertiesDepth) > 0;
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public IEnumerable GetErrors(string? propertyName)
    {
        lock (this.syncRoot)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return this.errorsByPropertyName.Values.SelectMany(errors => errors).Distinct().ToArray();
            }

            return this.errorsByPropertyName.TryGetValue(propertyName, out IReadOnlyList<XValidationError>? errors)
                ? errors.ToArray()
                : Array.Empty<XValidationError>();
        }
    }

    /// <summary>
    /// Validates the complete object immediately.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> when the object is valid; otherwise <c>false</c>.</returns>
    public async Task<bool> ValidateAllAsync(CancellationToken cancellationToken = default)
    {
        XValidationResult result = await this.ValidateAllDetailedAsync(cancellationToken);
        return result.IsValid;
    }

    /// <summary>
    /// Validates the complete object immediately and returns the full validation result.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed validation result.</returns>
    public async Task<XValidationResult> ValidateAllDetailedAsync(CancellationToken cancellationToken = default)
    {
        this.VerifyOwnerThread();

        if (!this.IsValidationEnabled)
        {
            this.CancelPendingValidation();
            Interlocked.Increment(ref this.validationVersion);
            this.ClearValidation();
            this.IsValidating = false;
            return XValidationResult.Success();
        }

        this.CancelPendingValidation();

        int version = Interlocked.Increment(ref this.validationVersion);
        await this.ValidateInternalAsync(version, TimeSpan.Zero, cancellationToken, null);
        return XValidationResult.FromMessages(this.ValidationMessages);
    }

    /// <summary>
    /// Clears all validation messages.
    /// </summary>
    public void ClearValidation()
    {
        this.VerifyOwnerThread();
        this.ReplaceValidationMessages([]);
    }

    /// <summary>
    /// Requests a full validation run using the configured validation delay.
    /// </summary>
    /// <param name="triggeredByProperty">The optional property that triggered the request.</param>
    public void RequestValidation(string? triggeredByProperty = null)
    {
        this.VerifyOwnerThread();
        this.QueueValidation(triggeredByProperty);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.VerifyOwnerThread();

        int depth = Interlocked.Increment(ref this.onPropertyChangedDepth);

        try
        {
            base.OnPropertyChanged(e);

            if (depth == 1 && this.ShouldValidateAfterPropertyChanged(e.PropertyName))
            {
                this.QueueValidation(e.PropertyName);
            }
        }
        finally
        {
            Interlocked.Decrement(ref this.onPropertyChangedDepth);
        }
    }

    /// <summary>
    /// Invalidates dependent properties and queues a single validation run for them.
    /// </summary>
    /// <param name="propertyNames">The dependent property names.</param>
    protected void InvalidateProperties(params string[] propertyNames)
    {
        this.RevalidateProperties(propertyNames);
    }

    /// <summary>
    /// Revalidates dependent properties and raises property notifications for them.
    /// </summary>
    /// <param name="propertyNames">The dependent property names.</param>
    protected void RevalidateProperties(params string[] propertyNames)
    {
        this.VerifyOwnerThread();

        string[] normalizedPropertyNames = XValidationHelpers.NormalizeExplicitPropertyNames(propertyNames);
        if (normalizedPropertyNames.Length == 0)
        {
            return;
        }

        Interlocked.Increment(ref this.invalidatingPropertiesDepth);

        try
        {
            foreach (string propertyName in normalizedPropertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }
        finally
        {
            Interlocked.Decrement(ref this.invalidatingPropertiesDepth);
        }

        if (this.IsValidationEnabled && this.ValidateOnPropertyChanged)
        {
            this.QueueValidation(string.Join(", ", normalizedPropertyNames));
        }
    }

    /// <summary>
    /// Executes object validation and writes messages to the specified context.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds validation messages that are maintained outside the normal validation core.
    /// </summary>
    /// <param name="context">The validation context.</param>
    protected virtual void CollectAdditionalValidationMessages(XValidationContext context)
    {
        _ = context;
    }

    /// <summary>
    /// Replaces the current validation messages with the specified snapshot.
    /// </summary>
    /// <param name="messages">The validation messages.</param>
    protected void ReplaceValidationMessages(IReadOnlyList<XValidationError> messages)
    {
        this.ReplaceValidationMessagesCore(messages);
    }

    /// <summary>
    /// Called whenever a validation run is requested or started.
    /// </summary>
    /// <param name="triggeredByProperty">The property that triggered validation, or <see langword="null"/> for explicit validation.</param>
    protected virtual void OnValidationStarting(string? triggeredByProperty)
    {
        if (!this.ValidationTraceEnabled)
        {
            return;
        }

        this.OnValidationTrace(
            $"[Validation] {this.GetType().Name}: triggered by '{triggeredByProperty ?? "<explicit>"}', " +
            $"Enabled={this.IsValidationEnabled}, OnPropChanged={this.ValidateOnPropertyChanged}, Delay={this.ValidationDelay}");
    }

    /// <summary>
    /// Writes a validation trace message.
    /// </summary>
    /// <param name="message">The trace message.</param>
    protected virtual void OnValidationTrace(string message)
    {
        if (Debugger.IsAttached)
        {
            Debug.WriteLine(message);
        }
    }

    /// <summary>
    /// Determines whether validation should run after a property changed.
    /// </summary>
    /// <param name="propertyName">The changed property name.</param>
    /// <returns><c>true</c> when validation should run; otherwise <c>false</c>.</returns>
    protected virtual bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return this.IsValidationEnabled
            && this.ValidateOnPropertyChanged
            && !this.IsInvalidatingProperties
            && !IsValidationInfrastructurePropertyName(propertyName);
    }

    /// <summary>
    /// Determines whether a property name belongs to the validation infrastructure and should not be treated as a domain property.
    /// </summary>
    /// <param name="propertyName">The property name to check.</param>
    /// <returns><c>true</c> when the property name is owned by validation infrastructure; otherwise <c>false</c>.</returns>
    protected static bool IsValidationInfrastructurePropertyName(string? propertyName)
    {
        return string.IsNullOrWhiteSpace(propertyName)
            || propertyName is IsValidationEnabledPropertyName
            || propertyName is ValidateOnPropertyChangedPropertyName
            || propertyName is ValidationDelayPropertyName
            || propertyName is ValidationTraceEnabledPropertyName
            || propertyName is ValidationMessagesPropertyName
            || propertyName is ValidationErrorsPropertyName
            || propertyName is ValidationWarningsPropertyName
            || propertyName is ValidationInformationMessagesPropertyName
            || propertyName is ValidationSummaryTextPropertyName
            || propertyName is ValidationErrorCountPropertyName
            || propertyName is ValidationWarningCountPropertyName
            || propertyName is ValidationInformationCountPropertyName
            || propertyName is HasValidationMessagesPropertyName
            || propertyName is HasValidationWarningsPropertyName
            || propertyName is HasValidationInformationPropertyName
            || propertyName is HasErrorsPropertyName
            || propertyName is IsValidPropertyName
            || propertyName is IsValidatingPropertyName;
    }
    #endregion

    #region ### Private Methods ###
    private void QueueValidation(string? triggeredByProperty)
    {
        if (!this.IsValidationEnabled)
        {
            return;
        }

        this.OnValidationStarting(triggeredByProperty);

        CancellationTokenSource cancellationTokenSource = new();
        CancellationTokenSource? previousCancellationTokenSource = Interlocked.Exchange(ref this.pendingValidationCancellationTokenSource, cancellationTokenSource);
        previousCancellationTokenSource?.Cancel();
        previousCancellationTokenSource?.Dispose();

        int version = Interlocked.Increment(ref this.validationVersion);
        _ = this.ValidateInternalAsync(version, this.ValidationDelay, cancellationTokenSource.Token, triggeredByProperty);
    }

    private async Task ValidateInternalAsync(int version, TimeSpan delay, CancellationToken cancellationToken, string? triggeredByProperty)
    {
        if (!this.IsValidationEnabled)
        {
            return;
        }

        this.OnValidationStarting(triggeredByProperty);

        Interlocked.Increment(ref this.runningValidationCount);
        this.IsValidating = true;

        try
        {
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, cancellationToken);
            }

            if (!this.IsValidationEnabled)
            {
                return;
            }

            XValidationContext context = new(this);
            await this.ValidateCoreAsync(context, cancellationToken);
            this.CollectAdditionalValidationMessages(context);

            if (!this.IsValidationEnabled || version != Volatile.Read(ref this.validationVersion) || cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.ReplaceValidationMessages(context.Messages);
        }
        catch (OperationCanceledException)
        {
            // Expected when validation is superseded by a newer validation request.
        }
        finally
        {
            if (Interlocked.Decrement(ref this.runningValidationCount) == 0)
            {
                this.IsValidating = false;
            }
        }
    }

    private void ReplaceValidationMessagesCore(IReadOnlyList<XValidationError> messages)
    {
        // VIA.WPF view models are expected to be used from the owner thread.
        // Async validation can prepare data asynchronously, but the final replacement is applied as one locked snapshot.
        IReadOnlyList<XValidationError> newMessages = messages.ToArray();
        IReadOnlyList<XValidationError> newValidationErrors = newMessages
            .Where(message => message.Severity == XValidationSeverity.Error)
            .ToArray();
        IReadOnlyList<XValidationError> newValidationWarnings = newMessages
            .Where(message => message.Severity == XValidationSeverity.Warning)
            .ToArray();
        IReadOnlyList<XValidationError> newValidationInformationMessages = newMessages
            .Where(message => message.Severity == XValidationSeverity.Information)
            .ToArray();
        string newValidationSummaryText = string.Join(
            Environment.NewLine,
            newMessages.Take(5).Select(message => message.Message));
        Dictionary<string, IReadOnlyList<XValidationError>> newErrors = newValidationErrors
            .SelectMany(message => message.PropertyNames.Select(propertyName => new { PropertyName = propertyName, Message = message }))
            .GroupBy(item => item.PropertyName, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => (IReadOnlyList<XValidationError>)group.Select(item => item.Message).ToArray(), StringComparer.Ordinal);

        string[] changedPropertyNames;
        bool validationMessagesChanged;

        lock (this.syncRoot)
        {
            string[] oldPropertyNames = this.errorsByPropertyName.Keys.ToArray();

            changedPropertyNames = oldPropertyNames
                .Concat(newErrors.Keys)
                .Distinct(StringComparer.Ordinal)
                .Where(propertyName =>
                {
                    this.errorsByPropertyName.TryGetValue(propertyName, out IReadOnlyList<XValidationError>? oldErrors);
                    newErrors.TryGetValue(propertyName, out IReadOnlyList<XValidationError>? currentErrors);

                    return !XValidationHelpers.ValidationMessagesEqual(oldErrors ?? [], currentErrors ?? []);
                })
                .ToArray();

            validationMessagesChanged = !XValidationHelpers.ValidationMessagesEqual(this.validationMessages, newMessages);

            if (!validationMessagesChanged && changedPropertyNames.Length == 0)
            {
                return;
            }

            this.errorsByPropertyName.Clear();

            foreach (KeyValuePair<string, IReadOnlyList<XValidationError>> pair in newErrors)
            {
                this.errorsByPropertyName[pair.Key] = pair.Value;
            }

            this.validationMessages = newMessages;
            this.validationErrors = newValidationErrors;
            this.validationWarnings = newValidationWarnings;
            this.validationInformationMessages = newValidationInformationMessages;
            this.validationSummaryText = newValidationSummaryText;
        }

        foreach (string propertyName in changedPropertyNames)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        this.OnPropertiesChanged(
            ValidationMessagesPropertyName,
            ValidationErrorsPropertyName,
            ValidationWarningsPropertyName,
            ValidationInformationMessagesPropertyName,
            ValidationSummaryTextPropertyName,
            ValidationErrorCountPropertyName,
            ValidationWarningCountPropertyName,
            ValidationInformationCountPropertyName,
            HasValidationMessagesPropertyName,
            HasValidationWarningsPropertyName,
            HasValidationInformationPropertyName,
            HasErrorsPropertyName,
            IsValidPropertyName);
    }

    private void CancelPendingValidation()
    {
        CancellationTokenSource? previousCancellationTokenSource = Interlocked.Exchange(ref this.pendingValidationCancellationTokenSource, null);
        previousCancellationTokenSource?.Cancel();
        previousCancellationTokenSource?.Dispose();
    }

    [Conditional("DEBUG")]
    private void VerifyOwnerThread([CallerMemberName] string? callerMemberName = null)
    {
#if DEBUG
        if (!this.verifyOwnerThread)
        {
            return;
        }

        if (Environment.CurrentManagedThreadId != this.ownerManagedThreadId)
        {
            throw new InvalidOperationException(
                $"XValidatableObject member '{callerMemberName}' was accessed from a different thread than the one that created the object.");
        }
#endif
    }
    #endregion
}
#endregion
