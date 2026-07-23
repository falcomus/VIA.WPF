// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridFilterDefinition.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Controls;

#region ### Class XDataGridFilterDefinition ###
/// <summary>
/// Describes a filterable column of an <see cref="XDataGrid"/>.
/// </summary>
public sealed class XDataGridFilterDefinition
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the property name of the column.
    /// </summary>
    public required string ColumnName { get; init; }

    /// <summary>
    /// Gets or sets the display name shown inside the filter popup.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the column filter is enabled.
    /// </summary>
    public bool IsEnabled { get; init; } = true;
    #endregion
}
#endregion