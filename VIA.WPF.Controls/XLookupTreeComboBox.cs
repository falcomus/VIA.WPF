// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupTreeComboBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XLookupTreeComboBox ###
/// <summary>
/// Provides a lookup combo box that shows a hierarchical tree inside the dropdown.
/// </summary>
[TemplatePart(Name = TreePartName, Type = typeof(XTreeView))]
[TemplatePart(Name = ClearSelectionButtonPartName, Type = typeof(Button))]
[TemplatePart(Name = ResetButtonPartName, Type = typeof(Button))]
public class XLookupTreeComboBox : Control
{
    #region ### Constants ###
    private const string TreePartName = "PART_Tree";
    private const string ClearSelectionButtonPartName = "PART_ClearSelectionButton";
    private const string ResetButtonPartName = "PART_ResetButton";
    #endregion

    #region ### Dependency Properties ###
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null, OnSelectionContextChanged));

    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata("Name", OnSelectionContextChanged));

    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
        nameof(SelectedValuePath),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata("Id", OnSelectionContextChanged));

    public static readonly DependencyProperty ChildrenMemberPathProperty = DependencyProperty.Register(
        nameof(ChildrenMemberPath),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata("Children"));

    public static readonly DependencyProperty ExpandedMemberPathProperty = DependencyProperty.Register(
        nameof(ExpandedMemberPath),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata("IsExpanded"));

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));

    public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.Register(
        nameof(SelectedText),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(string.Empty, OnSelectedTextChanged));

    public static readonly DependencyProperty IncludeEmptyOptionProperty = DependencyProperty.Register(
        nameof(IncludeEmptyOption),
        typeof(bool),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(false));

    public static readonly DependencyProperty EmptyOptionTextProperty = DependencyProperty.Register(
        nameof(EmptyOptionText),
        typeof(string),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata("Keine Auswahl"));

    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XLookupComboBoxVariant),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(XLookupComboBoxVariant.Default));

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(new CornerRadius(8d)));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
        nameof(IconTemplate),
        typeof(DataTemplate),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize));

    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
        nameof(MaxDropDownHeight),
        typeof(double),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(320d));

    public static readonly DependencyProperty TreeSelectedItemProperty = DependencyProperty.Register(
        nameof(TreeSelectedItem),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null, OnTreeSelectedItemChanged));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XLookupTreeComboBox),
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
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconTemplateProperty = DependencyProperty.Register(
        nameof(ResetIconTemplate),
        typeof(DataTemplate),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconSizeProperty = DependencyProperty.Register(
        nameof(ResetIconSize),
        typeof(double),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetButtonForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonForegroundProperty = DependencyProperty.Register(
        nameof(ResetButtonForeground),
        typeof(Brush),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(Brushes.Gray));



    /// <summary>
    /// Identifies the <see cref="ResetCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register(
        nameof(ResetCommand),
        typeof(ICommand),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null, OnResetCommandChanged));

    /// <summary>
    /// Identifies the <see cref="ResetCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register(
        nameof(ResetCommandParameter),
        typeof(object),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey HasSelectionPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasSelection),
        typeof(bool),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasSelection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSelectionProperty = HasSelectionPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResetButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResetButtonVisibility),
        typeof(Visibility),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// Identifies the read-only <see cref="ResetButtonVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonVisibilityProperty = ResetButtonVisibilityPropertyKey.DependencyProperty;
    #endregion

    #region ### Fields ###
    private bool isSynchronizingSelection;
    private readonly ICommand clearSelectionCommand;
    private Button? clearSelectionButton;
    private XTreeView? treeView;

    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XLookupTreeComboBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupTreeComboBox"/> class.
    /// </summary>
    public XLookupTreeComboBox()
    {
        this.clearSelectionCommand = new XLookupTreeComboBoxClearSelectionCommand(this);
        this.UpdateHasSelection();
    }

    static XLookupTreeComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XLookupTreeComboBox),
            new FrameworkPropertyMetadata(typeof(XLookupTreeComboBox)));
    }
    #endregion

    #region ### Public Properties ###
    public object? Header
    {
        get => this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    public DataTemplate? HeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderTemplateProperty);
        set => this.SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the description displayed below the lookup tree combo box.
    /// </summary>
    public object? Description
    {
        get => this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    public string Placeholder
    {
        get => (string)this.GetValue(PlaceholderProperty);
        set => this.SetValue(PlaceholderProperty, value);
    }

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    public string DisplayMemberPath
    {
        get => (string)this.GetValue(DisplayMemberPathProperty);
        set => this.SetValue(DisplayMemberPathProperty, value);
    }

    public string SelectedValuePath
    {
        get => (string)this.GetValue(SelectedValuePathProperty);
        set => this.SetValue(SelectedValuePathProperty, value);
    }

    public string ChildrenMemberPath
    {
        get => (string)this.GetValue(ChildrenMemberPathProperty);
        set => this.SetValue(ChildrenMemberPathProperty, value);
    }

    public string ExpandedMemberPath
    {
        get => (string)this.GetValue(ExpandedMemberPathProperty);
        set => this.SetValue(ExpandedMemberPathProperty, value);
    }

    public object? SelectedItem
    {
        get => this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    public object? SelectedValue
    {
        get => this.GetValue(SelectedValueProperty);
        set => this.SetValue(SelectedValueProperty, value);
    }

    public string SelectedText
    {
        get => (string)this.GetValue(SelectedTextProperty);
        set => this.SetValue(SelectedTextProperty, value);
    }

    public bool IncludeEmptyOption
    {
        get => (bool)this.GetValue(IncludeEmptyOptionProperty);
        set => this.SetValue(IncludeEmptyOptionProperty, value);
    }

    public string EmptyOptionText
    {
        get => (string)this.GetValue(EmptyOptionTextProperty);
        set => this.SetValue(EmptyOptionTextProperty, value);
    }

    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    public XLookupComboBoxVariant Variant
    {
        get => (XLookupComboBoxVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
    }

    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    public bool IsDropDownOpen
    {
        get => (bool)this.GetValue(IsDropDownOpenProperty);
        set => this.SetValue(IsDropDownOpenProperty, value);
    }

    public double MaxDropDownHeight
    {
        get => (double)this.GetValue(MaxDropDownHeightProperty);
        set => this.SetValue(MaxDropDownHeightProperty, value);
    }

    public object? TreeSelectedItem
    {
        get => this.GetValue(TreeSelectedItemProperty);
        set => this.SetValue(TreeSelectedItemProperty, value);
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
    /// Gets a value indicating whether the lookup tree combo box currently has a selection.
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
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (this.clearSelectionButton is not null)
        {
            this.clearSelectionButton.Click -= this.OnClearSelectionButtonClick;
        }

        this.clearSelectionButton = this.GetTemplateChild(ClearSelectionButtonPartName) as Button;
        this.treeView = this.GetTemplateChild(TreePartName) as XTreeView;

        if (this.clearSelectionButton is not null)
        {
            this.clearSelectionButton.Click += this.OnClearSelectionButtonClick;
        }

        this.SynchronizeFromCurrentSelection();
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == IsEnabledProperty)
        {
            if (!this.IsEnabled)
            {
                this.IsDropDownOpen = false;
            }

            this.UpdateResetButtonVisibility();
        }
    }
    #endregion

    #region ### Private Static Methods ###
    private static void OnSelectionContextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.SynchronizeFromCurrentSelection();
        }
    }

    private static void OnSelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.OnSelectedItemChangedInternal(e.NewValue);
        }
    }

    private static void OnSelectedValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.OnSelectedValueChangedInternal(e.NewValue);
        }
    }

    private static void OnTreeSelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.OnTreeSelectedItemChangedInternal(e.NewValue);
        }
    }

    private static void OnSelectedTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.UpdateHasSelection();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private static void OnShowResetButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupTreeComboBox control)
        {
            control.UpdateResetButtonVisibility();
        }
    }

    private static void OnResetCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
    }
    #endregion

    #region ### Private Methods ###
    private void OnSelectedItemChangedInternal(object? newValue)
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;

            object? resolvedItem = newValue ?? this.FindItemByValue(this.SelectedValue);
            this.TreeSelectedItem = resolvedItem;
            this.SetCurrentValue(SelectedItemProperty, resolvedItem);
            this.SetCurrentValue(SelectedValueProperty, resolvedItem is null ? null : this.GetPathValue(resolvedItem, this.SelectedValuePath));
            this.SetCurrentValue(SelectedTextProperty, this.GetDisplayText(resolvedItem));
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    private void OnSelectedValueChangedInternal(object? newValue)
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;

            object? resolvedItem = this.FindItemByValue(newValue);
            this.TreeSelectedItem = resolvedItem;
            this.SetCurrentValue(SelectedItemProperty, resolvedItem);
            this.SetCurrentValue(SelectedTextProperty, this.GetDisplayText(resolvedItem));
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    private void OnTreeSelectedItemChangedInternal(object? newValue)
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;
            this.SetCurrentValue(SelectedItemProperty, newValue);
            this.SetCurrentValue(SelectedValueProperty, newValue is null ? null : this.GetPathValue(newValue, this.SelectedValuePath));
            this.SetCurrentValue(SelectedTextProperty, this.GetDisplayText(newValue));
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }

        if (newValue is not null)
        {
            this.IsDropDownOpen = false;
        }
    }

    private void SynchronizeFromCurrentSelection()
    {
        if (this.isSynchronizingSelection)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;

            object? resolvedItem = this.SelectedItem ?? this.FindItemByValue(this.SelectedValue);
            this.TreeSelectedItem = resolvedItem;
            this.SetCurrentValue(SelectedItemProperty, resolvedItem);
            this.SetCurrentValue(SelectedValueProperty, resolvedItem is null ? null : this.GetPathValue(resolvedItem, this.SelectedValuePath));
            this.SetCurrentValue(SelectedTextProperty, this.GetDisplayText(resolvedItem));
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

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

        try
        {
            this.isSynchronizingSelection = true;
            this.TreeSelectedItem = null;
            this.SetCurrentValue(SelectedItemProperty, null);
            this.SetCurrentValue(SelectedValueProperty, null);
            this.SetCurrentValue(SelectedTextProperty, string.Empty);
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }

        this.IsDropDownOpen = false;
        this.UpdateHasSelection();
    }

    private void UpdateHasSelection()
    {
        this.HasSelection = this.SelectedItem is not null || this.SelectedValue is not null || !string.IsNullOrEmpty(this.SelectedText);
        this.UpdateResetButtonVisibility();
    }

    private void UpdateResetButtonVisibility()
    {
        this.ResetButtonVisibility = this.ShowResetButton && this.HasSelection && this.IsEnabled
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    private void OnClearSelectionButtonClick(object sender, RoutedEventArgs eventArgs)
    {
        this.ClearSelection();
        eventArgs.Handled = true;
    }

    private string GetDisplayText(object? item)
    {
        if (item is null)
        {
            return string.Empty;
        }

        object? value = this.GetPathValue(item, this.DisplayMemberPath);
        return Convert.ToString(value ?? item, CultureInfo.CurrentCulture) ?? string.Empty;
    }

    private object? FindItemByValue(object? selectedValue)
    {
        if (this.ItemsSource is null)
        {
            return null;
        }

        foreach (object item in this.EnumerateItemsRecursive(this.ItemsSource))
        {
            object? candidateValue = this.GetPathValue(item, this.SelectedValuePath);
            if (AreValuesEqual(candidateValue, selectedValue))
            {
                return item;
            }
        }

        return null;
    }

    private IEnumerable EnumerateChildItems(object item)
    {
        object? value = this.GetPathValue(item, this.ChildrenMemberPath);
        return value as IEnumerable ?? Array.Empty<object>();
    }

    private IEnumerable<object> EnumerateItemsRecursive(IEnumerable source)
    {
        foreach (object? item in source)
        {
            if (item is null)
            {
                continue;
            }

            yield return item;

            foreach (object childItem in this.EnumerateItemsRecursive(this.EnumerateChildItems(item)))
            {
                yield return childItem;
            }
        }
    }

    private object? GetPathValue(object source, string path)
    {
        if (source is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            return source;
        }

        object? current = source;

        foreach (string propertyName in path.Split('.'))
        {
            if (current is null)
            {
                return null;
            }

            PropertyInfo? propertyInfo = current.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            current = propertyInfo?.GetValue(current);
        }

        return current;
    }

    private static bool AreValuesEqual(object? left, object? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        if (left.GetType() == right.GetType())
        {
            return left.Equals(right);
        }

        try
        {
            object? convertedRight = Convert.ChangeType(right, left.GetType(), CultureInfo.CurrentCulture);
            return left.Equals(convertedRight);
        }
        catch
        {
            return left.Equals(right);
        }
    }
    #endregion

    #region ### Class XLookupTreeComboBoxClearSelectionCommand ###
    /// <summary>
    /// Provides the internal reset command for <see cref="XLookupTreeComboBox"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XLookupTreeComboBoxClearSelectionCommand"/> class.
    /// </remarks>
    /// <param name="owner">The owning lookup tree combo box.</param>
    private sealed class XLookupTreeComboBoxClearSelectionCommand(XLookupTreeComboBox owner) : ICommand
    {
        #region ### Private Fields ###
        private readonly XLookupTreeComboBox owner = owner;
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
