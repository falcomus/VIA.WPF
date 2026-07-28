// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTimePicker.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

#region ### Class XTimePicker ###
/// <summary>
/// Represents the standard time picker control of VIA.WPF.
/// </summary>
[TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
[TemplatePart(Name = ClearButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = DropDownButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
[TemplatePart(Name = HourListBoxPartName, Type = typeof(ListBox))]
[TemplatePart(Name = MinuteListBoxPartName, Type = typeof(ListBox))]
public class XTimePicker : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the inner text box template part.
    /// </summary>
    private const string TextBoxPartName = "PART_TextBox";

    /// <summary>
    /// The name of the clear button template part.
    /// </summary>
    private const string ClearButtonPartName = "PART_ClearButton";

    /// <summary>
    /// The name of the drop-down button template part.
    /// </summary>
    private const string DropDownButtonPartName = "PART_DropDownButton";

    /// <summary>
    /// The name of the popup template part.
    /// </summary>
    private const string PopupPartName = "PART_Popup";

    /// <summary>
    /// The name of the hour list box template part.
    /// </summary>
    private const string HourListBoxPartName = "PART_HourListBox";

    /// <summary>
    /// The name of the minute list box template part.
    /// </summary>
    private const string MinuteListBoxPartName = "PART_MinuteListBox";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="SelectedTime"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
        nameof(SelectedTime),
        typeof(TimeSpan?),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

    /// <summary>
    /// Identifies the <see cref="Text"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

    /// <summary>
    /// Identifies the <see cref="MinimumTime"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumTimeProperty = DependencyProperty.Register(
        nameof(MinimumTime),
        typeof(TimeSpan?),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null, OnTimeRangeChanged));

    /// <summary>
    /// Identifies the <see cref="MaximumTime"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaximumTimeProperty = DependencyProperty.Register(
        nameof(MaximumTime),
        typeof(TimeSpan?),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null, OnTimeRangeChanged));

    /// <summary>
    /// Identifies the <see cref="StepMinutes"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StepMinutesProperty = DependencyProperty.Register(
        nameof(StepMinutes),
        typeof(int),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(15, OnStepMinutesChanged, CoerceStepMinutes));

    /// <summary>
    /// Identifies the <see cref="TimeFormatString"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TimeFormatStringProperty = DependencyProperty.Register(
        nameof(TimeFormatString),
        typeof(string),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(@"hh\:mm", OnTimeFormatStringChanged));

    /// <summary>
    /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(true, OnShowResetButtonChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null, OnLeadingIconChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null, OnTrailingIconChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsReadOnly"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

    /// <summary>
    /// Identifies the <see cref="CaretBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
        nameof(CaretBrush),
        typeof(Brush),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XTimePicker),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The current inner text box.
    /// </summary>
    private TextBox? textBox;

    /// <summary>
    /// The current clear button.
    /// </summary>
    private Button? clearButton;

    /// <summary>
    /// The current drop-down button.
    /// </summary>
    private Button? dropDownButton;

    /// <summary>
    /// The current popup.
    /// </summary>
    private Popup? popup;

    /// <summary>
    /// The current hour list box.
    /// </summary>
    private ListBox? hourListBox;

    /// <summary>
    /// The current minute list box.
    /// </summary>
    private ListBox? minuteListBox;

    /// <summary>
    /// Prevents recursive synchronization between text and time.
    /// </summary>
    private bool isSynchronizing;

    /// <summary>
    /// Prevents recursive synchronization between popup selections and selected time.
    /// </summary>
    private bool isSynchronizingSelection;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTimePicker"/> class.
    /// </summary>
    public XTimePicker()
    {
        this.IsEnabledChanged += this.OnIsEnabledChanged;
        this.HourItems = [];
        this.MinuteItems = [];
        this.RebuildHourItems();
        this.RebuildMinuteItems();
    }

    /// <summary>
    /// Initializes static members of the <see cref="XTimePicker"/> class.
    /// </summary>
    static XTimePicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTimePicker),
            new FrameworkPropertyMetadata(typeof(XTimePicker)));

        FocusableProperty.OverrideMetadata(
            typeof(XTimePicker),
            new FrameworkPropertyMetadata(true));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the available hour items shown by the popup.
    /// </summary>
    public ObservableCollection<XTimePickerValueItem> HourItems { get; }

    /// <summary>
    /// Gets the available minute items shown by the popup.
    /// </summary>
    public ObservableCollection<XTimePickerValueItem> MinuteItems { get; }

    /// <summary>
    /// Gets or sets the selected time.
    /// </summary>
    public TimeSpan? SelectedTime
    {
        get => (TimeSpan?)this.GetValue(SelectedTimeProperty);
        set => this.SetValue(SelectedTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the current text representation.
    /// </summary>
    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum selectable time.
    /// </summary>
    public TimeSpan? MinimumTime
    {
        get => (TimeSpan?)this.GetValue(MinimumTimeProperty);
        set => this.SetValue(MinimumTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum selectable time.
    /// </summary>
    public TimeSpan? MaximumTime
    {
        get => (TimeSpan?)this.GetValue(MaximumTimeProperty);
        set => this.SetValue(MaximumTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the number of minutes between minute entries.
    /// Supported values are 5, 10, 15, 30 and 60.
    /// </summary>
    public int StepMinutes
    {
        get => (int)this.GetValue(StepMinutesProperty);
        set => this.SetValue(StepMinutesProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string used for displayed times.
    /// </summary>
    public string TimeFormatString
    {
        get => (string)this.GetValue(TimeFormatStringProperty);
        set => this.SetValue(TimeFormatStringProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the time popup is open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => (bool)this.GetValue(IsDropDownOpenProperty);
        set => this.SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a clear button is shown.
    /// </summary>
    public bool ShowResetButton
    {
        get => (bool)this.GetValue(ShowResetButtonProperty);
        set => this.SetValue(ShowResetButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the control.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size of the control.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the placeholder text.
    /// </summary>
    public string Placeholder
    {
        get => (string)this.GetValue(PlaceholderProperty);
        set => this.SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets the header text displayed above the input area.
    /// </summary>
    public string Header
    {
        get => (string)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the description text displayed below the input area.
    /// </summary>
    public string Description
    {
        get => (string)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the leading icon content.
    /// </summary>
    public object? LeadingIcon
    {
        get => this.GetValue(LeadingIconProperty);
        set => this.SetValue(LeadingIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the leading icon.
    /// </summary>
    public double LeadingIconSize
    {
        get => (double)this.GetValue(LeadingIconSizeProperty);
        set => this.SetValue(LeadingIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the leading icon.
    /// </summary>
    public DataTemplate? LeadingIconTemplate
    {
        get => (DataTemplate?)this.GetValue(LeadingIconTemplateProperty);
        set => this.SetValue(LeadingIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the trailing icon content.
    /// </summary>
    public object? TrailingIcon
    {
        get => this.GetValue(TrailingIconProperty);
        set => this.SetValue(TrailingIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the trailing icon.
    /// </summary>
    public double TrailingIconSize
    {
        get => (double)this.GetValue(TrailingIconSizeProperty);
        set => this.SetValue(TrailingIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the trailing icon.
    /// </summary>
    public DataTemplate? TrailingIconTemplate
    {
        get => (DataTemplate?)this.GetValue(TrailingIconTemplateProperty);
        set => this.SetValue(TrailingIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the text editing surface is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)this.GetValue(IsReadOnlyProperty);
        set => this.SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// Gets or sets the caret brush used by the inner text box.
    /// </summary>
    public Brush? CaretBrush
    {
        get => (Brush?)this.GetValue(CaretBrushProperty);
        set => this.SetValue(CaretBrushProperty, value);
    }
    /// <summary>
    /// Gets or sets the font size used by the header text.
    /// </summary>
    public double HeaderFontSize
    {
        get => (double)this.GetValue(HeaderFontSizeProperty);
        set => this.SetValue(HeaderFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font weight used by the header text.
    /// </summary>
    public FontWeight HeaderFontWeight
    {
        get => (FontWeight)this.GetValue(HeaderFontWeightProperty);
        set => this.SetValue(HeaderFontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether validation hints can use multiple lines.
    /// </summary>
    public bool MultiLineValidationHints
    {
        get => (bool)this.GetValue(MultiLineValidationHintsProperty);
        set => this.SetValue(MultiLineValidationHintsProperty, value);
    }

    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.DetachTemplateParts();

        this.textBox = this.GetTemplateChild(TextBoxPartName) as TextBox;
        this.clearButton = this.GetTemplateChild(ClearButtonPartName) as Button;
        this.dropDownButton = this.GetTemplateChild(DropDownButtonPartName) as Button;
        this.popup = this.GetTemplateChild(PopupPartName) as Popup;
        this.hourListBox = this.GetTemplateChild(HourListBoxPartName) as ListBox;
        this.minuteListBox = this.GetTemplateChild(MinuteListBoxPartName) as ListBox;

        this.AttachTemplateParts();
        this.SynchronizeTextFromSelectedTime();
        this.SynchronizeListSelectionsFromSelectedTime();
        this.UpdateClearButtonVisibility();
        this.ApplyCurrentIconSizes();
    }

    /// <inheritdoc />
    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnGotKeyboardFocus(e);

        this.textBox?.Focus();
        this.textBox?.SelectAll();
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == LanguageProperty)
        {
            this.RebuildHourItems();
            this.RebuildMinuteItems();
            this.SynchronizeTextFromSelectedTime();
        }
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Attaches event handlers to the current template parts.
    /// </summary>
    private void AttachTemplateParts()
    {
        if (this.textBox is not null)
        {
            this.textBox.LostKeyboardFocus += this.OnInnerTextBoxLostKeyboardFocus;
            this.textBox.KeyDown += this.OnInnerTextBoxKeyDown;
        }

        if (this.clearButton is not null)
        {
            this.clearButton.Click += this.OnClearButtonClick;
            this.clearButton.LostKeyboardFocus += this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.dropDownButton is not null)
        {
            this.dropDownButton.Click += this.OnDropDownButtonClick;
            this.dropDownButton.LostKeyboardFocus += this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.popup is not null)
        {
            this.popup.Closed += this.OnPopupClosed;
        }

        if (this.hourListBox is not null)
        {
            this.hourListBox.ItemsSource = this.HourItems;
            this.hourListBox.SelectionChanged += this.OnHourListBoxSelectionChanged;
            this.hourListBox.MouseLeftButtonUp += this.OnHourListBoxMouseLeftButtonUp;
            this.hourListBox.LostKeyboardFocus += this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.minuteListBox is not null)
        {
            this.minuteListBox.ItemsSource = this.MinuteItems;
            this.minuteListBox.SelectionChanged += this.OnMinuteListBoxSelectionChanged;
            this.minuteListBox.MouseLeftButtonUp += this.OnMinuteListBoxMouseLeftButtonUp;
            this.minuteListBox.LostKeyboardFocus += this.OnFocusableTemplatePartLostKeyboardFocus;
        }
    }

    /// <summary>
    /// Detaches event handlers from the current template parts.
    /// </summary>
    private void DetachTemplateParts()
    {
        if (this.textBox is not null)
        {
            this.textBox.LostKeyboardFocus -= this.OnInnerTextBoxLostKeyboardFocus;
            this.textBox.KeyDown -= this.OnInnerTextBoxKeyDown;
        }

        if (this.clearButton is not null)
        {
            this.clearButton.Click -= this.OnClearButtonClick;
            this.clearButton.LostKeyboardFocus -= this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.dropDownButton is not null)
        {
            this.dropDownButton.Click -= this.OnDropDownButtonClick;
            this.dropDownButton.LostKeyboardFocus -= this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.popup is not null)
        {
            this.popup.Closed -= this.OnPopupClosed;
        }

        if (this.hourListBox is not null)
        {
            this.hourListBox.SelectionChanged -= this.OnHourListBoxSelectionChanged;
            this.hourListBox.MouseLeftButtonUp -= this.OnHourListBoxMouseLeftButtonUp;
            this.hourListBox.LostKeyboardFocus -= this.OnFocusableTemplatePartLostKeyboardFocus;
        }

        if (this.minuteListBox is not null)
        {
            this.minuteListBox.SelectionChanged -= this.OnMinuteListBoxSelectionChanged;
            this.minuteListBox.MouseLeftButtonUp -= this.OnMinuteListBoxMouseLeftButtonUp;
            this.minuteListBox.LostKeyboardFocus -= this.OnFocusableTemplatePartLostKeyboardFocus;
        }
    }

    /// <summary>
    /// Applies the configured icon sizes to the current leading and trailing icon objects.
    /// </summary>
    private void ApplyCurrentIconSizes()
    {
        ApplyIconSize(this.LeadingIcon, this.LeadingIconSize);
        ApplyIconSize(this.TrailingIcon, this.TrailingIconSize);
    }

    /// <summary>
    /// Applies a numeric icon size to common WPF icon controls.
    /// </summary>
    /// <param name="icon">The icon object.</param>
    /// <param name="iconSize">The icon size.</param>
    private static void ApplyIconSize(object? icon, double iconSize)
    {
        if (icon is null || double.IsNaN(iconSize) || iconSize <= 0d)
        {
            return;
        }

        if (icon is DependencyObject dependencyObject)
        {
            TrySetDependencyProperty(dependencyObject, "SizeProperty", iconSize);
        }

        if (icon is FrameworkElement frameworkElement && ShouldApplyFrameworkElementSize(frameworkElement))
        {
            frameworkElement.Width = iconSize;
            frameworkElement.Height = iconSize;
        }
    }

    /// <summary>
    /// Gets a value indicating whether width and height should be applied directly to the specified element.
    /// </summary>
    /// <param name="frameworkElement">The framework element to inspect.</param>
    /// <returns><c>true</c> if direct width and height assignment is appropriate; otherwise, <c>false</c>.</returns>
    private static bool ShouldApplyFrameworkElementSize(FrameworkElement frameworkElement)
    {
        if (frameworkElement is ButtonBase)
        {
            return false;
        }

        string typeName = frameworkElement.GetType().Name;
        string? namespaceName = frameworkElement.GetType().Namespace;

        return typeName.Contains("Icon", StringComparison.OrdinalIgnoreCase)
            || namespaceName?.Contains("IconPacks", StringComparison.OrdinalIgnoreCase) == true;
    }

    /// <summary>
    /// Sets a dependency property by its static field name if it exists and accepts a <see cref="double"/> value.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="propertyFieldName">The static dependency property field name.</param>
    /// <param name="value">The value to set.</param>
    private static void TrySetDependencyProperty(DependencyObject dependencyObject, string propertyFieldName, double value)
    {
        FieldInfo? fieldInfo = dependencyObject.GetType().GetField(
            propertyFieldName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (fieldInfo?.GetValue(null) is not DependencyProperty dependencyProperty ||
            dependencyProperty.PropertyType != typeof(double))
        {
            return;
        }

        dependencyObject.SetValue(dependencyProperty, value);
    }

    /// <summary>
    /// Rebuilds the available hour values.
    /// </summary>
    private void RebuildHourItems()
    {
        this.HourItems.Clear();

        for (int hour = 0; hour <= 23; hour++)
        {
            this.HourItems.Add(new XTimePickerValueItem(hour, hour.ToString("00", CultureInfo.CurrentCulture)));
        }

        this.SynchronizeListSelectionsFromSelectedTime();
    }

    /// <summary>
    /// Rebuilds the available minute values.
    /// </summary>
    private void RebuildMinuteItems()
    {
        this.MinuteItems.Clear();

        for (int minute = 0; minute <= 59; minute += this.StepMinutes)
        {
            this.MinuteItems.Add(new XTimePickerValueItem(minute, minute.ToString("00", CultureInfo.CurrentCulture)));
        }

        this.SynchronizeListSelectionsFromSelectedTime();
    }

    /// <summary>
    /// Formats the specified time value.
    /// </summary>
    /// <param name="time">The time to format.</param>
    /// <returns>The formatted text.</returns>
    private string FormatTime(TimeSpan time)
    {
        DateTime dateTime = DateTime.Today.Add(time);
        return dateTime.ToString(this.GetDateTimeFormatString(), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Gets the current date-time based format string.
    /// </summary>
    /// <returns>The format string.</returns>
    private string GetDateTimeFormatString()
    {
        return this.TimeFormatString switch
        {
            @"hh\:mm" => "HH:mm",
            @"hh\:mm\:ss" => "HH:mm:ss",
            _ => this.TimeFormatString
        };
    }

    /// <summary>
    /// Synchronizes <see cref="Text"/> from <see cref="SelectedTime"/>.
    /// </summary>
    private void SynchronizeTextFromSelectedTime()
    {
        if (this.isSynchronizing)
        {
            return;
        }

        try
        {
            this.isSynchronizing = true;

            string text = this.SelectedTime.HasValue
                ? this.FormatTime(this.SelectedTime.Value)
                : string.Empty;

            if (this.Text != text)
            {
                this.SetCurrentValue(TextProperty, text);
            }

            if (this.textBox is not null && this.textBox.Text != text)
            {
                this.textBox.Text = text;
            }
        }
        finally
        {
            this.isSynchronizing = false;
        }
    }

    /// <summary>
    /// Synchronizes the popup list selections from <see cref="SelectedTime"/>.
    /// </summary>
    private void SynchronizeListSelectionsFromSelectedTime()
    {
        if (this.hourListBox is null || this.minuteListBox is null)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;

            TimeSpan selectionTime = this.SelectedTime ?? this.GetDefaultPopupSelectionTime();

            int selectedHour = selectionTime.Hours;
            int selectedMinute = this.GetNearestMinuteValue(selectionTime.Minutes);

            XTimePickerValueItem? selectedHourItem = null;
            foreach (XTimePickerValueItem item in this.HourItems)
            {
                if (item.Value == selectedHour)
                {
                    selectedHourItem = item;
                    break;
                }
            }

            XTimePickerValueItem? selectedMinuteItem = null;
            foreach (XTimePickerValueItem item in this.MinuteItems)
            {
                if (item.Value == selectedMinute)
                {
                    selectedMinuteItem = item;
                    break;
                }
            }

            this.hourListBox.SelectedItem = selectedHourItem;
            this.minuteListBox.SelectedItem = selectedMinuteItem;

            if (selectedHourItem is not null)
            {
                this.hourListBox.ScrollIntoView(selectedHourItem);
            }

            if (selectedMinuteItem is not null)
            {
                this.minuteListBox.ScrollIntoView(selectedMinuteItem);
            }
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Commits the current text to <see cref="SelectedTime"/> when possible.
    /// </summary>
    private void CommitText()
    {
        if (string.IsNullOrWhiteSpace(this.Text))
        {
            this.SetCurrentValue(SelectedTimeProperty, null);
            this.SynchronizeTextFromSelectedTime();
            this.SynchronizeListSelectionsFromSelectedTime();
            this.UpdateClearButtonVisibility();
            return;
        }

        if (!DateTime.TryParse(this.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime parsedDateTime))
        {
            this.SynchronizeTextFromSelectedTime();
            this.UpdateClearButtonVisibility();
            return;
        }

        TimeSpan parsedTime = new(parsedDateTime.Hour, parsedDateTime.Minute, 0);
        parsedTime = this.NormalizeTime(parsedTime);
        parsedTime = this.ClampTime(parsedTime);

        this.SetCurrentValue(SelectedTimeProperty, parsedTime);
        this.SynchronizeTextFromSelectedTime();
        this.SynchronizeListSelectionsFromSelectedTime();
        this.UpdateClearButtonVisibility();
    }

    /// <summary>
    /// Gets the default popup selection used when no time is selected yet.
    /// </summary>
    /// <returns>The default popup selection time.</returns>
    private TimeSpan GetDefaultPopupSelectionTime()
    {
        TimeSpan defaultTime = this.MinimumTime ?? TimeSpan.Zero;
        defaultTime = this.NormalizeTime(defaultTime);
        return this.ClampTime(defaultTime);
    }

    /// <summary>
    /// Normalizes a time according to the configured minute step.
    /// </summary>
    /// <param name="time">The time to normalize.</param>
    /// <returns>The normalized time.</returns>
    private TimeSpan NormalizeTime(TimeSpan time)
    {
        int minute = this.GetNearestMinuteValue(time.Minutes);
        return new TimeSpan(time.Hours, minute, 0);
    }

    /// <summary>
    /// Gets the nearest configured minute value for the specified minute.
    /// </summary>
    /// <param name="minute">The source minute.</param>
    /// <returns>The nearest valid minute value.</returns>
    private int GetNearestMinuteValue(int minute)
    {
        if (this.MinuteItems.Count == 0)
        {
            return 0;
        }

        int nearestMinute = this.MinuteItems[0].Value;
        int nearestDistance = Math.Abs(minute - nearestMinute);

        foreach (XTimePickerValueItem item in this.MinuteItems)
        {
            int distance = Math.Abs(minute - item.Value);
            if (distance < nearestDistance)
            {
                nearestMinute = item.Value;
                nearestDistance = distance;
            }
        }

        return nearestMinute;
    }

    /// <summary>
    /// Clamps the specified time to the configured range.
    /// </summary>
    /// <param name="time">The time to clamp.</param>
    /// <returns>The clamped time.</returns>
    private TimeSpan ClampTime(TimeSpan time)
    {
        if (this.MinimumTime.HasValue && time < this.MinimumTime.Value)
        {
            time = this.MinimumTime.Value;
        }

        if (this.MaximumTime.HasValue && time > this.MaximumTime.Value)
        {
            time = this.MaximumTime.Value;
        }

        return time;
    }

    /// <summary>
    /// Applies the current hour and minute selections to <see cref="SelectedTime"/>.
    /// </summary>
    /// <param name="closePopup">A value indicating whether the popup should be closed afterwards.</param>
    private void ApplySelectionFromLists(bool closePopup)
    {
        if (this.hourListBox?.SelectedItem is not XTimePickerValueItem hourItem ||
            this.minuteListBox?.SelectedItem is not XTimePickerValueItem minuteItem)
        {
            return;
        }

        TimeSpan selectedTime = new(hourItem.Value, minuteItem.Value, 0);
        selectedTime = this.ClampTime(selectedTime);

        this.SetCurrentValue(SelectedTimeProperty, selectedTime);
        this.SynchronizeTextFromSelectedTime();
        this.UpdateClearButtonVisibility();

        if (closePopup)
        {
            this.SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    /// <summary>
    /// Closes the picker after focus has really left both the control and the popup content.
    /// </summary>
    /// <param name="newFocus">The element receiving keyboard focus.</param>
    private void ClosePickerAfterFocusLossIfNeeded(DependencyObject? newFocus)
    {
        if (!this.IsDropDownOpen)
        {
            return;
        }

        _ = this.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                DependencyObject? focusedElement = Keyboard.FocusedElement as DependencyObject ?? newFocus;

                if (this.IsKeyboardFocusWithin || this.IsElementWithinPicker(focusedElement))
                {
                    return;
                }

                this.SetCurrentValue(IsDropDownOpenProperty, false);
            }));
    }

    /// <summary>
    /// Gets a value indicating whether the specified element belongs to this picker or its popup content.
    /// </summary>
    /// <param name="element">The element to inspect.</param>
    /// <returns><c>true</c> when the element belongs to this picker; otherwise, <c>false</c>.</returns>
    private bool IsElementWithinPicker(DependencyObject? element)
    {
        while (element is not null)
        {
            if (ReferenceEquals(element, this) ||
                ReferenceEquals(element, this.textBox) ||
                ReferenceEquals(element, this.clearButton) ||
                ReferenceEquals(element, this.dropDownButton) ||
                ReferenceEquals(element, this.popup) ||
                ReferenceEquals(element, this.hourListBox) ||
                ReferenceEquals(element, this.minuteListBox))
            {
                return true;
            }

            element = GetParent(element);
        }

        return false;
    }

    /// <summary>
    /// Gets the most likely parent of the specified dependency object.
    /// </summary>
    /// <param name="element">The element whose parent is requested.</param>
    /// <returns>The parent element, if available; otherwise, <c>null</c>.</returns>
    private static DependencyObject? GetParent(DependencyObject element)
    {
        if (element is Visual or Visual3D)
        {
            DependencyObject? visualParent = VisualTreeHelper.GetParent(element);
            if (visualParent is not null)
            {
                return visualParent;
            }
        }

        DependencyObject? logicalParent = LogicalTreeHelper.GetParent(element);
        if (logicalParent is not null)
        {
            return logicalParent;
        }

        if (element is FrameworkElement frameworkElement)
        {
            if (frameworkElement.Parent is not null)
            {
                return frameworkElement.Parent;
            }

            if (frameworkElement.TemplatedParent is DependencyObject templatedParent)
            {
                return templatedParent;
            }
        }

        return element is FrameworkContentElement frameworkContentElement
            ? frameworkContentElement.Parent
            : null;
    }

    /// <summary>
    /// Finds the first visual ancestor of the specified type.
    /// </summary>
    /// <typeparam name="T">The ancestor type.</typeparam>
    /// <param name="dependencyObject">The starting object.</param>
    /// <returns>The ancestor instance or <see langword="null"/>.</returns>
    private static T? FindAncestor<T>(DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        while (dependencyObject is not null)
        {
            if (dependencyObject is T typedDependencyObject)
            {
                return typedDependencyObject;
            }

            dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
        }

        return null;
    }

    /// <summary>
    /// Handles lost keyboard focus on the inner text box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        this.CommitText();
        this.ClosePickerAfterFocusLossIfNeeded(e.NewFocus as DependencyObject);
    }

    /// <summary>
    /// Handles lost keyboard focus on a focusable template part.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnFocusableTemplatePartLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        this.ClosePickerAfterFocusLossIfNeeded(e.NewFocus as DependencyObject);
    }

    /// <summary>
    /// Handles key presses on the inner text box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerTextBoxKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                this.CommitText();
                e.Handled = true;
                break;

            case Key.Down:
                if (!this.IsDropDownOpen && this.IsEnabled && !this.IsReadOnly)
                {
                    this.SetCurrentValue(IsDropDownOpenProperty, true);
                    this.SynchronizeListSelectionsFromSelectedTime();
                    e.Handled = true;
                }

                break;

            case Key.Escape:
                if (this.IsDropDownOpen)
                {
                    this.SetCurrentValue(IsDropDownOpenProperty, false);
                    e.Handled = true;
                }

                break;
        }
    }

    /// <summary>
    /// Clears the current time.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnClearButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        this.SetCurrentValue(SelectedTimeProperty, null);
        this.SynchronizeTextFromSelectedTime();
        this.SynchronizeListSelectionsFromSelectedTime();
        this.UpdateClearButtonVisibility();
        this.textBox?.Focus();
    }

    /// <summary>
    /// Toggles the time popup.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnDropDownButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        if (this.IsDropDownOpen)
        {
            this.SetCurrentValue(IsDropDownOpenProperty, false);
            return;
        }

        this.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                if (!this.IsEnabled || this.IsReadOnly || this.IsDropDownOpen)
                {
                    return;
                }

                this.SetCurrentValue(IsDropDownOpenProperty, true);
                this.SynchronizeListSelectionsFromSelectedTime();
            }));
    }

    /// <summary>
    /// Handles popup close.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnPopupClosed(object? sender, EventArgs e)
    {
        this.SetCurrentValue(IsDropDownOpenProperty, false);
    }

    /// <summary>
    /// Handles selection changes in the hour list box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnHourListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        this.ApplySelectionFromLists(false);
    }

    /// <summary>
    /// Handles selection changes in the minute list box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnMinuteListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        this.ApplySelectionFromLists(true);
    }

    /// <summary>
    /// Handles mouse clicks in the hour list box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnHourListBoxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (this.hourListBox is null)
        {
            return;
        }

        ListBoxItem? listBoxItem = FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
        if (listBoxItem?.DataContext is not XTimePickerValueItem clickedItem)
        {
            return;
        }

        this.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                this.hourListBox.SelectedItem = clickedItem;
                this.ApplySelectionFromLists(false);
            }));
    }

    /// <summary>
    /// Handles mouse clicks in the minute list box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnMinuteListBoxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (this.minuteListBox is null)
        {
            return;
        }

        ListBoxItem? listBoxItem = FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
        if (listBoxItem?.DataContext is not XTimePickerValueItem clickedItem)
        {
            return;
        }

        this.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                this.minuteListBox.SelectedItem = clickedItem;
                this.ApplySelectionFromLists(true);
            }));
    }

    /// <summary>
    /// Handles changes to the enabled state of the control.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        this.UpdateClearButtonVisibility();

        if (e.NewValue is false)
        {
            this.SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    /// <summary>
    /// Updates the clear button visibility state.
    /// </summary>
    private void UpdateClearButtonVisibility()
    {
        if (this.clearButton is null)
        {
            return;
        }

        bool shouldShow =
            this.ShowResetButton &&
            this.IsEnabled &&
            !this.IsReadOnly &&
            !string.IsNullOrWhiteSpace(this.Text);

        this.clearButton.Visibility = shouldShow
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>
    /// Handles changes to <see cref="LeadingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnLeadingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTimePicker timePicker)
        {
            ApplyIconSize(eventArgs.NewValue, timePicker.LeadingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="TrailingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTrailingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTimePicker timePicker)
        {
            ApplyIconSize(eventArgs.NewValue, timePicker.TrailingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to icon size properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTimePicker timePicker)
        {
            timePicker.ApplyCurrentIconSizes();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="SelectedTime"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XTimePicker timePicker)
        {
            return;
        }

        timePicker.SynchronizeTextFromSelectedTime();
        timePicker.SynchronizeListSelectionsFromSelectedTime();
        timePicker.UpdateClearButtonVisibility();
    }

    /// <summary>
    /// Handles changes to <see cref="Text"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XTimePicker timePicker)
        {
            return;
        }

        timePicker.UpdateClearButtonVisibility();
    }

    /// <summary>
    /// Handles changes to the time range.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnTimeRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XTimePicker timePicker)
        {
            return;
        }

        if (timePicker.SelectedTime.HasValue)
        {
            timePicker.SetCurrentValue(SelectedTimeProperty, timePicker.ClampTime(timePicker.SelectedTime.Value));
        }

        timePicker.SynchronizeTextFromSelectedTime();
        timePicker.RebuildHourItems();
        timePicker.RebuildMinuteItems();
    }

    /// <summary>
    /// Handles changes to <see cref="StepMinutes"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnStepMinutesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XTimePicker timePicker)
        {
            return;
        }

        timePicker.RebuildMinuteItems();

        if (timePicker.SelectedTime.HasValue)
        {
            TimeSpan normalizedTime = timePicker.NormalizeTime(timePicker.SelectedTime.Value);
            timePicker.SetCurrentValue(SelectedTimeProperty, timePicker.ClampTime(normalizedTime));
        }
    }

    /// <summary>
    /// Coerces the step size to a valid positive value.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="baseValue">The proposed value.</param>
    /// <returns>The coerced value.</returns>
    private static object CoerceStepMinutes(DependencyObject d, object baseValue)
    {
        int value = (int)baseValue;

        return value switch
        {
            5 => 5,
            10 => 10,
            15 => 15,
            30 => 30,
            60 => 60,
            _ => 15
        };
    }

    /// <summary>
    /// Handles changes to <see cref="TimeFormatString"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnTimeFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XTimePicker timePicker)
        {
            return;
        }

        timePicker.SynchronizeTextFromSelectedTime();
    }

    /// <summary>
    /// Handles changes to <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnShowResetButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XTimePicker timePicker)
        {
            timePicker.UpdateClearButtonVisibility();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IsReadOnly"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XTimePicker timePicker)
        {
            timePicker.UpdateClearButtonVisibility();

            if (e.NewValue is true)
            {
                timePicker.SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }
    }
    #endregion
}
#endregion

#region ### Class XTimePickerValueItem ###
/// <summary>
/// Represents a selectable numeric value entry in <see cref="XTimePicker"/>.
/// </summary>
public sealed class XTimePickerValueItem
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTimePickerValueItem"/> class.
    /// </summary>
    /// <param name="value">The numeric value.</param>
    /// <param name="displayText">The formatted display text.</param>
    public XTimePickerValueItem(int value, string displayText)
    {
        this.Value = value;
        this.DisplayText = displayText;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the numeric value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the formatted display text.
    /// </summary>
    public string DisplayText { get; }
    #endregion
}
#endregion
