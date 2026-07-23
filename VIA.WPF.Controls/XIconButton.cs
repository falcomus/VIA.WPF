// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Automation;

namespace VIA.WPF.Controls;

#region ### Class XIconButton ###
/// <summary>
/// Represents a themed icon-only button of VIA.WPF.
/// </summary>
public class XIconButton : XButton
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XIconButton"/> class.
    /// </summary>
    static XIconButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XIconButton),
            new FrameworkPropertyMetadata(typeof(XIconButton)));
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.ApplyAutomationNameFallback();
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == ToolTipProperty)
        {
            this.ApplyAutomationNameFallback();
        }
    }
    #endregion

    #region ### Private Methods ###
    private void ApplyAutomationNameFallback()
    {
        object localAutomationName = this.ReadLocalValue(AutomationProperties.NameProperty);
        if (localAutomationName != DependencyProperty.UnsetValue)
        {
            return;
        }

        if (this.ToolTip is string text && !string.IsNullOrWhiteSpace(text))
        {
            AutomationProperties.SetName(this, text);
        }
    }
    #endregion
}
#endregion
