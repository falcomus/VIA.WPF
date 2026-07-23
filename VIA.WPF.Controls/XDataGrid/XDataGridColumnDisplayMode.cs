// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridColumnDisplayMode.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XDataGridColumnDisplayMode ###
/// <summary>
/// Defines the active display mode used by an <see cref="XDataGrid" /> to show or hide configured columns.
/// </summary>
public enum XDataGridColumnDisplayMode
{
    /// <summary>
    /// Shows columns configured for the full display mode.
    /// </summary>
    Full,

    /// <summary>
    /// Shows columns configured for the compact display mode.
    /// </summary>
    Compact
}
#endregion

#region ### Enum XDataGridColumnDisplayModes ###
/// <summary>
/// Defines the display modes in which a data grid column is visible.
/// </summary>
[Flags]
public enum XDataGridColumnDisplayModes
{
    /// <summary>
    /// The column is hidden whenever a display mode is applied.
    /// </summary>
    None = 0,

    /// <summary>
    /// The column is visible in compact display mode.
    /// </summary>
    Compact = 1,

    /// <summary>
    /// The column is visible in full display mode.
    /// </summary>
    Full = 2,

    /// <summary>
    /// The column is visible in all display modes.
    /// </summary>
    All = Compact | Full
}
#endregion
