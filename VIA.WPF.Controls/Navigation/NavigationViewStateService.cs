// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationViewStateService.cs" >
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Text.Json;

namespace VIA.WPF.Navigation;

#region ### Class NavigationViewStateService ###
/// <summary>
/// Stores user preferences for navigation view state behavior.
/// </summary>
public sealed class NavigationViewStateService
{
    #region ### Private Fields ###
    private readonly object syncRoot = new();
    private Dictionary<string, bool> rememberViewStates = [];
    private readonly string storageFilePath;
    private bool isLoaded;
    #endregion

    #region ### Constructors ###
    private NavigationViewStateService()
    {
        this.storageFilePath = CreateDefaultStorageFilePath();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the global navigation view state service instance.
    /// </summary>
    public static NavigationViewStateService Current { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates the preference key for a navigation page.
    /// </summary>
    /// <param name="sectionValue">The owning section value.</param>
    /// <param name="pageValue">The page value.</param>
    /// <returns>The preference key.</returns>
    public static string CreatePageKey(object? sectionValue, object? pageValue)
    {
        return $"Page|{ToKeyPart(sectionValue)}|{ToKeyPart(pageValue)}";
    }

    /// <summary>
    /// Creates the preference key for a navigation workspace.
    /// </summary>
    /// <param name="sectionValue">The owning section value.</param>
    /// <returns>The preference key.</returns>
    public static string CreateWorkspaceKey(object? sectionValue)
    {
        return $"Workspace|{ToKeyPart(sectionValue)}";
    }

    /// <summary>
    /// Gets the stored remember-view-state flag or returns the specified default value.
    /// </summary>
    /// <param name="key">The preference key.</param>
    /// <param name="defaultValue">The default value used when no preference exists.</param>
    /// <returns>The stored value or the default value.</returns>
    public bool GetRememberViewStateOrDefault(string key, bool defaultValue)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return defaultValue;
        }

        lock (this.syncRoot)
        {
            this.EnsureLoaded();
            return this.rememberViewStates.TryGetValue(key, out bool value)
                ? value
                : defaultValue;
        }
    }

    /// <summary>
    /// Stores the remember-view-state flag for the specified key.
    /// </summary>
    /// <param name="key">The preference key.</param>
    /// <param name="rememberViewState">The remember-view-state flag.</param>
    public void SetRememberViewState(string key, bool rememberViewState)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        lock (this.syncRoot)
        {
            this.EnsureLoaded();
            this.rememberViewStates[key] = rememberViewState;
            this.Save();
        }
    }
    #endregion

    #region ### Private Methods ###
    private static string ToKeyPart(object? value)
    {
        return value?.ToString() ?? "None";
    }

    private static string CreateDefaultStorageFilePath()
    {
        string appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Application";
        string directoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appName,
            "VIA.WPF",
            "ViewState");

        return Path.Combine(directoryPath, "NavigationPreferences.json");
    }

    private void EnsureLoaded()
    {
        if (this.isLoaded)
        {
            return;
        }

        this.isLoaded = true;

        if (!File.Exists(this.storageFilePath))
        {
            this.rememberViewStates = [];
            return;
        }

        try
        {
            string json = File.ReadAllText(this.storageFilePath);
            this.rememberViewStates = JsonSerializer.Deserialize<Dictionary<string, bool>>(json) ?? [];
        }
        catch
        {
            this.rememberViewStates = [];
        }
    }

    private void Save()
    {
        try
        {
            string? directoryPath = Path.GetDirectoryName(this.storageFilePath);
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(this.rememberViewStates, options);
            File.WriteAllText(this.storageFilePath, json);
        }
        catch
        {
            // View state preferences must never break the application.
        }
    }
    #endregion
}
#endregion
