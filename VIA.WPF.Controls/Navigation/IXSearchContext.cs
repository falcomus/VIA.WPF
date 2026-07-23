// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXSearchContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Controls.Navigation;

#region ### Interface IXSearchContext ###
/// <summary>
/// Provides a reusable search context for pages that expose a global search field in the shell toolbar.
/// </summary>
public interface IXSearchContext
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the current search term.
    /// </summary>
    string SearchTerm { get; set; }

    /// <summary>
    /// Gets the command that resets the current search term.
    /// </summary>
    ICommand ResetSearchCommand { get; }
    #endregion
}
#endregion
