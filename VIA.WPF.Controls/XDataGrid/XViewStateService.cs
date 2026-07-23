// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewStateService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace VIA.WPF.Controls;

#region ### Class XViewStateService ###
/// <summary>
/// Provides global persistence for reusable view states.
/// </summary>
public sealed class XViewStateService
{
    #region ### Private Fields ###
    private readonly SemaphoreSlim syncSemaphore = new(1, 1);
    private Dictionary<string, XDataGridViewState> dataGridStates = [];
    private string storageFilePath;
    private XViewStatePersistenceMode persistenceMode = XViewStatePersistenceMode.File;
    private bool isLoaded;
    #endregion

    #region ### Constructors ###
    private XViewStateService()
    {
        this.storageFilePath = CreateDefaultStorageFilePath();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the global view state service instance.
    /// </summary>
    public static XViewStateService Current { get; } = new();

    /// <summary>
    /// Gets or sets the persistence mode.
    /// </summary>
    public XViewStatePersistenceMode PersistenceMode
    {
        get
        {
            this.syncSemaphore.Wait();

            try
            {
                return this.persistenceMode;
            }
            finally
            {
                this.syncSemaphore.Release();
            }
        }
        set
        {
            this.syncSemaphore.Wait();

            try
            {
                if (this.persistenceMode == value)
                {
                    return;
                }

                this.persistenceMode = value;

                if (value == XViewStatePersistenceMode.File)
                {
                    this.dataGridStates = [];
                    this.isLoaded = false;
                }
            }
            finally
            {
                this.syncSemaphore.Release();
            }
        }
    }

    /// <summary>
    /// Gets or sets the JSON storage file path used when <see cref="PersistenceMode"/> is <see cref="XViewStatePersistenceMode.File"/>.
    /// </summary>
    public string StorageFilePath
    {
        get
        {
            this.syncSemaphore.Wait();

            try
            {
                return this.storageFilePath;
            }
            finally
            {
                this.syncSemaphore.Release();
            }
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            this.syncSemaphore.Wait();

            try
            {
                if (string.Equals(this.storageFilePath, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                this.storageFilePath = value;
                this.dataGridStates = [];
                this.isLoaded = false;
            }
            finally
            {
                this.syncSemaphore.Release();
            }
        }
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Tries to load a data grid view state asynchronously.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The loaded state if one exists; otherwise, <c>null</c>.</returns>
    public async Task<XDataGridViewState?> TryLoadDataGridStateAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        await this.syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (this.persistenceMode == XViewStatePersistenceMode.None)
            {
                return null;
            }

            await this.EnsureLoadedAsync(cancellationToken).ConfigureAwait(false);
            return this.dataGridStates.TryGetValue(key, out XDataGridViewState? state)
                ? state
                : null;
        }
        finally
        {
            this.syncSemaphore.Release();
        }
    }

    /// <summary>
    /// Saves a data grid view state asynchronously.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="state">The state to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveDataGridStateAsync(string key, XDataGridViewState state, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key) || state is null)
        {
            return;
        }

        await this.syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (this.persistenceMode == XViewStatePersistenceMode.None)
            {
                return;
            }

            await this.EnsureLoadedAsync(cancellationToken).ConfigureAwait(false);
            this.dataGridStates[key] = state;

            if (this.persistenceMode == XViewStatePersistenceMode.File)
            {
                await this.SaveFileAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            this.syncSemaphore.Release();
        }
    }

    /// <summary>
    /// Removes a data grid view state asynchronously.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous remove operation.</returns>
    public async Task RemoveDataGridStateAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        await this.syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (this.persistenceMode == XViewStatePersistenceMode.None)
            {
                return;
            }

            await this.EnsureLoadedAsync(cancellationToken).ConfigureAwait(false);

            if (!this.dataGridStates.Remove(key))
            {
                return;
            }

            if (this.persistenceMode == XViewStatePersistenceMode.File)
            {
                await this.SaveFileAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            this.syncSemaphore.Release();
        }
    }

    /// <summary>
    /// Clears all stored view states asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous clear operation.</returns>
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await this.syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            this.dataGridStates.Clear();
            this.isLoaded = true;

            if (this.persistenceMode == XViewStatePersistenceMode.File)
            {
                await this.SaveFileAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            this.syncSemaphore.Release();
        }
    }
    #endregion

    #region ### Private Methods ###
    private static string CreateDefaultStorageFilePath()
    {
        string appName = Assembly.GetEntryAssembly()?.GetName().Name
            ?? AppDomain.CurrentDomain.FriendlyName
            ?? "VIAApplication";

        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            appName = appName.Replace(invalidChar, '_');
        }

        string localApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localApplicationDataPath, appName, "VIA.WPF", "ViewState", "XDataGridViewStates.json");
    }

    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        if (this.isLoaded || this.persistenceMode != XViewStatePersistenceMode.File)
        {
            this.isLoaded = true;
            return;
        }

        this.dataGridStates = await this.LoadFileAsync(cancellationToken).ConfigureAwait(false);
        this.isLoaded = true;
    }

    private async Task<Dictionary<string, XDataGridViewState>> LoadFileAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(this.storageFilePath))
            {
                return [];
            }

            await using FileStream stream = new(
                this.storageFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                4096,
                FileOptions.Asynchronous | FileOptions.SequentialScan);

            return await JsonSerializer.DeserializeAsync<Dictionary<string, XDataGridViewState>>(
                    stream,
                    CreateJsonSerializerOptions(),
                    cancellationToken)
                .ConfigureAwait(false) ?? [];
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            LogPersistenceError("load", exception);
            return [];
        }
    }

    private async Task SaveFileAsync(CancellationToken cancellationToken)
    {
        try
        {
            string? directoryName = Path.GetDirectoryName(this.storageFilePath);
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            await using FileStream stream = new(
                this.storageFilePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read,
                4096,
                FileOptions.Asynchronous);

            await JsonSerializer.SerializeAsync(
                    stream,
                    this.dataGridStates,
                    CreateJsonSerializerOptions(),
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            LogPersistenceError("save", exception);
        }
    }

    private static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    private static void LogPersistenceError(string operationName, Exception exception)
    {
        Debug.WriteLine($"XViewStateService failed to {operationName} persisted view state: {exception}");
    }
    #endregion
}
#endregion
