// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationListVariant.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XNavigationListVariant ###
/// <summary>
/// Defines the visual surface variant of an <see cref="XNavigationList"/>.
/// </summary>
public enum XNavigationListVariant
{
    /// <summary>
    /// Preserves the established dark navigation presentation.
    /// </summary>
    Default,

    /// <summary>
    /// Uses theme-adaptive surface, text and selection brushes.
    /// </summary>
    Surface,

    /// <summary>
    /// Explicitly uses the dark navigation presentation in every theme mode.
    /// </summary>
    Dark
}
#endregion
