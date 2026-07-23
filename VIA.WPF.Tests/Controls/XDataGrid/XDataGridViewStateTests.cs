// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridViewStateTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.DataGrid;

#region ### Class XDataGridViewStateTests ###
/// <summary>
/// Provides tests for XDataGrid view-state model and dependency-property contracts.
/// </summary>
public sealed class XDataGridViewStateTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Verifies that a new view state starts with empty mutable collections.
    /// </summary>
    [Fact]
    public void XDataGridViewState_ShouldInitializeEmptyCollections()
    {
        XDataGridViewState state = new();

        Assert.Null(state.SearchTerm);
        Assert.NotNull(state.SortDescriptions);
        Assert.NotNull(state.ColumnFilters);
        Assert.Empty(state.SortDescriptions);
        Assert.Empty(state.ColumnFilters);
    }

    /// <summary>
    /// Verifies that sort state constructors initialize the expected property values.
    /// </summary>
    [Fact]
    public void XDataGridSortState_ShouldStorePropertyNameAndDirection()
    {
        XDataGridSortState defaultState = new();
        XDataGridSortState configuredState = new("Name", ListSortDirection.Descending);

        Assert.Equal(string.Empty, defaultState.PropertyName);
        Assert.Equal(ListSortDirection.Ascending, defaultState.Direction);
        Assert.Equal("Name", configuredState.PropertyName);
        Assert.Equal(ListSortDirection.Descending, configuredState.Direction);
    }

    /// <summary>
    /// Verifies that persisted data grid state survives JSON serialization.
    /// </summary>
    [Fact]
    public void XDataGridViewState_ShouldRoundTripThroughJson()
    {
        XDataGridViewState state = CreateSampleState();

        string json = JsonSerializer.Serialize(state);
        XDataGridViewState? restoredState = JsonSerializer.Deserialize<XDataGridViewState>(json);

        Assert.NotNull(restoredState);
        Assert.Equal("alpha", restoredState.SearchTerm);
        Assert.Collection(
            restoredState.SortDescriptions,
            sortState =>
            {
                Assert.Equal("Name", sortState.PropertyName);
                Assert.Equal(ListSortDirection.Ascending, sortState.Direction);
            },
            sortState =>
            {
                Assert.Equal("Status", sortState.PropertyName);
                Assert.Equal(ListSortDirection.Descending, sortState.Direction);
            });
        Assert.True(restoredState.ColumnFilters.ContainsKey("Status"));
        Assert.Equal(["Open", "Closed"], restoredState.ColumnFilters["Status"]);
    }

    /// <summary>
    /// Verifies the default values and change notifications of filter items.
    /// </summary>
    [Fact]
    public void XDataGridFilterItem_ShouldExposeDefaultsAndRaisePropertyChanged()
    {
        XDataGridFilterItem item = new();
        List<string?> changedProperties = [];
        item.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        item.Value = 42;
        item.DisplayValue = "42";
        item.IsSelected = false;

        Assert.Equal(42, item.Value);
        Assert.Equal("42", item.DisplayValue);
        Assert.False(item.IsSelected);
        Assert.Equal([nameof(XDataGridFilterItem.Value), nameof(XDataGridFilterItem.DisplayValue), nameof(XDataGridFilterItem.IsSelected)], changedProperties);
    }

    /// <summary>
    /// Verifies that unchanged filter item assignments do not raise duplicate notifications.
    /// </summary>
    [Fact]
    public void XDataGridFilterItem_ShouldNotRaisePropertyChangedForSameValue()
    {
        XDataGridFilterItem item = new()
        {
            Value = "Open",
            DisplayValue = "Open",
            IsSelected = false
        };
        int changeCount = 0;
        item.PropertyChanged += (_, _) => changeCount++;

        item.Value = "Open";
        item.DisplayValue = "Open";
        item.IsSelected = false;

        Assert.Equal(0, changeCount);
    }

    /// <summary>
    /// Verifies the default and configured values of filter definitions.
    /// </summary>
    [Fact]
    public void XDataGridFilterDefinition_ShouldExposeConfiguredValuesAndDefaults()
    {
        XDataGridFilterDefinition defaultDefinition = new()
        {
            ColumnName = "Status"
        };
        XDataGridFilterDefinition disabledDefinition = new()
        {
            ColumnName = "Archived",
            DisplayName = "Archived items",
            IsEnabled = false
        };

        Assert.Equal("Status", defaultDefinition.ColumnName);
        Assert.Null(defaultDefinition.DisplayName);
        Assert.True(defaultDefinition.IsEnabled);
        Assert.Equal("Archived", disabledDefinition.ColumnName);
        Assert.Equal("Archived items", disabledDefinition.DisplayName);
        Assert.False(disabledDefinition.IsEnabled);
    }

    /// <summary>
    /// Verifies the view-state related dependency-property defaults of XDataGrid.
    /// </summary>
    [Fact]
    public void XDataGrid_ShouldExposeExpectedViewStateDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDataGrid grid = new();

                Assert.True(grid.PersistViewState);
                Assert.Null(grid.ViewStateKey);
                Assert.Equal(string.Empty, grid.SearchTerm);
                Assert.Same(grid.FilterDefinitions, grid.FilterDefinitions);
                Assert.Same(grid.SearchableColumns, grid.SearchableColumns);
                Assert.Empty(grid.FilterDefinitions);
                Assert.Empty(grid.SearchableColumns);
            });
    }

    /// <summary>
    /// Verifies metadata for the view-state related dependency properties.
    /// </summary>
    [Fact]
    public void XDataGrid_ViewStateDependencyProperties_ShouldExposeExpectedMetadata()
    {
        FrameworkPropertyMetadata searchTermMetadata = Assert.IsAssignableFrom<FrameworkPropertyMetadata>(
            XDataGrid.SearchTermProperty.GetMetadata(typeof(XDataGrid)));
        PropertyMetadata persistMetadata = XDataGrid.PersistViewStateProperty.GetMetadata(typeof(XDataGrid));
        PropertyMetadata viewStateKeyMetadata = XDataGrid.ViewStateKeyProperty.GetMetadata(typeof(XDataGrid));

        Assert.True(searchTermMetadata.BindsTwoWayByDefault);
        Assert.Equal(string.Empty, searchTermMetadata.DefaultValue);
        Assert.Equal(true, persistMetadata.DefaultValue);
        Assert.Null(viewStateKeyMetadata.DefaultValue);
    }

    /// <summary>
    /// Verifies that view-state related dependency properties can be changed by callers.
    /// </summary>
    [Fact]
    public void XDataGrid_ViewStateDependencyProperties_ShouldStoreAssignedValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDataGrid grid = new()
                {
                    PersistViewState = false,
                    ViewStateKey = "CustomersGrid",
                    SearchTerm = "Acme"
                };

                Assert.False(grid.PersistViewState);
                Assert.Equal("CustomersGrid", grid.ViewStateKey);
                Assert.Equal("Acme", grid.SearchTerm);
            });
    }
    #endregion

    #region ### Private Methods ###
    private static XDataGridViewState CreateSampleState()
    {
        XDataGridViewState state = new()
        {
            SearchTerm = "alpha"
        };
        state.SortDescriptions.Add(new XDataGridSortState("Name", ListSortDirection.Ascending));
        state.SortDescriptions.Add(new XDataGridSortState("Status", ListSortDirection.Descending));
        state.ColumnFilters["Status"] = ["Open", "Closed"];
        return state;
    }
    #endregion
}
#endregion
