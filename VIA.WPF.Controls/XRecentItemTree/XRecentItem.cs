// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRecentItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VIA.WPF.Controls;

#region ### Class XRecentItem ###
/// <summary>
/// Represents a default recent item model for <see cref="XRecentItemTree"/>.
/// </summary>
public class XRecentItem : INotifyPropertyChanged
{
    #region ### Private Fields ###
    /// <summary>
    /// The optional stable item identifier.
    /// </summary>
    private object? id;

    /// <summary>
    /// The display text.
    /// </summary>
    private string? text;

    /// <summary>
    /// The optional detail description.
    /// </summary>
    private string? description;

    /// <summary>
    /// The optional icon content.
    /// </summary>
    private object? icon;

    /// <summary>
    /// The optional tooltip content.
    /// </summary>
    private object? toolTip;

    /// <summary>
    /// The optional payload item.
    /// </summary>
    private object? data;

    /// <summary>
    /// Indicates whether the recent item is pinned.
    /// </summary>
    private bool isPinned;
    #endregion

    #region ### Events ###
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the optional stable item identifier.
    /// </summary>
    public object? Id
    {
        get => this.id;
        set => this.SetProperty(ref this.id, value);
    }

    /// <summary>
    /// Gets or sets the display text.
    /// </summary>
    public string? Text
    {
        get => this.text;
        set => this.SetProperty(ref this.text, value);
    }

    /// <summary>
    /// Gets or sets the optional detail description.
    /// </summary>
    public string? Description
    {
        get => this.description;
        set => this.SetProperty(ref this.description, value);
    }

    /// <summary>
    /// Gets or sets the optional icon content.
    /// </summary>
    public object? Icon
    {
        get => this.icon;
        set => this.SetProperty(ref this.icon, value);
    }

    /// <summary>
    /// Gets or sets the optional tooltip content.
    /// </summary>
    public object? ToolTip
    {
        get => this.toolTip;
        set => this.SetProperty(ref this.toolTip, value);
    }

    /// <summary>
    /// Gets or sets the optional payload item.
    /// </summary>
    public object? Data
    {
        get => this.data;
        set => this.SetProperty(ref this.data, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the recent item is pinned.
    /// </summary>
    public bool IsPinned
    {
        get => this.isPinned;
        set => this.SetProperty(ref this.isPinned, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(this.Text) ? base.ToString() ?? string.Empty : this.Text;
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Raises <see cref="PropertyChanged"/> for the specified property.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Sets a backing field and raises <see cref="PropertyChanged"/> if the value changed.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="field">The backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns><see langword="true"/> if the value changed; otherwise <see langword="false"/>.</returns>
    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
    #endregion
}
#endregion
