// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XComboBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XComboBox ###
/// <summary>
/// Represents the standard combo box control of VIA.WPF.
/// </summary>
[TemplatePart(Name = ResetButtonPartName, Type = typeof(Button))]
public class XComboBox : ComboBox
{
    #region ### Constants ###
    /// <summary>
    /// The name of the reset button template part.
    /// </summary>
    private const string ResetButtonPartName = "PART_ResetButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(new CornerRadius(8d)));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MaxVisibleItems"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxVisibleItemsProperty = DependencyProperty.Register(
        nameof(MaxVisibleItems),
        typeof(int),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(10, OnMaxVisibleItemsChanged, CoerceMaxVisibleItems));

    /// <summary>
    /// Identifies the <see cref="EmptyText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EmptyTextProperty = DependencyProperty.Register(
        nameof(EmptyText),
        typeof(string),
        typeof(XComboBox),
        new FrameworkPropertyMetadata("No entries"));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(false, OnShowResetButtonChanged));

    /// <summary>
    /// Identifies the preferred <see cref="CanClearSelection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CanClearSelectionProperty = ShowResetButtonProperty;

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
        typeof(XComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconTemplateProperty = DependencyProperty.Register(
        nameof(ResetIconTemplate),
        typeof(DataTemplate),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconSizeProperty = DependencyProperty.Register(
        nameof(ResetIconSize),
        typeof(double),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetButtonForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonForegroundProperty = DependencyProperty.Register(
        nameof(ResetButtonForeground),
        typeof(Brush),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(Brushes.Gray));



    /// <summary>
    /// Identifies the <see cref="ResetCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register(
        nameof(ResetCommand),
        typeof(ICommand),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(null, OnResetCommandChanged));

    /// <summary>
    /// Identifies the <see cref="ResetCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register(
        nameof(ResetCommandParameter),
        typeof(object),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey HasSelectionPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasSelection),
        typeof(bool),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasSelection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSelectionProperty = HasSelectionPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResetButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResetButtonVisibility),
        typeof(Visibility),
        typeof(XComboBox),
        new FrameworkPropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// Identifies the read-only <see cref="ResetButtonVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonVisibilityProperty = ResetButtonVisibilityPropertyKey.DependencyProperty;
    #endregion

    #region ### Fields ###
    /// <summary>
    /// The command used by the reset button inside the control template.
    /// </summary>
    private readonly ICommand clearSelectionCommand;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XComboBox"/> class.
    /// </summary>
    public XComboBox()
    {
        this.clearSelectionCommand = new XComboBoxClearSelectionCommand(this);
        this.UpdateHasSelection();
        this.UpdateMaxDropDownHeight();
    }

    /// <summary>
    /// Initializes static members of the <see cref="XComboBox"/> class.
    /// </summary>
    static XComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XComboBox),
            new FrameworkPropertyMetadata(typeof(XComboBox)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius of the combo box.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the combo box.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
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

    /// <summary>
    /// Gets or sets the maximum number of items visible before the popup scrolls.
    /// </summary>
    public int MaxVisibleItems
    {
        get => (int)this.GetValue(MaxVisibleItemsProperty);
        set => this.SetValue(MaxVisibleItemsProperty, value);
    }

    /// <summary>
    /// Gets or sets the message displayed when the combo box has no items.
    /// </summary>
    public string EmptyText
    {
        get => (string)this.GetValue(EmptyTextProperty);
        set => this.SetValue(EmptyTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current selection can be cleared.
    /// </summary>
    public bool CanClearSelection
    {
        get => (bool)this.GetValue(CanClearSelectionProperty);
        set => this.SetValue(CanClearSelectionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the reset button is shown when a selection exists.
    /// </summary>
    public bool ShowResetButton
    {
        get => (bool)this.GetValue(ShowResetButtonProperty);
        set => this.SetValue(ShowResetButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the clear button is shown when a selection exists.
    /// This property is kept for compatibility and maps to <see cref="ShowResetButton"/>.
    /// </summary>
    public bool HasClearButton
    {
        get => this.ShowResetButton;
        set => this.ShowResetButton = value;
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
    /// Gets or sets the reset icon size.
    /// </summary>
    public double ResetIconSize
    {
        get => (double)this.GetValue(ResetIconSizeProperty);
        set => this.SetValue(ResetIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the reset button foreground brush.
    /// </summary>
    public Brush? ResetButtonForeground
    {
        get => (Brush?)this.GetValue(ResetButtonForegroundProperty);
        set => this.SetValue(ResetButtonForegroundProperty, value);
    }



    /// <summary>
    /// Gets or sets the optional command executed when the reset button is clicked.
    /// </summary>
    public ICommand? ResetCommand
    {
        get => (ICommand?)this.GetValue(ResetCommandProperty);
        set => this.SetValue(ResetCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional reset command parameter.
    /// </summary>
    public object? ResetCommandParameter
    {
        get => this.GetValue(ResetCommandParameterProperty);
        set => this.SetValue(ResetCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the combo box currently has a selection.
    /// </summary>
    public bool HasSelection
    {
        get => (bool)this.GetValue(HasSelectionProperty);
        private set => this.SetValue(HasSelectionPropertyKey, value);
    }

    /// <summary>
    /// Gets the internal command used by the reset button.
    /// </summary>
    public ICommand ClearSelectionCommand => this.clearSelectionCommand;

    /// <summary>
    /// Gets the effective reset button visibility.
    /// </summary>
    public Visibility ResetButtonVisibility
    {
        get => (Visibility)this.GetValue(ResetButtonVisibilityProperty);
        private set => this.SetValue(ResetButtonVisibilityPropertyKey, value);
    }

    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Clears the selected item or delegates to the configured reset command.
    /// </summary>
    private void ClearSelection()
    {
        ICommand? resetCommand = this.ResetCommand;
        object? resetCommandParameter = this.ResetCommandParameter;

        if (resetCommand is not null)
        {
            if (resetCommand.CanExecute(resetCommandParameter))
            {
                resetCommand.Execute(resetCommandParameter);
            }

            return;
        }

        this.SetCurrentValue(SelectedIndexProperty, -1);
        this.SetCurrentValue(SelectedItemProperty, null);
        this.SetCurrentValue(SelectedValueProperty, null);
        this.SetCurrentValue(TextProperty, string.Empty);
        this.UpdateHasSelection();
    }

    /// <summary>
    /// Updates the read-only <see cref="HasSelection"/> property.
    /// </summary>
    private void UpdateHasSelection()
    {
        this.HasSelection = this.SelectedIndex >= 0 || this.SelectedItem is not null;
        this.UpdateResetButtonVisibility();
    }

    /// <summary>
    /// Updates the effective reset button visibility.
    /// </summary>
    private void UpdateResetButtonVisibility()
    {
        this.ResetButtonVisibility = this.ShowResetButton && this.HasSelection && this.IsEnabled && !this.IsReadOnly
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>
    /// Updates the popup height from the semantic control density and visible item count.
    /// </summary>
    private void UpdateMaxDropDownHeight()
    {
        double itemHeight = XControlSizeMetrics.GetHeight(this.Size);
        double desiredHeight = (this.MaxVisibleItems * itemHeight) + 8d;
        double workAreaLimit = Math.Max(itemHeight, SystemParameters.WorkArea.Height - 64d);

        this.SetCurrentValue(MaxDropDownHeightProperty, Math.Min(desiredHeight, workAreaLimit));
    }

    private static object CoerceMaxVisibleItems(DependencyObject dependencyObject, object baseValue)
    {
        return Math.Clamp((int)baseValue, 1, 100);
    }

    private static void OnMaxVisibleItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XComboBox comboBox)
        {
            comboBox.UpdateMaxDropDownHeight();
        }
    }

    /// <summary>
    /// Handles changes of <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnShowResetButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XComboBox comboBox)
        {
            comboBox.UpdateResetButtonVisibility();
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

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);
        this.UpdateHasSelection();
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

        if (e.Property == SizeProperty)
        {
            this.UpdateMaxDropDownHeight();
        }
    }

    /// <summary>
    /// Determines whether the specified item is, or is eligible to be, its own item container.
    /// </summary>
    /// <param name="item">The item to evaluate.</param>
    /// <returns><see langword="true"/> if the item is its own container; otherwise, <see langword="false"/>.</returns>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is XComboBoxItem;
    }

    /// <summary>
    /// Creates or identifies the element used to display the specified item.
    /// </summary>
    /// <returns>A new <see cref="XComboBoxItem"/> instance.</returns>
    protected override DependencyObject GetContainerForItemOverride()
    {
        return new XComboBoxItem();
    }
    #endregion

    #region ### Class XComboBoxClearSelectionCommand ###
    /// <summary>
    /// Provides the internal reset command for <see cref="XComboBox"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XComboBoxClearSelectionCommand"/> class.
    /// </remarks>
    /// <param name="owner">The owning combo box.</param>
    private sealed class XComboBoxClearSelectionCommand(XComboBox owner) : ICommand
    {
        #region ### Private Fields ###
        private readonly XComboBox owner = owner;
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

            return this.owner.HasSelection;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.owner.ClearSelection();
        }
        #endregion
    }
    #endregion
}
#endregion
