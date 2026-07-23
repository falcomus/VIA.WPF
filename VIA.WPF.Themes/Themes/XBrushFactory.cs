// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBrushFactory.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XBrushFactory ###
/// <summary>
/// Creates brushes for VIA.WPF runtime and static resource scenarios.
/// </summary>
public static class XBrushFactory
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates a mutable <see cref="SolidColorBrush"/> for runtime resources that may be replaced or animated.
    /// </summary>
    /// <param name="color">The brush color.</param>
    /// <returns>A mutable brush instance.</returns>
    public static SolidColorBrush CreateRuntimeBrush(Color color)
    {
        return new SolidColorBrush(color);
    }

    /// <summary>
    /// Creates a frozen <see cref="SolidColorBrush"/> for static defaults and immutable resources.
    /// </summary>
    /// <param name="color">The brush color.</param>
    /// <returns>A frozen brush instance.</returns>
    public static SolidColorBrush CreateFrozenBrush(Color color)
    {
        return FreezeIfPossible(new SolidColorBrush(color));
    }

    /// <summary>
    /// Creates a frozen opaque <see cref="SolidColorBrush"/> for static defaults and immutable resources.
    /// </summary>
    /// <param name="red">The red channel.</param>
    /// <param name="green">The green channel.</param>
    /// <param name="blue">The blue channel.</param>
    /// <returns>A frozen brush instance.</returns>
    public static SolidColorBrush CreateFrozenBrush(byte red, byte green, byte blue)
    {
        return CreateFrozenBrush(Color.FromRgb(red, green, blue));
    }

    /// <summary>
    /// Creates a frozen translucent <see cref="SolidColorBrush"/> for static defaults and immutable resources.
    /// </summary>
    /// <param name="alpha">The alpha channel.</param>
    /// <param name="red">The red channel.</param>
    /// <param name="green">The green channel.</param>
    /// <param name="blue">The blue channel.</param>
    /// <returns>A frozen brush instance.</returns>
    public static SolidColorBrush CreateFrozenBrush(byte alpha, byte red, byte green, byte blue)
    {
        return CreateFrozenBrush(Color.FromArgb(alpha, red, green, blue));
    }

    /// <summary>
    /// Freezes the specified <see cref="Freezable"/> instance if possible.
    /// </summary>
    /// <typeparam name="T">The freezable type.</typeparam>
    /// <param name="freezable">The freezable instance.</param>
    /// <returns>The same instance after an optional freeze operation.</returns>
    public static T FreezeIfPossible<T>(T freezable)
        where T : Freezable
    {
        if (freezable.CanFreeze)
        {
            freezable.Freeze();
        }

        return freezable;
    }
    #endregion
}
#endregion