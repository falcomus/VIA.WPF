// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VIA.WPF.Demo.Models;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class MainWindowViewModel ###
/// <summary>
/// Represents the main showcase shell view model.
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    #region ### Private Fields ###
    private readonly List<DemoGroup> _allDemoGroups = [];
    private bool _hasNavigationMatches = true;
    private bool _isUpdatingSelection;
    private string _navigationSearchText = string.Empty;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
        DemoGroup startGroup = new()
        {
            Title = "Start"
        };

        DemoGroup windowingGroup = new()
        {
            Title = "Windowing"
        };

        DemoGroup designSystemGroup = new()
        {
            Title = "Design System"
        };

        DemoGroup layoutGroup = new()
        {
            Title = "Layout"
        };

        DemoGroup inputGroup = new()
        {
            Title = "Input"
        };

        DemoGroup dataAndNavigationGroup = new()
        {
            Title = "Data & Navigation"
        };

        DemoGroup displayAndFeedbackGroup = new()
        {
            Title = "Display & Feedback"
        };

        DemoGroup infrastructureGroup = new()
        {
            Title = "Infrastructure"
        };

        DemoItem welcomeItem = new()
        {
            Title = "Welcome",
            Summary = "A polished workbench dashboard built from VIA.WPF controls.",
            ViewModel = new XWelcomeDashboardViewModel()
        };

        DemoItem gettingStartedItem = new()
        {
            Title = "Getting Started",
            Summary = "Installation, setup, key concepts and package structure.",
            ViewModel = new XGettingStartedViewModel()
        };

        DemoItem overviewItem = new()
        {
            Title = "Control Gallery",
            Summary = "Every VIA.WPF control in one visual, non-interactive reference page.",
            ViewModel = new XOverviewViewModel()
        };

        DemoItem xTypographyItem = new()
        {
            Title = "Typography",
            Summary = "Design-system typography roles, font sizes and semantic text usage.",
            ViewModel = new XTypographyDemoViewModel()
        };

        DemoItem xColorSchemeItem = new()
        {
            Title = "Color Scheme",
            Summary = "Theme colors, brush roles and light/dark palette behavior.",
            ViewModel = new XColorSchemeDemoViewModel()
        };

        DemoItem xExperimentalDesignItem = new()
        {
            Title = "Visual Contract",
            Summary = "Modern Workbench layers, density, command treatment and interaction states.",
            ViewModel = new XExperimentalDesignDemoViewModel()
        };

        DemoItem xWindowItem = new()
        {
            Title = "XWindow",
            Summary = "Custom app shell with title bar, overlays, flyouts and resize support.",
            ViewModel = new XWindowDemoViewModel()
        };

        DemoItem xDialogItem = new()
        {
            Title = "XDialog",
            Summary = "Service-managed modal dialogs with owner resolution, dimming, normalized results and focus restoration.",
            ViewModel = new XDialogDemoViewModel()
        };

        DemoItem xLocalizationItem = new()
        {
            Title = "Localization",
            Summary = "Dynamic DE/EN resources, culture formatting, localized code messages and XWindow language selection.",
            ViewModel = new XLocalizationDemoViewModel()
        };

        DemoItem xBorderItem = new()
        {
            Title = "XBorder",
            Summary = "Semantic highlighted border surfaces with subtle and very subtle variants.",
            ViewModel = new XBorderDemoViewModel()
        };

        DemoItem xCheckerBoardItem = new()
        {
            Title = "XCheckerBoard",
            Summary = "Checkerboard content surface for transparency and image previews.",
            ViewModel = new XCheckerBoardDemoViewModel()
        };

        DemoItem xExpanderItem = new()
        {
            Title = "XExpander",
            Summary = "Modern expander with themed header, optional indicator and configurable expand direction.",
            ViewModel = new XExpanderDemoViewModel()
        };

        DemoItem xGridItem = new()
        {
            Title = "XGrid",
            Summary = "Grid with compact row and column syntax plus logical spacing support.",
            ViewModel = new XGridDemoViewModel()
        };

        DemoItem xPanelItem = new()
        {
            Title = "XGroup",
            Summary = "Standard content group with title, subtitle, arbitrary actions and footer.",
            ViewModel = new XGroupDemoViewModel()
        };

        DemoItem xSeparatorItem = new()
        {
            Title = "XSeparator",
            Summary = "Themed horizontal and vertical separators for panels and toolbars.",
            ViewModel = new XSeparatorDemoViewModel()
        };

        DemoItem xSplitViewItem = new()
        {
            Title = "XSplitView",
            Summary = "Two-pane split layout with draggable splitter, orientation support and compact GridLength sizing.",
            ViewModel = new XSplitViewDemoViewModel()
        };

        DemoItem xMasterDetailSplitViewItem = new()
        {
            Title = "XMasterDetailSplitView",
            Summary = "Master-detail layout with optional pinned detail pane, resizer, header actions and compact master columns.",
            ViewModel = new XMasterDetailSplitViewDemoViewModel()
        };

        DemoItem xStackPanelItem = new()
        {
            Title = "XStackPanel",
            Summary = "Stack panel with built-in spacing for vertical and horizontal layouts.",
            ViewModel = new XStackPanelDemoViewModel()
        };

        DemoItem xUniformGridItem = new()
        {
            Title = "XUniformGrid",
            Summary = "Uniform grid with equally sized cells, automatic layout options and built-in spacing.",
            ViewModel = new XUniformGridDemoViewModel()
        };

        DemoItem xWrapPanelItem = new()
        {
            Title = "XWrapPanel",
            Summary = "Wrap panel with built-in spacing, optional fixed item size and horizontal or vertical wrapping.",
            ViewModel = new XWrapPanelDemoViewModel()
        };

        DemoItem xButtonItem = new()
        {
            Title = "XButton",
            Summary = "Buttons with variants, appearances, icons, loading state and elevation.",
            ViewModel = new XButtonDemoViewModel()
        };

        DemoItem xButtonGroupItem = new()
        {
            Title = "XButtonGroup",
            Summary = "Segmented single-selection buttons for view modes and compact command states.",
            ViewModel = new XButtonGroupDemoViewModel()
        };

        DemoItem xCheckBoxItem = new()
        {
            Title = "XCheckBox",
            Summary = "Themed check box with shared size logic, checked states and practical settings and filter usage.",
            ViewModel = new XCheckBoxDemoViewModel()
        };

        DemoItem xCheckGroupItem = new()
        {
            Title = "XCheckGroup",
            Summary = "Grouped multi-selection options with title, descriptions and variants.",
            ViewModel = new XCheckGroupDemoViewModel()
        };

        DemoItem xComboBoxItem = new()
        {
            Title = "XComboBox",
            Summary = "Non-editable combo box with compact item syntax, corner radius and theming support.",
            ViewModel = new XComboBoxDemoViewModel()
        };

        DemoItem xDatePickerItem = new()
        {
            Title = "XDatePicker",
            Summary = "Themed date input with calendar popup, placeholder, header, description and icon support.",
            ViewModel = new XDatePickerDemoViewModel()
        };

        DemoItem xCalendarItem = new()
        {
            Title = "XCalendar",
            Summary = "Standalone themed calendar with month, year, decade and complete interaction states.",
            ViewModel = new XCalendarDemoViewModel()
        };

        DemoItem xIconButtonItem = new()
        {
            Title = "XIconButton",
            Summary = "Dedicated icon-only button with centered icon layout and square action defaults.",
            ViewModel = new XIconButtonDemoViewModel()
        };

        DemoItem xLookupComboBoxItem = new()
        {
            Title = "XLookupComboBox",
            Summary = "Searchable lookup input with empty option, sorting and variants.",
            ViewModel = new XLookupComboBoxDemoViewModel()
        };

        DemoItem xLookupTreeComboBoxItem = new()
        {
            Title = "XLookupTreeComboBox",
            Summary = "Hierarchical lookup input with selected item/value binding and expandable tree dropdown.",
            ViewModel = new XLookupTreeComboBoxDemoViewModel()
        };

        DemoItem xNumberBoxItem = new()
        {
            Title = "XNumberBox",
            Summary = "Themed numeric input with range limits, spinner buttons, formatting and shared sizing.",
            ViewModel = new XNumberBoxDemoViewModel()
        };

        DemoItem xPasswordBoxItem = new()
        {
            Title = "XPasswordBox",
            Summary = "Themed password input with reveal button, placeholder, header, description and icon support.",
            ViewModel = new XPasswordBoxDemoViewModel()
        };

        DemoItem xRadioButtonItem = new()
        {
            Title = "XRadioButton",
            Summary = "Themed radio button with shared size logic, grouped single selection and practical mode selection usage.",
            ViewModel = new XRadioButtonDemoViewModel()
        };

        DemoItem xRadioGroupItem = new()
        {
            Title = "XRadioGroup",
            Summary = "Grouped single-selection options with title, descriptions and variants.",
            ViewModel = new XRadioGroupDemoViewModel()
        };

        DemoItem xSearchBoxItem = new()
        {
            Title = "XSearchBox",
            Summary = "Search input with clear action and toolbar-friendly sizing.",
            ViewModel = new XSearchBoxDemoViewModel()
        };

        DemoItem xSecurePasswordBoxItem = new()
        {
            Title = "XSecurePasswordBox",
            Summary = "Secure password input with reveal button and password length state.",
            ViewModel = new XSecurePasswordBoxDemoViewModel()
        };

        DemoItem xSliderItem = new()
        {
            Title = "XSlider",
            Summary = "",
            ViewModel = new XSliderDemoViewModel()
        };

        DemoItem xZoomSliderItem = new()
        {
            Title = "XZoomSlider",
            Summary = "Compact zoom control for workbench canvases.",
            ViewModel = new XZoomSliderDemoViewModel()
        };

        DemoItem xColorSchemeViewItem = new()
        {
            Title = "XColorSchemeView",
            Summary = "Semantic application palette preview.",
            ViewModel = new XColorSchemeViewDemoViewModel()
        };

        DemoItem xTextBoxItem = new()
        {
            Title = "XTextBox",
            Summary = "Themed text input with placeholder, header, description, clear button and multiline support.",
            ViewModel = new XTextBoxDemoViewModel()
        };

        DemoItem xTimePickerItem = new()
        {
            Title = "XTimePicker",
            Summary = "Themed time input with popup selection, configurable minute steps, clear button and shared sizing.",
            ViewModel = new XTimePickerDemoViewModel()
        };

        DemoItem xToggleButtonItem = new()
        {
            Title = "XToggleButton",
            Summary = "Toggle button with checked and unchecked icons, shared button sizing and checked state colors.",
            ViewModel = new XToggleButtonDemoViewModel()
        };

        DemoItem xToggleDropDownItem = new()
        {
            Title = "XToggleDropDown",
            Summary = "Toggle button with integrated arrow, popup content, checked state styling and drop-down behavior.",
            ViewModel = new XToggleDropDownDemoViewModel()
        };

        DemoItem xToggleSwitchItem = new()
        {
            Title = "XToggleSwitch",
            Summary = "Themed toggle switch with shared size logic and modern on and off state presentation.",
            ViewModel = new XToggleSwitchDemoViewModel()
        };

        DemoItem xDataGridItem = new()
        {
            Title = "XDataGrid",
            Summary = "Enhanced grid with action column, filter buttons and search highlighting.",
            ViewModel = new XDataGridDemoViewModel()
        };

        DemoItem xListBoxItem = new()
        {
            Title = "XListBox",
            Summary = "Themed navigation and compact selection list control with reusable item surfaces.",
            ViewModel = new XListBoxDemoViewModel()
        };

        DemoItem xContextMenuItem = new()
        {
            Title = "XContextMenu",
            Summary = "Themed context menus with placement-target data, semantic actions, check items and submenus.",
            ViewModel = new XContextMenuDemoViewModel()
        };

        DemoItem xNavigationListItem = new()
        {
            Title = "XNavigationList",
            Summary = "Selectable navigation list with header, footer and item styling.",
            ViewModel = new XNavigationListDemoViewModel()
        };

        DemoItem xNavigationTabControlItem = new()
        {
            Title = "XNavigationTabControl",
            Summary = "Tab-based navigation header without selected content rendering.",
            ViewModel = new XNavigationTabControlDemoViewModel()
        };

        DemoItem xTabControlItem = new()
        {
            Title = "XTabControl",
            Summary = "Themed tabs with close buttons, add button and header appearances.",
            ViewModel = new XTabControlDemoViewModel()
        };

        DemoItem xToolbarPresenterItem = new()
        {
            Title = "XHeaderBar",
            Summary = "Workbench header with breadcrumb, contextual groups and arbitrary actions.",
            ViewModel = new XHeaderBarDemoViewModel()
        };

        DemoItem xTreeViewItem = new()
        {
            Title = "XTreeView",
            Summary = "Themed hierarchical tree with selected item binding and node actions.",
            ViewModel = new XTreeViewDemoViewModel()
        };

        DemoItem xRecentItemTreeItem = new()
        {
            Title = "XRecentItemTree",
            Summary = "Composite tree navigation with a separate recent-items area, pinning and hover actions.",
            ViewModel = new XRecentItemTreeDemoViewModel()
        };

        DemoItem xViewContainerItem = new()
        {
            Title = "XViewContainer",
            Summary = "Reusable list and tree host with local detail flyout and overlay.",
            ViewModel = new XViewContainerDemoViewModel()
        };

        DemoItem xBadgeItem = new()
        {
            Title = "XBadge",
            Summary = "Compact status labels with variants, appearances, sizes and elevation.",
            ViewModel = new XBadgeDemoViewModel()
        };

        DemoItem xTextBlockDisplayItem = new()
        {
            Title = "XTextBlock",
            Summary = "Semantic text block with roles, variants, sizes and multiline helper text.",
            ViewModel = new XTextBlockDemoViewModel()
        };

        DemoItem xHighlightTextBlockItem = new()
        {
            Title = "XHighlightTextBlock",
            Summary = "TextBlock with inline search match highlighting.",
            ViewModel = new XHighlightTextBlockDemoViewModel()
        };

        DemoItem xProgressBarItem = new()
        {
            Title = "XProgressBar",
            Summary = "",
            ViewModel = new XProgressBarDemoViewModel()
        };

        DemoItem xConvertersItem = new()
        {
            Title = "Converters",
            Summary = "Reusable visibility, equality, collection and layout converters for WPF bindings.",
            ViewModel = new XConvertersDemoViewModel()
        };

        DemoItem xBehaviorsItem = new()
        {
            Title = "Behaviors",
            Summary = "Attached WPF behaviors for focus, keyboard commands, select-all and double-click actions.",
            ViewModel = new XBehaviorsDemoViewModel()
        };

        DemoItem xValidationItem = new()
        {
            Title = "Validation",
            Summary = "VIA.WPF.MVVM validation with XBind, async rules, multi-field errors and summary output.",
            ViewModel = new XValidationDemoViewModel()
        };

        DemoItem xExtensionsItem = new()
        {
            Title = "Extensions & Services",
            Summary = "Helper extensions, markup extensions and lightweight UI services used across VIA.WPF.",
            ViewModel = new XExtensionsDemoViewModel()
        };

        startGroup.Items.Add(welcomeItem);
        startGroup.Items.Add(gettingStartedItem);
        startGroup.Items.Add(overviewItem);

        windowingGroup.Items.Add(xWindowItem);
        windowingGroup.Items.Add(xDialogItem);

        designSystemGroup.Items.Add(xTypographyItem);
        designSystemGroup.Items.Add(xColorSchemeItem);
        designSystemGroup.Items.Add(xExperimentalDesignItem);

        layoutGroup.Items.Add(xBorderItem);
        layoutGroup.Items.Add(xCheckerBoardItem);
        layoutGroup.Items.Add(xExpanderItem);
        layoutGroup.Items.Add(xGridItem);
        layoutGroup.Items.Add(xPanelItem);
        layoutGroup.Items.Add(xSeparatorItem);
        layoutGroup.Items.Add(xSplitViewItem);
        layoutGroup.Items.Add(xMasterDetailSplitViewItem);
        layoutGroup.Items.Add(xStackPanelItem);
        layoutGroup.Items.Add(xUniformGridItem);
        layoutGroup.Items.Add(xWrapPanelItem);

        inputGroup.Items.Add(xButtonItem);
        inputGroup.Items.Add(xButtonGroupItem);
        inputGroup.Items.Add(xCheckBoxItem);
        inputGroup.Items.Add(xCheckGroupItem);
        inputGroup.Items.Add(xComboBoxItem);
        inputGroup.Items.Add(xCalendarItem);
        inputGroup.Items.Add(xDatePickerItem);
        inputGroup.Items.Add(xIconButtonItem);
        inputGroup.Items.Add(xLookupComboBoxItem);
        inputGroup.Items.Add(xLookupTreeComboBoxItem);
        inputGroup.Items.Add(xNumberBoxItem);
        inputGroup.Items.Add(xPasswordBoxItem);
        inputGroup.Items.Add(xRadioButtonItem);
        inputGroup.Items.Add(xRadioGroupItem);
        inputGroup.Items.Add(xSearchBoxItem);
        inputGroup.Items.Add(xSecurePasswordBoxItem);
        inputGroup.Items.Add(xSliderItem);
        inputGroup.Items.Add(xZoomSliderItem);
        inputGroup.Items.Add(xColorSchemeViewItem);
        inputGroup.Items.Add(xTextBoxItem);
        inputGroup.Items.Add(xTimePickerItem);
        inputGroup.Items.Add(xToggleButtonItem);
        inputGroup.Items.Add(xToggleDropDownItem);
        inputGroup.Items.Add(xToggleSwitchItem);

        dataAndNavigationGroup.Items.Add(xDataGridItem);
        dataAndNavigationGroup.Items.Add(xListBoxItem);
        dataAndNavigationGroup.Items.Add(xContextMenuItem);
        dataAndNavigationGroup.Items.Add(xNavigationListItem);
        dataAndNavigationGroup.Items.Add(xNavigationTabControlItem);
        dataAndNavigationGroup.Items.Add(xTabControlItem);
        dataAndNavigationGroup.Items.Add(xToolbarPresenterItem);
        dataAndNavigationGroup.Items.Add(xTreeViewItem);
        dataAndNavigationGroup.Items.Add(xRecentItemTreeItem);
        dataAndNavigationGroup.Items.Add(xViewContainerItem);

        displayAndFeedbackGroup.Items.Add(xBadgeItem);
        displayAndFeedbackGroup.Items.Add(xTextBlockDisplayItem);
        displayAndFeedbackGroup.Items.Add(xHighlightTextBlockItem);
        displayAndFeedbackGroup.Items.Add(xProgressBarItem);

        infrastructureGroup.Items.Add(xLocalizationItem);
        infrastructureGroup.Items.Add(xConvertersItem);
        infrastructureGroup.Items.Add(xBehaviorsItem);
        infrastructureGroup.Items.Add(xValidationItem);
        infrastructureGroup.Items.Add(xExtensionsItem);

        this._allDemoGroups.Add(startGroup);
        this._allDemoGroups.Add(windowingGroup);
        this._allDemoGroups.Add(designSystemGroup);
        this._allDemoGroups.Add(layoutGroup);
        this._allDemoGroups.Add(inputGroup);
        this._allDemoGroups.Add(dataAndNavigationGroup);
        this._allDemoGroups.Add(displayAndFeedbackGroup);
        this._allDemoGroups.Add(infrastructureGroup);

        DemoGroup allGroup = new()
        {
            Title = "All",
            IsAll = true
        };

        foreach (DemoItem item in this._allDemoGroups.SelectMany(static group => group.Items))
        {
            allGroup.Items.Add(item);
            item.PropertyChanged += OnDemoItemPropertyChanged;
        }

        this.NavigationGroups.Add(allGroup);

        foreach (DemoGroup group in this._allDemoGroups)
        {
            this.NavigationGroups.Add(group);
        }

        this.SelectedGroup = allGroup;
        this.RefreshVisibleDemoItems();

        welcomeItem.IsSelected = true;
        this.SelectedDemo = welcomeItem;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the available navigation groups, including the leading all-controls entry.
    /// </summary>
    public ObservableCollection<DemoGroup> NavigationGroups { get; } = [];

    /// <summary>
    /// Gets the control demos visible for the selected group and search filter.
    /// </summary>
    public ObservableCollection<DemoItem> VisibleDemoItems { get; } = [];

    /// <summary>
    /// Gets the amount of visible navigation entries after filtering.
    /// </summary>
    public int NavigationMatchCount => this.VisibleDemoItems.Count;

    /// <summary>
    /// Gets the user-facing result text for the navigation filter.
    /// </summary>
    public string NavigationResultText
    {
        get
        {
            int count = this.NavigationMatchCount;

            if (string.IsNullOrWhiteSpace(this.NavigationSearchText))
            {
                return count == 1
                    ? "1 demo available"
                    : $"{count} demos available";
            }

            return count == 1
                ? "1 matching demo"
                : $"{count} matching demos";
        }
    }

    /// <summary>
    /// Gets or sets the navigation search text.
    /// </summary>
    public string NavigationSearchText
    {
        get => this._navigationSearchText;
        set
        {
            if (this.SetProperty(ref this._navigationSearchText, value))
            {
                this.RefreshVisibleDemoItems();
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the current navigation filter has at least one result.
    /// </summary>
    public bool HasNavigationMatches
    {
        get => this._hasNavigationMatches;
        private set => this.SetProperty(ref this._hasNavigationMatches, value);
    }

    /// <summary>
    /// Gets or sets the currently selected showcase item.
    /// </summary>
    [ObservableProperty]
    private DemoItem? _selectedDemo;

    /// <summary>
    /// Gets or sets the selected navigation group.
    /// </summary>
    [ObservableProperty]
    private DemoGroup? _selectedGroup;

    /// <summary>
    /// Gets or sets a value indicating whether the left flyout is open.
    /// </summary>
    [ObservableProperty]
    private bool _isLeftFlyoutOpen;

    /// <summary>
    /// Gets or sets a value indicating whether the right flyout is open.
    /// </summary>
    [ObservableProperty]
    private bool _isRightFlyoutOpen;

    /// <summary>
    /// Gets or sets a value indicating whether the top flyout is open.
    /// </summary>
    [ObservableProperty]
    private bool _isTopFlyoutOpen;

    /// <summary>
    /// Gets or sets a value indicating whether the bottom flyout is open.
    /// </summary>
    [ObservableProperty]
    private bool _isBottomFlyoutOpen;
    #endregion

    #region ### Partial Methods ###
    partial void OnSelectedDemoChanged(DemoItem? value)
    {
        if (value is null || this._isUpdatingSelection)
        {
            return;
        }

        try
        {
            this._isUpdatingSelection = true;

            foreach (DemoItem item in this._allDemoGroups.SelectMany(static group => group.Items))
            {
                item.IsSelected = ReferenceEquals(item, value);
            }
        }
        finally
        {
            this._isUpdatingSelection = false;
        }
    }

    partial void OnSelectedGroupChanged(DemoGroup? value)
    {
        if (value is not null)
        {
            this.RefreshVisibleDemoItems();
        }
    }
    #endregion

    #region ### Private Methods ###
    private void RefreshVisibleDemoItems()
    {
        string searchText = this.NavigationSearchText?.Trim() ?? string.Empty;

        IEnumerable<DemoItem> sourceItems = this.SelectedGroup?.IsAll != false
            ? this._allDemoGroups.SelectMany(static group => group.Items)
            : this.SelectedGroup.Items;

        DemoItem[] matchingItems = sourceItems
            .Where(item => MatchesNavigationFilter(item, searchText))
            .OrderBy(static item => item.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToArray();

        this.VisibleDemoItems.Clear();

        foreach (DemoItem item in matchingItems)
        {
            this.VisibleDemoItems.Add(item);
        }

        this.HasNavigationMatches = this.VisibleDemoItems.Count > 0;
        this.OnPropertyChanged(nameof(this.NavigationMatchCount));
        this.OnPropertyChanged(nameof(this.NavigationResultText));
    }

    private static bool MatchesNavigationFilter(DemoItem item, string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return true;
        }

        string searchableText = $"{item.Title} {item.Summary}";
        string[] terms = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return terms.All(term => searchableText.Contains(term, StringComparison.CurrentCultureIgnoreCase));
    }

    private void OnDemoItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DemoItem.IsSelected) || this._isUpdatingSelection || sender is not DemoItem item || !item.IsSelected)
        {
            return;
        }

        this.SelectedDemo = item;
    }

    private void CloseAllFlyouts()
    {
        this.IsLeftFlyoutOpen = false;
        this.IsRightFlyoutOpen = false;
        this.IsTopFlyoutOpen = false;
        this.IsBottomFlyoutOpen = false;
    }
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Toggles the left flyout.
    /// </summary>
    [RelayCommand]
    private void ToggleLeftFlyout()
    {
        bool shouldOpen = !this.IsLeftFlyoutOpen;
        this.CloseAllFlyouts();
        this.IsLeftFlyoutOpen = shouldOpen;
    }

    /// <summary>
    /// Toggles the right flyout.
    /// </summary>
    [RelayCommand]
    private void ToggleRightFlyout()
    {
        bool shouldOpen = !this.IsRightFlyoutOpen;
        this.CloseAllFlyouts();
        this.IsRightFlyoutOpen = shouldOpen;
    }

    /// <summary>
    /// Toggles the top flyout.
    /// </summary>
    [RelayCommand]
    private void ToggleTopFlyout()
    {
        bool shouldOpen = !this.IsTopFlyoutOpen;
        this.CloseAllFlyouts();
        this.IsTopFlyoutOpen = shouldOpen;
    }

    /// <summary>
    /// Toggles the bottom flyout.
    /// </summary>
    [RelayCommand]
    private void ToggleBottomFlyout()
    {
        bool shouldOpen = !this.IsBottomFlyoutOpen;
        this.CloseAllFlyouts();
        this.IsBottomFlyoutOpen = shouldOpen;
    }

    /// <summary>
    /// Closes all flyouts.
    /// </summary>
    [RelayCommand]
    private void CloseAllFlyoutsCommand()
    {
        this.CloseAllFlyouts();
    }
    #endregion
}
#endregion
