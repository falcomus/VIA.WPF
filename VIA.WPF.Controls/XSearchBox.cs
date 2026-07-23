// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSearchBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XSearchBox ###
/// <summary>
/// Represents a themed search input control with a leading search icon and an optional reset button.
/// </summary>
[TemplatePart(Name = SearchTextBoxPartName, Type = typeof(XTextBox))]
[TemplatePart(Name = ResetButtonPartName, Type = typeof(XIconButton))]
public class XSearchBox : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the inner search text box template part.
    /// </summary>
    private const string SearchTextBoxPartName = "PART_SearchTextBox";

    /// <summary>
    /// The name of the reset button template part.
    /// </summary>
    private const string ResetButtonPartName = "PART_ResetButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Text"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata("Search"));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="SearchIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchIconProperty = DependencyProperty.Register(
        nameof(SearchIcon),
        typeof(object),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SearchIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchIconTemplateProperty = DependencyProperty.Register(
        nameof(SearchIconTemplate),
        typeof(DataTemplate),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SearchIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchIconSizeProperty = DependencyProperty.Register(
        nameof(SearchIconSize),
        typeof(double),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconProperty = DependencyProperty.Register(
        nameof(ResetIcon),
        typeof(object),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconTemplateProperty = DependencyProperty.Register(
        nameof(ResetIconTemplate),
        typeof(DataTemplate),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconSizeProperty = DependencyProperty.Register(
        nameof(ResetIconSize),
        typeof(double),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetButtonForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonForegroundProperty = DependencyProperty.Register(
        nameof(ResetButtonForeground),
        typeof(Brush),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(Brushes.Gray));



    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(true, OnShowResetButtonChanged));

    /// <summary>
    /// Identifies the <see cref="HasClearButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasClearButtonProperty = ShowResetButtonProperty;

    /// <summary>
    /// Identifies the <see cref="ResetSearchCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetSearchCommandProperty = DependencyProperty.Register(
        nameof(ResetSearchCommand),
        typeof(ICommand),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null, OnResetSearchCommandChanged));

    /// <summary>
    /// Identifies the <see cref="ResetSearchCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetSearchCommandParameterProperty = DependencyProperty.Register(
        nameof(ResetSearchCommandParameter),
        typeof(object),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasText),
        typeof(bool),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HasText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResetButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResetButtonVisibility),
        typeof(Visibility),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// Identifies the <see cref="ResetButtonVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonVisibilityProperty = ResetButtonVisibilityPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="IsReadOnly"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XSearchBox),
        new FrameworkPropertyMetadata(XControlSize.Medium, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The command used by the reset button inside the control template.
    /// </summary>
    private readonly ICommand clearSearchCommand;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XSearchBox"/> class.
    /// </summary>
    public XSearchBox()
    {
        this.clearSearchCommand = new XSearchBoxClearCommand(this);
        this.UpdateHasText();
    }

    /// <summary>
    /// Initializes static members of the <see cref="XSearchBox"/> class.
    /// </summary>
    static XSearchBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XSearchBox),
            new FrameworkPropertyMetadata(typeof(XSearchBox)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the current search text.
    /// </summary>
    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
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
    /// Gets or sets the optional header text displayed above the search box.
    /// </summary>
    public string Header
    {
        get => (string)this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional description text displayed below the search box.
    /// </summary>
    public string Description
    {
        get => (string)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the leading search icon.
    /// </summary>
    public object? SearchIcon
    {
        get => this.GetValue(SearchIconProperty);
        set => this.SetValue(SearchIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the leading search icon.
    /// </summary>
    public DataTemplate? SearchIconTemplate
    {
        get => (DataTemplate?)this.GetValue(SearchIconTemplateProperty);
        set => this.SetValue(SearchIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the leading search icon.
    /// </summary>
    public double SearchIconSize
    {
        get => (double)this.GetValue(SearchIconSizeProperty);
        set => this.SetValue(SearchIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the reset icon.
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
    /// Gets or sets the size of the reset icon.
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
    /// Gets or sets a value indicating whether the reset button is shown when text is present.
    /// </summary>
    public bool ShowResetButton
    {
        get => (bool)this.GetValue(ShowResetButtonProperty);
        set => this.SetValue(ShowResetButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the clear button is shown when text is present.
    /// This property is kept for compatibility and maps to <see cref="ShowResetButton"/>.
    /// </summary>
    public bool HasClearButton
    {
        get => this.ShowResetButton;
        set => this.ShowResetButton = value;
    }

    /// <summary>
    /// Gets or sets the command executed before the search text is reset.
    /// </summary>
    public ICommand? ResetSearchCommand
    {
        get => (ICommand?)this.GetValue(ResetSearchCommandProperty);
        set => this.SetValue(ResetSearchCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the reset command parameter.
    /// </summary>
    public object? ResetSearchCommandParameter
    {
        get => this.GetValue(ResetSearchCommandParameterProperty);
        set => this.SetValue(ResetSearchCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the search text is not empty.
    /// </summary>
    public bool HasText
    {
        get => (bool)this.GetValue(HasTextProperty);
        private set => this.SetValue(HasTextPropertyKey, value);
    }

    /// <summary>
    /// Gets the internal command used by the reset button.
    /// </summary>
    public ICommand ClearSearchCommand => this.clearSearchCommand;

    /// <summary>
    /// Gets the effective visibility of the reset button.
    /// </summary>
    public Visibility ResetButtonVisibility
    {
        get => (Visibility)this.GetValue(ResetButtonVisibilityProperty);
        private set => this.SetValue(ResetButtonVisibilityPropertyKey, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the search box is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)this.GetValue(IsReadOnlyProperty);
        set => this.SetValue(IsReadOnlyProperty, value);
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
    /// Gets or sets the size of the search box — Small, Medium or Large.
    /// The value is forwarded to the inner <see cref="XTextBox"/> via the control template.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Clears the search text or delegates to the configured reset command.
    /// </summary>
    private void ClearSearch()
    {
        object? parameter = this.ResetSearchCommandParameter;
        ICommand? resetSearchCommand = this.ResetSearchCommand;

        if (resetSearchCommand is not null && resetSearchCommand.CanExecute(parameter))
        {
            resetSearchCommand.Execute(parameter);
            return;
        }

        this.SetCurrentValue(TextProperty, string.Empty);
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
        this.ResetButtonVisibility = this.ShowResetButton && this.HasText
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>
    /// Handles changes to <see cref="Text"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XSearchBox searchBox)
        {
            searchBox.UpdateHasText();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="ResetSearchCommand"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnResetSearchCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        CommandManager.InvalidateRequerySuggested();
    }

    /// <summary>
    /// Handles changes to <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnShowResetButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XSearchBox searchBox)
        {
            searchBox.UpdateResetButtonVisibility();
        }
    }
    #endregion

    #region ### Class XSearchBoxClearCommand ###
    /// <summary>
    /// Provides the internal reset command for <see cref="XSearchBox"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XSearchBoxClearCommand"/> class.
    /// </remarks>
    /// <param name="owner">The owning search box.</param>
    private sealed class XSearchBoxClearCommand(XSearchBox owner) : ICommand
    {
        #region ### Private Fields ###
        /// <summary>
        /// The owning search box.
        /// </summary>
        private readonly XSearchBox owner = owner;

        #endregion
        #region ### Constructors ###
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
            if (!this.owner.IsEnabled)
            {
                return false;
            }

            if (this.owner.ResetSearchCommand is { } resetSearchCommand)
            {
                return resetSearchCommand.CanExecute(this.owner.ResetSearchCommandParameter);
            }

            return this.owner.HasText;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.owner.ClearSearch();
        }
        #endregion
    }
    #endregion
}
#endregion
