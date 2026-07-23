// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupInsertRequestTrigger.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XLookupInsertRequestTrigger ###
/// <summary>
/// Defines when <see cref="XLookupComboBox"/> raises insert requests for unmatched editable text.
/// </summary>
[Flags]
public enum XLookupInsertRequestTrigger
{
    /// <summary>
    /// Insert requests are disabled.
    /// </summary>
    None = 0,

    /// <summary>
    /// Insert requests are raised when Enter is pressed.
    /// </summary>
    Enter = 1,

    /// <summary>
    /// Insert requests are raised when the lookup loses keyboard focus.
    /// </summary>
    LostFocus = 2,

    /// <summary>
    /// Insert requests are raised when Enter is pressed or when the lookup loses keyboard focus.
    /// </summary>
    EnterOrLostFocus = Enter | LostFocus
}
#endregion
