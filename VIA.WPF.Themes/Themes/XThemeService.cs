// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Themes;

#region ### Class XThemeService ###
/// <summary>
/// Provides simplified access to VIA.WPF's theming system.
/// </summary>
public static class XThemeService
{
    #region ### Private Fields ###
    private static bool _builtInThemesRegistered;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the global <see cref="XThemeManager"/>.
    /// </summary>
    public static XThemeManager Manager => XThemeManager.Current;

    /// <summary>
    /// Gets the global theme registry.
    /// </summary>
    public static XThemeRegistry Registry => XThemeRegistry.Current;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Initializes VIA.WPF with the default theme.
    /// </summary>
    public static void Initialize()
    {
        EnsureBuiltInThemesRegistered();

        XThemeManager.Current.ApplyTheme(XThemePresets.Default);
        XThemeManager.Current.SetMode(XThemeMode.Light);
    }

    /// <summary>
    /// Initializes VIA.WPF with the specified theme and mode.
    /// </summary>
    /// <param name="theme">The theme to apply.</param>
    /// <param name="mode">The desired display mode.</param>
    public static void Initialize(XTheme theme, XThemeMode mode = XThemeMode.Light)
    {
        ArgumentNullException.ThrowIfNull(theme);

        EnsureBuiltInThemesRegistered();
        Registry.Register(theme);

        XThemeManager.Current.ApplyTheme(theme);
        XThemeManager.Current.SetMode(mode);
    }

    /// <summary>
    /// Sets the current theme mode.
    /// </summary>
    /// <param name="mode">The desired display mode.</param>
    public static void ChangeThemeMode(XThemeMode mode = XThemeMode.Light)
    {
        XThemeManager.Current.SetMode(mode);
    }

    /// <summary>
    /// Applies the specified theme.
    /// </summary>
    /// <param name="theme">The theme to apply.</param>
    public static void ChangeTheme(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        EnsureBuiltInThemesRegistered();
        Registry.Register(theme);

        XThemeManager.Current.ApplyTheme(theme);
    }

    /// <summary>Registers or updates a theme and applies the new definition immediately.</summary>
    public static void ApplyOrUpdateTheme(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        EnsureBuiltInThemesRegistered();
        Registry.RegisterOrReplace(theme);
        XThemeManager.Current.ApplyTheme(theme);
    }

    /// <summary>
    /// Applies the theme with the specified name.
    /// </summary>
    /// <param name="themeName">The theme name.</param>
    /// <returns><see langword="true"/> if a matching theme was applied; otherwise <see langword="false"/>.</returns>
    public static bool ChangeTheme(string themeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(themeName);

        EnsureBuiltInThemesRegistered();

        XTheme? theme = Registry.GetByName(themeName);

        if (theme is null)
        {
            return false;
        }

        XThemeManager.Current.ApplyTheme(theme);
        return true;
    }

    /// <summary>
    /// Registers the built-in themes once.
    /// </summary>
    public static void EnsureBuiltInThemesRegistered()
    {
        if (_builtInThemesRegistered)
        {
            return;
        }

        Registry.RegisterRange(XThemePresets.BuiltInThemes);
        _builtInThemesRegistered = true;
    }
    #endregion
}
#endregion
