// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCrudMode.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls.Navigation;

#region ### Enum XCrudMode ###
/// <summary>
/// Defines the operation mode of a CRUD detail area.
/// </summary>
public enum XCrudMode
{
    #region ### Values ###
    /// <summary>
    /// No CRUD operation is active.
    /// </summary>
    None = 0,

    /// <summary>
    /// An existing item is displayed read-only.
    /// </summary>
    View = 1,

    /// <summary>
    /// An existing item is edited.
    /// </summary>
    Edit = 2,

    /// <summary>
    /// A new item is created.
    /// </summary>
    Create = 3
    #endregion
}
#endregion
