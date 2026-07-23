// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows.Markup;
using System.Windows.Media;

namespace VIA.WPF.Icons;

#region ### Class XIconExtension ###
/// <summary>
/// Creates an <see cref="XIcon"/> instance from inline XAML syntax.
/// </summary>
[MarkupExtensionReturnType(typeof(XIcon))]
public class XIconExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XIconExtension"/> class.
    /// </summary>
    public XIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The material icon kind.</param>
    public XIconExtension(PackIconMaterialDesignKind kind)
    {
        this.Kind = kind;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the material icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public PackIconMaterialDesignKind Kind { get; set; } = PackIconMaterialDesignKind.None;

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double Size { get; set; } = 16d;

    /// <summary>
    /// Gets or sets the icon foreground brush.
    /// </summary>
    public Brush? Foreground { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Provides the created <see cref="XIcon"/> instance.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The created <see cref="XIcon"/>.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        XIcon icon = new()
        {
            Kind = this.Kind,
            Size = this.Size
        };

        if (this.Foreground is not null)
        {
            icon.Foreground = this.Foreground;
        }

        return icon;
    }
    #endregion
}
#endregion