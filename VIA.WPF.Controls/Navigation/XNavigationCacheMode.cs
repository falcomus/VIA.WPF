// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationCacheMode.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls.Navigation;

#region ### Enum XNavigationCacheMode ###
/// <summary>
/// Defines how navigation runtime objects are reused.
/// </summary>
public enum XNavigationCacheMode
{
    #region ### Values ###
    /// <summary>
    /// The navigation object is created each time it is requested.
    /// </summary>
    None,

    /// <summary>
    /// The navigation object is created once for its route and then reused.
    /// </summary>
    PerRoute
    #endregion
}
#endregion
