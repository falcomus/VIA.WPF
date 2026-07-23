// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.ColumnDisplay.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XDataGrid ###
/// <content>
/// Contains column display mode handling for <see cref="XDataGrid" />.
/// </content>
public partial class XDataGrid
{
    #region ### Private Methods (Column Display) ###
    /// <summary>
    /// Applies the current column display mode to all managed columns.
    /// </summary>
    private void ApplyColumnDisplayMode()
    {
        XDataGridColumnDisplayMode displayMode = this.ColumnDisplayMode;

        foreach (DataGridColumn column in this.Columns)
        {
            this.ApplyColumnDisplayMode(column, displayMode);
        }
    }

    /// <summary>
    /// Applies the specified display mode to a single column.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="displayMode">The active display mode.</param>
    private void ApplyColumnDisplayMode(DataGridColumn column, XDataGridColumnDisplayMode displayMode)
    {
        SetColumnDisplayOwner(column, this);

        if (GetColumnDisplayBaseVisibility(column) is not Visibility baseVisibility)
        {
            baseVisibility = column.Visibility;
            SetColumnDisplayBaseVisibility(column, baseVisibility);
        }

        if (this.IsActionColumn(column))
        {
            this.SetColumnVisibility(column, baseVisibility);
            return;
        }

        if (baseVisibility != Visibility.Visible)
        {
            this.SetColumnVisibility(column, baseVisibility);
            return;
        }

        XDataGridColumnDisplayModes columnDisplayModes = GetColumnDisplayModes(column);
        Visibility targetVisibility = this.IsColumnVisibleInDisplayMode(columnDisplayModes, displayMode)
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.SetColumnVisibility(column, targetVisibility);
    }

    /// <summary>
    /// Determines whether the specified column display mode flags contain the active display mode.
    /// </summary>
    /// <param name="columnDisplayModes">The configured column display modes.</param>
    /// <param name="displayMode">The active display mode.</param>
    /// <returns><c>true</c> if the column should be visible; otherwise, <c>false</c>.</returns>
    private bool IsColumnVisibleInDisplayMode(XDataGridColumnDisplayModes columnDisplayModes, XDataGridColumnDisplayMode displayMode)
    {
        return displayMode switch
        {
            XDataGridColumnDisplayMode.Compact => columnDisplayModes.HasFlag(XDataGridColumnDisplayModes.Compact),
            _ => columnDisplayModes.HasFlag(XDataGridColumnDisplayModes.Full)
        };
    }

    /// <summary>
    /// Sets the column visibility only when the value actually changes.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="visibility">The target visibility.</param>
    private void SetColumnVisibility(DataGridColumn column, Visibility visibility)
    {
        if (column.Visibility != visibility)
        {
            column.Visibility = visibility;
        }
    }
    #endregion
}
#endregion
