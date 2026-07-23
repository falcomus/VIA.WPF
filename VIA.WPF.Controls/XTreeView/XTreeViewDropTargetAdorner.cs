// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewDropTargetAdorner.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

#region ### Class XTreeViewDropTargetAdorner ###
/// <summary>
/// Draws the native XTreeView drop target feedback.
/// </summary>
internal sealed class XTreeViewDropTargetAdorner : FrameworkElement
{
    #region ### Constants ###
    private const double MinimumInsertLineLength = 64d;
    private const double MarkerRadius = 3d;
    private const double HintHorizontalPadding = 4d;
    private const double HintVerticalPadding = 1d;
    private const double HintMargin = 8d;
    private const double HintCornerRadius = 2d;
    #endregion

    #region ### Private Fields ###
    private readonly XTreeView treeView;
    private XTreeViewNodeDropPosition position;
    private Rect targetBounds = Rect.Empty;
    private string? hintText;
    #endregion

    #region ### Constructors ###
    internal XTreeViewDropTargetAdorner(XTreeView treeView)
    {
        this.treeView = treeView;
        this.IsHitTestVisible = false;
        this.SnapsToDevicePixels = true;
    }
    #endregion

    #region ### Public Methods ###
    public void Update(XTreeViewNodeDropPosition dropPosition, Rect bounds, string? hint)
    {
        this.position = dropPosition;
        this.targetBounds = bounds;
        this.hintText = hint;
        this.Width = Math.Max(0d, this.treeView.ActualWidth);
        this.Height = Math.Max(0d, this.treeView.ActualHeight);
        this.InvalidateMeasure();
        this.InvalidateArrange();
        this.InvalidateVisual();
    }
    #endregion

    #region ### Protected Methods ###
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (this.targetBounds.IsEmpty)
        {
            return;
        }

        if (this.position == XTreeViewNodeDropPosition.Into)
        {
            this.DrawIntoTarget(drawingContext);
        }
        else
        {
            this.DrawInsertTarget(drawingContext);
        }

