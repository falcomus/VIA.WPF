// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XComboBoxTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
    #endregion
}
#endregion
