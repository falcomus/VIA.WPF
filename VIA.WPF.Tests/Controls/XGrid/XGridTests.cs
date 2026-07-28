// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGridTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.XGrid;

#region ### Class XGridTests ###
/// <summary>
/// Provides targeted tests for <see cref="VIA.WPF.Controls.XGrid"/> compact definitions and logical child placement.
/// </summary>
public sealed class XGridTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that default values are initialized as expected.
    /// </summary>
    [Fact]
    public void XGrid_ShouldExposeDefaultValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new();

                Assert.Equal(string.Empty, grid.Rows);
                Assert.Equal(string.Empty, grid.Columns);
                Assert.Equal(string.Empty, grid.Areas);
                Assert.Equal(0d, grid.RowSpacing);
                Assert.Equal(0d, grid.ColumnSpacing);
                Assert.Empty(grid.RowDefinitions);
                Assert.Empty(grid.ColumnDefinitions);
            });
    }

    /// <summary>
    /// Ensures that native definitions supplied by XAML or a markup extension survive loading.
    /// </summary>
    [Fact]
    public void XGrid_ShouldPreserveNativeDefinitionsWhenCompactSyntaxIsUnused()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1d, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30d) });

                Window host = new()
                {
                    Width = 400d,
                    Height = 200d,
                    Content = grid
                };

                try
                {
                    host.Show();
                    host.UpdateLayout();

                    Assert.Equal(2, grid.RowDefinitions.Count);
                    Assert.Equal(GridUnitType.Star, grid.RowDefinitions[0].Height.GridUnitType);
                    Assert.Equal(30d, grid.RowDefinitions[1].Height.Value);
                }
                finally
                {
                    host.Close();
                }
            });
    }

    /// <summary>
    /// Ensures that compact row and column definitions are parsed into WPF definitions.
    /// </summary>
    [Fact]
    public void XGrid_ShouldParseRowsAndColumns()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*,2*,120",
                    Columns = "50,Auto,*"
                };

                Assert.Equal(4, grid.RowDefinitions.Count);
                Assert.True(grid.RowDefinitions[0].Height.IsAuto);
                Assert.Equal(GridUnitType.Star, grid.RowDefinitions[1].Height.GridUnitType);
                Assert.Equal(1d, grid.RowDefinitions[1].Height.Value);
                Assert.Equal(GridUnitType.Star, grid.RowDefinitions[2].Height.GridUnitType);
                Assert.Equal(2d, grid.RowDefinitions[2].Height.Value);
                Assert.Equal(GridUnitType.Pixel, grid.RowDefinitions[3].Height.GridUnitType);
                Assert.Equal(120d, grid.RowDefinitions[3].Height.Value);

                Assert.Equal(3, grid.ColumnDefinitions.Count);
                Assert.Equal(GridUnitType.Pixel, grid.ColumnDefinitions[0].Width.GridUnitType);
                Assert.Equal(50d, grid.ColumnDefinitions[0].Width.Value);
                Assert.True(grid.ColumnDefinitions[1].Width.IsAuto);
                Assert.Equal(GridUnitType.Star, grid.ColumnDefinitions[2].Width.GridUnitType);
            });
    }

    /// <summary>
    /// Ensures that row and column spacing insert physical spacer definitions.
    /// </summary>
    [Fact]
    public void XGrid_ShouldInsertSpacingDefinitions()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*",
                    Columns = "100,*",
                    RowSpacing = 8d,
                    ColumnSpacing = 12d
                };

                Assert.Equal(3, grid.RowDefinitions.Count);
                Assert.True(grid.RowDefinitions[0].Height.IsAuto);
                Assert.Equal(GridUnitType.Pixel, grid.RowDefinitions[1].Height.GridUnitType);
                Assert.Equal(8d, grid.RowDefinitions[1].Height.Value);
                Assert.Equal(GridUnitType.Star, grid.RowDefinitions[2].Height.GridUnitType);

                Assert.Equal(3, grid.ColumnDefinitions.Count);
                Assert.Equal(100d, grid.ColumnDefinitions[0].Width.Value);
                Assert.Equal(GridUnitType.Pixel, grid.ColumnDefinitions[1].Width.GridUnitType);
                Assert.Equal(12d, grid.ColumnDefinitions[1].Width.Value);
                Assert.Equal(GridUnitType.Star, grid.ColumnDefinitions[2].Width.GridUnitType);
            });
    }

    /// <summary>
    /// Ensures that logical child placement maps to physical WPF grid indices.
    /// </summary>
    [Fact]
    public void XGrid_ShouldMapLogicalChildPlacementWithSpacing()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border child = new();
                VIA.WPF.Controls.XGrid.SetRow(child, 1);
                VIA.WPF.Controls.XGrid.SetColumn(child, 1);
                VIA.WPF.Controls.XGrid.SetRowSpan(child, 2);
                VIA.WPF.Controls.XGrid.SetColumnSpan(child, 2);

                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*,Auto",
                    Columns = "Auto,*,Auto",
                    RowSpacing = 4d,
                    ColumnSpacing = 6d
                };

                grid.Children.Add(child);

                Assert.Equal(2, Grid.GetRow(child));
                Assert.Equal(2, Grid.GetColumn(child));
                Assert.Equal(3, Grid.GetRowSpan(child));
                Assert.Equal(3, Grid.GetColumnSpan(child));
            });
    }

    /// <summary>
    /// Ensures that standard WPF grid placement syntax is treated as logical XGrid placement.
    /// </summary>
    [Fact]
    public void XGrid_ShouldMapStandardGridPlacementWithSpacing()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border child = new();
                Grid.SetRow(child, 1);
                Grid.SetColumn(child, 1);
                Grid.SetRowSpan(child, 2);
                Grid.SetColumnSpan(child, 2);

                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*,Auto",
                    Columns = "Auto,*,Auto",
                    RowSpacing = 4d,
                    ColumnSpacing = 6d
                };

                grid.Children.Add(child);

                Assert.Equal(2, Grid.GetRow(child));
                Assert.Equal(2, Grid.GetColumn(child));
                Assert.Equal(3, Grid.GetRowSpan(child));
                Assert.Equal(3, Grid.GetColumnSpan(child));

                grid.ColumnSpacing = 10d;

                Assert.Equal(2, Grid.GetColumn(child));
                Assert.Equal(3, Grid.GetColumnSpan(child));
            });
    }

    /// <summary>
    /// Ensures that semantic areas are mapped to row, column and span values.
    /// </summary>
    [Fact]
    public void XGrid_ShouldMapNamedAreas()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border header = new();
                Border content = new();
                Border aside = new();
                VIA.WPF.Controls.XGrid.SetArea(header, "header");
                VIA.WPF.Controls.XGrid.SetArea(content, "content");
                VIA.WPF.Controls.XGrid.SetArea(aside, "aside");

                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*",
                    Columns = "*,220",
                    Areas = "header header; content aside"
                };

                grid.Children.Add(header);
                grid.Children.Add(content);
                grid.Children.Add(aside);

                Assert.Equal(0, Grid.GetRow(header));
                Assert.Equal(0, Grid.GetColumn(header));
                Assert.Equal(1, Grid.GetRowSpan(header));
                Assert.Equal(2, Grid.GetColumnSpan(header));

                Assert.Equal(1, Grid.GetRow(content));
                Assert.Equal(0, Grid.GetColumn(content));
                Assert.Equal(1, Grid.GetRow(aside));
                Assert.Equal(1, Grid.GetColumn(aside));
            });
    }

    /// <summary>
    /// Ensures that single-line area definitions are supported when row and column counts are known.
    /// </summary>
    [Fact]
    public void XGrid_ShouldMapFlatAreaDefinitionsWhenRowsAndColumnsAreKnown()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border target = new();
                VIA.WPF.Controls.XGrid.SetArea(target, "aside");

                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,*",
                    Columns = "*,Auto",
                    Areas = "header header content aside"
                };

                grid.Children.Add(target);

                Assert.Equal(1, Grid.GetRow(target));
                Assert.Equal(1, Grid.GetColumn(target));
            });
    }

    /// <summary>
    /// Ensures that empty area tokens are ignored while valid areas are still mapped.
    /// </summary>
    [Fact]
    public void XGrid_ShouldIgnoreEmptyAreaTokens()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border target = new();
                VIA.WPF.Controls.XGrid.SetArea(target, "right");

                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,Auto",
                    Columns = "Auto,Auto",
                    Areas = ". right; bottom _"
                };

                grid.Children.Add(target);

                Assert.Equal(0, Grid.GetRow(target));
                Assert.Equal(1, Grid.GetColumn(target));
            });
    }

    /// <summary>
    /// Ensures that invalid row or column definitions fail fast.
    /// </summary>
    [Fact]
    public void XGrid_ShouldRejectInvalidDefinitionTokens()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new();

                Assert.Throws<FormatException>(() => grid.Rows = "Smoke");
                Assert.Throws<FormatException>(() => grid.Columns = "Invalid*");
            });
    }

    /// <summary>
    /// Ensures that area definitions must form contiguous rectangles.
    /// </summary>
    [Fact]
    public void XGrid_ShouldRejectNonRectangularAreas()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XGrid grid = new()
                {
                    Rows = "Auto,Auto",
                    Columns = "Auto,Auto"
                };

                Assert.Throws<FormatException>(() => grid.Areas = "a b; b a");
            });
    }

    /// <summary>
    /// Ensures that attached property accessors validate null arguments.
    /// </summary>
    [Fact]
    public void XGrid_AttachedPropertyAccessors_ShouldRejectNullElements()
    {
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.SetRow(null!, 1));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.GetRow(null!));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.SetColumn(null!, 1));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.GetColumn(null!));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.SetRowSpan(null!, 1));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.GetRowSpan(null!));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.SetColumnSpan(null!, 1));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.GetColumnSpan(null!));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.SetArea(null!, "area"));
        Assert.Throws<ArgumentNullException>(() => VIA.WPF.Controls.XGrid.GetArea(null!));
    }
    #endregion
}
#endregion
