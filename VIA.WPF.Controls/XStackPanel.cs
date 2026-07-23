// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XStackPanel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XStackPanel ###
/// <summary>
/// Represents a stack panel with built-in spacing support.
/// </summary>
public class XStackPanel : Panel
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XStackPanel),
        new FrameworkPropertyMetadata(
            Orientation.Vertical,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    /// <summary>
    /// Identifies the <see cref="Spacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(XStackPanel),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
        ValidateSpacing);
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XStackPanel"/> class.
    /// </summary>
    public XStackPanel()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the orientation in which child elements are stacked.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between child elements.
    /// </summary>
    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        bool isVertical = this.Orientation == Orientation.Vertical;
        double spacing = this.InternalSpacing;
        double totalWidth = 0d;
        double totalHeight = 0d;
        int visibleChildCount = 0;

        Size childAvailableSize = isVertical
            ? new Size(availableSize.Width, double.PositiveInfinity)
            : new Size(double.PositiveInfinity, availableSize.Height);

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            child.Measure(childAvailableSize);
            Size desiredSize = child.DesiredSize;

            if (isVertical)
            {
                totalWidth = Math.Max(totalWidth, desiredSize.Width);
                totalHeight += desiredSize.Height;
            }
            else
            {
                totalWidth += desiredSize.Width;
                totalHeight = Math.Max(totalHeight, desiredSize.Height);
            }

            visibleChildCount++;
        }

        if (visibleChildCount > 1)
        {
            double totalSpacing = spacing * (visibleChildCount - 1);

            if (isVertical)
            {
                totalHeight += totalSpacing;
            }
            else
            {
                totalWidth += totalSpacing;
            }
        }

        return new Size(totalWidth, totalHeight);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        bool isVertical = this.Orientation == Orientation.Vertical;
        double spacing = this.InternalSpacing;
        double offset = 0d;

        foreach (UIElement child in this.InternalChildren)
        {
            if (child is null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            Size desiredSize = child.DesiredSize;

            if (isVertical)
            {
                Rect arrangeRect = new(
                    0d,
                    offset,
                    Math.Max(0d, finalSize.Width),
                    desiredSize.Height);

                child.Arrange(arrangeRect);
                offset += desiredSize.Height + spacing;
            }
            else
            {
                Rect arrangeRect = new(
                    offset,
                    0d,
                    desiredSize.Width,
                    Math.Max(0d, finalSize.Height));

                child.Arrange(arrangeRect);
                offset += desiredSize.Width + spacing;
            }
        }

        return finalSize;
    }
    #endregion

    #region ### Private Properties ###
    /// <summary>
    /// Gets the validated non-negative spacing value.
    /// </summary>
    private double InternalSpacing => Math.Max(0d, this.Spacing);
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Validates the spacing value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if the spacing is valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateSpacing(object value)
    {
        return value is double spacing && !double.IsNaN(spacing) && !double.IsInfinity(spacing) && spacing >= 0d;
    }
    #endregion
}
#endregion