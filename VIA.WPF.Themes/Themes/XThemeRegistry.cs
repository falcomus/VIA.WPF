// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeRegistry.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Themes;

#region ### Class XThemeRegistry ###
/// <summary>
/// Provides a registry for all available VIA.WPF themes.
/// </summary>
public sealed class XThemeRegistry
{
    #region ### Private Fields ###
    private readonly ObservableCollection<XTheme> _themes = [];
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XThemeRegistry"/> class.
    /// </summary>
    private XThemeRegistry()
    {
        this.Themes = new ReadOnlyObservableCollection<XTheme>(this._themes);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the global theme registry instance.
    /// </summary>
    public static XThemeRegistry Current { get; } = new();

    /// <summary>
    /// Gets the registered themes.
    /// </summary>
    public ReadOnlyObservableCollection<XTheme> Themes { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Registers the specified theme if it is not already present.
    /// </summary>
    /// <param name="theme">The theme to register.</param>
    public void Register(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        if (this.Contains(theme.Name))
        {
            return;
        }

        this._themes.Add(theme);
    }

    /// <summary>Registers a theme, replacing an already registered theme with the same name.</summary>
    public void RegisterOrReplace(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        int index = this._themes
            .Select((item, position) => new { item, position })
            .FirstOrDefault(entry => string.Equals(entry.item.Name, theme.Name, StringComparison.OrdinalIgnoreCase))?.position ?? -1;

        if (index < 0)
        {
            this._themes.Add(theme);
            return;
        }

        this._themes[index] = theme;
    }

    /// <summary>
    /// Registers the specified themes if they are not already present.
    /// </summary>
    /// <param name="themes">The themes to register.</param>
    public void RegisterRange(IEnumerable<XTheme> themes)
    {
        ArgumentNullException.ThrowIfNull(themes);

        foreach (XTheme theme in themes)
        {
            this.Register(theme);
        }
    }

    /// <summary>
    /// Gets the theme with the specified name.
    /// </summary>
    /// <param name="name">The theme name.</param>
    /// <returns>The matching theme, or <see langword="null"/> if not found.</returns>
    public XTheme? GetByName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return this._themes.FirstOrDefault(theme =>
            string.Equals(theme.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Determines whether a theme with the specified name exists.
    /// </summary>
    /// <param name="name">The theme name.</param>
    /// <returns><see langword="true"/> if the theme exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return this._themes.Any(theme =>
            string.Equals(theme.Name, name, StringComparison.OrdinalIgnoreCase));
    }
    #endregion
}
#endregion
