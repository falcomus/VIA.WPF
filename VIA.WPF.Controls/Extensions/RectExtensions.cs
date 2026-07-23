// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Extensions;

#region ### Class RectExtensions ###
/// <summary>
/// Provides convenience methods for WPF rectangles.
/// </summary>
public static class RectExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the center point of the rectangle.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns>The center point.</returns>
    public static Point GetCenter(this Rect rect)
    {
        return new Point(rect.Left + (rect.Width / 2d), rect.Top + (rect.Height / 2d));
    }

    /// <summary>
    /// Gets a value indicating whether the rectangle has valid finite dimensions.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <returns><c>true</c> if the rectangle is valid; otherwise, <c>false</c>.</returns>
    public static bool IsFinite(this Rect rect)
    {
        return !rect.IsEmpty &&
               !double.IsNaN(rect.X) &&
               !double.IsNaN(rect.Y) &&
               !double.IsNaN(rect.Width) &&
               !double.IsNaN(rect.Height) &&
               !double.IsInfinity(rect.X) &&
               !double.IsInfinity(rect.Y) &&
               !double.IsInfinity(rect.Width) &&
               !double.IsInfinity(rect.Height);
    }

    /// <summary>
    /// Returns a rectangle inflated by the specified horizontal and vertical amounts.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="horizontal">The horizontal amount.</param>
    /// <param name="vertical">The vertical amount.</param>
    /// <returns>The inflated rectangle.</returns>
    public static Rect Inflated(this Rect rect, double horizontal, double vertical)
    {
        rect.Inflate(horizontal, vertical);

        return rect;
    }

    /// <summary>
    /// Returns a rectangle deflated by the specified horizontal and vertical amounts.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    /// <param name="horizontal">The horizontal amount.</param>
    /// <param name="vertical">The vertical amount.</param>
    /// <returns>The deflated rectangle.</returns>
    public static Rect Deflated(this Rect rect, double horizontal, double vertical)
    {
        rect.Inflate(-horizontal, -vertical);

        return rect;
    }
    #endregion
}
#endregion
