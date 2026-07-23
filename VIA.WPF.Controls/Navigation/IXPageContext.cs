// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXPageContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls.Navigation;

#region ### Interface IXPageContext ###
/// <summary>
/// Describes a page that can be hosted by a shell and can expose toolbar capabilities.
/// </summary>
public interface IXPageContext
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the displayed page title.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the displayed page description.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the toolbar context exposed by the page.
    /// </summary>
    XToolbarContext Toolbar { get; }
    #endregion
}
#endregion
