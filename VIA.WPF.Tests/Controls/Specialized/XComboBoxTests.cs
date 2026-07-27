// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XComboBoxTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XComboBoxTests ###
/// <summary>
/// Tests the workbench defaults and compatibility aliases of <see cref="XComboBox"/>.
/// </summary>
public sealed class XComboBoxTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the standard visible item count and clear-selection defaults.
    /// </summary>
    [Fact]
    public void Constructor_ShouldUseWorkbenchDefaults()
    {
        WpfTestHelper.Run(
            () =>
            {
                XComboBox comboBox = new();

                Assert.Equal(10, comboBox.MaxVisibleItems);
                Assert.Equal(308d, comboBox.MaxDropDownHeight);
                Assert.False(comboBox.CanClearSelection);
                Assert.Equal("No entries", comboBox.EmptyText);
            });
    }

    /// <summary>
    /// Verifies that visible item counts are constrained to supported values.
    /// </summary>
    [Fact]
    public void MaxVisibleItems_ShouldBeCoercedAndUpdatePopupHeight()
    {
        WpfTestHelper.Run(
            () =>
            {
                XComboBox comboBox = new();

                comboBox.MaxVisibleItems = 0;
                Assert.Equal(1, comboBox.MaxVisibleItems);
                Assert.Equal(38d, comboBox.MaxDropDownHeight);

                comboBox.MaxVisibleItems = 200;
                Assert.Equal(100, comboBox.MaxVisibleItems);
            });
    }

    /// <summary>
    /// Verifies that the preferred clear-selection property retains compatibility with earlier names.
    /// </summary>
    [Fact]
    public void CanClearSelection_ShouldAliasCompatibilityProperties()
    {
        WpfTestHelper.Run(
            () =>
            {
                XComboBox comboBox = new();

                comboBox.CanClearSelection = true;

                Assert.True(comboBox.ShowResetButton);
                Assert.True(comboBox.HasClearButton);

                comboBox.ShowResetButton = false;

                Assert.False(comboBox.CanClearSelection);
            });
    }

    /// <summary>
    /// Verifies that semantic sizes apply distinct workbench metrics to a rendered combo box.
    /// </summary>
    [Fact]
    public void Size_ShouldApplyDistinctRenderedMetrics()
    {
        WpfTestHelper.Run(
            () =>
            {
                XComboBox small = new()
                {
                    Size = XControlSize.Small,
                    SelectedIndex = 0
                };
                small.Items.Add("Small");

                XComboBox medium = new()
                {
                    Size = XControlSize.Medium,
                    SelectedIndex = 0
                };
                medium.Items.Add("Medium");

                XComboBox large = new()
                {
                    Size = XControlSize.Large,
                    SelectedIndex = 0
                };
                large.Items.Add("Large");

                StackPanel panel = new()
                {
                    Margin = new Thickness(16d)
                };
                panel.Children.Add(small);
                panel.Children.Add(medium);
                panel.Children.Add(large);

                Window host = new()
                {
                    Width = 320d,
                    Height = 220d,
                    ShowInTaskbar = false,
                    Content = panel
                };

                try
                {
                    host.Show();
                    host.UpdateLayout();

                    Assert.Equal(XControlSizeMetrics.SmallHeight, small.MinHeight);
                    Assert.Equal(new Thickness(8d, 2d, 8d, 2d), small.Padding);
                    Assert.Equal(XControlSizeMetrics.SmallIconSize, small.ResetIconSize);
                    Assert.Equal(XControlSizeMetrics.SmallCornerRadius, small.CornerRadius);

                    Assert.Equal(XControlSizeMetrics.MediumHeight, medium.MinHeight);
                    Assert.Equal(new Thickness(10d, 4d, 10d, 4d), medium.Padding);
                    Assert.Equal(XControlSizeMetrics.MediumIconSize, medium.ResetIconSize);
                    Assert.Equal(XControlSizeMetrics.MediumCornerRadius, medium.CornerRadius);

                    Assert.Equal(XControlSizeMetrics.LargeHeight, large.MinHeight);
                    Assert.Equal(new Thickness(12d, 6d, 12d, 6d), large.Padding);
                    Assert.Equal(XControlSizeMetrics.LargeIconSize, large.ResetIconSize);
                    Assert.Equal(XControlSizeMetrics.LargeCornerRadius, large.CornerRadius);

                    Assert.True(small.ActualHeight < medium.ActualHeight);
                    Assert.True(medium.ActualHeight < large.ActualHeight);
                }
                finally
                {
                    host.Close();
                }
            });
    }
    #endregion
}
#endregion
