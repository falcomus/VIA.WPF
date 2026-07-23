// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXCrudPageContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Controls.Navigation;

#region ### Interface IXCrudPageContext ###
/// <summary>
/// Exposes the standard CRUD infrastructure consumed automatically by <see cref="VIA.WPF.Controls.XViewContainer" />.
/// </summary>
public interface IXCrudPageContext
{
    #region ### Properties ###
    /// <summary>
    /// Gets the CRUD context used by the page-local detail dialog.
    /// </summary>
    XCrudContext CrudContext { get; }

    /// <summary>
    /// Gets the command that saves the active detail editor.
    /// </summary>
    ICommand? SaveDetailCommand { get; }
    #endregion
}
#endregion
