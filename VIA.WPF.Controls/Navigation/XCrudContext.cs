// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCrudContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XCrudContext ###
/// <summary>
/// Provides a reusable state container for CRUD-capable views.
/// </summary>
public class XCrudContext : XNotifyPropertyChangedObject
{
    #region ### Fields ###
    private object? selectedItem;
    private object? editor;
    private string title = string.Empty;
    private XCrudMode mode = XCrudMode.None;
    private bool isOpen;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XCrudContext"/> class.
    /// </summary>
    public XCrudContext()
    {
        this.CloseCommand = new RelayCommand(this.Close);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the item currently selected by the view.
    /// </summary>
    public object? SelectedItem
    {
        get => this.selectedItem;
        set
        {
            if (this.SetProperty(ref this.selectedItem, value))
            {
                this.OnPropertyChanged(nameof(this.HasSelectedItem));
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether an item is currently selected.
    /// </summary>
    public bool HasSelectedItem => this.SelectedItem is not null;

    /// <summary>
    /// Gets or sets the view model displayed inside the detail area.
    /// </summary>
    public object? Editor
    {
        get => this.editor;
        set => this.SetProperty(ref this.editor, value);
    }

    /// <summary>
    /// Gets or sets the title displayed by the detail area.
    /// </summary>
    public string Title
    {
        get => this.title;
        set => this.SetProperty(ref this.title, value);
    }

    /// <summary>
    /// Gets or sets the current CRUD operation mode.
    /// </summary>
    public XCrudMode Mode
    {
        get => this.mode;
        set
        {
            if (this.SetProperty(ref this.mode, value))
            {
                this.OnPropertyChanged(nameof(this.IsViewMode));
                this.OnPropertyChanged(nameof(this.IsEditMode));
                this.OnPropertyChanged(nameof(this.IsCreateMode));
                this.OnPropertyChanged(nameof(this.IsReadOnly));
                this.OnPropertyChanged(nameof(this.IsEditable));
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the detail area is currently open.
    /// </summary>
    public bool IsOpen
    {
        get => this.isOpen;
        set => this.SetProperty(ref this.isOpen, value);
    }

    /// <summary>
    /// Gets a value indicating whether the current mode is <see cref="XCrudMode.View"/>.
    /// </summary>
    public bool IsViewMode => this.Mode == XCrudMode.View;

    /// <summary>
    /// Gets a value indicating whether the current mode is <see cref="XCrudMode.Edit"/>.
    /// </summary>
    public bool IsEditMode => this.Mode == XCrudMode.Edit;

    /// <summary>
    /// Gets a value indicating whether the current mode is <see cref="XCrudMode.Create"/>.
    /// </summary>
    public bool IsCreateMode => this.Mode == XCrudMode.Create;

    /// <summary>
    /// Gets a value indicating whether the current detail area should be read-only.
    /// </summary>
    public bool IsReadOnly => this.Mode == XCrudMode.View;

    /// <summary>
    /// Gets a value indicating whether the current detail area should be editable.
    /// </summary>
    public bool IsEditable => this.Mode is XCrudMode.Edit or XCrudMode.Create;

    /// <summary>
    /// Gets the command that closes the current detail area.
    /// </summary>
    public ICommand CloseCommand { get; }

    /// <summary>
    /// Gets the command that cancels the current detail operation.
    /// </summary>
    public ICommand CancelCommand => this.CloseCommand;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Opens the detail area with the specified mode, editor and title.
    /// </summary>
    /// <param name="mode">The CRUD operation mode.</param>
    /// <param name="editor">The detail editor view model.</param>
    /// <param name="title">The detail title.</param>
    public void Open(XCrudMode mode, object? editor, string? title)
    {
        this.Mode = mode;
        this.Editor = editor;
        this.Title = title ?? string.Empty;
        this.IsOpen = true;
    }

    /// <summary>
    /// Closes the detail area and clears the active detail editor.
    /// </summary>
    public void Close()
    {
        this.IsOpen = false;
        this.Editor = null;
        this.Title = string.Empty;
        this.Mode = XCrudMode.None;
    }

    /// <summary>
    /// Clears the current selection.
    /// </summary>
    public void ClearSelection()
    {
        this.SelectedItem = null;
    }
    #endregion
}
#endregion
