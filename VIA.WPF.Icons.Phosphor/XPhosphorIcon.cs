// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPhosphorIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XPhosphorIcon ###
/// <summary>
/// Represents a strongly typed Phosphor icon control of VIA.WPF.
/// </summary>
public class XPhosphorIcon : XIconBase<PackIconPhosphorIconsKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconPhosphorIconsKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconPhosphorIconsKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconPhosphorIconsKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XPhosphorIcon"/> class.
    /// </summary>
    static XPhosphorIcon()
    {
        OverrideDefaultStyleKey<XPhosphorIcon>();
    }
    #endregion
}
#endregion
