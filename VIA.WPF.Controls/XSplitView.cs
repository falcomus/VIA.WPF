// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSplitView.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XSplitView ###
/// <summary>
/// Represents a two-pane split view with a draggable splitter.
/// </summary>
[TemplatePart(Name = PartHorizontalFirstColumnDefinition, Type = typeof(ColumnDefinition))]
[TemplatePart(Name = PartHorizontalSplitterColumnDefinition, Type = typeof(ColumnDefinition))]
[TemplatePart(Name = PartHorizontalSecondColumnDefinition, Type = typeof(ColumnDefinition))]
[TemplatePart(Name = PartVerticalFirstRowDefinition, Type = typeof(RowDefinition))]
[TemplatePart(Name = PartVerticalSplitterRowDefinition, Type = typeof(RowDefinition))]
[TemplatePart(Name = PartVerticalSecondRowDefinition, Type = typeof(RowDefinition))]
[TemplatePart(Name = PartSplitter, Type = typeof(GridSplitter))]
[TemplatePart(Name = PartFirstContentPresenter, Type = typeof(FrameworkElement))]
[TemplatePart(Name = PartSecondContentPresenter, Type = typeof(FrameworkElement))]
public class XSplitView : Control
{
    #region ### Constants ###
    private const string PartHorizontalFirstColumnDefinition = "PART_HorizontalFirstColumnDefinition";
    private const string PartHorizontalSplitterColumnDefinition = "PART_HorizontalSplitterColumnDefinition";
    private const string PartHorizontalSecondColumnDefinition = "PART_HorizontalSecondColumnDefinition";
    private const string PartVerticalFirstRowDefinition = "PART_VerticalFirstRowDefinition";
    private const string PartVerticalSplitterRowDefinition = "PART_VerticalSplitterRowDefinition";
    private const string PartVerticalSecondRowDefinition = "PART_VerticalSecondRowDefinition";
    private const string PartSplitter = "PART_Splitter";
    private const string PartFirstContentPresenter = "PART_FirstContentPresenter";
    private const string PartSecondContentPresenter = "PART_SecondContentPresenter";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Orientation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            Orientation.Horizontal,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="FirstContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FirstContentProperty = DependencyProperty.Register(
        nameof(FirstContent),
        typeof(object),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SecondContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SecondContentProperty = DependencyProperty.Register(
        nameof(SecondContent),
        typeof(object),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FirstContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FirstContentTemplateProperty = DependencyProperty.Register(
        nameof(FirstContentTemplate),
        typeof(DataTemplate),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SecondContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SecondContentTemplateProperty = DependencyProperty.Register(
        nameof(SecondContentTemplate),
        typeof(DataTemplate),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FirstLength"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FirstLengthProperty = DependencyProperty.Register(
        nameof(FirstLength),
        typeof(GridLength),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            new GridLength(320d),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="SecondLength"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SecondLengthProperty = DependencyProperty.Register(
        nameof(SecondLength),
        typeof(GridLength),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            new GridLength(1d, GridUnitType.Star),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="MinFirstLength"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinFirstLengthProperty = DependencyProperty.Register(
        nameof(MinFirstLength),
        typeof(double),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            120d,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged),
        ValidateNonNegativeDouble);

    /// <summary>
    /// Identifies the <see cref="MinSecondLength"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinSecondLengthProperty = DependencyProperty.Register(
        nameof(MinSecondLength),
        typeof(double),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            120d,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged),
        ValidateNonNegativeDouble);

    /// <summary>
    /// Identifies the <see cref="SplitterThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SplitterThicknessProperty = DependencyProperty.Register(
        nameof(SplitterThickness),
        typeof(double),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            12d,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged),
        ValidatePositiveDouble);

    /// <summary>
    /// Identifies the <see cref="SplitterSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SplitterSpacingProperty = DependencyProperty.Register(
        nameof(SplitterSpacing),
        typeof(double),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            2d,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged),
        ValidateNonNegativeDouble);

    /// <summary>
    /// Identifies the <see cref="ShowsPreview"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowsPreviewProperty = DependencyProperty.Register(
        nameof(ShowsPreview),
        typeof(bool),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            true,
            OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsSecondPaneVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSecondPaneVisibleProperty = DependencyProperty.Register(
        nameof(IsSecondPaneVisible),
        typeof(bool),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            true,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsSplitterVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSplitterVisibleProperty = DependencyProperty.Register(
        nameof(IsSplitterVisible),
        typeof(bool),
        typeof(XSplitView),
        new FrameworkPropertyMetadata(
            true,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnLayoutPropertyChanged));
    #endregion

    #region ### Private Fields ###
    private ColumnDefinition? _horizontalFirstColumnDefinition;
    private ColumnDefinition? _horizontalSplitterColumnDefinition;
    private ColumnDefinition? _horizontalSecondColumnDefinition;
    private RowDefinition? _verticalFirstRowDefinition;
    private RowDefinition? _verticalSplitterRowDefinition;
    private RowDefinition? _verticalSecondRowDefinition;
    private GridSplitter? _splitter;
    private FrameworkElement? _firstContentPresenter;
    private FrameworkElement? _secondContentPresenter;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XSplitView"/> class.
    /// </summary>
    static XSplitView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XSplitView),
            new FrameworkPropertyMetadata(typeof(XSplitView)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XSplitView"/> class.
    /// </summary>
    public XSplitView()
    {
        this.SizeChanged += this.OnSizeChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the split orientation.
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the first pane.
    /// </summary>
    public object? FirstContent
    {
        get => this.GetValue(FirstContentProperty);
        set => this.SetValue(FirstContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the second pane.
    /// </summary>
    public object? SecondContent
    {
        get => this.GetValue(SecondContentProperty);
        set => this.SetValue(SecondContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template for the first pane content.
    /// </summary>
    public DataTemplate? FirstContentTemplate
    {
        get => (DataTemplate?)this.GetValue(FirstContentTemplateProperty);
        set => this.SetValue(FirstContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the data template for the second pane content.
    /// </summary>
    public DataTemplate? SecondContentTemplate
    {
        get => (DataTemplate?)this.GetValue(SecondContentTemplateProperty);
        set => this.SetValue(SecondContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the length of the first pane.
    /// </summary>
    public GridLength FirstLength
    {
        get => (GridLength)this.GetValue(FirstLengthProperty);
        set => this.SetValue(FirstLengthProperty, value);
    }

    /// <summary>
    /// Gets or sets the length of the second pane.
    /// </summary>
    public GridLength SecondLength
    {
        get => (GridLength)this.GetValue(SecondLengthProperty);
        set => this.SetValue(SecondLengthProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum size of the first pane.
    /// </summary>
    public double MinFirstLength
    {
        get => (double)this.GetValue(MinFirstLengthProperty);
        set => this.SetValue(MinFirstLengthProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum size of the second pane.
    /// </summary>
    public double MinSecondLength
    {
        get => (double)this.GetValue(MinSecondLengthProperty);
        set => this.SetValue(MinSecondLengthProperty, value);
    }

    /// <summary>
    /// Gets or sets the splitter thickness.
    /// </summary>
    public double SplitterThickness
    {
        get => (double)this.GetValue(SplitterThicknessProperty);
        set => this.SetValue(SplitterThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between the splitter and the pane content.
    /// </summary>
    public double SplitterSpacing
    {
        get => (double)this.GetValue(SplitterSpacingProperty);
        set => this.SetValue(SplitterSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether preview resizing is shown.
    /// </summary>
    public bool ShowsPreview
    {
        get => (bool)this.GetValue(ShowsPreviewProperty);
        set => this.SetValue(ShowsPreviewProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the second pane is visible.
    /// </summary>
    public bool IsSecondPaneVisible
    {
        get => (bool)this.GetValue(IsSecondPaneVisibleProperty);
        set => this.SetValue(IsSecondPaneVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the splitter is visible while the second pane is visible.
    /// </summary>
    public bool IsSplitterVisible
    {
        get => (bool)this.GetValue(IsSplitterVisibleProperty);
        set => this.SetValue(IsSplitterVisibleProperty, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this._horizontalFirstColumnDefinition = this.GetTemplateChild(PartHorizontalFirstColumnDefinition) as ColumnDefinition;
        this._horizontalSplitterColumnDefinition = this.GetTemplateChild(PartHorizontalSplitterColumnDefinition) as ColumnDefinition;
        this._horizontalSecondColumnDefinition = this.GetTemplateChild(PartHorizontalSecondColumnDefinition) as ColumnDefinition;
        this._verticalFirstRowDefinition = this.GetTemplateChild(PartVerticalFirstRowDefinition) as RowDefinition;
        this._verticalSplitterRowDefinition = this.GetTemplateChild(PartVerticalSplitterRowDefinition) as RowDefinition;
        this._verticalSecondRowDefinition = this.GetTemplateChild(PartVerticalSecondRowDefinition) as RowDefinition;
        this._splitter = this.GetTemplateChild(PartSplitter) as GridSplitter;
        this._firstContentPresenter = this.GetTemplateChild(PartFirstContentPresenter) as FrameworkElement;
        this._secondContentPresenter = this.GetTemplateChild(PartSecondContentPresenter) as FrameworkElement;

        this.UpdateLayoutParts();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Updates the template parts according to the current properties.
    /// </summary>
    private void UpdateLayoutParts()
    {
        bool isHorizontal = this.Orientation == Orientation.Horizontal;
        bool isSecondPaneVisible = this.IsSecondPaneVisible;
        bool isSplitterVisible = isSecondPaneVisible && this.IsSplitterVisible;

        if (this._horizontalFirstColumnDefinition is not null)
        {
            this._horizontalFirstColumnDefinition.Width = isHorizontal && isSecondPaneVisible
                ? this.FirstLength
                : new GridLength(1d, GridUnitType.Star);

            this._horizontalFirstColumnDefinition.MinWidth = isHorizontal ? this.MinFirstLength : 0d;
        }

        if (this._horizontalSplitterColumnDefinition is not null)
        {
            this._horizontalSplitterColumnDefinition.Width = isHorizontal && isSplitterVisible
                ? new GridLength(this.SplitterThickness)
                : new GridLength(0d);
        }

        if (this._horizontalSecondColumnDefinition is not null)
        {
            this._horizontalSecondColumnDefinition.Width = isHorizontal && isSecondPaneVisible
                ? this.SecondLength
                : isHorizontal ? new GridLength(0d) : new GridLength(1d, GridUnitType.Star);

            this._horizontalSecondColumnDefinition.MinWidth = isHorizontal && isSecondPaneVisible
                ? this.MinSecondLength
                : 0d;
        }

        if (this._verticalFirstRowDefinition is not null)
        {
            this._verticalFirstRowDefinition.Height = !isHorizontal && isSecondPaneVisible
                ? this.FirstLength
                : new GridLength(1d, GridUnitType.Star);

            this._verticalFirstRowDefinition.MinHeight = isHorizontal ? 0d : this.MinFirstLength;
        }

        if (this._verticalSplitterRowDefinition is not null)
        {
            this._verticalSplitterRowDefinition.Height = !isHorizontal && isSplitterVisible
                ? new GridLength(this.SplitterThickness)
                : new GridLength(0d);
        }

        if (this._verticalSecondRowDefinition is not null)
        {
            this._verticalSecondRowDefinition.Height = !isHorizontal && isSecondPaneVisible
                ? this.SecondLength
                : new GridLength(0d);

            this._verticalSecondRowDefinition.MinHeight = !isHorizontal && isSecondPaneVisible
                ? this.MinSecondLength
                : 0d;
        }

        if (this._splitter is not null)
        {
            this._splitter.ShowsPreview = this.ShowsPreview;
            this._splitter.ResizeDirection = isHorizontal
                ? GridResizeDirection.Columns
                : GridResizeDirection.Rows;
            this._splitter.Visibility = isSplitterVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        if (this._secondContentPresenter is not null)
        {
            this._secondContentPresenter.Visibility = isSecondPaneVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        this.UpdateResizeConstraints(isHorizontal, isSecondPaneVisible, isSplitterVisible);
        this.UpdateContentMargins(isSplitterVisible);
    }

    /// <summary>
    /// Updates the splitter resize constraints so that neither pane can be resized below its configured minimum length.
    /// </summary>
    /// <param name="isHorizontal">A value indicating whether the split is horizontal.</param>
    /// <param name="isSecondPaneVisible">A value indicating whether the second pane is visible.</param>
    /// <param name="isSplitterVisible">A value indicating whether the splitter is currently visible.</param>
    private void UpdateResizeConstraints(bool isHorizontal, bool isSecondPaneVisible, bool isSplitterVisible)
    {
        double splitterLength = isSecondPaneVisible && isSplitterVisible
            ? this.SplitterThickness
            : 0d;

        this.UpdateHorizontalResizeConstraints(isHorizontal, isSecondPaneVisible, splitterLength);
        this.UpdateVerticalResizeConstraints(isHorizontal, isSecondPaneVisible, splitterLength);
    }

    /// <summary>
    /// Updates the horizontal column resize constraints.
    /// </summary>
    /// <param name="isHorizontal">A value indicating whether the split is horizontal.</param>
    /// <param name="isSecondPaneVisible">A value indicating whether the second pane is visible.</param>
    /// <param name="splitterLength">The effective splitter length.</param>
    private void UpdateHorizontalResizeConstraints(bool isHorizontal, bool isSecondPaneVisible, double splitterLength)
    {
        if (this._horizontalFirstColumnDefinition is not null)
        {
            this._horizontalFirstColumnDefinition.MaxWidth = isHorizontal && isSecondPaneVisible
                ? CreatePaneMaximum(this.ActualWidth, splitterLength, this.MinSecondLength, this.MinFirstLength)
                : double.PositiveInfinity;
        }

        if (this._horizontalSecondColumnDefinition is not null)
        {
            this._horizontalSecondColumnDefinition.MaxWidth = isHorizontal && isSecondPaneVisible
                ? CreatePaneMaximum(this.ActualWidth, splitterLength, this.MinFirstLength, this.MinSecondLength)
                : double.PositiveInfinity;
        }
    }

    /// <summary>
    /// Updates the vertical row resize constraints.
    /// </summary>
    /// <param name="isHorizontal">A value indicating whether the split is horizontal.</param>
    /// <param name="isSecondPaneVisible">A value indicating whether the second pane is visible.</param>
    /// <param name="splitterLength">The effective splitter length.</param>
    private void UpdateVerticalResizeConstraints(bool isHorizontal, bool isSecondPaneVisible, double splitterLength)
    {
        if (this._verticalFirstRowDefinition is not null)
        {
            this._verticalFirstRowDefinition.MaxHeight = !isHorizontal && isSecondPaneVisible
                ? CreatePaneMaximum(this.ActualHeight, splitterLength, this.MinSecondLength, this.MinFirstLength)
                : double.PositiveInfinity;
        }

        if (this._verticalSecondRowDefinition is not null)
        {
            this._verticalSecondRowDefinition.MaxHeight = !isHorizontal && isSecondPaneVisible
                ? CreatePaneMaximum(this.ActualHeight, splitterLength, this.MinFirstLength, this.MinSecondLength)
                : double.PositiveInfinity;
        }
    }

    /// <summary>
    /// Creates a maximum pane length that keeps the opposite pane above its configured minimum length.
    /// </summary>
    /// <param name="totalLength">The available total length.</param>
    /// <param name="splitterLength">The effective splitter length.</param>
    /// <param name="oppositeMinimumLength">The opposite pane minimum length.</param>
    /// <param name="ownMinimumLength">The own pane minimum length.</param>
    /// <returns>The maximum pane length.</returns>
    private static double CreatePaneMaximum(double totalLength, double splitterLength, double oppositeMinimumLength, double ownMinimumLength)
    {
        if (totalLength <= 0d || double.IsNaN(totalLength) || double.IsInfinity(totalLength))
        {
            return double.PositiveInfinity;
        }

        double maximumLength = totalLength - splitterLength - oppositeMinimumLength;
        return Math.Max(ownMinimumLength, maximumLength);
    }

    /// <summary>
    /// Updates the content margins according to the current splitter orientation and spacing.
    /// </summary>
    /// <param name="isSplitterVisible">A value indicating whether the splitter is currently visible.</param>
    private void UpdateContentMargins(bool isSplitterVisible)
    {
        if (this._firstContentPresenter is null || this._secondContentPresenter is null)
        {
            return;
        }

        if (!this.IsSecondPaneVisible || !isSplitterVisible)
        {
            this._firstContentPresenter.Margin = new Thickness(0d);
            this._secondContentPresenter.Margin = new Thickness(0d);
            return;
        }

        if (this.Orientation == Orientation.Horizontal)
        {
            this._firstContentPresenter.Margin = new Thickness(0d, 0d, this.SplitterSpacing, 0d);
            this._secondContentPresenter.Margin = new Thickness(this.SplitterSpacing, 0d, 0d, 0d);
        }
        else
        {
            this._firstContentPresenter.Margin = new Thickness(0d, 0d, 0d, this.SplitterSpacing);
            this._secondContentPresenter.Margin = new Thickness(0d, this.SplitterSpacing, 0d, 0d);
        }
    }

    /// <summary>
    /// Handles size changes of the split view.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event arguments.</param>
    private void OnSizeChanged(object sender, SizeChangedEventArgs eventArgs)
    {
        bool isHorizontal = this.Orientation == Orientation.Horizontal;
        bool isSecondPaneVisible = this.IsSecondPaneVisible;
        bool isSplitterVisible = isSecondPaneVisible && this.IsSplitterVisible;

        this.UpdateResizeConstraints(isHorizontal, isSecondPaneVisible, isSplitterVisible);
    }

    /// <summary>
    /// Handles layout-related property changes.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XSplitView splitView)
        {
            splitView.UpdateLayoutParts();
        }
    }

    /// <summary>
    /// Validates a non-negative double value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidateNonNegativeDouble(object value)
    {
        return value is double doubleValue
               && !double.IsNaN(doubleValue)
               && !double.IsInfinity(doubleValue)
               && doubleValue >= 0d;
    }

    /// <summary>
    /// Validates a positive double value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise <see langword="false"/>.</returns>
    private static bool ValidatePositiveDouble(object value)
    {
        return value is double doubleValue
               && !double.IsNaN(doubleValue)
               && !double.IsInfinity(doubleValue)
               && doubleValue > 0d;
    }
    #endregion
}
#endregion
