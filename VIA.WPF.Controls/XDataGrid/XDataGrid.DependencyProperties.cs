// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGrid.DependencyProperties.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

#region ### Class XDataGrid ###
/// <content>
/// Contains dependency property registrations and property changed callbacks for <see cref="XDataGrid" />.
/// </content>
public partial class XDataGrid
{
    #region ### Dependency Properties ###

    /// <summary>
    /// Identifies the <see cref="SearchTerm"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchTermProperty =
        DependencyProperty.Register(
            nameof(SearchTerm),
            typeof(string),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSearchTermChanged));

    /// <summary>
    /// Identifies the <see cref="FilterDefinitions"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FilterDefinitionsProperty =
        DependencyProperty.Register(
            nameof(FilterDefinitions),
            typeof(ObservableCollection<XDataGridFilterDefinition>),
            typeof(XDataGrid),
            new PropertyMetadata(null, OnFilterDefinitionsChanged));

    /// <summary>
    /// Identifies the <see cref="SearchableColumns"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchableColumnsProperty =
        DependencyProperty.Register(
            nameof(SearchableColumns),
            typeof(ObservableCollection<string>),
            typeof(XDataGrid),
            new PropertyMetadata(null, OnSearchableColumnsChanged));

    /// <summary>
    /// Identifies the <see cref="SearchMatchHighlightRenderMode" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchMatchHighlightRenderModeProperty = DependencyProperty.Register(
        nameof(SearchMatchHighlightRenderMode),
        typeof(XHighlightRenderMode),
        typeof(XDataGrid),
        new FrameworkPropertyMetadata(XHighlightRenderMode.Badge));

    /// <summary>
    /// Identifies the <see cref="SearchMatchHighlightBrush" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchMatchHighlightBrushProperty = DependencyProperty.Register(
        nameof(SearchMatchHighlightBrush),
        typeof(Brush),
        typeof(XDataGrid),
        new FrameworkPropertyMetadata(XBrushFactory.CreateFrozenBrush(0x66, 0x20, 0xA0, 0xE0)));


    /// <summary>
    /// Identifies the <see cref="SearchMatchHighlightForeground" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchMatchHighlightForegroundProperty = DependencyProperty.Register(
        nameof(SearchMatchHighlightForeground),
        typeof(Brush),
        typeof(XDataGrid),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="AutoGenerateFilterDefinitions"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AutoGenerateFilterDefinitionsProperty =
        DependencyProperty.Register(
            nameof(AutoGenerateFilterDefinitions),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true, OnAutoGenerateFilterDefinitionsChanged));

    /// <summary>
    /// Identifies the <see cref="AutoGenerateSearchableColumns"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AutoGenerateSearchableColumnsProperty =
        DependencyProperty.Register(
            nameof(AutoGenerateSearchableColumns),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true, OnAutoGenerateSearchableColumnsChanged));

    /// <summary>
    /// Identifies the <see cref="ShowFilterButtons"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowFilterButtonsProperty =
        DependencyProperty.Register(
            nameof(ShowFilterButtons),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="ShowCurrentCellFocus"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowCurrentCellFocusProperty =
        DependencyProperty.Register(
            nameof(ShowCurrentCellFocus),
            typeof(bool),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ColumnDisplayMode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnDisplayModeProperty =
        DependencyProperty.Register(
            nameof(ColumnDisplayMode),
            typeof(XDataGridColumnDisplayMode),
            typeof(XDataGrid),
            new PropertyMetadata(XDataGridColumnDisplayMode.Full, OnColumnDisplayConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ColumnDisplayModes"/> attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnDisplayModesProperty =
        DependencyProperty.RegisterAttached(
            "ColumnDisplayModes",
            typeof(XDataGridColumnDisplayModes),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(XDataGridColumnDisplayModes.All, OnColumnDisplayConfigurationChanged));

    /// <summary>
    /// Identifies the internal column display base visibility attached dependency property.
    /// </summary>
    private static readonly DependencyProperty ColumnDisplayBaseVisibilityProperty =
        DependencyProperty.RegisterAttached(
            "ColumnDisplayBaseVisibility",
            typeof(object),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the internal column display owner attached dependency property.
    /// </summary>
    private static readonly DependencyProperty ColumnDisplayOwnerProperty =
        DependencyProperty.RegisterAttached(
            "ColumnDisplayOwner",
            typeof(XDataGrid),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowActionColumn"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowActionColumnProperty =
        DependencyProperty.Register(
            nameof(ShowActionColumn),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(false, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowViewButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowViewButtonProperty =
        DependencyProperty.Register(
            nameof(ShowViewButton),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(false, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowEditButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditButtonProperty =
        DependencyProperty.Register(
            nameof(ShowEditButton),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(false, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ShowDeleteButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteButtonProperty =
        DependencyProperty.Register(
            nameof(ShowDeleteButton),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(false, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ViewItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewItemCommandProperty =
        DependencyProperty.Register(
            nameof(ViewItemCommand),
            typeof(ICommand),
            typeof(XDataGrid),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="NewItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewItemCommandProperty =
        DependencyProperty.Register(
            nameof(NewItemCommand),
            typeof(ICommand),
            typeof(XDataGrid),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditItemCommandProperty =
        DependencyProperty.Register(
            nameof(EditItemCommand),
            typeof(ICommand),
            typeof(XDataGrid),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteItemCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteItemCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteItemCommand),
            typeof(ICommand),
            typeof(XDataGrid),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ActionColumnHeader"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionColumnHeaderProperty =
        DependencyProperty.Register(
            nameof(ActionColumnHeader),
            typeof(object),
            typeof(XDataGrid),
            new PropertyMetadata(null, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ResetAllFiltersButtonContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetAllFiltersButtonContentProperty =
        DependencyProperty.Register(
            nameof(ResetAllFiltersButtonContent),
            typeof(object),
            typeof(XDataGrid),
            new PropertyMetadata("RESET", OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ActionColumnWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionColumnWidthProperty =
        DependencyProperty.Register(
            nameof(ActionColumnWidth),
            typeof(DataGridLength),
            typeof(XDataGrid),
            new PropertyMetadata(new DataGridLength(75d), OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ActionColumnMinWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionColumnMinWidthProperty =
        DependencyProperty.Register(
            nameof(ActionColumnMinWidth),
            typeof(double),
            typeof(XDataGrid),
            new PropertyMetadata(75d, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ActionColumnMaxWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionColumnMaxWidthProperty =
        DependencyProperty.Register(
            nameof(ActionColumnMaxWidth),
            typeof(double),
            typeof(XDataGrid),
            new PropertyMetadata(double.PositiveInfinity, OnActionColumnConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ActionButtonSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionButtonSizeProperty =
        DependencyProperty.Register(
            nameof(ActionButtonSize),
            typeof(double),
            typeof(XDataGrid),
            new PropertyMetadata(24d));

    /// <summary>
    /// Identifies the <see cref="ActionButtonIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionButtonIconSizeProperty =
        DependencyProperty.Register(
            nameof(ActionButtonIconSize),
            typeof(double),
            typeof(XDataGrid),
            new PropertyMetadata(14d));

    /// <summary>
    /// Identifies the <see cref="ActionButtonSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActionButtonSpacingProperty =
        DependencyProperty.Register(
            nameof(ActionButtonSpacing),
            typeof(double),
            typeof(XDataGrid),
            new PropertyMetadata(5d));

    /// <summary>
    /// Identifies the <see cref="ViewButtonToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewButtonToolTipProperty =
        DependencyProperty.Register(
            nameof(ViewButtonToolTip),
            typeof(object),
            typeof(XDataGrid),
            new PropertyMetadata("View"));

    /// <summary>
    /// Identifies the <see cref="EditButtonToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditButtonToolTipProperty =
        DependencyProperty.Register(
            nameof(EditButtonToolTip),
            typeof(object),
            typeof(XDataGrid),
            new PropertyMetadata("Edit"));

    /// <summary>
    /// Identifies the <see cref="DeleteButtonToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteButtonToolTipProperty =
        DependencyProperty.Register(
            nameof(DeleteButtonToolTip),
            typeof(object),
            typeof(XDataGrid),
            new PropertyMetadata("Delete"));

    /// <summary>
    /// Identifies the <see cref="PersistViewState"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PersistViewStateProperty =
        DependencyProperty.Register(
            nameof(PersistViewState),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true, OnPersistViewStateChanged));

    /// <summary>
    /// Identifies the <see cref="ViewStateKey"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ViewStateKeyProperty =
        DependencyProperty.Register(
            nameof(ViewStateKey),
            typeof(string),
            typeof(XDataGrid),
            new PropertyMetadata(null, OnViewStateKeyChanged));

    /// <summary>
    /// Identifies the <see cref="EditOnRowDoubleClick"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditOnRowDoubleClickProperty =
        DependencyProperty.Register(
            nameof(EditOnRowDoubleClick),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="EditOnEnterKey"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditOnEnterKeyProperty =
        DependencyProperty.Register(
            nameof(EditOnEnterKey),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="NewOnInsertKey"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty NewOnInsertKeyProperty =
        DependencyProperty.Register(
            nameof(NewOnInsertKey),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true));

    /// <summary>
    /// Identifies the <see cref="DeleteOnDeleteKey"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteOnDeleteKeyProperty =
        DependencyProperty.Register(
            nameof(DeleteOnDeleteKey),
            typeof(bool),
            typeof(XDataGrid),
            new PropertyMetadata(true));

    /// <summary>
    /// Identifies the IsGeneratedActionColumn attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsGeneratedActionColumnProperty =
        DependencyProperty.RegisterAttached(
            "IsGeneratedActionColumn",
            typeof(bool),
            typeof(XDataGrid),
            new FrameworkPropertyMetadata(false));

    #endregion

    #region ### Attached Property Accessors ###
    /// <summary>
    /// Gets the display modes in which the specified column is visible.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <returns>The display modes in which the column is visible.</returns>
    public static XDataGridColumnDisplayModes GetColumnDisplayModes(DependencyObject element)
    {
        return (XDataGridColumnDisplayModes)element.GetValue(ColumnDisplayModesProperty);
    }

    /// <summary>
    /// Sets the display modes in which the specified column is visible.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <param name="value">The display modes.</param>
    public static void SetColumnDisplayModes(DependencyObject element, XDataGridColumnDisplayModes value)
    {
        element.SetValue(ColumnDisplayModesProperty, value);
    }

    /// <summary>
    /// Gets the base visibility of the specified column.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <returns>The base visibility if known; otherwise, <c>null</c>.</returns>
    private static Visibility? GetColumnDisplayBaseVisibility(DependencyObject element)
    {
        object value = element.GetValue(ColumnDisplayBaseVisibilityProperty);
        return value is Visibility visibility ? visibility : null;
    }

    /// <summary>
    /// Sets the base visibility of the specified column.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <param name="value">The base visibility.</param>
    private static void SetColumnDisplayBaseVisibility(DependencyObject element, Visibility value)
    {
        element.SetValue(ColumnDisplayBaseVisibilityProperty, value);
    }

    /// <summary>
    /// Gets the owning <see cref="XDataGrid" /> for the specified column display metadata.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <returns>The owning grid, or <c>null</c>.</returns>
    private static XDataGrid? GetColumnDisplayOwner(DependencyObject element)
    {
        return (XDataGrid?)element.GetValue(ColumnDisplayOwnerProperty);
    }

    /// <summary>
    /// Sets the owning <see cref="XDataGrid" /> for the specified column display metadata.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <param name="value">The owning grid.</param>
    private static void SetColumnDisplayOwner(DependencyObject element, XDataGrid? value)
    {
        element.SetValue(ColumnDisplayOwnerProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the specified column is the generated action column of an <see cref="XDataGrid" />.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <returns><c>true</c> if the column is the generated action column; otherwise, <c>false</c>.</returns>
    public static bool GetIsGeneratedActionColumn(DependencyObject element)
    {
        return (bool)element.GetValue(IsGeneratedActionColumnProperty);
    }

    /// <summary>
    /// Sets a value indicating whether the specified column is the generated action column of an <see cref="XDataGrid" />.
    /// </summary>
    /// <param name="element">The column.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsGeneratedActionColumn(DependencyObject element, bool value)
    {
        element.SetValue(IsGeneratedActionColumnProperty, value);
    }

    #endregion

    #region ### Private Static Methods (DP Callbacks) ###
    private static void OnSearchTermChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDataGrid grid)
        {
            return;
        }

        grid._activeSearchTerm = (e.NewValue as string)?.Trim() ?? string.Empty;

        if (grid._isRestoringViewState)
        {
            return;
        }

        grid.ScheduleFilterRefresh();
        grid.ScheduleViewStateStore();
    }

    private static void OnFilterDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDataGrid grid)
        {
            return;
        }

        if (e.OldValue is ObservableCollection<XDataGridFilterDefinition> oldCollection)
        {
            oldCollection.CollectionChanged -= grid.OnFilterDefinitionsCollectionChanged;
        }

        if (e.NewValue is ObservableCollection<XDataGridFilterDefinition> newCollection)
        {
            newCollection.CollectionChanged += grid.OnFilterDefinitionsCollectionChanged;
        }

        if (grid._isSynchronizingColumnMetadata)
        {
            return;
        }

        grid.EnsureDefinitionsAndSearchableColumns();
        grid.InvalidateAllColumnFilterItems();
        grid.RefreshFilters();
    }

    private static void OnSearchableColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDataGrid grid)
        {
            return;
        }

        if (e.OldValue is ObservableCollection<string> oldCollection)
        {
            oldCollection.CollectionChanged -= grid.OnSearchableColumnsCollectionChanged;
        }

        if (e.NewValue is ObservableCollection<string> newCollection)
        {
            newCollection.CollectionChanged += grid.OnSearchableColumnsCollectionChanged;
        }

        if (!grid._isSynchronizingColumnMetadata)
        {
            grid._autoGeneratedSearchableColumnKeys.Clear();
            grid.RefreshFilters();
        }
    }

    private static void OnAutoGenerateFilterDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDataGrid grid)
        {
            grid.EnsureDefinitionsAndSearchableColumns();
            grid.InvalidateAllColumnFilterItems();
            grid.RefreshFilters();
        }
    }

    private static void OnAutoGenerateSearchableColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDataGrid grid)
        {
            grid.EnsureDefinitionsAndSearchableColumns();
            grid.RefreshFilters();
        }
    }

    private static void OnActionColumnConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDataGrid grid)
        {
            grid.EnsureActionColumn();
            grid.ApplyColumnDisplayMode();
        }
    }

    private static void OnColumnDisplayConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDataGrid grid)
        {
            grid.ApplyColumnDisplayMode();
            return;
        }

        if (d is DataGridColumn column)
        {
            GetColumnDisplayOwner(column)?.ApplyColumnDisplayMode();
        }
    }

    private static void OnPersistViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDataGrid grid && grid.IsLoaded && (bool)e.NewValue)
        {
            grid.RestorePersistedViewStateOrApplyInitialSort();
            grid.RefreshFilters();
        }
    }

    private static void OnViewStateKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDataGrid grid)
        {
            return;
        }

        grid._cachedViewStateKey = null;

        if (!grid.IsLoaded)
        {
            return;
        }

        grid.RestorePersistedViewStateOrApplyInitialSort();
        grid.RefreshFilters();
    }
    #endregion
}
#endregion
