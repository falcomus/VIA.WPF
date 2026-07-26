// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhosphorIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class PhosphorIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XPhosphorIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(XPhosphorIcon))]
public sealed class PhosphorIconExtension : KindIconExtensionBase<XPhosphorIcon, PackIconPhosphorIconsKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="PhosphorIconExtension"/> class.
    /// </summary>
    public PhosphorIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhosphorIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The phosphor icon kind.</param>
    public PhosphorIconExtension(PackIconPhosphorIconsKind kind)
        : base(kind)
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the phosphor icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public new PackIconPhosphorIconsKind? Kind
    {
        get => base.Kind;
        set => base.Kind = value;
    }
    #endregion
}
#endregion
