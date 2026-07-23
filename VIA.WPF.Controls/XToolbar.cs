// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToolbar.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XToolbar ###
/// <summary>
/// Provides backward compatibility for the former toolbar name. Use <see cref="XHeaderBar"/> for new views.
/// </summary>
[Obsolete("Use XHeaderBar instead.")]
public class XToolbar : XHeaderBar
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToolbar"/> class.
    /// </summary>
    static XToolbar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XToolbar), new FrameworkPropertyMetadata(typeof(XHeaderBar)));
    }
    #endregion
}
#endregion
