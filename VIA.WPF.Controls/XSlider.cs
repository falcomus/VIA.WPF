// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSlider.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VIA.WPF.Controls;

#region ### Class XSlider ###
/// <summary>
/// Represents the standard slider control of VIA.WPF.
/// </summary>
public class XSlider : Slider
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XSlider),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XSlider),
        new FrameworkPropertyMetadata(XControlVariant.Primary));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XSlider),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XSlider),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ShowValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowValueProperty = DependencyProperty.Register(
        nameof(ShowValue),
        typeof(bool),
        typeof(XSlider),
        new FrameworkPropertyMetadata(false));


    /// <summary>
    /// Identifies the <see cref="ShowValueHint"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowValueHintProperty = DependencyProperty.Register(
        nameof(ShowValueHint),
        typeof(bool),
        typeof(XSlider),
        new FrameworkPropertyMetadata(false, OnShowValueHintChanged));

    /// <summary>
    /// Identifies the <see cref="ValueHintVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueHintVariantProperty = DependencyProperty.Register(
        nameof(ValueHintVariant),
        typeof(XControlVariant),
        typeof(XSlider),
        new FrameworkPropertyMetadata(XControlVariant.Info, OnValueHintVisualChanged));

    /// <summary>
    /// Identifies the <see cref="ValueHintAppearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueHintAppearanceProperty = DependencyProperty.Register(
        nameof(ValueHintAppearance),
        typeof(XControlAppearance),
        typeof(XSlider),
        new FrameworkPropertyMetadata(XControlAppearance.Solid, OnValueHintVisualChanged));

    /// <summary>
    /// Identifies the <see cref="ValueFormatString"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueFormatStringProperty = DependencyProperty.Register(
        nameof(ValueFormatString),
        typeof(string),
        typeof(XSlider),
        new FrameworkPropertyMetadata("F0", OnValueFormatStringChanged));

    /// <summary>
    /// Identifies the <see cref="TrackThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrackThicknessProperty = DependencyProperty.Register(
        nameof(TrackThickness),
        typeof(double),
        typeof(XSlider),
        new FrameworkPropertyMetadata(4d));

    /// <summary>
    /// Identifies the <see cref="ThumbSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ThumbSizeProperty = DependencyProperty.Register(
        nameof(ThumbSize),
        typeof(double),
        typeof(XSlider),
        new FrameworkPropertyMetadata(14d));

    /// <summary>
    /// Identifies the <see cref="ActiveTrackBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ActiveTrackBrushProperty = DependencyProperty.Register(
        nameof(ActiveTrackBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XSlider),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="InactiveTrackBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty InactiveTrackBrushProperty = DependencyProperty.Register(
        nameof(InactiveTrackBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XSlider),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ThumbBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ThumbBrushProperty = DependencyProperty.Register(
        nameof(ThumbBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XSlider),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FormattedValue"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey FormattedValuePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(FormattedValue),
        typeof(string),
        typeof(XSlider),
        new FrameworkPropertyMetadata("0"));

    /// <summary>
    /// Identifies the read-only <see cref="FormattedValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FormattedValueProperty = FormattedValuePropertyKey.DependencyProperty;
    #endregion

    #region ### Private Fields ###
    private Popup? valueHintPopup;
    private XBadge? valueHintBadge;
    private Thumb? valueHintThumb;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XSlider"/> class.
    /// </summary>
    static XSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XSlider),
            new FrameworkPropertyMetadata(typeof(XSlider)));

        ValueProperty.OverrideMetadata(
            typeof(XSlider),
            new FrameworkPropertyMetadata(0d, OnSliderValueChanged));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XSlider"/> class.
    /// </summary>
    public XSlider()
    {
        this.UpdateFormattedValue();
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        this.DetachValueHintThumb();

        base.OnApplyTemplate();

        if (this.GetTemplateChild("PART_Track") is Track track && track.Thumb is Thumb thumb)
        {
            this.valueHintThumb = thumb;
            this.valueHintThumb.DragStarted += this.OnValueHintThumbDragStarted;
            this.valueHintThumb.DragDelta += this.OnValueHintThumbDragDelta;
            this.valueHintThumb.DragCompleted += this.OnValueHintThumbDragCompleted;

            this.EnsureValueHintPopup(this.valueHintThumb);
            this.UpdateValueHintPopupContent();
        }
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the semantic size of the control.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used by the active track and thumb.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the header text displayed above the slider.
    /// </summary>
    public string Header
    {
        get => (string)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the description text displayed below the slider.
    /// </summary>
    public string Description
    {
        get => (string)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current value is shown.
    /// </summary>
    public bool ShowValue
    {
        get => (bool)this.GetValue(ShowValueProperty);
        set => this.SetValue(ShowValueProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a compact value hint is displayed at the thumb while dragging.
    /// </summary>
    public bool ShowValueHint
    {
        get => (bool)this.GetValue(ShowValueHintProperty);
        set => this.SetValue(ShowValueHintProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic color variant used by the drag value hint.
    /// </summary>
    public XControlVariant ValueHintVariant
    {
        get => (XControlVariant)this.GetValue(ValueHintVariantProperty);
        set => this.SetValue(ValueHintVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual appearance used by the drag value hint.
    /// </summary>
    public XControlAppearance ValueHintAppearance
    {
        get => (XControlAppearance)this.GetValue(ValueHintAppearanceProperty);
        set => this.SetValue(ValueHintAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string used for the displayed value.
    /// </summary>
    public string ValueFormatString
    {
        get => (string)this.GetValue(ValueFormatStringProperty);
        set => this.SetValue(ValueFormatStringProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual track thickness.
    /// </summary>
    public double TrackThickness
    {
        get => (double)this.GetValue(TrackThicknessProperty);
        set => this.SetValue(TrackThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the thumb size.
    /// </summary>
    public double ThumbSize
    {
        get => (double)this.GetValue(ThumbSizeProperty);
        set => this.SetValue(ThumbSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the active track.
    /// </summary>
    public System.Windows.Media.Brush? ActiveTrackBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(ActiveTrackBrushProperty);
        set => this.SetValue(ActiveTrackBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the inactive track.
    /// </summary>
    public System.Windows.Media.Brush? InactiveTrackBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(InactiveTrackBrushProperty);
        set => this.SetValue(InactiveTrackBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the thumb.
    /// </summary>
    public System.Windows.Media.Brush? ThumbBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(ThumbBrushProperty);
        set => this.SetValue(ThumbBrushProperty, value);
    }

    /// <summary>
    /// Gets the formatted value text.
    /// </summary>
    public string FormattedValue
    {
        get => (string)this.GetValue(FormattedValueProperty);
        private set => this.SetValue(FormattedValuePropertyKey, value);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Updates the formatted value text.
    /// </summary>
    private void UpdateFormattedValue()
    {
        string format = string.IsNullOrWhiteSpace(this.ValueFormatString)
            ? "F0"
            : this.ValueFormatString;

        this.FormattedValue = this.Value.ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Handles changes to the slider value.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSliderValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XSlider slider)
        {
            slider.UpdateFormattedValue();
            slider.UpdateValueHintPopupContent();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="ValueFormatString"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnValueFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XSlider slider)
        {
            slider.UpdateFormattedValue();
            slider.UpdateValueHintPopupContent();
        }
    }

    private void EnsureValueHintPopup(Thumb thumb)
    {
        this.valueHintBadge = new XBadge
        {
            Appearance = this.ValueHintAppearance,
            Elevation = XElevation.Low,
            Focusable = false,
            IsHitTestVisible = false,
            Padding = new Thickness(6d, 1d, 6d, 1d),
            Size = XControlSize.Small,
            Variant = this.ValueHintVariant
        };

        this.valueHintPopup = new Popup
        {
            AllowsTransparency = true,
            Child = this.valueHintBadge,
            CustomPopupPlacementCallback = PlaceValueHintPopup,
            Focusable = false,
            IsHitTestVisible = false,
            Placement = PlacementMode.Custom,
            PlacementTarget = thumb,
            PopupAnimation = PopupAnimation.Fade,
            StaysOpen = true
        };
    }

    private void DetachValueHintThumb()
    {
        if (this.valueHintThumb is not null)
        {
            this.valueHintThumb.DragStarted -= this.OnValueHintThumbDragStarted;
            this.valueHintThumb.DragDelta -= this.OnValueHintThumbDragDelta;
            this.valueHintThumb.DragCompleted -= this.OnValueHintThumbDragCompleted;
            this.valueHintThumb = null;
        }

        if (this.valueHintPopup is not null)
        {
            this.valueHintPopup.IsOpen = false;
            this.valueHintPopup.Child = null;
            this.valueHintPopup.CustomPopupPlacementCallback = null;
            this.valueHintPopup.PlacementTarget = null;
            this.valueHintPopup = null;
        }

        this.valueHintBadge = null;
    }

    private void ShowValueHintPopup()
    {
        if (!this.ShowValueHint || this.valueHintPopup is null)
        {
            return;
        }

        this.UpdateValueHintPopupContent();
        this.valueHintPopup.IsOpen = true;
    }

    private void HideValueHintPopup()
    {
        if (this.valueHintPopup is not null)
        {
            this.valueHintPopup.IsOpen = false;
        }
    }

    private void UpdateValueHintPopupContent()
    {
        if (this.valueHintBadge is null)
        {
            return;
        }

        this.valueHintBadge.Appearance = this.ValueHintAppearance;
        this.valueHintBadge.Content = this.FormattedValue;
        this.valueHintBadge.Variant = this.ValueHintVariant;
    }

    private void OnValueHintThumbDragStarted(object sender, DragStartedEventArgs e)
    {
        this.ShowValueHintPopup();
    }

    private void OnValueHintThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        this.UpdateValueHintPopupContent();
    }

    private void OnValueHintThumbDragCompleted(object sender, DragCompletedEventArgs e)
    {
        this.HideValueHintPopup();
    }

    private static CustomPopupPlacement[] PlaceValueHintPopup(Size popupSize, Size targetSize, Point offset)
    {
        double horizontalOffset = (targetSize.Width - popupSize.Width) / 2d;
        double verticalOffset = -popupSize.Height - 6d;

        return new[]
        {
            new CustomPopupPlacement(new Point(horizontalOffset, verticalOffset), PopupPrimaryAxis.Vertical)
        };
    }

    private static void OnShowValueHintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XSlider slider || e.NewValue is true)
        {
            return;
        }

        slider.HideValueHintPopup();
    }

    private static void OnValueHintVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XSlider slider)
        {
            slider.UpdateValueHintPopupContent();
        }
    }
    #endregion
}
#endregion
