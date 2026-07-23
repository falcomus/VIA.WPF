// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DragDropFilesCommandBehavior.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace VIA.WPF.Behaviors;

#region ### Class DragDropFilesCommandBehavior ###
/// <summary>
/// Provides an attached behavior that executes a command when files are dropped on an element.
/// </summary>
public static class DragDropFilesCommandBehavior
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the Command attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
        "Command",
        typeof(ICommand),
        typeof(DragDropFilesCommandBehavior),
        new PropertyMetadata(null, OnCommandChanged));

    /// <summary>
    /// Identifies the CommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
        "CommandParameter",
        typeof(object),
        typeof(DragDropFilesCommandBehavior),
        new PropertyMetadata(null));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the command that is executed when files are dropped.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured command.</returns>
    public static ICommand? GetCommand(DependencyObject element)
    {
        return (ICommand?)element.GetValue(CommandProperty);
    }

    /// <summary>
    /// Sets the command that is executed when files are dropped.
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
    #endregion

    #region ### Private Methods ###
    private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not UIElement element)
        {
            return;
        }

        element.AllowDrop = e.NewValue is ICommand;
        element.DragOver -= OnDragOver;
        element.Drop -= OnDrop;

        if (e.NewValue is ICommand)
        {
            element.DragOver += OnDragOver;
            element.Drop += OnDrop;
        }
    }

    private static void OnDragOver(object sender, DragEventArgs e)
    {
        if (sender is not UIElement element || !e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        string[] files = GetDroppedFiles(e);
        object? parameter = GetCommandParameter(element) ?? files;
        ICommand? command = GetCommand(element);

        e.Effects = command is not null && command.CanExecute(parameter)
            ? DragDropEffects.Copy
            : DragDropEffects.None;

        e.Handled = true;
    }

    private static void OnDrop(object sender, DragEventArgs e)
    {
        if (sender is not UIElement element || !e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        string[] files = GetDroppedFiles(e);
        object? parameter = GetCommandParameter(element) ?? files;
        ICommand? command = GetCommand(element);

        if (command is not null && command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }

        e.Handled = true;
    }

    private static string[] GetDroppedFiles(DragEventArgs e)
    {
        return e.Data.GetData(DataFormats.FileDrop) as string[] ?? Array.Empty<string>();
    }
    #endregion
}
#endregion
