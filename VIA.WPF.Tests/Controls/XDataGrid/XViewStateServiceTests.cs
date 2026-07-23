// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewStateServiceTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.IO;
using System.Text.Json;
using VIA.WPF.Controls;

namespace VIA.WPF.Tests.Controls.DataGrid;

#region ### Class XViewStateServiceTests ###
/// <summary>
/// Provides tests for the global XDataGrid view-state persistence service.
/// </summary>
public sealed class XViewStateServiceTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Verifies that memory mode can save and load a data grid view state.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_InMemoryMode_ShouldStoreState()
    {
        using ViewStateServiceTestContext context = new();
        XDataGridViewState state = CreateSampleState("memory");

        context.Service.PersistenceMode = XViewStatePersistenceMode.Memory;
        await context.Service.SaveDataGridStateAsync("grid", state);

        XDataGridViewState? restoredState = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.NotNull(restoredState);
        AssertEquivalentState(state, restoredState);
    }

    /// <summary>
    /// Verifies that remove deletes an in-memory state.
    /// </summary>
    [Fact]
    public async Task RemoveDataGridStateAsync_InMemoryMode_ShouldRemoveState()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.PersistenceMode = XViewStatePersistenceMode.Memory;
        await context.Service.SaveDataGridStateAsync("grid", CreateSampleState("memory"));

        await context.Service.RemoveDataGridStateAsync("grid");

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that clear removes all in-memory states.
    /// </summary>
    [Fact]
    public async Task ClearAsync_InMemoryMode_ShouldRemoveAllStates()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.PersistenceMode = XViewStatePersistenceMode.Memory;
        await context.Service.SaveDataGridStateAsync("grid-a", CreateSampleState("a"));
        await context.Service.SaveDataGridStateAsync("grid-b", CreateSampleState("b"));

        await context.Service.ClearAsync();

        Assert.Null(await context.Service.TryLoadDataGridStateAsync("grid-a"));
        Assert.Null(await context.Service.TryLoadDataGridStateAsync("grid-b"));
    }

    /// <summary>
    /// Verifies that disabled persistence ignores save requests.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_WhenPersistenceIsDisabled_ShouldIgnoreState()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.PersistenceMode = XViewStatePersistenceMode.None;

        await context.Service.SaveDataGridStateAsync("grid", CreateSampleState("none"));

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that invalid keys are ignored.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task TryLoadDataGridStateAsync_WithInvalidKey_ShouldReturnNull(string? key)
    {
        using ViewStateServiceTestContext context = new();
        context.Service.PersistenceMode = XViewStatePersistenceMode.Memory;

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync(key!);

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that file mode persists a state to disk and loads it again after resetting the service cache.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_InFileMode_ShouldPersistAndReloadState()
    {
        using ViewStateServiceTestContext context = new();
        string storageFilePath = context.CreateStorageFilePath("view-states.json");
        XDataGridViewState state = CreateSampleState("file");

        context.Service.StorageFilePath = storageFilePath;
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;
        await context.Service.SaveDataGridStateAsync("grid", state);
        await ResetFileCacheWithoutDeletingFileAsync(context.Service);

        XDataGridViewState? restoredState = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.True(File.Exists(storageFilePath));
        Assert.NotNull(restoredState);
        AssertEquivalentState(state, restoredState);
    }

    /// <summary>
    /// Verifies that file mode can asynchronously persist and reload a state.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_InFileMode_ShouldPersistAndReloadStateWithoutSyncApi()
    {
        using ViewStateServiceTestContext context = new();
        string storageFilePath = context.CreateStorageFilePath("async-view-states.json");
        XDataGridViewState state = CreateSampleState("async-file");

        context.Service.StorageFilePath = storageFilePath;
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;
        await context.Service.SaveDataGridStateAsync("grid", state);
        await ResetFileCacheWithoutDeletingFileAsync(context.Service);

        XDataGridViewState? restoredState = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.True(File.Exists(storageFilePath));
        Assert.NotNull(restoredState);
        AssertEquivalentState(state, restoredState);
    }

    /// <summary>
    /// Verifies that file mode stores multiple independent states.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_InFileMode_ShouldPersistMultipleStates()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.StorageFilePath = context.CreateStorageFilePath("multi-states.json");
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;
        XDataGridViewState stateA = CreateSampleState("a");
        XDataGridViewState stateB = CreateSampleState("b");

        await context.Service.SaveDataGridStateAsync("grid-a", stateA);
        await context.Service.SaveDataGridStateAsync("grid-b", stateB);
        await ResetFileCacheWithoutDeletingFileAsync(context.Service);

        XDataGridViewState? restoredStateA = await context.Service.TryLoadDataGridStateAsync("grid-a");
        XDataGridViewState? restoredStateB = await context.Service.TryLoadDataGridStateAsync("grid-b");

        Assert.NotNull(restoredStateA);
        Assert.NotNull(restoredStateB);
        AssertEquivalentState(stateA, restoredStateA);
        AssertEquivalentState(stateB, restoredStateB);
    }

    /// <summary>
    /// Verifies that removing a file-backed state updates the persisted file.
    /// </summary>
    [Fact]
    public async Task RemoveDataGridStateAsync_InFileMode_ShouldPersistRemoval()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.StorageFilePath = context.CreateStorageFilePath("remove-state.json");
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;
        await context.Service.SaveDataGridStateAsync("grid", CreateSampleState("remove"));

        await context.Service.RemoveDataGridStateAsync("grid");
        await ResetFileCacheWithoutDeletingFileAsync(context.Service);

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that clearing in file mode updates the persisted file.
    /// </summary>
    [Fact]
    public async Task ClearAsync_InFileMode_ShouldPersistEmptyStateSet()
    {
        using ViewStateServiceTestContext context = new();
        context.Service.StorageFilePath = context.CreateStorageFilePath("clear-state.json");
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;
        await context.Service.SaveDataGridStateAsync("grid", CreateSampleState("clear"));

        await context.Service.ClearAsync();
        await ResetFileCacheWithoutDeletingFileAsync(context.Service);

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that corrupt persisted JSON is ignored instead of throwing.
    /// </summary>
    [Fact]
    public async Task TryLoadDataGridStateAsync_WithCorruptFile_ShouldReturnNull()
    {
        using ViewStateServiceTestContext context = new();
        string storageFilePath = context.CreateStorageFilePath("corrupt-state.json");
        await File.WriteAllTextAsync(storageFilePath, "{ this is not valid json");
        context.Service.StorageFilePath = storageFilePath;
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;

        XDataGridViewState? state = await context.Service.TryLoadDataGridStateAsync("grid");

        Assert.Null(state);
    }

    /// <summary>
    /// Verifies that the persisted file has the expected serializable shape.
    /// </summary>
    [Fact]
    public async Task SaveDataGridStateAsync_InFileMode_ShouldWriteDictionaryJson()
    {
        using ViewStateServiceTestContext context = new();
        string storageFilePath = context.CreateStorageFilePath("shape-state.json");
        context.Service.StorageFilePath = storageFilePath;
        context.Service.PersistenceMode = XViewStatePersistenceMode.File;

        await context.Service.SaveDataGridStateAsync("grid", CreateSampleState("shape"));

        string json = await File.ReadAllTextAsync(storageFilePath);
        Dictionary<string, XDataGridViewState>? states = JsonSerializer.Deserialize<Dictionary<string, XDataGridViewState>>(json);

        Assert.NotNull(states);
        Assert.True(states.ContainsKey("grid"));
        Assert.Equal("shape", states["grid"].SearchTerm);
    }
    #endregion

    #region ### Private Methods ###
    private static XDataGridViewState CreateSampleState(string searchTerm)
    {
        XDataGridViewState state = new()
        {
            SearchTerm = searchTerm
        };
        state.SortDescriptions.Add(new XDataGridSortState("Name", ListSortDirection.Ascending));
        state.SortDescriptions.Add(new XDataGridSortState("Status", ListSortDirection.Descending));
        state.ColumnFilters["Status"] = ["Open", "Closed"];
        state.ColumnFilters["Owner"] = ["Claus"];
        return state;
    }

    private static void AssertEquivalentState(XDataGridViewState expected, XDataGridViewState actual)
    {
        Assert.Equal(expected.SearchTerm, actual.SearchTerm);
        Assert.Equal(expected.SortDescriptions.Count, actual.SortDescriptions.Count);

        for (int index = 0; index < expected.SortDescriptions.Count; index++)
        {
            Assert.Equal(expected.SortDescriptions[index].PropertyName, actual.SortDescriptions[index].PropertyName);
            Assert.Equal(expected.SortDescriptions[index].Direction, actual.SortDescriptions[index].Direction);
        }

        Assert.Equal(expected.ColumnFilters.Keys.Order(StringComparer.Ordinal), actual.ColumnFilters.Keys.Order(StringComparer.Ordinal));

        foreach (string key in expected.ColumnFilters.Keys)
        {
            Assert.Equal(expected.ColumnFilters[key], actual.ColumnFilters[key]);
        }
    }

    private static async Task ResetFileCacheWithoutDeletingFileAsync(XViewStateService service)
    {
        service.PersistenceMode = XViewStatePersistenceMode.Memory;
        await service.ClearAsync();
        service.PersistenceMode = XViewStatePersistenceMode.File;
    }
    #endregion

    #region ### Class ViewStateServiceTestContext ###
    private sealed class ViewStateServiceTestContext : IDisposable
    {
        #region ### Private Fields ###
        private readonly XViewStatePersistenceMode originalPersistenceMode;
        private readonly string originalStorageFilePath;
        private readonly string temporaryDirectoryPath;
        #endregion

        #region ### Constructors ###
        public ViewStateServiceTestContext()
        {
            this.Service = XViewStateService.Current;
            this.originalPersistenceMode = this.Service.PersistenceMode;
            this.originalStorageFilePath = this.Service.StorageFilePath;
            this.temporaryDirectoryPath = Path.Combine(Path.GetTempPath(), "VIA.WPF.Tests", "ViewState", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(this.temporaryDirectoryPath);

            this.Service.PersistenceMode = XViewStatePersistenceMode.Memory;
            this.Service.ClearAsync().GetAwaiter().GetResult();
        }
        #endregion

        #region ### Public Properties ###
        public XViewStateService Service { get; }
        #endregion

        #region ### Public Methods ###
        public string CreateStorageFilePath(string fileName)
        {
            return Path.Combine(this.temporaryDirectoryPath, fileName);
        }

        public void Dispose()
        {
            try
            {
                this.Service.PersistenceMode = XViewStatePersistenceMode.Memory;
                this.Service.ClearAsync().GetAwaiter().GetResult();
                this.Service.StorageFilePath = this.originalStorageFilePath;
                this.Service.PersistenceMode = this.originalPersistenceMode;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(this.temporaryDirectoryPath))
                    {
                        Directory.Delete(this.temporaryDirectoryPath, true);
                    }
                }
                catch (IOException)
                {
                    // Test cleanup must not hide the actual test result.
                }
                catch (UnauthorizedAccessException)
                {
                    // Test cleanup must not hide the actual test result.
                }
            }
        }
        #endregion
    }
    #endregion
}
#endregion

