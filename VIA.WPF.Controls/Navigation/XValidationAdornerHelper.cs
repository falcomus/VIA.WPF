// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationAdornerHelper.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using WpfValidation = System.Windows.Controls.Validation;

namespace VIA.WPF.Controls;

#region ### Class XValidationAdornerHelper ###
/// <summary>
/// Provides shared helpers for suppressing default WPF validation adorners on VIA.WPF chrome and layout controls.
/// </summary>
internal static class XValidationAdornerHelper
{
    #region ### Fields ###
    private static readonly ControlTemplate EmptyErrorTemplate = new();
    #endregion

    #region ### Internal Methods ###
    /// <summary>
    /// Suppresses the default WPF validation error template without replacing explicit user styles or bindings.
    /// </summary>
    /// <param name="element">The target element.</param>
    internal static void SuppressDefaultErrorTemplate(DependencyObject element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.SetCurrentValue(WpfValidation.ErrorTemplateProperty, EmptyErrorTemplate);
    }

    /// <summary>
    /// Clears a previously configured validation error template.
    /// </summary>
    /// <param name="element">The target element.</param>
    internal static void ClearErrorTemplate(DependencyObject element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.ClearValue(WpfValidation.ErrorTemplateProperty);
    }
    #endregion
}
#endregion
