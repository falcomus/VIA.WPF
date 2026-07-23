// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XControlVariant.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XControlVariant ###
/// <summary>
/// Defines the semantic color variant of a control.
/// </summary>
public enum XControlVariant
{
    /// <summary>
    /// Uses the default surface-based styling.
    /// </summary>
    Default,

    /// <summary>
    /// Uses the primary theme color.
    /// </summary>
    Primary,

    /// <summary>
    /// Uses the accent theme color.
    /// </summary>
    Accent,

    /// <summary>
    /// Uses the success theme color.
    /// </summary>
    Success,

    /// <summary>
    /// Uses the warning theme color.
    /// </summary>
    Warning,

    /// <summary>
    /// Uses the danger theme color.
    /// </summary>
    Danger,

    /// <summary>
    /// Uses the info theme color.
    /// </summary>
    Info
}
#endregion