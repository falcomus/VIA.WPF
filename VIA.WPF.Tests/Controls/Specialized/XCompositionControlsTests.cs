// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCompositionControlsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XCompositionControlsTests ###
/// <summary>
/// Tests the public behavior of the Modern Workbench composition controls.
/// </summary>
public sealed class XCompositionControlsTests
{
    #region ### Tests ###
    [Fact]
    public void XAdaptiveGrid_ShouldWrapChildrenAtMinimumWidth()
    {
        WpfTestHelper.Run(
            () =>
            {
                XAdaptiveGrid grid = new()
                {
                    MinItemWidth = 100d,
                    MaxColumns = 3,
                    ColumnSpacing = 10d,
                    RowSpacing = 12d
                };

                grid.Children.Add(new Border { Height = 20d });
                grid.Children.Add(new Border { Height = 20d });
                grid.Children.Add(new Border { Height = 20d });

                grid.Measure(new Size(250d, double.PositiveInfinity));
                grid.Arrange(new Rect(0d, 0d, 250d, grid.DesiredSize.Height));

                Assert.Equal(new Size(250d, 52d), grid.DesiredSize);
                Assert.Equal(120d, grid.Children[0].RenderSize.Width);
                Assert.Equal(new Point(130d, 0d), grid.Children[1].TranslatePoint(default, grid));
                Assert.Equal(new Point(0d, 32d), grid.Children[2].TranslatePoint(default, grid));
            });
    }

    [Fact]
    public void XContentStatePresenter_ShouldDefaultToRegularContent()
    {
        WpfTestHelper.Run(
            () =>
            {
                XContentStatePresenter presenter = new();

                Assert.Equal(XContentState.Content, presenter.State);
                Assert.Equal("Retry", presenter.RetryText);
            });
    }

    [Fact]
    public void XInfoBar_CloseCommandShouldRequireClosableOpenBar()
    {
        WpfTestHelper.Run(
            () =>
            {
                XInfoBar infoBar = new();

                Assert.False(infoBar.CloseCommand.CanExecute(null));

                infoBar.IsClosable = true;
                Assert.True(infoBar.CloseCommand.CanExecute(null));

                infoBar.CloseCommand.Execute(null);
                Assert.False(infoBar.IsOpen);
            });
    }

    [Fact]
    public void XMoreButton_ShouldUseQuietOverflowDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XMoreButton button = new();

                Assert.Equal("\u22EE", button.Content);
                Assert.Equal(XControlAppearance.Ghost, button.Appearance);
                Assert.Equal(XControlSize.Small, button.Size);
                Assert.Equal(28d, button.Width);
            });
    }

    [Fact]
    public void XHeaderBar_ShouldExposeCompositionSlots()
    {
        WpfTestHelper.Run(
            () =>
            {
                object breadcrumb = new();
                object actions = new();
                XHeaderBar headerBar = new()
                {
                    Title = "Projects",
                    Subtitle = "Planning overview",
                    Breadcrumb = breadcrumb,
                    Actions = actions
                };

                Assert.Equal("Projects", headerBar.Title);
                Assert.Equal("Planning overview", headerBar.Subtitle);
                Assert.Same(breadcrumb, headerBar.Breadcrumb);
                Assert.Same(actions, headerBar.Actions);
            });
    }
    #endregion
}
#endregion
