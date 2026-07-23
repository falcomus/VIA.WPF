// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTextBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XTextBox ###
/// <summary>
/// Represents the standard text input control of VIA.WPF.
/// </summary>
[TemplatePart(Name = ResetButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = ClearButtonPartName, Type = typeof(Button))]
public class XTextBox : TextBox
{
    #region ### Constants ###
    /// <summary>
    /// The name of the reset button template part.
    /// </summary>
    private const string ResetButtonPartName = "PART_ResetButton";

    /// <summary>
    /// The legacy name of the clear button template part.
    /// </summary>
    private const string ClearButtonPartName = "PART_ClearButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(new CornerRadius(8d)));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="LeadingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconProperty = DependencyProperty.Register(
        nameof(LeadingIcon),
        typeof(object),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="LeadingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconTemplateProperty = DependencyProperty.Register(
        nameof(LeadingIconTemplate),
        typeof(DataTemplate),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="LeadingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LeadingIconSizeProperty = DependencyProperty.Register(
        nameof(LeadingIconSize),
        typeof(double),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="TrailingIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
        nameof(TrailingIcon),
        typeof(object),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconTemplateProperty = DependencyProperty.Register(
        nameof(TrailingIconTemplate),
        typeof(DataTemplate),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="TrailingIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailingIconSizeProperty = DependencyProperty.Register(
        nameof(TrailingIconSize),
        typeof(double),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(false, OnShowResetButtonChanged));

    /// <summary>
    /// Identifies the <see cref="HasClearButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasClearButtonProperty = ShowResetButtonProperty;

    /// <summary>
    /// Identifies the <see cref="ResetIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconProperty = DependencyProperty.Register(
        nameof(ResetIcon),
        typeof(object),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconTemplateProperty = DependencyProperty.Register(
        nameof(ResetIconTemplate),
        typeof(DataTemplate),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconSizeProperty = DependencyProperty.Register(
        nameof(ResetIconSize),
        typeof(double),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetButtonForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonForegroundProperty = DependencyProperty.Register(
        nameof(ResetButtonForeground),
        typeof(Brush),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(Brushes.Gray));



    /// <summary>
    /// Identifies the <see cref="ResetCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register(
        nameof(ResetCommand),
        typeof(ICommand),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null, OnResetCommandChanged));

    /// <summary>
    /// Identifies the <see cref="ResetCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register(
        nameof(ResetCommandParameter),
        typeof(object),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasText),
        typeof(bool),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResetButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResetButtonVisibility),
        typeof(Visibility),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// Identifies the read-only <see cref="ResetButtonVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonVisibilityProperty = ResetButtonVisibilityPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XTextBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The command used by the default reset button.
    /// </summary>
    private readonly ICommand clearTextCommand;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTextBox"/> class.
    /// </summary>
    static XTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTextBox),
            new FrameworkPropertyMetadata(typeof(XTextBox)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XTextBox"/> class.
    /// </summary>
    public XTextBox()
    {
        this.clearTextCommand = new XTextBoxClearTextCommand(this);
        this.UpdateHasText();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius of the text box.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the text box.
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
    /// Gets or sets the template for the leading icon.
    /// </summary>
    public DataTemplate? LeadingIconTemplate
    {
        get => (DataTemplate?)this.GetValue(LeadingIconTemplateProperty);
        set => this.SetValue(LeadingIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the leading icon size.
    /// </summary>
    public double LeadingIconSize
    {
        get => (double)this.GetValue(LeadingIconSizeProperty);
        set => this.SetValue(LeadingIconSizeProperty, value);
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
    /// Gets or sets the template for the trailing icon.
    /// </summary>
    public DataTemplate? TrailingIconTemplate
    {
        get => (DataTemplate?)this.GetValue(TrailingIconTemplateProperty);
        set => this.SetValue(TrailingIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the trailing icon size.
    /// </summary>
    public double TrailingIconSize
    {
        get => (double)this.GetValue(TrailingIconSizeProperty);
        set => this.SetValue(TrailingIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a reset button is shown when text is present.
    /// </summary>
    public bool ShowResetButton
    {
        get => (bool)this.GetValue(ShowResetButtonProperty);
        set => this.SetValue(ShowResetButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a clear button is shown when text is present.
    /// This property is kept for compatibility and maps to <see cref="ShowResetButton"/>.
    /// </summary>
    public bool HasClearButton
    {
        get => this.ShowResetButton;
        set => this.ShowResetButton = value;
    }

    /// <summary>
    /// Gets or sets the reset icon content.
    /// </summary>
    public object? ResetIcon
    {
        get => this.GetValue(ResetIconProperty);
        set => this.SetValue(ResetIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the reset icon.
    /// </summary>
    public DataTemplate? ResetIconTemplate
    {
        get => (DataTemplate?)this.GetValue(ResetIconTemplateProperty);
        set => this.SetValue(ResetIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the reset icon size.
    /// </summary>
    public double ResetIconSize
    {
        get => (double)this.GetValue(ResetIconSizeProperty);
        set => this.SetValue(ResetIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground brush of the reset button.
    /// </summary>
    public Brush? ResetButtonForeground
    {
        get => (Brush?)this.GetValue(ResetButtonForegroundProperty);
        set => this.SetValue(ResetButtonForegroundProperty, value);
    }



    /// <summary>
    /// Gets or sets the command executed when the reset button is clicked.
    /// If no command is configured, the text box clears its own text.
    /// </summary>
    public ICommand? ResetCommand
    {
        get => (ICommand?)this.GetValue(ResetCommandProperty);
        set => this.SetValue(ResetCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the reset command parameter.
    /// </summary>
    public object? ResetCommandParameter
    {
        get => this.GetValue(ResetCommandParameterProperty);
        set => this.SetValue(ResetCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets the internal command used by the reset button.
    /// </summary>
    public ICommand ClearTextCommand => this.clearTextCommand;

    /// <summary>
    /// Gets a value indicating whether the text box contains text.
    /// </summary>
    public bool HasText
    {
        get => (bool)this.GetValue(HasTextProperty);
        private set => this.SetValue(HasTextPropertyKey, value);
    }

    /// <summary>
    /// Gets the effective reset button visibility.
    /// </summary>
    public Visibility ResetButtonVisibility
    {
        get => (Visibility)this.GetValue(ResetButtonVisibilityProperty);
        private set => this.SetValue(ResetButtonVisibilityPropertyKey, value);
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
    /// Gets or sets a value indicating whether inline validation hints may use up to two lines.
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
        this.UpdateHasText();
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);
        this.UpdateHasText();
        CommandManager.InvalidateRequerySuggested();
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == IsEnabledProperty || e.Property == IsReadOnlyProperty)
        {
            this.UpdateResetButtonVisibility();
        }
    }
    #endregion

    #region ### Private Static Methods ###
    /// <summary>
    /// Handles changes of <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnShowResetButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTextBox textBox)
        {
            textBox.UpdateResetButtonVisibility();
        }
    }

    /// <summary>
    /// Handles changes of <see cref="ResetCommand"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnResetCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        CommandManager.InvalidateRequerySuggested();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Clears the current text or delegates to the configured reset command.
    /// </summary>
    private void ClearText()
    {
        if (!this.IsEnabled || this.IsReadOnly)
        {
            return;
        }

        ICommand? resetCommand = this.ResetCommand;
        object? resetCommandParameter = this.ResetCommandParameter;

        if (resetCommand is not null)
        {
            if (resetCommand.CanExecute(resetCommandParameter))
            {
                resetCommand.Execute(resetCommandParameter);
            }

            this.Focus();
            this.CaretIndex = this.Text.Length;
            return;
        }

        this.SetCurrentValue(TextProperty, string.Empty);
        this.Focus();
        this.CaretIndex = this.Text.Length;
        this.UpdateHasText();
    }

    /// <summary>
    /// Updates the read-only <see cref="HasText"/> property.
    /// </summary>
    private void UpdateHasText()
    {
        this.HasText = !string.IsNullOrEmpty(this.Text);
        this.UpdateResetButtonVisibility();
    }

    /// <summary>
    /// Updates the effective reset button visibility.
    /// </summary>
    private void UpdateResetButtonVisibility()
    {
        this.ResetButtonVisibility = this.ShowResetButton && this.HasText && this.IsEnabled && !this.IsReadOnly
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
    #endregion

    #region ### Class XTextBoxClearTextCommand ###
    /// <summary>
    /// Provides the internal reset command for <see cref="XTextBox"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XTextBoxClearTextCommand"/> class.
    /// </remarks>
    /// <param name="owner">The owning text box.</param>
    private sealed class XTextBoxClearTextCommand(XTextBox owner) : ICommand
    {
        #region ### Private Fields ###
        private readonly XTextBox owner = owner;
        #endregion

        #region ### Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            if (!this.owner.IsEnabled || this.owner.IsReadOnly)
            {
                return false;
            }

            if (this.owner.ResetCommand is { } resetCommand)
            {
                return resetCommand.CanExecute(this.owner.ResetCommandParameter);
            }

            return this.owner.HasText;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.owner.ClearText();
        }
        #endregion
    }
    #endregion
}
#endregion
