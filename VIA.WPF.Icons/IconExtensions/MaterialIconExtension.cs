// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class MaterialIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XMaterialIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(XMaterialIcon))]
public sealed class MaterialIconExtension : KindIconExtensionBase<XMaterialIcon, PackIconMaterialKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialIconExtension"/> class.
    /// </summary>
    public MaterialIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The material icon kind.</param>
    public MaterialIconExtension(PackIconMaterialKind kind)
        : base(kind)
    {
    }
    #endregion
}
#endregion
