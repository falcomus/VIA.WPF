// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.Filter.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

public partial class XDataGrid
{
    #region ### Public Methods (Filter) ###
    /// <summary>
    /// Refreshes the current filters and the collection view.
    /// </summary>
    public void RefreshFilters()
    {
        ICollectionView? currentView = this.ResolveDisplayedItemsView();
        if (!ReferenceEquals(this._itemsView, currentView))
        {
            if (this._itemsView is not null)
            {
                this._itemsView.Filter = null;
            }

            this._itemsView = currentView;
        }

        this.UpdateFilterRuntimeCaches();

        if (this._itemsView is not null)
        {
            Predicate<object>? newFilter = this.HasAnyActiveFilter() ? this._filterPredicate : null;
            bool filterChanged = !ReferenceEquals(this._itemsView.Filter, newFilter);

            if (filterChanged)
            {
                this._itemsView.Filter = newFilter;
            }
            else if (newFilter is not null)
            {
                this._itemsView.Refresh();
            }
        }

        foreach (string columnKey in this.GetKnownFilterColumnKeys())
        {
            this.UpdateFilterButtonVisual(columnKey);
        }

        this.UpdateActionColumnFilterButtonVisual();
    }

    /// <summary>
    /// Clears all column filters and the free text search term.
    /// </summary>
    public void ClearAllFilters()
    {
        this._isBulkUpdatingFilters = true;

        try
        {
            this.SetSearchTermPreservingBinding(string.Empty);
            this._activeFilterValuesCache.Clear();

            foreach (XDataGridFilterDefinition definition in this.FilterDefinitions.Where(definition => definition.IsEnabled))
            {
                this._activeFilterValuesCache[definition.ColumnName] = null;
            }

            foreach (KeyValuePair<string, ObservableCollection<XDataGridFilterItem>> pair in this._columnFilterItems)
            {
                foreach (XDataGridFilterItem item in pair.Value)
                {
                    item.IsSelected = true;
                }

                this.UpdateActiveFilterCache(pair.Key);
            }
        }
        finally
        {
            this._isBulkUpdatingFilters = false;
        }

        this.RefreshFilters();
        this.ScheduleViewStateStore();
    }
    #endregion

    #region ### Private Methods (Filter Items) ###
    private void InvalidateAllColumnFilterItems()
    {
        foreach (string columnKey in this._columnFilterItems.Keys.ToArray())
        {
            this.InvalidateColumnFilterItems(columnKey);
        }

        foreach (XDataGridFilterDefinition definition in this.FilterDefinitions.Where(definition => definition.IsEnabled))
        {
            if (!this._activeFilterValuesCache.ContainsKey(definition.ColumnName))
            {
                this._activeFilterValuesCache[definition.ColumnName] = null;
            }

            this.UpdateFilterButtonVisual(definition.ColumnName);
        }

        this.UpdateActionColumnFilterButtonVisual();
    }

    private void InvalidateColumnFilterItems(string columnKey)
    {
        if (this._currentFilterColumnKey == columnKey)
        {
            this.CancelCurrentFilterItemsLoad();
        }

        if (!this._columnFilterItems.Remove(columnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            if (this._currentFilterColumnKey == columnKey)
            {
                this.CurrentFilterItems = [];
            }

            return;
        }

        foreach (XDataGridFilterItem item in items)
        {
            this.UnsubscribeFilterItem(item);
        }

        if (this._currentFilterColumnKey == columnKey)
        {
            this.CurrentFilterItems = [];
        }
    }

    private void ClearColumnFilterSelection(string columnKey)
    {
        this._activeFilterValuesCache[columnKey] = null;

        if (!this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            this.UpdateFilterButtonVisual(columnKey);
            return;
        }

        this._isBulkUpdatingFilters = true;

        try
        {
            foreach (XDataGridFilterItem item in items)
            {
                item.IsSelected = true;
            }
        }
        finally
        {
            this._isBulkUpdatingFilters = false;
        }

        this.UpdateActiveFilterCache(columnKey);
        this.UpdateFilterButtonVisual(columnKey);
    }

    private Task<ObservableCollection<XDataGridFilterItem>?> EnsureColumnFilterItemsAsync(string columnKey, bool forceReload = false)
    {
        if (!forceReload && this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? cachedItems))
        {
            return Task.FromResult<ObservableCollection<XDataGridFilterItem>?>(cachedItems);
        }

        Dictionary<string, bool> previousSelection = this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? previousItems)
            ? previousItems.ToDictionary(item => item.DisplayValue, item => item.IsSelected)
            : [];

