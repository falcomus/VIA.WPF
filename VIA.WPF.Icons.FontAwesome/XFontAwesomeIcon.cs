// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XFontAwesomeIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XFontAwesomeIcon ###
/// <summary>
/// Represents a strongly typed Font Awesome icon control of VIA.WPF.
/// </summary>
public class XFontAwesomeIcon : XIconBase<PackIconFontAwesome6Kind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconFontAwesome6Kind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconFontAwesome6Kind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconFontAwesome6Kind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XFontAwesomeIcon"/> class.
    /// </summary>
    static XFontAwesomeIcon()
    {
        OverrideDefaultStyleKey<XFontAwesomeIcon>();
    }
    #endregion
}
#endregion
