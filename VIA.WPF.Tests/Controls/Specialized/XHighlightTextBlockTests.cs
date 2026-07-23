// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHighlightTextBlockTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Documents;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XHighlightTextBlockTests ###
/// <summary>
/// Provides tests for highlight text rendering behavior.
/// </summary>
public sealed class XHighlightTextBlockTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that plain text is used when highlighting is disabled.
    /// </summary>
    [Fact]
    public void HighlightRenderModeNone_ShouldKeepPlainText()
    {
        WpfTestHelper.Run(
            () =>
            {
                XHighlightTextBlock textBlock = new()
                {
                    DisplayText = "Alpha Beta",
                    HighlightText = "Alpha",
                    HighlightRenderMode = XHighlightRenderMode.None
                };

                AssertPlainText(textBlock, "Alpha Beta");
            });
    }

    /// <summary>
    /// Ensures that simple highlight mode creates runs for matches and non-matches.
    /// </summary>
    [Fact]
    public void SimpleHighlight_ShouldCreateHighlightedRuns()
    {
        WpfTestHelper.Run(
            () =>
            {
                XHighlightTextBlock textBlock = new()
                {
                    DisplayText = "Alpha beta alpha",
                    HighlightText = "alpha",
                    HighlightBrush = Brushes.Yellow
                };

                List<Run> runs = [.. textBlock.Inlines.OfType<Run>()];

                Assert.Equal(3, runs.Count);
                Assert.Equal("Alpha", runs[0].Text);
                Assert.Same(Brushes.Yellow, runs[0].Background);
                Assert.Equal(" beta ", runs[1].Text);
                Assert.Null(runs[1].Background);
                Assert.Equal("alpha", runs[2].Text);
                Assert.Same(Brushes.Yellow, runs[2].Background);
            });
    }

    /// <summary>
    /// Ensures that case-sensitive matching only highlights exact-case matches.
    /// </summary>
    [Fact]
    public void SimpleHighlight_ShouldRespectCaseSensitivity()
    {
        WpfTestHelper.Run(
            () =>
            {
                XHighlightTextBlock textBlock = new()
                {
                    DisplayText = "Alpha alpha",
                    HighlightText = "alpha",
                    IsCaseSensitive = true
                };

                List<Run> runs = [.. textBlock.Inlines.OfType<Run>()];

                Assert.Equal(2, runs.Count);
                Assert.Equal("Alpha ", runs[0].Text);
                Assert.Null(runs[0].Background);
                Assert.Equal("alpha", runs[1].Text);
                Assert.NotNull(runs[1].Background);
            });
    }

    /// <summary>
    /// Ensures that badge mode creates inline UI containers for highlighted matches.
    /// </summary>
    [Fact]
    public void BadgeHighlight_ShouldCreateInlineContainers()
    {
        WpfTestHelper.Run(
            () =>
            {
                XHighlightTextBlock textBlock = new()
                {
                    DisplayText = "Alpha beta",
                    HighlightText = "Alpha",
                    HighlightRenderMode = XHighlightRenderMode.Badge
                };

                Assert.Contains(textBlock.Inlines, inline => inline is InlineUIContainer);
            });
    }

    /// <summary>
    /// Ensures that the minimum highlight text length suppresses highlighting.
    /// </summary>
    [Fact]
    public void MinimumHighlightTextLength_ShouldSuppressShortSearchText()
    {
        WpfTestHelper.Run(
            () =>
            {
                XHighlightTextBlock textBlock = new()
                {
                    DisplayText = "Alpha",
                    HighlightText = "Al",
                    MinimumHighlightTextLength = 3
                };

                AssertPlainText(textBlock, "Alpha");
            });
    }
    #endregion

    #region ### Private Methods ###
    private static void AssertPlainText(XHighlightTextBlock textBlock, string expectedText)
    {
        Assert.Equal(expectedText, textBlock.Text);
        Assert.DoesNotContain(textBlock.Inlines, inline => inline is InlineUIContainer);

        List<Run> runs = [.. textBlock.Inlines.OfType<Run>()];

        Assert.Single(runs);
        Assert.Equal(expectedText, runs[0].Text);
        Assert.Null(runs[0].Background);
    }
    #endregion
}
#endregion