        this._activeFilterValuesCache.TryGetValue(columnKey, out HashSet<string>? previouslySelectedValues);

        List<object?> sourceItems = this.CreateFilterSourceSnapshot();
        int loadVersion = this.BeginFilterItemsLoad(columnKey);
        CancellationToken cancellationToken = this._filterItemsLoadCancellationTokenSource?.Token ?? CancellationToken.None;

        return this.EnsureColumnFilterItemsAsyncCore(columnKey, previousItems, previousSelection, previouslySelectedValues, sourceItems, loadVersion, cancellationToken);
    }

    private async Task<ObservableCollection<XDataGridFilterItem>?> EnsureColumnFilterItemsAsyncCore(
        string columnKey,
        ObservableCollection<XDataGridFilterItem>? previousItems,
        Dictionary<string, bool> previousSelection,
        HashSet<string>? previouslySelectedValues,
        List<object?> sourceItems,
        int loadVersion,
        CancellationToken cancellationToken)
    {
        try
        {
            List<XDataGridFilterItem> createdItems = sourceItems.Any(item => item is DispatcherObject)
                ? this.CreateColumnFilterItems(columnKey, sourceItems, previousSelection, previouslySelectedValues, cancellationToken)
                : await Task.Run(
                    () => this.CreateColumnFilterItems(columnKey, sourceItems, previousSelection, previouslySelectedValues, cancellationToken),
                    cancellationToken).ConfigureAwait(true);

            if (cancellationToken.IsCancellationRequested || loadVersion != this._filterItemsLoadVersion)
            {
                return null;
            }

            if (previousItems is not null)
            {
                foreach (XDataGridFilterItem item in previousItems)
                {
                    this.UnsubscribeFilterItem(item);
                }
            }

            ObservableCollection<XDataGridFilterItem> items = [];
            foreach (XDataGridFilterItem item in createdItems)
            {
                this.SubscribeFilterItem(item, columnKey);
                items.Add(item);
            }

            this._columnFilterItems[columnKey] = items;
            this.UpdateActiveFilterCache(columnKey);

            if (this._currentFilterColumnKey == columnKey)
            {
                this.CurrentFilterItems = items;
                this.UpdateFilterButtonVisual(columnKey);
            }

            return items;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"XDataGrid failed to create filter items for column '{columnKey}': {exception}");
            return null;
        }
        finally
        {
            if (loadVersion == this._filterItemsLoadVersion)
            {
                this.IsCurrentFilterLoading = false;
            }
        }
    }

    private int BeginFilterItemsLoad(string columnKey)
    {
        this.CancelCurrentFilterItemsLoad();
        this._filterItemsLoadCancellationTokenSource = new CancellationTokenSource();
        this.IsCurrentFilterLoading = this._currentFilterColumnKey == columnKey;

        return ++this._filterItemsLoadVersion;
    }

    private void CancelCurrentFilterItemsLoad()
    {
        CancellationTokenSource? cancellationTokenSource = this._filterItemsLoadCancellationTokenSource;
        if (cancellationTokenSource is null)
        {
            return;
        }

        this._filterItemsLoadCancellationTokenSource = null;
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        this.IsCurrentFilterLoading = false;
    }

    private List<object?> CreateFilterSourceSnapshot()
    {
        if (this._rawItemsSource is not IEnumerable source)
        {
            return [];
        }

        try
        {
            return [.. source.Cast<object?>()];
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"XDataGrid failed to create filter source snapshot: {exception}");
            return [];
        }
    }

    private List<XDataGridFilterItem> CreateColumnFilterItems(
        string columnKey,
        IEnumerable<object?> sourceItems,
        IReadOnlyDictionary<string, bool> previousSelection,
        HashSet<string>? previouslySelectedValues,
        CancellationToken cancellationToken)
    {
        Dictionary<string, object?> valuesByDisplayValue = new(StringComparer.CurrentCulture);

        foreach (object? sourceItem in sourceItems)
        {
            cancellationToken.ThrowIfCancellationRequested();

            object? value = this.GetPropertyValue(sourceItem, columnKey);
            string displayValue = value?.ToString() ?? string.Empty;

            valuesByDisplayValue.TryAdd(displayValue, value);
        }

        List<XDataGridFilterItem> items = [];

        foreach (KeyValuePair<string, object?> pair in valuesByDisplayValue.OrderBy(pair => pair.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            cancellationToken.ThrowIfCancellationRequested();

            items.Add(
                new XDataGridFilterItem
                {
                    Value = pair.Value,
                    DisplayValue = pair.Key,
                    IsSelected = previousSelection.TryGetValue(pair.Key, out bool isSelected)
                        ? isSelected
                        : previouslySelectedValues is null || previouslySelectedValues.Contains(pair.Key)
                });
        }

        return items;
    }

    private void SubscribeFilterItem(XDataGridFilterItem item, string columnKey)
    {
        this._filterItemColumnMap[item] = columnKey;

        if (!this._subscribedFilterItems.Add(item))
        {
            return;
        }

        item.PropertyChanged += this.OnFilterItemPropertyChanged;
    }

    private void UnsubscribeFilterItem(XDataGridFilterItem item)
    {
        this._filterItemColumnMap.Remove(item);

        if (!this._subscribedFilterItems.Remove(item))
        {
            return;
        }

        item.PropertyChanged -= this.OnFilterItemPropertyChanged;
    }

    private void SelectAllColumnFilterItems(string columnKey)
    {
        if (!this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            this._activeFilterValuesCache[columnKey] = null;
            return;
        }

        this._isBulkUpdatingFilters = true;

        try
        {
            foreach (XDataGridFilterItem item in items)
            {
                item.IsSelected = true;
            }
        }
        finally
        {
            this._isBulkUpdatingFilters = false;
        }

        this.UpdateActiveFilterCache(columnKey);
        this.UpdateFilterButtonVisual(columnKey);
    }

    private void UpdateActiveFilterCache(string columnKey)
    {
        if (!this._columnFilterItems.TryGetValue(columnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            this._activeFilterValuesCache[columnKey] = null;
            return;
        }

        if (items.Count == 0)
        {
            if (!this._activeFilterValuesCache.ContainsKey(columnKey))
            {
                this._activeFilterValuesCache[columnKey] = null;
            }

            return;
        }

        bool allSelected = items.All(item => item.IsSelected);
        if (allSelected)
        {
            this._activeFilterValuesCache[columnKey] = null;
            return;
        }

        this._activeFilterValuesCache[columnKey] = items
            .Where(item => item.IsSelected)
            .Select(item => item.DisplayValue)
            .ToHashSet(StringComparer.CurrentCulture);
    }

    private bool IsColumnFilterActive(string columnKey)
    {
        return this._activeFilterValuesCache.TryGetValue(columnKey, out HashSet<string>? selectedValues) &&
            selectedValues is not null;
    }

    /// <summary>
    /// Determines whether at least one column filter or the search term is active.
    /// </summary>
    /// <returns><c>true</c> if any filter is active; otherwise, <c>false</c>.</returns>
    private bool HasAnyActiveFilter()
    {
        if (!string.IsNullOrWhiteSpace(this._activeSearchTerm))
        {
            return true;
        }

        return this._activeFilterValuesCache.Values.Any(values => values is not null);
    }
    #endregion

    #region ### Private Methods (Filter Event Handlers) ###
    private void OnFilterItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(XDataGridFilterItem.IsSelected))
        {
            return;
        }

        if (this._isBulkUpdatingFilters)
        {
            return;
        }

        if (sender is XDataGridFilterItem filterItem && this._filterItemColumnMap.TryGetValue(filterItem, out string? columnKey))
        {
            this.UpdateActiveFilterCache(columnKey);
        }

        this.RefreshFilters();
        this.ScheduleViewStateStore();
    }

    private void OnCurrentPopupClosed(object? sender, EventArgs e)
    {
        if (!ReferenceEquals(sender, this._currentPopup))
        {
            return;
        }

        if (sender is Popup popup)
        {
            popup.Closed -= this.OnCurrentPopupClosed;

            if (popup.Child is FrameworkElement popupChild)
            {
                popupChild.PreviewKeyDown -= this.OnCurrentPopupPreviewKeyDown;
            }
        }

        this.CommitCurrentFilterSelection();
        this.CancelCurrentFilterItemsLoad();
        this._currentPopup = null;
        this._currentPopupButton = null;
        this._currentFilterColumnKey = null;
        this.CurrentFilterItems = [];
    }
    #endregion

    #region ### Private Methods (Filter Popup) ###
    private void SelectAllCurrentColumn()
    {
        if (this._currentFilterColumnKey is null)
        {
            return;
        }

        if (!this._columnFilterItems.TryGetValue(this._currentFilterColumnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            return;
        }

        this._isBulkUpdatingFilters = true;

        try
        {
            foreach (XDataGridFilterItem item in items)
            {
                item.IsSelected = true;
            }

            this.UpdateActiveFilterCache(this._currentFilterColumnKey);
        }
        finally
        {
            this._isBulkUpdatingFilters = false;
        }

        this.RefreshFilters();
        this.ScheduleViewStateStore();
    }

    private void ClearCurrentColumn()
    {
        if (this._currentFilterColumnKey is null)
        {
            return;
        }

        if (!this._columnFilterItems.TryGetValue(this._currentFilterColumnKey, out ObservableCollection<XDataGridFilterItem>? items))
        {
            return;
        }

        this._isBulkUpdatingFilters = true;

        try
        {
            foreach (XDataGridFilterItem item in items)
            {
                item.IsSelected = false;
            }

            this.UpdateActiveFilterCache(this._currentFilterColumnKey);
        }
        finally
        {
            this._isBulkUpdatingFilters = false;
        }

        this.RefreshFilters();
        this.ScheduleViewStateStore();
    }

    private void CommitCurrentFilterSelection()
    {
        if (this._currentFilterColumnKey is null)
        {
            return;
        }

        this.UpdateCurrentFilterBindingSources();

        string columnKey = this._currentFilterColumnKey;

        if (this._columnFilterItems.ContainsKey(columnKey))
        {
            this.UpdateActiveFilterCache(columnKey);
            this.UpdateFilterButtonVisual(columnKey);
        }

        this.RefreshFilters();
        this.ScheduleViewStateStore();
    }

    private void UpdateCurrentFilterBindingSources()
    {
        if (this._currentPopup?.Child is not System.Windows.DependencyObject child)
        {
            return;
        }

        UpdateFilterBindingSources(child);
    }

    private static void UpdateFilterBindingSources(System.Windows.DependencyObject element)
    {
        if (element is ToggleButton toggleButton)
        {
            toggleButton.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        }

        int visualChildCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(element);
        for (int index = 0; index < visualChildCount; index++)
        {
            UpdateFilterBindingSources(System.Windows.Media.VisualTreeHelper.GetChild(element, index));
        }
    }

    private void OnCurrentPopupPreviewKeyDown(object sender, KeyEventArgs e)
    {
        Key key = GetEffectiveKey(e);

        if (key == Key.Escape)
        {
            this.CloseCurrentPopup();
            e.Handled = true;
            return;
        }

        if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
        {
            return;
        }

        if (key == Key.A)
        {
            this.SelectAllCurrentColumn();
            e.Handled = true;
        }
        else if (key == Key.N)
        {
            this.ClearCurrentColumn();
            e.Handled = true;
        }
    }

    private void FocusCurrentFilterPopup()
    {
        this.Dispatcher.BeginInvoke(
            new Action(
                () =>
                {
                    if (this._currentPopup?.IsOpen != true || this._currentPopup.Child is not DependencyObject child)
                    {
                        return;
                    }

                    XCheckBox? firstCheckBox = FindVisualChild<XCheckBox>(child);
                    if (firstCheckBox is not null)
                    {
                        firstCheckBox.Focus();
                        return;
                    }

                    FindVisualChild<ListBox>(child)?.Focus();
                }),
            DispatcherPriority.ContextIdle);
    }

    private static Key GetEffectiveKey(KeyEventArgs e)
    {
        if (e.Key == Key.System)
        {
            return e.SystemKey;
        }

        return e.Key == Key.ImeProcessed ? e.ImeProcessedKey : e.Key;
    }

    private static T? FindVisualChild<T>(DependencyObject parent)
        where T : DependencyObject
    {
        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int index = 0; index < childCount; index++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, index);

            if (child is T typedChild)
            {
                return typedChild;
            }

            T? result = FindVisualChild<T>(child);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private void CloseCurrentPopup()
    {
        if (this._currentPopup is null)
        {
            return;
        }

        Popup popup = this._currentPopup;
        popup.Closed -= this.OnCurrentPopupClosed;

        if (popup.Child is FrameworkElement popupChild)
        {
            popupChild.PreviewKeyDown -= this.OnCurrentPopupPreviewKeyDown;
        }

        this.CommitCurrentFilterSelection();
        this.CancelCurrentFilterItemsLoad();

        if (popup.IsOpen)
        {
            popup.IsOpen = false;
        }

        this._currentPopup = null;
        this._currentPopupButton = null;
        this._currentFilterColumnKey = null;
        this.CurrentFilterItems = [];
    }
    #endregion

    #region ### Private Methods (Filter Matching) ###
    private bool FilterItem(object item)
    {
        return this.MatchesSearch(item) && this.MatchesColumnFilters(item);
    }

    private bool MatchesSearch(object item)
    {
        if (string.IsNullOrWhiteSpace(this._activeSearchTerm))
        {
            return true;
        }

        if (this._searchableColumnSnapshot.Count == 0)
        {
            return true;
        }

        foreach (string columnName in this._searchableColumnSnapshot)
        {
            object? value = this.GetPropertyValue(item, columnName);
            string text = value?.ToString() ?? string.Empty;

            if (text.Contains(this._activeSearchTerm, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private bool MatchesColumnFilters(object item)
    {
        if (this._activeColumnFilters.Count == 0)
        {
            return true;
        }

        foreach (KeyValuePair<string, HashSet<string>> activeFilter in this._activeColumnFilters)
        {
            string columnName = activeFilter.Key;
            HashSet<string> selectedValues = activeFilter.Value;

            if (selectedValues.Count == 0)
            {
                return false;
            }

            object? rawValue = this.GetPropertyValue(item, columnName);
            string displayValue = rawValue?.ToString() ?? string.Empty;

            if (!selectedValues.Contains(displayValue))
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region ### Private Methods (Filter Runtime Caches) ###
    private void UpdateFilterRuntimeCaches()
    {
        this._activeSearchTerm = this.SearchTerm?.Trim() ?? string.Empty;

        this._searchableColumnSnapshot.Clear();
        this._searchableColumnSnapshot.AddRange(
            this.SearchableColumns
                .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
                .Distinct(StringComparer.Ordinal));

        this._enabledFilterDefinitionKeys.Clear();
        this._enabledFilterDefinitionKeys.UnionWith(
            this.FilterDefinitions
                .Where(definition => definition.IsEnabled)
                .Select(definition => definition.ColumnName)
                .Where(columnName => !string.IsNullOrWhiteSpace(columnName)));

        this._activeColumnFilters.Clear();

        foreach (KeyValuePair<string, HashSet<string>?> activeFilter in this._activeFilterValuesCache)
        {
            string columnName = activeFilter.Key;
            HashSet<string>? selectedValues = activeFilter.Value;

            if (selectedValues is null)
            {
                continue;
            }

            if (!this._enabledFilterDefinitionKeys.Contains(columnName) && !this.HasColumnForProperty(columnName))
            {
                continue;
            }

            this._activeColumnFilters.Add(new KeyValuePair<string, HashSet<string>>(columnName, selectedValues));
        }
    }
    private void PrewarmPropertyAccessorCache()
    {
        object? firstItem = null;

        if (this._rawItemsSource is not null)
        {
            foreach (object? candidate in this._rawItemsSource)
            {
                if (candidate is not null)
                {
                    firstItem = candidate;
                    break;
                }
            }
        }

        if (firstItem is null)
        {
            return;
        }

        HashSet<string> columnNames = this.CollectAllColumnNamesForPrewarm();
        if (columnNames.Count == 0)
        {
            return;
        }

        if (firstItem is DispatcherObject)
        {
            this.PrewarmPropertyAccessorCache(firstItem, columnNames);
            return;
        }

        _ = System.Threading.Tasks.Task.Run(() => this.PrewarmPropertyAccessorCache(firstItem, columnNames));
    }

    private HashSet<string> CollectAllColumnNamesForPrewarm()
    {
        HashSet<string> columnNames = new(StringComparer.Ordinal);

        foreach (string columnName in this._searchableColumnSnapshot)
        {
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                columnNames.Add(columnName);
            }
        }

        foreach (XDataGridFilterDefinition definition in this.FilterDefinitions.Where(definition => definition.IsEnabled))
        {
            if (!string.IsNullOrWhiteSpace(definition.ColumnName))
            {
                columnNames.Add(definition.ColumnName);
            }
        }

        foreach (DataGridColumn column in this.Columns.Cast<DataGridColumn>().Where(column => !this.IsActionColumn(column)))
        {
            string columnName = this.GetColumnKey(column);
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                columnNames.Add(columnName);
            }
        }

        return columnNames;
    }

    private void PrewarmPropertyAccessorCache(object firstItem, IEnumerable<string> columnNames)
    {
        foreach (string columnName in columnNames)
        {
            _ = this.GetPropertyValue(firstItem, columnName);
        }
    }

    #endregion

    #region ### Private Methods (Column Metadata Sync) ###
    private void EnsureDefinitionsAndSearchableColumns()
    {
        bool shouldGenerateFilterDefinitions = this.AutoGenerateFilterDefinitions;
        bool shouldGenerateSearchableColumns = this.ShouldAutoGenerateSearchableColumns();

        if (!shouldGenerateFilterDefinitions && !shouldGenerateSearchableColumns)
        {
            return;
        }

        List<XDataGridFilterDefinition> filterDefinitionsToAdd = [];
        List<string> searchableColumnsToAdd = [];
        HashSet<string> existingFilterDefinitionKeys = this.FilterDefinitions
            .Select(definition => definition.ColumnName)
            .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
            .ToHashSet(StringComparer.Ordinal);
        HashSet<string> existingSearchableColumnKeys = this.SearchableColumns
            .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
            .ToHashSet(StringComparer.Ordinal);

        foreach (DataGridColumn column in this.Columns)
        {
            if (this.IsActionColumn(column))
            {
                continue;
            }

            string columnKey = this.GetColumnKey(column);
            if (string.IsNullOrWhiteSpace(columnKey))
            {
                continue;
            }

            if (shouldGenerateFilterDefinitions && existingFilterDefinitionKeys.Add(columnKey))
            {
                filterDefinitionsToAdd.Add(
                    new XDataGridFilterDefinition
                    {
                        ColumnName = columnKey,
                        DisplayName = column.Header?.ToString()
                    });
            }

            if (shouldGenerateSearchableColumns && existingSearchableColumnKeys.Add(columnKey))
            {
                searchableColumnsToAdd.Add(columnKey);
            }
        }

        if (filterDefinitionsToAdd.Count == 0 && searchableColumnsToAdd.Count == 0)
        {
            return;
        }

        this._isSynchronizingColumnMetadata = true;

        try
        {
            foreach (XDataGridFilterDefinition filterDefinition in filterDefinitionsToAdd)
            {
                this.FilterDefinitions.Add(filterDefinition);
            }

            foreach (string columnKey in searchableColumnsToAdd)
            {
                this.SearchableColumns.Add(columnKey);
                this._autoGeneratedSearchableColumnKeys.Add(columnKey);
            }
        }
        finally
        {
            this._isSynchronizingColumnMetadata = false;
        }
    }

    private bool ShouldAutoGenerateSearchableColumns()
    {
        if (!this.AutoGenerateSearchableColumns || BindingOperations.IsDataBound(this, SearchableColumnsProperty))
        {
            return false;
        }

        return this.SearchableColumns.All(columnKey => this._autoGeneratedSearchableColumnKeys.Contains(columnKey));
    }
    #endregion

    #region ### Private Methods (Property Accessor) ###
    private object? GetPropertyValue(object? item, string propertyName)
    {
        if (item is null || string.IsNullOrWhiteSpace(propertyName))
        {
            return null;
        }

        Type itemType = item.GetType();
        Func<object, object?> accessor = this.GetPropertyAccessor(itemType, propertyName);
        return accessor(item);
    }

    private Func<object, object?> GetPropertyAccessor(Type itemType, string propertyName)
    {
        (Type Type, string PropertyName) cacheKey = (itemType, propertyName);

        lock (_propertyAccessorCache)
        {
            if (_propertyAccessorCache.TryGetValue(cacheKey, out Func<object, object?>? accessor))
            {
                return accessor;
            }

            System.Reflection.PropertyInfo? property = itemType.GetProperty(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (property is null || !property.CanRead)
            {
                accessor = static _ => null;
                _propertyAccessorCache[cacheKey] = accessor;
                return accessor;
            }

            System.Linq.Expressions.ParameterExpression instanceParameter = System.Linq.Expressions.Expression.Parameter(typeof(object), "instance");
            System.Linq.Expressions.UnaryExpression typedInstance = System.Linq.Expressions.Expression.Convert(instanceParameter, itemType);
            System.Linq.Expressions.MemberExpression propertyAccess = System.Linq.Expressions.Expression.Property(typedInstance, property);
            System.Linq.Expressions.UnaryExpression convertResult = System.Linq.Expressions.Expression.Convert(propertyAccess, typeof(object));

            accessor = System.Linq.Expressions.Expression.Lambda<Func<object, object?>>(convertResult, instanceParameter).Compile();
            _propertyAccessorCache[cacheKey] = accessor;

            return accessor;
        }
    }
    #endregion

    private void SetSearchTermPreservingBinding(string value)
    {
        this.SetCurrentValue(SearchTermProperty, value);
        this.GetBindingExpression(SearchTermProperty)?.UpdateSource();
    }
}
