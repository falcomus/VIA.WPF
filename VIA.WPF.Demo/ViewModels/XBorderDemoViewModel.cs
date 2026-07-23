// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBorderDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XBorderDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XBorder</c>.
/// </summary>
public sealed class XBorderDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XBorder";

    /// <inheritdoc/>
    public override string Description => "Demonstrates semantic surfaces with variants, appearances, corner radius and elevation.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XBorder
    Appearance="VerySubtle"
    BorderThickness="1"
    Variant="Info">
    <TextBlock Margin="12" Text="Informational highlight" />
</via:XBorder>

<via:XBorder
    Appearance="Subtle"
    BorderThickness="1"
    CornerRadius="10"
    Elevation="Medium"
    Variant="Success">
    <TextBlock Margin="12" Text="Elevated success surface" />
</via:XBorder>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var border = new XBorder
{
    Variant = XControlVariant.Info,
    Appearance = XControlAppearance.VerySubtle,
    BorderThickness = new Thickness(1),
    CornerRadius = new CornerRadius(10),
    Elevation = XElevation.Medium,
    Content = new TextBlock
    {
        Margin = new Thickness(12),
        Text = "Informational highlight",
    },
};
""";
    #endregion
}
#endregion
