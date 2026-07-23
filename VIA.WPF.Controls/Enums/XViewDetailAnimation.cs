// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewDetailAnimation.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XViewDetailAnimation ###
/// <summary>
/// Defines the animation style used by <see cref="XViewContainer"/> when its detail area opens or closes.
/// </summary>
public enum XViewDetailAnimation
{
    #region ### Values ###
    /// <summary>
    /// Disables detail area animation.
    /// </summary>
    None,

    /// <summary>
    /// Animates the detail area by fading it in and out.
    /// </summary>
    Fade,

    /// <summary>
    /// Animates the detail area by sliding it in and out with a subtle fade.
    /// </summary>
    Slide,

    /// <summary>
    /// Animates the detail area by zooming it in and out with a subtle fade.
    /// </summary>
    Zoom,

    /// <summary>
    /// Animates the detail area with a combined slide, zoom and fade effect.
    /// </summary>
    SlideZoom
    #endregion
}
#endregion
