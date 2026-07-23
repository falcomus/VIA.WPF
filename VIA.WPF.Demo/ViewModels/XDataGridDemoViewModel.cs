// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VIA.WPF.Controls;
using VIA.WPF.Controls.Navigation;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XDataGridDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XDataGrid showcase page.
/// </summary>
public sealed class XDataGridDemoViewModel : DemoPageViewModel, IXSearchContext
{
    #region ### Fields ###
    private const int InitialRowCount = 100;
    private const int BenchmarkRunCount = 5;

    private int nextId = InitialRowCount + 1;
    private ObservableCollection<XDataGridDemoItem> rows = [];
    private string searchTerm = string.Empty;
    private string lastGridAction = "Loaded 100 initial rows. Open Edit or double-click a row to edit data.";
    private string detailHeader = "Project details";
    private XDataGridDemoEditor? editor;
    private XDataGridDemoItem? editingSourceRow;
    private bool hasEditorChanges;
    private bool isCreatingEditorRow;
    private bool isDetailOpen;
    private bool isBenchmarkRunning;
    private bool isDemoReady;
    private int benchmarkRowCount;
    private string lastLoadDuration = "Pending";
    private string lastSearchDuration = "Not measured";
    private string lastScenarioDuration = "Not measured";
    private string overallPerformanceRating = "Not measured";
    private string overallPerformanceRatingState = "Neutral";
    private string performanceModeText = "Showcase mode";
    private XDataGridDemoItem? selectedRow;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridDemoViewModel"/> class.
    /// </summary>
    public XDataGridDemoViewModel()
    {
        this.Toolbar = XToolbarContext.CreateCrud();
        this.Toolbar.ShowViewButton = true;
        this.Toolbar.NewCommand = new RelayCommand(this.CreateRow);
        this.Toolbar.ViewCommand = new RelayCommand(() => this.ViewRow(this.SelectedRow));
        this.Toolbar.EditCommand = new RelayCommand(() => this.EditRow(this.SelectedRow));
        this.Toolbar.DeleteCommand = new RelayCommand(() => this.DeleteRow(this.SelectedRow));

        this.ResetSearchCommand = new RelayCommand(() => this.SearchTerm = string.Empty);
        this.CloseDetailCommand = new RelayCommand(this.CloseDetail);
        this.SaveDetailCommand = new RelayCommand(this.SaveDetail, this.CanSaveDetail);
        this.ViewRowCommand = new RelayCommand<XDataGridDemoItem?>(this.ViewRow);
        this.EditRowCommand = new RelayCommand<XDataGridDemoItem?>(this.EditRow);
        this.DeleteRowCommand = new RelayCommand<XDataGridDemoItem?>(this.DeleteRow);
        this.Load1KRowsCommand = new AsyncRelayCommand(() => this.LoadRowsAsync(1_000));
        this.Load10KRowsCommand = new AsyncRelayCommand(() => this.LoadRowsAsync(10_000));
        this.Load50KRowsCommand = new AsyncRelayCommand(() => this.LoadRowsAsync(50_000));
        this.RunSearchBenchmarkCommand = new AsyncRelayCommand(this.RunSearchBenchmarkAsync);
        this.RunScaleBenchmarkCommand = new AsyncRelayCommand(this.RunScaleBenchmarkAsync);
        this.ClearPerformanceSearchCommand = new RelayCommand(this.ClearPerformanceSearch);

        this.LoadInitialRows();
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XDataGrid";

    /// <inheritdoc/>
    public override string Description => "Demonstrates the enhanced data grid with toolbar search, column filters, action column, keyboard commands, view-state handling and modal row editing.";

    /// <summary>
    /// Gets the toolbar context used by the grid demo.
    /// </summary>
    public XToolbarContext Toolbar { get; }

    /// <summary>
    /// Gets the sample rows displayed by the demo grid.
    /// </summary>
    public ObservableCollection<XDataGridDemoItem> Rows
    {
        get => this.rows;
        private set => this.SetProperty(ref this.rows, value);
    }

    /// <summary>
    /// Gets the latest performance benchmark results.
    /// </summary>
    public ObservableCollection<XDataGridPerformanceResult> PerformanceResults { get; } = [];

    /// <summary>
    /// Gets the short explanation for the benchmark rating scale.
    /// </summary>
    public string PerformanceRatingHelpText => "Green = responsive, Yellow = acceptable, Red = investigate. Load tests include generation, collection swap and a dispatcher layout pass.";

    /// <summary>
    /// Gets the manually configured searchable columns.
    /// </summary>
    public ObservableCollection<string> SearchableColumns { get; } =
    [
        nameof(XDataGridDemoItem.Name),
        nameof(XDataGridDemoItem.Status),
        nameof(XDataGridDemoItem.Owner),
        nameof(XDataGridDemoItem.Area),
        nameof(XDataGridDemoItem.Priority),
        nameof(XDataGridDemoItem.Description)
    ];

    /// <summary>
    /// Gets the manually configured filter definitions.
    /// </summary>
    public ObservableCollection<XDataGridFilterDefinition> FilterDefinitions { get; } =
    [
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Id), DisplayName = "ID" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Name), DisplayName = "Project" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Status), DisplayName = "Status" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Owner), DisplayName = "Owner" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Area), DisplayName = "Area" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Priority), DisplayName = "Priority" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.DueDate), DisplayName = "Due date" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.Progress), DisplayName = "Progress" },
        new XDataGridFilterDefinition { ColumnName = nameof(XDataGridDemoItem.IsFeatured), DisplayName = "Featured" }
    ];

    /// <inheritdoc/>
    public string SearchTerm
    {
        get => this.searchTerm;
        set => this.SetProperty(ref this.searchTerm, value);
    }

    /// <inheritdoc/>
    public ICommand ResetSearchCommand { get; }

    /// <summary>
    /// Gets the command that closes the detail dialog.
    /// </summary>
    public ICommand CloseDetailCommand { get; }

    /// <summary>
    /// Gets the command that saves the active editor.
    /// </summary>
    public IRelayCommand SaveDetailCommand { get; }

    /// <summary>
    /// Gets the command that opens a row in view mode.
    /// </summary>
    public ICommand ViewRowCommand { get; }

    /// <summary>
    /// Gets the command that edits a row.
    /// </summary>
    public ICommand EditRowCommand { get; }

    /// <summary>
    /// Gets the command that removes a row.
    /// </summary>
    public ICommand DeleteRowCommand { get; }

    /// <summary>
    /// Gets the command that loads 1,000 synthetic rows.
    /// </summary>
    public IAsyncRelayCommand Load1KRowsCommand { get; }

    /// <summary>
    /// Gets the command that loads 10,000 synthetic rows.
    /// </summary>
    public IAsyncRelayCommand Load10KRowsCommand { get; }

    /// <summary>
    /// Gets the command that loads 50,000 synthetic rows.
    /// </summary>
    public IAsyncRelayCommand Load50KRowsCommand { get; }

    /// <summary>
    /// Gets the command that runs a search/filter refresh benchmark.
    /// </summary>
    public IAsyncRelayCommand RunSearchBenchmarkCommand { get; }

    /// <summary>
    /// Gets the command that runs the 1,000 / 10,000 / 50,000 scale benchmark.
    /// </summary>
    public IAsyncRelayCommand RunScaleBenchmarkCommand { get; }

    /// <summary>
    /// Gets the command that clears the benchmark search term.
    /// </summary>
    public ICommand ClearPerformanceSearchCommand { get; }

    /// <summary>
    /// Gets or sets the currently selected row.
    /// </summary>
    public XDataGridDemoItem? SelectedRow
    {
        get => this.selectedRow;
        set => this.SetProperty(ref this.selectedRow, value);
    }

    /// <summary>
    /// Gets the active row editor displayed in the detail dialog.
    /// </summary>
    public XDataGridDemoEditor? Editor
    {
        get => this.editor;
        private set
        {
            if (ReferenceEquals(this.editor, value))
            {
                return;
            }

            this.DetachEditor(this.editor);

            if (this.SetProperty(ref this.editor, value))
            {
                this.AttachEditor(value);
                this.SaveDetailCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the last command information displayed below the toolbar.
    /// </summary>
    public string LastGridAction
    {
        get => this.lastGridAction;
        set => this.SetProperty(ref this.lastGridAction, value);
    }

    /// <summary>
    /// Gets or sets the detail dialog header.
    /// </summary>
    public string DetailHeader
    {
        get => this.detailHeader;
        set => this.SetProperty(ref this.detailHeader, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail dialog is open.
    /// </summary>
    public bool IsDetailOpen
    {
        get => this.isDetailOpen;
        set
        {
            if (this.SetProperty(ref this.isDetailOpen, value))
            {
                if (!value)
                {
                    this.Editor = null;
                    this.editingSourceRow = null;
                    this.isCreatingEditorRow = false;
                    this.hasEditorChanges = false;
                }

                this.SaveDetailCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the performance test is running.
    /// </summary>
    public bool IsBenchmarkRunning
    {
        get => this.isBenchmarkRunning;
        set => this.SetProperty(ref this.isBenchmarkRunning, value);
    }

    /// <summary>
    /// Gets a value indicating whether the initial sample data has been loaded.
    /// </summary>
    public bool IsDemoReady
    {
        get => this.isDemoReady;
        private set => this.SetProperty(ref this.isDemoReady, value);
    }

    /// <summary>
    /// Gets or sets the currently loaded benchmark row count.
    /// </summary>
    public int BenchmarkRowCount
    {
        get => this.benchmarkRowCount;
        set => this.SetProperty(ref this.benchmarkRowCount, value);
    }

    /// <summary>
    /// Gets or sets the display text for the current performance mode.
    /// </summary>
    public string PerformanceModeText
    {
        get => this.performanceModeText;
        set => this.SetProperty(ref this.performanceModeText, value);
    }

    /// <summary>
    /// Gets or sets the latest measured collection swap and first layout duration.
    /// </summary>
    public string LastLoadDuration
    {
        get => this.lastLoadDuration;
        set => this.SetProperty(ref this.lastLoadDuration, value);
    }

    /// <summary>
    /// Gets or sets the latest measured search refresh duration.
    /// </summary>
    public string LastSearchDuration
    {
        get => this.lastSearchDuration;
        set => this.SetProperty(ref this.lastSearchDuration, value);
    }

    /// <summary>
    /// Gets or sets the latest measured scenario duration.
    /// </summary>
    public string LastScenarioDuration
    {
        get => this.lastScenarioDuration;
        set => this.SetProperty(ref this.lastScenarioDuration, value);
    }

    /// <summary>
    /// Gets or sets the overall performance rating display text.
    /// </summary>
    public string OverallPerformanceRating
    {
        get => this.overallPerformanceRating;
        set => this.SetProperty(ref this.overallPerformanceRating, value);
    }

    /// <summary>
    /// Gets or sets the overall performance rating state.
    /// </summary>
    public string OverallPerformanceRatingState
    {
        get => this.overallPerformanceRatingState;
        set => this.SetProperty(ref this.overallPerformanceRatingState, value);
    }

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XHeaderBar TitleWidth="0">
    <via:XHeaderBar.Actions>
        <via:XButton
            Command="{Binding Toolbar.NewCommand}"
            Content="New"
            Size="Small" />
    </via:XHeaderBar.Actions>
    <via:XHeaderGroup Header="Search" IsSeparatorVisible="False">
        <via:XSearchBox
            Width="300"
            Placeholder="Search projects"
            Text="{Binding SearchTerm, Mode=TwoWay}" />
    </via:XHeaderGroup>
</via:XHeaderBar>

<via:XViewContainer
    CancelDetailText="Cancel"
    CloseDetailCommand="{Binding CloseDetailCommand}"
    DetailHeader="{Binding DetailHeader}"
    DetailHost="{Binding Editor}"
    DetailPresentation="Dialog"
    IsDetailOpen="{Binding IsDetailOpen, Mode=TwoWay}"
    PrimaryDetailCommand="{Binding SaveDetailCommand}"
    PrimaryDetailText="Save"
    ShowDefaultDetailFooter="True"
    ShowValidationHint="True"
    ValidationSource="{Binding Editor}">
    <via:XViewContainer.ListHost>
        <via:XDataGrid
            AutoGenerateColumns="False"
            AutoGenerateFilterDefinitions="False"
            AutoGenerateSearchableColumns="False"
            DeleteItemCommand="{Binding DeleteRowCommand}"
            EditItemCommand="{Binding EditRowCommand}"
            FilterDefinitions="{Binding FilterDefinitions}"
            IsReadOnly="True"
            ItemsSource="{Binding Rows}"
            NewItemCommand="{Binding Toolbar.NewCommand}"
            SearchableColumns="{Binding SearchableColumns}"
            SearchTerm="{Binding SearchTerm, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
            SelectedItem="{Binding SelectedRow, Mode=TwoWay}"
            ShowActionColumn="True"
            ShowDeleteButton="True"
            ShowEditButton="True"
            ShowFilterButtons="True"
            ShowViewButton="True"
            ViewItemCommand="{Binding ViewRowCommand}" />
    </via:XViewContainer.ListHost>
</via:XViewContainer>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public sealed class ProjectsViewModel : ObservableObject, IXSearchContext
{
    public ProjectsViewModel()
    {
        Toolbar = XToolbarContext.CreateCrud();
        Toolbar.ShowViewButton = true;
        Toolbar.NewCommand = new RelayCommand(CreateProject);
        Toolbar.ViewCommand = new RelayCommand(() => ViewProject(SelectedProject));
        Toolbar.EditCommand = new RelayCommand(() => EditProject(SelectedProject));
        Toolbar.DeleteCommand = new RelayCommand(() => DeleteProject(SelectedProject));
        ResetSearchCommand = new RelayCommand(() => SearchTerm = string.Empty);
    }

    public XToolbarContext Toolbar { get; }
    public ObservableCollection<ProjectItem> Rows { get; } = [];
    public ObservableCollection<string> SearchableColumns { get; } = [nameof(ProjectItem.Name), nameof(ProjectItem.Status), nameof(ProjectItem.Owner)];
    public ObservableCollection<XDataGridFilterDefinition> FilterDefinitions { get; } = [new() { ColumnName = nameof(ProjectItem.Name), DisplayName = "Project" }];
    public string SearchTerm { get; set; } = string.Empty;
    public ICommand ResetSearchCommand { get; }
    public ProjectItem? SelectedProject { get; set; }
}
""";
    #endregion

    #region ### Private Methods ###
    private void LoadInitialRows()
    {
        IReadOnlyList<XDataGridDemoItem> items = CreateSyntheticRows(InitialRowCount);
        this.Rows = new ObservableCollection<XDataGridDemoItem>(items);
        this.SelectedRow = this.Rows.FirstOrDefault();
        this.BenchmarkRowCount = this.Rows.Count;
        this.nextId = this.Rows.Count + 1;
        this.LastLoadDuration = "Initial";
        this.PerformanceModeText = "Showcase mode";
        this.IsDemoReady = true;
        this.LastGridAction = "Loaded 100 initial rows. Open Edit or double-click a row to edit data.";
    }

    private async Task LoadRowsAsync(int rowCount)
    {
        if (this.IsBenchmarkRunning)
        {
            return;
        }

        this.IsBenchmarkRunning = true;

        try
        {
            this.PerformanceResults.Clear();
            this.OverallPerformanceRating = "Not measured";
            this.OverallPerformanceRatingState = "Neutral";
            XDataGridLoadMeasurement measurement = await this.LoadRowsCoreAsync(rowCount, true, true);
            this.LastGridAction = FormattableString.Invariant($"Loaded {rowCount:N0} rows. Generate: {FormatElapsed(measurement.GenerateDuration)}, UI swap + first layout: {FormatElapsed(measurement.UiDuration)}.");
        }
        finally
        {
            this.IsBenchmarkRunning = false;
        }
    }

    private async Task<XDataGridLoadMeasurement> LoadRowsCoreAsync(int rowCount, bool performanceMode, bool clearPerformanceResults)
    {
        this.CloseDetail();
        this.SearchTerm = string.Empty;

        if (clearPerformanceResults)
        {
            this.PerformanceResults.Clear();
        }

        Stopwatch generateStopwatch = Stopwatch.StartNew();
        IReadOnlyList<XDataGridDemoItem> items = await Task.Run(() => CreateSyntheticRows(rowCount));
        generateStopwatch.Stop();

        Stopwatch uiStopwatch = Stopwatch.StartNew();
        this.Rows = new ObservableCollection<XDataGridDemoItem>(items);
        this.SelectedRow = this.Rows.FirstOrDefault();
        this.BenchmarkRowCount = this.Rows.Count;
        this.nextId = this.Rows.Count + 1;
        this.PerformanceModeText = performanceMode ? "Performance mode" : "Showcase mode";

        await WaitForGridLayoutPassAsync();
        uiStopwatch.Stop();

        this.LastLoadDuration = FormatElapsed(uiStopwatch.Elapsed);

        return new XDataGridLoadMeasurement(rowCount, generateStopwatch.Elapsed, uiStopwatch.Elapsed);
    }

    private async Task RunSearchBenchmarkAsync()
    {
        if (this.IsBenchmarkRunning)
        {
            return;
        }

        this.IsBenchmarkRunning = true;

        try
        {
            await this.RunSearchBenchmarkCoreAsync();
        }
        finally
        {
            this.IsBenchmarkRunning = false;
        }
    }

    private async Task<TimeSpan> RunSearchBenchmarkCoreAsync(bool updateAction = true)
    {
        if (this.Rows.Count == 0)
        {
            this.LastSearchDuration = "No rows loaded";
            return TimeSpan.Zero;
        }

        string previousSearchTerm = this.SearchTerm;
        string[] benchmarkTerms = ["Anna", "Ready", "Services", "Project 00042", string.Empty];

        Stopwatch stopwatch = Stopwatch.StartNew();

        foreach (string term in benchmarkTerms)
        {
            this.SearchTerm = term;
            await WaitForGridLayoutPassAsync();
        }

        this.SearchTerm = previousSearchTerm;
        await WaitForGridLayoutPassAsync();
        stopwatch.Stop();

        this.LastSearchDuration = FormatElapsed(stopwatch.Elapsed);

        if (updateAction)
        {
            this.LastGridAction = FormattableString.Invariant($"Search benchmark finished across {benchmarkTerms.Length} terms in {this.LastSearchDuration} on {this.Rows.Count:N0} rows.");
        }

        return stopwatch.Elapsed;
    }

    private async Task RunScaleBenchmarkAsync()
    {
        if (this.IsBenchmarkRunning)
        {
            return;
        }

        this.IsBenchmarkRunning = true;
        this.PerformanceResults.Clear();
        this.OverallPerformanceRating = "Running...";
        this.OverallPerformanceRatingState = "Neutral";
        this.LastGridAction = "Running interleaved warmup and measurement passes...";

        try
        {
            Stopwatch seriesStopwatch = Stopwatch.StartNew();
            Dictionary<int, List<XDataGridLoadMeasurement>> measuredLoads = new()
            {
                [1_000] = [],
                [10_000] = [],
                [50_000] = []
            };

            int[] rowCounts = [1_000, 10_000, 50_000];

            for (int pass = 0; pass <= BenchmarkRunCount; pass++)
            {
                foreach (int rowCount in rowCounts)
                {
                    XDataGridLoadMeasurement measurement = await this.LoadRowsCoreAsync(rowCount, true, false);

                    if (pass > 0)
                    {
                        measuredLoads[rowCount].Add(measurement);
                        this.LastGridAction = FormattableString.Invariant($"Measured {rowCount:N0} rows, pass {pass:N0}/{BenchmarkRunCount:N0}. UI: {FormatElapsed(measurement.UiDuration)}.");
                    }
                    else
                    {
                        this.LastGridAction = FormattableString.Invariant($"Warmup completed for {rowCount:N0} rows.");
                    }
                }
            }

            foreach (int rowCount in rowCounts)
            {
                XDataGridLoadMeasurement medianMeasurement = XDataGridLoadMeasurement.GetMedian(measuredLoads[rowCount]);
                await this.LoadRowsCoreAsync(rowCount, true, false);
                TimeSpan searchDuration = await this.RunSearchBenchmarkCoreAsync(false);
                TimeSpan scenarioDuration = medianMeasurement.TotalDuration + searchDuration;
                XDataGridPerformanceResult result = XDataGridPerformanceResult.Create(rowCount, medianMeasurement, searchDuration, scenarioDuration, measuredLoads[rowCount]);
                this.PerformanceResults.Add(result);
            }

            seriesStopwatch.Stop();
            this.LastScenarioDuration = FormatElapsed(seriesStopwatch.Elapsed);
            this.UpdateOverallPerformanceRating();
            this.LastGridAction = FormattableString.Invariant($"Scale benchmark completed in {this.LastScenarioDuration}. Overall rating: {this.OverallPerformanceRating}.");
        }
        finally
        {
            this.IsBenchmarkRunning = false;
        }
    }

    private void UpdateOverallPerformanceRating()
    {
        if (this.PerformanceResults.Count == 0)
        {
            this.OverallPerformanceRating = "Not measured";
            this.OverallPerformanceRatingState = "Neutral";
            return;
        }

        XDataGridPerformanceRating worstRating = this.PerformanceResults.Max(result => result.Rating);
        this.OverallPerformanceRatingState = XDataGridPerformanceResult.GetRatingState(worstRating);
        this.OverallPerformanceRating = XDataGridPerformanceResult.GetRatingDisplay(worstRating);
    }

    private void ClearPerformanceSearch()
    {
        this.SearchTerm = string.Empty;
        this.LastGridAction = "Search term cleared.";
    }

    private void CreateRow()
    {
        XDataGridDemoEditor editor = new()
        {
            Id = 2000 + this.nextId,
            DueDate = DateTime.Today.AddDays(14),
            Progress = 0d,
            IsReadOnly = false
        };

        this.OpenEditor(editor, null, true, "Create project");
        this.LastGridAction = "New command opened an empty project editor. Save becomes available after valid changes.";
    }

    private void ViewRow(XDataGridDemoItem? item)
    {
        if (item is null)
        {
            this.LastGridAction = "Select a row before using View.";
            return;
        }

        this.SelectedRow = item;
        this.OpenEditor(XDataGridDemoEditor.FromItem(item, isReadOnly: true), item, false, $"Viewing {item.Name}");
        this.LastGridAction = $"View command executed for {item.Name}.";
    }

    private void EditRow(XDataGridDemoItem? item)
    {
        if (item is null)
        {
            this.LastGridAction = "Select a row before using Edit.";
            return;
        }

        this.SelectedRow = item;
        this.OpenEditor(XDataGridDemoEditor.FromItem(item, isReadOnly: false), item, false, $"Editing {item.Name}");
        this.LastGridAction = $"Edit command executed for {item.Name}. Save becomes available after a valid change.";
    }

    private void DeleteRow(XDataGridDemoItem? item)
    {
        if (item is null)
        {
            this.LastGridAction = "Select a row before using Delete.";
            return;
        }

        this.Rows.Remove(item);
        this.BenchmarkRowCount = this.Rows.Count;
        this.SelectedRow = this.Rows.FirstOrDefault();
        this.CloseDetail();
        this.LastGridAction = $"Deleted {item.Name}. Delete key uses the same command.";
    }

    private void OpenEditor(XDataGridDemoEditor editor, XDataGridDemoItem? sourceRow, bool isCreatingRow, string header)
    {
        this.DetailHeader = header;
        this.editingSourceRow = sourceRow;
        this.isCreatingEditorRow = isCreatingRow;
        this.hasEditorChanges = false;
        this.Editor = editor;
        this.IsDetailOpen = true;
        this.SaveDetailCommand.NotifyCanExecuteChanged();
    }

    private void CloseDetail()
    {
        this.IsDetailOpen = false;
    }

    private bool CanSaveDetail()
    {
        return this.IsDetailOpen &&
            this.hasEditorChanges &&
            this.Editor is { IsReadOnly: false, HasErrors: false };
    }

    private void SaveDetail()
    {
        if (this.Editor is null || !this.Editor.Validate())
        {
            this.SaveDetailCommand.NotifyCanExecuteChanged();
            return;
        }

        XDataGridDemoItem savedItem;

        if (this.isCreatingEditorRow || this.editingSourceRow is null)
        {
            savedItem = this.Editor.ToItem();
            this.nextId++;
            this.Rows.Insert(0, savedItem);
            this.BenchmarkRowCount = this.Rows.Count;
        }
        else
        {
            savedItem = this.editingSourceRow;
            this.Editor.ApplyTo(savedItem);
        }

        this.SelectedRow = savedItem;
        this.LastGridAction = $"Saved {savedItem.Name}.";
        this.CloseDetail();
    }

    private void AttachEditor(XDataGridDemoEditor? editor)
    {
        if (editor is null)
        {
            return;
        }

        editor.PropertyChanged += this.OnEditorPropertyChanged;
        editor.ErrorsChanged += this.OnEditorErrorsChanged;
    }

    private void DetachEditor(XDataGridDemoEditor? editor)
    {
        if (editor is null)
        {
            return;
        }

        editor.PropertyChanged -= this.OnEditorPropertyChanged;
        editor.ErrorsChanged -= this.OnEditorErrorsChanged;
    }

    private void OnEditorPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(XDataGridDemoEditor.IsReadOnly) and not nameof(XDataGridDemoEditor.IsEditable) and not "HasErrors")
        {
            this.hasEditorChanges = true;
        }

        this.SaveDetailCommand.NotifyCanExecuteChanged();
    }

    private void OnEditorErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        this.SaveDetailCommand.NotifyCanExecuteChanged();
    }

    private static IReadOnlyList<XDataGridDemoItem> CreateSyntheticRows(int rowCount)
    {
        string[] statuses = ["Blocked", "In Progress", "In Review", "Ready", "Released"];
        string[] owners = ["Anna", "Ben", "Mira", "Tom", "Emma", "Liam", "Zoe", "Max", "Ella", "Paul", "Lina", "Tim", "Sara", "Finn", "Mila", "Leo", "Nina", "Jonas", "Marie", "Noah", "Lea", "Oskar", "Ida", "Karl", "Sofia"];
        string[] areas = ["Controls", "Services", "Themes", "Security", "UI", "Icons", "Validation", "Navigation", "Windowing", "Quality", "Release", "Behaviors"];
        string[] priorities = ["Low", "Medium", "High", "Critical"];
        string[] prefixes = ["Grid", "Editor", "Export", "Theme", "Auth", "Search", "Report", "Audit", "Upload", "Notify", "Import", "Icon", "Layout", "Validation", "Navigation", "Window", "Lookup", "Keyboard", "Accessibility", "Package"];
        DateTime startDate = new(2026, 1, 1);
        List<XDataGridDemoItem> generatedRows = new(rowCount);

        for (int index = 0; index < rowCount; index++)
        {
            string status = statuses[index % statuses.Length];
            string owner = owners[(index * 7) % owners.Length];
            string area = areas[(index * 5) % areas.Length];
            string priority = priorities[(index * 3) % priorities.Length];
            string prefix = prefixes[index % prefixes.Length];
            int id = 10_000 + index + 1;
            int progress = (index * 37) % 101;

            generatedRows.Add(new XDataGridDemoItem(
                id,
                FormattableString.Invariant($"{prefix} Project {index + 1:00000}"),
                status,
                owner,
                area,
                priority,
                startDate.AddDays(index % 365),
                progress,
                index % 11 == 0,
                FormattableString.Invariant($"Synthetic benchmark row {index + 1:00000} for XDataGrid filtering, sorting and virtualization tests.")));
        }

        return generatedRows;
    }

    private static async Task WaitForGridLayoutPassAsync()
    {
        if (Application.Current is null)
        {
            await Task.Yield();
            return;
        }

        await Application.Current.Dispatcher.InvokeAsync(static () => { }, DispatcherPriority.Render);
        await Application.Current.Dispatcher.InvokeAsync(static () => { }, DispatcherPriority.ApplicationIdle);
    }

    private static TimeSpan GetMedian(IReadOnlyList<TimeSpan> values)
    {
        if (values.Count == 0)
        {
            return TimeSpan.Zero;
        }

        List<TimeSpan> ordered = values.OrderBy(value => value).ToList();
        return ordered[ordered.Count / 2];
    }

    private static string FormatElapsed(TimeSpan elapsed)
    {
        return elapsed.TotalMilliseconds < 1_000d
            ? string.Create(CultureInfo.InvariantCulture, $"{elapsed.TotalMilliseconds:N0} ms")
            : string.Create(CultureInfo.InvariantCulture, $"{elapsed.TotalSeconds:N2} s");
    }
    #endregion
}
#endregion

#region ### Class XDataGridLoadMeasurement ###
/// <summary>
/// Represents one measured load pass for the XDataGrid demo.
/// </summary>
public sealed class XDataGridLoadMeasurement
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridLoadMeasurement"/> class.
    /// </summary>
    /// <param name="rowCount">The measured row count.</param>
    /// <param name="generateDuration">The measured data generation duration.</param>
    /// <param name="uiDuration">The measured collection swap and first layout duration.</param>
    public XDataGridLoadMeasurement(int rowCount, TimeSpan generateDuration, TimeSpan uiDuration)
    {
        this.RowCount = rowCount;
        this.GenerateDuration = generateDuration;
        this.UiDuration = uiDuration;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the measured row count.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the measured data generation duration.
    /// </summary>
    public TimeSpan GenerateDuration { get; }

    /// <summary>
    /// Gets the measured collection swap and first layout duration.
    /// </summary>
    public TimeSpan UiDuration { get; }

    /// <summary>
    /// Gets the total measured duration.
    /// </summary>
    public TimeSpan TotalDuration => this.GenerateDuration + this.UiDuration;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the median measurement from the supplied measurements.
    /// </summary>
    /// <param name="measurements">The measurements.</param>
    /// <returns>The median measurement.</returns>
    public static XDataGridLoadMeasurement GetMedian(IReadOnlyList<XDataGridLoadMeasurement> measurements)
    {
        if (measurements.Count == 0)
        {
            return new XDataGridLoadMeasurement(0, TimeSpan.Zero, TimeSpan.Zero);
        }

        List<XDataGridLoadMeasurement> ordered = measurements.OrderBy(measurement => measurement.UiDuration).ToList();
        return ordered[ordered.Count / 2];
    }
    #endregion
}
#endregion

#region ### Class XDataGridDemoEditor ###
/// <summary>
/// Represents the editable detail model used by the XDataGrid showcase dialog.
/// </summary>
public sealed class XDataGridDemoEditor : ObservableValidator
{
    #region ### Fields ###
    private int id;
    private string name = string.Empty;
    private string status = string.Empty;
    private string owner = string.Empty;
    private string area = string.Empty;
    private string priority = string.Empty;
    private DateTime? dueDate;
    private double? progress;
    private bool isFeatured;
    private string description = string.Empty;
    private bool isReadOnly;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridDemoEditor"/> class.
    /// </summary>
    public XDataGridDemoEditor()
    {
        this.ValidateAllProperties();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the project identifier.
    /// </summary>
    public int Id
    {
        get => this.id;
        set => this.SetProperty(ref this.id, value);
    }

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    [Required(ErrorMessage = "Project is required.")]
    [MinLength(3, ErrorMessage = "Project must contain at least 3 characters.")]
    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value, true);
    }

    /// <summary>
    /// Gets or sets the project status.
    /// </summary>
    [Required(ErrorMessage = "Status is required.")]
    public string Status
    {
        get => this.status;
        set => this.SetProperty(ref this.status, value, true);
    }

    /// <summary>
    /// Gets or sets the owner name.
    /// </summary>
    [Required(ErrorMessage = "Owner is required.")]
    public string Owner
    {
        get => this.owner;
        set => this.SetProperty(ref this.owner, value, true);
    }

    /// <summary>
    /// Gets or sets the functional area.
    /// </summary>
    [Required(ErrorMessage = "Area is required.")]
    public string Area
    {
        get => this.area;
        set => this.SetProperty(ref this.area, value, true);
    }

    /// <summary>
    /// Gets or sets the priority label.
    /// </summary>
    [Required(ErrorMessage = "Priority is required.")]
    public string Priority
    {
        get => this.priority;
        set => this.SetProperty(ref this.priority, value, true);
    }

    /// <summary>
    /// Gets or sets the due date.
    /// </summary>
    [Required(ErrorMessage = "Due date is required.")]
    public DateTime? DueDate
    {
        get => this.dueDate;
        set => this.SetProperty(ref this.dueDate, value, true);
    }

    /// <summary>
    /// Gets or sets the completion percentage.
    /// </summary>
    [Required(ErrorMessage = "Progress is required.")]
    [Range(0d, 100d, ErrorMessage = "Progress must be between 0 and 100.")]
    public double? Progress
    {
        get => this.progress;
        set => this.SetProperty(ref this.progress, value, true);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item is featured.
    /// </summary>
    public bool IsFeatured
    {
        get => this.isFeatured;
        set => this.SetProperty(ref this.isFeatured, value);
    }

    /// <summary>
    /// Gets or sets the project description.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [MinLength(8, ErrorMessage = "Description must contain at least 8 characters.")]
    public string Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value, true);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the editor is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => this.isReadOnly;
        set
        {
            if (this.SetProperty(ref this.isReadOnly, value))
            {
                this.OnPropertyChanged(nameof(this.IsEditable));
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the editor fields can be changed.
    /// </summary>
    public bool IsEditable => !this.IsReadOnly;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates an editor from a grid row.
    /// </summary>
    /// <param name="item">The source row.</param>
    /// <param name="isReadOnly">Whether the created editor is read-only.</param>
    /// <returns>The created editor.</returns>
    public static XDataGridDemoEditor FromItem(XDataGridDemoItem item, bool isReadOnly)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new XDataGridDemoEditor
        {
            Id = item.Id,
            Name = item.Name,
            Status = item.Status,
            Owner = item.Owner,
            Area = item.Area,
            Priority = item.Priority,
            DueDate = item.DueDate,
            Progress = item.Progress,
            IsFeatured = item.IsFeatured,
            Description = item.Description,
            IsReadOnly = isReadOnly
        };
    }

    /// <summary>
    /// Validates all editor properties.
    /// </summary>
    /// <returns><c>true</c> when the editor is valid; otherwise <c>false</c>.</returns>
    public bool Validate()
    {
        this.ValidateAllProperties();
        return !this.HasErrors;
    }

    /// <summary>
    /// Creates a new row from the editor values.
    /// </summary>
    /// <returns>The created row.</returns>
    public XDataGridDemoItem ToItem()
    {
        return new XDataGridDemoItem(
            this.Id,
            Normalize(this.Name),
            Normalize(this.Status),
            Normalize(this.Owner),
            Normalize(this.Area),
            Normalize(this.Priority),
            this.DueDate.GetValueOrDefault(DateTime.Today).Date,
            ToProgress(this.Progress),
            this.IsFeatured,
            Normalize(this.Description));
    }

    /// <summary>
    /// Applies the editor values to an existing row.
    /// </summary>
    /// <param name="item">The row to update.</param>
    public void ApplyTo(XDataGridDemoItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        item.Name = Normalize(this.Name);
        item.Status = Normalize(this.Status);
        item.Owner = Normalize(this.Owner);
        item.Area = Normalize(this.Area);
        item.Priority = Normalize(this.Priority);
        item.DueDate = this.DueDate.GetValueOrDefault(DateTime.Today).Date;
        item.Progress = ToProgress(this.Progress);
        item.IsFeatured = this.IsFeatured;
        item.Description = Normalize(this.Description);
    }
    #endregion

    #region ### Private Methods ###
    private static string Normalize(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static int ToProgress(double? value)
    {
        return Math.Clamp((int)Math.Round(value.GetValueOrDefault()), 0, 100);
    }
    #endregion
}
#endregion

#region ### Class XDataGridDemoItem ###
/// <summary>
/// Represents one sample row for the XDataGrid showcase.
/// </summary>
public sealed class XDataGridDemoItem : ObservableObject
{
    #region ### Fields ###
    private int id;
    private string name;
    private string status;
    private string owner;
    private string area;
    private string priority;
    private DateTime dueDate;
    private int progress;
    private bool isFeatured;
    private string description;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDataGridDemoItem"/> class.
    /// </summary>
    public XDataGridDemoItem(int id, string name, string status, string owner, string area, string priority, DateTime dueDate, int progress, bool isFeatured, string description)
    {
        this.id = id;
        this.name = name;
        this.status = status;
        this.owner = owner;
        this.area = area;
        this.priority = priority;
        this.dueDate = dueDate;
        this.progress = progress;
        this.isFeatured = isFeatured;
        this.description = description;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the project identifier.
    /// </summary>
    public int Id
    {
        get => this.id;
        set => this.SetProperty(ref this.id, value);
    }

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value);
    }

    /// <summary>
    /// Gets or sets the project status.
    /// </summary>
    public string Status
    {
        get => this.status;
        set => this.SetProperty(ref this.status, value);
    }

    /// <summary>
    /// Gets or sets the owner name.
    /// </summary>
    public string Owner
    {
        get => this.owner;
        set => this.SetProperty(ref this.owner, value);
    }

    /// <summary>
    /// Gets or sets the functional area.
    /// </summary>
    public string Area
    {
        get => this.area;
        set => this.SetProperty(ref this.area, value);
    }

    /// <summary>
    /// Gets or sets the priority label.
    /// </summary>
    public string Priority
    {
        get => this.priority;
        set => this.SetProperty(ref this.priority, value);
    }

    /// <summary>
    /// Gets or sets the due date.
    /// </summary>
    public DateTime DueDate
    {
        get => this.dueDate;
        set
        {
            if (this.SetProperty(ref this.dueDate, value))
            {
                this.OnPropertyChanged(nameof(this.DueDateDisplay));
            }
        }
    }

    /// <summary>
    /// Gets or sets the completion percentage.
    /// </summary>
    public int Progress
    {
        get => this.progress;
        set
        {
            if (this.SetProperty(ref this.progress, value))
            {
                this.OnPropertyChanged(nameof(this.ProgressDisplay));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item is featured.
    /// </summary>
    public bool IsFeatured
    {
        get => this.isFeatured;
        set => this.SetProperty(ref this.isFeatured, value);
    }

    /// <summary>
    /// Gets or sets the project description.
    /// </summary>
    public string Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value);
    }

    /// <summary>
    /// Gets the display text for the due date.
    /// </summary>
    public string DueDateDisplay => this.DueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the display text for the progress value.
    /// </summary>
    public string ProgressDisplay => FormattableString.Invariant($"{this.Progress}%");
    #endregion
}
#endregion

#region ### Enum XDataGridPerformanceRating ###
/// <summary>
/// Defines the traffic-light benchmark rating used by the XDataGrid demo.
/// </summary>
public enum XDataGridPerformanceRating
{
    #region ### Enum Values ###
    /// <summary>
    /// Indicates responsive performance.
    /// </summary>
    Green = 0,

    /// <summary>
    /// Indicates acceptable performance that should be watched.
    /// </summary>
    Yellow = 1,

    /// <summary>
    /// Indicates performance that should be investigated.
    /// </summary>
    Red = 2
    #endregion
}
#endregion

#region ### Class XDataGridPerformanceResult ###
/// <summary>
/// Represents one benchmark result row for the XDataGrid performance demo.
/// </summary>
public sealed class XDataGridPerformanceResult
{
    #region ### Constructors ###
    private XDataGridPerformanceResult(
        int rowCount,
        TimeSpan generateDuration,
        TimeSpan loadDuration,
        TimeSpan searchDuration,
        TimeSpan scenarioDuration,
        IReadOnlyList<XDataGridLoadMeasurement> loadSamples,
        XDataGridPerformanceRating rating)
    {
        this.RowCount = rowCount;
        this.GenerateDuration = generateDuration;
        this.LoadDuration = loadDuration;
        this.SearchDuration = searchDuration;
        this.ScenarioDuration = scenarioDuration;
        this.LoadSamples = loadSamples;
        this.Rating = rating;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the measured row count.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the measured median generation duration.
    /// </summary>
    public TimeSpan GenerateDuration { get; }

    /// <summary>
    /// Gets the measured median collection swap and first layout duration.
    /// </summary>
    public TimeSpan LoadDuration { get; }

    /// <summary>
    /// Gets the measured search benchmark duration.
    /// </summary>
    public TimeSpan SearchDuration { get; }

    /// <summary>
    /// Gets the measured full scenario duration.
    /// </summary>
    public TimeSpan ScenarioDuration { get; }

    /// <summary>
    /// Gets the measured load samples.
    /// </summary>
    public IReadOnlyList<XDataGridLoadMeasurement> LoadSamples { get; }

    /// <summary>
    /// Gets the traffic-light rating.
    /// </summary>
    public XDataGridPerformanceRating Rating { get; }

    /// <summary>
    /// Gets the formatted row count.
    /// </summary>
    public string RowCountDisplay => this.RowCount.ToString("N0", CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the formatted generation duration.
    /// </summary>
    public string GenerateDurationDisplay => FormatElapsed(this.GenerateDuration);

    /// <summary>
    /// Gets the formatted median UI load duration.
    /// </summary>
    public string LoadDurationDisplay => FormatElapsed(this.LoadDuration);

    /// <summary>
    /// Gets the formatted search duration.
    /// </summary>
    public string SearchDurationDisplay => FormatElapsed(this.SearchDuration);

    /// <summary>
    /// Gets the formatted scenario duration.
    /// </summary>
    public string ScenarioDurationDisplay => FormatElapsed(this.ScenarioDuration);

    /// <summary>
    /// Gets the formatted UI load samples.
    /// </summary>
    public string LoadSamplesDisplay => string.Join(" / ", this.LoadSamples.Select(sample => FormatElapsed(sample.UiDuration)));

    /// <summary>
    /// Gets the formatted rating text.
    /// </summary>
    public string RatingDisplay => GetRatingDisplay(this.Rating);

    /// <summary>
    /// Gets the state used by the rating pill style.
    /// </summary>
    public string RatingState => GetRatingState(this.Rating);
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Creates a new benchmark result.
    /// </summary>
    public static XDataGridPerformanceResult Create(
        int rowCount,
        XDataGridLoadMeasurement medianMeasurement,
        TimeSpan searchDuration,
        TimeSpan scenarioDuration,
        IReadOnlyList<XDataGridLoadMeasurement> loadSamples)
    {
        XDataGridPerformanceRating rating = Classify(rowCount, medianMeasurement.UiDuration, searchDuration);
        return new XDataGridPerformanceResult(
            rowCount,
            medianMeasurement.GenerateDuration,
            medianMeasurement.UiDuration,
            searchDuration,
            scenarioDuration,
            loadSamples,
            rating);
    }

    /// <summary>
    /// Gets the display text for the specified rating.
    /// </summary>
    public static string GetRatingDisplay(XDataGridPerformanceRating rating)
    {
        return rating switch
        {
            XDataGridPerformanceRating.Green => "Green",
            XDataGridPerformanceRating.Yellow => "Yellow",
            XDataGridPerformanceRating.Red => "Red",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Gets the visual state name for the specified rating.
    /// </summary>
    public static string GetRatingState(XDataGridPerformanceRating rating)
    {
        return rating switch
        {
            XDataGridPerformanceRating.Green => "Green",
            XDataGridPerformanceRating.Yellow => "Yellow",
            XDataGridPerformanceRating.Red => "Red",
            _ => "Neutral"
        };
    }
    #endregion

    #region ### Private Methods ###
    private static XDataGridPerformanceRating Classify(int rowCount, TimeSpan loadDuration, TimeSpan searchDuration)
    {
        double loadMs = loadDuration.TotalMilliseconds;
        double searchMs = searchDuration.TotalMilliseconds;

        if ((rowCount <= 1_000 && loadMs <= 350d && searchMs <= 600d) ||
            (rowCount <= 10_000 && loadMs <= 900d && searchMs <= 1_800d) ||
            (rowCount <= 50_000 && loadMs <= 3_500d && searchMs <= 5_500d))
        {
            return XDataGridPerformanceRating.Green;
        }

        if ((rowCount <= 1_000 && loadMs <= 800d && searchMs <= 1_500d) ||
            (rowCount <= 10_000 && loadMs <= 2_500d && searchMs <= 4_500d) ||
            (rowCount <= 50_000 && loadMs <= 8_000d && searchMs <= 11_000d))
        {
            return XDataGridPerformanceRating.Yellow;
        }

        return XDataGridPerformanceRating.Red;
    }

    private static string FormatElapsed(TimeSpan elapsed)
    {
        return elapsed.TotalMilliseconds < 1_000d
            ? string.Create(CultureInfo.InvariantCulture, $"{elapsed.TotalMilliseconds:N0} ms")
            : string.Create(CultureInfo.InvariantCulture, $"{elapsed.TotalSeconds:N2} s");
    }
    #endregion
}
#endregion
