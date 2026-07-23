// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCrudEditorPageViewModelBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using VIA.WPF.MVVM;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XCrudEditorPageViewModelBase ###
/// <summary>
/// Provides a reusable base class for CRUD pages with a page-local detail editor.
/// </summary>
/// <typeparam name="TEntity">The list entity type.</typeparam>
/// <typeparam name="TEditor">The editor view model type.</typeparam>
/// <typeparam name="TKey">The entity key type.</typeparam>
public abstract class XCrudEditorPageViewModelBase<TEntity, TEditor, TKey> : XPageViewModelBase, IXPageContext, IXCrudPageContext
    where TEntity : class
    where TEditor : XEditorViewModelBase
    where TKey : notnull
{
    #region ### Fields ###
    private TEntity? selectedItem;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XCrudEditorPageViewModelBase{TEntity,TEditor,TKey}" /> class.
    /// </summary>
    protected XCrudEditorPageViewModelBase()
    {
        this.NewCommand = new AsyncRelayCommand(this.OpenCreateAsync, this.CanCreate);
        this.ViewCommand = new AsyncRelayCommand(this.OpenSelectedForViewAsync, this.CanViewSelected);
        this.EditCommand = new AsyncRelayCommand(this.OpenSelectedForEditAsync, this.CanEditSelected);
        this.DeleteCommand = new AsyncRelayCommand(this.DeleteSelectedAsync, this.CanDeleteSelected);
        this.SaveDetailCommand = new AsyncRelayCommand(this.SaveDetailAsync, this.CanSaveDetail);
        this.Toolbar = this.CreateToolbarContext();
        this.ConfigureToolbarCommands();
        this.CrudContext.PropertyChanged += this.OnCrudContextPropertyChanged;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XCrudEditorPageViewModelBase{TEntity,TEditor,TKey}" /> class.
    /// </summary>
    /// <param name="messengerService">The messenger service.</param>
    protected XCrudEditorPageViewModelBase(IXMessengerService messengerService)
        : base(messengerService)
    {
        this.NewCommand = new AsyncRelayCommand(this.OpenCreateAsync, this.CanCreate);
        this.ViewCommand = new AsyncRelayCommand(this.OpenSelectedForViewAsync, this.CanViewSelected);
        this.EditCommand = new AsyncRelayCommand(this.OpenSelectedForEditAsync, this.CanEditSelected);
        this.DeleteCommand = new AsyncRelayCommand(this.DeleteSelectedAsync, this.CanDeleteSelected);
        this.SaveDetailCommand = new AsyncRelayCommand(this.SaveDetailAsync, this.CanSaveDetail);
        this.Toolbar = this.CreateToolbarContext();
        this.ConfigureToolbarCommands();
        this.CrudContext.PropertyChanged += this.OnCrudContextPropertyChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the loaded list entities.
    /// </summary>
    public ObservableCollection<TEntity> Items { get; } = [];

    /// <summary>
    /// Gets or sets the selected entity.
    /// </summary>
    public TEntity? SelectedItem
    {
        get => this.selectedItem;
        set
        {
            if (this.SetProperty(ref this.selectedItem, value))
            {
                this.CrudContext.SelectedItem = value;
                this.NotifyCommandStatesChanged();
                this.OnPropertyChanged(nameof(this.HasSelectedItem));
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether an entity is selected.
    /// </summary>
    public bool HasSelectedItem => this.SelectedItem is not null;

    /// <inheritdoc />
    public XToolbarContext Toolbar { get; }

    /// <inheritdoc />
    public XCrudContext CrudContext { get; } = new();

    /// <summary>
    /// Gets the command that opens a create editor.
    /// </summary>
    public IAsyncRelayCommand NewCommand { get; }

    /// <summary>
    /// Gets the command that opens the selected entity read-only.
    /// </summary>
    public IAsyncRelayCommand ViewCommand { get; }

    /// <summary>
    /// Gets the command that opens the selected entity for editing.
    /// </summary>
    public IAsyncRelayCommand EditCommand { get; }

    /// <summary>
    /// Gets the command that deletes the selected entity.
    /// </summary>
    public IAsyncRelayCommand DeleteCommand { get; }

    /// <summary>
    /// Gets the command that saves the active detail editor.
    /// </summary>
    public IAsyncRelayCommand SaveDetailCommand { get; }

    /// <inheritdoc />
    ICommand? IXCrudPageContext.SaveDetailCommand => this.SaveDetailCommand;

    /// <inheritdoc />
    string IXPageContext.Title => this.Title ?? string.Empty;

    /// <inheritdoc />
    string IXPageContext.Description => this.Description ?? string.Empty;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Loads the entities and replaces the current list snapshot.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LoadItemsAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TEntity> loadedItems = await this.LoadItemsCoreAsync(cancellationToken);
        this.Items.Clear();

        foreach (TEntity item in loadedItems)
        {
            this.Items.Add(item);
        }

        this.SelectedItem = this.Items.FirstOrDefault();
        this.NotifyCommandStatesChanged();
    }
    #endregion

    #region ### Protected Properties ###
    /// <summary>
    /// Gets a value indicating whether create is supported by this page.
    /// </summary>
    protected virtual bool SupportsCreate => true;

    /// <summary>
    /// Gets a value indicating whether read-only view mode is supported by this page.
    /// </summary>
    protected virtual bool SupportsView => true;

    /// <summary>
    /// Gets a value indicating whether edit is supported by this page.
    /// </summary>
    protected virtual bool SupportsEdit => true;

    /// <summary>
    /// Gets a value indicating whether delete is supported by this page.
    /// </summary>
    protected virtual bool SupportsDelete => true;
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Loads the entity list.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The loaded entities.</returns>
    protected abstract Task<IReadOnlyList<TEntity>> LoadItemsCoreAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the entity key.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The entity key.</returns>
    protected abstract TKey GetEntityKey(TEntity entity);

    /// <summary>
    /// Creates an editor for a new entity.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The editor.</returns>
    protected abstract Task<TEditor> CreateEditorForCreateAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Creates an editor for an existing entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="mode">The detail mode.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The editor.</returns>
    protected abstract Task<TEditor> CreateEditorForEntityAsync(TEntity entity, XCrudMode mode, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new entity from an editor.
    /// </summary>
    /// <param name="editor">The editor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created entity.</returns>
    protected abstract Task<TEntity> CreateEntityAsync(TEditor editor, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity from an editor.
    /// </summary>
    /// <param name="entity">The original entity.</param>
    /// <param name="editor">The editor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated entity.</returns>
    protected abstract Task<TEntity> UpdateEntityAsync(TEntity entity, TEditor editor, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the toolbar context used by the page.
    /// </summary>
    /// <returns>The toolbar context.</returns>
    protected virtual XToolbarContext CreateToolbarContext()
    {
        return new XToolbarContext
        {
            ShowNewButton = this.SupportsCreate,
            ShowViewButton = this.SupportsView,
            ShowEditButton = this.SupportsEdit,
            ShowDeleteButton = this.SupportsDelete,
            ShowRefreshButton = true,
        };
    }

    /// <summary>
    /// Creates the detail title for the specified mode.
    /// </summary>
    /// <param name="mode">The detail mode.</param>
    /// <param name="entity">The optional entity.</param>
    /// <param name="editor">The editor.</param>
    /// <returns>The detail title.</returns>
    protected virtual string CreateDetailTitle(XCrudMode mode, TEntity? entity, TEditor editor)
    {
        _ = entity;
        _ = editor;

        return mode switch
        {
            XCrudMode.Create => "Create",
            XCrudMode.View => "View",
            XCrudMode.Edit => "Edit",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// Called after the active detail editor has been saved successfully.
    /// </summary>
    /// <param name="mode">The saved mode.</param>
    /// <param name="entity">The saved entity.</param>
    /// <param name="editor">The saved editor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task OnDetailSavedAsync(XCrudMode mode, TEntity entity, TEditor editor, CancellationToken cancellationToken)
    {
        _ = mode;
        _ = entity;
        _ = editor;
        _ = cancellationToken;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when saving the active detail editor failed.
    /// </summary>
    /// <param name="editor">The editor.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task OnDetailSaveFailedAsync(TEditor editor, Exception exception, CancellationToken cancellationToken)
    {
        _ = cancellationToken;

        editor.SetExternalValidationErrors(
            [
                XExternalValidationError.FromText(
                    exception.Message,
                    propertyName: null,
                    severity: XValidationSeverity.Error,
                    code: exception.GetType().Name)
            ]);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Called after an entity has been deleted successfully.
    /// </summary>
    /// <param name="entity">The deleted entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task OnEntityDeletedAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _ = entity;
        _ = cancellationToken;

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override async Task ReloadCoreAsync(CancellationToken cancellationToken)
    {
        await this.LoadItemsAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(this.IsBusy))
        {
            this.NotifyCommandStatesChanged();
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.CrudContext.PropertyChanged -= this.OnCrudContextPropertyChanged;
        }

        base.Dispose(disposing);
    }
    #endregion

    #region ### Private Methods ###
    private void ConfigureToolbarCommands()
    {
        this.Toolbar.NewCommand = this.NewCommand;
        this.Toolbar.ViewCommand = this.ViewCommand;
        this.Toolbar.EditCommand = this.EditCommand;
        this.Toolbar.DeleteCommand = this.DeleteCommand;
        this.Toolbar.RefreshCommand = this.ReloadCommand;
    }

    private bool CanCreate()
    {
        return this.SupportsCreate && !this.IsBusy;
    }

    private bool CanOpenSelected()
    {
        return this.SelectedItem is not null && !this.IsBusy;
    }

    private bool CanViewSelected()
    {
        return this.SupportsView && this.CanOpenSelected();
    }

    private bool CanEditSelected()
    {
        return this.SupportsEdit && this.CanOpenSelected();
    }

    private bool CanDeleteSelected()
    {
        return this.SupportsDelete && this.CanOpenSelected();
    }

    private bool CanSaveDetail()
    {
        return this.CrudContext.Editor is TEditor
            && this.CrudContext.Mode is XCrudMode.Create or XCrudMode.Edit
            && !this.IsBusy;
    }

    private async Task OpenCreateAsync()
    {
        if (!this.SupportsCreate)
        {
            return;
        }

        TEditor editor = await this.CreateEditorForCreateAsync(CancellationToken.None);
        editor.IsReadOnly = false;
        editor.MarkClean();
        this.CrudContext.Open(XCrudMode.Create, editor, this.CreateDetailTitle(XCrudMode.Create, null, editor));
        this.NotifyCommandStatesChanged();
    }

    private async Task OpenSelectedForViewAsync()
    {
        if (!this.SupportsView || this.SelectedItem is null)
        {
            return;
        }

        TEntity entity = this.SelectedItem;
        TEditor editor = await this.CreateEditorForEntityAsync(entity, XCrudMode.View, CancellationToken.None);
        editor.IsReadOnly = true;
        editor.MarkClean();
        this.CrudContext.Open(XCrudMode.View, editor, this.CreateDetailTitle(XCrudMode.View, entity, editor));
        this.NotifyCommandStatesChanged();
    }

    private async Task OpenSelectedForEditAsync()
    {
        if (!this.SupportsEdit || this.SelectedItem is null)
        {
            return;
        }

        TEntity entity = this.SelectedItem;
        TEditor editor = await this.CreateEditorForEntityAsync(entity, XCrudMode.Edit, CancellationToken.None);
        editor.IsReadOnly = false;
        editor.MarkClean();
        this.CrudContext.Open(XCrudMode.Edit, editor, this.CreateDetailTitle(XCrudMode.Edit, entity, editor));
        this.NotifyCommandStatesChanged();
    }

    private async Task SaveDetailAsync()
    {
        if (this.CrudContext.Editor is not TEditor editor || this.CrudContext.Mode is not (XCrudMode.Create or XCrudMode.Edit))
        {
            return;
        }

        editor.ClearExternalValidationErrors();

        if (!await editor.ValidateForSaveAsync())
        {
            return;
        }

        XCrudMode mode = this.CrudContext.Mode;
        TEntity? selectedEntity = this.SelectedItem;

        try
        {
            TEntity savedEntity = mode == XCrudMode.Create
                ? await this.CreateEntityAsync(editor, CancellationToken.None)
                : await this.UpdateEntityAsync(selectedEntity ?? throw new InvalidOperationException("No entity is selected for update."), editor, CancellationToken.None);

            this.UpsertSavedEntity(savedEntity);
            this.SelectedItem = savedEntity;
            editor.MarkClean();
            await this.OnDetailSavedAsync(mode, savedEntity, editor, CancellationToken.None);
            this.CrudContext.Close();
            this.SendEntitySavedMessages(mode, savedEntity);
        }
        catch (Exception exception)
        {
            await this.OnDetailSaveFailedAsync(editor, exception, CancellationToken.None);
        }
        finally
        {
            this.NotifyCommandStatesChanged();
        }
    }

    private async Task DeleteSelectedAsync()
    {
        if (!this.SupportsDelete || this.SelectedItem is null)
        {
            return;
        }

        TEntity entity = this.SelectedItem;
        await this.DeleteEntityAsync(entity, CancellationToken.None);
        this.Items.Remove(entity);
        this.SelectedItem = this.Items.FirstOrDefault();
        this.CrudContext.Close();
        await this.OnEntityDeletedAsync(entity, CancellationToken.None);
        this.SendMessage(new XEntityDeletedMessage<TEntity>(entity));
        this.NotifyCommandStatesChanged();
    }

    private void UpsertSavedEntity(TEntity savedEntity)
    {
        TKey savedKey = this.GetEntityKey(savedEntity);

        for (int index = 0; index < this.Items.Count; index++)
        {
            if (EqualityComparer<TKey>.Default.Equals(this.GetEntityKey(this.Items[index]), savedKey))
            {
                this.Items[index] = savedEntity;
                return;
            }
        }

        this.Items.Add(savedEntity);
    }

    private void SendEntitySavedMessages(XCrudMode mode, TEntity savedEntity)
    {
        if (mode == XCrudMode.Create)
        {
            this.SendMessage(new XEntityCreatedMessage<TEntity>(savedEntity));
        }
        else
        {
            this.SendMessage(new XEntityUpdatedMessage<TEntity>(savedEntity));
        }

        this.SendMessage(new XEntitySavedMessage<TEntity>(savedEntity));
    }

    private void NotifyCommandStatesChanged()
    {
        this.NewCommand.NotifyCanExecuteChanged();
        this.ViewCommand.NotifyCanExecuteChanged();
        this.EditCommand.NotifyCanExecuteChanged();
        this.DeleteCommand.NotifyCanExecuteChanged();
        this.SaveDetailCommand.NotifyCanExecuteChanged();
    }

    private void OnCrudContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _ = sender;

        if (e.PropertyName is nameof(XCrudContext.Editor) or nameof(XCrudContext.Mode) or nameof(XCrudContext.IsOpen))
        {
            this.NotifyCommandStatesChanged();
        }
    }
    #endregion
}
#endregion
