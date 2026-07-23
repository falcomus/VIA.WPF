// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToolbarGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XToolbarGroup ###
/// <summary>
/// Provides backward compatibility for the former toolbar group name. Use <see cref="XHeaderBarGroup"/> for new views.
/// </summary>
[Obsolete("Use XHeaderBarGroup instead.")]
public class XToolbarGroup : XHeaderBarGroup
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToolbarGroup"/> class.
    /// </summary>
    static XToolbarGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XToolbarGroup), new FrameworkPropertyMetadata(typeof(XHeaderBarGroup)));
    }
    #endregion
}
#endregion
