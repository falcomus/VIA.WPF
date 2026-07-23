// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxCommitOnEnterBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class TextBoxCommitOnEnterBehavior ###
/// <summary>
/// Provides an attached behavior that updates the text binding when Enter is pressed.
/// </summary>
public static class TextBoxCommitOnEnterBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the IsEnabled attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(TextBoxCommitOnEnterBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged));

    /// <summary>
    /// Identifies the MoveFocusAfterCommit attached dependency property.
    /// </summary>
    public static readonly DependencyProperty MoveFocusAfterCommitProperty = DependencyProperty.RegisterAttached(
        "MoveFocusAfterCommit",
        typeof(bool),
        typeof(TextBoxCommitOnEnterBehavior),
        new PropertyMetadata(false));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets whether the behavior is enabled for the specified text box.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the behavior is enabled; otherwise <c>false</c>.</returns>
    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// Sets whether the behavior is enabled for the specified text box.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Gets whether focus is moved to the next element after committing the binding.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when focus is moved; otherwise <c>false</c>.</returns>
    public static bool GetMoveFocusAfterCommit(DependencyObject element)
    {
        return (bool)element.GetValue(MoveFocusAfterCommitProperty);
    }

    /// <summary>
    /// Sets whether focus is moved to the next element after committing the binding.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetMoveFocusAfterCommit(DependencyObject element, bool value)
    {
        element.SetValue(MoveFocusAfterCommitProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnIsEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not TextBox textBox)
        {
            return;
        }

        textBox.KeyDown -= OnKeyDown;

        if (e.NewValue is true)
        {
            textBox.KeyDown += OnKeyDown;
        }
    }

    private static void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox || e.Key is not Key.Enter and not Key.Return)
        {
            return;
        }

        if (textBox.AcceptsReturn)
        {
            return;
        }

        BindingExpression? bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
        bindingExpression?.UpdateSource();

        if (GetMoveFocusAfterCommit(textBox))
        {
            textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        e.Handled = true;
    }
    #endregion
}
#endregion
