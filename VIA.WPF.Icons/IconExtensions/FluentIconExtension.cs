// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentIconExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;
using System.Windows.Media;
using FluentIconKind = FluentIcons.Common.Icon;
using FluentIconSize = FluentIcons.Common.IconSize;
using FluentIconVariant = FluentIcons.Common.IconVariant;

namespace VIA.WPF.Icons;

#region ### Class FluentIconExtension ###
/// <summary>
/// Provides a strongly typed markup extension for creating <see cref="XFluentIcon"/> instances.
/// </summary>
[MarkupExtensionReturnType(typeof(XFluentIcon))]
public sealed class FluentIconExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentIconExtension"/> class.
    /// </summary>
    public FluentIconExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentIconExtension"/> class.
    /// </summary>
    /// <param name="kind">The fluent icon kind.</param>
    public FluentIconExtension(FluentIconKind kind)
    {
        this.Kind = kind;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the strongly typed fluent icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public FluentIconKind? Kind { get; set; }

    /// <summary>
    /// Gets or sets the strongly typed fluent icon variant.
    /// </summary>
    public FluentIconVariant? IconVariant { get; set; }

    /// <summary>
    /// Gets or sets the strongly typed fluent icon size preset.
    /// </summary>
    public FluentIconSize? IconSize { get; set; }

    /// <summary>
    /// Gets or sets the rendered size.
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Gets or sets the stretch mode.
    /// </summary>
    public Stretch? Stretch { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Returns the created icon instance.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The created icon instance.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        XFluentIcon icon = new();

        if (this.Kind.HasValue)
        {
            icon.Icon = this.Kind.Value;
        }

        if (this.IconVariant.HasValue)
        {
            icon.IconVariant = this.IconVariant.Value;
        }

        if (this.IconSize.HasValue)
        {
            icon.IconSize = this.IconSize.Value;
        }

        if (this.Size.HasValue)
        {
            icon.Size = this.Size.Value;
        }

        if (this.Stretch.HasValue)
        {
            icon.Stretch = this.Stretch.Value;
        }

        return icon;
    }
    #endregion
}
#endregion
