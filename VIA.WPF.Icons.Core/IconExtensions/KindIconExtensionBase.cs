// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KindIconExtensionBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace VIA.WPF.Icons;

#region ### Class KindIconExtensionBase ###
/// <summary>
/// Provides a reusable base class for strongly typed XAML icon markup extensions with
/// kind, size and stretch support.
/// </summary>
/// <typeparam name="TIcon">The icon control type.</typeparam>
/// <typeparam name="TKind">The enum type used by the icon control.</typeparam>
public abstract class KindIconExtensionBase<TIcon, TKind> : MarkupExtension
    where TIcon : Control, new()
    where TKind : struct, Enum
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="KindIconExtensionBase{TIcon, TKind}"/> class.
    /// </summary>
    protected KindIconExtensionBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KindIconExtensionBase{TIcon, TKind}"/> class.
    /// </summary>
    /// <param name="kind">The strongly typed icon kind.</param>
    protected KindIconExtensionBase(TKind kind)
    {
        this.Kind = kind;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the strongly typed icon kind.
    /// </summary>
    [ConstructorArgument("kind")]
    public TKind? Kind { get; set; }

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Gets or sets the icon stretch mode.
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
        TIcon icon = new();

        if (this.Kind.HasValue)
        {
            this.SetKind(icon, this.Kind.Value);
        }

        if (this.Size.HasValue)
        {
            this.SetSize(icon, this.Size.Value);
        }

        if (this.Stretch.HasValue)
        {
            this.SetStretch(icon, this.Stretch.Value);
        }

        this.ApplyAdditional(icon);

        return icon;
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Sets the icon kind on the control.
    /// </summary>
    /// <param name="icon">The icon control.</param>
    /// <param name="kind">The strongly typed icon kind.</param>
    protected virtual void SetKind(TIcon icon, TKind kind)
    {
        if (icon is XIconBase<TKind> typedIcon)
        {
            typedIcon.Kind = kind;
        }
    }

    /// <summary>
    /// Sets the icon size on the control.
    /// </summary>
    /// <param name="icon">The icon control.</param>
    /// <param name="size">The icon size.</param>
    protected virtual void SetSize(TIcon icon, double size)
    {
        if (icon is XIconBase<TKind> typedIcon)
        {
            typedIcon.Size = size;
        }
    }

    /// <summary>
    /// Sets the icon stretch on the control.
    /// </summary>
    /// <param name="icon">The icon control.</param>
    /// <param name="stretch">The icon stretch mode.</param>
    protected virtual void SetStretch(TIcon icon, Stretch stretch)
    {
        if (icon is XIconBase<TKind> typedIcon)
        {
            typedIcon.Stretch = stretch;
        }
    }

    /// <summary>
    /// Applies optional additional settings to the icon.
    /// </summary>
    /// <param name="icon">The icon control.</param>
    protected virtual void ApplyAdditional(TIcon icon)
    {
    }
    #endregion
}
#endregion
