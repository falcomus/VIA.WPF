// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNumberBoxTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XNumberBoxTests ###
/// <summary>
/// Provides tests for number box value, text and range synchronization.
/// </summary>
public sealed class XNumberBoxTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that text input updates the numeric value.
    /// </summary>
    [Fact]
    public void Text_ShouldSynchronizeValue()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new();

                numberBox.Text = "42";

                Assert.Equal(42d, numberBox.Value);
            });
    }

    /// <summary>
    /// Ensures that empty text clears the numeric value.
    /// </summary>
    [Fact]
    public void Text_ShouldClearValueWhenEmpty()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new()
                {
                    Value = 42d
                };

                numberBox.Text = string.Empty;

                Assert.Null(numberBox.Value);
            });
    }

    /// <summary>
    /// Ensures that invalid text does not overwrite the current numeric value.
    /// </summary>
    [Fact]
    public void Text_ShouldIgnoreInvalidNumericInput()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new()
                {
                    Value = 12d
                };

                numberBox.Text = "not a number";

                Assert.Equal(12d, numberBox.Value);
            });
    }

    /// <summary>
    /// Ensures that assigned values are clamped to the configured range.
    /// </summary>
    [Fact]
    public void Value_ShouldBeCoercedToConfiguredRange()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new()
                {
                    Minimum = 10d,
                    Maximum = 20d
                };

                numberBox.Value = 30d;
                Assert.Equal(20d, numberBox.Value);

                numberBox.Value = -5d;
                Assert.Equal(10d, numberBox.Value);
            });
    }

    /// <summary>
    /// Ensures that range changes keep minimum and maximum consistent.
    /// </summary>
    [Fact]
    public void Range_ShouldKeepMinimumAndMaximumConsistent()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new();

                numberBox.Minimum = 150d;

                Assert.Equal(150d, numberBox.Minimum);
                Assert.Equal(150d, numberBox.Maximum);

                numberBox.Maximum = 50d;

                Assert.Equal(50d, numberBox.Minimum);
                Assert.Equal(50d, numberBox.Maximum);
            });
    }

    /// <summary>
    /// Ensures that changing the format string updates the displayed text for the current value.
    /// </summary>
    [Fact]
    public void FormatString_ShouldUpdateDisplayedText()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNumberBox numberBox = new()
                {
                    Value = 3d,
                    FormatString = "F2"
                };

                Assert.Equal(3d.ToString("F2", CultureInfo.CurrentCulture), numberBox.Text);
            });
    }
    #endregion
}
#endregion
