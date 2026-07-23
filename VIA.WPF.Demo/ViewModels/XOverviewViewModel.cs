// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OverviewViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class OverviewViewModel ###
/// <summary>
/// Represents the Overview page — all controls on a single scrollable canvas.
/// </summary>
public sealed partial class XOverviewViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XOverviewViewModel"/> class.
    /// </summary>
    public XOverviewViewModel()
    {
        this.SelectedOverviewCustomerId = 2;
        this.SelectedOverviewCategoryId = 112;
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Overview";

    /// <inheritdoc/>
    public override string Description => "All VIA.WPF controls at a glance on one scrollable page.";

    /// <summary>
    /// Gets the sample customers used by the overview lookup example.
    /// </summary>
    public ObservableCollection<OverviewLookupItem> OverviewCustomers { get; } =
    [
        new(1, "Northwind Traders"),
        new(2, "Contoso Retail"),
        new(3, "Fabrikam Logistics"),
        new(4, "Adventure Works"),
    ];

    /// <summary>
    /// Gets the sample categories used by the overview tree lookup example.
    /// </summary>
    public ObservableCollection<OverviewTreeItem> OverviewCategories { get; } =
    [
        new(100, "Warehouse", true,
        [
            new(110, "Aisle A", true,
            [
                new(111, "Shelf A-01"),
                new(112, "Shelf A-02"),
                new(113, "Shelf A-03"),
            ]),
            new(120, "Aisle B", false,
            [
                new(121, "Shelf B-01"),
                new(122, "Shelf B-02"),
            ]),
        ]),
        new(200, "Administration", true,
        [
            new(210, "Purchasing"),
            new(220, "Sales"),
            new(230, "Accounting"),
        ]),
    ];

    /// <summary>
    /// Gets or sets the selected overview customer id.
    /// </summary>
    [ObservableProperty]
    private int? _selectedOverviewCustomerId;

    /// <summary>
    /// Gets or sets the selected overview category id.
    /// </summary>
    [ObservableProperty]
    private int? _selectedOverviewCategoryId;

    /// <inheritdoc/>
    public override string XamlCode => string.Empty;

    /// <inheritdoc/>
    public override string CSharpCode => string.Empty;
    #endregion
}
#endregion

#region ### Class OverviewLookupItem ###
/// <summary>
/// Represents a flat lookup item used by the overview page.
/// </summary>
public sealed class OverviewLookupItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="OverviewLookupItem"/> class.
    /// </summary>
    /// <param name="id">The item id.</param>
    /// <param name="name">The display name.</param>
    public OverviewLookupItem(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    public string Name { get; }
    #endregion
}
#endregion

#region ### Class OverviewTreeItem ###
/// <summary>
/// Represents a hierarchical lookup item used by the overview page.
/// </summary>
public sealed class OverviewTreeItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="OverviewTreeItem"/> class.
    /// </summary>
    /// <param name="id">The item id.</param>
    /// <param name="name">The display name.</param>
    /// <param name="isExpanded">A value indicating whether this node is initially expanded.</param>
    /// <param name="children">The child items.</param>
    public OverviewTreeItem(int id, string name, bool isExpanded = false, IEnumerable<OverviewTreeItem>? children = null)
    {
        this.Id = id;
        this.Name = name;
        this.IsExpanded = isExpanded;
        this.Children = children is null
            ? []
            : new ObservableCollection<OverviewTreeItem>(children);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this node is expanded.
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Gets the child items.
    /// </summary>
    public ObservableCollection<OverviewTreeItem> Children { get; }
    #endregion
}
#endregion