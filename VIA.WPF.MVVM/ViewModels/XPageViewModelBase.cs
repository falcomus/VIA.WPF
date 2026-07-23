// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPageViewModelBase.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VIA.WPF.MVVM;

#region ### Class XPageViewModelBase ###
/// <summary>
/// Provides a reusable base class for page view models.
/// </summary>
public abstract class XPageViewModelBase : XViewModelBase
{
    #region ### Fields ###
    private string? searchTerm;
    private bool isSearchEnabled = true;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XPageViewModelBase"/> class.
    /// </summary>
    protected XPageViewModelBase()
    {
        this.SearchableColumns.CollectionChanged += this.OnSearchableColumnsCollectionChanged;
        this.ResetSearchCommand = new RelayCommand(this.ResetSearch, this.CanResetSearch);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XPageViewModelBase"/> class.
    /// </summary>
    /// <param name="messengerService">The messenger service.</param>
    protected XPageViewModelBase(IXMessengerService messengerService)
        : base(messengerService)
    {
        this.SearchableColumns.CollectionChanged += this.OnSearchableColumnsCollectionChanged;
        this.ResetSearchCommand = new RelayCommand(this.ResetSearch, this.CanResetSearch);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the current search term.
    /// </summary>
    public string? SearchTerm
    {
        get => this.searchTerm;
        set
        {
            if (this.SetProperty(ref this.searchTerm, value))
            {
                this.OnSearchChanged();
                this.ResetSearchCommand.NotifyCanExecuteChanged();
                this.OnPropertyChanged(nameof(this.HasSearch));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether search is enabled for the page.
    /// </summary>
    public bool IsSearchEnabled
    {
        get => this.isSearchEnabled;
        set
        {
            if (this.SetProperty(ref this.isSearchEnabled, value))
            {
                this.OnPropertyChanged(nameof(this.HasSearch));
            }
        }
    }

    /// <summary>
    /// Gets the searchable columns.
    /// </summary>
    public ObservableCollection<XSearchableColumn> SearchableColumns { get; } = [];

    /// <summary>
    /// Gets a value indicating whether the page has search capabilities.
    /// </summary>
    public bool HasSearch => this.IsSearchEnabled && (this.SearchableColumns.Any(column => column.IsEnabled) || !string.IsNullOrWhiteSpace(this.SearchTerm));

    /// <summary>
    /// Gets the command that resets the current search.
    /// </summary>
    public IRelayCommand ResetSearchCommand { get; }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Adds a searchable column.
    /// </summary>
    /// <param name="propertyName">The technical property name.</param>
    /// <param name="displayName">The display name.</param>
    /// <returns>The added column.</returns>
    public XSearchableColumn AddSearchableColumn(string propertyName, string? displayName = null)
    {
        XSearchableColumn column = new(propertyName, displayName ?? propertyName);
        this.SearchableColumns.Add(column);
        return column;
    }

    /// <summary>
    /// Adds a searchable column with localizable display text.
    /// </summary>
    /// <param name="propertyName">The technical property name.</param>
    /// <param name="displayText">The localizable display text.</param>
    /// <returns>The added column.</returns>
    public XSearchableColumn AddSearchableColumn(string propertyName, XValidationText displayText)
    {
        XSearchableColumn column = new(propertyName, displayText);
        this.SearchableColumns.Add(column);
        return column;
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Called when the search state changes.
    /// </summary>
    protected virtual void OnSearchChanged()
    {
    }

    /// <inheritdoc />
    protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
    {
        return base.ShouldValidateAfterPropertyChanged(propertyName)
            && propertyName is not nameof(this.SearchTerm)
            && propertyName is not nameof(this.IsSearchEnabled)
            && propertyName is not nameof(this.HasSearch);
    }
    #endregion

    #region ### Private Methods ###
    private bool CanResetSearch()
    {
        return !string.IsNullOrWhiteSpace(this.SearchTerm);
    }

    private void ResetSearch()
    {
        this.SearchTerm = string.Empty;
    }

    private void OnSearchableColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (XSearchableColumn column in e.OldItems.OfType<XSearchableColumn>())
            {
                column.PropertyChanged -= this.OnSearchableColumnPropertyChanged;
            }
        }

        if (e.NewItems is not null)
        {
            foreach (XSearchableColumn column in e.NewItems.OfType<XSearchableColumn>())
            {
                column.PropertyChanged += this.OnSearchableColumnPropertyChanged;
            }
        }

        this.OnPropertyChanged(nameof(this.HasSearch));
    }

    private void OnSearchableColumnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.PropertyName) || e.PropertyName == nameof(XSearchableColumn.IsEnabled))
        {
            this.OnPropertyChanged(nameof(this.HasSearch));
        }
    }
    #endregion
}
#endregion
