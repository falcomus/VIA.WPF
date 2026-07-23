// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XControlAppearance.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XControlAppearance ###
/// <summary>
/// Defines the visual appearance of a control.
/// </summary>
public enum XControlAppearance
{
    /// <summary>
    /// Renders the control with a filled background.
    /// </summary>
    Solid,

    /// <summary>
    /// Renders the control as an outline.
    /// </summary>
    Outline,

    /// <summary>
    /// Renders the control with a subtle tinted background.
    /// </summary>
    Subtle,

    /// <summary>
    /// Renders the control with a very subtle tinted background.
    /// </summary>
    VerySubtle,

    /// <summary>
    /// Renders the control with minimal chrome.
    /// </summary>
    Ghost,

    /// <summary>
    /// Renders the control like a link.
    /// </summary>
    Link
}
#endregion