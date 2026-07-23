// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseDoubleClickCommandBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class MouseDoubleClickCommandBehavior ###
/// <summary>
/// Provides an attached behavior that executes a command on mouse double-click.
/// </summary>
public static class MouseDoubleClickCommandBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the Command attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
        "Command",
        typeof(ICommand),
        typeof(MouseDoubleClickCommandBehavior),
        new PropertyMetadata(null, OnCommandChanged));

    /// <summary>
    /// Identifies the CommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
        "CommandParameter",
        typeof(object),
        typeof(MouseDoubleClickCommandBehavior),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the HandlesEvent attached dependency property.
    /// </summary>
    public static readonly DependencyProperty HandlesEventProperty = DependencyProperty.RegisterAttached(
        "HandlesEvent",
        typeof(bool),
        typeof(MouseDoubleClickCommandBehavior),
        new PropertyMetadata(true));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the command that is executed on double-click.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command.</returns>
    public static ICommand? GetCommand(DependencyObject element)
    {
        return (ICommand?)element.GetValue(CommandProperty);
    }

    /// <summary>
    /// Sets the command that is executed on double-click.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command to set.</param>
    public static void SetCommand(DependencyObject element, ICommand? value)
    {
        element.SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets the command parameter used for command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command parameter.</returns>
    public static object? GetCommandParameter(DependencyObject element)
    {
        return element.GetValue(CommandParameterProperty);
    }

    /// <summary>
    /// Sets the command parameter used for command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command parameter to set.</param>
    public static void SetCommandParameter(DependencyObject element, object? value)
    {
        element.SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Gets whether the mouse event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the event is handled; otherwise <c>false</c>.</returns>
    public static bool GetHandlesEvent(DependencyObject element)
    {
        return (bool)element.GetValue(HandlesEventProperty);
    }

    /// <summary>
    /// Sets whether the mouse event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetHandlesEvent(DependencyObject element, bool value)
    {
        element.SetValue(HandlesEventProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not UIElement element)
        {
            return;
        }

        element.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;

        if (e.NewValue is ICommand)
        {
            element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
    }

    private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element || e.ClickCount != 2)
        {
            return;
        }

        ICommand? command = GetCommand(element);
        object? parameter = GetCommandParameter(element) ?? element.DataContext;

        if (command is null || !command.CanExecute(parameter))
        {
            return;
        }

        command.Execute(parameter);

        if (GetHandlesEvent(element))
        {
            e.Handled = true;
        }
    }
    #endregion
}
#endregion
