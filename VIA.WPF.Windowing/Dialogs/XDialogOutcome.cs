// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogOutcome.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Windowing;

#region ### Enum XDialogOutcome ###
/// <summary>
/// Defines the normalized outcomes returned by a modal WPF dialog.
/// </summary>
public enum XDialogOutcome
{
    /// <summary>
    /// The dialog returned <see langword="true"/>.
    /// </summary>
    Accepted,

    /// <summary>
    /// The dialog returned <see langword="false"/>.
    /// </summary>
    NotAccepted,

    /// <summary>
    /// The dialog returned no boolean result.
    /// </summary>
    NoResult
}
#endregion