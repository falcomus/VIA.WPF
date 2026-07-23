// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BootstrapIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class BootstrapIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XBootstrapIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(XBootstrapIcon))]
public sealed class BootstrapIconExtension : KindIconExtensionBase<XBootstrapIcon, PackIconBootstrapIconsKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="BootstrapIconExtension"/> class.
    /// </summary>
    public BootstrapIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BootstrapIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The bootstrap icon kind.</param>
    public BootstrapIconExtension(PackIconBootstrapIconsKind kind)
        : base(kind)
    {
    }
    #endregion
}
#endregion
