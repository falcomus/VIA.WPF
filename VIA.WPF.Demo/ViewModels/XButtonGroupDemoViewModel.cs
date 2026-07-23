// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButtonGroupDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XButtonGroupDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XButtonGroup showcase page.
/// </summary>
public sealed class XButtonGroupDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XButtonGroup";

    /// <inheritdoc/>
    public override string Description => "Segmented single-selection control for view modes, density choices and compact option groups.";

    /// <inheritdoc/>
    public override string XamlCode => """
<!-- Horizontal segmented view-mode selector. -->
<via:XButtonGroup
    SelectedValue="Cards"
    Variant="Primary"
    ItemMinWidth="112">
    <via:XButtonGroupItem
        Content="List"
        Icon="{via:MaterialIcon Kind=FormatListBulleted}"
        Value="List" />
    <via:XButtonGroupItem
        Content="Cards"
        Icon="{via:MaterialIcon Kind=ViewGridOutline}"
        Value="Cards" />
    <via:XButtonGroupItem
        Content="Details"
        Icon="{via:MaterialIcon Kind=ViewHeadline}"
        Value="Details" />
</via:XButtonGroup>

<!-- Vertical layout uses the same selection logic. -->
<via:XButtonGroup
    Orientation="Vertical"
    SelectedIndex="1"
    Variant="Success"
    ItemMinWidth="180">
    <via:XButtonGroupItem Content="Compact rows" />
    <via:XButtonGroupItem Content="Comfortable rows" />
    <via:XButtonGroupItem Content="Spacious rows" />
</via:XButtonGroup>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XButtonGroup group = new()
{
    SelectedValue = "Cards",
    Variant = XControlVariant.Primary,
    ItemMinWidth = 112d,
};

group.Items.Add(new XButtonGroupItem { Content = "List", Value = "List" });
group.Items.Add(new XButtonGroupItem { Content = "Cards", Value = "Cards" });
group.Items.Add(new XButtonGroupItem { Content = "Details", Value = "Details" });
""";
    #endregion
}
#endregion
