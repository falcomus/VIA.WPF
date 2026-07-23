// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWrapPanelDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XWrapPanelDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XWrapPanel</c>.
/// </summary>
public sealed class XWrapPanelDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XWrapPanel";

    /// <inheritdoc/>
    public override string Description => "Demonstrates wrapping layouts with horizontal and vertical spacing, fixed item dimensions and vertical wrapping.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XWrapPanel HorizontalSpacing="8" VerticalSpacing="8">
    <via:XButton Appearance="Subtle" Content="Design" Size="Small" />
    <via:XButton Appearance="Subtle" Content="Controls" Size="Small" />
    <via:XButton Appearance="Subtle" Content="Themes" Size="Small" />
    <via:XButton Appearance="Subtle" Content="Icons" Size="Small" />
</via:XWrapPanel>

<via:XWrapPanel
    ItemWidth="150"
    ItemHeight="44"
    HorizontalSpacing="10"
    VerticalSpacing="10">
    <via:XBorder>
        <TextBlock Margin="12,0" VerticalAlignment="Center" Text="Fixed item" />
    </via:XBorder>
</via:XWrapPanel>

<via:XWrapPanel
    Orientation="Vertical"
    ItemWidth="120"
    ItemHeight="34"
    HorizontalSpacing="10"
    VerticalSpacing="8" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var wrapPanel = new XWrapPanel
{
    Orientation = Orientation.Horizontal,
    HorizontalSpacing = 8,
    VerticalSpacing = 8,
};

var fixedItemsPanel = new XWrapPanel
{
    ItemWidth = 150,
    ItemHeight = 44,
    HorizontalSpacing = 10,
    VerticalSpacing = 10,
};

var verticalWrapPanel = new XWrapPanel
{
    Orientation = Orientation.Vertical,
    ItemWidth = 120,
    ItemHeight = 34,
    HorizontalSpacing = 10,
    VerticalSpacing = 8,
};
""";
    #endregion
}
#endregion
