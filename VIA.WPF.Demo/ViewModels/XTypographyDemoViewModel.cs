// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTypographyDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTypographyDemoViewModel ###
/// <summary>
/// Represents the design-system typography showcase page.
/// </summary>
public sealed class XTypographyDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "Typography";

    /// <inheritdoc />
    public override string Description => "Shows the VIA.WPF typography scale, semantic XTextBlock roles and practical text patterns used across demos and applications.";

    /// <inheritdoc />
    public override string XamlCode => """
<via:XTextBlock Text="Inventory control" TextRole="Overline" />
<via:XTextBlock Text="Typography" TextRole="Display" />
<via:XTextBlock
    IsMultiline="True"
    Text="Use semantic text roles instead of local FontSize, FontWeight and Foreground values."
    TextRole="Subtitle" />

<via:XTextBlock Text="Section title" TextRole="SectionTitle" />
<via:XTextBlock Text="Form label" TextRole="Label" />
<via:XTextBlock Text="Default body copy." />
<via:XTextBlock Text="Secondary helper text." TextRole="Description" />
<via:XTextBlock Text="Compact metadata" TextRole="Caption" />
<via:XTextBlock Text="dotnet build VIA.WPF" TextRole="Code" />

<via:XTextBlock Text="Primary" Variant="Primary" />
<via:XTextBlock Text="Success" TextRole="Success" />
<via:XTextBlock Text="Warning" TextRole="Warning" />
<via:XTextBlock Text="Danger" TextRole="Error" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
// Typography is intentionally theme-driven.
// Application code should normally use XTextBlock roles instead of local TextBlock styles.
//
// Main roles:
// Display, Title, Subtitle, SectionTitle, Label, Body, Description, Caption, Code
//
// Semantic variants:
// Primary, Accent, Success, Warning, Danger, Info
""";
    #endregion
}
#endregion
