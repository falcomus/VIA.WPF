// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XAdaptiveGrid.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XAdaptiveGrid ###
/// <summary>
/// Arranges children in responsive columns based on a minimum item width.
/// </summary>
public class XAdaptiveGrid : Panel
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="MinItemWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinItemWidthProperty = DependencyProperty.Register(
        nameof(MinItemWidth),
        typeof(double),
        typeof(XAdaptiveGrid),
        new FrameworkPropertyMetadata(240d, FrameworkPropertyMetadataOptions.AffectsMeasure, null, CoerceNonNegativeDouble));

    /// <summary>
    /// Identifies the <see cref="MaxColumns"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register(
        nameof(MaxColumns),
        typeof(int),
        typeof(XAdaptiveGrid),
        new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure, null, CoercePositiveInteger));

    /// <summary>
    /// Identifies the <see cref="ColumnSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
        nameof(ColumnSpacing),
        typeof(double),
        typeof(XAdaptiveGrid),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure, null, CoerceNonNegativeDouble));

    /// <summary>
    /// Identifies the <see cref="RowSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
        nameof(RowSpacing),
        typeof(double),
        typeof(XAdaptiveGrid),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure, null, CoerceNonNegativeDouble));
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the minimum width of a child.
    /// </summary>
    public double MinItemWidth
    {
        get => (double)this.GetValue(MinItemWidthProperty);
        set => this.SetValue(MinItemWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum number of columns.
    /// </summary>
    public int MaxColumns
    {
        get => (int)this.GetValue(MaxColumnsProperty);
        set => this.SetValue(MaxColumnsProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal distance between children.
    /// </summary>
    public double ColumnSpacing
    {
        get => (double)this.GetValue(ColumnSpacingProperty);
        set => this.SetValue(ColumnSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical distance between rows.
    /// </summary>
    public double RowSpacing
    {
        get => (double)this.GetValue(RowSpacingProperty);
        set => this.SetValue(RowSpacingProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        int childCount = this.InternalChildren.Count;
        if (childCount == 0)
        {
            return default;
        }

        int columnCount = this.GetColumnCount(availableSize.Width, childCount);
        double itemWidth = this.GetItemWidth(availableSize.Width, columnCount);
        List<double> rowHeights = [];

        for (int index = 0; index < childCount; index++)
        {
            UIElement child = this.InternalChildren[index];
            child.Measure(new Size(itemWidth, double.PositiveInfinity));

            int row = index / columnCount;
            if (row == rowHeights.Count)
            {
                rowHeights.Add(child.DesiredSize.Height);
            }
            else
            {
                rowHeights[row] = Math.Max(rowHeights[row], child.DesiredSize.Height);
            }
        }

        double desiredWidth = double.IsInfinity(availableSize.Width)
            ? (columnCount * itemWidth) + ((columnCount - 1) * this.ColumnSpacing)
            : availableSize.Width;
        double desiredHeight = rowHeights.Sum() + (Math.Max(0, rowHeights.Count - 1) * this.RowSpacing);

        return new Size(desiredWidth, desiredHeight);
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        int childCount = this.InternalChildren.Count;
        if (childCount == 0)
        {
            return finalSize;
        }

        int columnCount = this.GetColumnCount(finalSize.Width, childCount);
        double itemWidth = this.GetItemWidth(finalSize.Width, columnCount);
        int rowCount = (int)Math.Ceiling(childCount / (double)columnCount);
        double[] rowHeights = new double[rowCount];

        for (int index = 0; index < childCount; index++)
        {
            int row = index / columnCount;
            rowHeights[row] = Math.Max(rowHeights[row], this.InternalChildren[index].DesiredSize.Height);
        }

        double y = 0d;
        for (int row = 0; row < rowCount; row++)
        {
            int rowStart = row * columnCount;
            int rowEnd = Math.Min(rowStart + columnCount, childCount);

            for (int index = rowStart; index < rowEnd; index++)
            {
                int column = index - rowStart;
                double x = column * (itemWidth + this.ColumnSpacing);
                this.InternalChildren[index].Arrange(new Rect(x, y, itemWidth, rowHeights[row]));
            }

            y += rowHeights[row] + this.RowSpacing;
        }

        return finalSize;
    }
    #endregion

    #region ### Private Methods ###
    private int GetColumnCount(double availableWidth, int childCount)
    {
        if (double.IsInfinity(availableWidth))
        {
            return Math.Min(childCount, this.MaxColumns);
        }

        int fittingColumns = (int)Math.Floor((availableWidth + this.ColumnSpacing) / (this.MinItemWidth + this.ColumnSpacing));
        return Math.Clamp(fittingColumns, 1, Math.Min(childCount, this.MaxColumns));
    }

    private double GetItemWidth(double availableWidth, int columnCount)
    {
        if (double.IsInfinity(availableWidth))
        {
            return this.MinItemWidth;
        }

        return Math.Max(0d, (availableWidth - ((columnCount - 1) * this.ColumnSpacing)) / columnCount);
    }

    private static object CoerceNonNegativeDouble(DependencyObject dependencyObject, object baseValue)
    {
        return Math.Max(0d, (double)baseValue);
    }

    private static object CoercePositiveInteger(DependencyObject dependencyObject, object baseValue)
    {
        return Math.Max(1, (int)baseValue);
    }
    #endregion
}
#endregion
