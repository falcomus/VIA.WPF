// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationTabItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XNavigationTabItem ###
/// <summary>
/// Represents a tab item used inside an <see cref="XNavigationTabControl"/>.
/// </summary>
public class XNavigationTabItem : XTabItem
{
    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XNavigationTabItem"/> class.
    /// </summary>
    static XNavigationTabItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XNavigationTabItem),
            new FrameworkPropertyMetadata(typeof(XTabItem)));
    }

    #endregion
}
#endregion
