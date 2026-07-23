// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPasswordBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XPasswordBox ###
/// <summary>
/// Represents the standard password input control of VIA.WPF.
/// </summary>
[TemplatePart(Name = PasswordBoxPartName, Type = typeof(PasswordBox))]
[TemplatePart(Name = RevealedTextBoxPartName, Type = typeof(TextBox))]
[TemplatePart(Name = RevealButtonPartName, Type = typeof(Button))]
public class XPasswordBox : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the password box template part.
    /// </summary>
    private const string PasswordBoxPartName = "PART_PasswordBox";

    /// <summary>
    /// The name of the revealed text box template part.
    /// </summary>
    private const string RevealedTextBoxPartName = "PART_RevealedTextBox";

    /// <summary>
    /// The name of the reveal button template part.
    /// </summary>
    private const string RevealButtonPartName = "PART_RevealButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Password"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password),
        typeof(string),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(null, OnLeadingIconChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(null, OnTrailingIconChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HasRevealButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasRevealButtonProperty = DependencyProperty.Register(
        nameof(HasRevealButton),
        typeof(bool),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(true, OnHasRevealButtonChanged));

    /// <summary>
    /// Identifies the <see cref="IsPasswordRevealed"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(
        nameof(IsPasswordRevealed),
        typeof(bool),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(false, OnIsPasswordRevealedChanged));

    /// <summary>
    /// Identifies the <see cref="CaretBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
        nameof(CaretBrush),
        typeof(System.Windows.Media.Brush),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XPasswordBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The current password box template part.
    /// </summary>
    private PasswordBox? passwordBox;

    /// <summary>
    /// The current revealed text box template part.
    /// </summary>
    private TextBox? revealedTextBox;

    /// <summary>
    /// The current reveal button template part.
    /// </summary>
    private Button? revealButton;

    /// <summary>
    /// Prevents recursive synchronization while updating text values.
    /// </summary>
    private bool isSynchronizingPassword;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XPasswordBox"/> class.
    /// </summary>
    public XPasswordBox()
    {
        this.IsEnabledChanged += this.OnIsEnabledChanged;
    }

    /// <summary>
    /// Initializes static members of the <see cref="XPasswordBox"/> class.
    /// </summary>
    static XPasswordBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XPasswordBox),
            new FrameworkPropertyMetadata(typeof(XPasswordBox)));

        FocusableProperty.OverrideMetadata(
            typeof(XPasswordBox),
            new FrameworkPropertyMetadata(true));
    }
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when the password changes.
    /// </summary>
    public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(PasswordChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(XPasswordBox));

    /// <summary>
    /// Occurs when the password changes.
    /// </summary>
    public event RoutedEventHandler PasswordChanged
    {
        add => this.AddHandler(PasswordChangedEvent, value);
        remove => this.RemoveHandler(PasswordChangedEvent, value);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the password text.
    /// </summary>
    public string Password
    {
        get => (string)this.GetValue(PasswordProperty);
        set => this.SetValue(PasswordProperty, value);
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
    /// Gets or sets the size of the control.
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
    /// Gets or sets a value indicating whether a reveal button is shown.
    /// </summary>
    public bool HasRevealButton
    {
        get => (bool)this.GetValue(HasRevealButtonProperty);
        set => this.SetValue(HasRevealButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the password is currently revealed.
    /// </summary>
    public bool IsPasswordRevealed
    {
        get => (bool)this.GetValue(IsPasswordRevealedProperty);
        set => this.SetValue(IsPasswordRevealedProperty, value);
    }

    /// <summary>
    /// Gets or sets the caret brush used by the inner editors.
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

        this.passwordBox = this.GetTemplateChild(PasswordBoxPartName) as PasswordBox;
        this.revealedTextBox = this.GetTemplateChild(RevealedTextBoxPartName) as TextBox;
        this.revealButton = this.GetTemplateChild(RevealButtonPartName) as Button;

        this.AttachTemplateParts();
        this.SynchronizeEditorsFromPassword();
        this.UpdateRevealButtonVisibility();
        this.ApplyCurrentIconSizes();
    }

    /// <inheritdoc />
    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnGotKeyboardFocus(e);

        if (this.IsPasswordRevealed)
        {
            this.revealedTextBox?.Focus();
            this.revealedTextBox?.Select(this.revealedTextBox.Text.Length, 0);
        }
        else
        {
            this.passwordBox?.Focus();
        }
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Attaches event handlers to the current template parts.
    /// </summary>
    private void AttachTemplateParts()
    {
        if (this.passwordBox is not null)
        {
            this.passwordBox.PasswordChanged += this.OnInnerPasswordBoxPasswordChanged;
        }

        if (this.revealedTextBox is not null)
        {
            this.revealedTextBox.TextChanged += this.OnRevealedTextBoxTextChanged;
        }

        if (this.revealButton is not null)
        {
            this.revealButton.Click += this.OnRevealButtonClick;
        }
    }

    /// <summary>
    /// Detaches event handlers from the current template parts.
    /// </summary>
    private void DetachTemplateParts()
    {
        if (this.passwordBox is not null)
        {
            this.passwordBox.PasswordChanged -= this.OnInnerPasswordBoxPasswordChanged;
        }

        if (this.revealedTextBox is not null)
        {
            this.revealedTextBox.TextChanged -= this.OnRevealedTextBoxTextChanged;
        }

        if (this.revealButton is not null)
        {
            this.revealButton.Click -= this.OnRevealButtonClick;
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
    /// Synchronizes the inner editor values from <see cref="Password"/>.
    /// </summary>
    private void SynchronizeEditorsFromPassword()
    {
        if (this.isSynchronizingPassword)
        {
            return;
        }

        try
        {
            this.isSynchronizingPassword = true;

            string password = this.Password ?? string.Empty;

            if (this.passwordBox is not null && this.passwordBox.Password != password)
            {
                this.passwordBox.Password = password;
            }

            if (this.revealedTextBox is not null && this.revealedTextBox.Text != password)
            {
                this.revealedTextBox.Text = password;
            }
        }
        finally
        {
            this.isSynchronizingPassword = false;
        }
    }

    /// <summary>
    /// Updates the public password value from an inner editor.
    /// </summary>
    /// <param name="password">The new password value.</param>
    private void UpdatePasswordFromInnerEditor(string password)
    {
        if (this.isSynchronizingPassword || this.Password == password)
        {
            return;
        }

        try
        {
            this.isSynchronizingPassword = true;
            this.SetCurrentValue(PasswordProperty, password);
            this.SynchronizeEditorsFromPassword();
        }
        finally
        {
            this.isSynchronizingPassword = false;
        }

        this.UpdateRevealButtonVisibility();
        this.RaiseEvent(new RoutedEventArgs(PasswordChangedEvent, this));
    }

    /// <summary>
    /// Updates the reveal button visibility.
    /// </summary>
    private void UpdateRevealButtonVisibility()
    {
        if (this.revealButton is null)
        {
            return;
        }

        bool shouldShow =
            this.HasRevealButton &&
            this.IsEnabled &&
            !string.IsNullOrEmpty(this.Password);

        this.revealButton.Visibility = shouldShow
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>
    /// Handles changes to the enabled state of the control.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        this.UpdateRevealButtonVisibility();
    }

    /// <summary>
    /// Handles changes to the inner password box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
        this.UpdatePasswordFromInnerEditor(this.passwordBox?.Password ?? string.Empty);
    }

    /// <summary>
    /// Handles changes to the revealed text box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnRevealedTextBoxTextChanged(object sender, TextChangedEventArgs e)
    {
        this.UpdatePasswordFromInnerEditor(this.revealedTextBox?.Text ?? string.Empty);
    }

    /// <summary>
    /// Toggles the reveal state.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnRevealButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled)
        {
            return;
        }

        this.SetCurrentValue(IsPasswordRevealedProperty, !this.IsPasswordRevealed);

        if (this.IsPasswordRevealed)
        {
            this.revealedTextBox?.Focus();
            this.revealedTextBox?.Select(this.revealedTextBox.Text.Length, 0);
        }
        else
        {
            this.passwordBox?.Focus();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="LeadingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnLeadingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XPasswordBox passwordBox)
        {
            ApplyIconSize(eventArgs.NewValue, passwordBox.LeadingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="TrailingIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTrailingIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XPasswordBox passwordBox)
        {
            ApplyIconSize(eventArgs.NewValue, passwordBox.TrailingIconSize);
        }
    }

    /// <summary>
    /// Handles changes to icon size properties.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XPasswordBox passwordBox)
        {
            passwordBox.ApplyCurrentIconSizes();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="Password"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XPasswordBox passwordBox)
        {
            passwordBox.SynchronizeEditorsFromPassword();
            passwordBox.UpdateRevealButtonVisibility();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IsPasswordRevealed"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnIsPasswordRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XPasswordBox passwordBox)
        {
            passwordBox.SynchronizeEditorsFromPassword();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="HasRevealButton"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnHasRevealButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XPasswordBox passwordBox)
        {
            passwordBox.UpdateRevealButtonVisibility();
        }
    }
    #endregion
}
#endregion
