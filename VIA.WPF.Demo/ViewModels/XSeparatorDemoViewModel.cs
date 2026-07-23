// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSeparatorDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XSeparatorDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XSeparator</c>.
/// </summary>
public sealed class XSeparatorDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XSeparator";

    /// <inheritdoc/>
    public override string Description => "Demonstrates horizontal and vertical themed separators with adjustable line thickness.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XStackPanel Spacing="10">
    <TextBlock Text="General" />
    <via:XSeparator Orientation="Horizontal" />
    <TextBlock Text="Advanced" />
</via:XStackPanel>

<via:XSeparator
    Orientation="Horizontal"
    LineThickness="2" />

<via:XGrid Columns="*,Auto,*" ColumnSpacing="16">
    <TextBlock Grid.Column="0" Text="Left pane" />
    <via:XSeparator
        Grid.Column="1"
        Orientation="Vertical" />
    <TextBlock Grid.Column="2" Text="Right pane" />
</via:XGrid>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var horizontalSeparator = new XSeparator
{
    Orientation = Orientation.Horizontal,
    LineThickness = 1,
};

var verticalSeparator = new XSeparator
{
    Orientation = Orientation.Vertical,
    LineThickness = 2,
};
""";
    #endregion
}
#endregion
