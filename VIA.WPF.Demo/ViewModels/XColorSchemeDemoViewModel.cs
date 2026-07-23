// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XColorSchemeDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XColorSchemeDemoViewModel ###
/// <summary>
/// Represents the design-system color scheme showcase page.
/// </summary>
public sealed class XColorSchemeDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "Color Scheme";

    /// <inheritdoc />
    public override string Description => "Shows the active VIA.WPF theme tokens for brand colors, neutral surfaces, states and application chrome.";

    /// <inheritdoc />
    public override string XamlCode => """
<Border Background="{DynamicResource {x:Static via:XBrushKeys.Primary}}" />
<Border Background="{DynamicResource {x:Static via:XBrushKeys.Surface}}" />
<Border Background="{DynamicResource {x:Static via:XBrushKeys.HoverBackground}}" />

<via:XTextBlock
    Text="Primary text"
    Variant="Primary" />

<via:XBorder
    Padding="16"
    Appearance="VerySubtle"
    BorderThickness="1"
    CornerRadius="12">
    <via:XTextBlock
        IsMultiline="True"
        Text="Surfaces, borders and text colors are always read from theme resources."
        TextRole="Description" />
</via:XBorder>
""";

    /// <inheritdoc />
    public override string CSharpCode => """
// Color scheme pages should not hard-code colors.
// Use dynamic theme resources so theme mode and preset changes are reflected immediately.
//
// Recommended access pattern in XAML:
// {DynamicResource {x:Static via:XBrushKeys.Primary}}
// {DynamicResource {x:Static via:XBrushKeys.Surface}}
// {DynamicResource {x:Static via:XBrushKeys.Border}}
""";
    #endregion
}
#endregion
