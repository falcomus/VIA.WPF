// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class FileIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XFileIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class FileIconExtension : KindIconExtensionBase<XFileIcon, PackIconFileIconsKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="FileIconExtension"/> class.
    /// </summary>
    public FileIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The file icon kind.</param>
    public FileIconExtension(PackIconFileIconsKind kind)
        : base(kind)
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the file icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public new PackIconFileIconsKind? Kind
    {
        get => base.Kind;
        set => base.Kind = value;
    }
    #endregion
}
#endregion
