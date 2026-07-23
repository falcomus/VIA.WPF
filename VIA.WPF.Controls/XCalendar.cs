// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCalendar.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XCalendar ###
/// <summary>
/// Represents the VIA.WPF themed calendar used by <see cref="XDatePicker"/> and standalone date-selection surfaces.
/// </summary>
public class XCalendar : Calendar
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XCalendar"/> class.
    /// </summary>
    static XCalendar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XCalendar),
            new FrameworkPropertyMetadata(typeof(XCalendar)));
    }
    #endregion
}
#endregion
