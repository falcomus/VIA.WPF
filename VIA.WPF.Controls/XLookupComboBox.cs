// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLookupComboBox.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XLookupComboBox ###
/// <summary>
/// Represents a themed lookup combo box that can display values from a source collection and expose a selected value.
/// </summary>
[TemplatePart(Name = ComboBoxPartName, Type = typeof(ComboBox))]
[TemplatePart(Name = ResetButtonPartName, Type = typeof(Button))]
public class XLookupComboBox : Control
{
    #region ### Constants ###
    /// <summary>
    /// The name of the inner combo box template part.
    /// </summary>
    private const string ComboBoxPartName = "PART_ComboBox";

    /// <summary>
    /// The name of the reset button template part.
    /// </summary>
    private const string ResetButtonPartName = "PART_ResetButton";
    #endregion

    #region ### Dependency Properties ###

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="HeaderTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate),
        typeof(DataTemplate),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Description"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ItemsSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

    /// <summary>
    /// Identifies the <see cref="DisplayMemberPath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedValuePath"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
        nameof(SelectedValuePath),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedItem"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedValue"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));

    /// <summary>
    /// Identifies the <see cref="SelectedText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.Register(
        nameof(SelectedText),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty, OnSelectedTextChanged));

    /// <summary>
    /// Identifies the <see cref="SearchText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
        nameof(SearchText),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty, OnSearchTextChanged));

    /// <summary>
    /// Identifies the <see cref="IsSearchEnabled"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSearchEnabledProperty = DependencyProperty.Register(
        nameof(IsSearchEnabled),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(true, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="IncludeEmptyOption"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IncludeEmptyOptionProperty = DependencyProperty.Register(
        nameof(IncludeEmptyOption),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="EmptyOptionText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EmptyOptionTextProperty = DependencyProperty.Register(
        nameof(EmptyOptionText),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata("Keine Auswahl", OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="ExcludeStrings"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExcludeStringsProperty = DependencyProperty.Register(
        nameof(ExcludeStrings),
        typeof(IEnumerable),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null, OnExcludeStringsChanged));

    /// <summary>
    /// Identifies the <see cref="IgnoreCase"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IgnoreCaseProperty = DependencyProperty.Register(
        nameof(IgnoreCase),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(true, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="IsDistinct"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDistinctProperty = DependencyProperty.Register(
        nameof(IsDistinct),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="SortDirection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(
        nameof(SortDirection),
        typeof(ListSortDirection?),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(ListSortDirection.Ascending, OnLookupConfigurationChanged));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XLookupComboBoxVariant),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(XLookupComboBoxVariant.Default));

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(new CornerRadius(8d)));

    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
        nameof(IconTemplate),
        typeof(DataTemplate),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize));

    /// <summary>
    /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="MaxDropDownHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
        nameof(MaxDropDownHeight),
        typeof(double),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(280d));

    /// <summary>
    /// Identifies the <see cref="IsEditable"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(
        nameof(IsEditable),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="AllowInsertRequest"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AllowInsertRequestProperty = DependencyProperty.Register(
        nameof(AllowInsertRequest),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="InsertRequestTrigger"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty InsertRequestTriggerProperty = DependencyProperty.Register(
        nameof(InsertRequestTrigger),
        typeof(XLookupInsertRequestTrigger),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(XLookupInsertRequestTrigger.Enter));

    /// <summary>
    /// Identifies the <see cref="RequestInsertCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RequestInsertCommandProperty = DependencyProperty.Register(
        nameof(RequestInsertCommand),
        typeof(ICommand),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="RequestInsertCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RequestInsertCommandParameterProperty = DependencyProperty.Register(
        nameof(RequestInsertCommandParameter),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey PendingInsertTextPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(PendingInsertText),
        typeof(string),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the read-only <see cref="PendingInsertText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PendingInsertTextProperty = PendingInsertTextPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the routed <see cref="RequestInsert"/> event.
    /// </summary>
    public static readonly RoutedEvent RequestInsertEvent = EventManager.RegisterRoutedEvent(
        nameof(RequestInsert),
        RoutingStrategy.Bubble,
        typeof(EventHandler<XLookupInsertRequestEventArgs>),
        typeof(XLookupComboBox));

    /// <summary>
    /// Identifies the <see cref="ShowResetButton"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowResetButtonProperty = DependencyProperty.Register(
        nameof(ShowResetButton),
        typeof(bool),
        typeof(XLookupComboBox),
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
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconTemplateProperty = DependencyProperty.Register(
        nameof(ResetIconTemplate),
        typeof(DataTemplate),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ResetIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetIconSizeProperty = DependencyProperty.Register(
        nameof(ResetIconSize),
        typeof(double),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(XControlSizeMetrics.SmallIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="ResetButtonForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonForegroundProperty = DependencyProperty.Register(
        nameof(ResetButtonForeground),
        typeof(Brush),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(Brushes.Gray));



    /// <summary>
    /// Identifies the <see cref="ResetCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register(
        nameof(ResetCommand),
        typeof(ICommand),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null, OnResetCommandChanged));

    /// <summary>
    /// Identifies the <see cref="ResetCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register(
        nameof(ResetCommandParameter),
        typeof(object),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(null));

    private static readonly DependencyPropertyKey HasSelectionPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasSelection),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the read-only <see cref="HasSelection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSelectionProperty = HasSelectionPropertyKey.DependencyProperty;

    private static readonly DependencyPropertyKey ResetButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(ResetButtonVisibility),
        typeof(Visibility),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// Identifies the read-only <see cref="ResetButtonVisibility"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResetButtonVisibilityProperty = ResetButtonVisibilityPropertyKey.DependencyProperty;

    #endregion

    #region ### Fields ###

    /// <summary>
    /// The generated lookup entries displayed by the inner combo box.
    /// </summary>
    private readonly ObservableCollection<XLookupComboBoxEntry> lookupEntries = [];

    /// <summary>
    /// The command used by the reset button inside the control template.
    /// </summary>
    private readonly ICommand clearSelectionCommand;

    /// <summary>
    /// Indicates whether selection synchronization is currently running.
    /// </summary>
    private bool isSynchronizingSelection;

    /// <summary>
    /// The currently attached source collection changed notifier.
    /// </summary>
    private INotifyCollectionChanged? itemsSourceCollectionChanged;

    /// <summary>
    /// The currently attached exclude collection changed notifier.
    /// </summary>
    private INotifyCollectionChanged? excludeStringsCollectionChanged;

    /// <summary>
    /// The inner combo box template part.
    /// </summary>
    private ComboBox? comboBox;


    /// <summary>
    /// Identifies the <see cref="HeaderFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
        nameof(HeaderFontSize),
        typeof(double),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="HeaderFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
        nameof(HeaderFontWeight),
        typeof(FontWeight),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(FontWeights.SemiBold, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="MultiLineValidationHints"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MultiLineValidationHintsProperty = DependencyProperty.Register(
        nameof(MultiLineValidationHints),
        typeof(bool),
        typeof(XLookupComboBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
    #endregion

    #region ### Constructors ###

    /// <summary>
    /// Initializes static members of the <see cref="XLookupComboBox"/> class.
    /// </summary>
    static XLookupComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XLookupComboBox),
            new FrameworkPropertyMetadata(typeof(XLookupComboBox)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XLookupComboBox"/> class.
    /// </summary>
    public XLookupComboBox()
    {
        this.clearSelectionCommand = new XLookupComboBoxClearSelectionCommand(this);
        this.LookupEntriesView = CollectionViewSource.GetDefaultView(this.lookupEntries);
        this.LookupEntriesView.Filter = this.FilterLookupEntry;
        this.UpdateHasSelection();
    }

    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when editable text does not match an existing lookup entry and an insert was requested.
    /// </summary>
    public event EventHandler<XLookupInsertRequestEventArgs> RequestInsert
    {
        add => this.AddHandler(RequestInsertEvent, value);
        remove => this.RemoveHandler(RequestInsertEvent, value);
    }
    #endregion

    #region ### Public Properties ###

    /// <summary>
    /// Gets or sets the header displayed above the lookup combo box.
    /// </summary>
    public object? Header
    {
        get => this.GetValue(HeaderProperty);
        set => this.SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the header template.
    /// </summary>
    public DataTemplate? HeaderTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderTemplateProperty);
        set => this.SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the description displayed below the lookup combo box.
    /// </summary>
    public object? Description
    {
        get => this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the placeholder displayed when no item is selected.
    /// </summary>
    public string Placeholder
    {
        get => (string)this.GetValue(PlaceholderProperty);
        set => this.SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets the source collection used to build lookup entries.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the property path used to display lookup items.
    /// </summary>
    public string DisplayMemberPath
    {
        get => (string)this.GetValue(DisplayMemberPathProperty);
        set => this.SetValue(DisplayMemberPathProperty, value);
    }

    /// <summary>
    /// Gets or sets the property path used as selected value.
    /// </summary>
    public string SelectedValuePath
    {
        get => (string)this.GetValue(SelectedValuePathProperty);
        set => this.SetValue(SelectedValuePathProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected source item.
    /// </summary>
    public object? SelectedItem
    {
        get => this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// Gets or sets the selected value.
    /// </summary>
    public object? SelectedValue
    {
        get => this.GetValue(SelectedValueProperty);
        set => this.SetValue(SelectedValueProperty, value);
    }

    /// <summary>
    /// Gets the selected display text.
    /// </summary>
    public string SelectedText
    {
        get => (string)this.GetValue(SelectedTextProperty);
        private set => this.SetValue(SelectedTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the current search text.
    /// </summary>
    public string SearchText
    {
        get => (string)this.GetValue(SearchTextProperty);
        set => this.SetValue(SearchTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the lookup dropdown contains a search box.
    /// </summary>
    public bool IsSearchEnabled
    {
        get => (bool)this.GetValue(IsSearchEnabledProperty);
        set => this.SetValue(IsSearchEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether an empty option is inserted before the lookup values.
    /// </summary>
    public bool IncludeEmptyOption
    {
        get => (bool)this.GetValue(IncludeEmptyOptionProperty);
        set => this.SetValue(IncludeEmptyOptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the text displayed for the empty option.
    /// </summary>
    public string EmptyOptionText
    {
        get => (string)this.GetValue(EmptyOptionTextProperty);
        set => this.SetValue(EmptyOptionTextProperty, value);
    }

    /// <summary>
    /// Gets or sets strings excluded from the lookup by display text.
    /// </summary>
    public IEnumerable? ExcludeStrings
    {
        get => (IEnumerable?)this.GetValue(ExcludeStringsProperty);
        set => this.SetValue(ExcludeStringsProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether display text comparison ignores case.
    /// </summary>
    public bool IgnoreCase
    {
        get => (bool)this.GetValue(IgnoreCaseProperty);
        set => this.SetValue(IgnoreCaseProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether duplicate lookup values should be removed.
    /// </summary>
    public bool IsDistinct
    {
        get => (bool)this.GetValue(IsDistinctProperty);
        set => this.SetValue(IsDistinctProperty, value);
    }

    /// <summary>
    /// Gets or sets the lookup sort direction. Use <see langword="null"/> to keep source order.
    /// </summary>
    public ListSortDirection? SortDirection
    {
        get => (ListSortDirection?)this.GetValue(SortDirectionProperty);
        set => this.SetValue(SortDirectionProperty, value);
    }

    /// <summary>
    /// Gets or sets the control size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual variant.
    /// </summary>
    public XLookupComboBoxVariant Variant
    {
        get => (XLookupComboBoxVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional icon displayed inside the lookup field.
    /// </summary>
    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional icon template.
    /// </summary>
    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the dropdown is open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => (bool)this.GetValue(IsDropDownOpenProperty);
        set => this.SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum dropdown height.
    /// </summary>
    public double MaxDropDownHeight
    {
        get => (double)this.GetValue(MaxDropDownHeightProperty);
        set => this.SetValue(MaxDropDownHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether free text can be typed into the lookup field.
    /// </summary>
    public bool IsEditable
    {
        get => (bool)this.GetValue(IsEditableProperty);
        set => this.SetValue(IsEditableProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether unmatched editable text can request a new lookup entry.
    /// </summary>
    public bool AllowInsertRequest
    {
        get => (bool)this.GetValue(AllowInsertRequestProperty);
        set => this.SetValue(AllowInsertRequestProperty, value);
    }

    /// <summary>
    /// Gets or sets when insert requests are raised for unmatched editable text.
    /// </summary>
    public XLookupInsertRequestTrigger InsertRequestTrigger
    {
        get => (XLookupInsertRequestTrigger)this.GetValue(InsertRequestTriggerProperty);
        set => this.SetValue(InsertRequestTriggerProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when unmatched editable text requests a new lookup entry.
    /// </summary>
    public ICommand? RequestInsertCommand
    {
        get => (ICommand?)this.GetValue(RequestInsertCommandProperty);
        set => this.SetValue(RequestInsertCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional command parameter for <see cref="RequestInsertCommand"/>.
    /// When omitted, an <see cref="XLookupInsertRequest"/> is used.
    /// </summary>
    public object? RequestInsertCommandParameter
    {
        get => this.GetValue(RequestInsertCommandParameterProperty);
        set => this.SetValue(RequestInsertCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets the currently typed text that can be inserted as a new lookup entry.
    /// </summary>
    public string PendingInsertText
    {
        get => (string)this.GetValue(PendingInsertTextProperty);
        private set => this.SetValue(PendingInsertTextPropertyKey, value);
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
    /// Gets a value indicating whether the lookup combo box currently has a selection.
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

    #region ### Internal Properties ###

    /// <summary>
    /// Gets the filtered lookup entries view used by the template.
    /// </summary>
    public ICollectionView LookupEntriesView { get; }

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
        if (this.comboBox is not null)
        {
            this.comboBox.SelectionChanged -= this.OnComboBoxSelectionChanged;
            this.comboBox.PreviewKeyDown -= this.OnComboBoxPreviewKeyDown;
            this.comboBox.LostKeyboardFocus -= this.OnComboBoxLostKeyboardFocus;
        }

        base.OnApplyTemplate();

        this.comboBox = this.GetTemplateChild(ComboBoxPartName) as ComboBox;

        if (this.comboBox is not null)
        {
            this.comboBox.SelectionChanged += this.OnComboBoxSelectionChanged;
            this.comboBox.PreviewKeyDown += this.OnComboBoxPreviewKeyDown;
            this.comboBox.LostKeyboardFocus += this.OnComboBoxLostKeyboardFocus;
            this.SyncComboBoxSelectionFromCurrentState();
        }
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

    #region ### Private Methods ###

    /// <summary>
    /// Clears the selected lookup item or delegates to the configured reset command.
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

        try
        {
            this.isSynchronizingSelection = true;
            this.SelectedItem = null;
            this.SelectedValue = null;
            this.SelectedText = string.Empty;
            this.SearchText = string.Empty;
            this.PendingInsertText = string.Empty;

            if (this.comboBox is not null)
            {
                this.comboBox.SelectedItem = null;
            }
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }

        this.IsDropDownOpen = false;
        this.UpdateHasSelection();
    }

    /// <summary>
    /// Updates the read-only <see cref="HasSelection"/> property.
    /// </summary>
    private void UpdateHasSelection()
    {
        this.HasSelection = this.SelectedItem is not null
            || this.SelectedValue is not null
            || !string.IsNullOrEmpty(this.SelectedText)
            || !string.IsNullOrEmpty(this.SearchText);

        this.UpdateResetButtonVisibility();
    }

    /// <summary>
    /// Updates the effective reset button visibility.
    /// </summary>
    private void UpdateResetButtonVisibility()
    {
        this.ResetButtonVisibility = this.ShowResetButton && this.HasSelection && this.IsEnabled
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>
    /// Handles source collection changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.RebuildLookupEntries();
    }

    /// <summary>
    /// Handles exclude collection changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnExcludeStringsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.RebuildLookupEntries();
    }

    /// <summary>
    /// Handles selection changes of the inner combo box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.isSynchronizingSelection || this.comboBox?.SelectedItem is not XLookupComboBoxEntry selectedEntry)
        {
            return;
        }

        try
        {
            this.isSynchronizingSelection = true;
            this.SelectedItem = selectedEntry.OriginalItem;
            this.SelectedValue = selectedEntry.Value;
            this.SelectedText = selectedEntry.DisplayText;
            this.SearchText = selectedEntry.DisplayText;
            this.PendingInsertText = string.Empty;
            this.UpdateHasSelection();
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Handles keyboard input of the inner combo box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnComboBoxPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Enter and not Key.Return)
        {
            return;
        }

        if (this.TryRequestInsert(XLookupInsertRequestTrigger.Enter))
        {
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles focus loss of the inner combo box.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnComboBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        this.Dispatcher.BeginInvoke(new Action(() =>
        {
            if (!this.IsKeyboardFocusWithin)
            {
                this.TryRequestInsert(XLookupInsertRequestTrigger.LostFocus);
            }
        }));
    }

    /// <summary>
    /// Rebuilds all lookup entries from the current source collection.
    /// </summary>
    private void RebuildLookupEntries()
    {
        object? previousSelectedValue = this.SelectedValue;
        object? previousSelectedItem = this.SelectedItem;

        this.lookupEntries.Clear();

        if (this.IncludeEmptyOption)
        {
            this.lookupEntries.Add(new XLookupComboBoxEntry(null, null, this.EmptyOptionText, true));
        }

        IEnumerable<XLookupComboBoxEntry> entries = this.CreateSourceEntries();

        if (this.IsDistinct)
        {
            entries = this.ApplyDistinct(entries);
        }

        if (this.SortDirection is not null)
        {
            entries = this.ApplySort(entries, this.SortDirection.Value);
        }

        foreach (XLookupComboBoxEntry entry in entries)
        {
            this.lookupEntries.Add(entry);
        }

        this.LookupEntriesView.Refresh();
        this.SyncSelectionAfterLookupEntriesChanged(previousSelectedValue, previousSelectedItem);
    }

    /// <summary>
    /// Creates lookup entries from the current source collection.
    /// </summary>
    /// <returns>The generated lookup entries.</returns>
    private IEnumerable<XLookupComboBoxEntry> CreateSourceEntries()
    {
        if (this.ItemsSource is null)
        {
            yield break;
        }

        HashSet<string> excludedStrings = this.CreateExcludedStringSet();

        foreach (object? item in this.ItemsSource)
        {
            if (item is null)
            {
                continue;
            }

            string displayText = Convert.ToString(this.GetPathValue(item, this.DisplayMemberPath) ?? item, CultureInfo.CurrentCulture) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(displayText))
            {
                continue;
            }

            if (excludedStrings.Contains(displayText))
            {
                continue;
            }

            object? value = string.IsNullOrWhiteSpace(this.SelectedValuePath)
                ? item
                : this.GetPathValue(item, this.SelectedValuePath);

            yield return new XLookupComboBoxEntry(item, value, displayText, false);
        }
    }

    /// <summary>
    /// Applies distinct filtering to the generated lookup entries.
    /// </summary>
    /// <param name="entries">The lookup entries.</param>
    /// <returns>The distinct lookup entries.</returns>
    private IEnumerable<XLookupComboBoxEntry> ApplyDistinct(IEnumerable<XLookupComboBoxEntry> entries)
    {
        HashSet<string> knownKeys = new(this.IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);

        foreach (XLookupComboBoxEntry entry in entries)
        {
            string key = entry.Value is not null
                ? Convert.ToString(entry.Value, CultureInfo.CurrentCulture) ?? entry.DisplayText
                : entry.DisplayText;

            if (knownKeys.Add(key))
            {
                yield return entry;
            }
        }
    }

    /// <summary>
    /// Applies sorting to lookup entries.
    /// </summary>
    /// <param name="entries">The lookup entries.</param>
    /// <param name="sortDirection">The sort direction.</param>
    /// <returns>The sorted lookup entries.</returns>
    private IEnumerable<XLookupComboBoxEntry> ApplySort(IEnumerable<XLookupComboBoxEntry> entries, ListSortDirection sortDirection)
    {
        return sortDirection == ListSortDirection.Ascending
            ? entries.OrderBy(entry => entry.DisplayText, this.IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture)
            : entries.OrderByDescending(entry => entry.DisplayText, this.IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);
    }

    /// <summary>
    /// Creates the excluded string set.
    /// </summary>
    /// <returns>The excluded string set.</returns>
    private HashSet<string> CreateExcludedStringSet()
    {
        HashSet<string> excludedStrings = new(this.IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);

        if (this.ExcludeStrings is null)
        {
            return excludedStrings;
        }

        foreach (object? excludedValue in this.ExcludeStrings)
        {
            string? text = excludedValue as string ?? Convert.ToString(excludedValue, CultureInfo.CurrentCulture);

            if (!string.IsNullOrWhiteSpace(text))
            {
                excludedStrings.Add(text);
            }
        }

        return excludedStrings;
    }

    /// <summary>
    /// Filters lookup entries by the current search text.
    /// </summary>
    /// <param name="item">The item to filter.</param>
    /// <returns><see langword="true"/> if the item should be visible; otherwise, <see langword="false"/>.</returns>
    private bool FilterLookupEntry(object item)
    {
        if (item is not XLookupComboBoxEntry entry)
        {
            return false;
        }

        if (!this.IsSearchEnabled || string.IsNullOrWhiteSpace(this.SearchText) || entry.IsEmptyItem)
        {
            return true;
        }

        StringComparison comparison = this.IgnoreCase
            ? StringComparison.CurrentCultureIgnoreCase
            : StringComparison.CurrentCulture;

        return entry.DisplayText.Contains(this.SearchText, comparison);
    }

    /// <summary>
    /// Synchronizes selection after the lookup entries changed.
    /// </summary>
    /// <param name="previousSelectedValue">The previously selected value.</param>
    /// <param name="previousSelectedItem">The previously selected item.</param>
    private void SyncSelectionAfterLookupEntriesChanged(object? previousSelectedValue, object? previousSelectedItem)
    {
        XLookupComboBoxEntry? selectedEntry = this.FindEntryByValue(previousSelectedValue) ?? this.FindEntryByItem(previousSelectedItem);

        try
        {
            this.isSynchronizingSelection = true;

            if (this.comboBox is not null)
            {
                this.comboBox.SelectedItem = selectedEntry;
            }

            this.SelectedItem = selectedEntry?.OriginalItem;
            this.SelectedValue = selectedEntry?.Value;
            this.SelectedText = selectedEntry?.DisplayText ?? string.Empty;
            this.SearchText = selectedEntry?.DisplayText ?? this.SearchText;
            this.PendingInsertText = string.Empty;
            this.UpdateHasSelection();
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Synchronizes the inner combo box selection from the current state.
    /// </summary>
    private void SyncComboBoxSelectionFromCurrentState()
    {
        XLookupComboBoxEntry? entry = this.FindEntryByValue(this.SelectedValue) ?? this.FindEntryByItem(this.SelectedItem);

        if (this.comboBox is not null)
        {
            this.comboBox.SelectedItem = entry;
        }

        if (entry is not null)
        {
            this.SelectedText = entry.DisplayText;
            this.SearchText = entry.DisplayText;
            this.PendingInsertText = string.Empty;
        }

        this.UpdateHasSelection();
    }

    /// <summary>
    /// Updates the control state from the manually typed search text.
    /// </summary>
    private void UpdateManualInputState()
    {
        if (!this.IsEditable || this.isSynchronizingSelection)
        {
            this.UpdatePendingInsertText();
            return;
        }

        string normalizedText = NormalizeInsertText(this.SearchText);
        XLookupComboBoxEntry? matchingEntry = string.IsNullOrEmpty(normalizedText)
            ? null
            : this.FindEntryByDisplayText(normalizedText);

        try
        {
            this.isSynchronizingSelection = true;

            if (matchingEntry is not null)
            {
                this.SelectedItem = matchingEntry.OriginalItem;
                this.SelectedValue = matchingEntry.Value;
                this.SelectedText = matchingEntry.DisplayText;
                this.PendingInsertText = string.Empty;

                if (this.comboBox is not null)
                {
                    this.comboBox.SelectedItem = matchingEntry;
                }
            }
            else
            {
                this.SelectedItem = null;
                this.SelectedValue = null;
                this.SelectedText = string.Empty;
                this.PendingInsertText = this.CanRequestInsertForText(normalizedText)
                    ? normalizedText
                    : string.Empty;

                if (this.comboBox is not null)
                {
                    this.comboBox.SelectedItem = null;
                }
            }

            this.UpdateHasSelection();
        }
        finally
        {
            this.isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Updates the pending insert text without changing selection.
    /// </summary>
    private void UpdatePendingInsertText()
    {
        string normalizedText = NormalizeInsertText(this.SearchText);
        this.PendingInsertText = this.CanRequestInsertForText(normalizedText)
            ? normalizedText
            : string.Empty;
    }

    /// <summary>
    /// Tries to request insertion for the current editable text.
    /// </summary>
    /// <param name="trigger">The current insert request trigger.</param>
    /// <returns><see langword="true"/> if an insert request was raised; otherwise, <see langword="false"/>.</returns>
    private bool TryRequestInsert(XLookupInsertRequestTrigger trigger)
    {
        if (!this.AllowInsertRequest || !this.IsEditable || !this.IsEnabled || (this.InsertRequestTrigger & trigger) != trigger)
        {
            return false;
        }

        string normalizedText = NormalizeInsertText(this.SearchText);
        if (!this.CanRequestInsertForText(normalizedText))
        {
            this.PendingInsertText = string.Empty;
            return false;
        }

        XLookupInsertRequest request = new(normalizedText, this);
        XLookupInsertRequestEventArgs eventArgs = new(RequestInsertEvent, this, request);
        this.RaiseEvent(eventArgs);

        ICommand? command = this.RequestInsertCommand;
        object? commandParameter = this.HasExplicitRequestInsertCommandParameter()
            ? this.RequestInsertCommandParameter
            : request;

        if (command is not null && command.CanExecute(commandParameter))
        {
            command.Execute(commandParameter);
        }

        this.IsDropDownOpen = false;
        this.UpdatePendingInsertText();
        return true;
    }

    /// <summary>
    /// Gets whether the current editable text can request insertion.
    /// </summary>
    /// <param name="text">The normalized text.</param>
    /// <returns><see langword="true"/> if insertion can be requested; otherwise, <see langword="false"/>.</returns>
    private bool CanRequestInsertForText(string text)
    {
        return this.AllowInsertRequest
            && this.IsEditable
            && !string.IsNullOrWhiteSpace(text)
            && this.FindEntryByDisplayText(text) is null;
    }

    /// <summary>
    /// Gets whether a custom request insert command parameter was explicitly assigned.
    /// </summary>
    /// <returns><see langword="true"/> when a custom parameter is assigned; otherwise, <see langword="false"/>.</returns>
    private bool HasExplicitRequestInsertCommandParameter()
    {
        return this.ReadLocalValue(RequestInsertCommandParameterProperty) != DependencyProperty.UnsetValue;
    }

    /// <summary>
    /// Finds a lookup entry by its display text.
    /// </summary>
    /// <param name="displayText">The display text.</param>
    /// <returns>The matching lookup entry, or <see langword="null"/>.</returns>
    private XLookupComboBoxEntry? FindEntryByDisplayText(string displayText)
    {
        StringComparison comparison = this.IgnoreCase
            ? StringComparison.CurrentCultureIgnoreCase
            : StringComparison.CurrentCulture;

        return this.lookupEntries.FirstOrDefault(entry =>
            !entry.IsEmptyItem &&
            string.Equals(NormalizeInsertText(entry.DisplayText), displayText, comparison));
    }

    /// <summary>
    /// Normalizes insert request text.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <returns>The normalized text.</returns>
    private static string NormalizeInsertText(string? text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? string.Empty
            : text.Trim();
    }

    /// <summary>
    /// Finds a lookup entry by value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The matching lookup entry, or <see langword="null"/>.</returns>
    private XLookupComboBoxEntry? FindEntryByValue(object? value)
    {
        return this.lookupEntries.FirstOrDefault(entry => AreValuesEqual(entry.Value, value));
    }

    /// <summary>
    /// Finds a lookup entry by source item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The matching lookup entry, or <see langword="null"/>.</returns>
    private XLookupComboBoxEntry? FindEntryByItem(object? item)
    {
        return this.lookupEntries.FirstOrDefault(entry => ReferenceEquals(entry.OriginalItem, item) || Equals(entry.OriginalItem, item));
    }

    /// <summary>
    /// Reads a path value from an object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="path">The property path.</param>
    /// <returns>The value.</returns>
    private object? GetPathValue(object source, string path)
    {
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

    /// <summary>
    /// Determines whether two selected values are equal.
    /// </summary>
    /// <param name="left">The first value.</param>
    /// <param name="right">The second value.</param>
    /// <returns><see langword="true"/> if both values are equal; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Handles changes of <see cref="ShowResetButton"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnShowResetButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XLookupComboBox lookupComboBox)
        {
            lookupComboBox.UpdateResetButtonVisibility();
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

    /// <summary>
    /// Handles selected text changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupComboBox lookupComboBox)
        {
            lookupComboBox.UpdateHasSelection();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// Handles source collection changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not XLookupComboBox lookupComboBox)
        {
            return;
        }

        if (lookupComboBox.itemsSourceCollectionChanged is not null)
        {
            lookupComboBox.itemsSourceCollectionChanged.CollectionChanged -= lookupComboBox.OnItemsSourceCollectionChanged;
        }

        lookupComboBox.itemsSourceCollectionChanged = e.NewValue as INotifyCollectionChanged;

        if (lookupComboBox.itemsSourceCollectionChanged is not null)
        {
            lookupComboBox.itemsSourceCollectionChanged.CollectionChanged += lookupComboBox.OnItemsSourceCollectionChanged;
        }

        lookupComboBox.RebuildLookupEntries();
    }

    /// <summary>
    /// Handles exclude collection changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnExcludeStringsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not XLookupComboBox lookupComboBox)
        {
            return;
        }

        if (lookupComboBox.excludeStringsCollectionChanged is not null)
        {
            lookupComboBox.excludeStringsCollectionChanged.CollectionChanged -= lookupComboBox.OnExcludeStringsCollectionChanged;
        }

        lookupComboBox.excludeStringsCollectionChanged = e.NewValue as INotifyCollectionChanged;

        if (lookupComboBox.excludeStringsCollectionChanged is not null)
        {
            lookupComboBox.excludeStringsCollectionChanged.CollectionChanged += lookupComboBox.OnExcludeStringsCollectionChanged;
        }

        lookupComboBox.RebuildLookupEntries();
    }

    /// <summary>
    /// Handles lookup configuration changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnLookupConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupComboBox lookupComboBox)
        {
            lookupComboBox.RebuildLookupEntries();
        }
    }

    /// <summary>
    /// Handles search text changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSearchTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is XLookupComboBox lookupComboBox)
        {
            lookupComboBox.LookupEntriesView.Refresh();
            lookupComboBox.UpdateManualInputState();
        }
    }

    /// <summary>
    /// Handles selected item changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not XLookupComboBox lookupComboBox || lookupComboBox.isSynchronizingSelection)
        {
            return;
        }

        XLookupComboBoxEntry? entry = lookupComboBox.FindEntryByItem(e.NewValue);

        try
        {
            lookupComboBox.isSynchronizingSelection = true;
            lookupComboBox.SelectedValue = entry?.Value;
            lookupComboBox.SelectedText = entry?.DisplayText ?? string.Empty;
            lookupComboBox.SearchText = entry?.DisplayText ?? string.Empty;
            lookupComboBox.PendingInsertText = string.Empty;
            lookupComboBox.UpdateHasSelection();

            if (lookupComboBox.comboBox is not null)
            {
                lookupComboBox.comboBox.SelectedItem = entry;
            }
        }
        finally
        {
            lookupComboBox.isSynchronizingSelection = false;
        }
    }

    /// <summary>
    /// Handles selected value changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="e">The event data.</param>
    private static void OnSelectedValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not XLookupComboBox lookupComboBox || lookupComboBox.isSynchronizingSelection)
        {
            return;
        }

        XLookupComboBoxEntry? entry = lookupComboBox.FindEntryByValue(e.NewValue);

        try
        {
            lookupComboBox.isSynchronizingSelection = true;
            lookupComboBox.SelectedItem = entry?.OriginalItem;
            lookupComboBox.SelectedText = entry?.DisplayText ?? string.Empty;
            lookupComboBox.SearchText = entry?.DisplayText ?? string.Empty;
            lookupComboBox.PendingInsertText = string.Empty;
            lookupComboBox.UpdateHasSelection();

            if (lookupComboBox.comboBox is not null)
            {
                lookupComboBox.comboBox.SelectedItem = entry;
            }
        }
        finally
        {
            lookupComboBox.isSynchronizingSelection = false;
        }
    }

    #endregion

    #region ### Class XLookupComboBoxClearSelectionCommand ###
    /// <summary>
    /// Provides the internal reset command for <see cref="XLookupComboBox"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="XLookupComboBoxClearSelectionCommand"/> class.
    /// </remarks>
    /// <param name="owner">The owning lookup combo box.</param>
    private sealed class XLookupComboBoxClearSelectionCommand(XLookupComboBox owner) : ICommand
    {
        #region ### Private Fields ###
        private readonly XLookupComboBox owner = owner;
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

    #region ### Nested Types ###

    /// <summary>
    /// Represents a generated lookup entry.
    /// </summary>
    private sealed class XLookupComboBoxEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XLookupComboBoxEntry"/> class.
        /// </summary>
        /// <param name="originalItem">The original source item.</param>
        /// <param name="value">The lookup value.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="isEmptyItem">A value indicating whether this is the empty item.</param>
        public XLookupComboBoxEntry(object? originalItem, object? value, string displayText, bool isEmptyItem)
        {
            this.OriginalItem = originalItem;
            this.Value = value;
            this.DisplayText = displayText;
            this.IsEmptyItem = isEmptyItem;
        }

        /// <summary>
        /// Gets the original source item.
        /// </summary>
        public object? OriginalItem { get; }

        /// <summary>
        /// Gets the lookup value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// Gets a value indicating whether this entry represents the empty option.
        /// </summary>
        public bool IsEmptyItem { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.DisplayText;
        }
    }

    #endregion
}
#endregion

#region ### Enum XLookupComboBoxVariant ###
/// <summary>
/// Defines visual variants for <see cref="XLookupComboBox"/>.
/// </summary>
public enum XLookupComboBoxVariant
{
    /// <summary>
    /// The default input appearance.
    /// </summary>
    Default,

    /// <summary>
    /// A filled surface appearance.
    /// </summary>
    Filled,

    /// <summary>
    /// An outlined appearance.
    /// </summary>
    Outline
}
#endregion
