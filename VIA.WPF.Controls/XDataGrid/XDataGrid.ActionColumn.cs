// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.ActionColumn.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Internal Methods (Action Column / Filter Button) ###
    /// <summary>
    /// Handles a filter button click for the specified column.
    /// </summary>
    /// <param name="column">The column whose header button was clicked.</param>
    /// <param name="popup">The filter popup of the column header.</param>
    /// <param name="button">The clicked filter button.</param>
    internal void HandleColumnHeaderFilterButtonClick(DataGridColumn column, System.Windows.Controls.Primitives.Popup? popup, Button button)
    {
        this.PrepareColumnHeaderFilterButton(column, button);

        if (this.IsActionColumn(column))
        {
            this.CloseCurrentPopup();
            this.ClearAllFilters();
            return;
        }

        if (popup is null)
        {
            return;
        }

        this.OpenFilter(column, popup, button);
    }

    /// <summary>
    /// Opens the filter popup for the specified column.
    /// </summary>
    /// <param name="column">The column whose filter should be opened.</param>
    /// <param name="popup">The filter popup of the column header.</param>
    /// <param name="button">The clicked filter button.</param>
    internal void OpenFilter(DataGridColumn column, System.Windows.Controls.Primitives.Popup popup, Button button)
    {
        _ = this.OpenFilterAsync(column, popup, button);
    }

    private async Task OpenFilterAsync(DataGridColumn column, System.Windows.Controls.Primitives.Popup popup, Button button)
    {
        try
        {
            string columnKey = this.GetColumnKey(column);
            if (string.IsNullOrWhiteSpace(columnKey))
            {
                return;
            }

            this.EnsureDefinitionsAndSearchableColumns();

            if (this._currentPopup is not null && this._currentPopup != popup)
            {
                this.CloseCurrentPopup();
            }

            this._currentFilterColumnKey = columnKey;
            this._currentPopup = popup;
            this._currentPopupButton = button;

            popup.Closed -= this.OnCurrentPopupClosed;
            popup.Closed += this.OnCurrentPopupClosed;
            popup.DataContext = this;

            if (popup.Child is FrameworkElement popupChild)
            {
                popupChild.DataContext = this;
                popupChild.PreviewKeyDown -= this.OnCurrentPopupPreviewKeyDown;
                popupChild.PreviewKeyDown += this.OnCurrentPopupPreviewKeyDown;
            }

            popup.PlacementTarget = button;
            popup.StaysOpen = false;
            popup.IsOpen = true;

            this._filterButtons[columnKey] = button;
            this.UpdateFilterButtonVisual(columnKey);

            if (this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? cachedItems))
            {
                this.CurrentFilterItems = cachedItems;
                this.IsCurrentFilterLoading = false;
                this.FocusCurrentFilterPopup();
                return;
            }

            this.CurrentFilterItems = [];
            ObservableCollection<XDataGridFilterItem>? loadedItems = await this.EnsureColumnFilterItemsAsync(columnKey).ConfigureAwait(true);

            if (loadedItems is not null && this._currentFilterColumnKey == columnKey && ReferenceEquals(this._currentPopup, popup))
            {
                this.CurrentFilterItems = loadedItems;
                this.FocusCurrentFilterPopup();
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"XDataGrid failed to open filter popup: {exception}");
            this.IsCurrentFilterLoading = false;
        }
    }

    /// <summary>
    /// Prepares the column header filter button for the specified column.
    /// </summary>
    /// <param name="column">The column whose header owns the button.</param>
    /// <param name="button">The filter button.</param>
    internal void PrepareColumnHeaderFilterButton(DataGridColumn column, Button button)
    {
        if (this.IsActionColumn(column))
        {
            this._actionColumnFilterButton = button;
            this.UpdateActionColumnFilterButtonVisual();
            return;
        }

        string columnKey = this.GetColumnKey(column);
        if (!string.IsNullOrWhiteSpace(columnKey))
        {
            this._filterButtons[columnKey] = button;
            this.UpdateFilterButtonVisual(columnKey);
        }

        button.ToolTip = "Filter";
    }

    internal bool IsColumnSearchable(DataGridColumn column)
    {
        string columnKey = this.GetColumnKey(column);

        return !string.IsNullOrWhiteSpace(columnKey)
            && this.SearchableColumns.Contains(columnKey);
    }
    #endregion

    #region ### Private Methods (Action Column) ###
    private bool ShouldShowActionColumn()
    {
        return this.ShowActionColumn && (this.ShowViewButton || this.ShowEditButton || this.ShowDeleteButton);
    }

    private bool IsActionColumn(DataGridColumn column)
    {
        return this._actionColumn is not null && ReferenceEquals(column, this._actionColumn);
    }

    /// <summary>
    /// Resolves the action column cell template from the grid resources or from the applied style resources.
    /// </summary>
    /// <returns>The resolved action cell template, or <c>null</c> if no template is available.</returns>
    private DataTemplate? ResolveActionColumnCellTemplate()
    {
        if (this.TryFindResource(ActionColumnCellTemplateResourceKey) is DataTemplate template)
        {
            return template;
        }

        return this.Style?.Resources[ActionColumnCellTemplateResourceKey] as DataTemplate;
    }


    private void EnsureActionColumn()
    {
        if (!this.ShouldShowActionColumn())
        {
            this.RemoveActionColumn();
            return;
        }

        DataTemplate? actionCellTemplate = this.ResolveActionColumnCellTemplate();

        if (actionCellTemplate is null)
        {
            return;
        }

        this._isUpdatingActionColumn = true;

        try
        {
            this._actionColumn ??= new DataGridTemplateColumn
            {
                CanUserReorder = false,
                CanUserResize = false,
                CanUserSort = false,
                IsReadOnly = true
            };

            this._actionColumn.CellTemplate = actionCellTemplate;
            this._actionColumn.Header = this.ActionColumnHeader ?? this.ResetAllFiltersButtonContent ?? string.Empty;
            SetIsGeneratedActionColumn(this._actionColumn, true);
            this._actionColumn.Width = this.ActionColumnWidth;
            this._actionColumn.MinWidth = this.ActionColumnMinWidth;
            this._actionColumn.MaxWidth = this.ActionColumnMaxWidth;

            int actionColumnIndex = this.Columns.IndexOf(this._actionColumn);

            if (actionColumnIndex < 0)
            {
                this.Columns.Add(this._actionColumn);
                return;
            }

            if (actionColumnIndex != this.Columns.Count - 1)
            {
                this.Columns.Remove(this._actionColumn);
                this.Columns.Add(this._actionColumn);
            }
        }
        finally
        {
            this._isUpdatingActionColumn = false;
        }
    }

    private void RemoveActionColumn()
    {
        if (this._actionColumn is null || !this.Columns.Contains(this._actionColumn))
        {
            return;
        }

        this._isUpdatingActionColumn = true;

        try
        {
            SetIsGeneratedActionColumn(this._actionColumn, false);
            this._actionColumn.Header = null;
            this.Columns.Remove(this._actionColumn);
        }
        finally
        {
            this._isUpdatingActionColumn = false;
        }
    }

    /// <summary>
    /// Updates the visual state of the action column filter button.
    /// </summary>
    private void UpdateActionColumnFilterButtonVisual()
    {
        if (this._actionColumnFilterButton is null)
        {
            return;
        }

        bool filterActive = this.HasAnyActiveFilter();

        this._actionColumnFilterButton.Opacity = filterActive ? 1d : 0.65d;
        this._actionColumnFilterButton.FontWeight = filterActive ? FontWeights.SemiBold : FontWeights.Normal;
        this._actionColumnFilterButton.ToolTip = filterActive ? "Clear all filters" : "No active filters";
        this._actionColumnFilterButton.SetResourceReference(
            Control.ForegroundProperty,
            filterActive ? XBrushKeys.Primary : XBrushKeys.BorderText);
    }

    private void UpdateFilterButtonVisual(string columnKey)
    {
        if (!this._filterButtons.TryGetValue(columnKey, out Button? button))
        {
            return;
        }

        if (!this._columnFilterItems.TryGetValue(columnKey, out System.Collections.ObjectModel.ObservableCollection<XDataGridFilterItem>? items) || items.Count == 0)
        {
            bool cachedFilterActive = this.IsColumnFilterActive(columnKey);

            button.Opacity = cachedFilterActive ? 1d : 0.65d;
            button.FontWeight = cachedFilterActive ? FontWeights.SemiBold : FontWeights.Normal;
            button.ToolTip = cachedFilterActive ? "Filter active" : "Filter";
            button.SetResourceReference(Control.ForegroundProperty, cachedFilterActive ? XBrushKeys.Primary : XBrushKeys.BorderText);
            return;
        }

        bool allSelected = items.All(item => item.IsSelected);
        bool filterActive = !allSelected;

        button.Opacity = filterActive ? 1d : 0.65d;
        button.FontWeight = filterActive ? FontWeights.SemiBold : FontWeights.Normal;
        button.ToolTip = filterActive ? "Filter active" : "Filter";
        button.SetResourceReference(Control.ForegroundProperty, filterActive ? XBrushKeys.Primary : XBrushKeys.BorderText);
    }

    private IEnumerable<string> GetKnownFilterColumnKeys()
    {
        return this._columnFilterItems.Keys
            .Concat(this._activeFilterValuesCache.Keys)
            .Concat(this._filterButtons.Keys)
            .Concat(this.FilterDefinitions.Where(definition => definition.IsEnabled).Select(definition => definition.ColumnName))
            .Where(columnKey => !string.IsNullOrWhiteSpace(columnKey))
            .Distinct(StringComparer.Ordinal);
    }
    #endregion
}
