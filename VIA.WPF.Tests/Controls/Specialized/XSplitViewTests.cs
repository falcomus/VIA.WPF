// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSplitViewTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XSplitViewTests ###
/// <summary>
/// Provides tests for split view defaults and dependency property metadata.
/// </summary>
public sealed class XSplitViewTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that the default split view state is stable.
    /// </summary>
    [Fact]
    public void Constructor_ShouldExposeExpectedDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XSplitView splitView = new();

                Assert.Equal(Orientation.Horizontal, splitView.Orientation);
                Assert.Equal(new GridLength(320d), splitView.FirstLength);
                Assert.Equal(new GridLength(1d, GridUnitType.Star), splitView.SecondLength);
                Assert.Equal(120d, splitView.MinFirstLength);
                Assert.Equal(120d, splitView.MinSecondLength);
                Assert.Equal(12d, splitView.SplitterThickness);
                Assert.Equal(2d, splitView.SplitterSpacing);
                Assert.True(splitView.ShowsPreview);
                Assert.Null(splitView.FirstContent);
                Assert.Null(splitView.SecondContent);
            });
    }

    /// <summary>
    /// Ensures that content and length properties roundtrip.
    /// </summary>
    [Fact]
    public void Properties_ShouldRoundtripAssignedValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                object firstContent = new();
                object secondContent = new();
                XSplitView splitView = new()
                {
                    Orientation = Orientation.Vertical,
                    FirstContent = firstContent,
                    SecondContent = secondContent,
                    FirstLength = new GridLength(2d, GridUnitType.Star),
                    SecondLength = new GridLength(240d),
                    MinFirstLength = 80d,
                    MinSecondLength = 90d,
                    SplitterThickness = 7d,
                    SplitterSpacing = 3d,
                    ShowsPreview = false
                };

                Assert.Equal(Orientation.Vertical, splitView.Orientation);
                Assert.Same(firstContent, splitView.FirstContent);
                Assert.Same(secondContent, splitView.SecondContent);
                Assert.Equal(new GridLength(2d, GridUnitType.Star), splitView.FirstLength);
                Assert.Equal(new GridLength(240d), splitView.SecondLength);
                Assert.Equal(80d, splitView.MinFirstLength);
                Assert.Equal(90d, splitView.MinSecondLength);
                Assert.Equal(7d, splitView.SplitterThickness);
                Assert.Equal(3d, splitView.SplitterSpacing);
                Assert.False(splitView.ShowsPreview);
            });
    }

    /// <summary>
    /// Ensures that pane lengths bind two-way by default.
    /// </summary>
    [Fact]
    public void LengthProperties_ShouldBindTwoWayByDefault()
    {
        FrameworkPropertyMetadata firstMetadata = Assert.IsType<FrameworkPropertyMetadata>(
            XSplitView.FirstLengthProperty.GetMetadata(typeof(XSplitView)));
        FrameworkPropertyMetadata secondMetadata = Assert.IsType<FrameworkPropertyMetadata>(
            XSplitView.SecondLengthProperty.GetMetadata(typeof(XSplitView)));

        Assert.True(firstMetadata.BindsTwoWayByDefault);
        Assert.True(secondMetadata.BindsTwoWayByDefault);
    }
    #endregion
}
#endregion
