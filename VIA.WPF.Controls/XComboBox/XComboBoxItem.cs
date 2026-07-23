// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XComboBoxItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XComboBoxItem ###
/// <summary>
/// Represents the standard combo box item control of VIA.WPF.
/// </summary>
public class XComboBoxItem : ComboBoxItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XComboBoxItem"/> class.
    /// </summary>
    static XComboBoxItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XComboBoxItem),
            new FrameworkPropertyMetadata(typeof(XComboBoxItem)));
    }
    #endregion
}
#endregion