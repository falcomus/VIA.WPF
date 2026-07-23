// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMaterialIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XMaterialIcon ###
/// <summary>
/// Represents a strongly typed Material icon control of VIA.WPF.
/// </summary>
public class XMaterialIcon : XIconBase<PackIconMaterialKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconMaterialKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconMaterialKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconMaterialKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XMaterialIcon"/> class.
    /// </summary>
    static XMaterialIcon()
    {
        OverrideDefaultStyleKey<XMaterialIcon>();
    }
    #endregion
}
#endregion
