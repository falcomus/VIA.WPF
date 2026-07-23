// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XModernIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XModernIcon ###
/// <summary>
/// Represents a strongly typed Modern icon control of VIA.WPF.
/// </summary>
public class XModernIcon : XIconBase<PackIconModernKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconModernKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconModernKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconModernKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XModernIcon"/> class.
    /// </summary>
    static XModernIcon()
    {
        OverrideDefaultStyleKey<XModernIcon>();
    }
    #endregion
}
#endregion
