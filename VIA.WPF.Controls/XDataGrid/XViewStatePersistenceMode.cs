// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewStatePersistenceMode.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XViewStatePersistenceMode ###
/// <summary>
/// Defines how view state is persisted.
/// </summary>
public enum XViewStatePersistenceMode
{
    /// <summary>
    /// View state is not persisted.
    /// </summary>
    None,

    /// <summary>
    /// View state is kept in memory for the current application process.
    /// </summary>
    Memory,

    /// <summary>
    /// View state is persisted in a local JSON file.
    /// </summary>
    File
}
#endregion
