// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BehaviourAttachedPropertyTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;
using VIA.WPF.Behaviors;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Behaviours;

#region ### Class BehaviourAttachedPropertyTests ###
/// <summary>
/// Tests attached property contracts for VIA.WPF behaviors.
/// </summary>
public sealed class BehaviourAttachedPropertyTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="AutoScrollOnDragOverBehavior" /> stores its attached property values.
    /// </summary>
    [Fact]
    public void AutoScrollOnDragOverBehavior_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();

                AutoScrollOnDragOverBehavior.SetIsEnabled(element, true);
                AutoScrollOnDragOverBehavior.SetEdgeThreshold(element, 24d);
                AutoScrollOnDragOverBehavior.SetScrollStep(element, 8d);

                Assert.True(AutoScrollOnDragOverBehavior.GetIsEnabled(element));
                Assert.True(element.AllowDrop);
                Assert.Equal(24d, AutoScrollOnDragOverBehavior.GetEdgeThreshold(element));
                Assert.Equal(8d, AutoScrollOnDragOverBehavior.GetScrollStep(element));

                AutoScrollOnDragOverBehavior.SetIsEnabled(element, false);

                Assert.False(AutoScrollOnDragOverBehavior.GetIsEnabled(element));
            });
    }

    /// <summary>
    /// Verifies that <see cref="DragDropFilesCommandBehavior" /> stores command related attached property values.
    /// </summary>
    [Fact]
    public void DragDropFilesCommandBehavior_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();
                object parameter = new();

                DragDropFilesCommandBehavior.SetCommand(element, command);
                DragDropFilesCommandBehavior.SetCommandParameter(element, parameter);

                Assert.Same(command, DragDropFilesCommandBehavior.GetCommand(element));
                Assert.Same(parameter, DragDropFilesCommandBehavior.GetCommandParameter(element));
                Assert.True(element.AllowDrop);

                DragDropFilesCommandBehavior.SetCommand(element, null);

                Assert.Null(DragDropFilesCommandBehavior.GetCommand(element));
                Assert.False(element.AllowDrop);
            });
    }

    /// <summary>
    /// Verifies that focus behaviors store their enabled values.
    /// </summary>
    [Fact]
    public void FocusBehaviors_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button loadedButton = new();
                Button visibleButton = new();

                FocusOnLoadedBehavior.SetIsEnabled(loadedButton, true);
                FocusOnVisibleBehavior.SetIsEnabled(visibleButton, true);

                Assert.True(FocusOnLoadedBehavior.GetIsEnabled(loadedButton));
                Assert.True(FocusOnVisibleBehavior.GetIsEnabled(visibleButton));

                FocusOnLoadedBehavior.SetIsEnabled(loadedButton, false);
                FocusOnVisibleBehavior.SetIsEnabled(visibleButton, false);

                Assert.False(FocusOnLoadedBehavior.GetIsEnabled(loadedButton));
                Assert.False(FocusOnVisibleBehavior.GetIsEnabled(visibleButton));
            });
    }

    /// <summary>
    /// Verifies that <see cref="KeyCommandBehavior" /> stores keyboard command values.
    /// </summary>
    [Fact]
    public void KeyCommandBehavior_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                Button element = new();
                TrackingCommand enterCommand = new();
                TrackingCommand escapeCommand = new();
                object enterParameter = new();
                object escapeParameter = new();

                KeyCommandBehavior.SetEnterCommand(element, enterCommand);
                KeyCommandBehavior.SetEnterCommandParameter(element, enterParameter);
                KeyCommandBehavior.SetEnterHandlesEvent(element, false);
                KeyCommandBehavior.SetEscapeCommand(element, escapeCommand);
                KeyCommandBehavior.SetEscapeCommandParameter(element, escapeParameter);
                KeyCommandBehavior.SetEscapeHandlesEvent(element, false);

                Assert.Same(enterCommand, KeyCommandBehavior.GetEnterCommand(element));
                Assert.Same(enterParameter, KeyCommandBehavior.GetEnterCommandParameter(element));
                Assert.False(KeyCommandBehavior.GetEnterHandlesEvent(element));
                Assert.Same(escapeCommand, KeyCommandBehavior.GetEscapeCommand(element));
                Assert.Same(escapeParameter, KeyCommandBehavior.GetEscapeCommandParameter(element));
                Assert.False(KeyCommandBehavior.GetEscapeHandlesEvent(element));
            });
    }

    /// <summary>
    /// Verifies that <see cref="MouseDoubleClickCommandBehavior" /> stores command values.
    /// </summary>
    [Fact]
    public void MouseDoubleClickCommandBehavior_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                TrackingCommand command = new();
                object parameter = new();

                MouseDoubleClickCommandBehavior.SetCommand(element, command);
                MouseDoubleClickCommandBehavior.SetCommandParameter(element, parameter);
                MouseDoubleClickCommandBehavior.SetHandlesEvent(element, false);

                Assert.Same(command, MouseDoubleClickCommandBehavior.GetCommand(element));
                Assert.Same(parameter, MouseDoubleClickCommandBehavior.GetCommandParameter(element));
                Assert.False(MouseDoubleClickCommandBehavior.GetHandlesEvent(element));
            });
    }

    /// <summary>
    /// Verifies that simple enabled behaviors store their attached property values.
    /// </summary>
    [Fact]
    public void SimpleEnabledBehaviors_ShouldStoreAttachedPropertyValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                ListBox listBox = new();
                TextBox selectAllTextBox = new();
                TextBox commitTextBox = new();
                Border dragElement = new();

                ScrollIntoViewBehavior.SetIsEnabled(listBox, true);
                SelectAllTextBoxBehavior.SetIsEnabled(selectAllTextBox, true);
                TextBoxCommitOnEnterBehavior.SetIsEnabled(commitTextBox, true);
                TextBoxCommitOnEnterBehavior.SetMoveFocusAfterCommit(commitTextBox, true);
                WindowDragMoveBehavior.SetIsEnabled(dragElement, true);

                Assert.True(ScrollIntoViewBehavior.GetIsEnabled(listBox));
                Assert.True(SelectAllTextBoxBehavior.GetIsEnabled(selectAllTextBox));
                Assert.True(TextBoxCommitOnEnterBehavior.GetIsEnabled(commitTextBox));
                Assert.True(TextBoxCommitOnEnterBehavior.GetMoveFocusAfterCommit(commitTextBox));
                Assert.True(WindowDragMoveBehavior.GetIsEnabled(dragElement));
            });
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TrackingCommand : System.Windows.Input.ICommand
    {
        #region ### Public Events ###
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion

        #region ### Public Methods ###
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
        }
        #endregion
    }
    #endregion
}
#endregion
