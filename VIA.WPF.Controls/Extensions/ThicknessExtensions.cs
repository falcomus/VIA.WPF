// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThicknessExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Extensions;

#region ### Class ThicknessExtensions ###
/// <summary>
/// Provides convenience methods for WPF thickness values.
/// </summary>
public static class ThicknessExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the sum of the left and right values.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <returns>The horizontal thickness.</returns>
    public static double Horizontal(this Thickness thickness)
    {
        return thickness.Left + thickness.Right;
    }

    /// <summary>
    /// Gets the sum of the top and bottom values.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <returns>The vertical thickness.</returns>
    public static double Vertical(this Thickness thickness)
    {
        return thickness.Top + thickness.Bottom;
    }

    /// <summary>
    /// Returns a thickness with a replaced left value.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <param name="left">The left value.</param>
    /// <returns>The adjusted thickness.</returns>
    public static Thickness WithLeft(this Thickness thickness, double left)
    {
        return new Thickness(left, thickness.Top, thickness.Right, thickness.Bottom);
    }

    /// <summary>
    /// Returns a thickness with a replaced top value.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <param name="top">The top value.</param>
    /// <returns>The adjusted thickness.</returns>
    public static Thickness WithTop(this Thickness thickness, double top)
    {
        return new Thickness(thickness.Left, top, thickness.Right, thickness.Bottom);
    }

    /// <summary>
    /// Returns a thickness with a replaced right value.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <param name="right">The right value.</param>
    /// <returns>The adjusted thickness.</returns>
    public static Thickness WithRight(this Thickness thickness, double right)
    {
        return new Thickness(thickness.Left, thickness.Top, right, thickness.Bottom);
    }

    /// <summary>
    /// Returns a thickness with a replaced bottom value.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <param name="bottom">The bottom value.</param>
    /// <returns>The adjusted thickness.</returns>
    public static Thickness WithBottom(this Thickness thickness, double bottom)
    {
        return new Thickness(thickness.Left, thickness.Top, thickness.Right, bottom);
    }

    /// <summary>
    /// Adds two thickness values.
    /// </summary>
    /// <param name="left">The left thickness.</param>
    /// <param name="right">The right thickness.</param>
    /// <returns>The added thickness.</returns>
    public static Thickness Add(this Thickness left, Thickness right)
    {
        return new Thickness(left.Left + right.Left, left.Top + right.Top, left.Right + right.Right, left.Bottom + right.Bottom);
    }

    /// <summary>
    /// Multiplies all sides by the specified factor.
    /// </summary>
    /// <param name="thickness">The thickness.</param>
    /// <param name="factor">The factor.</param>
    /// <returns>The scaled thickness.</returns>
    public static Thickness Scale(this Thickness thickness, double factor)
    {
        return new Thickness(thickness.Left * factor, thickness.Top * factor, thickness.Right * factor, thickness.Bottom * factor);
    }
    #endregion
}
#endregion
