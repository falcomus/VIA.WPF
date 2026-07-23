// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using VIA.WPF.Controls;
using VIA.WPF.MVVM;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XValidationDemoViewModel ###
/// <summary>
/// Represents the demo view model for the VIA.WPF MVVM validation concept.
/// </summary>
public sealed class XValidationDemoViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationDemoViewModel"/> class.
    /// </summary>
    public XValidationDemoViewModel()
    {
        this.Form = new XValidationSampleEditorViewModel();
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Validation";

    /// <inheritdoc/>
    public override string Description => "Demonstrates the VIA.WPF.MVVM validation concept with field errors, multi-field rules, async rules, dirty tracking and a validation summary.";

    /// <summary>
    /// Gets the sample editor form.
    /// </summary>
    public XValidationSampleEditorViewModel Form { get; }

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XValidationHintPopup
    Source="{Binding}"
    IncludeInformation="True"
    ShowWhenValid="True" />

<TextBlock Text="{Binding ValidationSummaryText}" />

<via:XBadge Content="{Binding DirtyBadgeText}" Variant="{Binding DirtyBadgeVariant}" />

<via:XTextBox
    Header="Article number"
    Text="{via:XBind ArticleNumber}" />

<via:XTextBox
    Header="Email"
    Text="{via:XBind Email}" />

<via:XTextBox
    Header="Website"
    Text="{via:XBind Website}" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public sealed class EditorViewModel : XEditorViewModelBase
{
    private static readonly Regex ArticleNumberRegex = new(
        "^[A-Z]{3}-[0-9]{4}$",
        RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(250));

    protected override async Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
    {
        context.Required(this.ArticleNumber, nameof(this.ArticleNumber), XValidationText.Text("Bitte Artikelnummer eingeben."));
        context.MatchesIf(
            !string.IsNullOrWhiteSpace(this.ArticleNumber),
            this.ArticleNumber,
            ArticleNumberRegex,
            nameof(this.ArticleNumber),
            XValidationText.Text("Format: ABC-1234."));

        context.EmailIf(
            !string.IsNullOrWhiteSpace(this.Email),
            this.Email,
            nameof(this.Email),
            XValidationText.Text("Bitte eine gültige E-Mail eingeben."));

        context.WebUrlIf(
            !string.IsNullOrWhiteSpace(this.Website),
            this.Website,
            nameof(this.Website),
            XValidationText.Text("Bitte eine gültige HTTP/HTTPS-Adresse eingeben."));

        DateTime? beginDate = this.ParseDate(this.BeginDate);
        DateTime? endDate = this.ParseDate(this.EndDate);
        context.Compare<DateTime?>(
            beginDate,
            nameof(this.BeginDate),
            endDate,
            nameof(this.EndDate),
            (begin, end) => begin.HasValue && end.HasValue && begin.Value > end.Value,
            XValidationText.Text("Beginn darf nicht nach Ende liegen."));

        await context.MustBeTrueAsync(
            async token => await this.ArticleNumberIsUniqueAsync(token),
            XValidationText.Text("Diese Artikelnummer existiert bereits."),
            cancellationToken,
            nameof(this.ArticleNumber));
    }
}
""";
    #endregion
}
#endregion

#region ### Class XValidationSampleEditorViewModel ###
/// <summary>
/// Represents a sample editor view model for the validation demo.
/// </summary>
public sealed class XValidationSampleEditorViewModel : XEditorViewModelBase
{
    #region ### Fields ###
    private static readonly Regex ArticleNumberRegex = new(
        "^[A-Z]{3}-[0-9]{4}$",
        RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(250));

    private string articleNumber = "ART-1000";
    private string stockQuantity = "25";
    private string email = "info@example.com";
    private string website = "https://example.com";
    private string note = "Short storage note";
    private string beginDate = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    private string endDate = DateTime.Today.AddDays(3).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    private string employeeId = "1";
    private bool canRentVehicles = true;
    private string? lastSaveMessage;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationSampleEditorViewModel"/> class.
    /// </summary>
    public XValidationSampleEditorViewModel()
    {
        this.ResetCommand = new RelayCommand(this.Reset);
        this.ValidateCommand = new AsyncRelayCommand(this.ValidateAsync);
        this.SaveCommand = new AsyncRelayCommand(this.SaveAsync, this.CanSave);
        this.MarkClean();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the article number.
    /// </summary>
    public string ArticleNumber
    {
        get => this.articleNumber;
        set => this.SetProperty(ref this.articleNumber, value);
    }

    /// <summary>
    /// Gets or sets the stock quantity text.
    /// </summary>
    public string StockQuantity
    {
        get => this.stockQuantity;
        set => this.SetProperty(ref this.stockQuantity, value);
    }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email
    {
        get => this.email;
        set => this.SetProperty(ref this.email, value);
    }

    /// <summary>
    /// Gets or sets the optional website URL.
    /// </summary>
    public string Website
    {
        get => this.website;
        set => this.SetProperty(ref this.website, value);
    }

    /// <summary>
    /// Gets or sets the note.
    /// </summary>
    public string Note
    {
        get => this.note;
        set => this.SetProperty(ref this.note, value);
    }

    /// <summary>
    /// Gets or sets the begin date text.
    /// </summary>
    public string BeginDate
    {
        get => this.beginDate;
        set => this.SetProperty(ref this.beginDate, value);
    }

    /// <summary>
    /// Gets or sets the end date text.
    /// </summary>
    public string EndDate
    {
        get => this.endDate;
        set => this.SetProperty(ref this.endDate, value);
    }

    /// <summary>
    /// Gets or sets the employee identifier text.
    /// </summary>
    public string EmployeeId
    {
        get => this.employeeId;
        set
        {
            if (this.SetProperty(ref this.employeeId, value))
            {
                this.CanRentVehicles = !string.Equals(value?.Trim(), "2", StringComparison.Ordinal);
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the selected employee may rent vehicles.
    /// </summary>
    public bool CanRentVehicles
    {
        get => this.canRentVehicles;
        set
        {
            if (this.SetProperty(ref this.canRentVehicles, value))
            {
                this.RevalidateProperties(nameof(this.EmployeeId));
            }
        }
    }

    /// <summary>
    /// Gets or sets the last save message.
    /// </summary>
    public string? LastSaveMessage
    {
        get => this.lastSaveMessage;
        set => this.SetProperty(ref this.lastSaveMessage, value);
    }

    /// <summary>
    /// Gets the dirty-state badge text.
    /// </summary>
    public string DirtyBadgeText => this.IsDirty ? "Modified" : "Clean";

    /// <summary>
    /// Gets the dirty-state badge variant.
    /// </summary>
    public XControlVariant DirtyBadgeVariant => this.IsDirty ? XControlVariant.Warning : XControlVariant.Default;

    /// <summary>
    /// Gets the validation-state badge text.
    /// </summary>
    public string ValidationBadgeText => this.IsValid ? "Valid" : "Invalid";

    /// <summary>
    /// Gets the validation-state badge variant.
    /// </summary>
    public XControlVariant ValidationBadgeVariant => this.IsValid ? XControlVariant.Success : XControlVariant.Danger;

    /// <summary>
    /// Gets the validation activity badge text.
    /// </summary>
    public string ActivityBadgeText => this.IsValidating ? "Validating" : "Ready";

    /// <summary>
    /// Gets the validation activity badge variant.
    /// </summary>
    public XControlVariant ActivityBadgeVariant => this.IsValidating ? XControlVariant.Info : XControlVariant.Default;

    /// <summary>
    /// Gets the reset command.
    /// </summary>
    public IRelayCommand ResetCommand { get; }

    /// <summary>
    /// Gets the validate command.
    /// </summary>
    public IAsyncRelayCommand ValidateCommand { get; }

    /// <summary>
    /// Gets the save command.
    /// </summary>
    public IAsyncRelayCommand SaveCommand { get; }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override async Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
    {
        context.Required(this.ArticleNumber, nameof(this.ArticleNumber), XValidationText.Text("Bitte Artikelnummer eingeben."));
        context.MaxLengthIf(!string.IsNullOrWhiteSpace(this.ArticleNumber), this.ArticleNumber, 12, nameof(this.ArticleNumber), XValidationText.Text("Die Artikelnummer darf maximal 12 Zeichen haben."));
        context.MatchesIf(!string.IsNullOrWhiteSpace(this.ArticleNumber), this.ArticleNumber, ArticleNumberRegex, nameof(this.ArticleNumber), XValidationText.Text("Die Artikelnummer muss dem Format ABC-1234 entsprechen."));

        int? quantity = ValidateRequiredInt(context, this.StockQuantity, nameof(this.StockQuantity), "Bitte Bestand eingeben.", "Der Bestand muss eine ganze Zahl sein.");
        context.RangeIf(quantity.HasValue, quantity, 0, 999, nameof(this.StockQuantity), XValidationText.Text("Der Bestand muss zwischen 0 und 999 liegen."));
        context.WarningIf(quantity > 500, XValidationText.Text("Hoher Bestand: Prüfe, ob diese Menge wirklich benötigt wird."), nameof(this.StockQuantity));

        context.Required(this.Email, nameof(this.Email), XValidationText.Text("Bitte E-Mail eingeben."));
        context.EmailIf(!string.IsNullOrWhiteSpace(this.Email), this.Email, nameof(this.Email), XValidationText.Text("Bitte eine gültige E-Mail eingeben."));

        context.WebUrlIf(!string.IsNullOrWhiteSpace(this.Website), this.Website, nameof(this.Website), XValidationText.Text("Bitte eine gültige HTTP/HTTPS-Adresse eingeben."));

        context.Required(this.Note, nameof(this.Note), XValidationText.Text("Bitte Hinweis eingeben."));
        context.MinLengthIf(!string.IsNullOrWhiteSpace(this.Note), this.Note, 5, nameof(this.Note), XValidationText.Text("Der Hinweis muss mindestens 5 Zeichen enthalten."));
        context.MaxLengthIf(!string.IsNullOrWhiteSpace(this.Note), this.Note, 30, nameof(this.Note), XValidationText.Text("Der Hinweis darf maximal 30 Zeichen haben."));

        DateTime? parsedBeginDate = ValidateRequiredDate(context, this.BeginDate, nameof(this.BeginDate), "Bitte Beginndatum eingeben.", "Bitte Beginndatum im Format yyyy-MM-dd eingeben.");
        DateTime? parsedEndDate = ValidateRequiredDate(context, this.EndDate, nameof(this.EndDate), "Bitte Enddatum eingeben.", "Bitte Enddatum im Format yyyy-MM-dd eingeben.");
        context.Compare<DateTime?>(
            parsedBeginDate,
            nameof(this.BeginDate),
            parsedEndDate,
            nameof(this.EndDate),
            (begin, end) => begin.HasValue && end.HasValue && begin.Value > end.Value,
            XValidationText.Text("Das Beginndatum darf nicht nach dem Enddatum liegen."));

        int? selectedEmployeeId = ValidateRequiredInt(context, this.EmployeeId, nameof(this.EmployeeId), "Bitte Mitarbeiter auswählen.", "Bitte Mitarbeiter-ID 1, 2 oder 3 eingeben.");
        if (selectedEmployeeId.HasValue)
        {
            context.RequiredSelection(selectedEmployeeId, nameof(this.EmployeeId), XValidationText.Text("Bitte Mitarbeiter auswählen."));
        }

        context.RangeIf(selectedEmployeeId.HasValue, selectedEmployeeId, 1, 3, nameof(this.EmployeeId), XValidationText.Text("Bitte Mitarbeiter-ID 1, 2 oder 3 eingeben."));
        context.ErrorIf(selectedEmployeeId == 2 && !this.CanRentVehicles, XValidationText.Text("Der gewählte Mitarbeiter darf keine Fahrzeuge mieten."), nameof(this.EmployeeId), nameof(this.CanRentVehicles));
        context.InformationIf(selectedEmployeeId.HasValue, XValidationText.Text("Externe Zustände können über Services, Repositories oder Lookups in ValidateCoreAsync geprüft werden."), nameof(this.EmployeeId));

        if (!string.IsNullOrWhiteSpace(this.ArticleNumber))
        {
            await context.MustBeTrueAsync(
                async token =>
                {
                    await Task.Delay(350, token);
                    return !string.Equals(this.ArticleNumber.Trim(), "DUP-1000", StringComparison.OrdinalIgnoreCase);
                },
                XValidationText.Text("Diese Artikelnummer existiert bereits. Testwert: DUP-1000"),
                cancellationToken,
                nameof(this.ArticleNumber));
        }
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(this.IsDirty))
        {
            this.OnPropertyChanged(nameof(this.DirtyBadgeText));
            this.OnPropertyChanged(nameof(this.DirtyBadgeVariant));
            this.SaveCommand.NotifyCanExecuteChanged();
        }

        if (e.PropertyName is nameof(this.IsValid) or nameof(this.HasErrors))
        {
            this.OnPropertyChanged(nameof(this.ValidationBadgeText));
            this.OnPropertyChanged(nameof(this.ValidationBadgeVariant));
            this.SaveCommand.NotifyCanExecuteChanged();
        }

        if (e.PropertyName is nameof(this.IsValidating))
        {
            this.OnPropertyChanged(nameof(this.ActivityBadgeText));
            this.OnPropertyChanged(nameof(this.ActivityBadgeVariant));
            this.SaveCommand.NotifyCanExecuteChanged();
        }
    }

    /// <inheritdoc />
    protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return base.ShouldValidateAfterPropertyChanged(propertyName)
            && propertyName is not nameof(this.DirtyBadgeText)
            && propertyName is not nameof(this.DirtyBadgeVariant)
            && propertyName is not nameof(this.ValidationBadgeText)
            && propertyName is not nameof(this.ValidationBadgeVariant)
            && propertyName is not nameof(this.ActivityBadgeText)
            && propertyName is not nameof(this.ActivityBadgeVariant)
            && propertyName is not nameof(this.LastSaveMessage);
    }

    /// <inheritdoc />
    protected override bool ShouldMarkDirty(string? propertyName)
    {
        return base.ShouldMarkDirty(propertyName)
            && propertyName is not nameof(this.DirtyBadgeText)
            && propertyName is not nameof(this.DirtyBadgeVariant)
            && propertyName is not nameof(this.ValidationBadgeText)
            && propertyName is not nameof(this.ValidationBadgeVariant)
            && propertyName is not nameof(this.ActivityBadgeText)
            && propertyName is not nameof(this.ActivityBadgeVariant)
            && propertyName is not nameof(this.LastSaveMessage);
    }
    #endregion

    #region ### Private Methods ###
    private bool CanSave()
    {
        return this.IsDirty && !this.IsValidating && this.IsValid;
    }

    private void Reset()
    {
        this.WithoutDirtyTracking(
            () =>
            {
                this.ArticleNumber = "ART-1000";
                this.StockQuantity = "25";
                this.Email = "info@example.com";
                this.Website = "https://example.com";
                this.Note = "Short storage note";
                this.BeginDate = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                this.EndDate = DateTime.Today.AddDays(3).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                this.EmployeeId = "1";
                this.CanRentVehicles = true;
                this.LastSaveMessage = null;
                this.ClearValidation();
                this.MarkClean();
            });
    }

    private async Task ValidateAsync()
    {
        await this.ValidateAllAsync();
    }

    private async Task SaveAsync()
    {
        bool isValid = await this.ValidateForSaveAsync();

        if (!isValid)
        {
            this.LastSaveMessage = "Speichern blockiert: Bitte Fehler korrigieren.";
            return;
        }

        this.LastSaveMessage = $"Gespeichert um {DateTime.Now:T}.";
        this.MarkClean();
    }

    private static int? ValidateRequiredInt(XValidationContext context, string? value, string propertyName, string requiredMessage, string invalidMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            context.AddError(XValidationText.Text(requiredMessage), propertyName);
            return null;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedValue))
        {
            context.AddError(XValidationText.Text(invalidMessage), propertyName);
            return null;
        }

        return parsedValue;
    }

    private static DateTime? ValidateRequiredDate(XValidationContext context, string? value, string propertyName, string requiredMessage, string invalidMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            context.AddError(XValidationText.Text(requiredMessage), propertyName);
            return null;
        }

        if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedValue))
        {
            context.AddError(XValidationText.Text(invalidMessage), propertyName);
            return null;
        }

        return parsedValue.Date;
    }
    #endregion
}
#endregion
