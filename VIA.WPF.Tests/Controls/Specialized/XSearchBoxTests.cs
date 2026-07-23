// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSearchBoxTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XSearchBoxTests ###
/// <summary>
/// Provides tests for search box state and reset behavior.
/// </summary>
public sealed class XSearchBoxTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that text changes update the derived text and reset-button states.
    /// </summary>
    [Fact]
    public void Text_ShouldUpdateHasTextAndResetButtonVisibility()
    {
        WpfTestHelper.Run(
            () =>
            {
                XSearchBox searchBox = new();

                Assert.False(searchBox.HasText);
                Assert.Equal(Visibility.Collapsed, searchBox.ResetButtonVisibility);

                searchBox.Text = "Query";

                Assert.True(searchBox.HasText);
                Assert.Equal(Visibility.Visible, searchBox.ResetButtonVisibility);

                searchBox.Text = string.Empty;

                Assert.False(searchBox.HasText);
                Assert.Equal(Visibility.Collapsed, searchBox.ResetButtonVisibility);
            });
    }

    /// <summary>
    /// Ensures that HasClearButton acts as an alias for ShowResetButton.
    /// </summary>
    [Fact]
    public void HasClearButton_ShouldAliasShowResetButton()
    {
        WpfTestHelper.Run(
            () =>
            {
                XSearchBox searchBox = new()
                {
                    Text = "Query"
                };

                searchBox.HasClearButton = false;

                Assert.False(searchBox.ShowResetButton);
                Assert.Equal(Visibility.Collapsed, searchBox.ResetButtonVisibility);

                searchBox.HasClearButton = true;

                Assert.True(searchBox.ShowResetButton);
                Assert.Equal(Visibility.Visible, searchBox.ResetButtonVisibility);
            });
    }

    /// <summary>
    /// Ensures that the internal clear command clears the text when no custom command is configured.
    /// </summary>
    [Fact]
    public void ClearSearchCommand_ShouldClearTextWhenNoCustomCommandIsConfigured()
    {
        WpfTestHelper.Run(
            () =>
            {
                XSearchBox searchBox = new()
                {
                    Text = "Query"
                };

                Assert.True(searchBox.ClearSearchCommand.CanExecute(null));

                searchBox.ClearSearchCommand.Execute(null);

                Assert.Equal(string.Empty, searchBox.Text);
                Assert.False(searchBox.HasText);
            });
    }

    /// <summary>
    /// Ensures that the internal clear command delegates to the configured reset command.
    /// </summary>
    [Fact]
    public void ClearSearchCommand_ShouldExecuteConfiguredResetCommand()
    {
        WpfTestHelper.Run(
            () =>
            {
                object parameter = new();
                TestCommand command = new(canExecute: true);
                XSearchBox searchBox = new()
                {
                    Text = "Query",
                    ResetSearchCommand = command,
                    ResetSearchCommandParameter = parameter
                };

                Assert.True(searchBox.ClearSearchCommand.CanExecute(null));

                searchBox.ClearSearchCommand.Execute(null);

                Assert.Equal("Query", searchBox.Text);
                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(parameter, command.LastParameter);
            });
    }

    /// <summary>
    /// Ensures that the clear command is disabled when the custom reset command cannot execute.
    /// </summary>
    [Fact]
    public void ClearSearchCommand_ShouldRespectConfiguredCommandCanExecute()
    {
        WpfTestHelper.Run(
            () =>
            {
                XSearchBox searchBox = new()
                {
                    Text = "Query",
                    ResetSearchCommand = new TestCommand(canExecute: false)
                };

                Assert.False(searchBox.ClearSearchCommand.CanExecute(null));
            });
    }
    #endregion

    #region ### Test Types ###
    private sealed class TestCommand(bool canExecute) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        public int ExecuteCount { get; private set; }

        public object? LastParameter { get; private set; }

        public bool CanExecute(object? parameter)
        {
            return canExecute;
        }

        public void Execute(object? parameter)
        {
            this.ExecuteCount++;
            this.LastParameter = parameter;
        }
    }
    #endregion
}
#endregion
