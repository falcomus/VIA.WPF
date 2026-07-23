// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMaterialDesignIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;

namespace VIA.WPF.Icons;

#region ### Class XMaterialDesignIcon ###
/// <summary>
/// Represents a strongly typed Material Design icon control of VIA.WPF.
/// </summary>
public class XMaterialDesignIcon : XIconBase<PackIconMaterialDesignKind>
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty KindProperty = XIconBase<PackIconMaterialDesignKind>.KindProperty;

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty SizeProperty = XIconBase<PackIconMaterialDesignKind>.SizeProperty;

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty StretchProperty = XIconBase<PackIconMaterialDesignKind>.StretchProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XMaterialDesignIcon"/> class.
    /// </summary>
    static XMaterialDesignIcon()
    {
        OverrideDefaultStyleKey<XMaterialDesignIcon>();
    }
    #endregion
}
#endregion
