// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNotifyPropertyChangedObject.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNotifyPropertyChangedObject ###
/// <summary>
/// Provides a small reusable base implementation for objects that notify property changes.
/// </summary>
public abstract class XNotifyPropertyChangedObject : INotifyPropertyChanged
{
    #region ### Events ###
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Raises <see cref="PropertyChanged"/> for the specified property name.
    /// </summary>
    /// <param name="propertyName">The changed property name.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName is not null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Updates a backing field and raises <see cref="PropertyChanged"/> when the value changed.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="field">The backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">The changed property name.</param>
    /// <returns><see langword="true"/> when the value changed; otherwise <see langword="false"/>.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
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
