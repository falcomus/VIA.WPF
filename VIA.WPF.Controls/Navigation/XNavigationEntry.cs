// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationEntry.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationEntry ###
/// <summary>
/// Represents a selectable navigation entry that can be bound to navigation controls.
/// </summary>
public sealed class XNavigationEntry : XNotifyPropertyChangedObject
{
    #region ### Fields ###
    private string title = string.Empty;
    private string description = string.Empty;
    private object? value;
    private object? icon;
    private bool isEnabled = true;
    private bool isVisible = true;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationEntry"/> class.
    /// </summary>
    public XNavigationEntry()
    {
        this.Children = new ObservableCollection<XNavigationEntry>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationEntry"/> class.
    /// </summary>
    /// <param name="title">The displayed title.</param>
    /// <param name="value">The value represented by this entry.</param>
    public XNavigationEntry(string title, object? value)
        : this()
    {
        this.Title = title;
        this.Value = value;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the displayed title.
    /// </summary>
    public string Title
    {
        get => this.title;
        set => this.SetProperty(ref this.title, value);
    }

    /// <summary>
    /// Gets or sets the displayed description.
    /// </summary>
    public string Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value);
    }

    /// <summary>
    /// Gets or sets the value represented by this entry.
    /// </summary>
    public object? Value
    {
        get => this.value;
        set => this.SetProperty(ref this.value, value);
    }

    /// <summary>
    /// Gets or sets the optional icon.
    /// </summary>
    public object? Icon
    {
        get => this.icon;
        set => this.SetProperty(ref this.icon, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the entry is enabled.
    /// </summary>
    public bool IsEnabled
    {
        get => this.isEnabled;
        set => this.SetProperty(ref this.isEnabled, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the entry is visible.
    /// </summary>
    public bool IsVisible
    {
        get => this.isVisible;
        set => this.SetProperty(ref this.isVisible, value);
    }

    /// <summary>
    /// Gets the child entries.
    /// </summary>
    public ObservableCollection<XNavigationEntry> Children { get; }
    #endregion
}
#endregion
