// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XStackPanelDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XStackPanelDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XStackPanel</c>.
/// </summary>
public sealed class XStackPanelDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XStackPanel";

    /// <inheritdoc/>
    public override string Description => "Demonstrates vertical and horizontal stack layouts with built-in spacing.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XStackPanel Spacing="12">
    <TextBlock Text="First item" />
    <TextBlock Text="Second item" />
    <TextBlock Text="Third item" />
</via:XStackPanel>

<via:XStackPanel Orientation="Horizontal" Spacing="8">
    <via:XButton
        Content="New"
        Icon="{via:MaterialIcon Kind=Plus}"
        Variant="Primary" />
    <via:XButton
        Appearance="Subtle"
        Content="Edit"
        Icon="{via:MaterialIcon Kind=Pencil}" />
    <via:XButton
        Appearance="Subtle"
        Content="Refresh"
        Icon="{via:MaterialIcon Kind=Refresh}" />
</via:XStackPanel>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var verticalPanel = new XStackPanel
{
    Orientation = Orientation.Vertical,
    Spacing = 12,
};

verticalPanel.Children.Add(new TextBlock { Text = "First item" });
verticalPanel.Children.Add(new TextBlock { Text = "Second item" });

var toolbar = new XStackPanel
{
    Orientation = Orientation.Horizontal,
    Spacing = 8,
};

toolbar.Children.Add(new XButton { Content = "New", Variant = XControlVariant.Primary });
toolbar.Children.Add(new XButton { Content = "Refresh", Appearance = XControlAppearance.Subtle });
""";
    #endregion
}
#endregion
