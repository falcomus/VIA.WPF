// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSecurePasswordBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XSecurePasswordBox ###
/// <summary>
/// Represents a secure password input control for login scenarios.
/// </summary>
/// <remarks>
/// This control intentionally does not expose a bindable clear-text password property.
/// Consumers can copy the current password as <see cref="SecureString"/> and should clear the control after login.
/// If the optional reveal button is enabled, clear text is only copied into the revealed editor while the password is revealed.
/// </remarks>
[TemplatePart(Name = PasswordBoxPartName, Type = typeof(PasswordBox))]
[TemplatePart(Name = RevealedTextBoxPartName, Type = typeof(TextBox))]
[TemplatePart(Name = RevealButtonPartName, Type = typeof(Button))]
public class XSecurePasswordBox : Control
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
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(null, OnLeadingIconChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(null, OnTrailingIconChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CaretBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(
        nameof(CaretBrush),
        typeof(Brush),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HasRevealButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasRevealButtonProperty = DependencyProperty.Register(
        nameof(HasRevealButton),
        typeof(bool),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(true, OnHasRevealButtonChanged));

    /// <summary>
    /// Identifies the <see cref="IsPasswordRevealed"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(
        nameof(IsPasswordRevealed),
        typeof(bool),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(false, OnIsPasswordRevealedChanged));

    /// <summary>
    /// Identifies the read-only <see cref="PasswordLength"/> dependency property.
    /// </summary>
    private static readonly DependencyPropertyKey PasswordLengthPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(PasswordLength),
        typeof(int),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(0));

    /// <summary>
    /// Identifies the <see cref="PasswordLength"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PasswordLengthProperty = PasswordLengthPropertyKey.DependencyProperty;

    #endregion

    #region ### Fields ###

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
    /// Indicates whether the inner editors are currently synchronized by code.
    /// </summary>
    private bool isSynchronizingEditors;


    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XSecurePasswordBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XSecurePasswordBox"/> class.
    /// </summary>
    static XSecurePasswordBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XSecurePasswordBox),
            new FrameworkPropertyMetadata(typeof(XSecurePasswordBox)));

        FocusableProperty.OverrideMetadata(
            typeof(XSecurePasswordBox),
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
        typeof(XSecurePasswordBox));

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
    /// Gets the current password length without exposing the password text.
    /// </summary>
    public int PasswordLength => (int)this.GetValue(PasswordLengthProperty);

    /// <summary>
    /// Gets a value indicating whether the control currently contains a password.
    /// </summary>
    public bool HasPassword => this.PasswordLength > 0;

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
    /// Gets or sets the caret brush used by the inner editors.
    /// </summary>
    public Brush? CaretBrush
    {
        get => (Brush?)this.GetValue(CaretBrushProperty);
        set => this.SetValue(CaretBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the reveal button is available.
    /// </summary>
    public bool HasRevealButton
    {
        get => (bool)this.GetValue(HasRevealButtonProperty);
        set => this.SetValue(HasRevealButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the password is currently revealed.
    /// </summary>
    /// <remarks>
    /// The clear-text editor is populated only while this value is <see langword="true"/>.
    /// </remarks>
    public bool IsPasswordRevealed
    {
        get => (bool)this.GetValue(IsPasswordRevealedProperty);
        set => this.SetValue(IsPasswordRevealedProperty, value);
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
        this.UpdatePasswordState();
        this.UpdateRevealButtonVisibility();
        this.ApplyCurrentIconSizes();
        this.ApplyRevealState();
    }

    /// <summary>
    /// Copies the current password into a new secure string.
    /// </summary>
    /// <returns>A copy of the current password. The caller owns and should dispose the returned instance.</returns>
    public SecureString CopySecurePassword()
    {
        return this.passwordBox?.SecurePassword.Copy() ?? new SecureString();
    }

    /// <summary>
    /// Clears the current password input.
    /// </summary>
    public void Clear()
    {
        this.SetCurrentValue(IsPasswordRevealedProperty, false);
        this.passwordBox?.Clear();

        if (this.revealedTextBox is not null)
        {
            this.revealedTextBox.Text = string.Empty;
        }

        this.UpdatePasswordState();
        this.UpdateRevealButtonVisibility();
    }

    /// <inheritdoc />
    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnGotKeyboardFocus(e);

        if (this.IsPasswordRevealed)
        {
            this.revealedTextBox?.Focus();
            return;
        }

        this.passwordBox?.Focus();
    }

    /// <inheritdoc />
    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == IsEnabledProperty && !this.IsEnabled)
        {
            this.IsPasswordRevealed = false;
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
    /// Updates the password state without exposing the password text.
    /// </summary>
    private void UpdatePasswordState()
    {
        int oldLength = this.PasswordLength;
        int newLength = this.passwordBox?.SecurePassword.Length ?? 0;

        if (oldLength == newLength)
        {
            return;
        }

        this.SetValue(PasswordLengthPropertyKey, newLength);
        this.OnPropertyChanged(new DependencyPropertyChangedEventArgs(PasswordLengthProperty, oldLength, newLength));
        this.RaiseEvent(new RoutedEventArgs(PasswordChangedEvent, this));
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
    /// Updates the reveal button visibility.
    /// </summary>
    private void UpdateRevealButtonVisibility()
    {
        if (this.revealButton is null)
        {
            return;
        }

        bool shouldShow = this.HasRevealButton && this.IsEnabled && this.PasswordLength > 0;

        this.revealButton.Visibility = shouldShow
            ? Visibility.Visible
            : Visibility.Collapsed;

        if (!shouldShow && this.IsPasswordRevealed)
        {
            this.SetCurrentValue(IsPasswordRevealedProperty, false);
        }
    }

    /// <summary>
    /// Applies the current reveal state to the inner editors.
    /// </summary>
    private void ApplyRevealState()
    {
        if (this.revealedTextBox is null)
        {
            return;
        }

        if (this.IsPasswordRevealed)
        {
            this.SynchronizeRevealedTextBoxFromPasswordBox();
            return;
        }

        this.revealedTextBox.Text = string.Empty;
    }

    /// <summary>
    /// Synchronizes the revealed text box from the inner password box.
    /// </summary>
    private void SynchronizeRevealedTextBoxFromPasswordBox()
    {
        if (this.passwordBox is null || this.revealedTextBox is null)
        {
            return;
        }

        try
        {
            this.isSynchronizingEditors = true;
            this.revealedTextBox.Text = this.passwordBox.Password;
        }
        finally
        {
            this.isSynchronizingEditors = false;
        }
    }

    /// <summary>
    /// Synchronizes the inner password box from the revealed text box.
    /// </summary>
    private void SynchronizePasswordBoxFromRevealedTextBox()
    {
        if (this.passwordBox is null || this.revealedTextBox is null)
        {
            return;
        }

        try
        {
            this.isSynchronizingEditors = true;
            this.passwordBox.Password = this.revealedTextBox.Text;
        }
        finally
        {
            this.isSynchronizingEditors = false;
        }
    }

    /// <summary>
    /// Handles changes to the inner password box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.isSynchronizingEditors)
        {
            return;
        }

        if (this.IsPasswordRevealed)
        {
            this.SynchronizeRevealedTextBoxFromPasswordBox();
        }

        this.UpdatePasswordState();
        this.UpdateRevealButtonVisibility();
    }

    /// <summary>
    /// Handles changes to the revealed text box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnRevealedTextBoxTextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.isSynchronizingEditors || !this.IsPasswordRevealed)
        {
            return;
        }

        this.SynchronizePasswordBoxFromRevealedTextBox();
        this.UpdatePasswordState();
        this.UpdateRevealButtonVisibility();
    }

    /// <summary>
    /// Toggles the reveal state.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event data.</param>
    private void OnRevealButtonClick(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled || this.PasswordLength == 0)
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
        if (dependencyObject is XSecurePasswordBox passwordBox)
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
        if (dependencyObject is XSecurePasswordBox passwordBox)
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
        if (dependencyObject is XSecurePasswordBox passwordBox)
        {
            passwordBox.ApplyCurrentIconSizes();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IsPasswordRevealed"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIsPasswordRevealedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XSecurePasswordBox passwordBox)
        {
            passwordBox.ApplyRevealState();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="HasRevealButton"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnHasRevealButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XSecurePasswordBox passwordBox)
        {
            passwordBox.UpdateRevealButtonVisibility();
        }
    }

    #endregion
}
#endregion
