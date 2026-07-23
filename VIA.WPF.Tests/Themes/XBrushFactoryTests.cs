// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBrushFactoryTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XBrushFactoryTests ###
/// <summary>
/// Tests brush factory behavior.
/// </summary>
public sealed class XBrushFactoryTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that runtime brushes remain mutable for runtime resource replacement and animation scenarios.
    /// </summary>
    [Fact]
    public void CreateRuntimeBrush_ShouldCreateMutableBrush()
    {
        Color color = Color.FromRgb(1, 2, 3);

        SolidColorBrush brush = XBrushFactory.CreateRuntimeBrush(color);

        Assert.Equal(color, brush.Color);
        Assert.False(brush.IsFrozen);
    }

    /// <summary>
    /// Verifies that frozen brushes are frozen and keep the supplied color.
    /// </summary>
    [Fact]
    public void CreateFrozenBrush_WithColor_ShouldCreateFrozenBrush()
    {
        Color color = Color.FromRgb(4, 5, 6);

        SolidColorBrush brush = XBrushFactory.CreateFrozenBrush(color);

        Assert.Equal(color, brush.Color);
        Assert.True(brush.IsFrozen);
    }

    /// <summary>
    /// Verifies that the RGB overload creates an opaque frozen brush.
    /// </summary>
    [Fact]
    public void CreateFrozenBrush_WithRgbChannels_ShouldCreateOpaqueFrozenBrush()
    {
        SolidColorBrush brush = XBrushFactory.CreateFrozenBrush(7, 8, 9);

        Assert.Equal(Color.FromRgb(7, 8, 9), brush.Color);
        Assert.True(brush.IsFrozen);
    }

    /// <summary>
    /// Verifies that the ARGB overload keeps the alpha channel.
    /// </summary>
    [Fact]
    public void CreateFrozenBrush_WithArgbChannels_ShouldCreateTranslucentFrozenBrush()
    {
        SolidColorBrush brush = XBrushFactory.CreateFrozenBrush(128, 10, 11, 12);

        Assert.Equal(Color.FromArgb(128, 10, 11, 12), brush.Color);
        Assert.True(brush.IsFrozen);
    }

    /// <summary>
    /// Verifies that freezable instances are frozen when possible.
    /// </summary>
    [Fact]
    public void FreezeIfPossible_ShouldFreezeFreezableWhenPossible()
    {
        SolidColorBrush brush = new(Colors.Red);

        SolidColorBrush result = XBrushFactory.FreezeIfPossible(brush);

        Assert.Same(brush, result);
        Assert.True(result.IsFrozen);
    }
    #endregion
}
#endregion
