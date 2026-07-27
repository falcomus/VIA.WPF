// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWorkbenchControlDefaultsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
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
    /// Verifies that primitive layout controls do not add implicit content spacing.
    /// </summary>
    [Fact]
    public void PrimitiveLayoutControls_ShouldUseNeutralPaddingDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                ResourceDictionary borderDictionary = new()
                {
                    Source = new Uri(
                        "/VIA.WPF.Controls;component/Themes/XBorder.xaml",
                        UriKind.Relative)
                };

                ResourceDictionary tabControlDictionary = new()
                {
                    Source = new Uri(
                        "/VIA.WPF.Controls;component/Themes/XTabControl.xaml",
                        UriKind.Relative)
                };

                Style borderStyle = Assert.IsType<Style>(borderDictionary[typeof(XBorder)]);
                Style tabControlStyle = Assert.IsType<Style>(tabControlDictionary[typeof(XTabControl)]);

                Setter borderPaddingSetter = Assert.Single(
                    borderStyle.Setters
                        .OfType<Setter>()
                        .Where(setter => setter.Property == Control.PaddingProperty));

                Setter tabControlPaddingSetter = Assert.Single(
                    tabControlStyle.Setters
                        .OfType<Setter>()
                        .Where(setter => setter.Property == Control.PaddingProperty));

                Assert.Equal(
                    new Thickness(0d),
                    Assert.IsType<Thickness>(borderPaddingSetter.Value));

                Assert.Equal(
                    new Thickness(0d),
                    Assert.IsType<Thickness>(tabControlPaddingSetter.Value));
            });
    }

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
