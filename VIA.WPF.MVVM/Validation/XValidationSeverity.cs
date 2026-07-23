// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationSeverity.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Enum XValidationSeverity ###
/// <summary>
/// Defines the severity of a validation message.
/// </summary>
public enum XValidationSeverity
{
    /// <summary>
    /// Indicates an informational validation message.
    /// </summary>
    Information,

    /// <summary>
    /// Indicates a validation warning that does not block saving.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates a validation error that blocks saving.
    /// </summary>
    Error
}
#endregion
