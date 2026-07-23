// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridFilterItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VIA.WPF.Controls;

#region ### Class XDataGridFilterItem ###
/// <summary>
/// Represents a selectable value within a column filter popup.
/// </summary>
public sealed class XDataGridFilterItem : INotifyPropertyChanged
{
    #region ### Private Fields ###
    private bool _isSelected = true;
    private object? _value;
    private string _displayValue = string.Empty;
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the raw value of the filter item.
    /// </summary>
    public object? Value
    {
        get => this._value;
        set
        {
            if (Equals(this._value, value))
            {
                return;
            }

            this._value = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the display value of the filter item.
    /// </summary>
    public string DisplayValue
    {
        get => this._displayValue;
        set
        {
            if (this._displayValue == value)
            {
                return;
            }

            this._displayValue = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the filter item is selected.
    /// </summary>
    public bool IsSelected
    {
        get => this._isSelected;
        set
        {
            if (this._isSelected == value)
            {
                return;
            }

            this._isSelected = value;
            this.OnPropertyChanged();
        }
    }
    #endregion

    #region ### Private Methods ###
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
#endregion