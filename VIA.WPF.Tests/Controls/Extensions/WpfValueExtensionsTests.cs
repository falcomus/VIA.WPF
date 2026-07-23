// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfValueExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class WpfValueExtensionsTests ###
/// <summary>
/// Provides tests for WPF value extension helpers.
/// </summary>
public sealed class WpfValueExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that thickness helpers calculate horizontal and vertical values.
    /// </summary>
    [Fact]
    public void ThicknessExtensions_ShouldCalculateHorizontalAndVerticalValues()
    {
        Thickness thickness = new(1d, 2d, 3d, 4d);

        Assert.Equal(4d, thickness.Horizontal());
        Assert.Equal(6d, thickness.Vertical());
    }

    /// <summary>
    /// Ensures that thickness helpers return adjusted copies without mutating the original value.
    /// </summary>
    [Fact]
    public void ThicknessExtensions_ShouldCreateAdjustedCopies()
    {
        Thickness thickness = new(1d, 2d, 3d, 4d);

        Assert.Equal(new Thickness(10d, 2d, 3d, 4d), thickness.WithLeft(10d));
        Assert.Equal(new Thickness(1d, 20d, 3d, 4d), thickness.WithTop(20d));
        Assert.Equal(new Thickness(1d, 2d, 30d, 4d), thickness.WithRight(30d));
        Assert.Equal(new Thickness(1d, 2d, 3d, 40d), thickness.WithBottom(40d));
        Assert.Equal(new Thickness(6d, 8d, 10d, 12d), thickness.Add(new Thickness(5d, 6d, 7d, 8d)));
        Assert.Equal(new Thickness(2d, 4d, 6d, 8d), thickness.Scale(2d));
        Assert.Equal(new Thickness(1d, 2d, 3d, 4d), thickness);
    }

    /// <summary>
    /// Ensures that rectangle helpers calculate center, finite state and adjusted rectangles.
    /// </summary>
    [Fact]
    public void RectExtensions_ShouldCalculateCenterFiniteStateAndInflatedCopies()
    {
        Rect rect = new(10d, 20d, 30d, 40d);

        Assert.Equal(new Point(25d, 40d), rect.GetCenter());
        Assert.True(rect.IsFinite());
        Assert.False(Rect.Empty.IsFinite());
        Assert.False(new Rect(double.NaN, 0d, 1d, 1d).IsFinite());
        Assert.False(new Rect(0d, 0d, double.PositiveInfinity, 1d).IsFinite());
        Assert.Equal(new Rect(5d, 10d, 40d, 60d), rect.Inflated(5d, 10d));
        Assert.Equal(new Rect(15d, 30d, 20d, 20d), rect.Deflated(5d, 10d));
        Assert.Equal(new Rect(10d, 20d, 30d, 40d), rect);
    }

    /// <summary>
    /// Ensures that freezable helpers freeze values and frozen clones when possible.
    /// </summary>
    [Fact]
    public void FreezableExtensions_ShouldFreezeValuesAndClones()
    {
        SolidColorBrush brush = new(Colors.Red);

        SolidColorBrush result = brush.FreezeIfPossible();

        Assert.Same(brush, result);
        Assert.True(brush.IsFrozen);

        SolidColorBrush source = new(Colors.Blue);
        SolidColorBrush clone = source.CloneCurrentValueFrozen();

        Assert.NotSame(source, clone);
        Assert.Equal(source.Color, clone.Color);
        Assert.True(clone.IsFrozen);
        Assert.False(source.IsFrozen);
    }

    /// <summary>
    /// Ensures that freezable helpers reject null values.
    /// </summary>
    [Fact]
    public void FreezableExtensions_ShouldRejectNullValues()
    {
        SolidColorBrush? brush = null;

        Assert.Throws<ArgumentNullException>(() => brush!.FreezeIfPossible());
        Assert.Throws<ArgumentNullException>(() => brush!.CloneCurrentValueFrozen());
    }

    /// <summary>
    /// Ensures that basic UI element helpers update visibility and hit testing while returning the same instance.
    /// </summary>
    [Fact]
    public void UIElementExtensions_ShouldSetVisibilityAndHitTesting()
    {
        WpfTestHelper.Run(
            () =>
            {
                FrameworkElement element = new();

                element.SetVisible(true);
                Assert.Equal(Visibility.Visible, element.Visibility);

                element.SetVisible(false);
                Assert.Equal(Visibility.Collapsed, element.Visibility);

                element.SetHitTestVisible(true);
                Assert.True(element.IsHitTestVisible);

                element.SetHitTestVisible(false);
                Assert.False(element.IsHitTestVisible);
            });
    }

    /// <summary>
    /// Ensures that UI element helpers reject null values.
    /// </summary>
    [Fact]
    public void UIElementExtensions_ShouldRejectNullValues()
    {
        Border? element = null;

        Assert.Throws<ArgumentNullException>(() => element!.SetVisible(true));
        Assert.Throws<ArgumentNullException>(() => element!.SetHiddenWhenInvisible(true));
        Assert.Throws<ArgumentNullException>(() => element!.SetHitTestVisible(true));
        Assert.Throws<ArgumentNullException>(() => element!.ReleaseMouseCaptureIfCaptured());
    }
    #endregion
}
#endregion
