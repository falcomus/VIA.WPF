// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewFlyoutPlacement.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Enum XViewFlyoutPlacement ###
/// <summary>
/// Defines where the local flyout of an <see cref="XViewContainer"/> is displayed inside the view area.
/// </summary>
public enum XViewFlyoutPlacement
{
    /// <summary>
    /// Displays the flyout at the top edge of the view area.
    /// </summary>
    Top,

    /// <summary>
    /// Displays the flyout centered inside the view area.
    /// </summary>
    Center,

    /// <summary>
    /// Displays the flyout at the bottom edge of the view area.
    /// </summary>
    Bottom,
}
#endregion
