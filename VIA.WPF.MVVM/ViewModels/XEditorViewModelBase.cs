// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEditorViewModelBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Class XEditorViewModelBase ###
/// <summary>
/// Provides a reusable base class for edit view models.
/// </summary>
public abstract class XEditorViewModelBase : XViewModelBase
{
    #region ### Fields ###
    private readonly List<XExternalValidationError> externalValidationErrors = [];
    private bool isDirty;
    private bool isReadOnly;
    private bool isDirtyTrackingEnabled = true;
    private bool clearExternalValidationErrorsOnEdit = true;
    private int dirtyTrackingSuppressionCount;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XEditorViewModelBase"/> class.
    /// </summary>
    protected XEditorViewModelBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XEditorViewModelBase"/> class.
    /// </summary>
    /// <param name="messengerService">The messenger service.</param>
    protected XEditorViewModelBase(IXMessengerService messengerService)
        : base(messengerService)
    {
    }
    #endregion

    #region ### Public Constants ###
    /// <summary>
    /// Provides the property name used for <see cref="ExternalValidationErrors" /> change notifications.
    /// </summary>
    public const string ExternalValidationErrorsPropertyName = nameof(ExternalValidationErrors);

    /// <summary>
    /// Provides the property name used for <see cref="HasExternalValidationErrors" /> change notifications.
    /// </summary>
    public const string HasExternalValidationErrorsPropertyName = nameof(HasExternalValidationErrors);
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the editor contains unsaved changes.
    /// </summary>
    public bool IsDirty
    {
        get => this.isDirty;
        set => this.SetProperty(ref this.isDirty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editor is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => this.isReadOnly;
        set
        {
            if (this.SetProperty(ref this.isReadOnly, value))
            {
                this.OnPropertyChanged(nameof(this.IsEditable));
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the editor can currently be edited.
    /// </summary>
    public bool IsEditable => !this.IsReadOnly;

    /// <summary>
    /// Gets the externally supplied validation errors, for example from server-side validation.
    /// </summary>
    public IReadOnlyList<XExternalValidationError> ExternalValidationErrors => this.externalValidationErrors;

    /// <summary>
    /// Gets a value indicating whether externally supplied validation errors exist.
    /// </summary>
    public bool HasExternalValidationErrors => this.externalValidationErrors.Count > 0;

    /// <summary>
    /// Gets or sets a value indicating whether dirty tracking is enabled.
    /// </summary>
    public bool IsDirtyTrackingEnabled
    {
        get => this.isDirtyTrackingEnabled;
        set => this.SetProperty(ref this.isDirtyTrackingEnabled, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether external validation errors are cleared when editable data changes.
    /// </summary>
    public bool ClearExternalValidationErrorsOnEdit
    {
        get => this.clearExternalValidationErrorsOnEdit;
        set => this.SetProperty(ref this.clearExternalValidationErrorsOnEdit, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Marks the editor as clean.
    /// </summary>
    public virtual void MarkClean()
    {
        this.IsDirty = false;
    }

    /// <summary>
    /// Validates the editor for saving.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> when the editor can be saved; otherwise <c>false</c>.</returns>
    public virtual Task<bool> ValidateForSaveAsync(CancellationToken cancellationToken = default)
    {
        return this.ValidateAllAsync(cancellationToken);
    }

    /// <summary>
    /// Replaces all external validation errors and requests validation refresh.
    /// </summary>
    /// <param name="errors">The external validation errors.</param>
    public void SetExternalValidationErrors(IEnumerable<XExternalValidationError>? errors)
    {
        this.externalValidationErrors.Clear();

        if (errors is not null)
        {
            this.externalValidationErrors.AddRange(errors.Where(error => error is not null));
        }

        this.OnExternalValidationErrorsChanged();
    }

    /// <summary>
    /// Adds one external validation error and requests validation refresh.
    /// </summary>
    /// <param name="error">The external validation error.</param>
    public void AddExternalValidationError(XExternalValidationError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        this.externalValidationErrors.Add(error);
        this.OnExternalValidationErrorsChanged();
    }

    /// <summary>
    /// Adds one external validation error and requests validation refresh.
    /// </summary>
    /// <param name="message">The validation message.</param>
    /// <param name="propertyName">The optional affected property name.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="code">The optional technical validation code.</param>
    public void AddExternalValidationError(string message, string? propertyName = null, XValidationSeverity severity = XValidationSeverity.Error, string? code = null)
    {
        this.AddExternalValidationError(XExternalValidationError.FromText(message, propertyName, severity, code));
    }

    /// <summary>
    /// Clears all external validation errors and requests validation refresh.
    /// </summary>
    public void ClearExternalValidationErrors()
    {
        if (this.externalValidationErrors.Count == 0)
        {
            return;
        }

        this.externalValidationErrors.Clear();
        this.OnExternalValidationErrorsChanged();
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Executes an action without marking the editor as dirty.
    /// </summary>
    /// <param name="action">The action.</param>
    protected void WithoutDirtyTracking(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        this.EnterDirtyTrackingSuppression();

        try
        {
            action();
        }
        finally
        {
            this.LeaveDirtyTrackingSuppression();
        }
    }

    /// <summary>
    /// Executes an asynchronous action without marking the editor as dirty.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task WithoutDirtyTrackingAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        this.EnterDirtyTrackingSuppression();

        try
        {
            await action().ConfigureAwait(false);
        }
        finally
        {
            this.LeaveDirtyTrackingSuppression();
        }
    }

    /// <summary>
    /// Executes an asynchronous action without marking the editor as dirty.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="action">The action.</param>
    /// <returns>The action result.</returns>
    protected async Task<TResult> WithoutDirtyTrackingAsync<TResult>(Func<Task<TResult>> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        this.EnterDirtyTrackingSuppression();

        try
        {
            return await action().ConfigureAwait(false);
        }
        finally
        {
            this.LeaveDirtyTrackingSuppression();
        }
    }

    /// <inheritdoc />
    protected override void CollectAdditionalValidationMessages(XValidationContext context)
    {
        base.CollectAdditionalValidationMessages(context);

        foreach (XExternalValidationError error in this.externalValidationErrors)
        {
            context.AddMessage(error.Text, error.Severity, error.PropertyNames, error.Code);
        }
    }

    /// <summary>
    /// Called when the external validation error collection changed.
    /// </summary>
    protected virtual void OnExternalValidationErrorsChanged()
    {
        this.OnPropertiesChanged(ExternalValidationErrorsPropertyName, HasExternalValidationErrorsPropertyName);
        this.RequestValidation(ExternalValidationErrorsPropertyName);
    }

    /// <summary>
    /// Determines whether a property should mark the editor as dirty when changed.
    /// </summary>
    /// <param name="propertyName">The changed property name.</param>
    /// <returns><c>true</c> when the property marks the editor as dirty; otherwise <c>false</c>.</returns>
    protected virtual bool ShouldMarkDirty(string? propertyName)
    {
        return this.IsDirtyTrackingEnabled
            && Volatile.Read(ref this.dirtyTrackingSuppressionCount) == 0
            && !this.IsInvalidatingProperties
            && !IsValidationInfrastructurePropertyName(propertyName)
            && propertyName is not nameof(this.IsDirty)
            && propertyName is not nameof(this.IsReadOnly)
            && propertyName is not nameof(this.IsEditable)
            && propertyName is not nameof(this.IsDirtyTrackingEnabled)
            && propertyName is not nameof(this.ClearExternalValidationErrorsOnEdit)
            && propertyName is not ExternalValidationErrorsPropertyName
            && propertyName is not HasExternalValidationErrorsPropertyName;
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        bool shouldMarkDirty = this.ShouldMarkDirty(e.PropertyName);
        if (!shouldMarkDirty)
        {
            return;
        }

        this.IsDirty = true;

        if (this.ClearExternalValidationErrorsOnEdit && this.HasExternalValidationErrors)
        {
            this.ClearExternalValidationErrors();
        }
    }

    /// <inheritdoc />
    protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return base.ShouldValidateAfterPropertyChanged(propertyName)
            && propertyName is not nameof(this.IsDirty)
            && propertyName is not nameof(this.IsReadOnly)
            && propertyName is not nameof(this.IsEditable)
            && propertyName is not nameof(this.IsDirtyTrackingEnabled)
            && propertyName is not nameof(this.ClearExternalValidationErrorsOnEdit)
            && propertyName is not ExternalValidationErrorsPropertyName
            && propertyName is not HasExternalValidationErrorsPropertyName;
    }
    #endregion

    #region ### Private Methods ###
    private void EnterDirtyTrackingSuppression()
    {
        Interlocked.Increment(ref this.dirtyTrackingSuppressionCount);
    }

    private void LeaveDirtyTrackingSuppression()
    {
        Interlocked.Decrement(ref this.dirtyTrackingSuppressionCount);
    }
    #endregion
}
#endregion
