// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresets.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Themes;

#region ### Class XThemePresets ###
/// <summary>
/// Provides predefined <see cref="XTheme" /> instances.
/// </summary>
public static class XThemePresets
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the default VIA.WPF theme.
    /// </summary>
    public static XTheme Default { get; } = XThemePresetDefault.Create();

    /// <summary>
    /// Gets the Amber VIA.WPF theme.
    /// </summary>
    public static XTheme Amber { get; } = XThemePresetAmber.Create();

    /// <summary>
    /// Gets the Azure VIA.WPF theme.
    /// </summary>
    public static XTheme Azure { get; } = XThemePresetAzure.Create();

    /// <summary>
    /// Gets the Crimson VIA.WPF theme.
    /// </summary>
    public static XTheme Crimson { get; } = XThemePresetCrimson.Create();

    /// <summary>
    /// Gets the Emerald VIA.WPF theme.
    /// </summary>
    public static XTheme Emerald { get; } = XThemePresetEmerald.Create();

    /// <summary>
    /// Gets the Graphite VIA.WPF theme.
    /// </summary>
    public static XTheme Graphite { get; } = XThemePresetGraphite.Create();

    /// <summary>
    /// Gets the Indigo VIA.WPF theme.
    /// </summary>
    public static XTheme Indigo { get; } = XThemePresetIndigo.Create();

    /// <summary>
    /// Gets the Magenta VIA.WPF theme.
    /// </summary>
    public static XTheme Magenta { get; } = XThemePresetMagenta.Create();

    /// <summary>
    /// Gets the Rose VIA.WPF theme.
    /// </summary>
    public static XTheme Rose { get; } = XThemePresetRose.Create();

    /// <summary>
    /// Gets the Sandstone VIA.WPF theme.
    /// </summary>
    public static XTheme Sandstone { get; } = XThemePresetSandstone.Create();

    /// <summary>
    /// Gets the Teal VIA.WPF theme.
    /// </summary>
    public static XTheme Teal { get; } = XThemePresetTeal.Create();

    /// <summary>
    /// Gets the Violet VIA.WPF theme.
    /// </summary>
    public static XTheme Violet { get; } = XThemePresetViolet.Create();

    /// <summary>
    /// Gets the legacy Blossom VIA.WPF theme alias. Use <see cref="Magenta" /> instead.
    /// </summary>
    [Obsolete("Use Magenta instead.")]
    public static XTheme Blossom => Magenta;

    /// <summary>
    /// Gets the legacy MistyRose VIA.WPF theme alias. Use <see cref="Rose" /> instead.
    /// </summary>
    [Obsolete("Use Rose instead.")]
    public static XTheme MistyRose => Rose;

    /// <summary>
    /// Gets the legacy Ocean VIA.WPF theme alias. Use <see cref="Azure" /> instead.
    /// </summary>
    [Obsolete("Use Azure instead.")]
    public static XTheme Ocean => Azure;

    /// <summary>
    /// Gets the legacy PurpleDream VIA.WPF theme alias. Use <see cref="Violet" /> instead.
    /// </summary>
    [Obsolete("Use Violet instead.")]
    public static XTheme PurpleDream => Violet;

    /// <summary>
    /// Gets all built-in themes.
    /// </summary>
    public static IReadOnlyList<XTheme> BuiltInThemes { get; } =
    [
        Default,
        Amber,
        Azure,
        Crimson,
        Emerald,
        Graphite,
        Indigo,
        Magenta,
        Rose,
        Sandstone,
        Teal,
        Violet
    ];
    #endregion
}
#endregion
