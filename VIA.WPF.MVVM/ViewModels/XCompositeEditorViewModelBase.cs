// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCompositeEditorViewModelBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VIA.WPF.MVVM;

#region ### Class XCompositeEditorViewModelBase ###
/// <summary>
/// Provides a reusable base class for aggregate editors that own child editors.
/// </summary>
/// <remarks>
/// The composite editor keeps child editor dirty and validation state observable at the root editor level.
/// Saving should call <see cref="ValidateForSaveAsync" /> on the root editor so all registered children are validated as well.
/// </remarks>
public abstract class XCompositeEditorViewModelBase : XEditorViewModelBase
{
    #region ### Fields ###
    private readonly ObservableCollection<XEditorViewModelBase> childEditors = [];
    private readonly ReadOnlyObservableCollection<XEditorViewModelBase> readOnlyChildEditors;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XCompositeEditorViewModelBase" /> class.
    /// </summary>
    protected XCompositeEditorViewModelBase()
    {
        this.readOnlyChildEditors = new ReadOnlyObservableCollection<XEditorViewModelBase>(this.childEditors);
        this.childEditors.CollectionChanged += this.OnChildEditorsCollectionChanged;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XCompositeEditorViewModelBase" /> class.
    /// </summary>
    /// <param name="messengerService">The messenger service.</param>
    protected XCompositeEditorViewModelBase(IXMessengerService messengerService)
        : base(messengerService)
    {
        this.readOnlyChildEditors = new ReadOnlyObservableCollection<XEditorViewModelBase>(this.childEditors);
        this.childEditors.CollectionChanged += this.OnChildEditorsCollectionChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the registered child editors.
    /// </summary>
    public ReadOnlyObservableCollection<XEditorViewModelBase> ChildEditors => this.readOnlyChildEditors;

    /// <summary>
    /// Gets a value indicating whether at least one child editor is dirty.
    /// </summary>
    public bool HasDirtyChildEditors => this.childEditors.Any(editor => editor.IsDirty);

    /// <summary>
    /// Gets a value indicating whether at least one child editor has validation errors.
    /// </summary>
    public bool HasChildErrors => this.childEditors.Any(editor => editor.HasErrors);

    /// <summary>
    /// Gets a value indicating whether this editor or any child editor has validation errors.
    /// </summary>
    public bool HasAnyErrors => this.HasErrors || this.HasChildErrors;
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void MarkClean()
    {
        foreach (XEditorViewModelBase childEditor in this.childEditors)
        {
            childEditor.MarkClean();
        }

        base.MarkClean();
        this.NotifyCompositeStateChanged();
    }

    /// <inheritdoc />
    public override async Task<bool> ValidateForSaveAsync(CancellationToken cancellationToken = default)
    {
        XValidationResult ownResult = await this.ValidateAllDetailedAsync(cancellationToken);
        List<XValidationError> combinedMessages = [.. ownResult.Messages];

        foreach (XEditorViewModelBase childEditor in this.childEditors)
        {
            _ = await childEditor.ValidateForSaveAsync(cancellationToken);
            combinedMessages.AddRange(childEditor.ValidationMessages);
        }

        this.ReplaceValidationMessages(combinedMessages);
        this.NotifyCompositeStateChanged();

        return combinedMessages.All(message => message.Severity != XValidationSeverity.Error);
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Registers a child editor for dirty and validation aggregation.
    /// </summary>
    /// <typeparam name="TEditor">The child editor type.</typeparam>
    /// <param name="editor">The child editor.</param>
    /// <returns>The registered editor.</returns>
    protected TEditor RegisterChildEditor<TEditor>(TEditor editor)
        where TEditor : XEditorViewModelBase
    {
        ArgumentNullException.ThrowIfNull(editor);

        if (!this.childEditors.Contains(editor))
        {
            this.childEditors.Add(editor);
        }

        return editor;
    }

    /// <summary>
    /// Unregisters a child editor.
    /// </summary>
    /// <param name="editor">The child editor.</param>
    protected void UnregisterChildEditor(XEditorViewModelBase? editor)
    {
        if (editor is not null)
        {
            this.childEditors.Remove(editor);
        }
    }

    /// <summary>
    /// Removes all registered child editors.
    /// </summary>
    protected void ClearChildEditors()
    {
        this.childEditors.Clear();
    }

    /// <inheritdoc />
    protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return base.ShouldValidateAfterPropertyChanged(propertyName)
            && propertyName is not nameof(this.ChildEditors)
            && propertyName is not nameof(this.HasDirtyChildEditors)
            && propertyName is not nameof(this.HasChildErrors)
            && propertyName is not nameof(this.HasAnyErrors);
    }

    /// <inheritdoc />
    protected override bool ShouldMarkDirty(string? propertyName)
    {
        return base.ShouldMarkDirty(propertyName)
            && propertyName is not nameof(this.ChildEditors)
            && propertyName is not nameof(this.HasDirtyChildEditors)
            && propertyName is not nameof(this.HasChildErrors)
            && propertyName is not nameof(this.HasAnyErrors);
    }
    #endregion

    #region ### Private Methods ###
    private void OnChildEditorsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (XEditorViewModelBase editor in e.OldItems.OfType<XEditorViewModelBase>())
            {
                this.DetachChildEditor(editor);
            }
        }

        if (e.NewItems is not null)
        {
            foreach (XEditorViewModelBase editor in e.NewItems.OfType<XEditorViewModelBase>())
            {
                this.AttachChildEditor(editor);
            }
        }

        this.NotifyCompositeStateChanged();
        this.RequestValidation(nameof(this.ChildEditors));
    }

    private void AttachChildEditor(XEditorViewModelBase editor)
    {
        editor.PropertyChanged += this.OnChildEditorPropertyChanged;
        editor.ErrorsChanged += this.OnChildEditorErrorsChanged;
    }

    private void DetachChildEditor(XEditorViewModelBase editor)
    {
        editor.PropertyChanged -= this.OnChildEditorPropertyChanged;
        editor.ErrorsChanged -= this.OnChildEditorErrorsChanged;
    }

    private void OnChildEditorPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(XEditorViewModelBase.IsDirty) && sender is XEditorViewModelBase { IsDirty: true })
        {
            this.IsDirty = true;
        }

        if (e.PropertyName is nameof(XEditorViewModelBase.IsDirty)
            or XValidatableObject.HasErrorsPropertyName
            or XValidatableObject.ValidationMessagesPropertyName
            or XValidatableObject.ValidationErrorsPropertyName)
        {
            this.NotifyCompositeStateChanged();
        }
    }

    private void OnChildEditorErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        _ = sender;
        _ = e;

        this.NotifyCompositeStateChanged();
    }

    private void NotifyCompositeStateChanged()
    {
        this.OnPropertiesChanged(
            nameof(this.ChildEditors),
            nameof(this.HasDirtyChildEditors),
            nameof(this.HasChildErrors),
            nameof(this.HasAnyErrors));
    }
    #endregion
}
#endregion
