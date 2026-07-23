// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHighlightTextBlockDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XHighlightTextBlockDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XHighlightTextBlock showcase page.
/// </summary>
public sealed partial class XHighlightTextBlockDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XHighlightTextBlock";

    /// <inheritdoc/>
    public override string Description => "Demonstrates inline and badge-style highlighting for searchable lists, grids and preview snippets.";

    /// <summary>
    /// Gets or sets the highlighted search term.
    /// </summary>
    [ObservableProperty]
    private string _highlightTerm = "highlights";

    /// <summary>
    /// Gets or sets a value indicating whether matching is case-sensitive.
    /// </summary>
    [ObservableProperty]
    private bool _isCaseSensitive;

    /// <summary>
    /// Gets the first sample sentence.
    /// </summary>
    public string SampleSentence => "VIA.WPF highlights matching search fragments inside longer text.";

    /// <summary>
    /// Gets the second sample sentence.
    /// </summary>
    public string SearchResultSentence => "The same control can be used inside list items, grids and command palettes.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XHighlightTextBlock
    DisplayText="VIA.WPF highlights matching search fragments inside longer text."
    HighlightText="highlights" />

<via:XHighlightTextBlock
    DisplayText="Search result highlighting can use rounded badges."
    HighlightRenderMode="Badge"
    HighlightCornerRadius="5"
    HighlightPadding="4,1"
    HighlightText="Search" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XHighlightTextBlock textBlock = new()
{
    DisplayText = "VIA.WPF highlights matching search fragments inside longer text.",
    HighlightText = "lib",
    HighlightRenderMode = XHighlightRenderMode.Badge,
    HighlightCornerRadius = new CornerRadius(5d),
    HighlightPadding = new Thickness(4d, 1d, 4d, 1d),
};
""";
    #endregion
}
#endregion
