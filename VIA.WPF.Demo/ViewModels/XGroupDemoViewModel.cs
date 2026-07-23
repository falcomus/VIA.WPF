// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGroupDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XGroupDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XGroup</c>.
/// </summary>
public sealed class XGroupDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XGroup";

    /// <inheritdoc/>
    public override string Description => "Demonstrates XGroup as the standard content container with title, subtitle, arbitrary actions, content and footer.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XGroup
    Title="Customers"
    Subtitle="Arbitrary actions keep the container independent.">
    <via:XGroup.Actions>
        <via:XStackPanel Orientation="Horizontal" Spacing="6">
            <via:XButton Content="New" Size="Small" />
            <via:XIconButton Icon="{via:MaterialIcon Kind=Refresh}" />
            <via:XMoreButton />
        </via:XStackPanel>
    </via:XGroup.Actions>
    <TextBlock Text="Structured content area" />
</via:XGroup>

<via:XGroup
    Title="Deployment"
    Subtitle="Footer commands remain attached to their content.">
    <via:XGroup.Footer>
        <via:XStackPanel Orientation="Horizontal" Spacing="8">
            <via:XButton
                Content="Deploy"
                Icon="{via:MaterialIcon Kind=Play}"
                Size="Small"
                Variant="Primary" />
            <via:XButton Appearance="Subtle" Content="Cancel" Size="Small" />
        </via:XStackPanel>
    </via:XGroup.Footer>

    <TextBlock Text="Ready for production" />
</via:XGroup>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var group = new XGroup
{
    Title = "Customers",
    Subtitle = "Arbitrary actions keep the container independent.",
    Actions = CreateHeaderActions(),
    Content = new TextBlock { Text = "Structured content area" },
};

group.Footer = new XStackPanel
{
    Orientation = Orientation.Horizontal,
    Spacing = 8,
};
""";
    #endregion
}
#endregion
