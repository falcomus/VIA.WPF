// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDatePicker.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XDatePicker ###
/// <summary>
/// Represents the standard date picker control of VIA.WPF.
/// </summary>
[TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
[TemplatePart(Name = ClearButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = DropDownButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
[TemplatePart(Name = CalendarPartName, Type = typeof(Calendar))]
public class XDatePicker : Control
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
    /// The name of the calendar template part.
    /// </summary>
    private const string CalendarPartName = "PART_Calendar";

    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="SelectedDate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
        nameof(SelectedDate),
        typeof(DateTime?),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));

    /// <summary>
    /// Identifies the <see cref="Text"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

    /// <summary>
    /// Identifies the <see cref="DisplayDate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register(
        nameof(DisplayDate),
        typeof(DateTime),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(DateTime.Today, OnDisplayDateChanged));

    /// <summary>
    /// Identifies the <see cref="MinimumDate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumDateProperty = DependencyProperty.Register(
        nameof(MinimumDate),
        typeof(DateTime?),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null, OnMinimumDateChanged));

    /// <summary>
    /// Identifies the <see cref="MaximumDate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaximumDateProperty = DependencyProperty.Register(
        nameof(MaximumDate),
        typeof(DateTime?),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null, OnMaximumDateChanged));

    /// <summary>
    /// Identifies the <see cref="DateFormatString"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DateFormatStringProperty = DependencyProperty.Register(
        nameof(DateFormatString),
        typeof(string),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata("d", OnDateFormatStringChanged));

    /// <summary>
    /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(true, OnShowResetButtonChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null, OnLeadingIconChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null, OnTrailingIconChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsReadOnly"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

    /// <summary>
    /// Identifies the <see cref="FirstDayOfWeek"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
        nameof(FirstDayOfWeek),
        typeof(DayOfWeek),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek, OnFirstDayOfWeekChanged));

    /// <summary>
    /// Identifies the <see cref="CaretBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
        nameof(CaretBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XDatePicker),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XDatePicker),
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
    /// The current calendar.
    /// </summary>
    private Calendar? calendar;


    /// <summary>
    /// Prevents recursive synchronization between text and date.
    /// </summary>
    private bool isSynchronizing;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XDatePicker"/> class.
    /// </summary>
    static XDatePicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XDatePicker),
            new FrameworkPropertyMetadata(typeof(XDatePicker)));

        FocusableProperty.OverrideMetadata(
            typeof(XDatePicker),
            new FrameworkPropertyMetadata(true));
    }

    public XDatePicker()
    {
        this.IsEnabledChanged += this.OnIsEnabledChanged;
    }

    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the selected date.
    /// </summary>
    public DateTime? SelectedDate
    {
        get => (DateTime?)this.GetValue(SelectedDateProperty);
        set => this.SetValue(SelectedDateProperty, value);
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
    /// Gets or sets the date displayed by the calendar.
    /// </summary>
    public DateTime DisplayDate
    {
        get => (DateTime)this.GetValue(DisplayDateProperty);
        set => this.SetValue(DisplayDateProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum selectable date.
    /// </summary>
    public DateTime? MinimumDate
    {
        get => (DateTime?)this.GetValue(MinimumDateProperty);
        set => this.SetValue(MinimumDateProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum selectable date.
    /// </summary>
    public DateTime? MaximumDate
    {
        get => (DateTime?)this.GetValue(MaximumDateProperty);
        set => this.SetValue(MaximumDateProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string used for displayed dates.
    /// </summary>
    public string DateFormatString
    {
        get => (string)this.GetValue(DateFormatStringProperty);
        set => this.SetValue(DateFormatStringProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the calendar popup is open.
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
    /// Gets or sets the first day of week shown by the calendar.
    /// </summary>
    public DayOfWeek FirstDayOfWeek
    {
        get => (DayOfWeek)this.GetValue(FirstDayOfWeekProperty);
        set => this.SetValue(FirstDayOfWeekProperty, value);
    }

    /// <summary>
    /// Gets or sets the caret brush used by the inner text box.
    /// </summary>
    public System.Windows.Media.Brush? CaretBrush
    {
        get => (System.Windows.Media.Brush?)this.GetValue(CaretBrushProperty);
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
        this.calendar = this.GetTemplateChild(CalendarPartName) as Calendar;

        this.ApplyCurrentIconSizes();
        this.AttachTemplateParts();
        this.SynchronizeTextFromSelectedDate();
        this.SynchronizeCalendarFromSelectedDate();
        this.UpdateClearButtonVisibility();
    }

    /// <inheritdoc />
    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnGotKeyboardFocus(e);

        this.textBox?.Focus();
        this.textBox?.SelectAll();
    }

    /// <summary>
    /// Handles changes to the enabled state of the control.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        this.UpdateClearButtonVisibility();
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
        }

        if (this.dropDownButton is not null)
        {
            this.dropDownButton.Click += this.OnDropDownButtonClick;
        }

        if (this.popup is not null)
        {
            this.popup.Closed += this.OnPopupClosed;
        }

        if (this.calendar is not null)
        {
            this.calendar.SelectedDatesChanged += this.OnCalendarSelectedDatesChanged;
            this.calendar.DisplayDate = this.DisplayDate;
            this.calendar.DisplayDateStart = this.MinimumDate;
            this.calendar.DisplayDateEnd = this.MaximumDate;
            this.calendar.FirstDayOfWeek = this.FirstDayOfWeek;
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
        }

        if (this.dropDownButton is not null)
        {
            this.dropDownButton.Click -= this.OnDropDownButtonClick;
        }

        if (this.popup is not null)
        {
            this.popup.Closed -= this.OnPopupClosed;
        }

        if (this.calendar is not null)
        {
            this.calendar.SelectedDatesChanged -= this.OnCalendarSelectedDatesChanged;
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

        if (fieldInfo?.GetValue(null) is not DependencyProperty dependencyProperty
            || dependencyProperty.PropertyType != typeof(double))
        {
            return;
        }

        dependencyObject.SetValue(dependencyProperty, value);
    }

    /// <summary>
    /// Synchronizes <see cref="Text"/> from <see cref="SelectedDate"/>.
    /// </summary>
    private void SynchronizeTextFromSelectedDate()
    {
        if (this.isSynchronizing)
        {
            return;
        }

        try
        {
            this.isSynchronizing = true;

            string text = this.SelectedDate.HasValue
                ? this.SelectedDate.Value.ToString(this.DateFormatString, System.Globalization.CultureInfo.CurrentCulture)
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
    /// Synchronizes calendar state from <see cref="SelectedDate"/>.
    /// </summary>
    private void SynchronizeCalendarFromSelectedDate()
    {
        if (this.calendar is null)
        {
            return;
        }

        if (this.SelectedDate.HasValue)
        {
            DateTime date = this.SelectedDate.Value.Date;
            if (this.calendar.SelectedDate != date)
            {
                this.calendar.SelectedDate = date;
            }

            this.calendar.DisplayDate = date;
        }
        else
        {
            this.calendar.SelectedDate = null;
            this.calendar.DisplayDate = this.DisplayDate;
        }
    }

    /// <summary>
    /// Commits the current text to <see cref="SelectedDate"/> when possible.
    /// </summary>
    private void CommitText()
    {
        if (string.IsNullOrWhiteSpace(this.Text))
        {
            this.SetCurrentValue(SelectedDateProperty, null);
            this.SynchronizeTextFromSelectedDate();
            this.UpdateClearButtonVisibility();
            return;
        }

        if (!DateTime.TryParse(this.Text, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            this.SynchronizeTextFromSelectedDate();
            this.UpdateClearButtonVisibility();
            return;
        }

        parsedDate = parsedDate.Date;

        if (this.MinimumDate.HasValue && parsedDate < this.MinimumDate.Value.Date)
        {
            parsedDate = this.MinimumDate.Value.Date;
        }

        if (this.MaximumDate.HasValue && parsedDate > this.MaximumDate.Value.Date)
        {
            parsedDate = this.MaximumDate.Value.Date;
        }

        this.SetCurrentValue(SelectedDateProperty, parsedDate);
        this.SetCurrentValue(DisplayDateProperty, parsedDate);
        this.SynchronizeTextFromSelectedDate();
        this.SynchronizeCalendarFromSelectedDate();
        this.UpdateClearButtonVisibility();
    }

    /// <summary>
    /// Handles lost keyboard focus on the inner text box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        this.CommitText();
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
    /// Clears the current date.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnClearButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        this.SetCurrentValue(SelectedDateProperty, null);
        this.SynchronizeTextFromSelectedDate();
        this.SynchronizeCalendarFromSelectedDate();
        this.UpdateClearButtonVisibility();
        this.textBox?.Focus();
    }

    /// <summary>
    /// Toggles the calendar popup.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnDropDownButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled)
        {
            return;
        }

        this.SetCurrentValue(IsDropDownOpenProperty, !this.IsDropDownOpen);
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
    /// Handles date selection in the calendar.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnCalendarSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (this.calendar?.SelectedDate is not DateTime selectedDate)
        {
            return;
        }

        this.SetCurrentValue(SelectedDateProperty, selectedDate.Date);
        this.SetCurrentValue(DisplayDateProperty, selectedDate.Date);
        this.SynchronizeTextFromSelectedDate();
        this.UpdateClearButtonVisibility();
        this.SetCurrentValue(IsDropDownOpenProperty, false);
    }

    /// <summary>
    /// Handles changes to <see cref="LeadingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnLeadingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XDatePicker datePicker)
        {
            ApplyIconSize(eventArgs.NewValue, datePicker.LeadingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="TrailingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTrailingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XDatePicker datePicker)
        {
            ApplyIconSize(eventArgs.NewValue, datePicker.TrailingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to icon size properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XDatePicker datePicker)
        {
            datePicker.ApplyCurrentIconSizes();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="SelectedDate"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDatePicker datePicker)
        {
            return;
        }

        datePicker.SynchronizeTextFromSelectedDate();
        datePicker.SynchronizeCalendarFromSelectedDate();
        datePicker.UpdateClearButtonVisibility();

        if (datePicker.SelectedDate.HasValue)
        {
            datePicker.SetCurrentValue(DisplayDateProperty, datePicker.SelectedDate.Value.Date);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="Text"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDatePicker datePicker)
        {
            return;
        }

        if (datePicker.isSynchronizing)
        {
            datePicker.UpdateClearButtonVisibility();
            return;
        }

        datePicker.UpdateClearButtonVisibility();
    }

    /// <summary>
    /// Handles changes to <see cref="DisplayDate"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDatePicker datePicker && datePicker.calendar is not null)
        {
            datePicker.calendar.DisplayDate = datePicker.DisplayDate;
        }
    }

    /// <summary>
    /// Handles changes to <see cref="MinimumDate"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnMinimumDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDatePicker datePicker)
        {
            return;
        }

        if (datePicker.calendar is not null)
        {
            datePicker.calendar.DisplayDateStart = datePicker.MinimumDate;
        }

        if (datePicker.SelectedDate.HasValue && datePicker.MinimumDate.HasValue && datePicker.SelectedDate.Value.Date < datePicker.MinimumDate.Value.Date)
        {
            datePicker.SetCurrentValue(SelectedDateProperty, datePicker.MinimumDate.Value.Date);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="MaximumDate"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnMaximumDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XDatePicker datePicker)
        {
            return;
        }

        if (datePicker.calendar is not null)
        {
            datePicker.calendar.DisplayDateEnd = datePicker.MaximumDate;
        }

        if (datePicker.SelectedDate.HasValue && datePicker.MaximumDate.HasValue && datePicker.SelectedDate.Value.Date > datePicker.MaximumDate.Value.Date)
        {
            datePicker.SetCurrentValue(SelectedDateProperty, datePicker.MaximumDate.Value.Date);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="DateFormatString"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnDateFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDatePicker datePicker)
        {
            datePicker.SynchronizeTextFromSelectedDate();
            datePicker.UpdateClearButtonVisibility();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="FirstDayOfWeek"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnFirstDayOfWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDatePicker datePicker && datePicker.calendar is not null)
        {
            datePicker.calendar.FirstDayOfWeek = datePicker.FirstDayOfWeek;
        }
    }

    /// <summary>
    /// Handles changes to <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnShowResetButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDatePicker datePicker)
        {
            datePicker.UpdateClearButtonVisibility();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IsReadOnly"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XDatePicker datePicker)
        {
            datePicker.UpdateClearButtonVisibility();
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
    #endregion
}
#endregion
