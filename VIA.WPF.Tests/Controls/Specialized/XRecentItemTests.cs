// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRecentItemTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Controls;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XRecentItemTests ###
/// <summary>
/// Provides tests for the default recent item model.
/// </summary>
public sealed class XRecentItemTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that property changes are raised only when values actually change.
    /// </summary>
    [Fact]
    public void Properties_ShouldRaisePropertyChangedWhenValueChanges()
    {
        XRecentItem item = new();
        List<string?> changedProperties = [];
        object data = new();

        item.PropertyChanged += (_, args) => changedProperties.Add(args.PropertyName);

        item.Id = 12;
        item.Text = "Recent";
        item.Description = "Description";
        item.Icon = "Icon";
        item.ToolTip = "Tooltip";
        item.Data = data;
        item.IsPinned = true;
        item.IsPinned = true;

        Assert.Equal(7, changedProperties.Count);
        Assert.Contains(nameof(XRecentItem.Id), changedProperties);
        Assert.Contains(nameof(XRecentItem.Text), changedProperties);
        Assert.Contains(nameof(XRecentItem.Description), changedProperties);
        Assert.Contains(nameof(XRecentItem.Icon), changedProperties);
        Assert.Contains(nameof(XRecentItem.ToolTip), changedProperties);
        Assert.Contains(nameof(XRecentItem.Data), changedProperties);
        Assert.Contains(nameof(XRecentItem.IsPinned), changedProperties);
        Assert.Same(data, item.Data);
    }

    /// <summary>
    /// Ensures that ToString returns the display text when available.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnTextWhenTextIsSet()
    {
        XRecentItem item = new()
        {
            Text = "Recent item"
        };

        Assert.Equal("Recent item", item.ToString());
    }
    #endregion
}
#endregion
