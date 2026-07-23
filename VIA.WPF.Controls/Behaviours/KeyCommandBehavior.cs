// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyCommandBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class KeyCommandBehavior ###
/// <summary>
/// Provides attached commands for common keyboard actions.
/// </summary>
public static class KeyCommandBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the EnterCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.RegisterAttached(
        "EnterCommand",
        typeof(ICommand),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(null, OnCommandPropertyChanged));

    /// <summary>
    /// Identifies the EnterCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EnterCommandParameterProperty = DependencyProperty.RegisterAttached(
        "EnterCommandParameter",
        typeof(object),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the EnterHandlesEvent attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EnterHandlesEventProperty = DependencyProperty.RegisterAttached(
        "EnterHandlesEvent",
        typeof(bool),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(true));

    /// <summary>
    /// Identifies the EscapeCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EscapeCommandProperty = DependencyProperty.RegisterAttached(
        "EscapeCommand",
        typeof(ICommand),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(null, OnCommandPropertyChanged));

    /// <summary>
    /// Identifies the EscapeCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EscapeCommandParameterProperty = DependencyProperty.RegisterAttached(
        "EscapeCommandParameter",
        typeof(object),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the EscapeHandlesEvent attached dependency property.
    /// </summary>
    public static readonly DependencyProperty EscapeHandlesEventProperty = DependencyProperty.RegisterAttached(
        "EscapeHandlesEvent",
        typeof(bool),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(true));
    #endregion

    #region ### Private Fields ###
    private static readonly DependencyProperty IsRegisteredProperty = DependencyProperty.RegisterAttached(
        "IsRegistered",
        typeof(bool),
        typeof(KeyCommandBehavior),
        new PropertyMetadata(false));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the command that is executed when Enter is pressed.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command.</returns>
    public static ICommand? GetEnterCommand(DependencyObject element)
    {
        return (ICommand?)element.GetValue(EnterCommandProperty);
    }

    /// <summary>
    /// Sets the command that is executed when Enter is pressed.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command to set.</param>
    public static void SetEnterCommand(DependencyObject element, ICommand? value)
    {
        element.SetValue(EnterCommandProperty, value);
    }

    /// <summary>
    /// Gets the command parameter used for the Enter command.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command parameter.</returns>
    public static object? GetEnterCommandParameter(DependencyObject element)
    {
        return element.GetValue(EnterCommandParameterProperty);
    }

    /// <summary>
    /// Sets the command parameter used for the Enter command.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command parameter to set.</param>
    public static void SetEnterCommandParameter(DependencyObject element, object? value)
    {
        element.SetValue(EnterCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets whether the Enter key event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the event is handled; otherwise <c>false</c>.</returns>
    public static bool GetEnterHandlesEvent(DependencyObject element)
    {
        return (bool)element.GetValue(EnterHandlesEventProperty);
    }

    /// <summary>
    /// Sets whether the Enter key event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetEnterHandlesEvent(DependencyObject element, bool value)
    {
        element.SetValue(EnterHandlesEventProperty, value);
    }

    /// <summary>
    /// Gets the command that is executed when Escape is pressed.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command.</returns>
    public static ICommand? GetEscapeCommand(DependencyObject element)
    {
        return (ICommand?)element.GetValue(EscapeCommandProperty);
    }

    /// <summary>
    /// Sets the command that is executed when Escape is pressed.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command to set.</param>
    public static void SetEscapeCommand(DependencyObject element, ICommand? value)
    {
        element.SetValue(EscapeCommandProperty, value);
    }

    /// <summary>
    /// Gets the command parameter used for the Escape command.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command parameter.</returns>
    public static object? GetEscapeCommandParameter(DependencyObject element)
    {
        return element.GetValue(EscapeCommandParameterProperty);
    }

    /// <summary>
    /// Sets the command parameter used for the Escape command.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The command parameter to set.</param>
    public static void SetEscapeCommandParameter(DependencyObject element, object? value)
    {
        element.SetValue(EscapeCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets whether the Escape key event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the event is handled; otherwise <c>false</c>.</returns>
    public static bool GetEscapeHandlesEvent(DependencyObject element)
    {
        return (bool)element.GetValue(EscapeHandlesEventProperty);
    }

    /// <summary>
    /// Sets whether the Escape key event is marked as handled after command execution.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetEscapeHandlesEvent(DependencyObject element, bool value)
    {
        element.SetValue(EscapeHandlesEventProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnCommandPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not UIElement element)
        {
            return;
        }

        UpdateRegistration(element);
    }

    private static void UpdateRegistration(UIElement element)
    {
        bool hasCommand = GetEnterCommand(element) is not null || GetEscapeCommand(element) is not null;
        bool isRegistered = (bool)element.GetValue(IsRegisteredProperty);

        if (hasCommand && !isRegistered)
        {
            element.PreviewKeyDown += OnPreviewKeyDown;
            element.SetValue(IsRegisteredProperty, true);
        }
        else if (!hasCommand && isRegistered)
        {
            element.PreviewKeyDown -= OnPreviewKeyDown;
            element.SetValue(IsRegisteredProperty, false);
        }
    }

    private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        if (e.Key is Key.Enter or Key.Return)
        {
            ExecuteCommand(GetEnterCommand(element), GetEnterCommandParameter(element) ?? element.DataContext, element, GetEnterHandlesEvent(element), e);
        }
        else if (e.Key == Key.Escape)
        {
            ExecuteCommand(GetEscapeCommand(element), GetEscapeCommandParameter(element) ?? element.DataContext, element, GetEscapeHandlesEvent(element), e);
        }
    }

    private static void ExecuteCommand(ICommand? command, object? parameter, IInputElement target, bool handlesEvent, KeyEventArgs e)
    {
        if (command is null)
        {
            return;
        }

        if (command is RoutedCommand routedCommand)
        {
            if (!routedCommand.CanExecute(parameter, target))
            {
                return;
            }

            routedCommand.Execute(parameter, target);
        }
        else
        {
            if (!command.CanExecute(parameter))
            {
                return;
            }

            command.Execute(parameter);
        }

        if (handlesEvent)
        {
            e.Handled = true;
        }
    }
    #endregion
}
#endregion
