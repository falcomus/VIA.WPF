// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XExperimentalDesignDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XExperimentalDesignDemoViewModel ###
/// <summary>
/// Represents the experimental design lab demo page.
/// </summary>
public sealed partial class XExperimentalDesignDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private DateTime? effectiveDate = DateTime.Today;
    private DesignExperimentNavigationSection? selectedSection;
    private DesignExperimentNavigationPage? selectedSectionPage;
    private string selectedParentCategory = "Arbeitsmittel";
    private string selectedPriority = "Normal";
    private string selectedStatus = "Aktiv";
    private DesignExperimentCategoryRow? selectedCategoryRow;
    private string statusMessage = "Bereit";
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XExperimentalDesignDemoViewModel"/> class.
    /// </summary>
    public XExperimentalDesignDemoViewModel()
    {
        DesignExperimentNavigationSection masterData = new(
            "Basisdaten",
            "Pflege von Kategorien, Artikeln, Material, Unternehmen, Fahrzeugen, Niederlassungen und Lagern.",
            [
                new("Kategorien", "Hierarchische Verwaltung der Kategorien."),
                new("Artikel", "Verwaltung der Artikelstammdaten."),
                new("Material", "Verwaltung einzelner Materialeinheiten."),
                new("Unternehmen", "Lieferanten, Werkstätten und externe Unternehmen."),
                new("Fahrzeugtypen", "Verwaltung der Fahrzeugtypen."),
                new("Fahrzeuge", "Verwaltung der Fahrzeuge."),
                new("Niederlassung", "Verwaltung der Niederlassungen."),
                new("Lager", "Verwaltung von Lagern und Lagerorten."),
            ]);

        DesignExperimentNavigationSection bookings = new(
            "Lagerbuchungen",
            "Erfassung und Bearbeitung von Warenausgang, Wareneingang und Umbuchungen.",
            [
                new("Warenausgang", "Buchung von Material aus einem Lager."),
                new("Wareneingang", "Buchung des Materialeingangs."),
                new("Umbuchung", "Bewegungen zwischen Lagerorten."),
                new("Offene Buchungen", "Noch nicht abgeschlossene Vorgänge."),
            ]);

        DesignExperimentNavigationSection articles = new(
            "Artikel",
            "Artikelübersicht mit Kategoriebaum und kompakten Detailflächen.",
            [
                new("Artikelübersicht", "Liste aller Artikel mit Status und Bestand."),
                new("Produktbündel", "Zusammenstellungen und Sets."),
                new("Kategoriebaum", "Hierarchische Artikelnavigation."),
            ]);

        DesignExperimentNavigationSection reports = new(
            "Berichte",
            "Auswertungen für Bestand, Bewegungen, Prüfungen und Historie.",
            [
                new("Bestand", "Aktuelle Bestandsübersicht."),
                new("Bewegungen", "Historie der Materialbewegungen."),
                new("Prüfungen", "Fällige und abgeschlossene Prüfungen."),
            ]);

        this.NavigationSections.Add(masterData);
        this.NavigationSections.Add(bookings);
        this.NavigationSections.Add(articles);
        this.NavigationSections.Add(reports);

        this.PageSearchContext = new DemoSearchContext(string.Empty);
        this.PageToolbar = XToolbarContext.CreateEmpty();
        this.PageToolbar.ShowNewButton = true;
        this.PageToolbar.ShowViewButton = true;
        this.PageToolbar.ShowDeleteButton = true;
        this.PageToolbar.ShowRefreshButton = true;
        this.PageToolbar.ShowViewModeSelector = true;
        this.PageToolbar.NewCommand = this.NewCommand;
        this.PageToolbar.ViewCommand = this.PreviewCommand;
        this.PageToolbar.DeleteCommand = this.DeleteCommand;
        this.PageToolbar.RefreshCommand = this.RefreshCommand;

        this.SelectedSection = masterData;
        this.SelectedCategoryRow = this.CategoryRows.FirstOrDefault();
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "Visual Contract";

    /// <inheritdoc />
    public override string Description => "Modern Workbench reference page for semantic layers, desktop density, quiet commands, data selection and field states.";

    /// <summary>
    /// Gets the toolbar context used by the page toolbar sample.
    /// </summary>
    public XToolbarContext PageToolbar { get; }

    /// <summary>
    /// Gets the search context used by the page toolbar sample.
    /// </summary>
    public IXSearchContext PageSearchContext { get; }

    /// <summary>
    /// Gets the top-level navigation sections used by the sample shell.
    /// </summary>
    public ObservableCollection<DesignExperimentNavigationSection> NavigationSections { get; } = [];

    /// <summary>
    /// Gets the side navigation pages of the selected navigation section.
    /// </summary>
    public ObservableCollection<DesignExperimentNavigationPage> SectionPages { get; } = [];

    /// <summary>
    /// Gets the parent category options used by the form sample.
    /// </summary>
    public ObservableCollection<string> ParentCategoryOptions { get; } =
    [
        "Arbeitsmittel",
        "Ausrüstung",
        "Basisdaten",
        "Fahrzeug",
        "Lager",
    ];

    /// <summary>
    /// Gets the priority options used by the state sample.
    /// </summary>
    public ObservableCollection<string> PriorityOptions { get; } =
    [
        "Niedrig",
        "Normal",
        "Hoch",
        "Kritisch",
    ];

    /// <summary>
    /// Gets the status options used by the input sample.
    /// </summary>
    public ObservableCollection<string> StatusOptions { get; } =
    [
        "Aktiv",
        "In Prüfung",
        "Gesperrt",
        "Archiviert",
    ];

    /// <summary>
    /// Gets or sets the selected top-level navigation section.
    /// </summary>
    public DesignExperimentNavigationSection? SelectedSection
    {
        get => this.selectedSection;
        set
        {
            if (!this.SetProperty(ref this.selectedSection, value))
            {
                return;
            }

            this.RebuildSectionPages();
            this.OnPropertyChanged(nameof(this.CurrentBreadcrumb));
            this.OnPropertyChanged(nameof(this.CurrentSectionDescription));
        }
    }

    /// <summary>
    /// Gets or sets the selected page in the side navigation.
    /// </summary>
    public DesignExperimentNavigationPage? SelectedSectionPage
    {
        get => this.selectedSectionPage;
        set
        {
            if (!this.SetProperty(ref this.selectedSectionPage, value))
            {
                return;
            }

            this.OnPropertyChanged(nameof(this.CurrentBreadcrumb));
            this.OnPropertyChanged(nameof(this.CurrentPageTitle));
            this.OnPropertyChanged(nameof(this.CurrentPageDescription));
        }
    }

    /// <summary>
    /// Gets the breadcrumb text of the sample shell.
    /// </summary>
    public string CurrentBreadcrumb => $"{this.SelectedSection?.Title ?? "Design"} > {this.SelectedSectionPage?.Title ?? "Übersicht"}";

    /// <summary>
    /// Gets the description of the selected navigation section.
    /// </summary>
    public string CurrentSectionDescription => this.SelectedSection?.Description ?? string.Empty;

    /// <summary>
    /// Gets the title of the selected sample page.
    /// </summary>
    public string CurrentPageTitle => this.SelectedSectionPage?.Title ?? "Kategorien";

    /// <summary>
    /// Gets the description of the selected sample page.
    /// </summary>
    public string CurrentPageDescription => this.SelectedSectionPage?.Description ?? "Hierarchische Verwaltung der Kategorien.";

    /// <summary>
    /// Gets or sets the selected parent category option.
    /// </summary>
    public string SelectedParentCategory
    {
        get => this.selectedParentCategory;
        set => this.SetProperty(ref this.selectedParentCategory, value);
    }

    /// <summary>
    /// Gets or sets the selected priority option.
    /// </summary>
    public string SelectedPriority
    {
        get => this.selectedPriority;
        set => this.SetProperty(ref this.selectedPriority, value);
    }

    /// <summary>
    /// Gets or sets the selected status option.
    /// </summary>
    public string SelectedStatus
    {
        get => this.selectedStatus;
        set => this.SetProperty(ref this.selectedStatus, value);
    }

    /// <summary>
    /// Gets or sets the effective date used by the form sample.
    /// </summary>
    public DateTime? EffectiveDate
    {
        get => this.effectiveDate;
        set => this.SetProperty(ref this.effectiveDate, value);
    }

    /// <summary>
    /// Gets or sets the selected row used by the edit form preview.
    /// </summary>
    public DesignExperimentCategoryRow? SelectedCategoryRow
    {
        get => this.selectedCategoryRow;
        set => this.SetProperty(ref this.selectedCategoryRow, value);
    }

    /// <summary>
    /// Gets or sets the status message shown in the detail card.
    /// </summary>
    public string StatusMessage
    {
        get => this.statusMessage;
        set => this.SetProperty(ref this.statusMessage, value);
    }

    /// <summary>
    /// Gets the sample rows used by the category grid preview.
    /// </summary>
    public ObservableCollection<DesignExperimentCategoryRow> CategoryRows { get; } =
    [
        new("CAT-0002", "A", "TESTCAT", "Ja", "Root category for article groups", "Heute"),
        new("CAT-0005", "A1", "A", "Ja", "First child category", "Heute"),
        new("CAT-0006", "A2", "A", "Ja", "Second child category", "Gestern"),
        new("CAT-0007", "A3", "B1", "Ja", "Assigned to branch B1", "Gestern"),
        new("ACH1", "Arbeitsmittel", "Basisdaten", "Ja", "Arbeitsmittel und Werkzeuge", "2 Tage"),
        new("ARC1", "Arbeitsmittel child 1", "Arbeitsmittel", "Ja", "Desc.", "2 Tage"),
        new("CAT-0014", "B", "", "Nein", "Inactive sample entry", "3 Tage"),
        new("LAG-010", "Lagerplatz", "Lager", "Ja", "Storage location node", "3 Tage"),
    ];

    /// <inheritdoc />
    public override string XamlCode => """
<!-- Quiet commands are integrated into the group header. -->
<via:XButton
    Appearance="Ghost"
    Content="New"
    Icon="{via:BootstrapIcon Kind=Plus}"
    Size="Small" />

<!-- New composition should use semantic roles instead of palette variants. -->
<Border
    Background="{DynamicResource {x:Static via:XBrushKeys.Surface}}"
    BorderBrush="{DynamicResource {x:Static via:XBrushKeys.BorderSubtle}}" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
// Application code consumes roles. The active theme supplies the colors.
XThemeService.Manager.ApplyTheme(XThemePresets.Default);
XThemeService.Manager.SetMode(XThemeMode.Dark);
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Adds a sample row.
    /// </summary>
    [RelayCommand]
    private void New()
    {
        DesignExperimentCategoryRow row = new("NEW-001", "Neue Kategorie", this.SelectedParentCategory, "Ja", "New local demo row", "Jetzt");
        this.CategoryRows.Insert(0, row);
        this.SelectedCategoryRow = row;
        this.StatusMessage = "Neu angelegt";
    }

    /// <summary>
    /// Marks the current state as saved.
    /// </summary>
    [RelayCommand]
    private void Save()
    {
        this.StatusMessage = "Gespeichert";
    }

    /// <summary>
    /// Marks the current state as previewed.
    /// </summary>
    [RelayCommand]
    private void Preview()
    {
        this.StatusMessage = "Vorschau aktiv";
    }

    /// <summary>
    /// Removes the selected sample row.
    /// </summary>
    [RelayCommand]
    private void Delete()
    {
        if (this.SelectedCategoryRow is null)
        {
            this.StatusMessage = "Keine Auswahl";
            return;
        }

        int index = this.CategoryRows.IndexOf(this.SelectedCategoryRow);
        this.CategoryRows.Remove(this.SelectedCategoryRow);
        this.SelectedCategoryRow = this.CategoryRows.Count == 0 ? null : this.CategoryRows[Math.Clamp(index, 0, this.CategoryRows.Count - 1)];
        this.StatusMessage = "Gelöscht";
    }

    /// <summary>
    /// Resets the transient sample state.
    /// </summary>
    [RelayCommand]
    private void Refresh()
    {
        this.SelectedCategoryRow = this.CategoryRows.FirstOrDefault();
        this.SelectedStatus = "Aktiv";
        this.SelectedPriority = "Normal";
        this.StatusMessage = "Aktualisiert";
    }
    #endregion

    #region ### Private Methods ###
    private void RebuildSectionPages()
    {
        this.SectionPages.Clear();

        if (this.SelectedSection is null)
        {
            this.SelectedSectionPage = null;
            return;
        }

        foreach (DesignExperimentNavigationPage page in this.SelectedSection.Pages)
        {
            this.SectionPages.Add(page);
        }

        this.SelectedSectionPage = this.SectionPages.FirstOrDefault();
    }
    #endregion
}
#endregion

#region ### Class DemoSearchContext ###
/// <summary>
/// Provides a minimal search context for the experimental toolbar sample.
/// </summary>
internal sealed class DemoSearchContext : ObservableObject, IXSearchContext
{
    #region ### Private Fields ###
    private string searchTerm;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="DemoSearchContext" /> class.
    /// </summary>
    /// <param name="initialSearchTerm">The initial search term.</param>
    public DemoSearchContext(string initialSearchTerm)
    {
        this.searchTerm = initialSearchTerm;
        this.ResetSearchCommand = new RelayCommand(() => this.SearchTerm = string.Empty);
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public string SearchTerm
    {
        get => this.searchTerm;
        set => this.SetProperty(ref this.searchTerm, value);
    }

    /// <inheritdoc />
    public ICommand ResetSearchCommand { get; }
    #endregion
}
#endregion

#region ### Class DesignExperimentNavigationSection ###
/// <summary>
/// Represents a top-level navigation section in the experimental sample shell.
/// </summary>
/// <param name="title">The section title.</param>
/// <param name="description">The section description.</param>
/// <param name="pages">The side-navigation pages of the section.</param>
public sealed class DesignExperimentNavigationSection(string title, string description, IEnumerable<DesignExperimentNavigationPage> pages)
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the section title.
    /// </summary>
    public string Title { get; } = title;

    /// <summary>
    /// Gets the section description.
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    /// Gets the side-navigation pages of the section.
    /// </summary>
    public IReadOnlyList<DesignExperimentNavigationPage> Pages { get; } = pages.ToArray();
    #endregion
}
#endregion

#region ### Class DesignExperimentNavigationPage ###
/// <summary>
/// Represents a side navigation page in the experimental sample shell.
/// </summary>
/// <param name="title">The page title.</param>
/// <param name="description">The page description.</param>
public sealed class DesignExperimentNavigationPage(string title, string description)
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the page title.
    /// </summary>
    public string Title { get; } = title;

    /// <summary>
    /// Gets the page description.
    /// </summary>
    public string Description { get; } = description;
    #endregion
}
#endregion

#region ### Class DesignExperimentCategoryRow ###
/// <summary>
/// Represents a single row in the experimental category grid.
/// </summary>
/// <param name="code">The category code.</param>
/// <param name="category">The category name.</param>
/// <param name="parent">The parent category.</param>
/// <param name="isActive">The active state.</param>
/// <param name="description">The description.</param>
/// <param name="updated">The last update label.</param>
public sealed class DesignExperimentCategoryRow(string code, string category, string parent, string isActive, string description, string updated) : ObservableObject
{
    #region ### Private Fields ###
    private string code = code;
    private string category = category;
    private string parent = parent;
    private string isActive = isActive;
    private string description = description;
    private string updated = updated;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the category code.
    /// </summary>
    public string Code
    {
        get => this.code;
        set => this.SetProperty(ref this.code, value);
    }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string Category
    {
        get => this.category;
        set => this.SetProperty(ref this.category, value);
    }

    /// <summary>
    /// Gets or sets the parent category.
    /// </summary>
    public string Parent
    {
        get => this.parent;
        set => this.SetProperty(ref this.parent, value);
    }

    /// <summary>
    /// Gets or sets the active state.
    /// </summary>
    public string IsActive
    {
        get => this.isActive;
        set => this.SetProperty(ref this.isActive, value);
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value);
    }

    /// <summary>
    /// Gets or sets the last update label.
    /// </summary>
    public string Updated
    {
        get => this.updated;
        set => this.SetProperty(ref this.updated, value);
    }
    #endregion
}
#endregion
