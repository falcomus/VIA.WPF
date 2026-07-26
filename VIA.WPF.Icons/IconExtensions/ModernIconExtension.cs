// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModernIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class ModernIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XModernIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class ModernIconExtension : KindIconExtensionBase<XModernIcon, PackIconModernKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="ModernIconExtension"/> class.
    /// </summary>
    public ModernIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModernIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The modern icon kind.</param>
    public ModernIconExtension(PackIconModernKind kind)
        : base(kind)
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the modern icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public new PackIconModernKind? Kind
    {
        get => base.Kind;
        set => base.Kind = value;
    }
    #endregion
}
#endregion
