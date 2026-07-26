// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontAwesomeIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;

namespace VIA.WPF.Icons;

#region ### Class FontAwesomeIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XFontAwesomeIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class FontAwesomeIconExtension : KindIconExtensionBase<XFontAwesomeIcon, PackIconFontAwesome6Kind>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="FontAwesomeIconExtension"/> class.
    /// </summary>
    public FontAwesomeIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontAwesomeIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The Font Awesome icon kind.</param>
    public FontAwesomeIconExtension(PackIconFontAwesome6Kind kind)
        : base(kind)
    {
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the Font Awesome icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public new PackIconFontAwesome6Kind? Kind
    {
        get => base.Kind;
        set => base.Kind = value;
    }
    #endregion
}
#endregion
