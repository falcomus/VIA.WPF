// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGridDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XGridDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XGrid</c>.
/// </summary>
public sealed class XGridDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XGrid";

    /// <inheritdoc/>
    public override string Description => "Demonstrates compact row and column definitions, logical spacing, spans and semantic named areas for readable layouts.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XGrid
    Rows="Auto,*,Auto"
    Columns="150,*,220"
    RowSpacing="12"
    ColumnSpacing="12"
    Areas="header header header; nav content aside; footer footer footer">

    <via:XBorder via:XGrid.Area="header">
        <TextBlock Margin="14" Text="Header" />
    </via:XBorder>

    <via:XBorder via:XGrid.Area="nav">
        <TextBlock Margin="14" Text="Navigation" />
    </via:XBorder>

    <via:XBorder via:XGrid.Area="content">
        <TextBlock Margin="14" Text="Content" />
    </via:XBorder>

    <via:XBorder via:XGrid.Area="aside">
        <TextBlock Margin="14" Text="Aside" />
    </via:XBorder>

    <via:XBorder via:XGrid.Area="footer">
        <TextBlock Margin="14" Text="Footer" />
    </via:XBorder>
</via:XGrid>

<via:XGrid
    Rows="Auto,Auto,Auto"
    Columns="140,*,Auto"
    RowSpacing="10"
    ColumnSpacing="12">

    <TextBlock
        Grid.Row="0"
        Grid.Column="0"
        Text="Endpoint" />

    <via:XTextBox
        Grid.Row="0"
        Grid.Column="1"
        Grid.ColumnSpan="2"
        Text="https://api.via.dev" />

    <TextBlock
        Grid.Row="1"
        Grid.Column="0"
        Text="Environment" />

    <ComboBox
        Grid.Row="1"
        Grid.Column="1" />

    <via:XButton
        Grid.Row="1"
        Grid.Column="2"
        Content="Connect"
        Icon="{via:MaterialIcon Kind=OpenInNew}"
        Variant="Primary" />
</via:XGrid>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
// XGrid supports compact row and column definitions.
var grid = new XGrid
{
    Rows = "Auto,*,Auto",
    Columns = "150,*,220",
    RowSpacing = 12,
    ColumnSpacing = 12,
    Areas = "header header header; nav content aside; footer footer footer",
};

// Logical positioning uses standard WPF Grid attached properties.
Grid.SetRow(someElement, 1);
Grid.SetColumn(someElement, 2);
Grid.SetRowSpan(someElement, 2);
Grid.SetColumnSpan(someElement, 3);

// Semantic placement via named areas.
XGrid.SetArea(headerElement, "header");
XGrid.SetArea(contentElement, "content");
XGrid.SetArea(footerElement, "footer");

// Notes:
// - Supported definitions include Auto, *, 2*, 3* and fixed pixel values.
// - RowSpacing and ColumnSpacing keep standard Grid.Row/Grid.Column indexing logical.
// - XGrid.Area takes precedence over explicit Grid.Row / Grid.Column placement.
// - Empty area cells can be written as '.', '-' or '_'.
""";
    #endregion
}
#endregion