        this.DrawHint(drawingContext);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        return new Size(Math.Max(0d, this.treeView.ActualWidth), Math.Max(0d, this.treeView.ActualHeight));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        return new Size(Math.Max(0d, this.treeView.ActualWidth), Math.Max(0d, this.treeView.ActualHeight));
    }

    protected override Geometry? GetLayoutClip(Size layoutSlotSize)
    {
        return null;
    }
    #endregion

    #region ### Private Methods ###
    private void DrawIntoTarget(DrawingContext drawingContext)
    {
        Rect bounds = this.SnapRect(this.ClampToTreeView(this.targetBounds));
        if (bounds.IsEmpty)
        {
            return;
        }

        Brush fill = this.CreateHighlightBrush();
        Pen pen = this.CreatePen();

        double radius = Math.Max(0d, this.treeView.NodeCornerRadius.TopLeft);
        drawingContext.DrawRoundedRectangle(fill, pen, bounds, radius, radius);
    }

    private void DrawInsertTarget(DrawingContext drawingContext)
    {
        Pen pen = this.CreatePen();

        double y = this.position switch
        {
            XTreeViewNodeDropPosition.Before => this.targetBounds.Top,
            XTreeViewNodeDropPosition.After => this.targetBounds.Bottom,
            _ => this.targetBounds.Top
        };

        double halfThickness = pen.Thickness / 2d;
        y = this.ClampY(y, halfThickness);

        double left = this.ClampX(this.targetBounds.Left);
        double right = this.ClampX(Math.Max(left + MinimumInsertLineLength, this.treeView.ActualWidth - 2d));

        Point start = this.SnapPoint(new Point(left, y));
        Point end = this.SnapPoint(new Point(right, y));

        drawingContext.DrawLine(pen, start, end);
        drawingContext.DrawEllipse(pen.Brush, null, start, MarkerRadius + halfThickness, MarkerRadius + halfThickness);
    }

    private void DrawHint(DrawingContext drawingContext)
    {
        if (string.IsNullOrWhiteSpace(this.hintText))
        {
            return;
        }

        Color backgroundColor = Color.FromRgb(255, 244, 170);
        Brush background = new SolidColorBrush(backgroundColor);
        Brush borderBrush = this.GetThemeBrush(XBrushKeys.Primary, Brushes.LightGray);

        double luminance = ((0.2126d * backgroundColor.R) + (0.7152d * backgroundColor.G) + (0.0722d * backgroundColor.B)) / 255d;
        Brush textBrush = luminance > 0.60d
            ? new SolidColorBrush(Color.FromRgb(35, 35, 35))
            : Brushes.White;

        Brush backgroundClone = background.CloneCurrentValue();
        backgroundClone.Opacity = Math.Max(0.92d, backgroundClone.Opacity);

        Pen borderPen = new(borderBrush, 0.5d);

        double pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        FormattedText formattedText = new(
            this.hintText,
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            11.5d,
            textBrush,
            pixelsPerDip);

        double width = formattedText.Width + (HintHorizontalPadding * 2d);
        double height = formattedText.Height + (HintVerticalPadding * 2d);

        double left = Math.Max(8d, this.targetBounds.Left + 4d);
        double top = this.position switch
        {
            XTreeViewNodeDropPosition.After => this.targetBounds.Bottom + HintMargin,
            _ => Math.Max(4d, this.targetBounds.Top - height - HintMargin)
        };

        if (left + width > this.treeView.ActualWidth - 8d)
        {
            left = Math.Max(8d, this.treeView.ActualWidth - width - 8d);
        }

        if (top + height > this.treeView.ActualHeight - 4d)
        {
            top = Math.Max(4d, this.treeView.ActualHeight - height - 4d);
        }

        Rect hintRect = this.SnapRect(new Rect(left, top, width, height));

        drawingContext.DrawRoundedRectangle(backgroundClone, borderPen, hintRect, HintCornerRadius, HintCornerRadius);
        drawingContext.DrawText(formattedText, new Point(hintRect.Left + HintHorizontalPadding, hintRect.Top + HintVerticalPadding - 1));
    }

    private Pen CreatePen()
    {
        Pen? sourcePen = this.treeView.DropTargetAdornerPen;
        if (sourcePen is not null)
        {
            return sourcePen.CloneCurrentValue();
        }

        Brush brush = this.treeView.DropTargetAdornerBrush ?? this.GetThemeBrush(XBrushKeys.Primary, Brushes.IndianRed);
        return new Pen(brush, 0.75d)
        {
            DashStyle = new DashStyle(new double[] { 3d, 2d }, 0d)
        };
    }

    private Brush CreateHighlightBrush()
    {
        Brush brush = this.treeView.DropTargetHighlightBrush
            ?? this.treeView.DropTargetAdornerBrush
            ?? this.GetThemeBrush(XBrushKeys.PrimaryVeryLight, Brushes.LightSkyBlue);

        Brush clone = brush.CloneCurrentValue();
        clone.Opacity = Math.Min(clone.Opacity <= 0d ? 0.12d : clone.Opacity, 0.12d);
        return clone;
    }

    private Brush GetThemeBrush(object resourceKey, Brush fallback)
    {
        object? resource = this.treeView.TryFindResource(resourceKey) ?? Application.Current?.TryFindResource(resourceKey);
        return resource as Brush ?? fallback;
    }

    private Rect ClampToTreeView(Rect rect)
    {
        double ownerWidth = Math.Max(0d, this.treeView.ActualWidth);
        double ownerHeight = Math.Max(0d, this.treeView.ActualHeight);
        double left = Math.Min(Math.Max(0d, rect.Left), ownerWidth);
        double top = Math.Min(Math.Max(0d, rect.Top), ownerHeight);
        double right = Math.Min(Math.Max(left, rect.Right), ownerWidth);
        double bottom = Math.Min(Math.Max(top, rect.Bottom), ownerHeight);

        return right <= left || bottom <= top
            ? Rect.Empty
            : new Rect(new Point(left, top), new Point(right, bottom));
    }

    private double ClampX(double x)
    {
        double ownerWidth = Math.Max(0d, this.treeView.ActualWidth);
        return Math.Min(Math.Max(0d, x), ownerWidth);
    }

    private double ClampY(double y, double halfThickness)
    {
        double ownerHeight = Math.Max(0d, this.treeView.ActualHeight);
        return Math.Min(Math.Max(halfThickness, y), Math.Max(halfThickness, ownerHeight - halfThickness));
    }

    private Rect SnapRect(Rect rect)
    {
        Point topLeft = this.SnapPoint(rect.TopLeft);
        Point bottomRight = this.SnapPoint(rect.BottomRight);
        return new Rect(topLeft, bottomRight);
    }

    private Point SnapPoint(Point point)
    {
        double dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        return new Point(
            Math.Round(point.X * dpiScale) / dpiScale,
            Math.Round(point.Y * dpiScale) / dpiScale);
    }
    #endregion
}
#endregion
