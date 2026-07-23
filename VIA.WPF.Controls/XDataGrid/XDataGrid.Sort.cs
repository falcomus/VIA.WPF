// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.Sort.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Private Methods (Sort) ###
    private void RestorePersistedViewStateOrApplyInitialSort()
    {
        _ = this.RestorePersistedViewStateOrApplyInitialSortAsync();
    }

    private async Task RestorePersistedViewStateOrApplyInitialSortAsync()
    {
        try
        {
            bool restoredSort = await this.RestorePersistedViewStateAsync().ConfigureAwait(true);
            bool sortApplied = false;

            if (!restoredSort)
            {
                sortApplied = this.ApplyConfiguredInitialSort();
            }

            if (restoredSort || sortApplied)
            {
                this.RefreshFilters();
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"XDataGrid failed to restore persisted view state: {exception}");
        }
    }

    private bool ApplyConfiguredInitialSort()
    {
        this._itemsView ??= this.ResolveDisplayedItemsView();

        if (this._itemsView is null || this._itemsView.SortDescriptions.Count > 0)
        {
            return false;
        }

        SortDescription[] initialSortDescriptions = [.. this.Columns
            .Cast<DataGridColumn>()
            .Where(column => !this.IsActionColumn(column) && column.SortDirection.HasValue)
            .OrderBy(column => column.DisplayIndex)
            .Select(column =>
            {
                string columnKey = this.GetColumnKey(column);
                ListSortDirection direction = column.SortDirection == ListSortDirection.Descending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;

                return string.IsNullOrWhiteSpace(columnKey)
                    ? (SortDescription?)null
                    : new SortDescription(columnKey, direction);
            })
            .Where(sortDescription => sortDescription.HasValue)
            .Select(sortDescription => sortDescription!.Value)];

        if (initialSortDescriptions.Length == 0)
        {
            return false;
        }

        this.ApplySortDescriptions(initialSortDescriptions);
        return true;
    }

    private bool TryApplyUserSort(DataGridColumn column)
    {
        if (this._isApplyingSort || this.IsActionColumn(column) || !this.CanUserSortColumns || !column.CanUserSort)
        {
            return false;
        }

        string columnKey = this.GetColumnKey(column);
        if (string.IsNullOrWhiteSpace(columnKey))
        {
            return false;
        }

        ListSortDirection direction = column.SortDirection == ListSortDirection.Ascending
            ? ListSortDirection.Descending
            : ListSortDirection.Ascending;

        this.ApplySortDescriptions([new SortDescription(columnKey, direction)]);
        return true;
    }

    private void ApplySortDescriptions(IReadOnlyCollection<SortDescription> sortDescriptions)
    {
        this._itemsView ??= this.ResolveDisplayedItemsView();

        if (this._itemsView is null)
        {
            return;
        }

        this._isApplyingSort = true;

        try
        {
            foreach (DataGridColumn column in this.Columns.Cast<DataGridColumn>().Where(column => !this.IsActionColumn(column)))
            {
                column.SortDirection = null;
            }

            using (this._itemsView.DeferRefresh())
            {
                this._itemsView.SortDescriptions.Clear();

                foreach (SortDescription sortDescription in sortDescriptions.Where(sortDescription => !string.IsNullOrWhiteSpace(sortDescription.PropertyName)))
                {
                    this._itemsView.SortDescriptions.Add(sortDescription);

                    DataGridColumn? column = this.Columns
                        .Cast<DataGridColumn>()
                        .FirstOrDefault(candidate => !this.IsActionColumn(candidate) && IsSamePropertyPath(this.GetColumnKey(candidate), sortDescription.PropertyName));

                    if (column is not null)
                    {
                        column.SortDirection = sortDescription.Direction;
                    }
                }
            }
        }
        finally
        {
            this._isApplyingSort = false;
        }
    }

    private bool HasColumnForProperty(string propertyName)
    {
        return this.Columns
            .Cast<DataGridColumn>()
            .Where(column => !this.IsActionColumn(column))
            .Select(this.GetColumnKey)
            .Any(columnKey => IsSamePropertyPath(columnKey, propertyName));
    }
    #endregion
}
