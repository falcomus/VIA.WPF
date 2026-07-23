// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckerBoard.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

#region ### Class XCheckerBoard ###
/// <summary>
/// Represents a content border that displays a configurable checkerboard background.
/// </summary>
public class XCheckerBoard : XBorder
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CheckerBoardBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckerBoardBrushProperty = DependencyProperty.Register(
        nameof(CheckerBoardBrush),
        typeof(Brush),
        typeof(XCheckerBoard),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnCheckerBoardBrushChanged));

    /// <summary>
    /// Identifies the <see cref="CheckerLightBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckerLightBrushProperty = DependencyProperty.Register(
        nameof(CheckerLightBrush),
        typeof(Brush),
        typeof(XCheckerBoard),
        new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender, OnGeneratedCheckerBoardBrushPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CheckerDarkBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckerDarkBrushProperty = DependencyProperty.Register(
        nameof(CheckerDarkBrush),
        typeof(Brush),
        typeof(XCheckerBoard),
        new FrameworkPropertyMetadata(XBrushFactory.CreateFrozenBrush(224, 224, 224), FrameworkPropertyMetadataOptions.AffectsRender, OnGeneratedCheckerBoardBrushPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CheckerSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckerSizeProperty = DependencyProperty.Register(
        nameof(CheckerSize),
        typeof(double),
        typeof(XCheckerBoard),
        new FrameworkPropertyMetadata(8d, FrameworkPropertyMetadataOptions.AffectsRender, OnGeneratedCheckerBoardBrushPropertyChanged, CoerceCheckerSize));

    private static readonly DependencyPropertyKey EffectiveCheckerBoardBrushPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(EffectiveCheckerBoardBrush),
        typeof(Brush),
        typeof(XCheckerBoard),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="EffectiveCheckerBoardBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EffectiveCheckerBoardBrushProperty = EffectiveCheckerBoardBrushPropertyKey.DependencyProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XCheckerBoard"/> class.
    /// </summary>
    public XCheckerBoard()
    {
        this.UpdateEffectiveCheckerBoardBrush();
    }

    /// <summary>
    /// Initializes static members of the <see cref="XCheckerBoard"/> class.
    /// </summary>
    static XCheckerBoard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XCheckerBoard),
            new FrameworkPropertyMetadata(typeof(XCheckerBoard)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a custom checkerboard brush. If this value is <see langword="null"/>, a brush is generated from the checker properties.
    /// </summary>
    public Brush? CheckerBoardBrush
    {
        get => (Brush?)this.GetValue(CheckerBoardBrushProperty);
        set => this.SetValue(CheckerBoardBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the light square brush used by the generated checkerboard brush.
    /// </summary>
    public Brush CheckerLightBrush
    {
        get => (Brush)this.GetValue(CheckerLightBrushProperty);
        set => this.SetValue(CheckerLightBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the dark square brush used by the generated checkerboard brush.
    /// </summary>
    public Brush CheckerDarkBrush
    {
        get => (Brush)this.GetValue(CheckerDarkBrushProperty);
        set => this.SetValue(CheckerDarkBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of a single checker square.
    /// </summary>
    public double CheckerSize
    {
        get => (double)this.GetValue(CheckerSizeProperty);
        set => this.SetValue(CheckerSizeProperty, value);
    }

    /// <summary>
    /// Gets the checkerboard brush currently used by the template.
    /// </summary>
    public Brush? EffectiveCheckerBoardBrush
    {
        get => (Brush?)this.GetValue(EffectiveCheckerBoardBrushProperty);
        private set => this.SetValue(EffectiveCheckerBoardBrushPropertyKey, value);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Updates the effective checkerboard brush.
    /// </summary>
    private void UpdateEffectiveCheckerBoardBrush()
    {
        this.EffectiveCheckerBoardBrush = this.CheckerBoardBrush ?? CreateCheckerBoardBrush(
            this.CheckerLightBrush,
            this.CheckerDarkBrush,
            this.CheckerSize);
    }

    /// <summary>
    /// Creates a tiled checkerboard brush.
    /// </summary>
    /// <param name="lightBrush">The light square brush.</param>
    /// <param name="darkBrush">The dark square brush.</param>
    /// <param name="checkerSize">The size of one checker square.</param>
    /// <returns>The generated checkerboard brush.</returns>
    private static Brush CreateCheckerBoardBrush(Brush lightBrush, Brush darkBrush, double checkerSize)
    {
        double size = Math.Max(1d, checkerSize);
        double tileSize = size * 2d;

        DrawingGroup drawingGroup = new();

        drawingGroup.Children.Add(new GeometryDrawing(
            lightBrush,
            null,
            new RectangleGeometry(new Rect(0d, 0d, tileSize, tileSize))));

        drawingGroup.Children.Add(new GeometryDrawing(
            darkBrush,
            null,
            new GeometryGroup
            {
                Children =
                {
                    new RectangleGeometry(new Rect(0d, 0d, size, size)),
                    new RectangleGeometry(new Rect(size, size, size, size))
                }
            }));

        DrawingBrush brush = new(drawingGroup)
        {
            TileMode = TileMode.Tile,
            Viewport = new Rect(0d, 0d, tileSize, tileSize),
            ViewportUnits = BrushMappingMode.Absolute,
            Viewbox = new Rect(0d, 0d, tileSize, tileSize),
            ViewboxUnits = BrushMappingMode.Absolute,
            Stretch = Stretch.None
        };

        return XBrushFactory.FreezeIfPossible(brush);
    }

    /// <summary>
    /// Handles changes to <see cref="CheckerBoardBrush"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnCheckerBoardBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XCheckerBoard checkerBoard)
        {
            checkerBoard.UpdateEffectiveCheckerBoardBrush();
        }
    }

    /// <summary>
    /// Handles changes to generated checkerboard brush properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnGeneratedCheckerBoardBrushPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XCheckerBoard checkerBoard)
        {
            checkerBoard.UpdateEffectiveCheckerBoardBrush();
        }
    }

    /// <summary>
    /// Coerces the checker size to a valid value.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="baseValue">The proposed value.</param>
    /// <returns>The coerced value.</returns>
    private static object CoerceCheckerSize(DependencyObject dependencyObject, object baseValue)
    {
        double value = (double)baseValue;

        return double.IsNaN(value) || value <= 0d
            ? 1d
            : value;
    }
    #endregion
}
#endregion