// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewDetailPresentation.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XViewDetailPresentation ###
/// <summary>
/// Defines how the detail area of an <see cref="XViewContainer"/> is presented.
/// </summary>
public enum XViewDetailPresentation
{
    #region ### Values ###
    /// <summary>
    /// Shows the detail area as a centered modal dialog inside the view container.
    /// </summary>
    Dialog,

    /// <summary>
    /// Shows the detail area as a flyout inside the view container.
    /// </summary>
    Flyout
    #endregion
}
#endregion
