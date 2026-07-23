// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToolbarContextTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class XToolbarContextTests ###
/// <summary>
/// Provides tests for <see cref="XToolbarContext" />.
/// </summary>
public sealed class XToolbarContextTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that an empty toolbar context exposes no visible items.
    /// </summary>
    [Fact]
    public void CreateEmpty_ShouldExposeNoVisibleItems()
    {
        XToolbarContext toolbar = XToolbarContext.CreateEmpty();

        Assert.False(toolbar.ShowNewButton);
        Assert.False(toolbar.ShowViewButton);
        Assert.False(toolbar.ShowEditButton);
        Assert.False(toolbar.ShowDeleteButton);
        Assert.False(toolbar.ShowRefreshButton);
        Assert.False(toolbar.ShowViewModeSelector);
        Assert.False(toolbar.ShowRememberViewToggle);
        Assert.False(toolbar.IsRememberViewStateEnabled);
        Assert.Equal(XContentViewMode.Grid, toolbar.ViewMode);
        Assert.False(toolbar.HasCommandButtons);
        Assert.False(toolbar.HasToolbarItems);
        Assert.False(toolbar.ShowCommandSeparator);
        Assert.Null(toolbar.NewCommand);
        Assert.Null(toolbar.ViewCommand);
        Assert.Null(toolbar.EditCommand);
        Assert.Null(toolbar.DeleteCommand);
        Assert.Null(toolbar.RefreshCommand);
    }

    /// <summary>
    /// Ensures that the CRUD factory enables the standard command buttons.
    /// </summary>
    [Fact]
    public void CreateCrud_ShouldEnableCrudCommandButtons()
    {
        XToolbarContext toolbar = XToolbarContext.CreateCrud();

        Assert.True(toolbar.ShowNewButton);
        Assert.False(toolbar.ShowViewButton);
        Assert.True(toolbar.ShowEditButton);
        Assert.True(toolbar.ShowDeleteButton);
        Assert.False(toolbar.ShowRefreshButton);
        Assert.False(toolbar.ShowViewModeSelector);
        Assert.True(toolbar.HasCommandButtons);
        Assert.True(toolbar.HasToolbarItems);
        Assert.False(toolbar.ShowCommandSeparator);
    }

    /// <summary>
    /// Ensures that the CRUD-with-view-mode factory enables commands and the view selector.
    /// </summary>
    [Fact]
    public void CreateCrudWithViewMode_ShouldEnableCommandsAndSelector()
    {
        XToolbarContext toolbar = XToolbarContext.CreateCrudWithViewMode();

        Assert.True(toolbar.ShowNewButton);
        Assert.True(toolbar.ShowEditButton);
        Assert.True(toolbar.ShowDeleteButton);
        Assert.True(toolbar.ShowViewModeSelector);
        Assert.Equal(XContentViewMode.Grid, toolbar.ViewMode);
        Assert.True(toolbar.HasCommandButtons);
        Assert.True(toolbar.HasToolbarItems);
        Assert.True(toolbar.ShowCommandSeparator);
    }

    /// <summary>
    /// Ensures that the view-mode factory exposes only the view selector.
    /// </summary>
    [Fact]
    public void CreateViewMode_ShouldEnableOnlyViewModeSelector()
    {
        XToolbarContext toolbar = XToolbarContext.CreateViewMode();

        Assert.True(toolbar.ShowViewModeSelector);
        Assert.Equal(XContentViewMode.Grid, toolbar.ViewMode);
        Assert.False(toolbar.HasCommandButtons);
        Assert.True(toolbar.HasToolbarItems);
        Assert.False(toolbar.ShowCommandSeparator);
    }

    /// <summary>
    /// Ensures that visible-item changes raise direct and derived property notifications.
    /// </summary>
    [Fact]
    public void VisibilityProperties_ShouldRaiseDirectAndDerivedNotifications()
    {
        XToolbarContext toolbar = new();
        List<string?> changedProperties = [];
        toolbar.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        toolbar.ShowNewButton = true;
        toolbar.ShowViewModeSelector = true;

        Assert.True(toolbar.HasCommandButtons);
        Assert.True(toolbar.HasToolbarItems);
        Assert.True(toolbar.ShowCommandSeparator);
        Assert.Contains(nameof(XToolbarContext.ShowNewButton), changedProperties);
        Assert.Contains(nameof(XToolbarContext.ShowViewModeSelector), changedProperties);
        Assert.True(changedProperties.Count(name => name == nameof(XToolbarContext.HasCommandButtons)) >= 2);
        Assert.True(changedProperties.Count(name => name == nameof(XToolbarContext.HasToolbarItems)) >= 2);
        Assert.True(changedProperties.Count(name => name == nameof(XToolbarContext.ShowCommandSeparator)) >= 2);
    }

    /// <summary>
    /// Ensures that command and state properties support round-trips and notifications.
    /// </summary>
    [Fact]
    public void CommandAndStateProperties_ShouldSupportRoundTrips()
    {
        XToolbarContext toolbar = new();
        TestCommand newCommand = new();
        TestCommand viewCommand = new();
        TestCommand editCommand = new();
        TestCommand deleteCommand = new();
        TestCommand refreshCommand = new();
        List<string?> changedProperties = [];
        toolbar.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        toolbar.NewCommand = newCommand;
        toolbar.ViewCommand = viewCommand;
        toolbar.EditCommand = editCommand;
        toolbar.DeleteCommand = deleteCommand;
        toolbar.RefreshCommand = refreshCommand;
        toolbar.IsRememberViewStateEnabled = true;
        toolbar.ViewMode = XContentViewMode.Tree;

        Assert.Same(newCommand, toolbar.NewCommand);
        Assert.Same(viewCommand, toolbar.ViewCommand);
        Assert.Same(editCommand, toolbar.EditCommand);
        Assert.Same(deleteCommand, toolbar.DeleteCommand);
        Assert.Same(refreshCommand, toolbar.RefreshCommand);
        Assert.True(toolbar.IsRememberViewStateEnabled);
        Assert.Equal(XContentViewMode.Tree, toolbar.ViewMode);
        Assert.Contains(nameof(XToolbarContext.NewCommand), changedProperties);
        Assert.Contains(nameof(XToolbarContext.ViewCommand), changedProperties);
        Assert.Contains(nameof(XToolbarContext.EditCommand), changedProperties);
        Assert.Contains(nameof(XToolbarContext.DeleteCommand), changedProperties);
        Assert.Contains(nameof(XToolbarContext.RefreshCommand), changedProperties);
        Assert.Contains(nameof(XToolbarContext.IsRememberViewStateEnabled), changedProperties);
        Assert.Contains(nameof(XToolbarContext.ViewMode), changedProperties);
    }
    #endregion

    #region ### Nested Types ###
    private sealed class TestCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
}
#endregion
