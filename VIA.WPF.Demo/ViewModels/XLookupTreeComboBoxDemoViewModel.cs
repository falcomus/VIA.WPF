// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupTreeComboBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XLookupTreeComboBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XLookupTreeComboBox showcase page.
/// </summary>
public sealed partial class XLookupTreeComboBoxDemoViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupTreeComboBoxDemoViewModel"/> class.
    /// </summary>
    public XLookupTreeComboBoxDemoViewModel()
    {
        this.SelectedCategoryId = 112;
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XLookupTreeComboBox";

    /// <inheritdoc/>
    public override string Description => "Demonstrates hierarchical lookup selection with icons, empty option, selected item/value binding and tree expansion member paths.";

    /// <summary>
    /// Gets the hierarchical sample categories.
    /// </summary>
    public ObservableCollection<XLookupTreeComboBoxDemoNode> Categories { get; } =
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
        new(300, "Archive", false,
        [
            new(310, "2024"),
            new(320, "2025"),
        ]),
    ];

    /// <summary>
    /// Gets or sets the selected category.
    /// </summary>
    [ObservableProperty]
    private XLookupTreeComboBoxDemoNode? _selectedCategory;

    /// <summary>
    /// Gets or sets the selected category id.
    /// </summary>
    [ObservableProperty]
    private int? _selectedCategoryId;

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XLookupTreeComboBox
    Width="360"
    ChildrenMemberPath="Children"
    DisplayMemberPath="Name"
    EmptyOptionText="No category"
    ExpandedMemberPath="IsExpanded"
    Header="Category"
    Icon="{via:MaterialIcon Kind=FileTreeOutline}"
    IncludeEmptyOption="True"
    ItemsSource="{Binding Categories}"
    Placeholder="Select category"
    SelectedItem="{Binding SelectedCategory, Mode=TwoWay}"
    SelectedValue="{Binding SelectedCategoryId, Mode=TwoWay}"
    SelectedValuePath="Id"
    ShowResetButton="True"
    Variant="Outline" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XLookupTreeComboBox lookup = new()
{
    ItemsSource = categories,
    DisplayMemberPath = nameof(CategoryNode.Name),
    SelectedValuePath = nameof(CategoryNode.Id),
    ChildrenMemberPath = nameof(CategoryNode.Children),
    ExpandedMemberPath = nameof(CategoryNode.IsExpanded),
    IncludeEmptyOption = true,
    ShowResetButton = true,
};
""";
    #endregion
}
#endregion

#region ### Class XLookupTreeComboBoxDemoNode ###
/// <summary>
/// Represents one hierarchical lookup item.
/// </summary>
public sealed class XLookupTreeComboBoxDemoNode
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupTreeComboBoxDemoNode"/> class.
    /// </summary>
    /// <param name="id">The node id.</param>
    /// <param name="name">The node name.</param>
    /// <param name="isExpanded">A value indicating whether the node is expanded.</param>
    /// <param name="children">The child nodes.</param>
    public XLookupTreeComboBoxDemoNode(int id, string name, bool isExpanded = false, IEnumerable<XLookupTreeComboBoxDemoNode>? children = null)
    {
        this.Id = id;
        this.Name = name;
        this.IsExpanded = isExpanded;
        this.Children = children is null ? [] : new ObservableCollection<XLookupTreeComboBoxDemoNode>(children);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the node id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the node display name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is expanded.
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Gets the child nodes.
    /// </summary>
    public ObservableCollection<XLookupTreeComboBoxDemoNode> Children { get; }
    #endregion
}
#endregion
