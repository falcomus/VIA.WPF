// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCrudEditorPageViewModelBaseTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls.Navigation;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class XCrudEditorPageViewModelBaseTests ###
/// <summary>
/// Tests <see cref="XCrudEditorPageViewModelBase{TEntity,TEditor,TKey}" />.
/// </summary>
public sealed class XCrudEditorPageViewModelBaseTests
{
    #region ### Tests ###
    /// <summary>
    /// Ensures that loading replaces the item list and selects the first item.
    /// </summary>
    [Fact]
    public async Task LoadItemsAsync_ShouldReplaceItemsAndSelectFirstItem()
    {
        TestCrudPageViewModel viewModel = new();

        await viewModel.LoadItemsAsync();

        Assert.Equal(2, viewModel.Items.Count);
        Assert.Same(viewModel.Items[0], viewModel.SelectedItem);
        Assert.Same(viewModel.SelectedItem, viewModel.CrudContext.SelectedItem);
    }

    /// <summary>
    /// Ensures that edit opens a writable editor through the CRUD context.
    /// </summary>
    [Fact]
    public async Task EditCommand_ShouldOpenEditableDetailEditor()
    {
        TestCrudPageViewModel viewModel = new();
        await viewModel.LoadItemsAsync();

        await viewModel.EditCommand.ExecuteAsync(null);

        TestEditorViewModel editor = Assert.IsType<TestEditorViewModel>(viewModel.CrudContext.Editor);
        Assert.True(viewModel.CrudContext.IsOpen);
        Assert.Equal(XCrudMode.Edit, viewModel.CrudContext.Mode);
        Assert.False(editor.IsReadOnly);
        Assert.False(editor.IsDirty);
        Assert.Equal("Edit Alpha", viewModel.CrudContext.Title);
    }

    /// <summary>
    /// Ensures that invalid editors keep the detail context open.
    /// </summary>
    [Fact]
    public async Task SaveDetailCommand_WithInvalidEditor_ShouldKeepDetailOpen()
    {
        TestCrudPageViewModel viewModel = new();
        await viewModel.LoadItemsAsync();
        await viewModel.EditCommand.ExecuteAsync(null);
        TestEditorViewModel editor = Assert.IsType<TestEditorViewModel>(viewModel.CrudContext.Editor);
        editor.Name = null;

        await viewModel.SaveDetailCommand.ExecuteAsync(null);

        Assert.True(viewModel.CrudContext.IsOpen);
        Assert.True(editor.HasErrors);
        Assert.Equal(0, viewModel.UpdateCount);
    }

    /// <summary>
    /// Ensures that save updates an existing item and closes the detail context.
    /// </summary>
    [Fact]
    public async Task SaveDetailCommand_WithValidEdit_ShouldUpdateItemAndCloseDetail()
    {
        TestCrudPageViewModel viewModel = new();
        await viewModel.LoadItemsAsync();
        await viewModel.EditCommand.ExecuteAsync(null);
        TestEditorViewModel editor = Assert.IsType<TestEditorViewModel>(viewModel.CrudContext.Editor);
        editor.Name = "Alpha updated";

        await viewModel.SaveDetailCommand.ExecuteAsync(null);

        Assert.False(viewModel.CrudContext.IsOpen);
        Assert.Equal(1, viewModel.UpdateCount);
        Assert.Equal("Alpha updated", viewModel.Items[0].Name);
        Assert.False(editor.IsDirty);
    }

    /// <summary>
    /// Ensures that create adds a new item.
    /// </summary>
    [Fact]
    public async Task NewAndSaveDetailCommand_ShouldCreateItem()
    {
        TestCrudPageViewModel viewModel = new();
        await viewModel.LoadItemsAsync();

        await viewModel.NewCommand.ExecuteAsync(null);
        TestEditorViewModel editor = Assert.IsType<TestEditorViewModel>(viewModel.CrudContext.Editor);
        editor.Name = "Gamma";
        await viewModel.SaveDetailCommand.ExecuteAsync(null);

        Assert.False(viewModel.CrudContext.IsOpen);
        Assert.Equal(3, viewModel.Items.Count);
        Assert.Equal("Gamma", viewModel.Items[2].Name);
        Assert.Equal(1, viewModel.CreateCount);
    }

    /// <summary>
    /// Ensures that delete removes the selected item.
    /// </summary>
    [Fact]
    public async Task DeleteCommand_ShouldDeleteSelectedItem()
    {
        TestCrudPageViewModel viewModel = new();
        await viewModel.LoadItemsAsync();

        await viewModel.DeleteCommand.ExecuteAsync(null);

        Assert.Single(viewModel.Items);
        Assert.Equal("Beta", viewModel.SelectedItem?.Name);
        Assert.Equal(1, viewModel.DeleteCount);
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestCrudPageViewModel : XCrudEditorPageViewModelBase<TestEntity, TestEditorViewModel, int>
    {
        #region ### Fields ###
        private int nextId = 3;
        #endregion

        #region ### Public Properties ###
        public int CreateCount { get; private set; }

        public int UpdateCount { get; private set; }

        public int DeleteCount { get; private set; }
        #endregion

        #region ### Protected Methods ###
        protected override Task<IReadOnlyList<TestEntity>> LoadItemsCoreAsync(CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            IReadOnlyList<TestEntity> items =
            [
                new TestEntity(1, "Alpha"),
                new TestEntity(2, "Beta"),
            ];

            return Task.FromResult(items);
        }

        protected override int GetEntityKey(TestEntity entity)
        {
            return entity.Id;
        }

        protected override Task<TestEditorViewModel> CreateEditorForCreateAsync(CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            return Task.FromResult(new TestEditorViewModel());
        }

        protected override Task<TestEditorViewModel> CreateEditorForEntityAsync(TestEntity entity, XCrudMode mode, CancellationToken cancellationToken)
        {
            _ = mode;
            _ = cancellationToken;

            return Task.FromResult(new TestEditorViewModel(entity.Name));
        }

        protected override Task<TestEntity> CreateEntityAsync(TestEditorViewModel editor, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            this.CreateCount++;
            return Task.FromResult(new TestEntity(this.nextId++, editor.Name ?? string.Empty));
        }

        protected override Task<TestEntity> UpdateEntityAsync(TestEntity entity, TestEditorViewModel editor, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            this.UpdateCount++;
            return Task.FromResult(new TestEntity(entity.Id, editor.Name ?? string.Empty));
        }

        protected override Task DeleteEntityAsync(TestEntity entity, CancellationToken cancellationToken)
        {
            _ = entity;
            _ = cancellationToken;

            this.DeleteCount++;
            return Task.CompletedTask;
        }

        protected override string CreateDetailTitle(XCrudMode mode, TestEntity? entity, TestEditorViewModel editor)
        {
            _ = editor;
            return mode == XCrudMode.Create ? "Create" : $"{mode} {entity?.Name}";
        }
        #endregion
    }

    private sealed class TestEditorViewModel : XEditorViewModelBase
    {
        #region ### Fields ###
        private string? name;
        #endregion

        #region ### Constructors ###
        public TestEditorViewModel()
        {
        }

        public TestEditorViewModel(string? name)
        {
            this.Name = name;
            this.MarkClean();
        }
        #endregion

        #region ### Public Properties ###
        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        #endregion

        #region ### Protected Methods ###
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            context.Required(this.Name, nameof(this.Name), XValidationText.Text("Name is required."));
            return Task.CompletedTask;
        }
        #endregion
    }

    private sealed record TestEntity(int Id, string Name);
    #endregion
}
#endregion
