// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBootstrapIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XBootstrapIcon ###
/// <summary>
/// Represents a strongly typed Bootstrap icon control of VIA.WPF.
/// </summary>
public class XBootstrapIcon : XIconBase<PackIconBootstrapIconsKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconBootstrapIconsKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconBootstrapIconsKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconBootstrapIconsKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XBootstrapIcon"/> class.
    /// </summary>
    static XBootstrapIcon()
    {
        OverrideDefaultStyleKey<XBootstrapIcon>();
    }
    #endregion
}
#endregion
