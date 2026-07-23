// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupBounds.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Geometry;

/// <summary>
/// Describes an element rectangle in logical designer coordinates.
/// </summary>
/// <param name="X">The horizontal origin.</param>
/// <param name="Y">The vertical origin.</param>
/// <param name="Width">The width.</param>
/// <param name="Height">The height.</param>
public readonly record struct MockupBounds(double X, double Y, double Width, double Height)
{
    /// <summary>
    /// Gets whether the bounds have a positive size.
    /// </summary>
    public bool HasArea => Width > 0d && Height > 0d;
}
