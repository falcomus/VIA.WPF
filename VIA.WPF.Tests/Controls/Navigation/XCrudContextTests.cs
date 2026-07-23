// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCrudContextTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class XCrudContextTests ###
/// <summary>
/// Provides tests for <see cref="XCrudContext" />.
/// </summary>
public sealed class XCrudContextTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that a new CRUD context exposes the expected defaults.
    /// </summary>
    [Fact]
    public void Constructor_ShouldExposeExpectedDefaults()
    {
        XCrudContext context = new();

        Assert.Null(context.SelectedItem);
        Assert.False(context.HasSelectedItem);
        Assert.Null(context.Editor);
        Assert.Equal(string.Empty, context.Title);
        Assert.Equal(XCrudMode.None, context.Mode);
        Assert.False(context.IsOpen);
        Assert.False(context.IsViewMode);
        Assert.False(context.IsEditMode);
        Assert.False(context.IsCreateMode);
        Assert.False(context.IsReadOnly);
        Assert.False(context.IsEditable);
        Assert.NotNull(context.CloseCommand);
        Assert.Same(context.CloseCommand, context.CancelCommand);
    }

    /// <summary>
    /// Ensures that selection changes update the derived selection flag.
    /// </summary>
    [Fact]
    public void SelectedItem_ShouldUpdateHasSelectedItemAndRaiseNotifications()
    {
        XCrudContext context = new();
        object selectedItem = new();
        List<string?> changedProperties = [];
        context.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        context.SelectedItem = selectedItem;

        Assert.Same(selectedItem, context.SelectedItem);
        Assert.True(context.HasSelectedItem);
        Assert.Contains(nameof(XCrudContext.SelectedItem), changedProperties);
        Assert.Contains(nameof(XCrudContext.HasSelectedItem), changedProperties);
    }

    /// <summary>
    /// Ensures that opening in edit mode updates detail state and mode flags.
    /// </summary>
    [Fact]
    public void Open_ShouldSetDetailStateAndModeFlags()
    {
        XCrudContext context = new();
        object editor = new();

        context.Open(XCrudMode.Edit, editor, "Edit item");

        Assert.True(context.IsOpen);
        Assert.Same(editor, context.Editor);
        Assert.Equal("Edit item", context.Title);
        Assert.Equal(XCrudMode.Edit, context.Mode);
        Assert.False(context.IsViewMode);
        Assert.True(context.IsEditMode);
        Assert.False(context.IsCreateMode);
        Assert.False(context.IsReadOnly);
        Assert.True(context.IsEditable);
    }

    /// <summary>
    /// Ensures that a null title is normalized to an empty title.
    /// </summary>
    [Fact]
    public void Open_ShouldNormalizeNullTitleToEmptyString()
    {
        XCrudContext context = new();

        context.Open(XCrudMode.Create, editor: null, title: null);

        Assert.Equal(string.Empty, context.Title);
        Assert.True(context.IsCreateMode);
        Assert.True(context.IsEditable);
    }

    /// <summary>
    /// Ensures that close resets active editor state but keeps the selection unchanged.
    /// </summary>
    [Fact]
    public void Close_ShouldResetDetailStateAndKeepSelection()
    {
        XCrudContext context = new();
        object selectedItem = new();
        object editor = new();
        context.SelectedItem = selectedItem;
        context.Open(XCrudMode.View, editor, "View item");

        context.Close();

        Assert.False(context.IsOpen);
        Assert.Null(context.Editor);
        Assert.Equal(string.Empty, context.Title);
        Assert.Equal(XCrudMode.None, context.Mode);
        Assert.Same(selectedItem, context.SelectedItem);
        Assert.False(context.IsReadOnly);
        Assert.False(context.IsEditable);
    }

    /// <summary>
    /// Ensures that the close command invokes the close logic.
    /// </summary>
    [Fact]
    public void CloseCommand_ShouldCloseContext()
    {
        XCrudContext context = new();
        context.Open(XCrudMode.Edit, new object(), "Edit item");

        context.CloseCommand.Execute(null);

        Assert.False(context.IsOpen);
        Assert.Null(context.Editor);
        Assert.Equal(XCrudMode.None, context.Mode);
    }

    /// <summary>
    /// Ensures that clearing the selection resets selected item state.
    /// </summary>
    [Fact]
    public void ClearSelection_ShouldResetSelectedItem()
    {
        XCrudContext context = new()
        {
            SelectedItem = new object()
        };

        context.ClearSelection();

        Assert.Null(context.SelectedItem);
        Assert.False(context.HasSelectedItem);
    }

    /// <summary>
    /// Ensures that mode changes raise derived mode notifications.
    /// </summary>
    [Fact]
    public void Mode_ShouldRaiseDerivedModeNotifications()
    {
        XCrudContext context = new();
        List<string?> changedProperties = [];
        context.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        context.Mode = XCrudMode.View;

        Assert.True(context.IsViewMode);
        Assert.True(context.IsReadOnly);
        Assert.Contains(nameof(XCrudContext.Mode), changedProperties);
        Assert.Contains(nameof(XCrudContext.IsViewMode), changedProperties);
        Assert.Contains(nameof(XCrudContext.IsEditMode), changedProperties);
        Assert.Contains(nameof(XCrudContext.IsCreateMode), changedProperties);
        Assert.Contains(nameof(XCrudContext.IsReadOnly), changedProperties);
        Assert.Contains(nameof(XCrudContext.IsEditable), changedProperties);
    }
    #endregion
}
#endregion
