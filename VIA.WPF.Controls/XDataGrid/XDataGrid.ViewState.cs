// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.ViewState.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Private Methods (View State) ###
    private async Task<bool> RestorePersistedViewStateAsync()
    {
        if (this._isRestoringViewState)
        {
            return true;
        }

        if (!this.IsViewStatePersistenceEnabledForCurrentContext() ||
            this.DataContext is null ||
            this._itemsView is null)
        {
            return false;
        }

        XDataGridViewState? restoredState = await XViewStateService.Current
            .TryLoadDataGridStateAsync(this.GetEffectiveViewStateKey())
            .ConfigureAwait(true);

        if (restoredState is null || restoredState.Version != XDataGridViewState.CurrentVersion)
        {
            return false;
        }

        bool restoredSort = false;
        this._isRestoringViewState = true;

        try
        {
            this.SetSearchTermPreservingBinding(restoredState.SearchTerm ?? string.Empty);
            restoredSort = this.TryRestoreSortState(restoredState);
            this.RestoreFilterState(restoredState);
        }
        finally
        {
            this._isRestoringViewState = false;
        }

        return restoredSort;
    }

    private bool TryRestoreSortState(XDataGridViewState state)
    {
        if (this._itemsView is null)
        {
            return false;
        }

        List<XDataGridSortState> validSortStates = [.. state.SortDescriptions
            .Where(sortState => !string.IsNullOrWhiteSpace(sortState.PropertyName) && this.HasColumnForProperty(sortState.PropertyName))];

        if (validSortStates.Count == 0)
        {
            return false;
        }

        this.ApplySortDescriptions([.. validSortStates.Select(sortState => new SortDescription(sortState.PropertyName, sortState.Direction))]);

        return true;
    }

    private void RestoreFilterState(XDataGridViewState state)
    {
        this._activeFilterValuesCache.Clear();

        foreach (string columnKey in this._columnFilterItems.Keys.ToArray())
        {
            if (!state.ColumnFilters.ContainsKey(columnKey))
            {
                this.ClearColumnFilterSelection(columnKey);
            }
        }

        foreach (KeyValuePair<string, string[]> pair in state.ColumnFilters)
        {
            HashSet<string> selectedValues = pair.Value.ToHashSet(StringComparer.CurrentCulture);
            this._activeFilterValuesCache[pair.Key] = selectedValues;

            if (!this._columnFilterItems.TryGetValue(pair.Key, out ObservableCollection<XDataGridFilterItem>? items))
            {
                this.UpdateFilterButtonVisual(pair.Key);
                continue;
            }

            if (items.Count == 0)
            {
                this.InvalidateColumnFilterItems(pair.Key);
                this.UpdateFilterButtonVisual(pair.Key);
                continue;
            }

            this._isBulkUpdatingFilters = true;

            try
            {
                foreach (XDataGridFilterItem item in items)
                {
                    item.IsSelected = selectedValues.Contains(item.DisplayValue);
                }
            }
            finally
            {
                this._isBulkUpdatingFilters = false;
            }

            this.UpdateFilterButtonVisual(pair.Key);
        }

        this.UpdateActionColumnFilterButtonVisual();
    }

    private void StoreCurrentViewState()
    {
        if (!this.IsViewStatePersistenceEnabledForCurrentContext() || this.DataContext is null || this._itemsView is null || this._isRestoringViewState)
        {
            return;
        }

        _ = this.StoreCurrentViewStateSafeAsync();
    }

    private async Task StoreCurrentViewStateSafeAsync()
    {
        try
        {
            await XViewStateService.Current
                .SaveDataGridStateAsync(this.GetEffectiveViewStateKey(), this.CreateCurrentViewState())
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"[XDataGrid] Failed to store view state: {exception}");
        }
    }

    private void ScheduleViewStateStore()
    {
        if (!this.IsViewStatePersistenceEnabledForCurrentContext() || this.DataContext is null || this._itemsView is null || this._isRestoringViewState)
        {
            return;
        }

        this._viewStateStoreTimer.Stop();
        this._viewStateStoreTimer.Start();
    }

    private void ScheduleFilterRefresh()
    {
        this._filterRefreshTimer.Stop();
        this._filterRefreshTimer.Start();
    }

    private XDataGridViewState CreateCurrentViewState()
    {
        XDataGridViewState state = new();

        if (!string.IsNullOrWhiteSpace(this.SearchTerm))
        {
            state.SearchTerm = this.SearchTerm;
        }

        if (this._itemsView is not null)
        {
            foreach (SortDescription sortDescription in this._itemsView.SortDescriptions)
            {
                if (string.IsNullOrWhiteSpace(sortDescription.PropertyName))
                {
                    continue;
                }

                state.SortDescriptions.Add(new XDataGridSortState(sortDescription.PropertyName, sortDescription.Direction));
            }
        }

        foreach (KeyValuePair<string, ObservableCollection<XDataGridFilterItem>> pair in this._columnFilterItems)
        {
            ObservableCollection<XDataGridFilterItem> items = pair.Value;

            if (items.Count == 0)
            {
                continue;
            }

            bool allSelected = items.All(item => item.IsSelected);
            if (allSelected)
            {
                continue;
            }

            state.ColumnFilters[pair.Key] = [.. items
                .Where(item => item.IsSelected)
                .Select(item => item.DisplayValue)];
        }

        foreach (KeyValuePair<string, HashSet<string>?> pair in this._activeFilterValuesCache)
        {
            if (pair.Value is null || state.ColumnFilters.ContainsKey(pair.Key))
            {
                continue;
            }

            state.ColumnFilters[pair.Key] = [.. pair.Value];
        }

        return state;
    }

    private bool IsViewStatePersistenceEnabledForCurrentContext()
    {
        if (!this.PersistViewState)
        {
            return false;
        }

        if (this.DataContext is IXPageContext pageContext && pageContext.Toolbar.ShowRememberViewToggle)
        {
            return pageContext.Toolbar.IsRememberViewStateEnabled;
        }

        return true;
    }

    private string GetEffectiveViewStateKey()
    {
        return this._cachedViewStateKey ??= this.ComputeEffectiveViewStateKey();
    }

    private string ComputeEffectiveViewStateKey()
    {
        string dataContextKey = this.DataContext?.GetType().FullName ?? "NoDataContext";

        if (!string.IsNullOrWhiteSpace(this.ViewStateKey))
        {
            return $"XDataGrid|{dataContextKey}|{this.ViewStateKey.Trim()}";
        }

        string[] columnKeys = [.. this.Columns
            .Cast<System.Windows.Controls.DataGridColumn>()
            .Where(column => !this.IsActionColumn(column))
            .Select(this.GetColumnKey)
            .Where(columnKey => !string.IsNullOrWhiteSpace(columnKey))];

        string gridKey = columnKeys.Length == 0
            ? "Default"
            : string.Join("|", columnKeys);

        return $"XDataGrid|{dataContextKey}|{gridKey}";
    }
    #endregion
}

