// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XFileIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XFileIcon ###
/// <summary>
/// Represents a strongly typed file icon control of VIA.WPF.
/// </summary>
public class XFileIcon : XIconBase<PackIconFileIconsKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconFileIconsKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconFileIconsKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconFileIconsKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XFileIcon"/> class.
    /// </summary>
    static XFileIcon()
    {
        OverrideDefaultStyleKey<XFileIcon>();
    }
    #endregion
}
#endregion
