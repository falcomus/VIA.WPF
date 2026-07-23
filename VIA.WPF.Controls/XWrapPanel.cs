// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWrapPanel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XWrapPanel ###
/// <summary>
/// Represents a wrap panel with built-in horizontal and vertical spacing support.
/// </summary>
public class XWrapPanel : Panel
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            Orientation.Horizontal,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    /// <summary>
    /// Identifies the <see cref="ItemWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
        nameof(ItemWidth),
        typeof(double),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            double.NaN,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateItemDimension);

    /// <summary>
    /// Identifies the <see cref="ItemHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
        nameof(ItemHeight),
        typeof(double),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            double.NaN,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateItemDimension);

    /// <summary>
    /// Identifies the <see cref="Spacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            0d,
            OnSpacingChanged),
        ValidateSpacing);

    /// <summary>
    /// Identifies the <see cref="HorizontalSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HorizontalSpacingProperty = DependencyProperty.Register(
        nameof(HorizontalSpacing),
        typeof(double),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateSpacing);

    /// <summary>
    /// Identifies the <see cref="VerticalSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VerticalSpacingProperty = DependencyProperty.Register(
        nameof(VerticalSpacing),
        typeof(double),
        typeof(XWrapPanel),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateSpacing);
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XWrapPanel"/> class.
    /// </summary>
    public XWrapPanel()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the orientation that controls the primary wrapping direction.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the uniform width of child elements.
    /// </summary>
    public double ItemWidth
    {
        get => (double)this.GetValue(ItemWidthProperty);
        set => this.SetValue(ItemWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the uniform height of child elements.
    /// </summary>
    public double ItemHeight
    {
        get => (double)this.GetValue(ItemHeightProperty);
        set => this.SetValue(ItemHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the uniform spacing that is applied to both horizontal and vertical spacing.
    /// </summary>
    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between items in the horizontal direction.
    /// </summary>
    public double HorizontalSpacing
    {
        get => (double)this.GetValue(HorizontalSpacingProperty);
        set => this.SetValue(HorizontalSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between items in the vertical direction.
    /// </summary>
    public double VerticalSpacing
    {
        get => (double)this.GetValue(VerticalSpacingProperty);
        set => this.SetValue(VerticalSpacingProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        return this.Orientation == Orientation.Horizontal
            ? this.MeasureHorizontal(availableSize)
            : this.MeasureVertical(availableSize);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        return this.Orientation == Orientation.Horizontal
            ? this.ArrangeHorizontal(finalSize)
            : this.ArrangeVertical(finalSize);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Measures the panel in horizontal wrapping mode.
    /// </summary>
    /// <param name="availableSize">The available size.</param>
    /// <returns>The desired size.</returns>
    private Size MeasureHorizontal(Size availableSize)
    {
        double availableWidth = double.IsInfinity(availableSize.Width) ? double.PositiveInfinity : availableSize.Width;
        double horizontalSpacing = this.HorizontalSpacing;
        double verticalSpacing = this.VerticalSpacing;

        Size childConstraint = new(
            double.IsNaN(this.ItemWidth) ? availableSize.Width : this.ItemWidth,
            double.IsNaN(this.ItemHeight) ? availableSize.Height : this.ItemHeight);

        double lineWidth = 0d;
        double lineHeight = 0d;
        double totalWidth = 0d;
        double totalHeight = 0d;
        bool hasLine = false;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            child.Measure(childConstraint);

            Size childSize = this.GetChildSize(child.DesiredSize);
            bool wrap = hasLine
                && !double.IsInfinity(availableWidth)
                && (lineWidth + horizontalSpacing + childSize.Width) > availableWidth;

            if (wrap)
            {
                totalWidth = Math.Max(totalWidth, lineWidth);
                totalHeight += lineHeight + verticalSpacing;
                lineWidth = 0d;
                lineHeight = 0d;
                hasLine = false;
            }

            if (hasLine)
            {
                lineWidth += horizontalSpacing;
            }

            lineWidth += childSize.Width;
            lineHeight = Math.Max(lineHeight, childSize.Height);
            hasLine = true;
        }

        if (hasLine)
        {
            totalWidth = Math.Max(totalWidth, lineWidth);
            totalHeight += lineHeight;
        }

        return new Size(totalWidth, totalHeight);
    }

    /// <summary>
    /// Measures the panel in vertical wrapping mode.
    /// </summary>
    /// <param name="availableSize">The available size.</param>
    /// <returns>The desired size.</returns>
    private Size MeasureVertical(Size availableSize)
    {
        double availableHeight = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height;
        double horizontalSpacing = this.HorizontalSpacing;
        double verticalSpacing = this.VerticalSpacing;

        Size childConstraint = new(
            double.IsNaN(this.ItemWidth) ? availableSize.Width : this.ItemWidth,
            double.IsNaN(this.ItemHeight) ? availableSize.Height : this.ItemHeight);

        double columnWidth = 0d;
        double columnHeight = 0d;
        double totalWidth = 0d;
        double totalHeight = 0d;
        bool hasColumn = false;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            child.Measure(childConstraint);

            Size childSize = this.GetChildSize(child.DesiredSize);
            bool wrap = hasColumn
                && !double.IsInfinity(availableHeight)
                && (columnHeight + verticalSpacing + childSize.Height) > availableHeight;

            if (wrap)
            {
                totalHeight = Math.Max(totalHeight, columnHeight);
                totalWidth += columnWidth + horizontalSpacing;
                columnWidth = 0d;
                columnHeight = 0d;
                hasColumn = false;
            }

            if (hasColumn)
            {
                columnHeight += verticalSpacing;
            }

            columnHeight += childSize.Height;
            columnWidth = Math.Max(columnWidth, childSize.Width);
            hasColumn = true;
        }

        if (hasColumn)
        {
            totalHeight = Math.Max(totalHeight, columnHeight);
            totalWidth += columnWidth;
        }

        return new Size(totalWidth, totalHeight);
    }

    /// <summary>
    /// Arranges the panel in horizontal wrapping mode.
    /// </summary>
    /// <param name="finalSize">The final size.</param>
    /// <returns>The arranged size.</returns>
    private Size ArrangeHorizontal(Size finalSize)
    {
        double horizontalSpacing = this.HorizontalSpacing;
        double verticalSpacing = this.VerticalSpacing;

        List<UIElement> lineChildren = [];
        double lineWidth = 0d;
        double lineHeight = 0d;
        double offsetY = 0d;
        double availableWidth = finalSize.Width;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            Size childSize = this.GetChildSize(child.DesiredSize);
            bool wrap = lineChildren.Count > 0
                && !double.IsInfinity(availableWidth)
                && (lineWidth + horizontalSpacing + childSize.Width) > availableWidth;

            if (wrap)
            {
                this.ArrangeHorizontalLine(lineChildren, offsetY, lineHeight, finalSize.Width, horizontalSpacing);
                offsetY += lineHeight + verticalSpacing;
                lineChildren.Clear();
                lineWidth = 0d;
                lineHeight = 0d;
            }

            if (lineChildren.Count > 0)
            {
                lineWidth += horizontalSpacing;
            }

            lineChildren.Add(child);
            lineWidth += childSize.Width;
            lineHeight = Math.Max(lineHeight, childSize.Height);
        }

        if (lineChildren.Count > 0)
        {
            this.ArrangeHorizontalLine(lineChildren, offsetY, lineHeight, finalSize.Width, horizontalSpacing);
        }

        return finalSize;
    }

    /// <summary>
    /// Arranges the panel in vertical wrapping mode.
    /// </summary>
    /// <param name="finalSize">The final size.</param>
    /// <returns>The arranged size.</returns>
    private Size ArrangeVertical(Size finalSize)
    {
        double horizontalSpacing = this.HorizontalSpacing;
        double verticalSpacing = this.VerticalSpacing;

        List<UIElement> columnChildren = [];
        double columnWidth = 0d;
        double columnHeight = 0d;
        double offsetX = 0d;
        double availableHeight = finalSize.Height;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            Size childSize = this.GetChildSize(child.DesiredSize);
            bool wrap = columnChildren.Count > 0
                && !double.IsInfinity(availableHeight)
                && (columnHeight + verticalSpacing + childSize.Height) > availableHeight;

            if (wrap)
            {
                this.ArrangeVerticalColumn(columnChildren, offsetX, columnWidth, finalSize.Height, verticalSpacing);
                offsetX += columnWidth + horizontalSpacing;
                columnChildren.Clear();
                columnWidth = 0d;
                columnHeight = 0d;
            }

            if (columnChildren.Count > 0)
            {
                columnHeight += verticalSpacing;
            }

            columnChildren.Add(child);
            columnHeight += childSize.Height;
            columnWidth = Math.Max(columnWidth, childSize.Width);
        }

        if (columnChildren.Count > 0)
        {
            this.ArrangeVerticalColumn(columnChildren, offsetX, columnWidth, finalSize.Height, verticalSpacing);
        }

        return finalSize;
    }

    /// <summary>
    /// Arranges one horizontal line.
    /// </summary>
    /// <param name="children">The children of the line.</param>
    /// <param name="offsetY">The vertical offset.</param>
    /// <param name="lineHeight">The line height.</param>
    /// <param name="finalWidth">The final width.</param>
    /// <param name="horizontalSpacing">The horizontal spacing.</param>
    private void ArrangeHorizontalLine(IReadOnlyList<UIElement> children, double offsetY, double lineHeight, double finalWidth, double horizontalSpacing)
    {
        double offsetX = 0d;

        foreach (UIElement child in children)
        {
            Size childSize = this.GetChildSize(child.DesiredSize);
            Rect rect = new(offsetX, offsetY, childSize.Width, lineHeight);
            child.Arrange(rect);
            offsetX += childSize.Width + horizontalSpacing;
        }
    }

    /// <summary>
    /// Arranges one vertical column.
    /// </summary>
    /// <param name="children">The children of the column.</param>
    /// <param name="offsetX">The horizontal offset.</param>
    /// <param name="columnWidth">The column width.</param>
    /// <param name="finalHeight">The final height.</param>
    /// <param name="verticalSpacing">The vertical spacing.</param>
    private void ArrangeVerticalColumn(IReadOnlyList<UIElement> children, double offsetX, double columnWidth, double finalHeight, double verticalSpacing)
    {
        double offsetY = 0d;

        foreach (UIElement child in children)
        {
            Size childSize = this.GetChildSize(child.DesiredSize);
            Rect rect = new(offsetX, offsetY, columnWidth, childSize.Height);
            child.Arrange(rect);
            offsetY += childSize.Height + verticalSpacing;
        }
    }

    /// <summary>
    /// Gets the effective child size based on item width and height settings.
    /// </summary>
    /// <param name="desiredSize">The desired child size.</param>
    /// <returns>The effective child size.</returns>
    private Size GetChildSize(Size desiredSize)
    {
        return new Size(
            double.IsNaN(this.ItemWidth) ? desiredSize.Width : this.ItemWidth,
            double.IsNaN(this.ItemHeight) ? desiredSize.Height : this.ItemHeight);
    }

    /// <summary>
    /// Validates the spacing value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateSpacing(object value)
    {
        return value is double spacing && !double.IsNaN(spacing) && !double.IsInfinity(spacing) && spacing >= 0d;
    }

    /// <summary>
    /// Validates the item width and height values.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateItemDimension(object value)
    {
        return value is double dimension
            && !double.IsInfinity(dimension)
            && (double.IsNaN(dimension) || dimension >= 0d);
    }

    /// <summary>
    /// Handles uniform spacing changes.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XWrapPanel panel)
        {
            return;
        }

        double spacing = (double)e.NewValue;
        panel.SetCurrentValue(HorizontalSpacingProperty, spacing);
        panel.SetCurrentValue(VerticalSpacingProperty, spacing);
    }
    #endregion
}
#endregion