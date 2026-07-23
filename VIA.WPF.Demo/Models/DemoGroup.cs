// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.Models;

#region ### Class DemoGroup ###
/// <summary>
/// Represents a group of showcase items in the navigation.
/// </summary>
public sealed partial class DemoGroup : ObservableObject
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the group title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether this entry represents all demo groups.
    /// </summary>
    public bool IsAll { get; init; }

    /// <summary>
    /// Gets the number of controls contained in this navigation group.
    /// </summary>
    public int Count => this.Items.Count;

    /// <summary>
    /// Gets or sets a value indicating whether the navigation group is expanded.
    /// </summary>
    [ObservableProperty]
    private bool _isExpanded = true;

    /// <summary>
    /// Gets the contained demo items.
    /// </summary>
    public ObservableCollection<DemoItem> Items { get; } = [];
    #endregion
}
#endregion
