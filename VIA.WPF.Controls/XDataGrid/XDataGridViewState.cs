// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridViewState.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace VIA.WPF.Controls;

#region ### Class XDataGridViewState ###
/// <summary>
/// Stores the persisted view state of an <see cref="XDataGrid"/>.
/// </summary>
public sealed class XDataGridViewState
{
    #region ### Public Constants ###
    /// <summary>
    /// Defines the current persisted view-state format version.
    /// </summary>
    public const int CurrentVersion = 1;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the persisted view-state format version.
    /// </summary>
    public int Version { get; set; } = CurrentVersion;

    /// <summary>
    /// Gets or sets the persisted free text search term.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets the persisted sort descriptions.
    /// </summary>
    public List<XDataGridSortState> SortDescriptions { get; set; } = [];

    /// <summary>
    /// Gets the persisted column filters.
    /// </summary>
    public Dictionary<string, string[]> ColumnFilters { get; set; } = [];
    #endregion
}
#endregion

#region ### Class XDataGridSortState ###
/// <summary>
/// Stores one persisted sort description of an <see cref="XDataGrid"/>.
/// </summary>
public sealed class XDataGridSortState
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridSortState"/> class.
    /// </summary>
    public XDataGridSortState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridSortState"/> class.
    /// </summary>
    /// <param name="propertyName">The sorted property name.</param>
    /// <param name="direction">The sort direction.</param>
    public XDataGridSortState(string propertyName, ListSortDirection direction)
    {
        this.PropertyName = propertyName;
        this.Direction = direction;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the sorted property name.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sort direction.
    /// </summary>
    public ListSortDirection Direction { get; set; }
    #endregion
}
#endregion
