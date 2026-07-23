// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSplitViewDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XSplitViewDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XSplitView showcase page.
/// </summary>
public sealed class XSplitViewDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XSplitView";

    /// <inheritdoc/>
    public override string Description => "Demonstrates horizontal and vertical resizable two-pane layouts.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XSplitView
    Orientation="Horizontal"
    FirstLength="260"
    MinFirstLength="160"
    MinSecondLength="220"
    SplitterThickness="6"
    ShowsPreview="True">
    <via:XSplitView.FirstContent>
        <TextBlock Text="Navigation pane" />
    </via:XSplitView.FirstContent>
    <via:XSplitView.SecondContent>
        <TextBlock Text="Content pane" />
    </via:XSplitView.SecondContent>
</via:XSplitView>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XSplitView splitView = new()
{
    Orientation = Orientation.Horizontal,
    FirstLength = new GridLength(260d),
    MinFirstLength = 160d,
    MinSecondLength = 220d,
    SplitterThickness = 6d,
    ShowsPreview = true,
    FirstContent = new TextBlock { Text = "Navigation pane" },
    SecondContent = new TextBlock { Text = "Content pane" },
};
""";
    #endregion
}
#endregion
