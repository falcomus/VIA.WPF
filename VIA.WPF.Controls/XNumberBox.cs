// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNumberBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XNumberBox ###
/// <summary>
/// Represents the standard numeric input control of VIA.WPF.
/// </summary>
[TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
[TemplatePart(Name = SpinUpButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = SpinDownButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = SpinnerPanelPartName, Type = typeof(FrameworkElement))]
[TemplatePart(Name = SpinnerColumnPartName, Type = typeof(ColumnDefinition))]
public class XNumberBox : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the inner text box template part.
    /// </summary>
    private const string TextBoxPartName = "PART_TextBox";

    /// <summary>
    /// The name of the increment button template part.
    /// </summary>
    private const string SpinUpButtonPartName = "PART_SpinUpButton";

    /// <summary>
    /// The name of the spinner panel template part.
    /// </summary>
    private const string SpinnerPanelPartName = "PART_SpinnerPanel";

    /// <summary>
    /// The name of the spinner column template part.
    /// </summary>
    private const string SpinnerColumnPartName = "PART_SpinnerColumn";

    /// <summary>
    /// The name of the decrement button template part.
    /// </summary>
    private const string SpinDownButtonPartName = "PART_SpinDownButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Value"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double?),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue));

    /// <summary>
    /// Identifies the <see cref="Text"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

    /// <summary>
    /// Identifies the <see cref="Minimum"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(0d, OnMinimumChanged));

    /// <summary>
    /// Identifies the <see cref="Maximum"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(100d, OnMaximumChanged));

    /// <summary>
    /// Identifies the <see cref="Step"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
        nameof(Step),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(1d));

    /// <summary>
    /// Identifies the <see cref="FormatString"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
        nameof(FormatString),
        typeof(string),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata("G", OnFormatStringChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null, OnLeadingIconChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null, OnTrailingIconChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowSpinnerButtons"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowSpinnerButtonsProperty = DependencyProperty.Register(
        nameof(ShowSpinnerButtons),
        typeof(bool),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(true, OnSpinnerVisibilityPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsReadOnly"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(false, OnSpinnerVisibilityPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="CaretBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
        nameof(CaretBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XNumberBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The current inner text box.
    /// </summary>
    private TextBox? textBox;

    /// <summary>
    /// The current increment button.
    /// </summary>
    private Button? spinUpButton;

    /// <summary>
    /// The current decrement button.
    /// </summary>
    private Button? spinDownButton;

    /// <summary>
    /// The current spinner panel.
    /// </summary>
    private FrameworkElement? spinnerPanel;

    /// <summary>
    /// The current spinner column.
    /// </summary>
    private ColumnDefinition? spinnerColumn;

    /// <summary>
    /// Prevents recursive synchronization between text and value.
    /// </summary>
    private bool isSynchronizing;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XNumberBox"/> class.
    /// </summary>
    static XNumberBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XNumberBox),
            new FrameworkPropertyMetadata(typeof(XNumberBox)));

        FocusableProperty.OverrideMetadata(
            typeof(XNumberBox),
            new FrameworkPropertyMetadata(true));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XNumberBox"/> class.
    /// </summary>
    public XNumberBox()
    {
        this.IsEnabledChanged += this.XNumberBox_IsEnabledChanged;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the numeric value.
    /// </summary>
    public double? Value
    {
        get => (double?)this.GetValue(ValueProperty);
        set => this.SetValue(ValueProperty, value);
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
    /// Gets or sets the minimum allowed value.
    /// </summary>
    public double Minimum
    {
        get => (double)this.GetValue(MinimumProperty);
        set => this.SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    public double Maximum
    {
        get => (double)this.GetValue(MaximumProperty);
        set => this.SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets the increment and decrement step.
    /// </summary>
    public double Step
    {
        get => (double)this.GetValue(StepProperty);
        set => this.SetValue(StepProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string used for displayed numeric values.
    /// </summary>
    public string FormatString
    {
        get => (string)this.GetValue(FormatStringProperty);
        set => this.SetValue(FormatStringProperty, value);
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
    /// Gets or sets a value indicating whether spinner buttons are shown.
    /// </summary>
    public bool ShowSpinnerButtons
    {
        get => (bool)this.GetValue(ShowSpinnerButtonsProperty);
        set => this.SetValue(ShowSpinnerButtonsProperty, value);
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
        this.spinUpButton = this.GetTemplateChild(SpinUpButtonPartName) as Button;
        this.spinDownButton = this.GetTemplateChild(SpinDownButtonPartName) as Button;
        this.spinnerPanel = this.GetTemplateChild(SpinnerPanelPartName) as FrameworkElement;
        this.spinnerColumn = this.GetTemplateChild(SpinnerColumnPartName) as ColumnDefinition;

        this.AttachTemplateParts();
        this.SynchronizeTextFromValue();
        this.ApplyCurrentIconSizes();
        this.UpdateSpinnerVisibility();
    }

    /// <inheritdoc />
    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnGotKeyboardFocus(e);

        this.textBox?.Focus();
        this.textBox?.SelectAll();
    }

    /// <inheritdoc />
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        if (e.Delta > 0)
        {
            this.CommitText();
            this.Increment();
            e.Handled = true;
        }
        else if (e.Delta < 0)
        {
            this.CommitText();
            this.Decrement();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles enabled-state changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void XNumberBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        this.UpdateSpinnerVisibility();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Updates the visual visibility of the spinner template parts.
    /// </summary>
    private void UpdateSpinnerVisibility()
    {
        bool isSpinnerVisible = this.ShowSpinnerButtons && !this.IsReadOnly && this.IsEnabled;

        if (this.spinnerPanel is not null)
        {
            this.spinnerPanel.Visibility = isSpinnerVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        if (this.spinnerColumn is not null)
        {
            this.spinnerColumn.Width = isSpinnerVisible ? GridLength.Auto : new GridLength(0d);
        }
    }

    /// <summary>
    /// Attaches event handlers to the current template parts.
    /// </summary>
    private void AttachTemplateParts()
    {
        if (this.textBox is not null)
        {
            this.textBox.LostKeyboardFocus += this.OnInnerTextBoxLostKeyboardFocus;
            this.textBox.KeyDown += this.OnInnerTextBoxKeyDown;
            this.textBox.PreviewMouseWheel += this.OnInnerTextBoxPreviewMouseWheel;
        }

        if (this.spinUpButton is not null)
        {
            this.spinUpButton.Click += this.OnSpinUpButtonClick;
        }

        if (this.spinDownButton is not null)
        {
            this.spinDownButton.Click += this.OnSpinDownButtonClick;
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
            this.textBox.PreviewMouseWheel -= this.OnInnerTextBoxPreviewMouseWheel;
        }

        if (this.spinUpButton is not null)
        {
            this.spinUpButton.Click -= this.OnSpinUpButtonClick;
        }

        if (this.spinDownButton is not null)
        {
            this.spinDownButton.Click -= this.OnSpinDownButtonClick;
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
    /// Synchronizes <see cref="Text"/> from <see cref="Value"/>.
    /// </summary>
    private void SynchronizeTextFromValue()
    {
        if (this.isSynchronizing)
        {
            return;
        }

        try
        {
            this.isSynchronizing = true;

            string text = this.Value.HasValue
                ? this.FormatValue(this.Value.Value)
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
    /// Synchronizes <see cref="Value"/> from the current <see cref="Text"/> when possible.
    /// </summary>
    private void SynchronizeValueFromText()
    {
        if (this.isSynchronizing)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(this.Text))
        {
            this.SetCurrentValue(ValueProperty, null);
            return;
        }

        if (!double.TryParse(this.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double parsedValue))
        {
            return;
        }

        this.SetCurrentValue(ValueProperty, this.Clamp(parsedValue));
    }

    /// <summary>
    /// Commits the current text to the value and normalizes the display text.
    /// </summary>
    private void CommitText()
    {
        if (string.IsNullOrWhiteSpace(this.Text))
        {
            this.SetCurrentValue(ValueProperty, null);
            this.SynchronizeTextFromValue();
            return;
        }

        if (double.TryParse(this.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double parsedValue))
        {
            this.SetCurrentValue(ValueProperty, this.Clamp(parsedValue));
        }

        this.SynchronizeTextFromValue();
    }

    /// <summary>
    /// Formats the specified numeric value.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>The formatted string.</returns>
    private string FormatValue(double value)
    {
        return value.ToString(this.FormatString, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Clamps the specified value to the current range.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <returns>The clamped value.</returns>
    private double Clamp(double value)
    {
        return Math.Min(this.Maximum, Math.Max(this.Minimum, value));
    }

    /// <summary>
    /// Increments the current value.
    /// </summary>
    private void Increment()
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        double baseValue = this.Value ?? this.Minimum;
        this.SetCurrentValue(ValueProperty, this.Clamp(baseValue + this.Step));
        this.SynchronizeTextFromValue();
    }

    /// <summary>
    /// Decrements the current value.
    /// </summary>
    private void Decrement()
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        double baseValue = this.Value ?? this.Minimum;
        this.SetCurrentValue(ValueProperty, this.Clamp(baseValue - this.Step));
        this.SynchronizeTextFromValue();
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

            case Key.Up:
                this.CommitText();
                this.Increment();
                e.Handled = true;
                break;

            case Key.Down:
                this.CommitText();
                this.Decrement();
                e.Handled = true;
                break;
        }
    }

    /// <summary>
    /// Redirects mouse wheel handling from the inner text box to the outer control.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerTextBoxPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        if (e.Delta > 0)
        {
            this.CommitText();
            this.Increment();
            e.Handled = true;
        }
        else if (e.Delta < 0)
        {
            this.CommitText();
            this.Decrement();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles clicks on the increment button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnSpinUpButtonClick(object sender, RoutedEventArgs e)
    {
        this.Increment();
    }

    /// <summary>
    /// Handles clicks on the decrement button.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnSpinDownButtonClick(object sender, RoutedEventArgs e)
    {
        this.Decrement();
    }

    /// <summary>
    /// Handles changes that influence spinner visibility.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnSpinnerVisibilityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNumberBox numberBox)
        {
            numberBox.UpdateSpinnerVisibility();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="LeadingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnLeadingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNumberBox numberBox)
        {
            ApplyIconSize(eventArgs.NewValue, numberBox.LeadingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="TrailingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTrailingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNumberBox numberBox)
        {
            ApplyIconSize(eventArgs.NewValue, numberBox.TrailingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to icon size properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNumberBox numberBox)
        {
            numberBox.ApplyCurrentIconSizes();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="Value"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XNumberBox numberBox)
        {
            numberBox.SynchronizeTextFromValue();
        }
    }

    /// <summary>
    /// Coerces <see cref="Value"/> into the current range.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="baseValue">The base value.</param>
    /// <returns>The coerced value.</returns>
    private static object? CoerceValue(DependencyObject d, object? baseValue)
    {
        if (d is not XNumberBox numberBox || baseValue is not double value)
        {
            return baseValue;
        }

        return numberBox.Clamp(value);
    }

    /// <summary>
    /// Handles changes to <see cref="Text"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XNumberBox numberBox)
        {
            numberBox.SynchronizeValueFromText();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="Minimum"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XNumberBox numberBox)
        {
            return;
        }

        if (numberBox.Minimum > numberBox.Maximum)
        {
            numberBox.SetCurrentValue(MaximumProperty, numberBox.Minimum);
        }

        numberBox.CoerceValue(ValueProperty);
        numberBox.SynchronizeTextFromValue();
    }

    /// <summary>
    /// Handles changes to <see cref="Maximum"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XNumberBox numberBox)
        {
            return;
        }

        if (numberBox.Maximum < numberBox.Minimum)
        {
            numberBox.SetCurrentValue(MinimumProperty, numberBox.Maximum);
        }

        numberBox.CoerceValue(ValueProperty);
        numberBox.SynchronizeTextFromValue();
    }

    /// <summary>
    /// Handles changes to <see cref="FormatString"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XNumberBox numberBox)
        {
            numberBox.SynchronizeTextFromValue();
        }
    }
    #endregion
}
#endregion
