// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToolbarContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XToolbarContext ###
/// <summary>
/// Describes the toolbar capabilities exposed by the currently active page.
/// </summary>
public class XToolbarContext : XNotifyPropertyChangedObject
{
    #region ### Fields ###
    private bool showNewButton;
    private bool showViewButton;
    private bool showEditButton;
    private bool showDeleteButton;
    private bool showRefreshButton;
    private bool showViewModeSelector;
    private bool showRememberViewToggle;
    private bool isRememberViewStateEnabled;
    private XContentViewMode viewMode = XContentViewMode.Grid;
    private ICommand? newCommand;
    private ICommand? viewCommand;
    private ICommand? editCommand;
    private ICommand? deleteCommand;
    private ICommand? refreshCommand;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the New button is visible.
    /// </summary>
    public bool ShowNewButton
    {
        get => this.showNewButton;
        set
        {
            if (this.SetProperty(ref this.showNewButton, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the View button is visible.
    /// </summary>
    public bool ShowViewButton
    {
        get => this.showViewButton;
        set
        {
            if (this.SetProperty(ref this.showViewButton, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Edit button is visible.
    /// </summary>
    public bool ShowEditButton
    {
        get => this.showEditButton;
        set
        {
            if (this.SetProperty(ref this.showEditButton, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Delete button is visible.
    /// </summary>
    public bool ShowDeleteButton
    {
        get => this.showDeleteButton;
        set
        {
            if (this.SetProperty(ref this.showDeleteButton, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Refresh button is visible.
    /// </summary>
    public bool ShowRefreshButton
    {
        get => this.showRefreshButton;
        set
        {
            if (this.SetProperty(ref this.showRefreshButton, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the view mode selector is visible.
    /// </summary>
    public bool ShowViewModeSelector
    {
        get => this.showViewModeSelector;
        set
        {
            if (this.SetProperty(ref this.showViewModeSelector, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the remember-view-state toggle is visible.
    /// </summary>
    public bool ShowRememberViewToggle
    {
        get => this.showRememberViewToggle;
        set
        {
            if (this.SetProperty(ref this.showRememberViewToggle, value))
            {
                this.NotifyToolbarVisibilityChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current view state should be remembered when navigating.
    /// </summary>
    public bool IsRememberViewStateEnabled
    {
        get => this.isRememberViewStateEnabled;
        set => this.SetProperty(ref this.isRememberViewStateEnabled, value);
    }

    /// <summary>
    /// Gets or sets the current content view mode.
    /// </summary>
    public XContentViewMode ViewMode
    {
        get => this.viewMode;
        set => this.SetProperty(ref this.viewMode, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the New button.
    /// </summary>
    public ICommand? NewCommand
    {
        get => this.newCommand;
        set => this.SetProperty(ref this.newCommand, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the View button.
    /// </summary>
    public ICommand? ViewCommand
    {
        get => this.viewCommand;
        set => this.SetProperty(ref this.viewCommand, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the Edit button.
    /// </summary>
    public ICommand? EditCommand
    {
        get => this.editCommand;
        set => this.SetProperty(ref this.editCommand, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the Delete button.
    /// </summary>
    public ICommand? DeleteCommand
    {
        get => this.deleteCommand;
        set => this.SetProperty(ref this.deleteCommand, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the Refresh button.
    /// </summary>
    public ICommand? RefreshCommand
    {
        get => this.refreshCommand;
        set => this.SetProperty(ref this.refreshCommand, value);
    }

    /// <summary>
    /// Gets a value indicating whether at least one command button is visible.
    /// </summary>
    public bool HasCommandButtons =>
        this.ShowNewButton
        || this.ShowViewButton
        || this.ShowEditButton
        || this.ShowDeleteButton
        || this.ShowRefreshButton;

    /// <summary>
    /// Gets a value indicating whether the toolbar contains any visible item.
    /// </summary>
    public bool HasToolbarItems => this.HasCommandButtons || this.ShowViewModeSelector || this.ShowRememberViewToggle;

    /// <summary>
    /// Gets a value indicating whether a separator between commands and view-mode selector should be shown.
    /// </summary>
    public bool ShowCommandSeparator => this.HasCommandButtons && (this.ShowViewModeSelector || this.ShowRememberViewToggle);
    #endregion

    #region ### Factory Methods ###
    /// <summary>
    /// Creates an empty toolbar context.
    /// </summary>
    /// <returns>The toolbar context.</returns>
    public static XToolbarContext CreateEmpty()
    {
        return new XToolbarContext();
    }

    /// <summary>
    /// Creates a toolbar context for standard create, edit and delete pages.
    /// </summary>
    /// <returns>The toolbar context.</returns>
    public static XToolbarContext CreateCrud()
    {
        return new XToolbarContext
        {
            ShowNewButton = true,
            ShowEditButton = true,
            ShowDeleteButton = true
        };
    }

    /// <summary>
    /// Creates a toolbar context for standard create, edit and delete pages with view-mode selection.
    /// </summary>
    /// <returns>The toolbar context.</returns>
    public static XToolbarContext CreateCrudWithViewMode()
    {
        return new XToolbarContext
        {
            ShowNewButton = true,
            ShowEditButton = true,
            ShowDeleteButton = true,
            ShowViewModeSelector = true,
            ViewMode = XContentViewMode.Grid
        };
    }

    /// <summary>
    /// Creates a toolbar context that only exposes a view-mode selector.
    /// </summary>
    /// <returns>The toolbar context.</returns>
    public static XToolbarContext CreateViewMode()
    {
        return new XToolbarContext
        {
            ShowViewModeSelector = true,
            ViewMode = XContentViewMode.Grid
        };
    }
    #endregion

    #region ### Private Methods ###
    private void NotifyToolbarVisibilityChanged()
    {
        this.OnPropertyChanged(nameof(HasCommandButtons));
        this.OnPropertyChanged(nameof(HasToolbarItems));
        this.OnPropertyChanged(nameof(ShowCommandSeparator));
    }
    #endregion
}
#endregion
