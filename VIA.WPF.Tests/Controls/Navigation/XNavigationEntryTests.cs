// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationEntryTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Tests.Controls.Navigation;

#region ### Class XNavigationEntryTests ###
/// <summary>
/// Provides tests for navigation entry state objects.
/// </summary>
public sealed class XNavigationEntryTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that a new entry exposes the expected defaults.
    /// </summary>
    [Fact]
    public void Entry_ShouldExposeExpectedDefaults()
    {
        XNavigationEntry entry = new();

        Assert.Equal(string.Empty, entry.Title);
        Assert.Equal(string.Empty, entry.Description);
        Assert.Null(entry.Value);
        Assert.Null(entry.Icon);
        Assert.True(entry.IsEnabled);
        Assert.True(entry.IsVisible);
        Assert.Empty(entry.Children);
    }

    /// <summary>
    /// Ensures that the title/value constructor initializes the matching properties.
    /// </summary>
    [Fact]
    public void Entry_Constructor_ShouldInitializeTitleAndValue()
    {
        XNavigationEntry entry = new("Title", "Value");

        Assert.Equal("Title", entry.Title);
        Assert.Equal("Value", entry.Value);
        Assert.Empty(entry.Children);
    }

    /// <summary>
    /// Ensures that entry property changes raise notifications.
    /// </summary>
    [Fact]
    public void Entry_SetProperties_ShouldRaisePropertyChanged()
    {
        XNavigationEntry entry = new();
        List<string?> changedProperties = [];
        object icon = new();
        entry.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        entry.Title = "Title";
        entry.Description = "Description";
        entry.Value = "Value";
        entry.Icon = icon;
        entry.IsEnabled = false;
        entry.IsVisible = false;

        Assert.Equal("Title", entry.Title);
        Assert.Equal("Description", entry.Description);
        Assert.Equal("Value", entry.Value);
        Assert.Same(icon, entry.Icon);
        Assert.False(entry.IsEnabled);
        Assert.False(entry.IsVisible);
        Assert.Contains(nameof(XNavigationEntry.Title), changedProperties);
        Assert.Contains(nameof(XNavigationEntry.Description), changedProperties);
        Assert.Contains(nameof(XNavigationEntry.Value), changedProperties);
        Assert.Contains(nameof(XNavigationEntry.Icon), changedProperties);
        Assert.Contains(nameof(XNavigationEntry.IsEnabled), changedProperties);
        Assert.Contains(nameof(XNavigationEntry.IsVisible), changedProperties);
    }

    /// <summary>
    /// Ensures that the side content wrapper stores entries and selects the first entry by default.
    /// </summary>
    [Fact]
    public void SideContent_Constructor_ShouldCopyItemsAndSelectFirstItem()
    {
        XNavigationEntry first = new("First", "first");
        XNavigationEntry second = new("Second", "second");

        XNavigationListSideContent sideContent = new([first, second]);

        Assert.Equal(2, sideContent.Items.Count);
        Assert.Same(first, sideContent.Items[0]);
        Assert.Same(second, sideContent.Items[1]);
        Assert.Same(first, sideContent.SelectedItem);
    }

    /// <summary>
    /// Ensures that side content selection changes raise notifications.
    /// </summary>
    [Fact]
    public void SideContent_SelectedItem_ShouldRaisePropertyChanged()
    {
        XNavigationEntry first = new("First", "first");
        XNavigationEntry second = new("Second", "second");
        XNavigationListSideContent sideContent = new([first, second]);
        List<string?> changedProperties = [];
        sideContent.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        sideContent.SelectedItem = second;

        Assert.Same(second, sideContent.SelectedItem);
        Assert.Contains(nameof(XNavigationListSideContent.SelectedItem), changedProperties);
    }
    #endregion
}
#endregion
