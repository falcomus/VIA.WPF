// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewNodeDropMode.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XTreeViewNodeDropMode ###
/// <summary>
/// Defines the drop positions that are allowed for tree node drag and drop.
/// </summary>
public enum XTreeViewNodeDropMode
{
    #region ### Values ###
    /// <summary>
    /// No node drop position is allowed.
    /// </summary>
    None,

    /// <summary>
    /// Only inserting before a target node is allowed.
    /// </summary>
    Before,

    /// <summary>
    /// Only inserting after a target node is allowed.
    /// </summary>
    After,

    /// <summary>
    /// Only inserting into a target node is allowed.
    /// </summary>
    Into,

    /// <summary>
    /// Inserting before or after a target node is allowed.
    /// </summary>
    BeforeAfter,

    /// <summary>
    /// Inserting before or into a target node is allowed.
    /// </summary>
    BeforeInto,

    /// <summary>
    /// Inserting after or into a target node is allowed.
    /// </summary>
    AfterInto,

    /// <summary>
    /// Inserting before, after or into a target node is allowed.
    /// </summary>
    BeforeAfterInto
    #endregion
}
#endregion
