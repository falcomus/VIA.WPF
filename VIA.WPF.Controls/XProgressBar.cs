// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XProgressBar.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VIA.WPF.Controls;

#region ### Class XProgressBar ###
/// <summary>
/// Represents the standard progress bar control of VIA.WPF.
/// </summary>
public class XProgressBar : ProgressBar
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(XControlVariant.Primary));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ShowValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowValueProperty = DependencyProperty.Register(
        nameof(ShowValue),
        typeof(bool),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ValueFormatString"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueFormatStringProperty = DependencyProperty.Register(
        nameof(ValueFormatString),
        typeof(string),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata("F0", OnValueDisplayPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="TrackThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrackThicknessProperty = DependencyProperty.Register(
        nameof(TrackThickness),
        typeof(double),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(8d));

    /// <summary>
    /// Identifies the <see cref="ProgressBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ProgressBrushProperty = DependencyProperty.Register(
        nameof(ProgressBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrackBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrackBrushProperty = DependencyProperty.Register(
        nameof(TrackBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the read-only <see cref="FormattedValue"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey FormattedValuePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(FormattedValue),
        typeof(string),
        typeof(XProgressBar),
        new FrameworkPropertyMetadata("0"));

    /// <summary>
    /// Identifies the <see cref="FormattedValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FormattedValueProperty = FormattedValuePropertyKey.DependencyProperty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XProgressBar"/> class.
    /// </summary>
    static XProgressBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XProgressBar),
            new FrameworkPropertyMetadata(typeof(XProgressBar)));

        ValueProperty.OverrideMetadata(
            typeof(XProgressBar),
            new FrameworkPropertyMetadata(0d, OnValueDisplayPropertyChanged));

        MinimumProperty.OverrideMetadata(
            typeof(XProgressBar),
            new FrameworkPropertyMetadata(0d, OnValueDisplayPropertyChanged));

        MaximumProperty.OverrideMetadata(
            typeof(XProgressBar),
            new FrameworkPropertyMetadata(100d, OnValueDisplayPropertyChanged));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XProgressBar"/> class.
    /// </summary>
    public XProgressBar()
    {
        this.UpdateFormattedValue();
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
    /// Gets or sets the semantic color variant used by the progress indicator.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the header text displayed above the progress bar.
    /// </summary>
    public string Header
    {
        get => (string)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the description text displayed below the progress bar.
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
    /// Gets or sets the brush used for the progress indicator.
    /// </summary>
    public System.Windows.Media.Brush? ProgressBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(ProgressBrushProperty);
        set => this.SetValue(ProgressBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used for the track.
    /// </summary>
    public System.Windows.Media.Brush? TrackBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(TrackBrushProperty);
        set => this.SetValue(TrackBrushProperty, value);
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
    /// Handles changes to value display related properties.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnValueDisplayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XProgressBar progressBar)
        {
            progressBar.UpdateFormattedValue();
        }
    }
    #endregion
}
#endregion



#region ### Class XProgressBarWidthConverter ###
/// <summary>
/// Converts progress values to an indicator width.
/// </summary>
public sealed class XProgressBarWidthConverter : IMultiValueConverter
{
    #region ### Public Methods ###
    /// <inheritdoc />
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 4 ||
            values[0] is not double value ||
            values[1] is not double minimum ||
            values[2] is not double maximum ||
            values[3] is not double trackWidth ||
            trackWidth <= 0d ||
            maximum <= minimum)
        {
            return 0d;
        }

        double clampedValue = Math.Max(minimum, Math.Min(maximum, value));
        double ratio = (clampedValue - minimum) / (maximum - minimum);

        return trackWidth * ratio;
    }

    /// <inheritdoc />
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    #endregion
}
#endregion