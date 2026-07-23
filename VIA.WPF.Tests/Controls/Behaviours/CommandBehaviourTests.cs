// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandBehaviourTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using VIA.WPF.Behaviors;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Behaviours;

#region ### Class CommandBehaviourTests ###
/// <summary>
/// Tests command based VIA.WPF behaviors.
/// </summary>
public sealed class CommandBehaviourTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="KeyCommandBehavior" /> executes the Enter command with the configured parameter.
    /// </summary>
    [Fact]
    public void KeyCommandBehavior_ShouldExecuteEnterCommand()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button element = new();
                TrackingCommand command = new();
                object parameter = new();

                KeyCommandBehavior.SetEnterCommand(element, command);
                KeyCommandBehavior.SetEnterCommandParameter(element, parameter);

                using HwndSource source = CreateInputSource();
                KeyEventArgs args = CreateKeyEventArgs(source, Key.Enter);

                element.RaiseEvent(args);

                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(parameter, command.LastParameter);
                Assert.True(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="KeyCommandBehavior" /> executes the Escape command with the data context fallback.
    /// </summary>
    [Fact]
    public void KeyCommandBehavior_ShouldExecuteEscapeCommandWithDataContextFallback()
    {
        WpfTestHelper.Run(
            () =>
            {
                object dataContext = new();
                Button element = new()
                {
                    DataContext = dataContext
                };
                TrackingCommand command = new();

                KeyCommandBehavior.SetEscapeCommand(element, command);

                using HwndSource source = CreateInputSource();
                KeyEventArgs args = CreateKeyEventArgs(source, Key.Escape);

                element.RaiseEvent(args);

                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(dataContext, command.LastParameter);
                Assert.True(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="KeyCommandBehavior" /> does not handle the event when the command cannot execute.
    /// </summary>
    [Fact]
    public void KeyCommandBehavior_ShouldNotHandleEventWhenCommandCannotExecute()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button element = new();
                TrackingCommand command = new()
                {
                    CanExecuteResult = false
                };

                KeyCommandBehavior.SetEnterCommand(element, command);

                using HwndSource source = CreateInputSource();
                KeyEventArgs args = CreateKeyEventArgs(source, Key.Enter);

                element.RaiseEvent(args);

                Assert.Equal(0, command.ExecuteCount);
                Assert.False(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="MouseDoubleClickCommandBehavior" /> executes the configured command on double-click.
    /// </summary>
    [Fact]
    public void MouseDoubleClickCommandBehavior_ShouldExecuteCommandOnDoubleClick()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();
                object parameter = new();

                MouseDoubleClickCommandBehavior.SetCommand(element, command);
                MouseDoubleClickCommandBehavior.SetCommandParameter(element, parameter);

                MouseButtonEventArgs args = CreateMouseButtonEventArgs(2, UIElement.PreviewMouseLeftButtonDownEvent);

                element.RaiseEvent(args);

                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(parameter, command.LastParameter);
                Assert.True(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="MouseDoubleClickCommandBehavior" /> ignores single-clicks.
    /// </summary>
    [Fact]
    public void MouseDoubleClickCommandBehavior_ShouldIgnoreSingleClick()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();

                MouseDoubleClickCommandBehavior.SetCommand(element, command);

                MouseButtonEventArgs args = CreateMouseButtonEventArgs(1, UIElement.PreviewMouseLeftButtonDownEvent);

                element.RaiseEvent(args);

                Assert.Equal(0, command.ExecuteCount);
                Assert.False(args.Handled);
            });
    }

    /// <summary>
    /// Verifies that <see cref="DragDropFilesCommandBehavior" /> enables file dropping when a command is set.
    /// </summary>
    [Fact]
    public void DragDropFilesCommandBehavior_ShouldEnableAllowDropWhenCommandIsSet()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();

                DragDropFilesCommandBehavior.SetCommand(element, command);

                Assert.True(element.AllowDrop);
                Assert.Same(command, DragDropFilesCommandBehavior.GetCommand(element));
            });
    }

    /// <summary>
    /// Verifies that <see cref="DragDropFilesCommandBehavior" /> disables file dropping when the command is cleared.
    /// </summary>
    [Fact]
    public void DragDropFilesCommandBehavior_ShouldDisableAllowDropWhenCommandIsCleared()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();

                DragDropFilesCommandBehavior.SetCommand(element, command);
                DragDropFilesCommandBehavior.SetCommand(element, null);

                Assert.False(element.AllowDrop);
                Assert.Null(DragDropFilesCommandBehavior.GetCommand(element));
            });
    }

    /// <summary>
    /// Verifies that <see cref="WindowDragMoveBehavior" /> ignores mouse events without a containing window.
    /// </summary>
    [Fact]
    public void WindowDragMoveBehavior_ShouldIgnoreMouseDownWithoutWindow()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();

                WindowDragMoveBehavior.SetIsEnabled(element, true);

                MouseButtonEventArgs args = CreateMouseButtonEventArgs(1, UIElement.MouseLeftButtonDownEvent);

                element.RaiseEvent(args);

                Assert.False(args.Handled);
            });
    }
    #endregion

    #region ### Private Methods ###
    private static HwndSource CreateInputSource()
    {
        return new HwndSource(new HwndSourceParameters("VIA.WPF.Tests")
        {
            Width = 1,
            Height = 1
        });
    }

    private static KeyEventArgs CreateKeyEventArgs(PresentationSource source, Key key)
    {
        return new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, key)
        {
            RoutedEvent = Keyboard.PreviewKeyDownEvent
        };
    }

    private static MouseButtonEventArgs CreateMouseButtonEventArgs(int clickCount, RoutedEvent routedEvent)
    {
        MouseButtonEventArgs args = new(Mouse.PrimaryDevice, 0, MouseButton.Left)
        {
            RoutedEvent = routedEvent
        };

        PropertyInfo? clickCountProperty = typeof(MouseButtonEventArgs).GetProperty(nameof(MouseButtonEventArgs.ClickCount));
        if (clickCountProperty?.CanWrite == true)
        {
            clickCountProperty.SetValue(args, clickCount);
            return args;
        }

        FieldInfo? clickCountField = typeof(MouseButtonEventArgs).GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
        clickCountField?.SetValue(args, clickCount);

        return args;
    }

    #endregion

    #region ### Private Classes ###
    private sealed class TrackingCommand : ICommand
    {
        #region ### Public Events ###
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region ### Public Properties ###
        public bool CanExecuteResult { get; set; } = true;

        public int ExecuteCount { get; private set; }

        public object? LastParameter { get; private set; }
        #endregion

        #region ### Public Methods ###
        public bool CanExecute(object? parameter)
        {
            return this.CanExecuteResult;
        }

        public void Execute(object? parameter)
        {
            this.ExecuteCount++;
            this.LastParameter = parameter;
        }
        #endregion
    }
    #endregion
}
#endregion
