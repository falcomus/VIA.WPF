// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XUniformGrid.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XUniformGrid ###
/// <summary>
/// Represents a panel that arranges child elements in a grid with uniformly sized cells.
/// </summary>
public class XUniformGrid : Panel
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Rows"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
        nameof(Rows),
        typeof(int),
        typeof(XUniformGrid),
        new FrameworkPropertyMetadata(
            0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateNonNegativeInteger);

    /// <summary>
    /// Identifies the <see cref="Columns"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
        nameof(Columns),
        typeof(int),
        typeof(XUniformGrid),
        new FrameworkPropertyMetadata(
            0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateNonNegativeInteger);

    /// <summary>
    /// Identifies the <see cref="Spacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(XUniformGrid),
        new FrameworkPropertyMetadata(
            0d,
            OnSpacingChanged),
        ValidateSpacing);

    /// <summary>
    /// Identifies the <see cref="RowSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
        nameof(RowSpacing),
        typeof(double),
        typeof(XUniformGrid),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateSpacing);

    /// <summary>
    /// Identifies the <see cref="ColumnSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
        nameof(ColumnSpacing),
        typeof(double),
        typeof(XUniformGrid),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateSpacing);
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XUniformGrid"/> class.
    /// </summary>
    public XUniformGrid()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the number of rows.
    /// </summary>
    public int Rows
    {
        get => (int)this.GetValue(RowsProperty);
        set => this.SetValue(RowsProperty, value);
    }

    /// <summary>
    /// Gets or sets the number of columns.
    /// </summary>
    public int Columns
    {
        get => (int)this.GetValue(ColumnsProperty);
        set => this.SetValue(ColumnsProperty, value);
    }

    /// <summary>
    /// Gets or sets the uniform spacing applied to rows and columns.
    /// </summary>
    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between rows.
    /// </summary>
    public double RowSpacing
    {
        get => (double)this.GetValue(RowSpacingProperty);
        set => this.SetValue(RowSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between columns.
    /// </summary>
    public double ColumnSpacing
    {
        get => (double)this.GetValue(ColumnSpacingProperty);
        set => this.SetValue(ColumnSpacingProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        int visibleChildCount = this.GetVisibleChildCount();
        if (visibleChildCount == 0)
        {
            return new Size();
        }

        (int rows, int columns) = this.GetEffectiveDimensions(visibleChildCount);

        double rowSpacing = this.RowSpacing;
        double columnSpacing = this.ColumnSpacing;

        double availableWidth = availableSize.Width;
        double availableHeight = availableSize.Height;

        double totalHorizontalSpacing = Math.Max(0d, columns - 1) * columnSpacing;
        double totalVerticalSpacing = Math.Max(0d, rows - 1) * rowSpacing;

        double cellWidth = double.IsInfinity(availableWidth)
            ? double.PositiveInfinity
            : Math.Max(0d, (availableWidth - totalHorizontalSpacing) / columns);

        double cellHeight = double.IsInfinity(availableHeight)
            ? double.PositiveInfinity
            : Math.Max(0d, (availableHeight - totalVerticalSpacing) / rows);

        Size childConstraint = new(cellWidth, cellHeight);

        double maxChildWidth = 0d;
        double maxChildHeight = 0d;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            child.Measure(childConstraint);
            maxChildWidth = Math.Max(maxChildWidth, child.DesiredSize.Width);
            maxChildHeight = Math.Max(maxChildHeight, child.DesiredSize.Height);
        }

        double desiredWidth = double.IsInfinity(availableWidth)
            ? (columns * maxChildWidth) + totalHorizontalSpacing
            : availableWidth;

        double desiredHeight = double.IsInfinity(availableHeight)
            ? (rows * maxChildHeight) + totalVerticalSpacing
            : availableHeight;

        return new Size(desiredWidth, desiredHeight);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        int visibleChildCount = this.GetVisibleChildCount();
        if (visibleChildCount == 0)
        {
            return finalSize;
        }

        (int rows, int columns) = this.GetEffectiveDimensions(visibleChildCount);

        double rowSpacing = this.RowSpacing;
        double columnSpacing = this.ColumnSpacing;

        double totalHorizontalSpacing = Math.Max(0d, columns - 1) * columnSpacing;
        double totalVerticalSpacing = Math.Max(0d, rows - 1) * rowSpacing;

        double cellWidth = Math.Max(0d, (finalSize.Width - totalHorizontalSpacing) / columns);
        double cellHeight = Math.Max(0d, (finalSize.Height - totalVerticalSpacing) / rows);

        int visibleIndex = 0;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            int row = visibleIndex / columns;
            int column = visibleIndex % columns;

            double x = column * (cellWidth + columnSpacing);
            double y = row * (cellHeight + rowSpacing);

            child.Arrange(new Rect(x, y, cellWidth, cellHeight));
            visibleIndex++;
        }

        return finalSize;
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Gets the effective row and column count.
    /// </summary>
    /// <param name="visibleChildCount">The number of visible children.</param>
    /// <returns>The effective row and column count.</returns>
    private (int Rows, int Columns) GetEffectiveDimensions(int visibleChildCount)
    {
        int rows = this.Rows;
        int columns = this.Columns;

        if (rows > 0 && columns > 0)
        {
            return (rows, columns);
        }

        if (rows > 0)
        {
            columns = (int)Math.Ceiling((double)visibleChildCount / rows);
            return (rows, Math.Max(1, columns));
        }

        if (columns > 0)
        {
            rows = (int)Math.Ceiling((double)visibleChildCount / columns);
            return (Math.Max(1, rows), columns);
        }

        columns = (int)Math.Ceiling(Math.Sqrt(visibleChildCount));
        rows = (int)Math.Ceiling((double)visibleChildCount / columns);

        return (Math.Max(1, rows), Math.Max(1, columns));
    }

    /// <summary>
    /// Gets the number of visible child elements.
    /// </summary>
    /// <returns>The number of visible child elements.</returns>
    private int GetVisibleChildCount()
    {
        return this.InternalChildren.OfType<UIElement>().Count(static child => child.Visibility != Visibility.Collapsed);
    }

    /// <summary>
    /// Validates a non-negative integer value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateNonNegativeInteger(object value)
    {
        return value is int intValue && intValue >= 0;
    }

    /// <summary>
    /// Validates a spacing value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateSpacing(object value)
    {
        return value is double doubleValue
            && !double.IsNaN(doubleValue)
            && !double.IsInfinity(doubleValue)
            && doubleValue >= 0d;
    }

    /// <summary>
    /// Handles changes of the uniform spacing property.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XUniformGrid panel)
        {
            return;
        }

        double spacing = (double)e.NewValue;
        panel.SetCurrentValue(RowSpacingProperty, spacing);
        panel.SetCurrentValue(ColumnSpacingProperty, spacing);
    }
    #endregion
}
#endregion