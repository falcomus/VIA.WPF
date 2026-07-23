// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeTransitionDirection.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Themes;

#region ### Enum XThemeTransitionDirection ###
/// <summary>
/// Defines the visual direction used for theme transition reveal animations.
/// </summary>
public enum XThemeTransitionDirection
{
    /// <summary>
    /// Reveals the new theme from top to bottom.
    /// </summary>
    TopToBottom,

    /// <summary>
    /// Reveals the new theme from bottom to top.
    /// </summary>
    BottomToTop,

    /// <summary>
    /// Reveals the new theme from left to right.
    /// </summary>
    LeftToRight,

    /// <summary>
    /// Reveals the new theme from right to left.
    /// </summary>
    RightToLeft
}
#endregion