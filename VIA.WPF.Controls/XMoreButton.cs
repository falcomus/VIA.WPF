// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMoreButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Automation;

namespace VIA.WPF.Controls;

#region ### Class XMoreButton ###
/// <summary>
/// Represents the standard compact overflow menu button.
/// </summary>
public class XMoreButton : XMenuButton
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XMoreButton"/> class.
    /// </summary>
    public XMoreButton()
    {
        this.SetCurrentValue(ContentProperty, "\u22EE");
        this.SetCurrentValue(AppearanceProperty, XControlAppearance.Ghost);
        this.SetCurrentValue(SizeProperty, XControlSize.Small);
        this.SetCurrentValue(MinWidthProperty, 28d);
        this.SetCurrentValue(WidthProperty, 28d);
        this.SetCurrentValue(PaddingProperty, new Thickness(0d));
        this.SetCurrentValue(ToolTipProperty, "More actions");
        AutomationProperties.SetName(this, "More actions");
    }
    #endregion
}
#endregion
