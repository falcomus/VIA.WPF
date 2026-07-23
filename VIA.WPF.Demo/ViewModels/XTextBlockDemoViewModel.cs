// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTextBlockDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTextBlockDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XTextBlock showcase page.
/// </summary>
public sealed class XTextBlockDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the page title.
    /// </summary>
    public override string Title => "XTextBlock";

    /// <summary>
    /// Gets the page description.
    /// </summary>
    public override string Description => "Demonstrates semantic text roles, shared sizes, variants and multiline helper text with the lightweight XTextBlock control.";

    /// <summary>
    /// Gets the displayed XAML source.
    /// </summary>
    public override string XamlCode => """
<via:XTextBlock
    Text="Semantic text roles"
    TextRole="SectionTitle" />

<via:XTextBlock
    Text="XTextBlock keeps typography consistent without repeating FontSize, FontWeight, Foreground and TextWrapping on every TextBlock."
    TextRole="Description" />

<via:XTextBlock Text="Title" TextRole="Title" />
<via:XTextBlock Text="Subtitle text for pages, panels and dialogs." TextRole="Subtitle" />
<via:XTextBlock Text="Body text uses the standard surface foreground." />
<via:XTextBlock Text="Error text" TextRole="Error" />

<via:XTextBlock
    Width="360"
    IsMultiline="True"
    Text="Set IsMultiline to true when explanatory text should wrap naturally." />
""";

    /// <summary>
    /// Gets the displayed C# source.
    /// </summary>
    public override string CSharpCode => """
// XTextBlock derives from TextBlock and adds only VIA.WPF-specific convenience properties.
// Typical usage stays declarative in XAML:
//
// TextRole    -> Title, Subtitle, SectionTitle, Body, Description, Caption, Error, Success, Warning, Info, Code
// Size        -> Small, Medium, Large
// Variant     -> Default, Primary, Accent, Success, Warning, Danger, Info
// IsMultiline -> switches wrapping/trimming defaults for long helper text
""";
    #endregion
}
#endregion
