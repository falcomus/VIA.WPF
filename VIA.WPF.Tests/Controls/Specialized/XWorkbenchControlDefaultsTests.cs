// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWorkbenchControlDefaultsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XWorkbenchControlDefaultsTests ###
/// <summary>
/// Tests public defaults introduced by the Modern Workbench contract.
/// </summary>
public sealed class XWorkbenchControlDefaultsTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that cell focus decoration remains opt-in on data grids.
    /// </summary>
    [Fact]
    public void XDataGrid_ShouldHideCurrentCellFocusByDefault()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDataGrid dataGrid = new();

                Assert.False(dataGrid.ShowCurrentCellFocus);

                dataGrid.ShowCurrentCellFocus = true;

                Assert.True(dataGrid.ShowCurrentCellFocus);
            });
    }

    /// <summary>
    /// Verifies the lightweight group composition defaults.
    /// </summary>
    [Fact]
    public void XGroup_ShouldExposeArbitraryCompositionSlots()
    {
        WpfTestHelper.Run(
            () =>
            {
                object actions = new();
                object footer = new();
                XGroup group = new()
                {
                    Title = "Projects",
                    Subtitle = "Current work",
                    Actions = actions,
                    Footer = footer
                };

                Assert.Equal("Projects", group.Title);
                Assert.Equal("Current work", group.Subtitle);
                Assert.Same(actions, group.Actions);
                Assert.Same(footer, group.Footer);
                Assert.Equal(new CornerRadius(4d), group.CornerRadius);
                Assert.Equal(new Thickness(16d), group.ContentPadding);
            });
    }
    #endregion
}
#endregion
