// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialDesignIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class MaterialDesignIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XMaterialDesignIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class MaterialDesignIconExtension : KindIconExtensionBase<XMaterialDesignIcon, PackIconMaterialDesignKind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialDesignIconExtension"/> class.
    /// </summary>
    public MaterialDesignIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialDesignIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The material design icon kind.</param>
    public MaterialDesignIconExtension(PackIconMaterialDesignKind kind)
        : base(kind)
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the material design icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public new PackIconMaterialDesignKind? Kind
    {
        get => base.Kind;
        set => base.Kind = value;
    }
    #endregion
}
#endregion
